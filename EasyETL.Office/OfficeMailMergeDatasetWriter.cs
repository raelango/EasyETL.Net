using EasyETL.Attributes;
using EasyETL.Writers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    [DisplayName("Office MailMerge Writer")]
    [EasyField("ExportFileName", "Name of output file.  You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    [EasyField("TemplateFileName", "Name of template file to use. You can use variables with [varname].. date and time can be specified [dd],[hh] etc.,")]
    public class OfficeMailMergeDatasetWriter : OfficeDatasetWriter
    {
        public OfficeMailMergeDatasetWriter() : base(OfficeFileType.WordDocument) {}

        public OfficeMailMergeDatasetWriter(OfficeFileType fileType)
            : base(fileType)
        {
            PopulatePropertiesOnly = true;
        }

        public OfficeMailMergeDatasetWriter(DataSet dataSet)
            : base(dataSet)
        {
            PopulatePropertiesOnly = true;
        }

        public OfficeMailMergeDatasetWriter(DataSet dataSet, string fileName, string templateFileName = "")
            : base(dataSet, fileName, templateFileName)
        {
            PopulatePropertiesOnly = true;
        }

        public override void Write()
        {
            DataSet fullDataSet = _dataSet;
            if (_dataSet == null) return;
            string originalTemplateFileName = TemplateFileName;
            string originalFileName = ExportFileName;
            foreach (DataTable dTable in fullDataSet.Tables)
            {
                DataSet tempDataSet = new DataSet(fullDataSet.DataSetName);
                tempDataSet.Tables.Add(dTable.Clone());
                DataTable tempTable = tempDataSet.Tables[0];
                foreach (DataRow dRow in dTable.Rows)
                {
                    tempTable.Clear();
                    tempTable.ImportRow(dRow);
                    _dataSet = tempDataSet;
                    string tempTemplateFileName =  PopulatedName(originalTemplateFileName);
                    string tempFileName = PopulatedName(originalFileName);

                    ExportFileName = tempFileName;
                    TemplateFileName = tempTemplateFileName;
                    base.Write();
                }
            }
            ExportFileName = originalFileName;
            TemplateFileName = originalTemplateFileName;
            _dataSet = fullDataSet;
        }
    }
}
