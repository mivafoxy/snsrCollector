using snsrCollector.core;
using snsrCollector.utils;
using System;

namespace snsrCollector
{
    /// <summary>
    /// Главный класс приложения по работе с приборами мониторинга окружающей среды.
    /// </summary>
    class Program
    {
        static void Main()
        {
            try
            {
                // Загрузить настройки приложения из файла конфигурации.
                var configFilePath = "./appconfig.ini"; // Debug string.
                var config = new AppConfig(configFilePath);

                // Инициализировать приложение исходя из параметров конфигурационного файла.
                Console.WriteLine("Application initialization...");
                ApplicationCore.GetInstance().Initialize(config);
            }
            catch (Exception e)
            {
                // log error.
                Console.Error.WriteLine("ERROR!");
                Console.Error.WriteLine(e.Message);
            }
            finally
            {
                // Закончить все процессы программы при завершении работы ядра.
                Console.Error.WriteLine("Stopping application...");
                ApplicationCore.GetInstance().Stop();
            }
        }
    }
}
