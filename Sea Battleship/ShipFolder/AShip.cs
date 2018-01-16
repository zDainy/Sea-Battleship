using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    public abstract class AShip
    {
        int size;
        int countAlive;
        Image[] images;
        bool isHorizontal = true;
        int x = -1;
        int y = -1;
        bool isDead = false;

        public Image[] Images { get => images; set => images = value; }
        public bool IsHorizontal { get => isHorizontal; set => isHorizontal = value; }
        public int Size { get => size; set => size = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int CountAlive { get => countAlive; set => countAlive = value; }
        public bool IsDead { get => isDead; set => isDead = value; }

        public void Place(PlayField field, int x, int y, bool isHorizontal)
        {
            X = x;
            Y = y;
            IsHorizontal = isHorizontal;
            if (isHorizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    PlayField.SetCell(x + i, y, field.FieldGrid, Images[i]);
                }
            }
            else
            {
                ChangeOrientation();
                for (int i = 0; i < size; i++)
                {

                    PlayField.SetCell(x, y + i, field.FieldGrid, Images[i]);
                }
            }
        }

        public void Place(PlayField field)
        {
            if (IsHorizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    PlayField.SetCell(x + i, y, field.FieldGrid, Images[i]);
                }
            }
            else
            {
                ChangeOrientation();
                for (int i = 0; i < size; i++)
                {

                    PlayField.SetCell(x, y + i, field.FieldGrid, Images[i]);
                }
            }
        }

        private void ChangeOrientation()
        {
            for (int i = 0; i < size; i++)
            {
                Images[i].LayoutTransform = new RotateTransform(90);
            }
        }

        public bool isHere(Image image, PlayPage window, bool isOnline, out bool isDead)
        {
            isDead = false;
            if (!isOnline)
            {
                foreach (Image im in images)
                {
                    if (im == image)
                    {
                        countAlive--;
                        if (countAlive == 0)
                        {
                            this.isDead = true;
                            isDead = true;
                            if (isHorizontal)
                            {
                                for (int i = 0; i < size; i++)
                                    window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.DestroyedShip, x + i, y);
                                for (int i = X - 1; i < x + size + 1; i++)
                                {
                                    if (i >= 0 && i < 10 && y + 1 >= 0 && y + 1 < 10)
                                    {
                                        PlayField.SetCell(i, y + 1, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                        window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, i, y + 1);
                                    }
                                    if (i >= 0 && i < 10 && y - 1 >= 0 && y - 1 < 10)
                                    {
                                        PlayField.SetCell(i, y - 1, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                        window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, i, y - 1);
                                    }
                                }
                                if (x - 1 >= 0 && x - 1 < 10)
                                {
                                    PlayField.SetCell(x - 1, y, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, x - 1, y);
                                }
                                if (x + size >= 0 && x + size < 10)
                                {
                                    PlayField.SetCell(x + size, y, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, x + size, y);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < size; i++)
                                    window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.DestroyedShip, x, y + i);
                                for (int i = y - 1; i < y + size + 1; i++)
                                {
                                    if (i >= 0 && i < 10 && x + 1 >= 0 && x + 1 < 10)
                                    {
                                        PlayField.SetCell(x + 1, i, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                        window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, x + 1, i);
                                    }
                                    if (i >= 0 && i < 10 && x - 1 >= 0 && x - 1 < 10)
                                    {
                                        PlayField.SetCell(x - 1, i, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                        window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, x - 1, i);
                                    }
                                }
                                if (y - 1 >= 0 && y - 1 < 10)
                                {
                                    PlayField.SetCell(x, y - 1, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, x, y - 1);
                                }
                                if (y + size >= 0 && y + size < 10)
                                {
                                    PlayField.SetCell(x, y + size, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ServerShipArrangement.SetCellState(Core.CellStatе.WoundedWater, x, y + size);
                                }
                            }
                        }
                        return true;
                    }
                }
                return false;
            }
            else
            {
                foreach (Image im in images)
                {
                    if (im == image)
                    {
                        countAlive--;
                        if (countAlive == 0)
                        {
                            this.isDead = true;
                            isDead = true;
                            if (isHorizontal)
                            {
                                for (int i = X - 1; i < x + size + 1; i++)
                                {
                                    if (i >= 0 && i < 10 && y + 1 >= 0 && y + 1 < 10)
                                    {
                                        PlayField.SetCell(i, y + 1, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                    }
                                    if (i >= 0 && i < 10 && y - 1 >= 0 && y - 1 < 10)
                                    {
                                        PlayField.SetCell(i, y - 1, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                    }
                                }
                                if (x - 1 >= 0 && x - 1 < 10)
                                {
                                    PlayField.SetCell(x - 1, y, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                                if (x + size >= 0 && x + size < 10)
                                {
                                    PlayField.SetCell(x + size, y, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                            }
                            else
                            {
                                for (int i = y - 1; i < y + size + 1; i++)
                                {
                                    if (i >= 0 && i < 10 && x + 1 >= 0 && x + 1 < 10)
                                    {
                                        PlayField.SetCell(x + 1, i, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                    }
                                    if (i >= 0 && i < 10 && x - 1 >= 0 && x - 1 < 10)
                                    {
                                        PlayField.SetCell(x - 1, i, window.MyField.FieldGrid, new Image()
                                        {
                                            Stretch = Stretch.Fill,
                                            Opacity = 100,
                                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                        });
                                    }
                                }
                                if (y - 1 >= 0 && y - 1 < 10)
                                {
                                    PlayField.SetCell(x, y - 1, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                                if (y + size >= 0 && y + size < 10)
                                {
                                    PlayField.SetCell(x, y + size, window.MyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                            }
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        public bool isHere(int x, int y, PlayPage window, bool isOnline, out bool isDead)
        {
            isDead = false;
            if (!isOnline)
            {
                if (isHorizontal && (
                size == 1 && X == x && Y == y ||
                size == 2 && (X == x && Y == y || X + 1 == x && Y == y) ||
                size == 3 && (X == x && Y == y || X + 1 == x && Y == y || X + 2 == x && Y == y) ||
                size == 4 && (X == x && Y == y || X + 1 == x && Y == y || X + 2 == x && Y == y || X + 3 == x && Y == y)
                ) ||
                size == 2 && (X == x && Y == y || X == x && Y + 1 == y) ||
                size == 3 && (X == x && Y == y || X == x && Y + 1 == y || X == x && Y + 2 == y) ||
                size == 4 && (X == x && Y == y || X == x && Y + 1 == y || X == x && Y + 2 == y || X == x && Y + 3 == y)
                )
                {
                    countAlive--;
                    if (countAlive == 0)
                    {
                        this.isDead = true;
                        isDead = true;
                        if (isHorizontal)
                        {
                            for (int i = 0; i < size; i++)
                                window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.DestroyedShip, X + i, y);
                            for (int i = X - 1; i < X + size + 1; i++)
                            {
                                if (i >= 0 && i < 10 && Y + 1 >= 0 && Y + 1 < 10)
                                {
                                    PlayField.SetCell(i, Y + 1, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, i, Y + 1);
                                }
                                if (i >= 0 && i < 10 && Y - 1 >= 0 && Y - 1 < 10)
                                {
                                    PlayField.SetCell(i, Y - 1, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, i, Y - 1);
                                }
                            }
                            if (X - 1 >= 0 && X - 1 < 10)
                            {
                                PlayField.SetCell(X - 1, Y, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                                window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, X - 1, Y);
                            }
                            if (X + size >= 0 && X + size < 10)
                            {
                                PlayField.SetCell(X + size, Y, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                                window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, X + size, Y);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < size; i++)
                                window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.DestroyedShip, X, Y + i);
                            for (int i = Y - 1; i < Y + size + 1; i++)
                            {
                                if (i >= 0 && i < 10 && X + 1 >= 0 && X + 1 < 10)
                                {
                                    PlayField.SetCell(X + 1, i, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, X + 1, i);
                                }
                                if (i >= 0 && i < 10 && X - 1 >= 0 && X - 1 < 10)
                                {
                                    PlayField.SetCell(X - 1, i, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                    window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, X - 1, i);
                                }
                            }
                            if (Y - 1 >= 0 && Y - 1 < 10)
                            {
                                PlayField.SetCell(X, Y - 1, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                                window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, X, Y - 1);
                            }
                            if (Y + size >= 0 && Y + size < 10)
                            {
                                PlayField.SetCell(X, Y + size, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                                window.Game.ClientShipArrangement.SetCellState(Core.CellStatе.WoundedWater, X, Y + size);
                            }
                        }
                    }
                    return true;
                }

                return false;
            }
            else
            {
                if (isHorizontal && (
                size == 1 && X == x && Y == y ||
                size == 2 && (X == x && Y == y || X + 1 == x && Y == y) ||
                size == 3 && (X == x && Y == y || X + 1 == x && Y == y || X + 2 == x && Y == y) ||
                size == 4 && (X == x && Y == y || X + 1 == x && Y == y || X + 2 == x && Y == y || X + 3 == x && Y == y)
                ) ||
                size == 2 && (X == x && Y == y || X == x && Y + 1 == y) ||
                size == 3 && (X == x && Y == y || X == x && Y + 1 == y || X == x && Y + 2 == y) ||
                size == 4 && (X == x && Y == y || X == x && Y + 1 == y || X == x && Y + 2 == y || X == x && Y + 3 == y)
                )
                {
                    countAlive--;
                    if (countAlive == 0)
                    {
                        this.isDead = true;
                        isDead = true;
                        if (isHorizontal)
                        {
                            for (int i = X - 1; i < X + size + 1; i++)
                            {
                                if (i >= 0 && i < 10 && Y + 1 >= 0 && Y + 1 < 10)
                                {
                                    PlayField.SetCell(i, Y + 1, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                                if (i >= 0 && i < 10 && Y - 1 >= 0 && Y - 1 < 10)
                                {
                                    PlayField.SetCell(i, Y - 1, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                            }
                            if (X - 1 >= 0 && X - 1 < 10)
                            {
                                PlayField.SetCell(X - 1, Y, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                            }
                            if (X + size >= 0 && X + size < 10)
                            {
                                PlayField.SetCell(X + size, Y, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                            }
                        }
                        else
                        {
                            for (int i = Y - 1; i < Y + size + 1; i++)
                            {
                                if (i >= 0 && i < 10 && X + 1 >= 0 && X + 1 < 10)
                                {
                                    PlayField.SetCell(X + 1, i, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                                if (i >= 0 && i < 10 && X - 1 >= 0 && X - 1 < 10)
                                {
                                    PlayField.SetCell(X - 1, i, window.EnemyField.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                }
                            }
                            if (Y - 1 >= 0 && Y - 1 < 10)
                            {
                                PlayField.SetCell(X, Y - 1, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                            }
                            if (Y + size >= 0 && Y + size < 10)
                            {
                                PlayField.SetCell(X, Y + size, window.EnemyField.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                            }
                        }
                    }
                    return true;
                }

                return false;
            }
        }

        public void SetAroundDead(Grid grid)
        {
            if (countAlive == 0)
            {
                this.isDead = true;
                isDead = true;
                if (isHorizontal)
                {
                    for (int i = X - 1; i < X + size + 1; i++)
                    {
                        if (i >= 0 && i < 10 && Y + 1 >= 0 && Y + 1 < 10)
                        {
                            PlayField.SetCell(i, Y + 1, grid, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                        }
                        if (i >= 0 && i < 10 && Y - 1 >= 0 && Y - 1 < 10)
                        {
                            PlayField.SetCell(i, Y - 1, grid, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                        }
                    }
                    if (X - 1 >= 0 && X - 1 < 10)
                    {
                        PlayField.SetCell(X - 1, Y, grid, new Image()
                        {
                            Stretch = Stretch.Fill,
                            Opacity = 100,
                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        });
                    }
                    if (X + size >= 0 && X + size < 10)
                    {
                        PlayField.SetCell(X + size, Y, grid, new Image()
                        {
                            Stretch = Stretch.Fill,
                            Opacity = 100,
                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        });
                    }
                }
                else
                {
                    for (int i = Y - 1; i < Y + size + 1; i++)
                    {
                        if (i >= 0 && i < 10 && X + 1 >= 0 && X + 1 < 10)
                        {
                            PlayField.SetCell(X + 1, i, grid, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                        }
                        if (i >= 0 && i < 10 && X - 1 >= 0 && X - 1 < 10)
                        {
                            PlayField.SetCell(X - 1, i, grid, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                        }
                    }
                    if (Y - 1 >= 0 && Y - 1 < 10)
                    {
                        PlayField.SetCell(X, Y - 1, grid, new Image()
                        {
                            Stretch = Stretch.Fill,
                            Opacity = 100,
                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        });
                    }
                    if (Y + size >= 0 && Y + size < 10)
                    {
                        PlayField.SetCell(X, Y + size, grid, new Image()
                        {
                            Stretch = Stretch.Fill,
                            Opacity = 100,
                            Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        });
                    }
                }
            }
        }
    }
}