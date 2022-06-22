using System.IO;
using System.Xml;
using log4net.Config;
using UnityEngine;

namespace Logger.Assets.Scripts
{
    public static class LoggerConfigurator
    {
        public static LoggerType ActiveLogger { get; set; }
        
        public static bool Configure(LoggerType loggerType)
        {
            switch (loggerType) {
                case LoggerType.Log4Net:
                    return ConfigureLogNet();
                default:
                    return false;
            }
        }
        public static bool ConfigureLogNet()
        {
            XmlConfigurator.Configure(new FileInfo($"{Application.dataPath}/log4net.xml"));
            return true;
            /*XmlNode log4net = xml.GetElementsByTagName("log4net")[0];
            if (log4net == null) {
                return false;
            }
            if (string.IsNullOrEmpty(log4net.InnerXml)) {
                return false;
            }
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode newElement = xmlDocument.CreateNode(XmlNodeType.Element, log4net.Name, "");
            newElement.InnerXml = log4net.InnerXml;
            xmlDocument.AppendChild(newElement);
            XmlConfigurator.Configure(xmlDocument.DocumentElement);
            ActiveLogger = LoggerType.LOG4NET;
            return true;*/
        }
        
    }
}