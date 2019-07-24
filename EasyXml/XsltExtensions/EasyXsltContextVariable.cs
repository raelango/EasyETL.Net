using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace EasyXml.XsltExtensions
{
    public class EasyXsltContextVariable : IXsltContextVariable
    {
        // Namespace of user-defined variable.
        private string prefix;
        // The name of the user-defined variable.
        private string varName;

        // Constructor used in the overridden ResolveVariable function of custom XsltContext.
        public EasyXsltContextVariable(string prefix, string varName)
        {
            this.prefix = prefix;
            this.varName = varName;
        }

        // Function to return the value of the specified user-defined variable.
        // The GetParam method of the XsltArgumentList property of the active
        // XsltContext object returns value assigned to the specified variable.
        public object Evaluate(XsltContext xsltContext)
        {
            XsltArgumentList vars = ((EasyXsltContext)xsltContext).ArgList;
            return vars.GetParam(varName, prefix);
        }

        // Determines whether this variable is a local XSLT variable.
        // Needed only when using a style sheet.
        public bool IsLocal
        {
            get
            {
                return false;
            }
        }

        // Determines whether this parameter is an XSLT parameter.
        // Needed only when using a style sheet.
        public bool IsParam
        {
            get
            {
                return false;
            }
        }

        public System.Xml.XPath.XPathResultType VariableType
        {
            get
            {
                return XPathResultType.Any;
            }
        }
    }
}
