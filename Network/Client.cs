using System;
using System.Net;
using System.Net.Sockets;
using Common;

namespace Network
{
    class Client
    {
        // Устанавливаем порт для сокета
        private int _port = 27015;
        private Socket _server;
        private Socket _client;
        private IPAddress _ipAddress;
        private IPEndPoint _ipEndPoint;

        /// <summary>
        /// Подлкючает клиента к серверу
        /// </summary>
        public void Connect(IPAddress ip)
        {
            LogService.Trace("Подключаемся к серверу...");
            _ipAddress = ip;
            try
            {
                _ipEndPoint = new IPEndPoint(_ipAddress, _port);
                // Создаем сокет Tcp/Ip
                _client = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Соединяем сокет с удаленной точкой
                _client.Connect(_ipEndPoint);

                LogService.Trace("Соединение установлено");
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось создать сервер: {e}");
            }
        }

        /// <summary>
        /// Отправляет запрос на сервер
        /// </summary>
        public void SendRequest()
        {

        }

        /// <summary>
        /// Получает ответ на запрос
        /// </summary>
        public void GetResponse()
        {

        }

        /// <summary>
        /// Закрывает сокет клиента
        /// </summary>
        public void Close()
        {
            LogService.Trace("Отключаемся от сервера...");
            try
            {
                //Отключает передачу и прием на Socket.
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();
                LogService.Trace("Соединение завершено");
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось закрыть сокет клиента: {e}");
            }
        }
    }
}
