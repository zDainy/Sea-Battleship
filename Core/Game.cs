using System;

namespace Core
{
    public class Game
    {
        public ShipArrangement ServerShipArrangement { get; set; }
        public ShipArrangement ClientShipArrangement { get; set; }
        public GameStatus GameStatus { get; set; }
        private bool _isServerTurn;
        private readonly bool _isOnline;

        /// <summary>
        /// Инициализирует объект игра
        /// </summary>
        /// <param name="serverShipArr">Расстановка кораблей сервера</param>
        /// <param name="clientShipArr">Расстановка кораблей клиента</param>
        /// <param name="isOnline">Флаг, показывающий находится ли игра в режиме "по сети"</param>
        /// <param name="isServerTurn">Флаг, показывающий является ли текущий ход - ходом сервера</param>
        /// <param name="gStatus">Состояние игры</param>
        public Game(ShipArrangement serverShipArr, ShipArrangement clientShipArr, bool isOnline = false, bool isServerTurn = true, GameStatus gStatus = GameStatus.Game)
        {
            ServerShipArrangement = serverShipArr;
            ClientShipArrangement = clientShipArr;
            GameStatus = gStatus;
            _isServerTurn = isServerTurn;
            _isOnline = isOnline;
        }

        /// <summary>
        /// Свитчер хода
        /// </summary>
        public void ChangeTurn()
        {
            // Первых ход всегда за сервером, за исключением случаев когда игра была сохранена
            _isServerTurn = !_isServerTurn;
        }
    }

    /// <summary>
    /// Состояния игры
    /// </summary>
    public enum GameStatus
    {
        Game,
        Pause,
        End
    }
}
