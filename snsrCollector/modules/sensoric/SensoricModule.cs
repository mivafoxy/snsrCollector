using snsrCollector.core;
using snsrCollector.db;
using snsrCollector.dbTables;
using snsrCollector.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snsrCollector.modules.sensoric
{
    /// <summary>
    /// Модуль по обработке данных, получаемых с приборов мониторинга сенсорики.
    /// </summary>
    public class SensoricModule : IModule
    {
        // У модуля из идентификаторов есть идентификатор базовой станции и идентификатор узла.
        private readonly string baseStationId;
        private readonly string nodeId;

        private readonly List<DeviceLogical> logicalDevices = new List<DeviceLogical>();

        public SensoricModule(string baseStationId, string nodeId, List<DeviceLogical> logicalDeviceIds)
        {
            this.baseStationId = baseStationId;
            this.nodeId = nodeId;

            this.logicalDevices.AddRange(logicalDeviceIds);
        }

        public string GetSerial()
        {
            return $"{baseStationId} : {nodeId}";
        }

        public List<DeviceLogical> GetLogicalDevice()
        {
            return logicalDevices;
        }

        public void HandleData(byte[] data)
        {
            var moduleValues = new List<ModuleObjectValue>();

            const int startOfData = 10;
            
            string encodedDataPackage = 
                Encoding.UTF8.GetString(
                    data, 
                    startOfData, 
                    (data.Length - startOfData));


            // Пока доступна только температура.
            // Доделать остальные показания.
            ProcessPollResults(
                GetObjectValuesFrom(
                    encodedDataPackage));
        }

        public void ProcessPollResults(List<ModuleObjectValue> objectValues)
        {
            DbService.WriteValuesToDb(objectValues);
            ApplicationCore.GetInstance().GetLogger().LogInfo($"Закончен опрос {GetSerial()}");
        }

        public bool HasCorrectPackage(byte[] data)
        {
            return true; // Как проверить полученный пакет на целостность?
            // Провекрка кратности пакета.
        }

        public bool IsPackageBelongsToModule(byte[] data)
        {
            const int minPackageLength = 10;

            if (data.Length < minPackageLength)
                return false;

            string packageBaseStationId = GetIdStringFrom(data, 0);
            string packageNodeId = GetIdStringFrom(data, 5);

            return 
                (baseStationId.Equals(packageBaseStationId) && nodeId.Equals(packageNodeId));
        }

        private string GetIdStringFrom(byte[] data, int offset)
        {
            const int idBytesCount = 5;
            var idBytes = new byte[idBytesCount];

            Array.Copy(data, offset, idBytes, 0, idBytesCount);

            return System.Text.Encoding.UTF8.GetString(idBytes);
        }

        private List<ModuleObjectValue> GetObjectValuesFrom(string package)
        {
            List<ModuleObjectValue> objectValues = new List<ModuleObjectValue>();

            foreach (var entry in SensoricConsts.GetSupportedSensorsToObjects())
            {
                string sensorValue = GetSensorValueFromPackageOrEmpty(package, entry.Key);

                if (string.IsNullOrEmpty(sensorValue))
                    continue;

                int objectId = entry.Value;
                string logicalId = FindLogicalIdForRequiredObjectId(entry.Value);

                ApplicationCore.GetInstance().GetLogger().LogDebug($"В пакете найдено значение: {entry.Key}:{sensorValue}");

                objectValues.Add(
                    new ModuleObjectValue()
                    {
                        ModuleValue = sensorValue,
                        ObjectId = objectId,
                        ReceivedTime = DateTime.Now,
                        LogicalDeviceId = logicalId
                    });
            }

            return objectValues;
        }

        private string GetSensorValueFromPackageOrEmpty(string package, string key)
        {
            int startOfValue = package.IndexOf(key);

            if (startOfValue == -1)
                return String.Empty;

            startOfValue = package.IndexOf(key) + key.Length;

            int valueLength = -1;
            for (var charIndex = startOfValue; charIndex < package.Length; charIndex++)
            {
                bool isDigit = 
                    Char.IsDigit(package[charIndex]) ||
                    package[charIndex] == '.' ||
                    package[charIndex] == ',';
                
                if (!isDigit)
                {
                    valueLength = charIndex - startOfValue;
                    break;
                }
                else if (isDigit && charIndex == (package.Length - 1))
                {
                    valueLength = (charIndex + 1) - startOfValue;
                    break;
                }
            }

            string objectValue = package.Substring(startOfValue, valueLength);

            return objectValue;
        }  
        
        private string FindLogicalIdForRequiredObjectId(int objectId)
        {
            return 
                logicalDevices.Where(
                    ld => 
                        ld.DeviceObject.Any(
                            devObj => 
                                devObj.ObjectDictId == objectId)).
                    Select(
                        ldResult => ldResult.IdKey).
                    FirstOrDefault();
        }
    }
}
