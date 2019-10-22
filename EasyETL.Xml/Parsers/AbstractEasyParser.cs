using EasyETL.Attributes;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Xml.Parsers
{

    public class EasyParserExceptionEventArgs : EventArgs
    {
        public EasyParserExceptionEventArgs(MalformedLineException exception)
        {
            Exception = exception;
        }

        public MalformedLineException Exception;
    }

    public class EasyParserProgressEventArgs : EventArgs
    {
        public EasyParserProgressEventArgs(long lNumber)
        {
            LineNumber = lNumber;
        }
        public long LineNumber;
    }

    public abstract class AbstractEasyParser : IEasyParser, IEasyFieldInterface
    {
        public string[] FieldNames = null;
        public string RootNodeName = "data";
        public string RowNodeName = "row";
        public string FieldPrefix = "Field_";
        public long MaxRecords = long.MaxValue;
        public string[] OnLoadSettings = null;
        public XslCompiledTransform OnLoadXsl = null;
        public event EventHandler<XmlNodeChangedEventArgs> OnRowAdd;
        public event EventHandler<EasyParserExceptionEventArgs> OnError;
        public event EventHandler<EasyParserProgressEventArgs> OnProgress;

        public List<MalformedLineException> Exceptions = new List<MalformedLineException>();
        public int MaximumErrorsToAbort = 20;
        public int ProgressInterval = 1;
        public int LastProgressUpdate = 0;

        public virtual XmlNode ConvertFieldsToXmlNode(XmlDocument xDoc, string[] fieldValues)
        {
            XmlElement rowNode = xDoc.CreateElement(RowNodeName);
            int colIndex = 0;
            foreach (string field in fieldValues)
            {
                string fieldName = (FieldNames != null && FieldNames.Length >= colIndex) ? FieldNames[colIndex] : FieldPrefix + colIndex.ToString();
                XmlElement colNode = xDoc.CreateElement(fieldName);
                colNode.InnerText = field;
                rowNode.AppendChild(colNode);
                colIndex++;
            }

            return AddRow(xDoc, rowNode);
        }

        public virtual XmlNode AddRow(XmlDocument xDoc, XmlNode childNode)
        {
           
            XmlNode returnNode = childNode;
            if (OnLoadSettings != null)
            {
                if (OnLoadXsl == null)
                {
                    returnNode = childNode.Clone();
                    XmlDocument rDoc = new XmlDocument();
                    returnNode = rDoc.ImportNode(childNode, true);
                    OnLoadXsl = returnNode.GetCompiledTransform(OnLoadSettings);
                }
                returnNode = childNode.TransformXml(OnLoadXsl);
            }
            if ((returnNode != null) && (OnRowAdd != null))
            {
                OnRowAdd.Invoke(this, new XmlNodeChangedEventArgs(returnNode, xDoc.DocumentElement, xDoc.DocumentElement, "", "", XmlNodeChangedAction.Insert));
            }
            if (returnNode == null) 
                childNode.RemoveAll();
            else 
                childNode.InnerXml = returnNode.InnerXml;
            return returnNode;
        }

        public virtual void RaiseException(MalformedLineException exception)
        {
            if (OnError !=null) {
                OnError.Invoke(this, new EasyParserExceptionEventArgs(exception));
            }
        }

        public void UpdateProgress(long lineNumber, bool forceInvoke = false)
        {
            if (OnProgress == null) return;
            if ((forceInvoke) || ((lineNumber % ProgressInterval) == 0))
            {
                OnProgress.Invoke(this, new EasyParserProgressEventArgs(lineNumber));
            }
        }

        public virtual void SetFieldNames(params string[] fieldNames)
        {
            FieldNames = fieldNames;
        }

        public virtual XmlDocument LoadStr(string strToLoad, XmlDocument xDoc = null)
        {
            return Load(new StringReader(strToLoad), xDoc);
        }

        public virtual XmlDocument Load(string filename, XmlDocument xDoc = null)
        {

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return Load(fs, xDoc);
            }
        }

        public virtual XmlDocument Load(Stream inStream, XmlDocument xDoc = null)
        {
            using (StreamReader sr = new StreamReader(inStream))
            {
                return Load(sr, xDoc);
            }
        }

        public virtual XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            throw new NotImplementedException();
        }


        public virtual bool IsFieldSettingsComplete()
        {
            return true;
        }

        public void LoadFieldSettings(Dictionary<string, string> settingsDictionary)
        {
            foreach (KeyValuePair<string, string> kvPair in settingsDictionary.DecryptPasswords(this.GetEasyFields()))
            {
                LoadSetting(kvPair.Key, kvPair.Value);
            }
        }

        public void SaveFieldSettingsToXmlNode(XmlNode parentNode)
        {
            parentNode.SaveFieldSettingsToXmlNode(GetSettingsAsDictionary());
        }


        public virtual void LoadSetting(string fieldName, string fieldValue)
        {

        }

        public virtual bool CanFunction()
        {
            return IsFieldSettingsComplete();
        }

        public virtual Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string,string> resultDict = new Dictionary<string, string>();
            resultDict.Add("parsertype","abstract");
            return resultDict;
        }
    
    
    }
}
