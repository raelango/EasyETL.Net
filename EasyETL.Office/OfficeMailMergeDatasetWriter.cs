using EasyETL.Writers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Writers
{
    public class OfficeMailMergeDatasetWriter : OfficeDatasetWriter
    {

        public OfficeMailMergeDatasetWriter(OfficeFileType fileType = OfficeFileType.WordDocument)
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
            string originalFileName = _fileName;
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

                    _fileName = tempFileName;
                    TemplateFileName = tempTemplateFileName;
                    base.Write();
                }
            }
            _fileName = originalFileName;
            TemplateFileName = originalTemplateFileName;
            _dataSet = fullDataSet;
        }
    }
}
