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

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlayField.xaml
    /// </summary>
    public partial class PlayField : UserControl
    {
        Ships ships;
        bool[,] _possibilityToPlace;

        public PlayField()
        {
            InitializeComponent();
            PossibilityToPlace = new bool[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    PossibilityToPlace[i, j] = true;
            Ships = new Ships(this, PossibilityToPlace);
            for(int y = 0; y<10; y++)
                for(int x = 0; x<10; x++)
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

        public bool[,] PossibilityToPlace { get => _possibilityToPlace; set => _possibilityToPlace = value; }
        public Ships Ships { get => ships; set => ships = value; }

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
            if (WindowConfig.ShipState == WindowConfig.State.Ship4)
            {
                WindowConfig.ShipState = WindowConfig.State.None;
                foreach (Ship4 ship in Ships.ShipList4)
                    if (!ship.IsPlaced)
                    {
                        ship.Place((Image)sender, this);
                        break;
                    }
            }
            else if (WindowConfig.ShipState == WindowConfig.State.Ship3)
            {
                WindowConfig.ShipState = WindowConfig.State.None;
                foreach (Ship3 ship in Ships.ShipList3)
                    if (!ship.IsPlaced)
                    {
                        ship.Place((Image)sender, this);
                        break;
                    }
            }
        }

        private void _SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FieldGrid.Width = FieldGrid.ActualHeight;
        }
    }
}
