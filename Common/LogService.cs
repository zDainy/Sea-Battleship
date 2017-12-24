using System;
using System.IO;
using System.Text;

namespace Common
{
    public static class LogService
    {
        private static StreamWriter _sr;

        /// <summary>
        /// Записывает объект в лог
        /// </summary>
        public static void Debug(object obj)
        {
            _sr.WriteLine($"[{DateTime.Now}]: [{obj.GetType()}] {obj}");
            _sr.Flush();
        }

        /// <summary>
        /// Записывает строку в лог
        /// </summary>
        public static void Trace(string str)
        {
            _sr.WriteLine($"[{DateTime.Now}]: {str}");
            _sr.Flush();
        }

        /// <summary>
        /// Запускает сервис записи в лог
        /// </summary>
        public static void Start()
        {
            int count = File.Exists("Logs.log") ? File.ReadAllLines("Logs.log").Length : 0;
            // Если записей в логе слишком много, скопируем его в OldLogs, и очистим текущие логи
            if (count > 100)
            {
                _sr = new StreamWriter("OldLogs.log", false, Encoding.Default);
                string[] strs = File.ReadAllLines("Logs.log", Encoding.Default);
                foreach(string str in strs)
                {
                    _sr.WriteLine(str);
                }
                Close();
                File.Delete("Logs.log");
            }
            _sr = new StreamWriter("Logs.log", true, Encoding.Default);
        }

        public static void Close()
        {
            _sr.Close();
        }
    }
}
