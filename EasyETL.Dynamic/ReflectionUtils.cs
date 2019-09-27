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
using EasyETL.Attributes;

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
        public Assembly Assembly;
        public Dictionary<string, string> Fields;
    }

    
    public class ReflectionUtils
    {

        public static string[] LoadAllLibrariesWithClass(Type baseClassType, string libraryPath, bool includeSubFolders = false)
        {
            List<string> lstLibraries = new List<string>();
            if (String.IsNullOrWhiteSpace(libraryPath))
            {
                //libraryPath = Environment.CurrentDirectory;

            }
            else
            {
                if (!Directory.Exists(libraryPath)) return lstLibraries.ToArray();
                CompileClassFilesToLibrary(libraryPath, includeSubFolders);
                foreach (string dllFileName in Directory.GetFiles(libraryPath, "*.dll", includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
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
            }
            return lstLibraries.ToArray() ;
        }

        public static ClassMapping[] LoadClassesFromLibrary(Type baseClassType, string dllFileName = "")
        {
            List<ClassMapping> lstClasses = new List<ClassMapping>();

            if (String.IsNullOrWhiteSpace(dllFileName))
            {
                //libraryPath = Environment.CurrentDirectory;
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        foreach (Type type in asm.GetTypes())
                        {
                            if ((baseClassType.IsAssignableFrom(type)) && (!type.IsAbstract)) AddClassToList(lstClasses, type);
                        }
                    }
                    catch
                    {

                    }
                }
                foreach (string dllFile in Directory.GetFiles(Environment.CurrentDirectory, "*.dll", SearchOption.AllDirectories))
                {
                    if (!AppDomain.CurrentDomain.GetAssemblies().Select(f=>f.FullName).Contains(Assembly.LoadFile(dllFile).FullName))
                        lstClasses.AddRange(LoadClassesFromLibrary(baseClassType, dllFile));
                }
            }
            else
            {
                if (File.Exists(dllFileName))
                {
                    Assembly asm = Assembly.LoadFile(dllFileName);
                    foreach (Type type in asm.GetTypes())
                    {
                        if ((baseClassType.IsAssignableFrom(type)) && (!type.IsAbstract)) AddClassToList(lstClasses, type);
                    }
                }
            }

            return lstClasses.ToArray();
        }

        private static void AddClassToList(List<ClassMapping> lstClasses, Type type)
        {
            ClassMapping cMapping = new ClassMapping();
            cMapping.Class = type;
            cMapping.DisplayName = type.GetDisplayName();
            cMapping.Description = type.GetDescription();
            cMapping.Fields = type.GetEasyProperties();
            cMapping.Assembly = type.Assembly;
            lstClasses.Add(cMapping);
        }

        public static ClassMapping LoadClassFromLibrary(string dllFileName, Type baseClassType, string typeName)
        {
            ClassMapping classMapping = null;
            if (!File.Exists(dllFileName)) return classMapping;
            try
            {
                Assembly asm = Assembly.ReflectionOnlyLoad(dllFileName);
                foreach (Type type in asm.GetTypes())
                {
                    if ((baseClassType.IsAssignableFrom(type)) && (!type.IsAbstract))
                    {
                        if (typeName == type.GetDisplayName())
                        {
                            classMapping = new ClassMapping();
                            classMapping.Class = type;
                            classMapping.DisplayName = type.GetDisplayName();
                            classMapping.Description = type.GetDescription();
                            classMapping.Fields = type.GetEasyProperties();
                            return classMapping;
                        }
                    }
                }
            }
            catch
            {

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

        private static void CompileClassFilesToLibrary(string dllPath, bool includeSubFolders = false)
        {
            foreach (string csFileName in Directory.GetFiles(dllPath, "*.cs",includeSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
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
