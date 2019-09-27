using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("ODBC Database")]
    [EasyField("ConnectionString","ODBC Connection String")]
    [EasyField("Query","Query to Execute")]
    public class OdbcDatabaseEasyParser : DatabaseEasyParser
    {
        public OdbcDatabaseEasyParser() : base(EasyDatabaseConnectionType.edctODBC) { }
        public OdbcDatabaseEasyParser(string connectionString) : base(EasyDatabaseConnectionType.edctODBC,connectionString) { }
    }
}
