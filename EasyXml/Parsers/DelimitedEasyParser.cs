using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyXml.Parsers
{
    public class DelimitedEasyParser : SingleLineEasyParser
    {
        public List<string> Delimiters = new List<string>();

        public DelimitedEasyParser(bool hasHeaderRow = true, params string[] delimiters)
        {
            FirstRowHasFieldNames = hasHeaderRow;
            Delimiters.AddRange(delimiters);
        }

        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null, XslCompiledTransform xslt = null)
        {
            XmlNode xmlNode = null;
            if (Delimiters.Count == 0)
            {
                string lineContents = txtReader.ReadLine();
                DetectDelimiter(lineContents);

                txtFieldParser = new TextFieldParser(new StringReader(lineContents)) { TextFieldType = FieldType.Delimited };
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
            txtFieldParser = new TextFieldParser(txtReader) { TextFieldType = FieldType.Delimited };
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

    }
}
