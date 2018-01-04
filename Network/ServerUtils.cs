using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Text;
using Common;
using Newtonsoft.Json;
using Core;

namespace Network
{
    public static class ServerUtils
    {
        private static Stream _stream;
        private static IPAddress _ip;
        private static StreamReader _sr;

        /// <summary>
        /// Получает внешний IP-адрес
        /// </summary>
        public static IPAddress GetExternalIp()
        {
            LogService.Trace("Получаем внешний IP-адрес с сайта...");
            try
            {
                WebClient wClient = new WebClient();
                _stream = wClient.OpenRead("http://www.ip-ping.ru/");
                _sr = new StreamReader(_stream ?? throw new InvalidOperationException());
                string newLine;
                Regex regex = new Regex("<div class=\"hc2\">(.*)</div>");
                while ((newLine = _sr.ReadLine()) != null)
                {
                    Match match = regex.Match(newLine);
                    string str = match.Groups[1].ToString();
                    if (str != "")
                    {
                        _ip = IPAddress.Parse(str);
                        LogService.Trace($"Внешний IP-адрес получен: {str}");
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка получения внешнего IP-адреса: {e.Message}");
            }
            finally
            {
                _sr.Close();
                _stream?.Close();
            }
            return _ip;
        }

        /// <summary>
        /// Получает внутрений IP-адрес
        /// </summary>
        public static IPAddress GetInternalIp()
        {
            LogService.Trace("Получаем внутрений IP-адрес...");
            try
            {
                string host = Dns.GetHostName();
                _ip = Array.Find(Dns.GetHostEntry(host).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                LogService.Trace($"Внутрений IP-адрес получен: {_ip}");
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка получения внутреннего IP-адреса: {e.Message}");
            }
            return _ip;
        }

        /// <summary>
        /// Преобразовывает Json в объект
        /// </summary>
        /// <param name="operType">Тип операции</param>
        /// <param name="jsonData">Json операции</param>
        /// <returns></returns>
        public static Tuple<OpearationTypes, IOperation> GetObjectByJson(OpearationTypes operType, string jsonData)
        {
            IOperation result = null;
            OpearationTypes resOperType = operType;
            try
            {
                switch (operType)
                {
                    case OpearationTypes.Shot:
                        result = JsonConvert.DeserializeObject<Shot>(jsonData);
                        break;
                    case OpearationTypes.ShotResult:
                        result = JsonConvert.DeserializeObject<ShotResult>(jsonData);
                        break;
                    case OpearationTypes.GameStatus:
                        result = JsonConvert.DeserializeObject<GameStatus>(jsonData);
                        break;
                    case OpearationTypes.StartConfig:
                        result = JsonConvert.DeserializeObject<StartConfig>(jsonData);
                        break;
                    case OpearationTypes.ShipArrangement:
                        result = new ShipArrangement(CryptSystem.ByteToArrangement(jsonData));
                        break;
                    case OpearationTypes.Error:
                        break;
                    case OpearationTypes.SwitchTurn:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                LogService.Trace($"Запрос {operType} принят");
            }
            catch (ArgumentOutOfRangeException e)
            {
                resOperType = OpearationTypes.Error;
                LogService.Trace($"Указана неверная операция: {e.Message}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Tuple.Create(resOperType, result);
        }

        /// <summary>
        /// Получает тип и объект операции
        /// </summary>
        /// <param name="client">Сокет клиента</param>
        /// <param name="networkStream">Поток записи</param>
        /// <returns>Возвращает кортеж: тип операции и объект результата</returns>
        public static Tuple<OpearationTypes, IOperation> ReadJsonData(TcpClient client, NetworkStream networkStream)
        {
            OpearationTypes operType;
            IOperation resultOper;
            try
            {
                // Принимаем массив байт от клиента
                byte[] bytes = new byte[client.ReceiveBufferSize];
                networkStream.Read(bytes, 0, client.ReceiveBufferSize);

                // Входные данные в формате Json
                string inData = Encoding.UTF8.GetString(bytes);
                LogService.Trace($"Получен JSON: {inData.Trim("\0".ToCharArray())}");

                // Общий объект операций
                JsonData jsonData = JsonConvert.DeserializeObject<JsonData>(inData);
                operType = jsonData.Header;
                var resultObj = GetObjectByJson(operType, jsonData.Body);
                operType = resultObj.Item1;
                resultOper = resultObj.Item2;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return Tuple.Create(operType, resultOper);
        }

        /// <summary>
        /// Возвращает текстовое представление IP-адреса.
        /// </summary>
        /// <param name="address">IP-адрес.</param>
        /// <returns></returns>
        public static string IPToString(IPAddress address)
        {
            byte[] ip = address.GetAddressBytes();
            byte[] hash = CryptSystem.GetHash(ip);
            ip = CryptSystem.Vigenere(ip, hash);
            byte[] tmp = new byte[6];
            for (int i = 0; i < 4; i++)
            {
                tmp[i] = ip[i];
            }
            tmp[4] = hash[0];
            tmp[5] = hash[1];
            string res = "";
            for (int i = 0; i < tmp.Length; i++)
            {
                res += CryptSystem.ByteToHex(tmp[i]);
            }
            return res;
        }

        /// <summary>
        /// Возвращает IP-адрес по его текстовому представлению.
        /// </summary>
        /// <param name="address">Текстовое представление IP-адреса.</param>
        /// <returns></returns>
        public static IPAddress StringToIP(string address)
        {
            byte[] tmp = new byte[6];
            for (int i=0;i<6;i++)
            {
                tmp[i]= CryptSystem.HexToByte(address[2 * i].ToString() + address[2 * i + 1]);
            }
            byte[] ip = new byte[4];
            byte[] hash = new byte[2] { tmp[4],tmp[5]};
            for (int i = 0; i < 4; i++)
            {
                ip[i] = tmp[i];
            }
            ip = CryptSystem.UnVigenere(ip, hash);
            IPAddress res = new IPAddress(ip);
            return res;
        }
    }
}