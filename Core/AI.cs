using System.Windows;
namespace Core
{
    public static class AI
    {
        private static System.Random random = new System.Random();
        private static System.Collections.Generic.List<Vector> Will = new System.Collections.Generic.List<Vector>();

        private static bool FindFourDestroyed(ShipArrangement arrangement)
        {
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (arrangement.GetCellState(i, j) == CellStatе.DestroyedShip) count++;
                    else count = 0;
                    if (count == 4) return true;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (arrangement.GetCellState(j, i) == CellStatе.DestroyedShip) count++;
                    else count = 0;
                    if (count == 4) return true;
                }
            }
            return false;
        }

        private static bool SomethingExceptOne(ShipArrangement arrangement)
        {
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (arrangement.GetCellState(i, j) == CellStatе.DestroyedShip) count++;
                    else count = 0;
                    if (count == 2) return true;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (arrangement.GetCellState(j, i) == CellStatе.DestroyedShip) count++;
                    else count = 0;
                    if (count == 2) return true;
                }
            }
            return false;
        }

        private static int Count(ShipArrangement arrangement)
        {
            int res = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (arrangement.GetCellState(i, j) == CellStatе.Ship || arrangement.GetCellState(i, j) == CellStatе.Water) res++;
                }
            }
            return res;
        }

        private static void FillList(ShipArrangement arrangement)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (arrangement.GetCellState(i, j) == CellStatе.Ship || arrangement.GetCellState(i, j) == CellStatе.Water)
                        Will.Add(new Vector(i, j));
                }
            }
        }

        public static Point MakeAMove(Game g)
        {
            if (Will.Count == 0 && Count(g.ServerShipArrangement) < 28) FillList(g.ServerShipArrangement);
            int x = 0;
            int y = 0;
            switch (g.GameConfig.BotLvl)
            {
                case BotLevels.Easy:
                    if (Will.Count == 0)
                    {
                        do
                        {
                            x = random.Next(10);
                            y = random.Next(10);
                        }
                        while (g.MakeAMove(x, y) == MoveResult.Error);
                    }
                    else
                    {
                        int n = random.Next(Will.Count);
                        Vector vector = Will[n];
                        Will.RemoveAt(n);
                        x = (int)vector.X;
                        y = (int)vector.Y;
                        g.MakeAMove(x, y);
                    }
                    break;
                case BotLevels.Medium:
                    int fx = -1;
                    int fy = -1;
                    int i = 0;
                    int j = 0;
                    while ((i < 10) && (fx == -1))
                    {
                        j = 0;
                        while ((j < 10) && (fy == -1))
                        {
                            if (g.ServerShipArrangement.GetCellState(i, j) == CellStatе.WoundedShip)
                            {
                                fx = i;
                                fy = j;
                            }
                            else
                            {
                                j++;
                            }
                        }
                        i++;
                    }
                    if (fx == -1)
                    {
                        if (Will.Count == 0)
                        {
                            do
                            {
                                x = random.Next(10);
                                y = random.Next(10);
                            }
                            while (g.MakeAMove(x, y) == MoveResult.Error);
                        }
                        else
                        {
                            int n = random.Next(Will.Count);
                            Vector vector = Will[n];
                            Will.RemoveAt(n);
                            x = (int)vector.X;
                            y = (int)vector.Y;
                            g.MakeAMove(x, y);
                        }
                        break;
                    }
                    else
                    {
                        if (fx + 1 < 10 && g.ServerShipArrangement.GetCellState(fx + 1, fy) == CellStatе.WoundedShip)
                        {
                            int px = fx + 1;
                            while ((px < 10) && (g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedShip))
                            {
                                px++;
                            }
                            if (fx - 1 >= 0 && (px == 10) || (g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx - 1, fy);
                                x = fx - 1;
                                y = fy;
                            }
                            else
                            {
                                g.MakeAMove(px, fy);
                                x = px;
                                y = fy;
                            }
                        }
                        else if (fy + 1 < 10 && g.ServerShipArrangement.GetCellState(fx, fy + 1) == CellStatе.WoundedShip)
                        {
                            int py = fy + 1;
                            while ((py < 10) && (g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedShip))
                            {
                                py++;
                            }
                            if (fy - 1 >= 0 && (py == 10) || (g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy - 1);
                                x = fx;
                                y = fy - 1;
                            }
                            else
                            {
                                g.MakeAMove(fx, py);
                                x = fx;
                                y = py;
                            }
                        }
                        else
                        {
                            if ((fx + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx + 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx + 1, fy);
                                x = fx + 1;
                                y = fy;
                            }
                            else if ((fy + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx, fy + 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy + 1);
                                x = fx;
                                y = fy + 1;
                            }
                            else if ((fx - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx - 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx - 1, fy);
                                x = fx - 1;
                                y = fy;
                            }
                            else if ((fy - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx, fy - 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy - 1);
                                x = fx;
                                y = fy - 1;
                            }
                        }
                    }
                    break;
                case BotLevels.Hard:
                    fx = -1;
                    fy = -1;
                    i = 0;
                    j = 0;
                    while ((i < 10) && (fx == -1))
                    {
                        j = 0;
                        while ((j < 10) && (fy == -1))
                        {
                            if (g.ServerShipArrangement.GetCellState(i, j) == CellStatе.WoundedShip)
                            {
                                fx = i;
                                fy = j;
                            }
                            else
                            {
                                j++;
                            }
                        }
                        i++;
                    }
                    if (fx == -1)
                    {
                        if (Will.Count == 0)
                        {
                            if (FindFourDestroyed(g.ServerShipArrangement))
                            {
                                if (SomethingExceptOne(g.ServerShipArrangement))
                                {
                                    int rem = 0;
                                    do
                                    {
                                        x = random.Next(10);
                                        y = random.Next(10);
                                        System.Math.DivRem(x + y, 2, out rem);
                                    }
                                    while ((rem == 1) || g.MakeAMove(x, y) == MoveResult.Error);
                                }
                                else
                                {
                                    do
                                    {
                                        x = random.Next(10);
                                        y = random.Next(10);
                                    }
                                    while (g.MakeAMove(x, y) == MoveResult.Error);
                                }
                            }
                            else
                            {
                                int k = 0;
                                int sum = 0;
                                do
                                {
                                    do
                                    {
                                        x = random.Next(10);
                                        double r = random.NextDouble();
                                        sum = r < 0.077 ? 0 : r < 0.308 ? 4 : r < 0.693 ? 8 : r < 0.924 ? 12 : 16;
                                        k++;
                                    }
                                    while (k < 52 && (sum + 8 - x < 0 || sum + 8 - x >= 10));
                                }
                                while (k < 52 && g.MakeAMove(x, sum + 8 - x) == MoveResult.Error);
                                if (k == 52)
                                    do
                                    {
                                        x = random.Next(10);
                                        y = random.Next(10);
                                    }
                                    while (g.MakeAMove(x, y) == MoveResult.Error);
                                else y = sum + 8 - x;
                            }
                        }
                        else
                        {
                            int n = random.Next(Will.Count);
                            Vector vector = Will[n];
                            Will.RemoveAt(n);
                            x = (int)vector.X;
                            y = (int)vector.Y;
                            g.MakeAMove(x, y);
                        }
                        break;
                    }
                    else
                    {
                        if (fx + 1 < 10 && g.ServerShipArrangement.GetCellState(fx + 1, fy) == CellStatе.WoundedShip)
                        {
                            int px = fx + 1;
                            while ((px < 10) && (g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedShip))
                            {
                                px++;
                            }
                            if (fx - 1 >= 0 && (px == 10) || (g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx - 1, fy);
                                x = fx - 1;
                                y = fy;
                            }
                            else
                            {
                                g.MakeAMove(px, fy);
                                x = px;
                                y = fy;
                            }
                        }
                        else if (fy + 1 < 10 && g.ServerShipArrangement.GetCellState(fx, fy + 1) == CellStatе.WoundedShip)
                        {
                            int py = fy + 1;
                            while ((py < 10) && (g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedShip))
                            {
                                py++;
                            }
                            if (fy - 1 >= 0 && (py == 10) || (g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy - 1);
                                x = fx;
                                y = fy - 1;
                            }
                            else
                            {
                                g.MakeAMove(fx, py);
                                x = fx;
                                y = py;
                            }
                        }
                        else
                        {
                            if ((fx + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx + 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx + 1, fy);
                                x = fx + 1;
                                y = fy;
                            }
                            else if ((fy + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx, fy + 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy + 1);
                                x = fx;
                                y = fy + 1;
                            }
                            else if ((fx - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx - 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx - 1, fy);
                                x = fx - 1;
                                y = fy;
                            }
                            else if ((fy - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx, fy - 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy - 1);
                                x = fx;
                                y = fy - 1;
                            }
                        }
                    }
                    break;
            }
            return new Point(x, y);
        }
    }
}