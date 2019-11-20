using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EasyETL.Utils
{
    public static class DynamicCode
    {
        [STAThread]
        public static string EvaluateString(string strToEvaluate, Dictionary<string, object> props = null)
        {
            try
            {
                using (CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    CompilerParameters cp = new CompilerParameters();

                    string className = "Class" + System.Guid.NewGuid().ToString().Replace("-", "");

                    cp.ReferencedAssemblies.Add("system.dll");
                    cp.CompilerOptions = "/t:library";
                    cp.GenerateInMemory = true;

                    StringBuilder sb = new StringBuilder("");
                    sb.Append("using System;\n");
                    sb.Append("using System.Collections.Generic;\n");

                    sb.Append("namespace EasyObject.Evaluator { \n");
                    sb.Append("\tpublic class " + className + "{ \n");
                    if (props != null)
                    {
                        foreach (KeyValuePair<string, object> kvPair in props)
                        {
                            sb.AppendFormat("\t\t{0} {1};\n", kvPair.Value.GetType().Name, kvPair.Key);
                        }
                    }
                    sb.Append("\t\tpublic object Evaluate(object props = null){\n");
                    if (props != null)
                    {
                        sb.Append("\t\t\tif ((props !=null) && (props is Dictionary<string,object>)){\n");

                        foreach (KeyValuePair<string, object> kvPair in props)
                        {
                            sb.AppendFormat("\t\t\t\t{1} = ({0})((Dictionary<string,object>)props)[\"{1}\"]; \n", kvPair.Value.GetType().Name, kvPair.Key);
                        }
                        sb.Append("\t\t\t}\n");
                    }
                    sb.Append("\t\t\treturn " + strToEvaluate + "; \n");
                    sb.Append("\t\t} \n");
                    sb.Append("\t} \n");
                    sb.Append("}\n");

                    CompilerResults cr = codeProvider.CompileAssemblyFromSource(cp, sb.ToString());
                    if (cr.Errors.Count > 0)
                    {
                        return strToEvaluate;
                    }

                    System.Reflection.Assembly a = cr.CompiledAssembly;
                    object o = a.CreateInstance("EasyObject.Evaluator." + className);
                    Type t = o.GetType();
                    MethodInfo mi = t.GetMethod("Evaluate");

                    object s = String.Empty;
                    s = mi.Invoke(o, new object[] { props });
                    return s.ToString();
                }
            }
            catch
            {
                return strToEvaluate;
            }
        }
    }
}
