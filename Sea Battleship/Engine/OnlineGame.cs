using System;
using System.Net;
using System.Threading;
using System.Windows;
using Core;
using Network;
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
        public bool IsOne { get; set; } = false;

        public OnlineGame(PlayerRole playerRole, PlacementState placement, IPAddress ip = null)
        {
            PlayerRole = playerRole;
            Placement = placement;
            InitGame(ip);
        }

        public void SetGameSettings(GameConfig config)
        {
            GameConfig = config;
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
            if (placement != PlacementState.Manualy)
            {
                CreateGame(shipArrangement);
                PlayWindow window = new PlayWindow (this) { Owner = owner };
                window.Show();
            }
            else
            {
                PlacingWindow window = new PlacingWindow(this) { Owner = owner };
                window.Show();
            }
        }

        public void CreateGame(ShipArrangement arragment)
        {
            EnemyArrangement = new ShipArrangement();
            MyArrangement = arragment;
            if (PlayerRole == PlayerRole.Server)
            {
                Connect.Server.SendResponse(OpearationTypes.StartConfig, new StartConfig(GameConfig.GameSpeed));
                var res = Connect.Server.GetRequest();
                Network.ShipArrangement clArrangement = (Network.ShipArrangement)res.Item2;
                Game = new Game(arragment, clArrangement.Arragment, GameConfig);
                IsMyTurn = true;
            }
            else
            {
                var res = Connect.Client.GetResponse();
                StartConfig startConfig = (StartConfig)res.Item2;
                GameConfig = new GameConfig(BotLevels.Easy, startConfig.GameSpeed);
                Connect.Client.SendRequest(OpearationTypes.ShipArrangement, new Network.ShipArrangement(arragment));
                IsMyTurn = false;
            }
        }

        public CellStatе Turn(int x, int y)
        {
            Connect.SendOperation(PlayerRole, OpearationTypes.Shot, new Shot(new Vector(x, y)));
            var res = Connect.GetOperation(PlayerRole);
            var shotRes = (ShotResult)res.Item2;
            SetShotResult(shotRes.State, x, y);
            return shotRes.State;
        }

        public Vector WaitEnemyTurn()
        {
            var res = Connect.GetOperation(PlayerRole);
            Vector shotRes = new Vector(0, 0);
            if (res.Item1 == OpearationTypes.Shot)
            {
                var shot = (Shot)res.Item2;
                shotRes = shot.TargetPosition;
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