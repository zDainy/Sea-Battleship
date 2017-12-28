using System;
using System.Windows;
using Timer = System.Windows.Forms.Timer;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Sea_Battleship.ShipFolder;
using Core;
using Sea_Battleship.Engine;
using System.Threading;
using Common;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlayField.xaml
    /// </summary>
    public partial class PlayField : UserControl
    {
        private PlayWindow _pw;

        Ships ships;
        private static bool isHiddenField = true;
        private OnlineGame _onlineGame;
        private bool _isOnlineGame;
        private Timer _turnTimer;


        public PlayField()
        {
            InitializeComponent();
            Game gg = WindowConfig.game;
            _onlineGame = WindowConfig.OnlineGame;
            CellStatе[,] myArr;
            CellStatе[,] enemyArr;

            if (!(_onlineGame is null))
            {
                _isOnlineGame = true;
                gg = _onlineGame.Game;
                _turnTimer = new Timer { Interval = (int)_onlineGame.GameConfig.GameSpeed };
                if (_onlineGame.PlayerRole == PlayerRole.Client)
                {
                    myArr = _onlineGame.MyArrangement.GetArrangement();
                    enemyArr = _onlineGame.EnemyArrangement.GetArrangement();
                    ThreadPool.QueueUserWorkItem(OnlineEnemyTurn);
                }
                else
                {
                    myArr = gg.ServerShipArrangement.GetArrangement();
                    enemyArr = gg.ClientShipArrangement.GetArrangement();
                }
            }
            else
            {
                enemyArr = gg.ClientShipArrangement.GetArrangement();
                myArr = gg.ServerShipArrangement.GetArrangement();
            }

            Ships = new Ships(this);
            if (isHiddenField)
            {
                //  ships.Init();
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        Image img = new Image
                        {
                            Stretch = Stretch.Fill,
                            Source = new BitmapImage(new Uri("/Resources/Water.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                            Opacity = 0
                        };
                        img.MouseLeftButtonDown += FieldCell_Click;
                        FieldGrid.Children.Add(img);
                        Grid.SetColumn(img, x);
                        Grid.SetRow(img, y);
                    }
                }
                CellStatе[,] arr = enemyArr;
                CellStatе[,] copyarr = new CellStatе[10, 10];
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        copyarr[i, j] = arr[i, j];
                    }
                }
                PlaceFromMassive(copyarr, Ships, false);
            }
            else
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        Image img = new Image
                        {
                            Stretch = Stretch.Fill,
                            Source = new BitmapImage(new Uri("/Resources/Water.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                            Opacity = 0
                        };
                        //img.MouseLeftButtonDown += FieldCell_Click;
                        FieldGrid.Children.Add(img);
                        Grid.SetColumn(img, x);
                        Grid.SetRow(img, y);
                    }

                }
                CellStatе[,] arr = myArr;
                CellStatе[,] copyarr = new CellStatе[10, 10];
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        copyarr[i, j] = arr[i, j];
                    }
                }
                PlaceFromMassive(copyarr, ships, true);
                // ShipArrangement arr = ShipArrangement.Strategy();
                // PlaceFromMassive(arr.GetArrangement());

                //ships.ShipList4[0].Place(this, 0, 0, true);

                //ships.ShipList3[0].Place(this, 1, 1, false);

                //ships.ShipList2[0].Place(this, 5, 5, false);

                //ships.ShipList1[0].Place(this, 9, 9, true);
            }
            isHiddenField = !isHiddenField;
            //   _turnTimer.Tick += (sender, e) => SwitchTurn();
           //  _turnTimer.Start();
        }

        public void SwitchTurn()
        {
           // _turnTimer.Stop();
            if (_onlineGame.IsMyTurn)
            {
                // <--- Переключалка хода
                _onlineGame.IsMyTurn = false;
                _onlineGame.IsOne = false;
                WindowConfig.PlayWindowCon.Dispatcher.Invoke(() =>
                {
                    //_pw.EnemyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E")); //(Brush)new BrushConverter().ConvertFromString("#FF00287E");
                    WindowConfig.PlayWindowCon.MyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E"));
                });
                // _pw.EnemyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E")); //(Brush)new BrushConverter().ConvertFromString("#FF00287E");
                // _pw.MyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF98ECFF")); //(Brush)new BrushConverter().ConvertFromString("#FF98ECFF");
                ThreadPool.QueueUserWorkItem(OnlineEnemyTurn);
            }
            else
            {
                // <--- Переключалка хода
                WindowConfig.PlayWindowCon.Dispatcher.Invoke(() =>
                {
                    // _pw.EnemyTurnLabel.Background =  new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF98ECFF"));// (Brush)new BrushConverter().ConvertFromString("#FF98ECFFFF00287E");
                    WindowConfig.PlayWindowCon.MyTurnLabel.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF93FF3A"));
                });

                //Dispatcher.Invoke( _pw.EnemyTurnLabel.Background = new SolidColorBrush(Colors.Red));// new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF98ECFF"));// (Brush)new BrushConverter().ConvertFromString("#FF98ECFFFF00287E");
                //_pw.MyTurnLabel.Background = new SolidColorBrush(Colors.Red);// new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00287E")); //(Brush)new BrushConverter().ConvertFromString("#FF00287E");
                _onlineGame.IsMyTurn = true;
            }

           // _turnTimer.Start();
        }
            //  _pw//
        

        public Ships Ships { get => ships; set => ships = value; }

        // public bool IsHiddenField { get => isHiddenField; set => isHiddenField = value; }

        public void PlaceShips()
        {
            ships.PlaceAll();
        }

        public static void DeleteCell(int x, int y, PlayField playField)
        {
            Image image = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/Water.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                Opacity = 0
            };
            image.MouseLeftButtonDown += playField.FieldCell_Click;
            playField.FieldGrid.Children.RemoveAt(10 * y + x);
            playField.FieldGrid.Children.Insert(10 * y + x, image);
            Grid.SetRow(image, y);
            Grid.SetColumn(image, x);
        }

        public static void SetCell(int x, int y, Grid grid, Image image)
        {
            grid.Children.RemoveAt(10 * y + x);
            grid.Children.Insert(10 * y + x, image);
            Grid.SetRow(image, y);
            Grid.SetColumn(image, x);
        }

        private void SetShotOnField(int x, int y, CellStatе state, bool onMyField)
        {
            _pw = (PlayWindow)((Grid)Parent).Parent;
            var fg = onMyField ? _pw.MyField.FieldGrid : _pw.EnemyField.FieldGrid;
            //string uri = state == CellStatе.WoundedShip ? "/Resources/shipCrushed.png" : "/Resources/waterCrushed.png";
            if (onMyField)
            {
                fg.Dispatcher.Invoke(() =>
                {
                    switch (state)
                    {
                        case CellStatе.WoundedShip:
                            _pw.MyField.Ships.CheckEnemy(new Point(x, y), _pw, true);
                            break;
                        case CellStatе.WoundedWater:
                            SetCell(x, y, fg, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                            break;
                    }
                });
            }
            else
            {
                fg.Dispatcher.Invoke(() =>
                {
                    switch (state)
                    {
                        case CellStatе.WoundedShip:
                            ships.Check(x, y, _pw, true);
                            break;
                        case CellStatе.WoundedWater:
                            SetCell(x, y, fg, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                            break;
                    }
                });
            }
        }


        private void OnlineMyTurn(object obj)
        {
            Vector vect = (Vector)obj;
            try
            {
                var shotRes = _onlineGame.Turn((int)vect.X, (int)vect.Y);
                SetShotOnField((int)vect.X, (int)vect.Y, shotRes, false);
                if (shotRes == CellStatе.WoundedWater)
                {
                    SwitchTurn();
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Соединение разорвано", "Противник отключился", MessageBoxButton.OK, MessageBoxImage.Warning);
                LogService.Trace("Противник отключился");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
                LogService.Trace($"Ошибка соединения: {e.Message}");
                OnlineMyTurn(vect);
            }
        }

        private void OnlineEnemyTurn(object obj)
        {
            try
            {
                if (!_onlineGame.IsOne)
                {
                    CellStatе shotRes;
                    _onlineGame.IsOne = true;
                    do
                    {
                        var comeVector = _onlineGame.WaitEnemyTurn();
                        shotRes = _onlineGame.CheckShot(comeVector);
                        SetShotOnField((int)comeVector.X, (int)comeVector.Y, shotRes, true);
                    } while (shotRes == CellStatе.WoundedShip);
                    SwitchTurn();
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Соединение разорвано", "Противник отключился", MessageBoxButton.OK, MessageBoxImage.Warning);
                LogService.Trace("Противник отключился");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка соединения", MessageBoxButton.OK, MessageBoxImage.Error);
                LogService.Trace($"Ошибка соединения: {e.Message}");
                OnlineEnemyTurn(null);
            }
        }

        private void FieldCell_Click(object sender, MouseButtonEventArgs e)
        {
            PlayWindow z = (PlayWindow)((Grid)Parent).Parent;
            Image image = (Image)sender;
            int X = Grid.GetColumn(image);
            int Y = Grid.GetRow(image);
            string uriString = "";
            if (_isOnlineGame && _onlineGame.IsMyTurn)
            {
                ThreadPool.QueueUserWorkItem(OnlineMyTurn, new Vector(X, Y));
            }
            else if (!_isOnlineGame)
            {
                MoveResult result = z.Game.MakeAMove(X, Y); //ход первого игрока
                switch (result)
                {
                    case MoveResult.Hit:
                        ships.Check(X, Y, z, false);
                        break;
                    case MoveResult.Miss:
                        uriString = "/Resources/waterCrushed.png";
                        SetCell(Grid.GetColumn(image), Grid.GetRow(image), FieldGrid, new Image()
                        {
                            Stretch = Stretch.Fill,
                            Opacity = 100,
                            Source = new BitmapImage(new Uri(uriString, UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        });
                        break;
                    case MoveResult.Destroyed:
                        break;
                    case MoveResult.Error:
                        break;
                }
                if (result != MoveResult.Hit) //если не попал, ход второго игрока
                {
                    EnemyStep(z);
                }

            }
        }

        private void EnemyStep(PlayWindow z)
        {
            Image image;
            Point p = AI.MakeAMove(z.Game);
            bool was = false;
            do //ходит, пока не промахнётся
            {
                image = (Image)z.MyField.FieldGrid.Children[10 * (int)p.Y + (int)p.X];
                was = false;
                foreach (AShip sh in z.MyField.ships.ShipList1)
                {
                    if (sh.isHere(image, z, false))
                    {
                        SetShotOnField((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                        {
                            Stretch = Stretch.Fill,
                            Opacity = 100,
                            Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        });
                        was = true;
                        break;
                    }
                }
                if (!was)
                    foreach (AShip sh in z.MyField.ships.ShipList2)
                    {
                        if (sh.isHere(image, z, false))
                        {
                            SetShotOnField((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                            was = true;
                            break;
                        }
                    }
                if (!was)
                    foreach (AShip sh in z.MyField.ships.ShipList3)
                    {
                        if (sh.isHere(image, z, false))
                        {
                            SetShotOnField((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                            was = true;
                            break;
                        }
                    }
                if (!was)
                    foreach (AShip sh in z.MyField.ships.ShipList4)
                    {
                        if (sh.isHere(image, z, false))
                        {
                            SetShotOnField((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                            {
                                Stretch = Stretch.Fill,
                                Opacity = 100,
                                Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                            });
                            was = true;
                            break;
                        }
                    }
                if (was)
                {
                    p = AI.MakeAMove(z.Game);
                }
            }
            while (was);
            if (!was)
            {
                SetShotOnField((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                {
                    Stretch = Stretch.Fill,
                    Opacity = 100,
                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                });
            }
        }



        private void _SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FieldGrid.Width = FieldGrid.ActualHeight;
        }

        public void PlaceFromMassive(CellStatе[,] cells, Ships ships, bool isPlace)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (cells[i, j] == CellStatе.Ship)
                        PluckShip(i, j, cells, ships);
                }
            }
            if (isPlace)
                PlaceShips();
        }

        private void PluckShip(int i, int j, CellStatе[,] cells, Ships Ships)
        {
            int len = 1;
            cells[i, j] = CellStatе.Water;
            if (i + 1 < 10 && cells[i + 1, j] == CellStatе.Ship)
            {
                len++;
                cells[i + 1, j] = CellStatе.Water;
                if (i + 2 < 10 && cells[i + 2, j] == CellStatе.Ship)
                {
                    len++;
                    cells[i + 2, j] = CellStatе.Water;
                    if (i + 3 < 10 && cells[i + 3, j] == CellStatе.Ship)
                    {
                        len++;
                        cells[i + 3, j] = CellStatе.Water;
                        Ships.ShipList4.Add(new Ship4()
                        {
                            IsHorizontal = true,
                            X = i,
                            Y = j,
                            Size = len,
                        });
                    }
                    else
                    {
                        Ships.ShipList3.Add(new Ship3()
                        {
                            IsHorizontal = true,
                            X = i,
                            Y = j,
                            Size = len,
                        });
                    }
                }
                else
                {
                    Ships.ShipList2.Add(new Ship2()
                    {
                        IsHorizontal = true,
                        X = i,
                        Y = j,
                        Size = len,
                    });
                }
            }
            else if (j + 1 < 10 && cells[i, j + 1] == CellStatе.Ship)
            {
                len++;
                cells[i, j + 1] = CellStatе.Water;
                if (j + 2 < 10 && cells[i, j + 2] == CellStatе.Ship)
                {
                    len++;
                    cells[i, j + 2] = CellStatе.Water;
                    if (j + 3 < 10 && cells[i, j + 3] == CellStatе.Ship)
                    {
                        len++;
                        cells[i, j + 3] = CellStatе.Water;
                        Ships.ShipList4.Add(new Ship4()
                        {
                            IsHorizontal = false,
                            X = i,
                            Y = j,
                            Size = len,
                        });
                    }
                    else
                    {
                        Ships.ShipList3.Add(new Ship3()
                        {
                            IsHorizontal = false,
                            X = i,
                            Y = j,
                            Size = len,
                        });
                    }
                }
                else
                {
                    Ships.ShipList2.Add(new Ship2()
                    {
                        IsHorizontal = false,
                        X = i,
                        Y = j,
                        Size = len,
                    });
                }
            }
            else
            {
                Ships.ShipList1.Add(new Ship1()
                {
                    IsHorizontal = true,
                    X = i,
                    Y = j,
                    Size = len,
                });
            }
        }

       // bool isTimer = false;
        private void SetShotOnField(int x, int y, Grid grid, Image image)
        {
            // _pw = (PlayWindow)((Grid)Parent).Parent;
            // Grid fg = onMyField ? _pw.MyField.FieldGrid : _pw.EnemyField.FieldGrid;
            grid.Dispatcher.Invoke(() =>
            {
                SetCell(x, y, grid, image);
            });
        }
    }
}
