using System;

namespace Core
{
    public class Game
    {
        public ShipArrangement ServerShipArrangement { get; set; }
        public ShipArrangement ClientShipArrangement { get; set; }
        public GameConfig GameConfig { get; set; }
        private PlayerRole _turnOwner;

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
        /// Свитчер хода
        /// </summary>
        public void ChangeTurn()
        {
            // Первых ход всегда за сервером, за исключением случаев когда игра была сохранена
            _turnOwner = _turnOwner == PlayerRole.Server? PlayerRole.Client : PlayerRole.Server;
        }
    }
}
