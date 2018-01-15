using System;

namespace Core
{
    public class GameConfig
    {
        public bool IsOnline { get; set; }
        public BotLevels BotLvl { get; }
        public GameStatus GameStatus { get; set; }
        public GameSpeed GameSpeed { get; set; }
        public string Connection { get; set; }
        public PlayerRole OnlineRole { get; }

        /// <summary>
        /// Инициализирует объект конфигурации игры
        /// </summary>
        /// <param name="botLvl">Уровень сложности ИИ</param>
        /// <param name="gameSpeed">Скорость игры (ограничение по времени на ход)</param>
        /// <param name="gameStatus">Статус игры</param>
        public GameConfig(BotLevels botLvl, GameSpeed gameSpeed = GameSpeed.Medium, GameStatus gameStatus = GameStatus.Game)
        {
            IsOnline = false;
            GameSpeed = gameSpeed;
            BotLvl = botLvl;
            GameStatus = gameStatus;
        }

        /// <summary>
        /// Инициализирует объект конфигурации онлайн игры
        /// </summary>
        /// <param name="onlineRole">Роль игрока в текущей игре</param>
        /// <param name="connection">Строка соединения</param>
        /// <param name="gameSpeed">Скорость игры (ограничение по времени на ход)</param>
        /// <param name="gameStatus">Статус игры</param>
        public GameConfig(PlayerRole onlineRole, string connection, GameSpeed gameSpeed = GameSpeed.Medium, GameStatus gameStatus = GameStatus.Game)
        {
            IsOnline = true;
            Connection = connection;
            OnlineRole = onlineRole;
            GameSpeed = gameSpeed;
            GameStatus = gameStatus;
        }
    }

    /// <summary>
    /// Роль игрока
    /// </summary>
    public enum PlayerRole
    {
        Server,
        Client
    }

    /// <summary>
    /// Скорость игры
    /// </summary>
    public enum GameSpeed
    {
        Fast = 30000,
        Medium = 60000,
        Slow = 120000,
        Turtle = 300000
    }

    /// <summary>
    /// Состояние игры
    /// </summary>
    public enum GameStatus
    {
        Game,
        Pause,
        End,
        Loaded,
        Break
    }

    /// <summary>
    /// Уровень сложности бота
    /// </summary>
    public enum BotLevels
    {
        Easy,
        Medium,
        Hard
    }

    /// <summary>
    /// Состояние отдельной ячейки на поле
    /// </summary>
    public enum CellStatе
    {
        Water,
        WoundedWater,
        Ship,
        WoundedShip,
        DestroyedShip,
        BlankShot,
        BreakShot
    }

    /// <summary>
    /// Указывает направление установки корабля.
    /// </summary>
    public enum Direction
    {
        Right,
        Down
    }

    /// <summary>
    /// Результат совершенного хода.
    /// </summary>
    public enum MoveResult
    {
        Hit,
        Destroyed,
        Miss,
        Error
    }
        
    public enum PlacementState
    {
        Manualy,
        Randomly,
        Strategily,
        Loaded
    }
}
