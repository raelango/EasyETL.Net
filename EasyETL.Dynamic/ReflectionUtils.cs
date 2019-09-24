using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyETL;
using System.ComponentModel;

namespace EasyETL
{

    public class TypeClassMapping
    {
        public Type Class;
        public MethodInfo Method;
    }

    public class ClassMapping
    {
        public Type Class;
        public string DisplayName;
        public string Description;
        public Dictionary<string, string> Fields;
    }

    
    public class ReflectionUtils
    {

        public static string[] LoadAllLibrariesWithClass(string libraryPath, Type baseClassType)
        {
            List<string> lstLibraries = new List<string>();
            if (!Directory.Exists(libraryPath)) return lstLibraries.ToArray();
            CompileClassFilesToLibrary(libraryPath);
            foreach (string dllFileName in Directory.GetFiles(libraryPath, "*.dll"))
            {
                Assembly asm = Assembly.LoadFile(dllFileName);
                foreach (Type type in asm.GetTypes())
                {
                    if ((baseClassType.IsAssignableFrom(type)) && (!type.IsAbstract))
                    {
                        lstLibraries.Add(Path.GetFileNameWithoutExtension(dllFileName));
                        break;
                    }
                }
            }
            return lstLibraries.ToArray() ;
        }

        public static ClassMapping[] LoadClassesFromLibrary(string dllFileName, Type baseClassType)
        {
            List<ClassMapping> lstClasses = new List<ClassMapping>();
            if (!File.Exists(dllFileName)) return lstClasses.ToArray();
            Assembly asm = Assembly.LoadFile(dllFileName);
            foreach (Type type in asm.GetTypes())
            {
                if ((baseClassType.IsAssignableFrom(type)) && (!type.IsAbstract))
                {
                    string displayName = type.Name;
                    DisplayNameAttribute displayAttr = (DisplayNameAttribute)type.GetCustomAttribute(typeof(DisplayNameAttribute),true);
                    if (displayAttr !=null) displayName = displayAttr.DisplayName;
                    ClassMapping cMapping = new ClassMapping();
                    cMapping.DisplayName = displayName;
                    cMapping.Description = ((DescriptionAttribute)type.GetCustomAttribute(typeof(DescriptionAttribute), true)).Description;
                    cMapping.Class = type;
                    cMapping.Fields = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
                    lstClasses.Add(cMapping);
                }
            }
            return lstClasses.ToArray();
        }

        public static ClassMapping LoadClassFromLibrary(string dllFileName, Type baseClassType, string typeName)
        {
            ClassMapping classMapping = null;
            if (!File.Exists(dllFileName)) return classMapping;
            Assembly asm = Assembly.LoadFile(dllFileName);
            foreach (Type type in asm.GetTypes())
            {
                if ((baseClassType.IsAssignableFrom(type)) && (!type.IsAbstract))
                {
                    string displayName = type.Name;
                    DisplayNameAttribute displayAttr = (DisplayNameAttribute)type.GetCustomAttribute(typeof(DisplayNameAttribute), true);
                    if (displayAttr != null) displayName = displayAttr.DisplayName;
                    if (displayName == typeName)
                    {
                        classMapping = new ClassMapping();
                        classMapping.DisplayName = displayName;
                        classMapping.Description = ((DescriptionAttribute)type.GetCustomAttribute(typeof(DescriptionAttribute), true)).Description;
                        classMapping.Class = type;
                        return classMapping;
                    }
                }
            }
            return classMapping;
        }

        public static Dictionary<string,TypeClassMapping> ExtractMethodsFromFile(string dllPath)
        {
            Dictionary<string, TypeClassMapping> Methods = new Dictionary<string, TypeClassMapping>(StringComparer.CurrentCultureIgnoreCase);
            if (!Directory.Exists(dllPath)) return Methods;
            CompileClassFilesToLibrary(dllPath);

            foreach (string dllFileName in Directory.GetFiles(dllPath, "*.dll"))
            {
                Assembly asm = Assembly.LoadFile(dllFileName);
                foreach (Type type in asm.GetTypes())
                {
                    if (type.IsClass || (type.IsAbstract && type.IsSealed))
                    {
                        foreach (MethodInfo mi in type.GetMethods())
                        {
                            if (!mi.IsAbstract && !mi.IsConstructor && mi.IsPublic && (mi.DeclaringType == type))
                            {
                                if (!Methods.ContainsKey(mi.Name))
                                {
                                    Methods.Add(mi.Name, new TypeClassMapping() { Class = type, Method = mi });
                                }
                                Methods.Add(type.Name + "_" + mi.Name, new TypeClassMapping() { Class = type, Method = mi });
                            }
                        }
                    }
                }
            }
            return Methods;
        }

        private static void CompileClassFilesToLibrary(string dllPath)
        {
            foreach (string csFileName in Directory.GetFiles(dllPath, "*.cs"))
            {
                CompileCSSource(csFileName, Path.ChangeExtension(csFileName, "dll"));
            }
        }

        public static void CompileCSSource(string csFileName, string dllFileName)
        {
            string errorFileName = Path.ChangeExtension(csFileName, "error");
            if (File.Exists(dllFileName)) File.Delete(dllFileName);
            if (File.Exists(errorFileName)) File.Delete(errorFileName);
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameters = new CompilerParameters();
            parameters.OutputAssembly = dllFileName;
            var assemblies = AppDomain.CurrentDomain
                                        .GetAssemblies()
                                        .Where(a => !a.IsDynamic)
                                        .Select(a => a.Location);

            parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());
            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, csFileName);
            if (results.Errors.Count > 0)
            {
                using (StreamWriter sw = new StreamWriter(errorFileName, false))
                {
                    foreach (CompilerError cError in results.Errors)
                    {
                        sw.WriteLine(String.Format("Line Number: {0}, Error Number: {1}, Error: {2}", cError.Line, cError.ErrorNumber, cError.ErrorText));
                    }
                }
            }
        }


    }
}
