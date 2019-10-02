using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Xml.Parsers
{
    [DisplayName("Oledb Database")]
    [EasyField("ConnectionString","Oledb Connection String")]
    [EasyField("Query","Query to Execute")]
    public class OledbDatabaseEasyParser : DatabaseEasyParser
    {
        public OledbDatabaseEasyParser() : base(EasyDatabaseConnectionType.edctOledb) { }
        public OledbDatabaseEasyParser(string connectionString) : base(EasyDatabaseConnectionType.edctOledb, connectionString) { }

    }
}
