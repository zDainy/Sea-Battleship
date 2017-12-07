using System;
using System.Runtime.Serialization;
using System.Windows;

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
    }

    /// <summary>
    /// Типы операций
    /// </summary>
    public enum OpearationTypes
    {
        Shot
        /*
         * 1) Выстрел
         * 2) Пауза / Конец игры
         * 3) Переход хода
         * 4) ...
         */
    }

    /// <summary>
    /// Интерфейс операций
    /// </summary>
    public interface IOperation { }

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
    }
}
