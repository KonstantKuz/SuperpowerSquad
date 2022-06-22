using System;
using System.Xml;
using log4net.Config;

namespace Logger.Assets.Scripts
{
    public static class LoggerConfigurator
    {
        public static LoggerType ActiveLogger { get; set; }
        
        public static bool Configure(XmlDocument xml)
        {
            string activeLogger = xml.GetElementsByTagName("activeLogger")[0].InnerText;
            var loggerType = (LoggerType) Enum.Parse(typeof(LoggerType), activeLogger, true);
            return loggerType switch {
                    LoggerType.Log4Net => ConfigureLog4Net(xml),
                    LoggerType.Unity => ConfigureUnityLogger(),
                    _ => false
            };
        }
        private static bool ConfigureUnityLogger()
        {
            ActiveLogger = LoggerType.Unity;
            return true;
        }
        private static bool ConfigureLog4Net(XmlDocument xml)
        {
            var log4net = xml.GetElementsByTagName("log4net")[0];
            if (log4net == null) {
                return false;
            }
            if (string.IsNullOrEmpty(log4net.InnerXml)) {
                return false;
            }
            var xmlDocument = new XmlDocument();
            var newElement = xmlDocument.CreateNode(XmlNodeType.Element, log4net.Name, "");
            newElement.InnerXml = log4net.InnerXml;
            xmlDocument.AppendChild(newElement);
            XmlConfigurator.Configure(xmlDocument.DocumentElement);
            ActiveLogger = LoggerType.Log4Net;
            return true;
        }
        
      
    }
}