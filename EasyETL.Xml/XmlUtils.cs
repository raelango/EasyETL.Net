using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace EasyETL.Xml
{
    public static class XmlUtils
    {
        public static XmlNode TransformXml(this XmlNode xmlNode, params string[] settingsCommands)
        {
            if ((settingsCommands.Length == 0) || (xmlNode == null) || !xmlNode.HasChildNodes) return xmlNode;
            string rowElementName = xmlNode.Name;

            if (String.IsNullOrWhiteSpace(rowElementName)) return xmlNode;

            bool bTransformRequired = false;
            Dictionary<string, string> dctAdditions = new Dictionary<string, string>();
            List<string> lstRemoveCommands = new List<string>();
            List<string> lstFilters = new List<string>();
            Dictionary<string, string> dctRenames = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            List<string> lstRowElements = new List<string>();

            StringBuilder xslSB = new StringBuilder();
            Dictionary<string, string> dctSortOrders = new Dictionary<string, string>();
            string strSortOrder = String.Empty;
            foreach (string settingCommand in settingsCommands)
            {
                string[] settings = settingCommand.Split(' ');
                switch (settings[0].ToUpper())
                {
                    case "ADD":
                        if (settingCommand.Length > (settings[1].Length + 4))
                        {
                            dctAdditions.Add(settings[1], settingCommand.Substring(4 + settings[1].Length + 1));
                            bTransformRequired = true;
                        }
                        break;
                    case "HIDE":
                    case "REMOVE":
                        lstRemoveCommands.Add(settings[1]);
                        bTransformRequired = true;
                        break;
                    case "RENAME":
                        dctRenames.Add(settings[1], settings[2]);
                        bTransformRequired = true;
                        break;
                    case "FILTER":
                        lstFilters.Add(settingCommand.Substring(6).Trim());
                        bTransformRequired = true;
                        break;
                    case "GO":
                    case "EXECUTE":
                        if (bTransformRequired || rowElementName.Contains("=>"))
                        {
                            xmlNode.BuildXsltString(rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                            strSortOrder = String.Empty;
                            xmlNode = xmlNode.TransformXml(xslSB);
                            if ((xmlNode == null) || (!xmlNode.HasChildNodes)) return xmlNode;
                            xslSB = new StringBuilder();
                            rowElementName = xmlNode.Name;
                            bTransformRequired = false;
                        }
                        break;
                }
                if (!String.IsNullOrWhiteSpace(settingCommand.Trim()))
                {


                    if (settingCommand.Trim()[0] == '[')
                    {
                        if (bTransformRequired || rowElementName.Contains("=>"))
                        {
                            xmlNode.BuildXsltString(rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                            strSortOrder = String.Empty;
                        }
                        rowElementName = settingCommand.Trim().Substring(1, settingCommand.Trim().Length - 2);
                        bTransformRequired = false;
                    }
                }

            }

            if (bTransformRequired || rowElementName.Contains("=>"))
            {
                xmlNode.BuildXsltString(rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                xmlNode = xmlNode.TransformXml(xslSB);
            }

            return xmlNode;
        }

        public static XmlNode TransformXml(this XmlNode xmlNode, StringBuilder xslSB)
        {

            try
            {
                XslCompiledTransform xsl = GetCompiledTransform(xslSB);
                return xmlNode.TransformXml(xsl);
            }
            catch
            {
                return null;
            }
        }

        public static XmlNode TransformXml(this XmlNode xmlNode, XslCompiledTransform xsl)
        {
            try
            {
                XsltArgumentList xal = new XsltArgumentList();
                xal.AddExtensionObject(XsltExtensions.XsltStringExtensions.Namespace, new XsltExtensions.XsltStringExtensions()); //This is to make the "easy" extension functions available...
                StringBuilder xmlSB = new StringBuilder();
                XmlWriterSettings xwSettings = new XmlWriterSettings();
                xwSettings.OmitXmlDeclaration = true;
                xwSettings.ConformanceLevel = ConformanceLevel.Auto;
                XmlWriter xWriter = XmlWriter.Create(xmlSB, xwSettings);
                xsl.Transform(xmlNode, xal, xWriter);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xmlSB.ToString());
                return xDoc.DocumentElement;
            }
            catch
            {
                return null;
            }
        }

        public static XslCompiledTransform GetCompiledTransform(this XmlNode xmlNode, params string[] settingsCommands)
        {
            if ((settingsCommands.Length == 0) || (xmlNode == null) || !xmlNode.HasChildNodes) return null;
            string rowElementName = xmlNode.Name;

            if (String.IsNullOrWhiteSpace(rowElementName)) return null;

            bool bTransformRequired = false;
            Dictionary<string, string> dctAdditions = new Dictionary<string, string>();
            List<string> lstRemoveCommands = new List<string>();
            List<string> lstFilters = new List<string>();
            Dictionary<string, string> dctRenames = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            List<string> lstRowElements = new List<string>();

            StringBuilder xslSB = new StringBuilder();
            Dictionary<string, string> dctSortOrders = new Dictionary<string, string>();
            string strSortOrder = String.Empty;
            foreach (string settingCommand in settingsCommands)
            {
                string[] settings = settingCommand.Split(' ');
                switch (settings[0].ToUpper())
                {
                    case "ADD":
                        if (settingCommand.Length > (settings[1].Length + 4))
                        {
                            dctAdditions.Add(settings[1], settingCommand.Substring(4 + settings[1].Length + 1));
                            bTransformRequired = true;
                        }
                        break;
                    case "HIDE":
                    case "REMOVE":
                        lstRemoveCommands.Add(settings[1]);
                        bTransformRequired = true;
                        break;
                    case "RENAME":
                        dctRenames.Add(settings[1], settings[2]);
                        bTransformRequired = true;
                        break;
                    case "FILTER":
                        lstFilters.Add(settingCommand.Substring(6).Trim());
                        bTransformRequired = true;
                        break;
                    case "GO":
                    case "EXECUTE":
                        if (bTransformRequired || rowElementName.Contains("=>"))
                        {
                            xmlNode.BuildXsltString(rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                            strSortOrder = String.Empty;
                            xmlNode = xmlNode.TransformXml(xslSB);
                            if ((xmlNode == null) || (!xmlNode.HasChildNodes)) return null;
                            xslSB = new StringBuilder();
                            rowElementName = xmlNode.Name;
                            bTransformRequired = false;
                        }
                        break;
                }
                if (!String.IsNullOrWhiteSpace(settingCommand.Trim()))
                {


                    if (settingCommand.Trim()[0] == '[')
                    {
                        if (bTransformRequired || rowElementName.Contains("=>"))
                        {
                            xmlNode.BuildXsltString(rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
                            strSortOrder = String.Empty;
                        }
                        rowElementName = settingCommand.Trim().Substring(1, settingCommand.Trim().Length - 2);
                        bTransformRequired = false;
                    }
                }

            }

            xmlNode.BuildXsltString(rowElementName, dctAdditions, lstRemoveCommands, lstFilters, dctRenames, lstRowElements, xslSB);
            return GetCompiledTransform(xslSB);
        }

        public static XslCompiledTransform GetCompiledTransform(StringBuilder xslSB)
        {
            StringBuilder rootSB = new StringBuilder();

            rootSB.AppendLine("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            rootSB.AppendLine("<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:easy=\"http://EasyXsltExtensions/1.0\">");
            rootSB.AppendLine("<xsl:output method=\"xml\" indent=\"yes\" omit-xml-declaration=\"yes\"/>");
            rootSB.AppendLine("<xsl:template match=\"@*|node()\">");
            rootSB.AppendLine("<xsl:copy>");
            rootSB.AppendLine("<xsl:apply-templates select=\"@*|node()\">");
            rootSB.AppendLine("</xsl:apply-templates>");
            rootSB.AppendLine("</xsl:copy>");
            rootSB.AppendLine("</xsl:template>");

            rootSB.Append(xslSB.ToString());
            rootSB.AppendLine("</xsl:stylesheet>");

            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(XmlReader.Create(new StringReader(rootSB.ToString())));
            return xsl;
        }

        public static void BuildXsltString(this XmlNode xmlNode, string rowElementName, Dictionary<string, string> dctAdditions, List<string> lstRemoveCommands, List<string> lstFilters, Dictionary<string, string> dctRenames, List<string> lstRowElements, StringBuilder xslSB)
        {
            string newRowElementName = rowElementName;
            if (rowElementName.Contains("=>"))
            {
                newRowElementName = rowElementName.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[1];
                rowElementName = rowElementName.Split(new string[] { "=>" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
            if (!xmlNode.Name.Equals(rowElementName)) return;

            lstRowElements.Add(rowElementName);
            string xslString = String.Empty;

            foreach (XmlNode xNode in xmlNode.ChildNodes)
            {
                if (!lstRemoveCommands.Contains(xNode.Name, StringComparer.CurrentCultureIgnoreCase))
                {
                    string columnName = xNode.Name;
                    if (dctRenames.ContainsKey(columnName)) columnName = dctRenames[columnName];
                    xslString += String.Format("<{0}><xsl:value-of select=\"{1}\"/></{0}>", columnName, xNode.Name);
                    xslString += Environment.NewLine;
                }
            }

            foreach (KeyValuePair<string, string> kvPair in dctAdditions)
            {
                switch (kvPair.Value.Trim().ToUpper())
                {
                    case "AUTONUMBER":
                        xslString += String.Format("<{0}><xsl:number {1}/></{0}>", kvPair.Key, kvPair.Value.Substring(10));
                        xslString += Environment.NewLine;
                        break;
                    default:
                        xslString += String.Format("<{0}><xsl:value-of select=\"{1}\"/></{0}>", kvPair.Key, kvPair.Value);
                        xslString += Environment.NewLine;
                        break;
                }
            }



            if (lstFilters.Count == 0)
            {
                xslSB.AppendLine("<xsl:template match=\"" + rowElementName + "\">");
                xslSB.AppendLine("<xsl:element name=\"" + newRowElementName + "\">");
                xslSB.AppendLine(xslString);
                xslSB.AppendLine("</xsl:element>");
                xslSB.AppendLine("</xsl:template>");
            }
            else
            {
                foreach (string strFilter in lstFilters)
                {
                    xslSB.AppendLine("<xsl:template match=\"" + rowElementName + "[" + strFilter + "]\">");
                    xslSB.AppendLine("<xsl:element name=\"" + newRowElementName + "\">");
                    xslSB.Append(xslString);
                    xslSB.AppendLine("</xsl:element>");
                    xslSB.AppendLine("</xsl:template>");
                }
                xslSB.AppendLine("<xsl:template match=\"" + rowElementName + "\"/>");
            }
            dctAdditions.Clear();
            lstFilters.Clear();
            dctRenames.Clear();
        }

        public static void Encrypt(this XmlDocument Doc, string ElementToEncrypt, SymmetricAlgorithm Alg, string KeyName)
        {
            // Check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (ElementToEncrypt == null)
                throw new ArgumentNullException("ElementToEncrypt");
            if (Alg == null)
                throw new ArgumentNullException("Alg");

            ////////////////////////////////////////////////
            // Find the specified element in the XmlDocument
            // object and create a new XmlElemnt object.
            ////////////////////////////////////////////////

            XmlElement elementToEncrypt = Doc.GetElementsByTagName(ElementToEncrypt)[0] as XmlElement;

            // Throw an XmlException if the element was not found.
            if (elementToEncrypt == null)
            {
                throw new XmlException("The specified element was not found");

            }

            //////////////////////////////////////////////////
            // Create a new instance of the EncryptedXml class 
            // and use it to encrypt the XmlElement with the 
            // symmetric key.
            //////////////////////////////////////////////////

            EncryptedXml eXml = new EncryptedXml();

            // Add the key mapping.
            eXml.AddKeyNameMapping(KeyName, Alg);

            // Encrypt the element.
            EncryptedData edElement = eXml.Encrypt(elementToEncrypt, KeyName);


            ////////////////////////////////////////////////////
            // Replace the element from the original XmlDocument
            // object with the EncryptedData element.
            ////////////////////////////////////////////////////

            EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);

        }

        public static void Decrypt(this XmlDocument Doc, SymmetricAlgorithm Alg, string KeyName)
        {
            // Check the arguments.  
            if (Doc == null)
                throw new ArgumentNullException("Doc");
            if (Alg == null)
                throw new ArgumentNullException("Alg");
            if (KeyName == null)
                throw new ArgumentNullException("KeyName");

            // Create a new EncryptedXml object.
            EncryptedXml exml = new EncryptedXml(Doc);

            // Add the key name mapping.
            exml.AddKeyNameMapping(KeyName, Alg);

            // Decrypt the XML document.
            exml.DecryptDocument();

        }
    
    }
}
