using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Data.OleDb;

namespace EasyETL.DataSets
{
    public class ExcelDataSet : RegexDataSet 
    {

        private string _excelFileName = String.Empty;

        public bool HeaderRow = false;

        public override void Fill(string textFileName)
        {
            if (File.Exists(textFileName))
            {
                _excelFileName = textFileName;
                Fill();
            }
            else
            {
                _excelFileName = String.Empty;
            }
        }

        public override void Fill()
        {
            this.Tables.Clear();
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + _excelFileName + ";Extended Properties=\"Excel 12.0 Xml;HDR=" + (HeaderRow ? "YES" : "NO") + "\";";
            if (_excelFileName.EndsWith(".XLS", StringComparison.CurrentCultureIgnoreCase))
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _excelFileName + ";Extended Properties=\"Excel 8.0;HDR=" + (HeaderRow ? "YES" : "NO") + "\";";
            }
            using (OleDbConnection conn = new OleDbConnection(connString)) {
                conn.Open();
                DataTable schemaTable = conn.GetSchema("Tables");
                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheetName = schemaRow["TABLE_NAME"].ToString();
                    if (sheetName.EndsWith("'"))
                    {
                        using (OleDbDataAdapter sheetAdapter = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "]", conn))
                        {
                            DataTable excelTable = this.Tables.Add(sheetName);
                            sheetAdapter.Fill(excelTable);
                        }
                    }
                }
            }
        }

    }
}
