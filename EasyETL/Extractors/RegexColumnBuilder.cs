using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EasyETL.DataSets
{
    /// <summary>
    ///     Class for creating regular expressions for use in TextFileDataSet
    /// </summary>
    public class RegexColumnBuilder
    {
        /// <summary>
        ///     Get the collection of created RegexColumns
        /// </summary>
        public List<RegexColumn> Columns = new List<RegexColumn>();

        /// <summary>
        ///     A sepator string that will be appended after each
        ///     column expression (including any trailing regular expression)
        /// </summary>
        public string Separator = string.Empty;

        /// <summary>
        ///     Indicates wether the regular expression will start
        ///     searching at each beginning of a line/string
        ///     default is set to true
        /// </summary>
        public bool StartAtBeginOfString = true;

        /// <summary>
        ///     Indicates wether the regular expression will end
        ///     searching at each end of a line/string
        ///     default is set to true
        /// </summary>
        public bool EndAtEndOfString = true;

        public RegexColumnBuilder(string separator, params string[] columnNames)
        {
            Separator = separator;
            for (int i = 0; i < columnNames.Length; i++)
            {
                if (i < columnNames.Length - 1) {
                    AddColumn(columnNames[i]);
                }
                else
                {
                    if (columnNames[i].Contains(":"))
                    {
                        AddColumn(columnNames[i].Split(':')[0], ".*" );
                    }
                    else
                    {
                        AddColumn(columnNames[i], ".*");
                    }
                }
            }
        }

        public string RegexFormattedOutput(char separator)
        {
            return Regex.Escape(separator.ToString());
        }


        public void AddColumn(string columnName)
        {
            if ((!String.IsNullOrWhiteSpace(Separator)) && (!columnName.Contains(":")))
            {
                AddColumn(columnName, Separator[0]);
            }
            else
            {
                if (columnName.Contains(":"))
                {
                    AddColumn(columnName.Split(':')[0], Int16.Parse(columnName.Split(':')[1]));
                }
            }
        }

        /// <summary>
        ///     Adds a column to the collection of columns from which
        ///     the regular expression will be created
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="separator">column separator</param>
        public void AddColumn(string columnName, char separator, RegexColumnType columnType = RegexColumnType.STRING)
        {
            if ((columnName.StartsWith("\"")) && (columnName.EndsWith("\"")))
            {
                AddColumn(columnName, "[^\"]*", columnType);
            }
            else
            {
                if (columnName.Contains(":"))
                {
                    string columnNameOnly = columnName.Substring(0, columnName.IndexOf(':'));
                    int columnLength = Int16.Parse(columnName.Split(':')[1]);
                    AddColumn(columnNameOnly, columnLength, columnType);
                }
                else
                {
                    AddColumn(columnName, "[^" + RegexFormattedOutput(separator) + "\n]*", columnType);
                }
            }
            Separator = separator.ToString();
        }

        /// <summary>
        ///     Adds a column to the collection of columns from which
        ///     the regular expression will be created
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="length">amount of characters this column has</param>
        /// <param name="columnType">RegexColumnType of this column</param>
        public void AddColumn(string columnName, int length, RegexColumnType columnType = RegexColumnType.STRING)
        {
            AddColumn(columnName, ".{" + length + "}", columnType);
        }

        /// <summary>
        ///     Adds a column to the collection of columns from which
        ///     the regular expression will be created
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="columnRegEx">regular expression for capturing the value of this column</param>
        /// <param name="columnType">RegexColumnType of this column</param>
        public void AddColumn(string columnName, string columnRegEx, RegexColumnType columnType = RegexColumnType.STRING)
        {
            string precedingRegEx = columnName.StartsWith("\"") ? "\"" : String.Empty;
            string trailingRegEx = columnName.EndsWith("\"") ? "\"" :String.Empty;
            columnName = columnName.Trim('\"');
            AddColumn(columnName, columnRegEx, trailingRegEx,precedingRegEx, columnType);
        }

        /// <summary>
        ///     Adds a column to the collection of columns from which
        ///     the regular expression will be created
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="columnRegEx">regular expression for capturing the value of this column</param>
        /// <param name="trailingRegEx">regular expression for any data not te be captured for this column</param>
        public void AddColumn(string columnName, string columnRegEx, string trailingRegEx, RegexColumnType columnType = RegexColumnType.STRING)
        {
            Columns.Add(new RegexColumn(columnName, columnRegEx, trailingRegEx, String.Empty, columnType));
        }

        /// <summary>
        ///     Adds a column to the collection of columns from which
        ///     the regular expression will be created
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="columnRegEx">regular expression for capturing the value of this column</param>
        /// <param name="trailingRegEx">Trailing regular expression for any data not to be captured for this column</param>
        /// <param name="precedingRegEx">Preceding regular expression for any data not to be captured for this column</param>
        /// <param name="columnType">RegexColumnType of this column</param>
        public void AddColumn(string columnName, string columnRegEx, string trailingRegEx, string precedingRegEx,
                              RegexColumnType columnType = RegexColumnType.STRING)
        {
            Columns.Add(new RegexColumn(columnName, columnRegEx, trailingRegEx, precedingRegEx,columnType));
        }

        /// <summary>
        ///     creates a regular expression string based on the added columns and
        ///     optional separator
        /// </summary>
        /// <returns>regular expression for use in TextFileDataSet</returns>
        public string CreateRegularExpressionString()
        {
            var result = new StringBuilder();
            if (StartAtBeginOfString) result.Append("^");
            for (var i = 0; i < Columns.Count; i++)
            {
                // let us check for separator and if present, make this field optional....
                if ((!String.IsNullOrEmpty(Separator)) && (i > 0))
                {
                    result.Append("(?(" + RegexFormattedOutput(Separator[0]) + ")(" + RegexFormattedOutput(Separator[0]) + "(");
                }
                result.Append(Columns[i].PrecedingRegEx);
                if (String.IsNullOrWhiteSpace(Columns[i].ColumnName))
                {
                    result.Append("(?:");
                }
                else
                {
                    result.Append("(?<");
                    result.Append(Columns[i].ColumnName);
                    result.Append(">");
                }
                result.Append(Columns[i].RegEx);
                result.Append(")");
                result.Append(Columns[i].TrailingRegEx);
                if ((!String.IsNullOrEmpty(Separator) && (i > 0)))
                {
                    result.Append(")|()))");
                }
            }
            if (EndAtEndOfString) result.Append("$");
            return result.ToString();
        }

        /// <summary>
        ///     creates a regular expression based on the added columns and
        ///     optional separator
        /// </summary>
        /// <returns></returns>
        public Regex CreateRegularExpression()
        {
            return new Regex(CreateRegularExpressionString());
        }

        /// <summary>
        ///     creates a regular expression based on the added columns and
        ///     optional separator
        /// </summary>
        /// <param name="aOptions"></param>
        /// <returns></returns>
        public Regex CreateRegularExpression(RegexOptions aOptions)
        {
            return new Regex(CreateRegularExpressionString(), aOptions);
        }
    }

    /// <summary>
    ///     Enumeration for certain types used in TextFileDataSet
    /// </summary>
    public enum RegexColumnType
    {
        /// <summary>
        ///     Int32
        /// </summary>
        INTEGER,

        /// <summary>
        ///     Double
        /// </summary>
        DOUBLE,

        /// <summary>
        ///     String
        /// </summary>
        STRING,

        /// <summary>
        ///     DateTime
        /// </summary>
        DATE
    }

    /// <summary>
    ///     Class for defining a regular expression column
    /// </summary>
    public class RegexColumn
    {


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="columnName">name of the column</param>
        /// <param name="regEx">regular expression for capturing the value of this column</param>
        /// <param name="trailingRegex">Trailing regular expression for any data not to be captured for this column</param>
        /// <param name="precedingRegex">Preceding regular expression for any data not to be captured for this column</param>
        /// <param name="columnType">Type of this column</param>
        public RegexColumn(string columnName, string regEx, string trailingRegex, string precedingRegex = "", RegexColumnType columnType = RegexColumnType.STRING)
        {
            Init(columnName, regEx, trailingRegex, precedingRegex, columnType);
        }

        /// <summary>
        ///     Get or set the name of the column
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     Get or set the regular expression for capturing the value of this column
        /// </summary>
        public string RegEx { get; set; }

        /// <summary>
        /// Get or set the preceding regular expression for any data not to be captured for this column
        /// </summary>

        public string PrecedingRegEx { get; set; }

        /// <summary>
        ///     Get or set the trailing regular expression for any data not to be captured for this column
        /// </summary>
        public string TrailingRegEx { get; set; }

        /// <summary>
        ///     Get or set the Type of this column
        /// </summary>
        public RegexColumnType ColumnType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ValueMatchingCondition { get; set; }

        /// <summary>
        ///     Get the System.Type of this RegexColumn
        /// </summary>
        public Type ColumnTypeAsType
        {
            get
            {
                switch (ColumnType)
                {
                    case RegexColumnType.INTEGER:
                        return typeof(int);
                    case RegexColumnType.DOUBLE:
                        return typeof(double);
                    case RegexColumnType.DATE:
                        return typeof(DateTime);
                }
                return typeof(string);
            }
        }

        private void Init(string columnName, string regEx, string trailingRegex, string precedingRegex, RegexColumnType columnType)
        {
            ColumnName = columnName;
            RegEx = regEx;
            TrailingRegEx = trailingRegex;
            PrecedingRegEx = precedingRegex;
            ColumnType = columnType;
        }
    }
}