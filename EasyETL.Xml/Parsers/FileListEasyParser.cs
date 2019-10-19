using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("File List")]
    [EasyField("Directory", "Full path of the folder")]
    [EasyField("Filter", "Match Pattern to retrieve files", "*.*")]
    [EasyField("IncludeSubFolders", "True to include all files in subfolders, false to limit search to the root folder", "False", "", "True;False")]
    [EasyField("OutputFields", "Field Names to output.  Separate by semicolon.  Supported fields -- FolderName, FileName, FullPath, Size, LastAccessedTime, CreationTime, LastModifiedTime")]
    public class FileListEasyParser : DatasourceEasyParser
    {
        public string DirectoryName;
        public string Filter = "*.*";
        public bool IncludeSubFolders = false;
        public List<string> OutputFields = new List<string>();
        public FileListEasyParser()
        {
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            switch (fieldName.ToLower())
            {
                case "directory":
                    DirectoryName = fieldValue; break;
                case "filter":
                    Filter = fieldValue; break;
                case "includesubfolders":
                    IncludeSubFolders = Convert.ToBoolean(fieldValue); break;
                case "outputfields":
                    OutputFields.Clear();
                    if (!String.IsNullOrWhiteSpace(fieldValue))
                    {
                        foreach (string outputField in fieldValue.Split(';'))
                        {
                            //if ("FolderName;FileName;FullPath;Size;LastAccessedTime;CreationTime;LastModifiedTime".Split(';').Contains(outputField,StringComparer.CurrentCultureIgnoreCase)) 
                            OutputFields.Add(outputField);
                        }
                    }
                    break;
            }
        }

        public override bool IsFieldSettingsComplete()
        {
            return base.IsFieldSettingsComplete() && !String.IsNullOrWhiteSpace(DirectoryName) && !String.IsNullOrWhiteSpace(Filter) && (OutputFields.Count > 0);
        }

        public override bool CanFunction()
        {
            if (!IsFieldSettingsComplete()) return false;
            return Directory.Exists(DirectoryName);
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("directory", DirectoryName);
            resultDict.Add("filter", Filter);
            resultDict.Add("includesubfolders", IncludeSubFolders.ToString());
            resultDict.Add("outputfields", String.Join(";", OutputFields));
            return resultDict;
        }

        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            return LoadStr(txtReader.ReadToEnd(), xDoc);
        }

        public override XmlDocument Load(string filename, XmlDocument xDoc = null)
        {
            return LoadStr(filename, xDoc);
        }

        public override XmlDocument LoadStr(string strToLoad, XmlDocument xDoc = null)
        {
            if (!String.IsNullOrWhiteSpace(strToLoad)) Filter = strToLoad;

            #region setup the rootNode
            XmlNode rootNode;
            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }
            if (xDoc.DocumentElement == null)
            {
                rootNode = xDoc.CreateElement(RootNodeName);
                xDoc.AppendChild(rootNode);
            }

            rootNode = xDoc.DocumentElement;
            #endregion

            if (OutputFields.Count == 0) return xDoc;
            int lineNumber = 0;
            int rowCount = 0;
            SearchOption searchOption = SearchOption.TopDirectoryOnly;
            if (IncludeSubFolders) searchOption = SearchOption.AllDirectories;

            foreach (string file in Directory.EnumerateFiles(DirectoryName, Filter, searchOption))
            {
                FileInfo fileInfo = new FileInfo(file);
                Dictionary<string, string> extendedProperties = null;
                lineNumber++;
                UpdateProgress(lineNumber);
                XmlElement rowNode = xDoc.CreateElement(RowNodeName);
                foreach (string outputField in OutputFields)
                {
                    XmlElement colNode = xDoc.CreateElement(outputField);
                    switch (outputField.ToLower())
                    {
                        case "foldername":
                            colNode.InnerText = fileInfo.DirectoryName; break;
                        case "filename":
                            colNode.InnerText = fileInfo.Name; break;
                        case "fullpath":
                            colNode.InnerText = fileInfo.FullName; break;
                        case "size":
                            colNode.InnerText = fileInfo.Length.ToString(); break;
                        case "lastaccessedtime":
                            colNode.InnerText = File.GetLastAccessTime(file).ToString(); break;
                        case "creationtime":
                            colNode.InnerText = File.GetCreationTime(file).ToString(); break;
                        case "lastmodifiedtime":
                            colNode.InnerText = File.GetLastWriteTime(file).ToString(); break;
                        default:
                            if (extendedProperties == null)
                            {
                                extendedProperties = GetExtendedProperties(file);
                            }
                            if ((extendedProperties != null) && (extendedProperties.ContainsKey(outputField)))
                                colNode.InnerText = extendedProperties[outputField];
                            break;
                    }
                    rowNode.AppendChild(colNode);
                }

                AddRow(xDoc, rowNode);
                if ((rowNode != null) && (rowNode.HasChildNodes))
                {
                    rootNode.AppendChild(rowNode);
                    rowCount++;
                }
                if (rowCount >= MaxRecords) break;
            }

            UpdateProgress(lineNumber, true);

            return xDoc;
        }

        private Dictionary<string, string> GetExtendedProperties(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            var shell = new Shell32.Shell();
            var shellFolder = shell.NameSpace(directory);
            var fileName = Path.GetFileName(filePath);
            var folderitem = shellFolder.ParseName(fileName);
            var dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            var i = -1;
            while (++i < 320)
            {
                var header = shellFolder.GetDetailsOf(null, i);
                if (String.IsNullOrEmpty(header)) continue;
                var value = shellFolder.GetDetailsOf(folderitem, i);
                if (!dictionary.ContainsKey(header)) dictionary.Add(header, value);
                Console.WriteLine(header + ": " + value);
            }
            Marshal.ReleaseComObject(shell);
            Marshal.ReleaseComObject(shellFolder);
            return dictionary;
        }

    }
}
