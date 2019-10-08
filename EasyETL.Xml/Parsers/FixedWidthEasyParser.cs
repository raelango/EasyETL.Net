using EasyETL.Attributes;
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
    [DisplayName("Fixed Width")]
    [EasyField("widths", "The widths of each column.  Use space to separate multiple widths.  If left empty, implies AutoDetect.")]
    [EasyField("HasHeader", "The first row contains the column names.", "True", "True|False", "True;False")]
    [EasyField("Comments", "Lines starting with this prefix will be ignored for import")]
    public class FixedWidthEasyParser : SingleLineEasyParser
    {
        public List<int> ColumnWidths = new List<int>();

        public FixedWidthEasyParser() : base()
        {
            FirstRowHasFieldNames = true;
            ColumnWidths.Add(-1);
        }

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

        public override bool IsFieldSettingsComplete()
        {
            return (ColumnWidths.Count > 0);
        }

        public override Dictionary<string, string> GetSettingsAsDictionary()
        {
            Dictionary<string, string> resultDict = base.GetSettingsAsDictionary();
            resultDict.Add("widths", String.Join(" ", ColumnWidths.ToArray()));
            resultDict["parsertype"] = "Fixed Width";
            return resultDict;
        }

        public override void LoadSetting(string fieldName, string fieldValue)
        {
            base.LoadSetting(fieldName, fieldValue);
            if (String.IsNullOrWhiteSpace(fieldValue)) return;
            switch (fieldName.ToLower())
            {
                case "widths":
                    ColumnWidths.Clear();
                    foreach (string columnWidth in fieldValue.Split(' '))
                    {
                        ColumnWidths.Add(Int16.Parse(columnWidth));
                    }
                    break;
            }
        }
    }
}
