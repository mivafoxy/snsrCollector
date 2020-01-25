using snsrCollector.communication;
using snsrCollector.db;
using snsrCollector.logUtils;
using snsrCollector.modules;
using snsrCollector.modules.sensoric;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace snsrCollector.core
{
    /// <summary>
    /// Инкапсуляция работы ядра опроса.
    /// Производится диспетчирезация между каналами связи и приборами в них содержащихся.
    /// </summary>
    public class PollCore
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Ядро опроса должно вызывать запуск сервера для прослушивания входящих данных.
        // Каналы, в которые поступают данные, затем передают их на обработку нижележащим модулям.
        // Нижележащие модули обработанные данные посылают в сервисы по работе с БД для сохранения.

        private bool isStarted = false;

        private readonly List<IChannel> pollChannels = new List<IChannel>();

        public void Start()
        {
            if (isStarted)
                return;

            isStarted = true;
            // При старте загружаются каналы из БД с модулями опроса.

            List<IChannel> commChannels = DbService.LoadCommChannels();

            SnsrLogger logger = ApplicationCore.GetInstance().GetLogger();

            logger.LogDebug("Starting poll core.");
            
            foreach (var commChannel in commChannels)
                StartChannel(commChannel);
        }

        public void Stop()
        {
            // Остановка всех каналов  опроса.
            cancellationTokenSource.Cancel();
            isStarted = false;
        }

        private void StartChannel(IChannel channel)
        {
            Task.Factory.StartNew(() => channel.Start(cancellationTokenSource.Token), cancellationTokenSource.Token); // Используется пул потоков. Доработать до использования CancellationToken для закрытия потока.
        }
    }
}
