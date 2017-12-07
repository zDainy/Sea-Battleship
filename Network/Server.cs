using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Network
{

    public class Server
    {
        // Устанавливаем порт для сокета
        private int _port = 27015;
        private TcpListener _server;
        private TcpClient _client;
        private NetworkStream _networkStream;

        /// <summary>
        /// Создает сервер через сокет
        /// <param name="ip">Внутренний IP</param>
        /// </summary>
        public void Create(IPAddress ip)
        {
            LogService.Trace("Создаем сервер...");
            try
            {
                _server = new TcpListener(ip, _port);

                LogService.Trace("Сервер создан: " + ServerUtils.GetExternalIp() + ":" + _port);

                // Cлушаем входящие запросы
                _server.Start();
                
                LogService.Trace("Ожидание подключений...");

                while (true)
                {
                    // Ожидаем входящее соединение
                    _client = _server.AcceptTcpClient();
                    _networkStream = _client.GetStream();
                    LogService.Trace("Клиент подключен");
                    break;
                }
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось создать сервер: {e}");
            }
        }

        /// <summary>
        /// Принимает запрос
        /// </summary>
        public void GetRequest()
        {
            byte[] bytes = new byte[_client.ReceiveBufferSize];
            int bytesRead = _networkStream.Read(bytes, 0, _client.ReceiveBufferSize);

            // Строка, содержащая ответ от сервера
            string returnData = Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Отправляет ответ на запрос
        /// </summary>
        public void SendResponse()
        {

        }

        /// <summary>
        /// Закрывает сокет сервера
        /// </summary>
        public void Stop()
        {
            LogService.Trace("Отключаем сервер...");
            try
            {
                _server.Stop();
                LogService.Trace("Сервер отключен");
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось закрыть сокет сервера: {e}");
            }
        }
    }
}
