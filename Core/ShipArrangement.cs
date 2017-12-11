using System.Runtime.Serialization;
using System.Windows;

namespace Core
{
    public class ShipArrangement
    {
        private readonly CellStatе[,] _arragment;

        public ShipArrangement()
        {
            _arragment = new CellStatе[10, 10];
            ClearFieldbyWater();
        }

        /// <summary>
        /// Заполняет игровое поле водой
        /// </summary>
        public void ClearFieldbyWater()
        {
            for (int i = 0; i < _arragment.GetLength(0); i++)
            {
                for (int j = 0; j < _arragment.GetLength(1); j++)
                {
                    SetCellState(CellStatе.Water, i, j);
                }
            }
        }

        /// <summary>
        /// Возвращает расстановку кораблей
        /// </summary>
        /// <returns>Расстановка</returns>
        public CellStatе[,] GetArrangement()
        {
            return _arragment;
        }

        /// <summary>
        /// Возвращает состояние клетки по 2 координатам
        /// </summary>
        /// <param name="vertical">Координаты по вертикали</param>
        /// <param name="horizontal">Координаты по горизонтали</param>
        /// <returns>Состояние клетки</returns>
        public CellStatе GetCellState(int vertical, int horizontal)
        {
            return _arragment[vertical, horizontal];
        }

        /// <summary>
        /// Возвращает состояние клетки по точке 
        /// </summary>
        /// <param name="vector">Точка на игровом поле</param>
        /// <returns>Состояние клетки</returns>
        public CellStatе GetCellState(Vector vector)
        {
            return _arragment[(int)vector.X, (int)vector.Y];
        }

        /// <summary>
        /// Устанавливает состояние клетки по 2 координатам
        /// </summary>
        /// <param name="state">Состояние клетки</param>
        /// <param name="vertical">Координаты по вертикали</param>
        /// <param name="horizontal">Координаты по горизонтали</param>
        public void SetCellState(CellStatе state, int vertical, int horizontal)
        {
            _arragment[vertical, horizontal] = state;
        }

        /// <summary>
        /// Устанавливает состояние клетки по точке
        /// </summary>
        /// <param name="state">Состояние клетки</param>
        /// <param name="vector">Точка на игровом поле</param>
        private void SetCellState(CellStatе state, Vector vector)
        {
            _arragment[(int)vector.X, (int)vector.Y] = state;
        }

        /// <summary>
        /// Устанавливает состояние клеток по точкам
        /// </summary>
        /// <param name="state">Состояние клетки</param>
        /// <param name="cells">Перечисление точек</param>
        private void SetCellState(CellStatе state, params Vector[] cells)
        {
            foreach (Vector с in cells)
            {
                _arragment[(int)с.X, (int)с.Y] = state;
            }
        }

        /// <summary>
        /// Устанавливает корабль по указанным координатам в указанном направлении. Возвращает значение, указывающее, успешна ли операция. 
        /// </summary>
        /// <param name="direction">Направление установки.</param>
        /// <param name="vertical">Координата по вертикали.</param>
        /// <param name="horizontal">Координата по горизонтали.</param>
        /// <param name="Length">Длина корабля. Если длина меньше нуля или больше четырех, то установка будет неуспешна.</param>
        public bool SetShip(int vertical, int horizontal, Direction direction, int Length)
        {
            if ((Length < 0) || (Length > 4) || (vertical < 0) || (horizontal < 0) || (vertical > 9) || (horizontal > 9)) return false;
            if (direction == Direction.Down)
            {
                if (vertical + Length > 10) return false;
                for (int k = 0; k < Length; k++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if ((vertical + i + k >= 0) && (vertical + i + k <= 9) && (horizontal + j >= 0) && (horizontal + j <= 9))   // проверка на выход за границы массива
                                if (GetCellState(vertical + i + k, horizontal + j) == CellStatе.Ship) return false;     // проверка на наличие корабля рядом с устанавливаемым
                        }
                    }
                }
                for (int k = 0; k < Length; k++)
                {
                    SetCellState(CellStatе.Ship, vertical + k, horizontal);
                }
            }
            else
            {
                if (horizontal + Length > 10) return false;
                for (int k = 0; k < Length; k++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if ((vertical + i >= 0) && (vertical + i <= 9) && (horizontal + j + k >= 0) && (horizontal + j + k <= 9))
                                if (GetCellState(vertical + i, horizontal + j + k) == CellStatе.Ship) return false;
                        }
                    }
                }
                for (int k = 0; k < Length; k++)
                {
                    SetCellState(CellStatе.Ship, vertical, horizontal + k);
                }
            }
            return true;
        }
    }
    /// <summary>
    /// Указывает направление установки корабля.
    /// </summary>
    public enum Direction
    {
        Right,
        Down
    }
}
