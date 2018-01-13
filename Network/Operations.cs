using System;
using System.Runtime.Serialization;
using System.Windows;
using Core;

namespace Network
{
    /// <summary>
    /// Класс данных передачи между сервером и клиентом
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// Тип операции
        /// </summary>
        [DataMember]
        public OpearationTypes Header { get; set; }

        /// <summary>
        /// Тело операции
        /// </summary>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        /// Инициализирует объект данных для передачи
        /// </summary>
        /// <param name="operType">Тип операции</param>
        /// <param name="body">Тело операции</param>
        public JsonData(OpearationTypes operType, string body)
        {
            Header = operType;
            Body = body;
        }
    }

    /// <summary>
    /// Типы операций
    /// </summary>
    public enum OpearationTypes
    {
        Error,
        Shot,
        ShotResult,
        SwitchTurn,
        GameStatus,
        StartConfig,
        ShipArrangement
    }

    /// <summary>
    /// Интерфейс операций
    /// </summary>
    public interface IOperation
    {
    }

    /// <inheritdoc />
    /// <summary>
    /// Класс выстрела
    /// </summary>
    [DataContract]
    public class Shot : IOperation
    {
        /// <summary>
        /// Координаты выстрела
        /// </summary>
        [DataMember]
        public Vector TargetPosition { get; set; }

        /// <summary>
        /// Инициализирует объект выстрел
        /// </summary>
        /// <param name="vect">Координаты на поле</param>
        public Shot(Vector vect)
        {
            TargetPosition = vect;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Класс результата выстрела
    /// </summary>
    [DataContract]
    public class ShotResult : IOperation
    {
        /// <summary>
        /// Результат выстрела
        /// </summary>
        [DataMember]
        public CellStatе State { get; set; }

        /// <summary>
        /// Инициализирует объект результата выстрела
        /// </summary>
        /// <param name="state">Состояние клетки</param>
        public ShotResult(CellStatе state)
        {
            State = state;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Класс изменения статуса игры
    /// </summary>
    [DataContract]
    public class GameStatus : IOperation
    {
        /// <summary>
        /// Статус игры
        /// </summary>
        [DataMember]
        public Core.GameStatus Status { get; set; }

        /// <summary>
        /// Инициализирует объект состояния игры
        /// </summary>
        /// <param name="status">Состояние игры</param>
        public GameStatus(Core.GameStatus status)
        {
            Status = status;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Класс начальных настроек
    /// </summary>
    [DataContract]
    public class StartConfig : IOperation
    {
        /// <summary>
        /// Скорость игры
        /// </summary>
        [DataMember]
        public GameSpeed GameSpeed { get; set; }

        [DataMember]
        public Core.GameStatus GameStatus { get; set; }

        /// <summary>
        /// Инициализирует объект начальных настроек
        /// </summary>
        /// <param name="speed"></param>
        public StartConfig(GameSpeed speed, Core.GameStatus gameStatus )
        {
            GameSpeed = speed;
            GameStatus = gameStatus;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Класс расстановки
    /// </summary>
    [DataContract]
    public class ShipArrangement : IOperation
    {
        /// <summary>
        /// Расстановка
        /// </summary>
        [DataMember]
        public Core.ShipArrangement Arragment { get; set; }

        /// <summary>
        /// Инициализирует объект расстановки
        /// </summary>
        /// <param name="arr">Расстановка</param>
        public ShipArrangement(Core.ShipArrangement arr)
        {
            Arragment = arr;
        }
    }
}
