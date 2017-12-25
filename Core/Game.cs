

namespace Core
{
    public class Game
    {
        public ShipArrangement ServerShipArrangement { get; set; }
        public ShipArrangement ClientShipArrangement { get; set; }
        public GameConfig GameConfig { get; set; }
        private PlayerRole _turnOwner;
        private ShipArrangement _currentArrangement
        {
            get
            {
                return _turnOwner == PlayerRole.Server ? ClientShipArrangement : ServerShipArrangement;
            }
        }

        /// <summary>
        /// Инициализирует объект игра
        /// </summary>
        /// <param name="serverShipArr">Расстановка кораблей сервера</param>
        /// <param name="clientShipArr">Расстановка кораблей клиента</param>
        /// <param name="gameConfig">Конфигурация игры</param>
        /// <param name="turnOwner"></param>
        public Game(ShipArrangement serverShipArr, ShipArrangement clientShipArr, GameConfig gameConfig, PlayerRole turnOwner = PlayerRole.Server)
        {
            ServerShipArrangement = serverShipArr;
            ClientShipArrangement = clientShipArr;
            GameConfig = gameConfig;
            _turnOwner = turnOwner;
        }

        public PlayerRole GetTurnOwner()
        {
            return _turnOwner;
        }

        /// <summary>
        /// Метод совершения хода для ИИ.
        /// </summary>
        /*public void MakeAMove()
        {
            System.Windows.Vector vector = new System.Windows.Vector();
            MoveResult result = MoveResult.Error;
            do
            {
                vector = AI.MakeAMove(GameConfig.BotLvl, ServerShipArrangement, result);
                result = MakeAMove((int)vector.X, (int)vector.Y);
            }
            while (_turnOwner == PlayerRole.Client);
        }*/

        /// <summary>
        /// Метод совершения хода для онлайн игры.
        /// </summary>
        /// <param name="vertical">Координата по вертикали.</param>
        /// <param name="horizontal">Координата по горизонтали.</param>
        /// <returns></returns>
        public MoveResult MakeAMove(int vertical, int horizontal)
        {
            switch (_currentArrangement.GetCellState(vertical, horizontal))
            {
                case (CellStatе.Water):
                    _currentArrangement.SetCellState(CellStatе.WoundedWater, vertical, horizontal);
                    //ChangeTurn();
                    return MoveResult.Miss;
                case (CellStatе.Ship):
                    _currentArrangement.SetCellState(CellStatе.WoundedShip, vertical, horizontal);
                    System.Collections.Generic.List<System.Windows.Vector> ship = new System.Collections.Generic.List<System.Windows.Vector>();
                    System.Collections.Generic.List<System.Windows.Vector> analysed = new System.Collections.Generic.List<System.Windows.Vector>();
                    ship.Add(new System.Windows.Vector(vertical, horizontal));
                    bool destroyed = true;
                    do
                    {
                        System.Windows.Vector cur = ship[0];
                        ship.Remove(cur);
                        analysed.Add(cur);
                        switch (_currentArrangement.GetCellState(cur))
                        {
                            case CellStatе.Ship:
                                destroyed = false;
                                break;
                            case CellStatе.WoundedShip:
                                if (cur.X > 0)
                                    if (!analysed.Contains(new System.Windows.Vector(cur.X - 1, cur.Y)))
                                        ship.Add(new System.Windows.Vector(cur.X - 1, cur.Y));
                                if (cur.X < 9)
                                    if (!analysed.Contains(new System.Windows.Vector(cur.X + 1, cur.Y)))
                                        ship.Add(new System.Windows.Vector(cur.X + 1, cur.Y));
                                if (cur.Y > 0)
                                    if (!analysed.Contains(new System.Windows.Vector(cur.X, cur.Y - 1)))
                                        ship.Add(new System.Windows.Vector(cur.X, cur.Y - 1));
                                if (cur.Y < 9)
                                    if (!analysed.Contains(new System.Windows.Vector(cur.X, cur.Y + 1)))
                                        ship.Add(new System.Windows.Vector(cur.X, cur.Y + 1));
                                break;
                        }
                    }
                    while (destroyed && ship.Count > 0);
                    if (destroyed)
                    {
                        foreach (System.Windows.Vector v in analysed)
                        {
                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    if (((int)v.X + i >= 0) && ((int)v.X + i <= 9) && ((int)v.Y + j >= 0) && ((int)v.Y + j <= 9))   // проверка на выход за границы массива
                                        if (_currentArrangement.GetCellState((int)v.X + i, (int)v.Y + j) == CellStatе.Water)
                                            _currentArrangement.SetCellState(CellStatе.WoundedWater, (int)v.X + i, (int)v.Y + j);
                                }
                            }
                        }
                        return MoveResult.Destroyed;
                    }
                    return MoveResult.Hit;
                default:
                    return MoveResult.Error;
            }
        }

        /// <summary>
        /// Свитчер хода
        /// </summary>
        public void ChangeTurn()
        {
            // Первых ход всегда за сервером, за исключением случаев когда игра была сохранена
            _turnOwner = _turnOwner == PlayerRole.Server ? PlayerRole.Client : PlayerRole.Server;
        }
    }

    public enum MoveResult
    {
        Hit,
        Destroyed,
        Miss,
        Error
    }
}
