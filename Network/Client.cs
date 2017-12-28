using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Common;
using Core;
using Newtonsoft.Json;

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
        /// </summary>
        /// <param name="ip">Внешний IP</param>
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
                LogService.Trace($"Не удалось подключиться к серверу: {e.Message}");
            }
        }

        /// <summary>
        /// Отправляет запрос на сервер
        /// </summary>
        /// <param name="operType">Тип операции</param>
        /// <param name="oper">Объект операции</param>
        public void SendRequest(OpearationTypes operType, IOperation oper)
        {
            LogService.Trace($"Клиент отправляет {operType}");
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
        /// Принимает ответ 
        /// </summary>
        /// <returns>Возвращает кортеж: тип операции и объект результата</returns>
        public Tuple<OpearationTypes, IOperation> GetResponse()
        { 
            LogService.Trace("Клиент принимает");
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
                LogService.Trace($"Не удалось закрыть сокет клиента: {e.Message}");
            }
        }
    }
}
