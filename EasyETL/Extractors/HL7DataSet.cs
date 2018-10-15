using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace EasyETL.DataSets
{
    public class  HL7DataSet : DelimitedDataSet 
    {
        public HL7DataSet()
        {
            
        }
        public override void Fill(Stream textFile)
        {
            UseFirstRowNamesAsColumnNames = false;
            using (StreamReader sr = new StreamReader(textFile)) {
                while (!sr.EndOfStream)
                {
                    string hl7Line = sr.ReadLine();
                    string[] hl7fields = hl7Line.Split('|');
                    if (!this.Tables.Contains(hl7fields[0]))
                    {
                        this.Tables.Add(hl7fields[0]);
                    }

                    DataTable hl7Table = this.Tables[hl7fields[0]];

                    while (hl7Table.Columns.Count < hl7fields.Length)
                    {
                        hl7Table.Columns.Add();
                    }
                    hl7Table.Rows.Add(hl7fields);
                }
            }

            List<string> emptyTables = new List<string>();
            foreach (DataTable dt in this.Tables)
            {
                if (dt.Columns.Count == 0)
                {
                    emptyTables.Add(dt.TableName);
                }
            }

            foreach (string emptyTable in emptyTables)
            {
                this.Tables.Remove(emptyTable);
            }

        }
    }
}
