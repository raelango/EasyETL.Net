using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EasyETL.DataSets
{
    public class DelimitedDataSet : RegexDataSet
    {
        protected string _delimiter = "";
        protected bool _hasHeaderRow = true;

        

        public DelimitedDataSet(string delimiter = "", bool hasHeaderRow = true)
        {
            _delimiter = delimiter;
            _hasHeaderRow = hasHeaderRow;
        }
        public override void Fill(Stream textFile)
        {
            UseFirstRowNamesAsColumnNames = _hasHeaderRow;
            if (_hasHeaderRow)
            {
                StreamReader sr = new StreamReader(textFile,Encoding.UTF8,true, 1024, true);
                string firstRow = sr.ReadLine();
                sr.Close();
                if (String.IsNullOrWhiteSpace(_delimiter))
                {
                    Regex regexSeparator = new Regex("^([a-zA-Z0-9_\"]*)(?<Separator>.)");
                    Match match = regexSeparator.Match(firstRow);
                    if (match.Success)
                    {
                        _delimiter = match.Groups["Separator"].ToString();
                    }
                }

                string[] columnNames = firstRow.Split(_delimiter[0]);
                ColumnBuilder = new RegexColumnBuilder(_delimiter, columnNames);
                SkipFirstRow = true;
            }
            base.Fill(textFile);
        }
    }
}
