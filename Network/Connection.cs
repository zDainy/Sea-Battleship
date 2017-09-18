using System;
using System.Net;
using Common;

namespace Network
{
    public class Connection
    {
        public Server Server { get; set; }

        public void CreateLobby()
        {
            try
            {
                Server = new Server();
                IPAddress ip = ServerUtils.GetInternalIP();
                Server.Create(ip);
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка создания лобби: {e}");
            }
            finally
            {
                // Останавливаем сервер
                Server.Close();
            }
        }
    }
}
