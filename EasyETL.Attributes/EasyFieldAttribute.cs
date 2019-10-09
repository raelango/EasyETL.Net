using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Attributes
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class EasyFieldAttribute : Attribute
    {
        public string FieldName;
        public string FieldDescription;
        public string DefaultValue;
        public string RegexMatch;
        public List<string> PossibleValues;
        public bool IsPassword = false;
        public EasyFieldAttribute(string fieldName, string fieldDescription = "", string defaultValue = "", string regexMatch = "", string possibleValues = "", bool isPassword = false)
        {
            FieldName = fieldName;
            FieldDescription = String.IsNullOrWhiteSpace(fieldDescription) ? fieldName:fieldDescription;
            DefaultValue = defaultValue;
            RegexMatch = regexMatch;
            PossibleValues = new List<string>();
            if (!String.IsNullOrWhiteSpace(possibleValues)) PossibleValues = possibleValues.Split(';').ToList();
            IsPassword = isPassword;
        }

    }
}
