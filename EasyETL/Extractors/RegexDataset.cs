using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace EasyETL.DataSets
{
    public class ConditionalRegexParser
    {
        public Regex ConditionRegex = null;
        public string TableName = "";
        public Regex parseRegex = null;
        public List<RegexColumn> RegexColumns = new List<RegexColumn>();
    }


    /// <summary>
    ///     Class for transforming a text-file into a dataset
    ///     based on one regular expression
    /// </summary>
    /// 
    public class RegexDataSet : EasyDataSet
    {
        private const string NewName = "NewName";
        private const string DefaultGroup = "0";

        private Regex _contentRegex;

        protected List<RegexColumn> _regexColumns;

        public List<ConditionalRegexParser> Parsers = new List<ConditionalRegexParser>();

        private bool _parsingInProgress = false;

        public Queue<string> QueueToParse = new Queue<string>();

        protected Dictionary<string, string> rowDict = new Dictionary<string, string>();

        /// <summary>
        ///     The text file to be read
        /// </summary>
        public Stream TextFile { get; set; }


        // ^(?<SNo>[^,]+),(?<Name>[^,]+),(?<DateOfBirth>[^,\n]+)(?<Separator>.)(?<Occupation>(?(Separator)(.*)|()))$

        /// <summary>
        ///     Set the RegexColumnBuilder
        ///     on setting this the columns and their RegexColumnTypes are set
        ///     as is the complete ContentExpression
        /// </summary>
        public RegexColumnBuilder ColumnBuilder
        {
            set
            {
                if (value == null)
                {
                    _regexColumns = null;
                    ContentExpression = null;
                }
                else
                {
                    _regexColumns = value.Columns;
                    ContentExpression = value.CreateRegularExpression();
                }
            }
        }


        /// <summary>
        ///     Regular Expression that is used for validating textlines and
        ///     defining the column names of the dataset
        /// </summary>
        public Regex ContentExpression
        {
            get { return _contentRegex; }
            set
            {
                if ((_contentRegex == null) || !_contentRegex.ToString().Equals(value.ToString()))
                {
                    _contentRegex = value;
                    ContentExpressionHasChanged = true;
                }
            }
        }

        private bool ContentExpressionHasChanged { get; set; }

        /// <summary>
        ///     The regular expression used for handling a first row that could
        ///     contain column headers. If you do not have a first row with headers
        ///     use UseFirstRowNamesAsColumnNames=false, or if you do have a row
        ///     with headers but don't want to use them use: SkipFirstRow=true
        /// </summary>
        public Regex FirstRowExpression { get; set; }

        /// <summary>
        ///     When set to true the values in the first match made are
        ///     used as column names instead of the ones supplied in
        ///     te regular expression
        /// </summary>
        public bool UseFirstRowNamesAsColumnNames = true;

        /// <summary>
        ///     When set to true the values in the first row are
        ///     discarded.
        /// </summary>
        public bool SkipFirstRow { get; set; }



        #region constructors
        public RegexDataSet(string fileName = "", string fieldSeparator = "", string tableName = "Table1", bool useFirstRowNamesAsColumns = true, bool skipFirstRow = false, params string[] columnNames)
        {

            TableName = tableName;
            UseFirstRowNamesAsColumnNames = useFirstRowNamesAsColumns;
            SkipFirstRow = skipFirstRow;
            if (useFirstRowNamesAsColumns)
            {
                if ((!String.IsNullOrWhiteSpace(fileName)) && (File.Exists(fileName)))
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        string firstRow = sr.ReadLine();
                        if (String.IsNullOrWhiteSpace(fieldSeparator))
                        {
                            Regex regexSeparator = new Regex("^([a-zA-Z0-9_\"]*)(?<Separator>.)");
                            Match match = regexSeparator.Match(firstRow);
                            if (match.Success)
                            {
                                fieldSeparator = match.Groups["Separator"].ToString();
                            }
                        }
                        columnNames = firstRow.Split(fieldSeparator[0]);
                    }
                    UseFirstRowNamesAsColumnNames = false;
                }
                else
                {
                    FirstRowExpression = new Regex(@"([^" + fieldSeparator + @"\n]*)[" + fieldSeparator + @"\w]");
                }
                SkipFirstRow = true;
            }

            if (columnNames.Length > 0)
            {
                ColumnBuilder = new RegexColumnBuilder(fieldSeparator, columnNames);
                Fill(fileName);
            }
            else
            {
                ColumnBuilder = null;
            }
        }


        public RegexDataSet(RegexColumnBuilder columnBuilder)
        {
            ColumnBuilder = columnBuilder;
        }

        public RegexDataSet(params ConditionalRegexParser[] parsers)
        {
            ColumnBuilder = null;
            SkipFirstRow = false;
            UseFirstRowNamesAsColumnNames = (parsers.Length == 0);
            foreach (ConditionalRegexParser crp in parsers)
            {
                Parsers.Add(crp);
            }
        }

        public RegexDataSet(XmlNode profileNode, string fileName, params ConditionalRegexParser[] parsers)
        {
            LoadProfileSettings(profileNode);
            Fill(fileName);
        }
        #endregion

        private void ParseColumnOrParser(RegexColumnBuilder columnBuilder, XmlNode childNode, string separator = "")
        {
            if (childNode.Name.ToUpper() == "IF")
            {
                string strCondition = String.Empty;
                string strTableName = TableName;
                //Conditional Table level attributes...
                foreach (XmlAttribute xAttr in childNode.Attributes)
                {
                    switch (xAttr.Name.ToUpper())
                    {
                        case "SEPARATOR":
                            separator = xAttr.Value;
                            break;
                        case "CONDITION":
                            strCondition = xAttr.Value;
                            break;
                        case "TABLENAME":
                            strTableName = xAttr.Value;
                            break;
                    }

                }
                RegexColumnBuilder conditionalRCB = new RegexColumnBuilder(separator);
                foreach (XmlNode subNode in childNode.ChildNodes)
                {
                    ParseColumnOrParser(conditionalRCB, subNode, separator);
                }
                ConditionalRegexParser crp = new ConditionalRegexParser() { ConditionRegex = new Regex(strCondition), TableName = strTableName, parseRegex = conditionalRCB.CreateRegularExpression(), RegexColumns = conditionalRCB.Columns };
                Parsers.Add(crp);
            }
            else
            {
                string prefix = "";
                string suffix = "";
                string strCondition = String.Empty;
                bool hasDoubleQuotes = false;
                bool bAutoIncrement = false;
                Int32 intStartValue = 1;
                Int32 intIncrement = 1;
                bool bForeignKey = false;
                bool bPrimaryKey = false;
                string strExpression = String.Empty;
                string strDisplayName = childNode.Name;
                string strDescription = String.Empty;
                int columnLength = 0;
                RegexColumnType rct = RegexColumnType.STRING;
                //Column level attributes...
                foreach (XmlAttribute xAttr in childNode.Attributes)
                {
                    switch (xAttr.Name.ToUpper())
                    {
                        case "SEPARATOR":
                            separator = xAttr.Value;
                            break;
                        case "PREFIX":
                            prefix = xAttr.Value;
                            break;
                        case "SUFFIX":
                            suffix = xAttr.Value;
                            break;
                        case "QUOTES":
                            hasDoubleQuotes = Boolean.Parse(xAttr.Value);
                            break;
                        case "LENGTH":
                            columnLength = Int16.Parse(xAttr.Value);
                            break;
                        case "TYPE":
                            rct = (RegexColumnType)Enum.Parse(typeof(RegexColumnType), xAttr.Value);
                            break;
                        case "CONDITION":
                            strCondition = xAttr.Value;
                            break;
                        case "AUTOINCREMENT":
                            bAutoIncrement = Boolean.Parse(xAttr.Value);
                            break;
                        case "STARTVALUE":
                        case "START":
                        case "SEED":
                            intStartValue = Int32.Parse(xAttr.Value);
                            break;
                        case "INCREMENT":
                            intIncrement = Int32.Parse(xAttr.Value);
                            break;
                        case "EXPRESSION":
                            strExpression = xAttr.Value;
                            break;
                        case "FOREIGNKEY":
                            bForeignKey = Boolean.Parse(xAttr.Value);
                            break;
                        case "UNIQUE":
                        case "PRIMARYKEY":
                        case "PRIMARY":
                            bPrimaryKey = Boolean.Parse(xAttr.Value);
                            break;
                        case "DISPLAYNAME":
                        case "CAPTION":
                            strDisplayName = xAttr.Value;
                            break;
                        case "DESCRIPTION":
                            strDescription = xAttr.Value;
                            break;
                    }
                }
                bool bColumnAdded = false;
                string strColumnName = childNode.Name;
                if (strColumnName.Trim('_') == String.Empty)
                {
                    strColumnName = String.Empty;
                }
                if (bAutoIncrement)
                {
                    columnBuilder.AddColumn(strColumnName, bAutoIncrement, intStartValue, intIncrement);
                    bColumnAdded = true;
                }
                if (!bColumnAdded && !String.IsNullOrEmpty(strExpression))
                {
                    columnBuilder.AddColumn(strColumnName, rct, strExpression);
                    bColumnAdded = true;
                }

                if ((!bColumnAdded) && (bForeignKey))
                {
                    columnBuilder.AddColumn(strColumnName, bForeignKey);
                    bColumnAdded = true;
                }

                if (!bColumnAdded) //This is a regular column with regex... let us add this to the column builder...
                {
                    if (!String.IsNullOrEmpty(separator))
                    {
                        if (hasDoubleQuotes)
                        {
                            columnBuilder.AddColumn('\"' + strColumnName + '\"', separator[0], rct);
                        }
                        else
                        {
                            if (childNode.NextSibling == null)
                            {
                                columnBuilder.AddColumn(strColumnName, ".*", rct);
                            }
                            else
                            {
                                columnBuilder.AddColumn(strColumnName, "[^" + columnBuilder.RegexFormattedOutput(separator[0]) + "\\n]*", prefix, suffix, rct);
                            }
                        }
                    }
                    else
                    {
                        if (columnLength > 0)
                        {
                            columnBuilder.AddColumn(strColumnName, columnLength, rct);
                        }
                        else
                        {
                            columnBuilder.AddColumn(strColumnName, ".*", rct);
                        }
                    }

                    RegexColumn addedColumn = columnBuilder.Columns[columnBuilder.Columns.Count - 1];

                    if (!String.IsNullOrWhiteSpace(strCondition))
                    {
                        //There is a condition to be matched with the value... let us set it to the last column added...
                        addedColumn.ValueMatchingCondition = strCondition;
                    }

                    if (bPrimaryKey)
                    {
                        addedColumn.IsUnique = bPrimaryKey;
                    }

                    if (strDisplayName != strColumnName)
                    {
                        addedColumn.DisplayName = strDisplayName;
                    }
                    if (!String.IsNullOrEmpty(strDescription)) {
                        addedColumn.Description = strDescription;
                    }
                }

            }
        }


        private DataTable DataTable
        {
            get
            {
                if (!Tables.Contains(TableName))
                {
                    var newTable = new DataTable(TableName);
                    Tables.Add(newTable);
                }
                return Tables[TableName];
            }
        }

        private void AddMisRead(string missRead)
        {
            if (MisReads == null)
                MisReads = new List<string>();
            MisReads.Add(missRead);
        }

        private void RemoveDataTables()
        {
            Relations.Clear();
            foreach (DataTable dt in Tables)
            {
                ClearConstraintsOnTable(dt);
            }
            EnforceConstraints = false;
            while (Tables.Count > 0)
            {
                Tables.RemoveAt(0);
            }
            EnforceConstraints = true;
        }

        private void ClearConstraintsOnTable(DataTable dt)
        {
            foreach (DataRelation dr in dt.ChildRelations)
            {
                ClearConstraintsOnTable(dr.ChildTable);
            }
            dt.Constraints.Clear();
        }

        private void BuildRegexSchemaIntoDataSet()
        {
            if ((ContentExpression == null) && !ContentExpressionHasChanged) return;
            RemoveDataTables();
            LoadColumnsToTable(DataTable, _regexColumns);

            foreach (ConditionalRegexParser crp in Parsers)
            {
                DataTable crpDT = null;
                if (!Tables.Contains(crp.TableName))
                {
                    crpDT = Tables.Add(crp.TableName);
                }
                else
                {
                    crpDT = Tables[crp.TableName];
                }
                LoadColumnsToTable(crpDT, crp.RegexColumns);
            }

        }

        private void LoadColumnsToTable(DataTable dataTable, List<RegexColumn> regexColumns)
        {
            foreach (RegexColumn rColumn in regexColumns)
            {
                if (!String.IsNullOrEmpty(rColumn.ColumnName))
                {

                    DataColumn dColumn = new DataColumn(rColumn.ColumnName, rColumn.ColumnTypeAsType);
                    if (rColumn.AutoIncrement)
                    {
                        dColumn.AutoIncrement = rColumn.AutoIncrement;
                        dColumn.AutoIncrementSeed = rColumn.StartValue;
                        dColumn.AutoIncrementStep = rColumn.Increment;
                    }
                    if (!String.IsNullOrEmpty(rColumn.Expression))
                    {
                        dColumn.Expression = rColumn.Expression;
                    }
                    dColumn.Unique = rColumn.IsUnique;
                    if (!String.IsNullOrEmpty(rColumn.DisplayName))
                    {
                        dColumn.Caption = rColumn.DisplayName;
                    }
                    dataTable.Columns.Add(dColumn);

                    if (rColumn.IsForeignKey)
                    {
                        foreach (DataTable dTable in dataTable.DataSet.Tables)
                        {
                            if ((dTable.TableName != dataTable.TableName) && (dTable.Columns.Contains(rColumn.ColumnName)))
                            {
                                DataColumn parentDataColumn = dTable.Columns[rColumn.ColumnName];
                                if (parentDataColumn.AutoIncrement || parentDataColumn.Unique)
                                {
                                    dColumn.DataType = dTable.Columns[rColumn.ColumnName].DataType;
                                    this.Relations.Add(dTable.Columns[rColumn.ColumnName], dColumn);
                                }
                            }
                        }
                    }
                }
            }
        }


        #region public methods
        public override void LoadProfileSettings(XmlNode xNode)
        {
            if (xNode == null) return;
            bool hasHeaderRow = false;
            bool skipFirstRow = false;
            string separator = "";
            //Table level attributes...
            foreach (XmlAttribute xAttr in xNode.Attributes)
            {
                switch (xAttr.Name.ToUpper())
                {
                    case "SEPARATOR":
                        separator = xAttr.Value;
                        break;
                    case "TABLENAME":
                        TableName = xAttr.Value;
                        break;
                    case "HASHEADER":
                        hasHeaderRow = Boolean.Parse(xAttr.Value);
                        break;
                    case "SKIPFIRSTROW":
                        skipFirstRow = Boolean.Parse(xAttr.Value);
                        break;
                }
            }

            UseFirstRowNamesAsColumnNames = hasHeaderRow;
            SkipFirstRow = skipFirstRow;
            RegexColumnBuilder rcb = new RegexColumnBuilder(separator);
            if (xNode.HasChildNodes)
            {
                foreach (XmlNode childNode in xNode.ChildNodes)
                {
                    ParseColumnOrParser(rcb, childNode, separator);
                }
            }
            ColumnBuilder = rcb;
            BuildRegexSchemaIntoDataSet();
        }

        /// <summary>
        ///     Reads every line in the text file and tries to match
        ///     it with the given regular expression.
        ///     Every match will be placed as a new row in the
        ///     datatable
        /// </summary>
        /// <param name="textFile"></param>
        /// <param name="regularExpression"></param>
        /// <param name="tableName"></param>
        public void Fill(Stream textFile, Regex regularExpression, string tableName)
        {
            TableName = tableName;
            Fill(textFile, regularExpression);
        }

        /// <summary>
        ///     Reads every line in the text file and tries to match
        ///     it with the given regular expression.
        ///     Every match will be placed as a new row in the
        ///     datatable
        /// </summary>
        /// <param name="textFile"></param>
        /// <param name="regularExpression"></param>
        public void Fill(Stream textFile, Regex regularExpression)
        {
            ContentExpression = regularExpression;
            Fill(textFile);
        }

        /// <summary>
        ///     Reads every line in the text file and tries to match
        ///     it with the given regular expression.
        ///     Every match will be placed as a new row in the
        ///     datatable
        /// </summary>
        /// <param name="textFile"></param>
        public void Fill(Stream textFile)
        {
            TextFile = textFile;
            Fill();
        }

        public virtual void Fill(string textFileName)
        {
            using (FileStream fs = new FileStream(textFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Fill(fs);
            }
        }

        /// <summary>
        ///     Reads every line in the text file and tries to match
        ///     it with the given regular expression.
        ///     Every match will be placed as a new row in the
        ///     datatable
        /// </summary>
        /// <returns></returns>
        public override void Fill()
        {
            if (TextFile == null)
                throw new ApplicationException("No stream available to convert to a DataSet");

            TextFile.Seek(0, SeekOrigin.Begin);
            var sr = new StreamReader(TextFile);

            var readLine = sr.ReadLine();
            var isFirstLine = true;

            int lineNumber = 1;

            SendMessageToCallingApplicationHandler(0, "Loading First Line");

            if (UseFirstRowNamesAsColumnNames && (FirstRowExpression == null) && ((_regexColumns == null) || (_regexColumns.Count == 0)))
            {
                string firstRow = readLine;
                Regex regexSeparator = new Regex("^([a-zA-Z0-9_\"]*)(?<Separator>.)");
                Match match = regexSeparator.Match(firstRow);
                if (match.Success)
                {
                    string fieldSeparator = match.Groups["Separator"].ToString();
                    string[] columnNames = firstRow.Split(fieldSeparator[0]);
                    ColumnBuilder = new RegexColumnBuilder(fieldSeparator, columnNames);
                }
                UseFirstRowNamesAsColumnNames = false;
                SkipFirstRow = true;
            }


            SendMessageToCallingApplicationHandler(lineNumber, "Building Schema...");
            BuildRegexSchemaIntoDataSet();

            while (readLine != null)
            {
                if (isFirstLine && UseFirstRowNamesAsColumnNames && !SkipFirstRow)
                {
                    SendMessageToCallingApplicationHandler(lineNumber, "Building First Row...");
                    if (FirstRowExpression == null)
                        throw new RegexDataSetException(
                            "FirstRowExpression is not set, but UseFirstRowNamesAsColumnNames is set to true");
                    if (!FirstRowExpression.IsMatch(readLine))
                        throw new RegexDataSetException(
                            "The first row in the file does not match the FirstRowExpression");

                    var m = FirstRowExpression.Match(readLine);
                    foreach (var sGroup in FirstRowExpression.GetGroupNames())
                        if (sGroup != DefaultGroup)
                            DataTable.Columns[sGroup].ExtendedProperties.Add(NewName, m.Groups[sGroup].Value);
                }
                else if (!(isFirstLine && SkipFirstRow))
                {
                    ProcessRowObject(readLine);
                }
                SendMessageToCallingApplicationHandler(lineNumber, "Processed line");

                readLine = sr.ReadLine();
                lineNumber += 1;
                isFirstLine = false;
            }
            if (!UseFirstRowNamesAsColumnNames) return;
            foreach (DataColumn column in DataTable.Columns)
                if (column.ExtendedProperties.ContainsKey(NewName))
                    column.ColumnName = column.ExtendedProperties[NewName].ToString();
        }

        public override void ProcessRowObject(object row)
        {
            if (row is string)
            {
                lock (QueueToParse)
                {
                    QueueToParse.Enqueue((string)row);
                }
            }
            if (row is Dictionary<string, object>)
            {
                Dictionary<string, object> Data = (Dictionary<string, object>)row;
                if (Data.ContainsKey("AdditionalContent"))
                {
                    lock (QueueToParse)
                    {
                        QueueToParse.Enqueue((string)Data["AdditionalContent"]);
                    }
                }
            }
            if (!_parsingInProgress)
            {
                _parsingInProgress = true;
                ParseAndLoadLinesFromQueue();
                _parsingInProgress = false;
            }
        }

        private void ParseAndLoadLinesFromQueue()
        {
            string lineToParse = String.Empty;
            lock (QueueToParse)
            {
                if (QueueToParse.Count > 0)
                {
                    lineToParse = QueueToParse.Dequeue();
                }
            }
            if (!String.IsNullOrEmpty(lineToParse))
            {
                ParseAndLoadLines(lineToParse);
                ParseAndLoadLinesFromQueue();
            }
        }

        protected virtual void ParseAndLoadLines(string lines)
        {

            foreach (string readLine in lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                bool bImportRow = false;
                if ((ContentExpression != null) && ContentExpression.IsMatch(readLine))
                {
                    var m = ContentExpression.Match(readLine);
                    bImportRow = true;
                    short groupNum;
                    Dictionary<string, object> rowDict = new Dictionary<string, object>();
                    foreach (var sGroup in ContentExpression.GetGroupNames())
                    {
                        if ((sGroup != DefaultGroup) && (!Int16.TryParse(sGroup, out groupNum)))
                        {
                            RegexColumn curRegexColumn = _regexColumns.Find(r => r.ColumnName == sGroup);
                            if (!String.IsNullOrWhiteSpace(curRegexColumn.ValueMatchingCondition) && (!Regex.IsMatch(m.Groups[sGroup].Value, curRegexColumn.ValueMatchingCondition)))
                            {
                                bImportRow = false;
                                break;
                            }
                            string fieldValue = m.Groups[sGroup].Value;
                            fieldValue = fieldValue.Trim('\"');
                            if (DataTable.Columns[sGroup] != null)
                            {

                                if (DataTable.Columns[sGroup].DataType == typeof(int))
                                    rowDict[sGroup] = Convert.ToInt32(fieldValue);
                                else if (DataTable.Columns[sGroup].DataType == typeof(double))
                                    rowDict[sGroup] = Convert.ToDouble(fieldValue);
                                else if (DataTable.Columns[sGroup].DataType == typeof(DateTime))
                                    rowDict[sGroup] = Convert.ToDateTime(fieldValue);
                                else
                                    rowDict[sGroup] = fieldValue;
                            }
                        }
                    }

                    if (bImportRow)
                    {
                        DataRow newRow = DataTable.NewRow();
                        PopulateDictionaryToRow(newRow);
                        foreach (KeyValuePair<string, object> kvPair in rowDict)
                        {
                            newRow[kvPair.Key] = kvPair.Value;
                        }
                        DataTable.Rows.Add(newRow);
                        PopulateRowToDictionary(DataTable.Rows[DataTable.Rows.Count - 1]);
                    }
                }

                bool bLineParsed = false;
                if (!bImportRow)
                {
                    foreach (ConditionalRegexParser crp in Parsers)
                    {
                        if (crp.ConditionRegex.IsMatch(readLine))
                        {
                            DataTable crpDataTable = Tables[crp.TableName];
                            var m = crp.parseRegex.Match(readLine);
                            short groupNum;
                            Dictionary<string, object> rowDict = new Dictionary<string, object>();
                            foreach (var sGroup in crp.parseRegex.GetGroupNames())
                            {
                                if ((sGroup != DefaultGroup) && (!Int16.TryParse(sGroup, out groupNum)))
                                {
                                    string fieldValue = m.Groups[sGroup].Value;
                                    fieldValue = fieldValue.Trim('\"');
                                    if (crpDataTable.Columns[sGroup] != null)
                                    {
                                        if (crpDataTable.Columns[sGroup].DataType == typeof(int))
                                            rowDict[sGroup] = Convert.ToInt32(fieldValue);
                                        else if (crpDataTable.Columns[sGroup].DataType == typeof(double))
                                            rowDict[sGroup] = Convert.ToDouble(fieldValue);
                                        else if (crpDataTable.Columns[sGroup].DataType == typeof(DateTime))
                                            rowDict[sGroup] = Convert.ToDateTime(fieldValue);
                                        else
                                            rowDict[sGroup] = fieldValue;
                                    }
                                }
                            }

                            DataRow newRow = crpDataTable.NewRow();
                            PopulateDictionaryToRow(newRow);
                            foreach (KeyValuePair<string, object> kvPair in rowDict)
                            {
                                newRow[kvPair.Key] = kvPair.Value;
                            }
                            crpDataTable.Rows.Add(newRow);
                            PopulateRowToDictionary(crpDataTable.Rows[crpDataTable.Rows.Count - 1]);
                            bLineParsed = true;
                            bImportRow = true;
                            break;
                        }
                    }
                }
                if (!bLineParsed)
                {
                    AddMisRead(readLine);
                }

            }

        }

        private void PopulateRowToDictionary(DataRow dataRow)
        {
            foreach (DataColumn dColumn in dataRow.Table.Columns)
            {
                rowDict[dColumn.ColumnName] = dataRow[dColumn].ToString();
            }
        }

        private void PopulateDictionaryToRow(DataRow dataRow)
        {
            foreach (DataColumn dColumn in dataRow.Table.Columns)
            {
                if (rowDict.ContainsKey(dColumn.ColumnName))
                {
                    if (dColumn.AutoIncrement || !String.IsNullOrEmpty(dColumn.Expression))
                    {
                        rowDict.Remove(dColumn.ColumnName);
                    }
                    else
                    {
                        dataRow[dColumn] = rowDict[dColumn.ColumnName];
                    }
                }
            }
        }
        #endregion


    }
}
