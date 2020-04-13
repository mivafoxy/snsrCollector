using snsrCollector.dbTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace snsrCollector.modules
{
    // Интерфейс логики работы модулей.
    public interface IModule
    {
        // Обработка данных, полученных от датчика.
        void HandleData(byte[] data);
        /// <summary>
        /// Принадлежать ли данные, полученные из интернета, модулю?
        /// </summary>
        bool IsPackageBelongsToModule(byte[] data);

        /// <summary>
        /// Корректны ли данные, полученные модулем?
        /// </summary>
        bool HasCorrectPackage(byte[] data);

        /// <summary>
        /// Получить серийный номер устройства для идентификации.
        /// </summary>
        string GetSerial();

        /// <summary>
        /// Обработать результаты опроса.
        /// </summary>
        void ProcessPollResults(List<ModuleObjectValue> objectValues);

        /// <summary>
        /// Получить идентификаторы логических устройств модуля.
        /// </summary>
        List<DeviceLogical> GetLogicalDevice();
    }
}
