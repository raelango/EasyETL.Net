using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace EasyXml.Parsers
{
    public class HtmlTableEasyParser : AbstractEasyParser
    {
        public override XmlDocument Load(TextReader txtReader, XmlDocument xDoc = null)
        {
            #region setup the rootNode
            XmlNode rootNode;
            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }
            if (xDoc.DocumentElement == null)
            {
                rootNode = xDoc.CreateElement(RootNodeName);
                xDoc.AppendChild(rootNode);
            }

            rootNode = xDoc.DocumentElement;
            #endregion

            string HTML = txtReader.ReadToEnd();
            ConvertHTMLTablesToDataSet(HTML, rootNode);


            return xDoc;
        }

        private void ConvertHTMLTablesToDataSet(string HTML, XmlNode rootNode)
        {
            string TableExpression = "<TABLE[^>]*>(.*?)</TABLE>";
            string HeaderExpression = "(<TH>|<TH[\\s]>)(.*?)</TH>";
            string RowExpression = "(<TR>|<TR[\\s]>)(.*?)</TR>";
            string ColumnExpression = "(<TD>|<TD[\\s]>)(.*?)</TD>";
            bool HeadersExist = false;
            int iCurrentColumn = 0;
            int iCurrentRow = 0;
            // Get a match for all the tables in the HTML  
            try
            {
                MatchCollection Tables = Regex.Matches(HTML, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                // Loop through each table element  
                int tableIndex = 1;
                foreach (Match Table in Tables)
                {
                    List<string> lstFields = new List<string>();
                    tableIndex++;
                    string tableName = RowNodeName + tableIndex.ToString();
                    // Reset the current row counter and the header flag  
                    iCurrentRow = 0;
                    HeadersExist = false;
                    Match TableNameMatch = null;
                    if (Regex.IsMatch(Table.Value, "id=(?<TableName>.\\w+)")) TableNameMatch = Regex.Match(Table.Value, "id=(?<TableName>.\\w+)");
                    if (Regex.IsMatch(Table.Value, "name=(?<TableName>.\\w+)")) TableNameMatch = Regex.Match(Table.Value, "name=(?<TableName>.\\w+)");

                    if (TableNameMatch != null)
                        tableName = TableNameMatch.Groups["TableName"].ToString().Trim('"');

                    //// Add a new table to the DataSet  
                    //XmlNode tableNode = rootNode.OwnerDocument.CreateElement(tableName);
                    //rootNode.AppendChild(tableNode);
                    //Create the relevant amount of columns for this table (use the headers if they exist, otherwise use default names)  

                    if (Table.Value.IndexOf("<TH", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Set the HeadersExist flag  
                        HeadersExist = true;
                        // Get a match for all the rows in the table  
                        MatchCollection Headers = Regex.Matches(Table.Value, HeaderExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        // Loop through each header element  
                        foreach (Match Header in Headers)
                        {

                            if (!lstFields.Contains(Header.Groups[2].ToString()))
                            {
                                lstFields.Add(Header.Groups[2].ToString());
                            }
                        }
                    }
                    else
                    {
                        MatchCollection tableCollection = Regex.Matches(Table.Value, TableExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        MatchCollection rowCollection = (tableCollection.Count > 0) ? Regex.Matches(tableCollection[0].ToString(), RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase) : null;
                        MatchCollection colCollection = (rowCollection.Count > 0) ? Regex.Matches(rowCollection[0].ToString(), ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase) : null;
                        if (colCollection.Count > 0)
                        {
                            for (int iColumns = 1; iColumns <= colCollection.Count; iColumns++)
                            {
                                lstFields.Add("Column" + iColumns);
                            }
                        }
                    }

                    SetFieldNames(lstFields.ToArray());

                    //Get a match for all the rows in the table  
                    MatchCollection Rows = Regex.Matches(Table.Value, RowExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    // Loop through each row element  
                    foreach (Match Row in Rows)
                    {
                        // Only loop through the row if it isn't a header row  
                        if (!(iCurrentRow == 0 && HeadersExist))
                        {
                            // Create a new row and reset the current column counter  
                            XmlNode rowNode = rootNode.OwnerDocument.CreateElement(tableName);
                            //rootNode.AppendChild(rowNode);

                            iCurrentColumn = 0;
                            // Get a match for all the columns in the row  
                            MatchCollection Columns = Regex.Matches(Row.Value, ColumnExpression, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

                            if (lstFields.Count < Columns.Count)
                            {
                                while (lstFields.Count < Columns.Count)
                                {
                                    lstFields.Add("Column" + (lstFields.Count + 1));
                                }
                                SetFieldNames(lstFields.ToArray());
                            }


                            // Loop through each column element  
                            foreach (Match Column in Columns)
                            {
                                XmlNode columnNode = rowNode.OwnerDocument.CreateElement(FieldNames[iCurrentColumn]);
                                if (Column.Groups.Count > 2) columnNode.InnerText = Column.Groups[2].ToString();
                                rowNode.AppendChild(columnNode);
                                iCurrentColumn++;
                            }

                            if (rowNode.HasChildNodes)
                            {
                                rootNode.AppendChild(rowNode);
                            }
                        }
                        // Increase the current row counter  
                        iCurrentRow++;
                    }
                }
            }
            catch 
            {

                throw;
            }
        }


    }
}
