﻿using System;
using System.Net;
using Core;

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
                IPAddress ip = ServerUtils.GetInternalIp();
                Server.Create(ip);
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка создания лобби: {e}");
            }
            finally
            {
                // Останавливаем сервер
                Server.Stop();
            }
        }
    }
}
