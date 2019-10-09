using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Attributes
{
    public static class AttributeExtensions
    {

        public static string GetDisplayName(this Type classType)
        {
            object[] objArray  = classType.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if ((objArray == null) || (objArray.Length ==0)) objArray = classType.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            if ((objArray == null) || (objArray.Length == 0) || (!(objArray[0] is DisplayNameAttribute))) return classType.Name;
            return ((DisplayNameAttribute)objArray[0]).DisplayName;
        }

        public static string GetDescription(this Type classType)
        {
            object[] objArray = classType.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((objArray == null) || (objArray.Length == 0) || (!(objArray[0] is DescriptionAttribute))) return classType.Name;
            return ((DescriptionAttribute)objArray[0]).Description;
        }

        public static Dictionary<string, string> GetEasyProperties(this object obj)
        {
            return obj.GetType().GetEasyProperties();
        }

        public static Dictionary<string, string> GetEasyProperties(this Type classType)
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            foreach (object attr in classType.GetCustomAttributes(typeof(EasyPropertyAttribute), false))
            {
                resultDict.Add(((EasyPropertyAttribute)attr).FieldName, ((EasyPropertyAttribute)attr).FieldValue);
            }

            foreach (object attr in classType.GetCustomAttributes(typeof(EasyPropertyAttribute), true))
            {
                if (!resultDict.ContainsKey(((EasyPropertyAttribute)attr).FieldName)) resultDict.Add(((EasyPropertyAttribute)attr).FieldName, ((EasyPropertyAttribute)attr).FieldValue);
            }
            return resultDict;
        }

        public static string GetEasyProperty(this object obj, string propertyName, string defaultValue = "")
        {
            return obj.GetType().GetEasyProperty(propertyName, defaultValue);
        }

        public static string GetEasyProperty(this Type classType, string propertyName, string defaultValue = "")
        {
            foreach (object obj in classType.GetCustomAttributes(typeof(EasyPropertyAttribute), false))
            {
                EasyPropertyAttribute epAttribute = (EasyPropertyAttribute)obj;
                if (epAttribute.FieldName.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase)) return epAttribute.FieldValue;
            }
            foreach (object obj in classType.GetCustomAttributes(typeof(EasyPropertyAttribute), true))
            {
                EasyPropertyAttribute epAttribute = (EasyPropertyAttribute)obj;
                if (epAttribute.FieldName.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase)) return epAttribute.FieldValue;
            }
            return defaultValue;
        }

        public static Dictionary<string, EasyFieldAttribute> GetEasyFields(this object obj)
        {
            return obj.GetType().GetEasyFields();
        }

        public static Dictionary<string, EasyFieldAttribute> GetEasyFields(this Type classType)
        {
            Dictionary<string, EasyFieldAttribute> resultDict = new Dictionary<string, EasyFieldAttribute>(StringComparer.CurrentCultureIgnoreCase);
            foreach (object attr in classType.GetCustomAttributes(typeof(EasyFieldAttribute), false))
            {
                resultDict.Add(((EasyFieldAttribute)attr).FieldName, (EasyFieldAttribute)attr);
            }

            foreach (object attr in classType.GetCustomAttributes(typeof(EasyFieldAttribute), false))
            {
                if (!resultDict.ContainsKey(((EasyFieldAttribute)attr).FieldName)) resultDict.Add(((EasyFieldAttribute)attr).FieldName, (EasyFieldAttribute)attr);
            }
            return resultDict;
        }

        public static EasyFieldAttribute GetEasyField(this Type classType, string fieldName)
        {
            foreach (object obj in classType.GetCustomAttributes(typeof(EasyFieldAttribute), false))
            {
                EasyFieldAttribute efAttribute = (EasyFieldAttribute)obj;
                if (efAttribute.FieldName.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)) return efAttribute;
            }
            foreach (object obj in classType.GetCustomAttributes(typeof(EasyFieldAttribute), true))
            {
                EasyFieldAttribute efAttribute = (EasyFieldAttribute)obj;
                if (efAttribute.FieldName.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase)) return efAttribute;
            }
            return null;
        }    

    }
}
