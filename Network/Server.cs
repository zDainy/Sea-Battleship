using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Core;
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
                LogService.Trace($"Не удалось создать сервер: {e.Message}");
            }
        }

        /// <summary>
        /// Принимает запрос 
        /// </summary>
        /// <returns>Возвращает кортеж: тип операции и объект результата</returns>
        public Tuple<OpearationTypes, IOperation> GetRequest()
        {
            LogService.Trace("Принимаем запрос");
            IOperation resultOper = null;
            OpearationTypes operType;
            try
            {
                var resultObj = ServerUtils.ReadJsonData(_client, _networkStream);
                operType = resultObj.Item1;
                resultOper = resultObj.Item2;
            }
            catch (Exception e)
            {
                operType = OpearationTypes.Error;
                LogService.Trace($"Не удалось принять запрос: {e.Message}");
            }
            return Tuple.Create(operType, resultOper);
        }

        /// <summary>
        /// Отправляет ответ на запрос
        /// </summary>
        /// <param name="operType">Тип операции</param>
        /// <param name="oper">Объект операции</param>
        public void SendResponse(OpearationTypes operType, IOperation oper)
        {
            LogService.Trace("Отправляем ответ");
            try
            {
                // Сериализуем тело ответа в строку Json
                string bodyJson = oper != null ? JsonConvert.SerializeObject(oper) : "";
                JsonData jsonData = new JsonData(operType, bodyJson);

                // Сериализуем объект ответа в строку Json
                string outData = JsonConvert.SerializeObject(jsonData);

                // Отправляем данные клиенту
                byte[] sendBytes = Encoding.UTF8.GetBytes(outData);
                _networkStream.Write(sendBytes, 0, sendBytes.Length);
                LogService.Trace($"Ответ отправлен: {outData}");
            }
            catch (Exception e)
            {
                LogService.Trace($"Не удалось отправить ответ: {e.Message}");
            }
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
                LogService.Trace($"Не удалось закрыть сокет сервера: {e.Message}");
            }
        }
    }
}
