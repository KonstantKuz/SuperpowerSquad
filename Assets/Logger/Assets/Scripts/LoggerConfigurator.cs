using System;
using System.Xml;
using JetBrains.Annotations;
using log4net.Config;
using UnityEngine;

namespace Logger.Assets.Scripts
{
    public static class LoggerConfigurator
    {
        public static LoggerType ActiveLogger { get; set; }
        
        public static bool Configure(string resourcesConfigPath)
        {
            return Configure(LoadConfig(resourcesConfigPath));
        } 
        
        public static bool Configure([CanBeNull] XmlDocument xml)
        {
            if (xml == null) {
                Debug.LogError("Logger configuration error! Config is null");
                return false;
            }
            var activeLogger = GetActiveLogger(xml);
            switch (activeLogger) {
                case LoggerType.Log4Net:
                    return ConfigureLog4Net(xml);
                case LoggerType.Unity:
                    ActiveLogger = LoggerType.Unity;
                    return true;
                default:
                    return false;
            }
        }
        private static LoggerType GetActiveLogger(XmlDocument xml)
        {
            string activeLogger = xml.GetElementsByTagName("activeLogger")[0].InnerText;
            var loggerType = LoggerType.Unity;
            try {
                loggerType = (LoggerType) Enum.Parse(typeof(LoggerType), activeLogger, true);
            } catch (Exception ex) {
                Debug.LogError($"Active logger parsing error, xml active logger:= {activeLogger}, Setted logger type:= {LoggerType.Unity.ToString()}, {ex}");
            }
            return loggerType;
        }
        private static XmlDocument LoadConfig(string path)
        {
            try {
                var configData = Resources.Load<TextAsset>(path);
                if (configData == null) {
                    Debug.LogError($"Not found logger config! Config path= {path}");
                    return null;
                }
                var configXml = new XmlDocument();
                configXml.LoadXml(configData.text);
                return configXml;
            } catch (Exception ex) {
                Debug.LogError($"Load logger config exception, {ex}");
                return null;
            }
        }
        private static bool ConfigureLog4Net(XmlDocument xml)
        {
            var log4NetXml = xml.GetElementsByTagName("log4net")[0];
            if (log4NetXml == null) {
                return false;
            }
            if (string.IsNullOrEmpty(log4NetXml.InnerXml)) {
                return false;
            }
            var xmlDocument = new XmlDocument();
            var newElement = xmlDocument.CreateNode(XmlNodeType.Element, log4NetXml.Name, "");
            newElement.InnerXml = log4NetXml.InnerXml;
            xmlDocument.AppendChild(newElement);
            XmlConfigurator.Configure(xmlDocument.DocumentElement);
            ActiveLogger = LoggerType.Log4Net;
            return true;
        }
    }
}