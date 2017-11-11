using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Common;
using System.Net.Sockets;

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
        public static IPAddress GetExternalIP()
        {
           // LogService.Trace("Получаем внешний IP-адрес с сайта...");
            try
            {
                WebClient wClient = new WebClient();
                _stream = wClient.OpenRead("http://www.ip-ping.ru/");
                _sr = new StreamReader(_stream);
                string newLine;
                Regex regex = new Regex("<div class=\"hc2\">(.*)</div>");
                while ((newLine = _sr.ReadLine()) != null)
                {
                    Match match = regex.Match(newLine);
                    string str = match.Groups[1].ToString();
                    if (str != "")
                    {
                        _ip = IPAddress.Parse(str);
               //         LogService.Trace($"Внешний IP-адрес получен: {str}");
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка получения внешнего IP-адреса: {e}");
            }
            finally
            {
                _sr.Close();
                _stream.Close();
            }
            return _ip;
        }

        /// <summary>
        /// Получает внутрений IP-адрес
        /// </summary>
        public static IPAddress GetInternalIP()
        {
            LogService.Trace("Получаем внутрений IP-адрес...");
            try
            {
                string host = Dns.GetHostName();
                _ip = Array.Find(Dns.GetHostEntry(host).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);
                LogService.Trace($"Внутрений IP-адрес получен: {_ip.ToString()}");
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка получения внутреннего IP-адреса: {e}");
            }
            return _ip;
        }
    }
}
