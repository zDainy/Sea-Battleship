using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using Newtonsoft.Json;

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
        public IOperation GetRequest()
        {
            LogService.Trace("Новый запрос");
            IOperation resultOper = null;
            try
            {
                // Принимаем массив байт от клиента
                byte[] bytes = new byte[_client.ReceiveBufferSize];
                _networkStream.Read(bytes, 0, _client.ReceiveBufferSize);

                // Входные данные в формате Json
                string inData = Encoding.UTF8.GetString(bytes);

                // Общий объект операций
                JsonData jsonData = JsonConvert.DeserializeObject<JsonData>(inData);
                LogService.Trace($"Получен JSON: {jsonData}");

                switch (jsonData.Header)
                {
                    case OpearationTypes.Shot:
                        resultOper = JsonConvert.DeserializeObject<Shot>(jsonData.Body);
                        LogService.Trace($"Запрос {jsonData.Header} принят");
                        break;
                }
            }
            catch (Exception e)
            {
                LogService.Trace($"Не удалось принять запрос: {e}");
            }
            return resultOper;
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
