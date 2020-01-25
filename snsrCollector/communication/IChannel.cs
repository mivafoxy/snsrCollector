using snsrCollector.modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace snsrCollector.communication
{
    // Интерфейс канала обмена данными.
    // В будущем сделать подобное: https://github.com/chronoxor/NetCoreServer/blob/master/source/NetCoreServer/TcpServer.cs
    public interface IChannel
    {
        /// <summary>
        // Стартует канал опроса.
        /// </summary>
        public void Start(CancellationToken cancellationToken);

        /// <summary>
        /// Получить идентификатор канала.
        /// </summary>
        public string GetChannelId();

        /// <summary>
        /// Добавить зависимый для канала модуль.
        /// </summary>
        public void LinkChannelWithPollModule(IModule module);
    }
}
