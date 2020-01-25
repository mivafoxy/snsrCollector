using System;
using System.Collections.Generic;
using System.Text;

namespace snsrCollector.modules
{
    /// <summary>
    /// Класс несёт в себе информацию о значении.
    /// </summary>
    public class ModuleObjectValue
    {
        /// <summary>
        /// Идентификатор сенсора.
        /// </summary>
        public string LogicalDeviceId { get; set; }

        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public int ObjectId { get; set; }

        /// <summary>
        /// Значение сенсора.
        /// </summary>
        public string ModuleValue { get; set; }

        /// <summary>
        /// Время получения значнения.
        /// </summary>
        public DateTime ReceivedTime { get; set; }
    }
}
