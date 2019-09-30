using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyETL.Endpoint.Quickbooks
{

    public enum QuickbooksItemType
    {
        Company,
        Account,
        Deposit,
        Bill,
        Barcode,
        Item,
        Inventory,
        Invoice,
        InvoicePayment,
        BillPayment,
        Customer,
        Employee

    }

    public static class QBInputRequests
    {
        public static string GetItemByID(string itemID)
        {
            XmlDocument inputDoc = GetQBXMLDocument();
            XmlNode qbXMLMsgsRq = inputDoc.SelectSingleNode("//QBXML/QBXMLMsgsRq");

            XmlElement itemRq = inputDoc.CreateElement("ItemInventoryQueryRq");
            itemRq.SetAttribute("requestID", itemID);
            qbXMLMsgsRq.AppendChild(itemRq);

            return inputDoc.OuterXml;
        }


        public static string GetAllItems(QuickbooksItemType itemType = QuickbooksItemType.Item)
        {
            string strItemType = "";
            switch (itemType)
            {
                case QuickbooksItemType.Item:
                    strItemType = "ItemQueryRq"; break;
                case QuickbooksItemType.Invoice:
                    strItemType = "InvoiceQueryRq"; break;
                case QuickbooksItemType.Company:
                    strItemType = "CompanyQueryRq"; break;
            }

            if (!String.IsNullOrWhiteSpace(strItemType))
            {
                return GetQBXMLDocument(strItemType, "requestID=1").OuterXml;
            }
            return String.Empty;
        }

        public static XmlDocument GetQBXMLDocument(string requestType = "", params string[] nameValues)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (string nameValue in nameValues)
            {
                string strKVPair = nameValue.Trim(' ', '=');
                if (strKVPair.Contains('='))
                {
                    keyValuePairs.Add(strKVPair.Split('=')[0], strKVPair.Split('=')[1]);
                }
            }
            return GetQBXMLDocument(requestType, keyValuePairs);
        }

        public static XmlDocument GetQBXMLDocument(string requestType, Dictionary<string, string> keyValuePairs)
        {
            XmlDocument inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"2.0\""));
            XmlElement qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            if (!String.IsNullOrWhiteSpace(requestType))
            {
                XmlElement requestElement = inputXMLDoc.CreateElement(requestType);
                foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
                {
                    requestElement.SetAttribute(keyValuePair.Key, keyValuePair.Value);
                }
                qbXMLMsgsRq.AppendChild(requestElement);
            }
            return inputXMLDoc;
        }
    }

}
