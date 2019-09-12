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
    public class FixedWidthEasyParser : SingleLineEasyParser
    {
        public List<int> ColumnWidths = new List<int>();

        public FixedWidthEasyParser(bool hasHeaderRow = true, params int[] columnWidths)
        {
            FirstRowHasFieldNames = hasHeaderRow;
            if (columnWidths.Length == 0)
                ColumnWidths.Add(-1);
            else 
                ColumnWidths.AddRange(columnWidths);
        }

        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            txtFieldParser = new TextFieldParser(txtReader) { TextFieldType = FieldType.FixedWidth };
            txtFieldParser.SetFieldWidths(ColumnWidths.ToArray());
            return GetXmlDocument();
        }

    }
}
