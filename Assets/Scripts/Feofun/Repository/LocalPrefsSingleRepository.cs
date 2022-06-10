using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Feofun.Repository
{
    public class LocalPrefsSingleRepository<T> : ISingleModelRepository<T>
            where T : class
    {
        private readonly string _key;
        private readonly JsonSerializerSettings _serializerSettings;

        private T _cache;

        public LocalPrefsSingleRepository(string key, JsonSerializerSettings serializerSettings = null)
        {
            _key = key;
            _serializerSettings = serializerSettings ?? CreateDefaultSerializeSettings();
        }

        public virtual T Get()
        {
            if (_cache != null ) {
                return _cache;
            }
            string info = PlayerPrefs.GetString(_key);
            if (string.IsNullOrEmpty(info)) {
                return null;
            }

            try
            {
                _cache = JsonConvert.DeserializeObject<T>(info, _serializerSettings);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse type {typeof(T).FullName} from {info}");
                Debug.LogException(e);
                throw;
            }

            return _cache;
        }

        public T Require()
        {
            T value = Get();
            if (value == null) {
                throw new NullReferenceException("Object  " + typeof(T) + " not found");
            }
            return value;
        }

        public bool Exists()
        {
            return Get() != null;
        }

        public virtual void Set(T model)
        {
            _cache = model;
            PlayerPrefs.SetString(_key, JsonConvert.SerializeObject(model, _serializerSettings));
            PlayerPrefs.Save();
        }

        public void Delete()
        {
            _cache = null;
            PlayerPrefs.DeleteKey(_key);
        }

        private JsonSerializerSettings CreateDefaultSerializeSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings {
                    ContractResolver = new DefaultContractResolver()
            };
            return settings;
        }
    }
}