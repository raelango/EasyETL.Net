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
    [DisplayName("Word Writer")]
    [EasyField("IncludeHeader", "Include Table Header", "True", "", "True;False")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("TemplateFileName", "Name of template file to use. Leave Empty for no template file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    public class WordDatasetWriter : OfficeDatasetWriter
    {
        public WordDatasetWriter() : base(OfficeFileType.WordDocument) { }

        public WordDatasetWriter(OfficeFileType fileType)
            : base()
        {
            DestinationType = fileType;
        }

        public WordDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
        }

        public WordDatasetWriter(DataSet dataSet, string fileName, string templateFileName = "")
            : base(dataSet, fileName)
        {
            TemplateFileName = templateFileName;
        }
    }
}
