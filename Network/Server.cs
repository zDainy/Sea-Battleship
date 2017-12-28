using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
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
        public bool IsClientConnected { get; set; }

        /// <summary>
        /// Создает сервер через сокет
        /// </summary>
        /// <param name="ip">Внутренний IP</param>
        public void Create(IPAddress ip)
        {
            IsClientConnected = false;
            LogService.Trace("Создаем сервер...");
            try
            {
                _server = new TcpListener(ip, _port);

                LogService.Trace("Сервер создан: " + ServerUtils.GetExternalIp() + ":" + _port);

                // Cлушаем входящие запросы
                _server.Start();
            }
            catch (SocketException e)
            {
                LogService.Trace($"Не удалось создать сервер: {e.Message}");
            }
        }

        /// <summary>
        /// Ожидает подключение клиента
        /// </summary>
        public void WaitConnection()
        {
            LogService.Trace("Ожидание подключений...");
            try
            {
                // Ожидаем входящее соединение
                _client = _server.AcceptTcpClient();
                _networkStream = _client.GetStream();
                IsClientConnected = true;
                LogService.Trace("Клиент подключен");
            }
            catch (Exception e)
            {
                LogService.Trace($"Клиент не смог подключиться: {e.Message}");
            }
        }

        /// <summary>
        /// Принимает запрос 
        /// </summary>
        /// <returns>Возвращает кортеж: тип операции и объект результата</returns>
        public Tuple<OpearationTypes, IOperation> GetRequest()
        {
            LogService.Trace("Сервер принимает");
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
                LogService.Trace($"Не удалось принять: {e.Message}");
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
            LogService.Trace($"Сервер отправляет {operType}");
            try
            {
                string bodyJson;
                if (operType == OpearationTypes.ShipArrangement)
                {
                    var arr = (ShipArrangement)oper;
                    bodyJson = CryptSystem.ArrangementToByte(arr.Arragment);
                }
                else
                {
                    // Сериализуем тело ответа в строку Json
                    bodyJson = oper != null ? JsonConvert.SerializeObject(oper) : "";
                }
               
                JsonData jsonData = new JsonData(operType, bodyJson);

                // Сериализуем объект ответа в строку Json
                string outData = JsonConvert.SerializeObject(jsonData);

                // Отправляем данные клиенту
                byte[] sendBytes = Encoding.UTF8.GetBytes(outData);
                _networkStream.Write(sendBytes, 0, sendBytes.Length);
                LogService.Trace($"Отправлено: {outData}");
            }
            catch (Exception e)
            {
                LogService.Trace($"Не удалось отправить: {e.Message}");
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