using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Common;

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
        public static IPAddress GetIP()
        {
            try
            {
                LogService.Trace("Пытаемся получить внешний IP-адрес с сайта");
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
                        LogService.Trace($"IP-адрес получен: {str}");
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка получения IP-адреса: {e}");
            }
            finally
            {
                _sr.Close();
                _stream.Close();
            }
            return _ip;
        }
    }
}
