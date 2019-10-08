using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Actions.DemoActions
{
    [DisplayName("Execute Command")]
    [Description("This performs calls a specific executable (EXE, COM, BAT) with the selected row field as argument")]
    [EasyField("ArgumentFields", "The name of the column(s) that needs to be passed as argument.  Separate multiple columns with ';'")]
    [EasyField("Command", "The Full path of the executable (EXE, COM, BAT) to be executed.")]
    public class ExecAction : AbstractEasyAction
    {
        public string Command;
        public string ArgumentFields;

        public override bool IsFieldSettingsComplete()
        {
            LoadSettings();
            return (!String.IsNullOrWhiteSpace(Command) && !String.IsNullOrWhiteSpace(ArgumentFields));
        }

        private void LoadSettings()
        {
            Command = (SettingsDictionary.ContainsKey("Command") ? SettingsDictionary["Command"] : "");
            ArgumentFields = (SettingsDictionary.ContainsKey("ArgumentFields") ? SettingsDictionary["ArgumentFields"] : "");
        }

        public override bool CanExecute(Dictionary<string, string> dataDictionary)
        {
            LoadSettings();
            if (String.IsNullOrWhiteSpace(Command) || String.IsNullOrWhiteSpace(ArgumentFields)) return false;
            foreach (string argument in ArgumentFields.Split(';'))
            {
                if (!String.IsNullOrWhiteSpace(argument))
                {
                    if ((argument.StartsWith("[")) && (argument.EndsWith("]")))
                    {
                        if (!dataDictionary.ContainsKey(argument.Trim('[', ']'))) return false;
                    }
                }
            }
            return true;
        }

        public override void Execute(Dictionary<string, string> dataDictionary)
        {
            if (CanExecute(dataDictionary))
            {
                ProcessStartInfo psi = new ProcessStartInfo(Command);
                string populatedArguments = "";
                foreach (string argument in ArgumentFields.Split(';'))
                {
                    if (!String.IsNullOrWhiteSpace(argument))
                    {
                        if (!String.IsNullOrWhiteSpace(populatedArguments)) populatedArguments += " ";
                        if ((argument.StartsWith("[")) && (argument.EndsWith("]")))
                        {
                            populatedArguments += '\"' + dataDictionary[argument.Trim('[', ']')] + '\"';
                        }
                        else
                        {
                            populatedArguments += '\"' + argument + '\"';
                        }
                    }
                }
                psi.Arguments = populatedArguments;
                psi.WorkingDirectory = Path.GetDirectoryName(Command);
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                Process.Start(psi);
            }
        }
    }
}
