using EasyETL.Attributes;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Xml.Parsers
{

    [DisplayName("Delimited")]
    [EasyField("Delimiters", "The delimiters to separate columns.  Input multiple delimiters in separate lines.  If left empty, implies AutoDetect.","")]
    [EasyField("HasHeader", "The first row contains the column names.","True","True|False","True;False")]
    [EasyField("ColumnNames","Enter the columns names in separate lines","")]
    [EasyField("Comments","Lines starting with this prefix will be ignored for import","")]
    [EasyField("TableName","Name of the table","row")]
    public class DelimitedEasyParser : SingleLineEasyParser
    {
        public List<string> Delimiters = new List<string>();

        public DelimitedEasyParser() : base()
        {
            FirstRowHasFieldNames = true;
        }

        public DelimitedEasyParser(bool hasHeaderRow = true, params string[] delimiters)
        {
            FirstRowHasFieldNames = hasHeaderRow;
            Delimiters.AddRange(delimiters);
        }

        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            XmlNode xmlNode = null;
            if (Delimiters.Count == 0)
            {
                string lineContents = txtReader.ReadLine();
                DetectDelimiter(lineContents);

                txtFieldParser = new TextFieldParser(new StringReader(lineContents)) { TextFieldType = FieldType.Delimited, CommentTokens = this.CommentTokens };
                txtFieldParser.SetDelimiters(Delimiters.ToArray());
                if (FirstRowHasFieldNames)
                {
                    SetFieldNames(txtFieldParser.ReadFields());
                }
                else
                {
                    string[] fields = txtFieldParser.ReadFields();
                    NewRow(fields);
                    xmlNode = ConvertFieldsToXmlNode(xDoc, fields);
                }
                txtFieldParser = null;
                FirstRowHasFieldNames = false;
            }
            txtFieldParser = new TextFieldParser(txtReader) { TextFieldType = FieldType.Delimited, CommentTokens = this.CommentTokens };

            txtFieldParser.SetDelimiters(Delimiters.ToArray());
            return GetXmlDocument(xmlNode, xDoc);
        }

        public virtual void DetectDelimiter(string lineContents)
        {
            int delimiterIndex = 0;
            while ((lineContents.Length > delimiterIndex) && ("\'\"-ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz_".Contains(lineContents[delimiterIndex])))
            {
                delimiterIndex++;
            }
            if (lineContents.Length > delimiterIndex)
            {
                Delimiters.Add(lineContents[delimiterIndex].ToString());
            }
        }

        public override bool IsFieldSettingsComplete()
        {
            return true;
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string,string> resultDict =  base.GetSettingsAsDictionary();
            resultDict.Add("delimiters", String.Join(Environment.NewLine, Delimiters));
//            resultDict.Add("columnnames", FirstRowHasFieldNames ? "" : String.Join(Environment.NewLine, FieldNames));
            resultDict["parsertype"] = "Delimited";
            return resultDict;
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            //if (String.IsNullOrWhiteSpace(fieldValue)) return;
            switch (fieldName.ToLower())
            {
                case "delimiters":
                    if (fieldValue.Length == 1)
                    {
                        Delimiters.Add(fieldValue);
                    }
                    else
                    {
                        foreach (string delimiter in fieldValue.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            Delimiters.Add(delimiter);
                        }
                    }
                    break;
            }
        }
    }
}
