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

        /// <summary>
        /// Генерация случайной расстановки
        /// </summary>        
        public static ShipArrangement Random()
        {
            System.Random random = new System.Random();
            int n = 15; // количество неудачных попыток до перегенерации поля
            int k = 0;
            ShipArrangement arrangement = new ShipArrangement();
            bool r = false; // флаг необходимости перегенерации поля
            do
            {
                r = false;
                k = 0;
                arrangement.ClearFieldbyWater();
                bool dir;
                int x = 0;
                int y = 0;
                do
                {
                    dir = random.NextDouble() < 0.5;
                    x = random.Next(dir ? 10 : 7);
                    y = random.Next(dir ? 7 : 10);
                }
                while (!arrangement.SetShip(x, y, dir ? Direction.Right : Direction.Down, 4));
                for (int i = 0; i < 2; i++) // установка трехпалубных кораблей
                {
                    if (!r)
                    {
                        do
                        {
                            dir = random.NextDouble() < 0.5;
                            x = random.Next(dir ? 10 : 8);
                            y = random.Next(dir ? 8 : 10);
                            r = k++ == n;
                        }
                        while (!r && !arrangement.SetShip(x, y, dir ? Direction.Right : Direction.Down, 3));
                    }
                }
                k = 0;
                for (int i = 0; i < 3; i++) // установка двухпалубных кораблей
                {
                    if (!r)
                    {
                        do
                        {
                            dir = random.NextDouble() < 0.5;
                            x = random.Next(dir ? 10 : 9);
                            y = random.Next(dir ? 9 : 10);
                            r = k++ == n;
                        }
                        while (!r && !arrangement.SetShip(x, y, dir ? Direction.Right : Direction.Down, 2));
                    }
                }
                for (int i = 0; i < 4; i++)                    // установка однопалубных кораблей
                {
                    if (!r)
                    {
                        k = 0;
                        do
                        {
                            x = random.Next(10);
                            y = random.Next(10);
                            r = k++ == n;
                        }
                        while (!r && !arrangement.SetShip(x, y, Direction.Down, 1));
                    }
                }
            }
            while (r);
            return arrangement;
        }

        /// <summary>
        /// Генерация расстановки по стратегии
        /// </summary>
        /// <returns></returns>
        public static ShipArrangement Strategy()
        {
            ShipArrangement arrangement = new ShipArrangement();
            System.Random random = new System.Random();
            int dir = random.Next(4);   // определяет сторону, с которой начинается построение 
            bool first = random.NextDouble() < 0.5;
            System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>(new int[3] { 4, 2, 2 });
            int x = 0;
            for (int i = 0; i < 3; i++)
            {
                int n = random.Next(list.Count);
                int l = list[n];
                list.Remove(l);
                switch (dir)
                {
                    case (0):
                        arrangement.SetShip(x, first ? 0 : 2, Direction.Down, l);
                        break;
                    case (1):
                        arrangement.SetShip(x, first ? 7 : 9, Direction.Down, l);
                        break;
                    case (2):
                        arrangement.SetShip(first ? 0 : 2, x, Direction.Right, l);
                        break;
                    case (3):
                        arrangement.SetShip(first ? 7 : 9, x, Direction.Right, l);
                        break;
                }
                x += l + 1;
            }
            list = new System.Collections.Generic.List<int>(new int[3] { 3, 2, 3 });
            x = 0;
            for (int i = 0; i < 3; i++)
            {
                int n = random.Next(list.Count);
                int l = list[n];
                list.Remove(l);
                switch (dir)
                {
                    case (0):
                        arrangement.SetShip(x, first ? 2 : 0, Direction.Down, l);
                        break;
                    case (1):
                        arrangement.SetShip(x, first ? 9 : 7, Direction.Down, l);
                        break;
                    case (2):
                        arrangement.SetShip(first ? 2 : 0, x, Direction.Right, l);
                        break;
                    case (3):
                        arrangement.SetShip(first ? 9 : 7, x, Direction.Right, l);
                        break;
                }
                x += l + 1;
            }
            for (int i = 0; i < 4; i++)
            {
                int y = 0;
                do
                {
                    x = random.Next(10);
                    y = random.Next(10);
                }
                while (!arrangement.SetShip(x, y, Direction.Down, 1));
            }
            return arrangement;
        }
    }
}
