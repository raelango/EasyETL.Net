using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.DataSets
{
    public class JsonDataSet : RegexDataSet
    {


        public override void Fill()
        {
            if (TextFile == null)
                throw new ApplicationException("No stream available to convert to a DataSet");

            SendMessageToCallingApplicationHandler(0, "Loading Json Contents");

            TextFile.Seek(0, SeekOrigin.Begin);
            var sr = new StreamReader(TextFile);

            var jsonString = sr.ReadToEnd();
            sr.Close();
            this.Tables.Clear();
            string TableExpression = @"({|,)\s*" + '"' + @"(?<TableName>.*?)" + '"' + @"\s*:\s*(\[\s*(?<TableContents>.*?)\s*\]\s*)";
            string RowExpression = @"{((\s*" + '"' + @"(?<ColumnName>.*?)" + '"' + @"\s*:\s*" + '"' + @"(?<ColumnValue>.*?)" + '"' + @").*?(,|\n))*.*?}";


            // Get a match for all the tables in the HTML  
            MatchCollection Tables = Regex.Matches(jsonString, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            // Loop through each table element  
            foreach (Match Table in Tables)
            {
                // Reset the current row counter and the header flag  
                string tableName = Table.Groups["TableName"].Value.Trim('"');
                string tableContents = Table.Groups["TableContents"].Value.Trim('"');
                DataTable dt = new DataTable
                {
                    //Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names)  
                    TableName = tableName
                };

                MatchCollection Rows = Regex.Matches(tableContents, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                if (Rows.Count > 0)
                {
                    foreach (Capture columnCapture in Rows[0].Groups["ColumnName"].Captures)
                    {
                        dt.Columns.Add(columnCapture.Value);
                    }
                    foreach (Match row in Rows)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < row.Groups["ColumnName"].Captures.Count; i++)
                        {
                            dr[row.Groups["ColumnName"].Captures[i].Value] = row.Groups["ColumnValue"].Captures[i].Value;
                        }
                        dt.Rows.Add(dr);
                    }

                }
                // Add the DataTable to the DataSet  
                this.Tables.Add(dt);
            }
        }

    }
}
