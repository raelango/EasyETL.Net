using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace EasyXml.XsltExtensions
{

    public class EasyXsltContextFunctions : IXsltContextFunction
    {
        // The data types of the arguments passed to XPath extension function.
        private System.Xml.XPath.XPathResultType[] argTypes;
        // The minimum number of arguments that can be passed to function.
        private int minArgs;
        // The maximum number of arguments that can be passed to function.
        private int maxArgs;
        // The data type returned by extension function.
        private System.Xml.XPath.XPathResultType returnType;
        // The name of the extension function.
        private string FunctionName;

        // Constructor used in the ResolveFunction method of the custom XsltContext 
        // class to return an instance of IXsltContextFunction at run time.
        public EasyXsltContextFunctions(int minArgs, int maxArgs,
            XPathResultType returnType, XPathResultType[] argTypes, string functionName)
        {
            this.minArgs = minArgs;
            this.maxArgs = maxArgs;
            this.returnType = returnType;
            this.argTypes = argTypes;
            this.FunctionName = functionName;
        }

        // Readonly property methods to access private fields.
        public System.Xml.XPath.XPathResultType[] ArgTypes
        {
            get
            {
                return argTypes;
            }
        }
        public int Maxargs
        {
            get
            {
                return maxArgs;
            }
        }

        public int Minargs
        {
            get
            {
                return maxArgs;
            }
        }

        public XPathResultType ReturnType
        {
            get
            {
                return returnType;
            }
        }

        // XPath extension functions.

        private int CountChar(XPathNodeIterator node, char charToCount)
        {
            int charCount = 0;
            for (int charIdx = 0; charIdx < node.Current.Value.Length; charIdx++)
            {
                if (node.Current.Value[charIdx] == charToCount)
                {
                    charCount++;
                }
            }
            return charCount;
        }

        // This overload will not force the user 
        // to cast to string in the xpath expression
        private string FindTaskBy(XPathNodeIterator node, string text)
        {
            if (node.Current.Value.Contains(text))
                return node.Current.Value;
            else
                return "";
        }

        private string Left(string str, int length)
        {
            return str.Substring(0, length);
        }

        private string Right(string str, int length)
        {
            return str.Substring((str.Length - length), length);
        }

        // Function to execute a specified user-defined XPath extension 
        // function at run time.
        public object Invoke(XsltContext xsltContext,
                       object[] args, XPathNavigator docContext)
        {
            string strToProcess = args[0].ToString();
            int intChars = 0;
            switch (FunctionName.ToUpper())
            {
                case "LEFT":
                    intChars = Convert.ToInt16(args[1]);
                    return (Object)(strToProcess.Substring(0, intChars));
                case "RIGHT":
                    intChars = Convert.ToInt16(args[1]);
                    return (Object)(strToProcess.Substring(strToProcess.Length - intChars));
                case "UPPER":
                    return (Object)(strToProcess.ToUpper());
                case "LOWER":
                    return (Object)(strToProcess.ToLower());
            }
            return null;
        }
    }
}
