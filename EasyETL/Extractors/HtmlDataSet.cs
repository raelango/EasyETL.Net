using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyETL.DataSets
{
    public class HtmlDataSet : RegexDataSet
    {
        public override void Fill()
        {
            if (TextFile == null)
                throw new ApplicationException("No stream available to convert to a DataSet");

            TextFile.Seek(0, SeekOrigin.Begin);
            var sr = new StreamReader(TextFile);

            var htmlString = sr.ReadToEnd();
            sr.Close();

            SendMessageToCallingApplicationHandler(0, "Loading Html Contents");
            ConvertHTMLTablesToDataSet(htmlString);
        }


        private void ConvertHTMLTablesToDataSet(string HTML)
        {
            DataTable dt = null;
            DataRow dr = null;
            string TableExpression = "<TABLE[^>]*>(.*?)</TABLE>";
            string HeaderExpression = "(<TH>|<TH[\\s]>)(.*?)</TH>";
            string RowExpression = "(<TR>|<TR[\\s]>)(.*?)</TR>";
            string ColumnExpression = "(<TD>|<TD[\\s]>)(.*?)</TD>";
            bool HeadersExist = false;
            int iCurrentColumn = 0;
            int iCurrentRow = 0;
            // Get a match for all the tables in the HTML  
            MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            // Loop through each table element  
            foreach (Match Table in Tables)
            {
                // Reset the current row counter and the header flag  
                iCurrentRow = 0;
                HeadersExist = false;
                // Add a new table to the DataSet  
                dt = new DataTable();
                //Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names)  
                // if (Table.Value.Contains("<th"))  
                dt.TableName = "Table" + (this.Tables.Count + 1).ToString();
                Match TableNameMatch = null;
                if (Regex.IsMatch(Table.Value,"id=(?<TableName>.\\w+)")) TableNameMatch = Regex.Match(Table.Value,"id=(?<TableName>.\\w+)");
                if (Regex.IsMatch(Table.Value,"name=(?<TableName>.\\w+)")) TableNameMatch = Regex.Match(Table.Value,"name=(?<TableName>.\\w+)");

                if (TableNameMatch != null)
                    dt.TableName = TableNameMatch.Groups["TableName"].ToString().Trim('"');

                if (Table.Value.IndexOf("<TH",StringComparison.OrdinalIgnoreCase) >=0)
                {
                    // Set the HeadersExist flag  
                    HeadersExist = true;
                    // Get a match for all the rows in the table  
                    MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    // Loop through each header element  
                    foreach (Match Header in Headers)
                    {
                        if (!dt.Columns.Contains(Header.Groups[2].ToString()))
                        {
                            dt.Columns.Add(Header.Groups[2].ToString());
                        }
                    }
                }
                else
                {
                    for (int iColumns = 1; iColumns <= Regex.Matches(Regex.Matches(Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase)[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase).Count; iColumns++)
                    {
                        dt.Columns.Add("Column " + iColumns);
                    }
                }
                //Get a match for all the rows in the table  
                MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                // Loop through each row element  
                foreach (Match Row in Rows)
                {
                    // Only loop through the row if it isn't a header row  
                    if (!(iCurrentRow == 0 && HeadersExist))
                    {
                        // Create a new row and reset the current column counter  
                        dr = dt.NewRow();
                        iCurrentColumn = 0;
                        // Get a match for all the columns in the row  
                        MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        bool bImportRow = Columns.Count >0;

                        // Loop through each column element  
                        foreach (Match Column in Columns)
                        {
                            // Add the value to the DataRow  
                            if (dr.ItemArray.Count() > iCurrentColumn)
                            {
                                if (_regexColumns != null)
                                {
                                    RegexColumn curRegexColumn = _regexColumns.Find(r => r.ColumnName == dt.Columns[iCurrentColumn].ColumnName);
                                    if (curRegexColumn != null)
                                    {
                                        if (!String.IsNullOrWhiteSpace(curRegexColumn.ValueMatchingCondition) && (!Regex.IsMatch(Column.Groups[2].ToString(), curRegexColumn.ValueMatchingCondition)))
                                        {
                                            bImportRow = false;
                                            break;
                                        }
                                    }
                                }

                                dr[iCurrentColumn] = Column.Groups[2].ToString();
                            }
                           // Increase the current column  
                            iCurrentColumn++;
                        }

                        // Add the DataRow to the DataTable  
                        if (bImportRow)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                    // Increase the current row counter  
                    SendMessageToCallingApplicationHandler(iCurrentRow, "Processed record for Table [" + dt.TableName + "]");
                    iCurrentRow++;
                }
                // Add the DataTable to the DataSet  
                this.Tables.Add(dt);
            }
        }  
    }
}
