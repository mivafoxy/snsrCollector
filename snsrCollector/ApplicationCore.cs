using snsrCollector.logUtils;
using snsrCollector.utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace snsrCollector.core
{
    /// <summary>
    /// Инкапсуляция работы ядра приложения
    /// </summary>
    public class ApplicationCore
    {
        private static ApplicationCore INSTANCE;

        private SnsrLogger snsrLogger;

        // Объект синхронизации потоков.
        private readonly object syncObject = new object();

        // Остановка приложения.
        private bool isStopped;

        // Конфигупация приложения.
        private AppConfig appConfig;

        // Ядро опроса датчиков.
        private PollCore pollCore;

        // IDb interface...
        // private IDb database;

        // Загрузка ядра приложения включает в себя:
        // Вызов загрузки данных из БД.
        // Вызов инициализации ядра опроса приборов.
        // Вызов инициализации каналов для связи с приборами. Канал стоит в вершине иерархии у приборов.
        // В канале производится создание и конфигурация модулей по работе с приборами.

        private ApplicationCore()
        {
        }

        public static ApplicationCore GetInstance()
        {
            if (INSTANCE is null)
                INSTANCE = new ApplicationCore();

            return INSTANCE;
        }

        /// <summary>
        /// Остановка ядра приложения и всех его дочерних процессов.
        /// </summary>
        public void Stop()
        {
            isStopped = true;

            if (pollCore is null)
                return;

            pollCore.Stop();
        }

        /// <summary>
        /// Инициализация работы ядра приложения.
        /// </summary>
        public void Initialize(AppConfig config)
        {
            isStopped = false;
            this.appConfig = config;
            snsrLogger = new SnsrLogger();

            snsrLogger.LogInfo("Initializing application...");


            // Инициализация ядра приложения.
            // Загрузка модулей (каналов) опроса и мониторинга данных приборов.
            // call db initialize from app config.

            // Запуск ядра опроса для работы с этими модулями.
            pollCore = new PollCore();

            snsrLogger.LogInfo("Starting poll core...");
            pollCore.Start();

            while (true)
            {
                lock (syncObject)
                {
                    if (isStopped) // Вызвать закрытия всех потоков приложения в ядре опроса.
                    {
                        snsrLogger.LogInfo("Stopping poll core...");
                        pollCore.Stop();
                        return;
                    }
                }
            }
        }

        public AppConfig GetAppConfig() => appConfig;

        public SnsrLogger GetLogger() => snsrLogger;
    }
}
