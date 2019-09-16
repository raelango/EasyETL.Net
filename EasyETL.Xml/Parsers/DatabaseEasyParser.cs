using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Xml.Parsers
{
    public enum EasyDatabaseConnectionType
    {
        edctOledb,
        edctODBC,
        edctSQL
    }

    public class DatabaseEasyParser : AbstractEasyParser
    {
        public string ConnectionString;
        public EasyDatabaseConnectionType ConnectionType;

        public DatabaseEasyParser(EasyDatabaseConnectionType connType, string connString) {
            ConnectionType = connType;
            ConnectionString = connString;
        }

        public override XmlDocument Load(string filename, XmlDocument xDoc = null)
        {
            return LoadFromQuery(filename, xDoc);
        }

        public XmlDocument LoadFromQuery(string sqlToExecute, XmlDocument xDoc = null)
        {
            DataSet ds = new DataSet();
            DataAdapter adapter = null;
            if (xDoc == null)
            {
                xDoc = new XmlDocument();
            }

            try
            {
                switch (ConnectionType)
                {
                    case EasyDatabaseConnectionType.edctODBC:
                        adapter = new OdbcDataAdapter(sqlToExecute, new OdbcConnection(ConnectionString));
                        break;
                    case EasyDatabaseConnectionType.edctOledb:
                        adapter = new OleDbDataAdapter(sqlToExecute, new OleDbConnection(ConnectionString));
                        break;
                    case EasyDatabaseConnectionType.edctSQL:
                        adapter = new SqlDataAdapter(sqlToExecute, new SqlConnection(ConnectionString));
                        break;
                }
                if (adapter != null)
                {
                    adapter.Fill(ds);
                }
                adapter.Dispose();
                xDoc.LoadXml(ds.GetXml());
            }
            catch
            {
            }
            return xDoc;
        }

    }
}
