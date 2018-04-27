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


        public void AddColumn(string columnName, bool bAutoIncrement, Int32 startValue, Int32 increment) {
            if (bAutoIncrement) {
                Columns.Add(new RegexColumn(columnName,startValue,increment));
            }
            else {
                AddColumn(columnName);
            } 
        }

        public void AddColumn(string columnName, bool bIsForeignKey)
        {
            if (bIsForeignKey)
            {
                Columns.Add(new RegexColumn(columnName, bIsForeignKey));
            }
            else
            {
                AddColumn(columnName);
            }
        }

        public void AddColumn(string columnName, RegexColumnType columnType, string expression)
        {
            Columns.Add(new RegexColumn(columnName, expression, columnType));
        }


        public void AddColumn(string columnName)
        {
            if ((!String.IsNullOrEmpty(Separator)) && (!columnName.Contains(":")))
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
            bool bFirstParsingColumn = true;
            for (var i = 0; i < Columns.Count; i++)
            {
                bool doImport = true;
                if ((Columns[i].AutoIncrement) || (!String.IsNullOrEmpty(Columns[i].Expression)) || (Columns[i].IsForeignKey))
                {
                    //This column is either an autoincrement column or a column with formula.. do not import;
                    doImport = false;
                }
                if (doImport)
                {
                    // let us check for separator and if present, make this field optional....
                    if ((!String.IsNullOrEmpty(Separator)) && (!bFirstParsingColumn))
                    {
                        result.Append("(?(" + RegexFormattedOutput(Separator[0]) + ")(" + RegexFormattedOutput(Separator[0]) + "(");
                    }
                    result.Append(Columns[i].PrecedingRegEx);

                    if (String.IsNullOrWhiteSpace(Columns[i].ColumnName)) //The column name is empty.. we need to skip this field....
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
                    if ((!String.IsNullOrEmpty(Separator) && (!bFirstParsingColumn)))
                    {
                        result.Append(")|()))");
                    }
                    bFirstParsingColumn = false;
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

        public string GetPropertiesAsXml()
        {
            StringBuilder sbResult = new StringBuilder();
            foreach (RegexColumn regexColumn in Columns)
            {
                sbResult.Append(regexColumn.GetPropertiesAsXml());
            }
            return sbResult.ToString();
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



        #region constructors
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

        public RegexColumn(string columnName, Int32 startValue = 1, Int32 increment = 1)
        {
            ColumnName = columnName;
            Expression = String.Empty;
            AutoIncrement = true;
            ColumnType = RegexColumnType.INTEGER;
            StartValue = startValue;
            Increment = increment;
            IsForeignKey = false;
            IsUnique = true;
        }

        public RegexColumn(string columnName, string expression, RegexColumnType columnType = RegexColumnType.STRING)
        {
            ColumnName = columnName;
            Expression = expression;
            ColumnType = columnType;
            AutoIncrement = false;
            IsForeignKey = false;
            IsUnique = false;
        }

        public RegexColumn(string columnName, bool isForeignKey)
        {
            ColumnName = columnName;
            IsForeignKey = isForeignKey;
            Expression = String.Empty;
            ColumnType = RegexColumnType.STRING;
            AutoIncrement = false;
            StartValue = 1;
            Increment = 1;
            IsUnique = false;
        }
        #endregion

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
        /// 
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 StartValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Increment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

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
            AutoIncrement = false;
            Expression = String.Empty;
            RegEx = regEx;
            TrailingRegEx = trailingRegex;
            PrecedingRegEx = precedingRegex;
            ColumnType = columnType;
            IsForeignKey = false;
            StartValue = 1;
            Increment = 1;
        }

        public string GetPropertiesAsXml()
        {
            StringBuilder sbResult = new StringBuilder();
            sbResult.Append("<" + (String.IsNullOrEmpty(ColumnName) ? "_" : ColumnName) + GetRegexProperties() );
            if (!String.IsNullOrEmpty(PrecedingRegEx.TrimEnd('"')))  sbResult.Append(" Prefix = '" + PrecedingRegEx + "'");
            if (!String.IsNullOrEmpty(TrailingRegEx.TrimEnd('"'))) sbResult.Append(" Suffix = '" + TrailingRegEx + "'");
            if (ColumnType != RegexColumnType.STRING) sbResult.Append(" Type = '" + ColumnType.ToString() + "'");
            if (!String.IsNullOrEmpty(ValueMatchingCondition)) sbResult.Append(" Condition = '" + ValueMatchingCondition + "'");
            if (AutoIncrement) sbResult.Append(" AutoIncrement = '" + AutoIncrement.ToString() + "'");
            if (AutoIncrement) sbResult.Append(" StartValue = '" + StartValue.ToString() + "'");
            if (AutoIncrement) sbResult.Append(" Increment = '" + Increment.ToString() + "'");
            if (!String.IsNullOrEmpty(Expression)) sbResult.Append(" Expression = '" + Expression + "'");
            if (IsForeignKey) sbResult.Append(" ForeignKey = '" + IsForeignKey.ToString() + "'");
            if (IsUnique) sbResult.Append(" PrimaryKey = '" + IsUnique.ToString() + "'");

            if (!String.IsNullOrEmpty(DisplayName)) sbResult.Append(" DisplayName = '" + DisplayName + "'");
            if (!String.IsNullOrEmpty(Description)) sbResult.Append(" Description = '" + Description.ToString() + "'");
            sbResult.Append(" />");
            return sbResult.ToString();
        }

        private string GetRegexProperties()
        {
            string strResult = " ";
            string strRegex = this.RegEx;
            if (!String.IsNullOrEmpty(strRegex))
            {
                strRegex = strRegex.TrimEnd('*');
                strRegex = strRegex.TrimEnd(']');
                if (strRegex.EndsWith("\\n")) strRegex = strRegex.Substring(0,strRegex.Length -2);
                if (strRegex.StartsWith("[^"))
                {
                    strRegex = strRegex.Substring(2);
                    if (strRegex.StartsWith("\""))
                    {
                        strResult += " QUOTES='true'";
                        strRegex = strRegex.TrimStart('\"');
                    }
                    if (strRegex.Length > 0)
                    {
                        strResult += " Separator='" + Regex.Unescape(strRegex) + "'";
                    }
                }
                else if (strRegex.StartsWith(".{"))
                {
                    //This is fixed length
                    strRegex = strRegex.Substring(2);
                    strRegex = strRegex.TrimEnd('}');
                    int intRegex;
                    if ((!String.IsNullOrEmpty(strRegex)) && (Int32.TryParse(strRegex,out intRegex))) {
                        strResult += " length='" + strRegex + "'";
                    }
                }

            }
            return strResult;
        }
    }
}