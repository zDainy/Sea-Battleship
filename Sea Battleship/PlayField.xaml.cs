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
            }
            isHiddenField = !isHiddenField;
          //  ships.ShipList4[0].Place(this, 0,0, true);

          //  ships.ShipList4[1].Place(this, 5, 5,false);
        }

        public Ships Ships { get => ships; set => ships = value; }
       // public bool IsHiddenField { get => isHiddenField; set => isHiddenField = value; }

        public void PlaceShips()
        {
            //
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
    }
}
