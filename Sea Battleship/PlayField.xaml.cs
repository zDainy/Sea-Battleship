using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sea_Battleship.ShipFolder;
using Core;
using Sea_Battleship.Engine;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlayField.xaml
    /// </summary>
    public partial class PlayField : UserControl
    {
        Ships ships;
        private static bool isHiddenField = true;

        public PlayField()
        {
            InitializeComponent();
            Ships = new Ships(this);
            if (isHiddenField)
            {
                ships.Init();
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
                // Core.ShipArrangement arr = new Core.ShipArrangement();
                //ShipArrangement arr =  ShipArrangement.Strategy();
                ShipArrangement arr = ShipArrangement.Strategy();
                PlaceFromMassive(arr.GetArrangement());

                //ships.ShipList4[0].Place(this, 0, 0, true);

                //ships.ShipList3[0].Place(this, 1, 1, false);

                //ships.ShipList2[0].Place(this, 5, 5, false);

                //ships.ShipList1[0].Place(this, 9, 9, true);
            }
            isHiddenField = !isHiddenField;
        }

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
            grid.Children.Insert(10*y+x,image);
            Grid.SetRow(image, y);
            Grid.SetColumn(image, x);
        }

        private void FieldCell_Click(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            int X = Grid.GetColumn(image);
            int Y = Grid.GetRow(image);
            SetCell(Grid.GetColumn(image), Grid.GetRow(image), FieldGrid, new Image()
            {
                Stretch = Stretch.Fill,
                Opacity = 100,
                Source = new BitmapImage(new Uri("/Resources/no-audio.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
            });
        }

        private void _SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FieldGrid.Width = FieldGrid.ActualHeight;
        }

        public void PlaceFromMassive(CellStatе[,] cells)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (cells[i, j] == CellStatе.Ship)
                        PluckShip(i,j,cells);
                }
            }
            PlaceShips();
        }

        private void PluckShip(int i, int j, CellStatе[,] cells)
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
    }
}
