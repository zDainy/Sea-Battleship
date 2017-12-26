using System;
using System.Net;
using Common;
using Core;

namespace Network
{
    public class Connection
    {
        public Server Server { get; set; }
        public Client Client { get; set; }

        public void CreateLobby()
        {
            try
            {
                Server = new Server();
                IPAddress ip = ServerUtils.GetInternalIp();
                Server.Create(ip);
                Server.WaitConnection();
            }
            catch (Exception e)
            {
                LogService.Trace($"Ошибка создания лобби: {e}");
            }
        }

        public void JoinToLobby(object ipObj)
        {
            IPAddress ip = (IPAddress)ipObj;
            Client = new Client();
            Client.Connect(ip);
        }

        public void SendOperation(PlayerRole role, OpearationTypes operType, IOperation oper)
        {
            if (role == PlayerRole.Server)
            {
                Server.SendResponse(operType, oper);
            }
            else
            {
                Client.SendRequest(operType, oper);
            }
        }

        public Tuple<OpearationTypes, IOperation> GetOperation(PlayerRole role)
        {
            return role == PlayerRole.Server ? Server.GetRequest() : Client.GetResponse();
        }
    }
}
