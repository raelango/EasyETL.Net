using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace EasyETL.Actions
{
    public abstract class AbstractEasyAction : IEasyAction, IEasyFieldInterface
    {

        public event EventHandler<EasyActionProgressEventArgs> OnProgress;

        public Dictionary<string, string> SettingsDictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

        public int MaximumItems;

        #region Protected Methods
        public void SetProgress(int currentIndex)
        {
            if (OnProgress != null)
            {
                OnProgress.Invoke(this, new EasyActionProgressEventArgs(MaximumItems,currentIndex));
            }
        }
        #endregion


        #region Implemented CanExecute methods
        public bool CanExecute(params Dictionary<string, string>[] dataDictionaries)
        {
            foreach (Dictionary<string, string> data in dataDictionaries)
            {
                if (!CanExecute(data)) return false;
            };
            return true;
        }

        public bool CanExecute(params XmlNode[] dataNodes)
        {
            foreach (XmlNode dataNode in dataNodes)
            {
                if (!CanExecute(dataNode)) return false;
            }
            return true;
        }


        public bool CanExecute(params EasyDynamicObject[] dataObjects)
        {
            foreach (EasyDynamicObject dataObject in dataObjects)
            {
                if (!CanExecute(dataObject)) return false;
            }
            return true;
        }

        public bool CanExecute(XmlNodeList dataNodes)
        {
            return CanExecute(dataNodes.Cast<XmlNode>().ToArray());
        }

        public bool CanExecute(params DataRow[] dataRows)
        {
            foreach (DataRow dataRow in dataRows)
            {
                if (!CanExecute(dataRow)) return false;
            }
            return true;
        }


        public virtual bool CanExecute(XmlNode dataNode)
        {
            return CanExecute(GetPropertiesFromNode(dataNode));
        }

        public virtual bool CanExecute(EasyDynamicObject dataObject)
        {
            return CanExecute(dataObject.Properties.ToDictionary(k => k.Key, k => k.Value.ToString()));
        }

        public virtual bool CanExecute(DataRow dataRow)
        {
            return CanExecute(GetPropertiesFromDataRow(dataRow));
        }        

        public virtual bool CanExecute(Dictionary<string, string> dataDictionary)
        {
            Dictionary<string, string> mergedDict = new Dictionary<string, string>(SettingsDictionary);
            foreach (KeyValuePair<string,string> kvPair in dataDictionary) {
                if (!mergedDict.ContainsKey(kvPair.Key))
                    mergedDict.Add(kvPair.Key, kvPair.Value);
                else
                    mergedDict[kvPair.Key] = kvPair.Value;
            }

            foreach (EasyFieldAttribute efAttribute in this.GetType().GetCustomAttributes(typeof(EasyFieldAttribute), true))
            {
                if (String.IsNullOrWhiteSpace(efAttribute.DefaultValue) && (!mergedDict.ContainsKey(efAttribute.FieldName))) return false;
            }
            return true;
        }

        
        #endregion 

        #region Implemented Execute Methods
        public virtual void Execute(params Dictionary<string, string>[] dataDictionaries)
        {
            MaximumItems = dataDictionaries.Length;
            int currentIndex = 0;
            foreach (Dictionary<string, string> data in dataDictionaries)
            {
                SetProgress(currentIndex++);
                Execute(data);
            }
        }

        public virtual void Execute(params XmlNode[] dataNodes)
        {
            MaximumItems = dataNodes.Length;
            int currentIndex = 0;
            foreach (XmlNode dataNode in dataNodes)
            {
                SetProgress(currentIndex++);
                Execute(dataNode);
            }
        }

        public virtual void Execute(params EasyDynamicObject[] dataObjects)
        {
            MaximumItems = dataObjects.Length;
            int currentIndex = 0;
            foreach (EasyDynamicObject dataObject in dataObjects)
            {
                SetProgress(currentIndex++);
                Execute(dataObject);
            }
        }

        public virtual void Execute(params DataRow[] dataRows)
        {
            MaximumItems = dataRows.Length;
            int currentIndex = 0;
            foreach (DataRow dataRow in dataRows)
            {
                SetProgress(currentIndex++);
                Execute(dataRow);
            }
        }

        public virtual void Execute(XmlNode dataNode)
        {
            Execute(GetPropertiesFromNode(dataNode));
        }

        public virtual void Execute(XmlNodeList dataNodes)
        {
            Execute(dataNodes.Cast<XmlNode>().ToArray());
        }

        public virtual void Execute(EasyDynamicObject dataObject)
        {
            Execute(dataObject.Properties.ToDictionary(k => k.Key, k => k.Value.ToString()));
        }

        public virtual void Execute(DataRow dataRow)
        {
            Execute(GetPropertiesFromDataRow(dataRow));
        }

        #endregion

        #region private methods
        private Dictionary<string, string> GetPropertiesFromNode(XmlNode dataNode)
        {
            Dictionary<string, string> props = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (XmlNode childNode in dataNode.ChildNodes)
            {
                props.Add(childNode.Name, childNode.Value);
            }
            foreach (XmlAttribute xAttr in dataNode.Attributes)
            {
                props.Add(xAttr.Name, xAttr.Value);
            }
            return props;
        }

        private Dictionary<string, string> GetPropertiesFromDataRow(DataRow dataRow)
        {
            Dictionary<string, string> props = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (DataColumn dColumn in dataRow.Table.Columns)
            {
                props.Add(dColumn.ColumnName, dataRow[dColumn].ToString());
            }
            return props;
        }


        #endregion 

        public virtual void Execute(Dictionary<string, string> dataDictionary)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsFieldSettingsComplete()
        {
            return true;
        }

        public virtual bool CanFunction()
        {
            return false;
        }

        public void LoadFieldSettings(Dictionary<string, string> settingsDictionary)
        {
            SettingsDictionary =  new Dictionary<string,string>(settingsDictionary);
        }

        public void SaveFieldSettingsToXmlNode(XmlNode parentNode)
        {
            foreach (KeyValuePair<string, string> kvPair in SettingsDictionary)
            {
                XmlElement xNode = parentNode.OwnerDocument.CreateElement("field");
                xNode.SetAttribute("name", kvPair.Key);
                xNode.SetAttribute("value", kvPair.Value);
                parentNode.AppendChild(xNode);
            }
        }
    }

    public class EasyActionProgressEventArgs : EventArgs
    {
        public int MaximumItems;
        public int CurrentIndex;
        public EasyActionProgressEventArgs(int maximumItems, int currentIndex)
        {
            MaximumItems = maximumItems;
            CurrentIndex = currentIndex;
        }
    }

}
