using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace EasyXml.XsltExtensions
{
    public class EasyXsltContext : XsltContext
    {
        public string ExtensionsNamespaceUri = "http://EasyExtensions";
        // XsltArgumentList to store names and values of user-defined variables.
        private XsltArgumentList argList;

        public EasyXsltContext()
        {

        }

        public EasyXsltContext(NameTable nt, XsltArgumentList args) : base(nt)
            {
                argList = args;
            }

        public override int CompareDocument(string baseUri, string nextbaseUri)
        {
            return 0;
        }

        public override bool PreserveWhitespace(XPathNavigator node)
        {
            return false;
        }

        public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes)
        {
            // Verify namespace of function.
            if (this.LookupNamespace(prefix) == ExtensionsNamespaceUri)
            {
                string strCase = name.ToUpper();

                switch (strCase)
                {
                    case "UPPER":
                    case "LOWER":
                        return new EasyXsltContextFunctions(1, 1, XPathResultType.String, ArgTypes, name);
                    case "RIGHT": 
                    case "LEFT": 
                        return new EasyXsltContextFunctions(2, 2, XPathResultType.String, ArgTypes, name);
                }
            }
            // Return null if none of the functions match name.
            return null;
        }

        public override IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            if (this.LookupNamespace(prefix) == ExtensionsNamespaceUri || !prefix.Equals(string.Empty))
            {
                throw new XPathException(string.Format("Variable '{0}:{1}' is not defined.", prefix, name));
            }
            List<string> lstAvailableFunctions = new List<string>(new string[] { "upper", "lower", "right", "left" });
            // Verify name of function is defined.
            if (lstAvailableFunctions.Contains(name,StringComparer.CurrentCultureIgnoreCase))
            {
                EasyXsltContextVariable var;
                var = new EasyXsltContextVariable(prefix, name);

                // The Evaluate method of the returned object will be used at run time
                // to resolve the user-defined variable that is referenced in the XPath
                // query expression. 
                return var;
            }
            return null;
        }

        public override bool Whitespace
        {
            get { return true; }
        }

        // The XsltArgumentList property is accessed by the Evaluate method of the 
        // XPathExtensionVariable object that the ResolveVariable method returns. It is used 
        // to resolve references to user-defined variables in XPath query expressions. 
        public XsltArgumentList ArgList
        {
            get
            {
                return argList;
            }
        }


    }
}
