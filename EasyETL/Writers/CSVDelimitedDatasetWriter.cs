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
