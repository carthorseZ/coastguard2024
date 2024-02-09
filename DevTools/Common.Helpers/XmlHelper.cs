namespace DevTools.Helpers
{
    using System;
    using System.Xml;

    public static class XmlHelper
    {
        public static T GetNodeValue<T>(XmlNode node, string xpath, T defaultValue)
        {
            return default(T);
        }

        public static XmlDocument GetXmlFromString(string xml)
        {
            XmlDocument document = null;
            if (!string.IsNullOrEmpty(xml))
            {
                document = new XmlDocument();
                document.LoadXml(xml);
            }
            return document;
        }
    }
}

