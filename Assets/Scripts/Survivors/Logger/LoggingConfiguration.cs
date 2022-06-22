using System;
using System.Xml;
using UnityEngine;
using Logger.Assets.Scripts;

namespace Survivors.Logger
{
    public static class LoggingConfiguration
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            LoggerConfigurator.Configure(LoadLocalConfig());
        }

        private static XmlDocument LoadLocalConfig()
        {
            try {
                TextAsset localConfigData = Resources.Load<TextAsset>("Log/log4net");
                if (localConfigData == null) {
                    Debug.LogError("Not found local config! Path=" );
                    return null;
                }

                XmlDocument localConfigXml = new XmlDocument();
                localConfigXml.LoadXml(localConfigData.text);
                return localConfigXml;
            } catch (Exception e) {
                Debug.LogError("Load local config exception");
                return null;
            }
        }
    }
}