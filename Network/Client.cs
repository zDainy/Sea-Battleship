using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;

namespace Network
{
    public class Client
    {
        // Устанавливаем порт для сокета
        private int _port = 27015;
        private TcpClient _client;
        private NetworkStream _networkStream;

        /// <summary>
        /// Подлкючает клиента к серверу
        /// <param name="ip">Внешний IP</param>
        /// </summary>
        public void Connect(IPAddress ip)
        {
            LogService.Trace("Подключаемся к серверу...");
            try
            {
                _client = new TcpClient(ip.ToString(), _port);
                _networkStream = _client.GetStream();
                LogService.Trace("Соединение установлено");
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось подключиться к серверу: {e}");
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
