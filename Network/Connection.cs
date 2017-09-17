using System;
using System.Net;
using Common;

namespace Network
{
    public class Connection
    {
        public SocketServer Server { get; set; }

        public void CreateLobby()
        {
            Server = new SocketServer();
            try
            {
                IPAddress ip = ServerUtils.GetIP();
                Server.Open(ip);
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка создания лобби: {e}");
            }
            finally
            {
                // Останавливаем TcpListener.
                Server.Stop();
            }
        }
    }
}
