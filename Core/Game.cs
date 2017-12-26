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
                    ChangeTurn();
                    return MoveResult.Miss;
                case (CellStatе.Ship):
                    _currentArrangement.SetCellState(CellStatе.WoundedShip, vertical, horizontal);
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
}
