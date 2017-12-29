namespace Core

{
    public class Game
    {
        public ShipArrangement ServerShipArrangement { get; set; }
        public ShipArrangement ClientShipArrangement { get; set; }
        public GameConfig GameConfig { get; set; }
        public PlayerRole TurnOwner { get; set; }
        private ShipArrangement _currentArrangement
        {
            get
            {
                return TurnOwner == PlayerRole.Server ? ClientShipArrangement : ServerShipArrangement;
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
            TurnOwner = turnOwner;
        }

        /// <summary>
        /// Метод совершения хода для онлайн игры.
        /// </summary>
        /// <param name="vertical">Координата по вертикали.</param>
        /// <param name="horizontal">Координата по горизонтали.</param>
        /// <returns></returns>
        public MoveResult MakeAMove(int vertical, int horizontal)
        {
            switch (TurnOwner == PlayerRole.Server ? ClientShipArrangement.GetCellState(vertical, horizontal) : ServerShipArrangement.GetCellState(vertical, horizontal))
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
            TurnOwner = TurnOwner == PlayerRole.Server ? PlayerRole.Client : PlayerRole.Server;
        }
    }
}
