using System.Windows;
namespace Core
{
    public static class AI
    {
        private static System.Random random = new System.Random();

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

        public static Point MakeAMove(Game g)
        {
            int x = 0;
            int y = 0;
            switch (g.GameConfig.BotLvl)
            {
                case BotLevels.Easy:
                    do
                    {
                        x = random.Next(10);
                        y = random.Next(10);
                    }
                    while (g.MakeAMove(x, y) == MoveResult.Error);
                    break;
                case BotLevels.Medium:
                    int fx = -1;
                    int fy = -1;
                    int i = 0;
                    int j = 0;
                    while ((i < 10) && (fx == -1))
                    {
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
                        do
                        {
                            x = random.Next(10);
                            y = random.Next(10);
                        }
                        while (g.MakeAMove(x, y) == MoveResult.Error);
                    }
                    else
                    {
                        if (g.ServerShipArrangement.GetCellState(fx + 1, fy) == CellStatе.WoundedShip)
                        {
                            int px = fx + 1;
                            while ((g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedShip) && (px < 10))
                            {
                                px++;
                            }
                            if ((px == 10) || (g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedWater)) g.MakeAMove(fx - 1, fy);
                            else g.MakeAMove(px, fy);
                        }
                        else if (g.ServerShipArrangement.GetCellState(fx, fy + 1) == CellStatе.WoundedShip)
                        {
                            int py = fy + 1;
                            while ((g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedShip) && (py < 10))
                            {
                                py++;
                            }
                            if ((py == 10) || (g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedWater)) g.MakeAMove(fx, fy - 1);
                            else g.MakeAMove(fx, py);
                        }
                        else
                        {
                            if ((fx + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx + 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx + 1, fy);
                            }
                            else if ((fy + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx, fy + 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy + 1);
                            }
                            else if ((fx - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx - 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx - 1, fy);
                            }
                            else if ((fy - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx, fy - 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy - 1);
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
                            int sum = 0;
                            do
                            {
                                x = random.Next(10);
                                double r = random.NextDouble();
                                sum = r < 0.2 ? 0 : r < 0.4 ? 4 : r < 0.6 ? 8 : r < 0.8 ? 12 : 16;
                            }
                            while (((sum + 8 - x >= 10) || (sum + 8 - x < 0)) || (g.MakeAMove(x, sum + 8 - x) == MoveResult.Error));
                        }
                    }
                    else
                    {
                        if (g.ServerShipArrangement.GetCellState(fx + 1, fy) == CellStatе.WoundedShip)
                        {
                            int px = fx + 1;
                            while ((g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedShip) && (px < 10))
                            {
                                px++;
                            }
                            if ((px == 10) || (g.ServerShipArrangement.GetCellState(px, fy) == CellStatе.WoundedWater)) g.MakeAMove(fx - 1, fy);
                            else g.MakeAMove(px, fy);
                        }
                        else if (g.ServerShipArrangement.GetCellState(fx, fy + 1) == CellStatе.WoundedShip)
                        {
                            int py = fy + 1;
                            while ((g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedShip) && (py < 10))
                            {
                                py++;
                            }
                            if ((py == 10) || (g.ServerShipArrangement.GetCellState(fx, py) == CellStatе.WoundedWater)) g.MakeAMove(fx, fy - 1);
                            else g.MakeAMove(fx, py);
                        }
                        else
                        {
                            if ((fx + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx + 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx + 1, fy);
                            }
                            else if ((fy + 1 < 10) && (g.ServerShipArrangement.GetCellState(fx, fy + 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy + 1);
                            }
                            else if ((fx - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx - 1, fy) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx - 1, fy);
                            }
                            else if ((fy - 1 >= 0) && (g.ServerShipArrangement.GetCellState(fx, fy - 1) != CellStatе.WoundedWater))
                            {
                                g.MakeAMove(fx, fy - 1);
                            }
                        }
                    }
                    break;
            }
            return new Point(x, y);
        }
    }
}
