using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("HL7")]
    public class HL7EasyParser : DelimitedEasyParser
    {
        public HL7EasyParser() : base(false, "|")
        {
            FirstRowHasFieldNames = false;
            Delimiters.Add("|");
        }

        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            txtFieldParser = new TextFieldParser(txtReader) { TextFieldType = FieldType.Delimited };
            txtFieldParser.SetDelimiters(Delimiters.ToArray());
            return GetXmlDocument(null, xDoc);
        }

        public override void NewRow(string[] fieldValues)
        {
            RowNodeName = fieldValues[0];
            FieldPrefix = fieldValues[0] + "_";
        }
    }
}
