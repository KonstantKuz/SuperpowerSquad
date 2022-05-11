using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Mono.Csv;
using SuperMaxim.Core.Extensions;

namespace Feofun.Config.Csv
{
    public class CsvSerializer
    {
        private readonly CultureInfo _cultureInfo;

        public CsvSerializer(CultureInfo cultureInfo = null)
        {
            _cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;
        }

        public T ReadSingleObject<T>(Stream stream)
        {
            RequireRefType<T>();

            var dict = ReadDictionary(stream);
            
            string FieldValueGetter(string fieldName) => !dict.ContainsKey(fieldName) ? null : dict[fieldName];
            return CreateObject<T>(FieldValueGetter);
        }

        public Dictionary<string, string> ReadDictionary(Stream stream)
        {
            var table = ReadTable(stream);
            var dict = new Dictionary<string, string>();
            foreach (var row in table)
            {
                dict[row[0]] = row[1];
            }

            return dict;
        }
        
        public IReadOnlyList<T> ReadObjectArray<T>(Stream stream)
        {
            RequireRefType<T>();
            
            var table = ReadTable(stream);
            var dict = ReadHeader(table);

            var rez = new List<T>();

            for (var rowIdx = 1; rowIdx < table.Count; rowIdx++)
            {
                var idx = rowIdx;
                string FieldValueGetter(string fieldName) => !dict.ContainsKey(fieldName) ? null : table[idx][dict[fieldName]];
                rez.Add(CreateObject<T>(FieldValueGetter));
            }

            return rez;
        }
        
        public IReadOnlyDictionary<string, T> ReadObjectDictionary<T>(Stream stream)
        {
            RequireRefType<T>();
            
            var table = ReadTable(stream);
            var dict = ReadHeader(table);

            var rez = new Dictionary<string, T>();
            
            for (var rowIdx = 1; rowIdx < table.Count; rowIdx++)
            {
                var key = table[rowIdx][0];
                if (key.IsNullOrEmpty()) continue;

                var idx = rowIdx;
                string FieldValueGetter(string fieldName) => !dict.ContainsKey(fieldName) ? null : table[idx][dict[fieldName]];
                rez[key] = CreateObject<T>(FieldValueGetter);
            }

            return rez;
        }
        public IReadOnlyDictionary<string, IReadOnlyDictionary<string,string>> ReadDictionaryTable(Stream stream)
        {
            var table = ReadTable(stream);
            var dict = ReadHeader(table);

            var rez = new Dictionary<string, IReadOnlyDictionary<string,string>>();
            
            for (var rowIdx = 1; rowIdx < table.Count; rowIdx++)
            {
                var key = table[rowIdx][0];
                if (key.IsNullOrEmpty()) continue;

                var idx = rowIdx;

                rez[key] = dict.Keys.ToDictionary(it => it, it => table[idx][dict[it]]);
            }

            return rez;
        }
        public Dictionary<string, IReadOnlyList<T>> ReadNestedTable<T>(Stream stream)
        {
            RequireRefType<T>();
            
            var table = ReadTable(stream);
            var dict = ReadHeader(table);
            
            var rez = new Dictionary<string, List<T>>();

            string key = null;
            for (var rowIdx = 1; rowIdx < table.Count; rowIdx++)
            {
                if (!table[rowIdx][0].IsNullOrEmpty())
                {
                    key = table[rowIdx][0];
                }

                if (key == null)
                {
                    throw new Exception($"Missing main key while reading config {typeof(T).FullName}");
                }

                if (!rez.ContainsKey(key))
                {
                    rez[key] = new List<T>();
                }

                var idx = rowIdx;
                string FieldValueGetter(string fieldName) => !dict.ContainsKey(fieldName) ? null : table[idx][dict[fieldName]];
                rez[key].Add(CreateObject<T>(FieldValueGetter));
            }

            return rez.ToDictionary(it => it.Key, it => (IReadOnlyList<T>)it.Value);
        }
        public Dictionary<string, Tuple<TStruct, IReadOnlyList<TNestedTable>>> ReadObjectAndNestedTable<TStruct, TNestedTable>(Stream stream)
        {
            RequireRefType<TStruct>();
            RequireRefType<TNestedTable>();
            
            var table = ReadTable(stream);
            var header = ReadHeader(table);
            
            var structRez = new Dictionary<string, TStruct>();
            var nestedTableRez = new Dictionary<string, List<TNestedTable>>();

            string key = null;
            for (var rowIdx = 1; rowIdx < table.Count; rowIdx++)
            {
                if (!table[rowIdx][0].IsNullOrEmpty()) {
                    key = table[rowIdx][0];
                }
                if (key == null) {
                    throw new Exception($"Missing main key while reading configs {typeof(TStruct).FullName}");
                } 
                var idx = rowIdx;
                string FieldValueGetter(string fieldName) => !header.ContainsKey(fieldName) ? null : table[idx][header[fieldName]];
               
                if (!structRez.ContainsKey(key)) {
                    structRez[key] = CreateObject<TStruct>(FieldValueGetter);
                }
                if (nestedTableRez.ContainsKey(key)) {
                    nestedTableRez[key].Add(CreateObject<TNestedTable>(FieldValueGetter));
                }
                if (!nestedTableRez.ContainsKey(key)) {
                    nestedTableRez[key] = new List<TNestedTable>();
                }

            }
            return structRez.ToDictionary(it => it.Key, it => new Tuple<TStruct, IReadOnlyList<TNestedTable>>(it.Value, nestedTableRez[it.Key]));
        } 
        
        private static void RequireRefType<T>()
        {
            if (typeof(T).IsValueType)
                throw new Exception($"Cannot deserialize config of class {typeof(T).FullName} cause it is not ref value");
        }

        private T CreateObject<T>(Func<string, string> fieldValueGetter)
        {
            var obj = (T)CreateObject(typeof(T), fieldValueGetter);
            return obj;
        }

        private object CreateObject(Type type, Func<string, string> fieldValueGetter)
        {
            var rez = Activator.CreateInstance(type);
            foreach (var fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attribute = GetDataMemberAttribute(fieldInfo);
                if (attribute == null) continue;
                
                if (HasDataContractAttribute(fieldInfo.FieldType))
                {
                    var classValue = CreateObject(fieldInfo.FieldType, fieldValueGetter);
                    fieldInfo.SetValue(rez, classValue);
                }
                else
                {
                    var fieldName = attribute.Name ?? fieldInfo.Name;
                    var strValue = fieldValueGetter(fieldName);
                    var fieldValue = ParseValue(fieldInfo.FieldType, strValue);
                    fieldInfo.SetValue(rez, fieldValue);
                }
            }
            if (rez is ICustomCsvSerializable serializable)
            {
                serializable.Deserialize(fieldValueGetter);
            }
            return rez;
        }

        private object ParseValue(Type fieldType, string strValue)
        {
            try
            {
                if (fieldType.IsArray)
                {
                    return ParseCommaSeparatedArray(fieldType, strValue);
                }
                
                if (strValue.IsNullOrEmpty()) return DefaultValue(fieldType);
                return fieldType.IsEnum ? 
                    Enum.Parse(fieldType, strValue) : 
                    Convert.ChangeType(strValue, fieldType, _cultureInfo);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to parse data '{strValue}' for type {fieldType}", e);
            }
        }

        private object ParseCommaSeparatedArray(Type fieldType, string strValue)
        {
            var objectArray = strValue.Split(',')
                .Select(it => it.Trim())
                .Select(element => ParseValue(fieldType.GetElementType(), element))
                .ToArray();
            var rez = Array.CreateInstance(fieldType.GetElementType(), objectArray.Length);
            Array.Copy(objectArray, rez, objectArray.Length);
            return rez;
        }

        private static object DefaultValue(Type type)
        {
            if (type.IsEnum) return Enum.ToObject(type, 0);
            return type.IsClass ? null : Activator.CreateInstance(type);
        }

        private static bool HasDataContractAttribute(Type type)
        {
            return type.GetCustomAttributes(typeof(DataContractAttribute)).Any();
        }

        private static DataMemberAttribute GetDataMemberAttribute(FieldInfo fieldInfo)
        {
            var dataMemberAttributes = fieldInfo.GetCustomAttributes(typeof(DataMemberAttribute), true);
            if (dataMemberAttributes.Length == 0) return null;
            return dataMemberAttributes[0] as DataMemberAttribute;
        }

        private static Dictionary<string, int> ReadHeader(List<List<string>> table)
        {
            var dict = new Dictionary<string, int>();
            var header = table[0];
            for (var columnIdx = 0; columnIdx < header.Count; columnIdx++)
            {
                if (header[columnIdx].IsNullOrEmpty()) continue;
                dict[header[columnIdx]] = columnIdx;
            }

            return dict;
        }
        private static List<List<string>> ReadTable(Stream stream)
        {
            var table = new List<List<string>>();
            using (var reader = new CsvFileReader(stream))
            {
                reader.ReadAll(table);
            }

            return CleanUpSpaces(table);
        }

        private static List<List<string>> CleanUpSpaces(List<List<string>> table)
        {
            return table.Select(row => row.Select(RemoveSpaces).ToList()).ToList();
        }

        private static string RemoveSpaces(string value)
        {
            var trimmed = value.TrimStart(' ').TrimEnd(' ');
            return trimmed.IsNullOrEmpty() ? null : trimmed;
        }
    }
}