using System;
using System.IO;
using Feofun.Config.Serializers;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using Zenject;
using ILogger = Logger.ILogger;

namespace Feofun.Config
{
    public class ConfigLoader
    {
        private static string MAIN_PATH = "Configs";
        
        private readonly DiContainer _container;
        private readonly IConfigDeserializer _deserializer;
        [CanBeNull]
        private readonly string _configOverrideFolder;

        public ConfigLoader(DiContainer container, IConfigDeserializer deserializer, string configOverrideFolder = null)
        {
            _container = container;
            _deserializer = deserializer;
            _configOverrideFolder = configOverrideFolder;
        }

        public ConfigLoader RegisterStringKeyedCollection<TValue>(string configName, bool withId = false, bool optional = false)
                where TValue : ICollectionItem<string>
        {
            return RegisterSingle<StringKeyedConfigCollection<TValue>>(configName, withId, optional);
        }  
        
        public ConfigLoader RegisterSingleObjectConfig<TValue>(string configName, bool withId = false, bool optional = false)
        {
            return Register<SingleObjectConfig<TValue>, TValue>(configName, withId, optional,
                config => config.Value);
        }
        public ConfigLoader RegisterCollection<TKey, TValue>(string configName, bool withId = false, bool optional = false)
                where TValue : ICollectionItem<TKey>
        {
            return RegisterSingle<ConfigCollection<TKey, TValue>>(configName, withId, optional);
        }

        public ConfigLoader RegisterSingle<T>(string configName, bool withId = false, bool optional = false)
            where T : ILoadableConfig
        {
            return Register<T, T>(configName, withId, optional, config => config);
        }
        
        private ConfigLoader Register<TLoadableConfig, TValue>(string configName, bool withId, bool optional, Func<TLoadableConfig, TValue> GetValueFunc)
            where TLoadableConfig : ILoadableConfig
        {
            try {
                var configText = FindConfigText(configName);
                if (configText == null && optional) {
                    return this;
                }
                if (configText == null) {
                    throw new NullReferenceException($"Config:={configName} not found on path:= {MAIN_PATH}/{configName}");
                }
                var config = _deserializer.Deserialize<TLoadableConfig>(configText);
                var binder = _container.Bind(typeof(TValue));
                var value = GetValueFunc.Invoke(config);
                if (withId) {
                    binder.WithId(configName).FromInstance(value);
                }
                else {
                    binder.FromInstance(value).AsSingle();    
                }
                return this;
            }
            catch (Exception)
            {
                this.Logger().Error($"Failed to parse config {configName}");
                throw;
            }
        }
        [CanBeNull]
        private string FindConfigText(string configName)
        {
            TextAsset configAsset = null;
            if (!_configOverrideFolder.IsNullOrEmpty())
            {
                configAsset = Resources.Load<TextAsset>(Path.Combine(_configOverrideFolder, configName));
            }
            if (configAsset == null)
            {
                configAsset = Resources.Load<TextAsset>(Path.Combine(MAIN_PATH, configName));
            }
            return configAsset != null ? configAsset.text : null;
        }

        public static T LoadConfig<T>(string path, IConfigDeserializer deserializer) where T:ILoadableConfig
        {
            var textAsset = Resources.Load<TextAsset>(path);
            return deserializer.Deserialize<T>(textAsset.text);
        }
    }
}