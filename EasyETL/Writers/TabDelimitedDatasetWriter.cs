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
    [DisplayName("TAB Writer")]
    [EasyProperty("Delimiter","TAB")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("IncludeHeader", "Include Table Header", "True", "", "True;False")]
    [EasyField("IncludeQuotes", "Surround the Field Name and Values by quotes?", "True", "", "True;False")]
    public class TabDelimitedDatasetWriter : DelimitedDatasetWriter
    {
        public TabDelimitedDatasetWriter()
            : base()
        {
            Delimiter = '\t';
        }

        public TabDelimitedDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
            Delimiter = '\t';
        }

        public TabDelimitedDatasetWriter(DataSet dataSet, string fileName)
            : base(dataSet, fileName)
        {
            Delimiter = '\t';
        }
    }
}
