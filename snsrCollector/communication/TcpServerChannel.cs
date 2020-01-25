using snsrCollector.core;
using snsrCollector.modules;
using snsrCollector.utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace snsrCollector.communication
{
    /// <summary>
    /// Содержит логику tcp канала для получения входящих данных и передачи нижележащему модулю для обработки.
    /// </summary>
    public class TcpServerChannel : IChannel
    {
        private List<IModule> modules = new List<IModule>();

        private readonly string channelId;

        private readonly string host;
        private readonly int port;

        private bool isStarted = false;

        private TcpListener listener;

        // Приборы, связанные с каналом.

        public TcpServerChannel(string host, int port, string channelId)
        {
            this.host = host;
            this.port = port;
            this.channelId = channelId;
        }

        public void AppendModule(IModule module)
        {
            if (isStarted)
                return; // log channel is started.

            modules.Add(module);
        }

        /// <summary>
        /// Смотри базовый класс.
        /// </summary>
        public void Start(CancellationToken cancellationToken)
        {
            if (isStarted)
            {
                ApplicationCore.GetInstance().GetLogger().LogDebug($"Channel already started...{host}:{port}");
                return;
            }

            ApplicationCore.GetInstance().GetLogger().LogDebug($"Starting channel...{host}:{port}");
            // Стартует канал для прослушивания.
            isStarted = true;
            // Захостить сервер.
            var hostAddress = IPAddress.Parse(host);
            listener = new TcpListener(hostAddress, this.port);
            listener.Start();

            Task.Factory.StartNew(() => ListenInputClients(cancellationToken));
        }

        /// <summary>
        /// Смотри базовый класс.
        /// </summary>
        public string GetChannelId()
        {
            return channelId;
        }

        /// <summary>
        /// Смотри базовый класс.
        /// </summary>
        public void LinkChannelWithPollModule(IModule module)
        {
            this.modules.Add(module);
        }

        private void ListenInputClients(CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(() => StopChannel()))
            {
                while (isStarted)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ApplicationCore.GetInstance().GetLogger().LogInfo($"Client connected! {client.Client.RemoteEndPoint.ToString()}");
                    Task.Factory.StartNew(() => HandleClientAsync(client, cancellationToken));
                }
            }
        }

        private async void HandleClientAsync(TcpClient client, CancellationToken token) // async void - avoid, надо сделать шт
        {
            ApplicationCore.GetInstance().GetLogger().LogDebug($"Handle input connection from {client.Client.RemoteEndPoint.ToString()}");
            var stream = client.GetStream();
            byte[] buffer = new byte[256];
            int readCount;

            try
            {
                while ((readCount = await stream.ReadAsync(buffer, 0, buffer.Length, token)) != 0)
                {
                    ApplicationCore.GetInstance().GetLogger().LogInfo($"New data received from {client.Client.RemoteEndPoint.ToString()}");
                    ApplicationCore.GetInstance().GetLogger().LogInfo($"<-: {BinaryUtils.BytesToHexString(buffer, 0, readCount)}");

                    foreach (var module in modules)
                    {
                        var data = new byte[readCount];

                        Array.Copy(buffer, 0, data, 0, data.Length);


                        if (module.IsPackageBelongsToModule(data))
                        {
                            if (module.HasCorrectPackage(data))
                            {
                                ApplicationCore.GetInstance().GetLogger().LogInfo($"{module.GetSerial()} handling data...");
                                module.HandleData(data);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
            finally
            {
                client.Close();
            }
        }

        public void StopChannel()
        {
            isStarted = false;
            listener.Stop();
        }
    }
}
