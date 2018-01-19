using System.Net;
using System.Threading;
using System.Windows;
using Core;
using Network;
using GameStatus = Core.GameStatus;
using ShipArrangement = Core.ShipArrangement;

namespace Sea_Battleship.Engine
{
    public class OnlineGame
    {
        public PlayerRole PlayerRole { get; set; }
        public GameConfig GameConfig { get; set; }
        private Thread _connectionThread;
        public Connection Connect { get; set; }
        public Game Game { get; set; }
        public ShipArrangement MyArrangement { get; set; }
        public ShipArrangement EnemyArrangement { get; set; }
        public PlacementState Placement { get; set; }
        public bool IsMyTurn { get; set; }
        public bool IsOne { get; set; }

        public OnlineGame(PlayerRole playerRole, PlacementState placement, IPAddress ip = null)
        {
            IsOne = false;
            PlayerRole = playerRole;
            Placement = placement;
            InitGame(ip);
        }

        public void SetGameSettings(GameConfig config)
        {
            GameConfig = config;
            GameConfig.Connection = ServerUtils.GetExternalIp() + ":27015";
        }

        private void InitGame(IPAddress ip)
        {
            Connect = new Connection();
            if (PlayerRole == PlayerRole.Server)
            {
                _connectionThread = new Thread(Connect.CreateLobby);
                _connectionThread.Start();
            }
            else
            {
                _connectionThread = new Thread(Connect.JoinToLobby);
                _connectionThread.Start(ip);
            }
        }



        public void GoToGameWindow(PlacementState placement, ShipArrangement shipArrangement, Window owner)
        {
            //if (placement != PlacementState.Manualy)
            //{
            //    CreateGame(shipArrangement);
            //    PlayWindow window = new PlayWindow(this) { Owner = owner };
            //    window.Show();
            //}
            //else
            //{
            //    PlacingWindow window = new PlacingWindow(this) { Owner = owner };
            //    window.Show();
            //}
        }

        public void CreateGame(ShipArrangement arragment)
        {
            EnemyArrangement = new ShipArrangement();
            MyArrangement = arragment;
            if (PlayerRole == PlayerRole.Server)
            {
                Connect.Server.SendResponse(OpearationTypes.StartConfig, new StartConfig(GameConfig.GameSpeed, Core.GameStatus.Game));
                var res = Connect.Server.GetRequest();
                Network.ShipArrangement clArrangement = (Network.ShipArrangement)res.Item2;
                Connect.Server.SendResponse(OpearationTypes.ShipArrangement, new Network.ShipArrangement(arragment));
                Game = new Game(arragment, clArrangement.Arragment, GameConfig);
                IsMyTurn = true;
            }
            else
            {
                var res = Connect.Client.GetResponse();
                StartConfig startConfig = (StartConfig)res.Item2;
                if (startConfig == null)
                    throw new CookieException();
                GameConfig = new GameConfig(BotLevels.Easy, startConfig.GameSpeed);
                if (startConfig.GameStatus == Core.GameStatus.Loaded)
                {
                    Connect.Client.SendRequest(OpearationTypes.GameStatus, new Network.GameStatus(Core.GameStatus.Game));
                    GameConfig.GameStatus = Core.GameStatus.Loaded;
                    var rArr = Connect.Client.GetResponse();
                    Network.ShipArrangement myArr = (Network.ShipArrangement) rArr.Item2;
                    MyArrangement = myArr.Arragment;
                    Connect.Client.SendRequest(OpearationTypes.GameStatus, new Network.GameStatus(Core.GameStatus.Game));
                }
                else
                {
                    Connect.Client.SendRequest(OpearationTypes.ShipArrangement, new Network.ShipArrangement(arragment));
                }
                var resArr = Connect.Client.GetResponse();
                Network.ShipArrangement enemyArrangementArrangement = (Network.ShipArrangement)resArr.Item2;
                EnemyArrangement = enemyArrangementArrangement.Arragment;
                IsMyTurn = false;
            }
        }

        public void LoadGame(Game game)
        {
            Game = game;
            GameConfig = game.GameConfig;
            GameConfig.GameStatus = Core.GameStatus.Loaded;
            MyArrangement = game.ServerShipArrangement;
            EnemyArrangement = game.ClientShipArrangement;
            Connect.Server.SendResponse(OpearationTypes.StartConfig, new StartConfig(GameConfig.GameSpeed, Core.GameStatus.Loaded));
            Connect.Server.GetRequest();
            Connect.Server.SendResponse(OpearationTypes.ShipArrangement, new Network.ShipArrangement(EnemyArrangement));
            Connect.Server.GetRequest();
            Connect.Server.SendResponse(OpearationTypes.ShipArrangement, new Network.ShipArrangement(MyArrangement));
            IsMyTurn = true;
        }

        public CellStatе Turn(int x, int y)
        {
            CellStatе cellRes;
            Connect.SendOperation(PlayerRole, OpearationTypes.Shot, new Shot(new Vector(x, y)));
            if (x == -1 && y == -1)
            {
                return CellStatе.BlankShot;
            }
            var res = Connect.GetOperation(PlayerRole);
            if (res.Item1 == OpearationTypes.ShotResult)
            {
                var shotRes = (ShotResult)res.Item2;
                SetShotResult(shotRes.State, x, y);
                cellRes = shotRes.State;
            }
            else
            {
                cellRes = CellStatе.BreakShot;
            }
            return cellRes;
        }

        public Vector WaitEnemyTurn()
        {
            var res = Connect.GetOperation(PlayerRole);
            Vector shotRes = new Vector(-1, -1);
            if (res.Item1 == OpearationTypes.Shot)
            {
                var shot = (Shot)res.Item2;
                shotRes = shot.TargetPosition;
            }
            else if (res.Item1 == OpearationTypes.GameStatus)
            {
                Network.GameStatus gs = (Network.GameStatus) res.Item2;
                if (gs.Status == GameStatus.Pause)
                {
                    shotRes = new Vector(-2, -2);
                }
                else if (gs.Status == GameStatus.Break)
                {
                    shotRes = new Vector(-4,-4);
                }
                else
                {
                    shotRes = new Vector(-3, -3);
                }
            }
            return shotRes;
        }

        public void SetShotResult(CellStatе state, int x, int y)
        {
            if (PlayerRole == PlayerRole.Server)
            {
                Game.ClientShipArrangement.SetCellState(state, x, y);
            }
            else
            {
                EnemyArrangement.SetCellState(state, x, y);
            }
        }

        public CellStatе CheckShot(Vector coords)
        {
            CellStatе newState = CellStatе.Water;
            if (PlayerRole == PlayerRole.Server)
            {
                switch (Game.ServerShipArrangement.GetCellState(coords))
                {
                    case CellStatе.Water:
                        newState = CellStatе.WoundedWater;
                        Game.ServerShipArrangement.SetCellState(newState, (int)coords.X, (int)coords.Y);
                        break;
                    case CellStatе.Ship:
                        newState = CellStatе.WoundedShip;
                        Game.ServerShipArrangement.SetCellState(newState, (int)coords.X, (int)coords.Y);
                        break;
                }
            }
            else
            {
                switch (MyArrangement.GetCellState(coords))
                {
                    case CellStatе.Water:
                        newState = CellStatе.WoundedWater;
                        MyArrangement.SetCellState(newState, (int)coords.X, (int)coords.Y);
                        break;
                    case CellStatе.Ship:
                        newState = CellStatе.WoundedShip;
                        MyArrangement.SetCellState(newState, (int)coords.X, (int)coords.Y);
                        break;
                }
            }
            Connect.SendOperation(PlayerRole, OpearationTypes.ShotResult, new ShotResult(newState));
            return newState;
        }
    }
}