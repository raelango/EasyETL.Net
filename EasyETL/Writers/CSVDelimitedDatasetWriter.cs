using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{

    [DisplayName("CSV Writer")]
    [EasyProperty("Delimiter", "Comma")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("IncludeHeader", "Include Table Header", "True", "", "True;False")]
    [EasyField("IncludeQuotes", "Surround the Field Name and Values by quotes?", "True", "", "True;False")]
    public class CSVDelimitedDatasetWriter : DelimitedDatasetWriter
    {
        public CSVDelimitedDatasetWriter()
            : base()
        {
        }

        public CSVDelimitedDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public CSVDelimitedDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet, fileName)
        {
        }
    }
    
}
