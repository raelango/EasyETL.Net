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
