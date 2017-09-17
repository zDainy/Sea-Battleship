using System;
using System.Net;
using System.Net.Sockets;
using Common;

namespace Network
{

    public class SocketServer
    {
        // Устанавливаем порт для TcpListener
        private int _port = 27015;
        private TcpListener _server;
        private TcpClient _client;
        private IPAddress _ipAddress;

        /// <summary>
        /// Открывает сокет сервера
        /// </summary>
        public void Open(IPAddress ip)
        {
            try
            {
                LogService.Trace("Открываем сокет");
                _ipAddress = ip;
                _server = new TcpListener(_ipAddress, _port);

                // Запускаем TcpListener и начинаем слушать клиентов.
                _server.Start();
                LogService.Trace("Ожидание подключений...");
                while (true)
                {
                    // Ожидаем входящее соединение
                    _client = _server.AcceptTcpClient();
                    LogService.Trace("Клиент подключен");
                }
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось создать сервер: {e}");
            }
        }

        public void Stop()
        {
            _server.Stop();
        }
    }
}
