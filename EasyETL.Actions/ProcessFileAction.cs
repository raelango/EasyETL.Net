using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Actions.DemoActions
{
    [DisplayName("Process File")]
    [Description("This performs specific action on the selected row data")]
    [EasyField("FileNameField", "The name of the column that contains the full path or filename")]
    [EasyField("FolderNameField", "The name of the column that contains the FolderName.  Optional Field")]
    [EasyField("DefaultFolder", "The name of the default folder.  If Empty, the EXE path becomes the default folder")]
    [EasyField("ActionName", "The action to be done on the file.")]
    public class ProcessFileAction : AbstractEasyAction
    {
        public string FileNameField;
        public string FolderNameField;
        public string DefaultFolder;
        public string ActionName;
        public string WorkingFileName;

        public override bool IsFieldSettingsComplete()
        {
            LoadSettings();
            return (!String.IsNullOrWhiteSpace(FileNameField) && !String.IsNullOrWhiteSpace(ActionName));
        }

        private void LoadSettings()
        {
            FileNameField = (SettingsDictionary.ContainsKey("FileNameField") ? SettingsDictionary["FileNameField"] : "");
            FolderNameField = (SettingsDictionary.ContainsKey("FolderNameField") ? SettingsDictionary["FolderNameField"] : "");
            DefaultFolder = (SettingsDictionary.ContainsKey("DefaultFolder") ? SettingsDictionary["DefaultFolder"] : "");
            ActionName = (SettingsDictionary.ContainsKey("ActionName") ? SettingsDictionary["ActionName"] : "");
        }

        private bool CanExecute(string fileName,string folderName)
        {
            if (String.IsNullOrWhiteSpace(fileName)) return false;
            if ((!fileName.Contains(Path.DirectorySeparatorChar)) && (!String.IsNullOrWhiteSpace(folderName))) fileName = Path.Combine(folderName, fileName);
            if ((!fileName.Contains(Path.DirectorySeparatorChar)) && (SettingsDictionary.ContainsKey("DefaultFolder"))) {
               fileName = Path.Combine(SettingsDictionary["DefaultFolder"],fileName);
            }
            WorkingFileName = fileName;
            return File.Exists(fileName);
        }

        public override bool CanExecute(Dictionary<string, string> dataDictionary)
        {
            LoadSettings();
            if (String.IsNullOrWhiteSpace(FileNameField)) return false;
            string fileName = dataDictionary.ContainsKey(FileNameField) ? dataDictionary[FileNameField] : "";
            string folderName = ((!String.IsNullOrWhiteSpace(FolderNameField)) && dataDictionary.ContainsKey(FolderNameField)) ? dataDictionary[FolderNameField] : "";
            return CanExecute(fileName, folderName);
        }

        public override void Execute(Dictionary<string, string> dataDictionary)
        {
            if (CanExecute(dataDictionary) && (!String.IsNullOrWhiteSpace(ActionName)))
            {
                ProcessStartInfo psi = new ProcessStartInfo(WorkingFileName)
                {
                    Verb = ActionName
                };
                Process.Start(psi);
            }
        }
    }
}
