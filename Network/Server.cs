using System;
using System.Net;
using System.Net.Sockets;
using Common;

namespace Network
{

    public class Server
    {
        // Устанавливаем порт для сокета
        private int _port = 27015;
        private Socket _server;
        private Socket _client;
        private IPAddress _ipAddress;
        private IPEndPoint _ipEndPoint;

        /// <summary>
        /// Создает сервер через сокет
        /// </summary>
        public void Create(IPAddress ip)
        {
            LogService.Trace("Создаем сервер...");
            _ipAddress = ip;
            try
            {
                _ipEndPoint = new IPEndPoint(_ipAddress, _port);
                // Создаем сокет Tcp/Ip
                _server = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Связываем Socket с локальной конечной точки.
                _server.Bind(_ipEndPoint);

                LogService.Trace("Сервер создан");

                // Cлушаем входящие сокеты
                _server.Listen(10);

                LogService.Trace("Ожидание подключений...");

                while (true)
                {
                    // Ожидаем входящее соединение
                    _client = _server.Accept();
                    LogService.Trace("Клиент подключен");
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
        public void Close()
        {
            LogService.Trace("Отключаем сервер...");
            try
            {
                //Отключает передачу и прием на Socket.
                _server.Shutdown(SocketShutdown.Both);
                _server.Close();
                LogService.Trace("Сервер отключен");
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось закрыть сокет сервера: {e}");
            }
        }
    }
}
