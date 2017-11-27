using System.Windows;

namespace Core
{
    public class ShipArrangement
    {
        private readonly CellStatе[,] _arragment;

        public ShipArrangement()
        {
            _arragment = new CellStatе[10, 10];
            ClearField();
        }

        /// <summary>
        /// Заполняет игровое поле водой
        /// </summary>
        public void ClearField()
        {
            for (int i = 0; i < _arragment.Length; i++)
            {
                for (int j = 0; j < _arragment.Length; j++)
                {
                    _arragment[i, j] = CellStatе.Water;
                }
            }
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

    }

    /// <summary>
    /// Состояния отдельной ячейки на поле
    /// </summary>
    public enum CellStatе
    {
        Water,
        WoundedWater,
        Ship,
        WoundedShip,
        DestroyedShip
    }
}
