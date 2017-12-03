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

        public PlayField()
        {
            InitializeComponent();
            for(int y = 0; y<10; y++)
                for(int x = 0; x<10; x++)
                {
                    Image img = new Image
                    {
                        Stretch = Stretch.Fill,
                        Source = new BitmapImage(new Uri("/Resources/no-audio.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                        LayoutTransform = new RotateTransform(90),
                    };
                    img.MouseLeftButtonUp += Up;
                    img.MouseLeftButtonDown += FieldCell_Click;
                    FieldGrid.Children.Add(img);
                    Grid.SetColumn(img, x);
                    Grid.SetRow(img, y);
                }
        }

        public void SetCell(int x, int y)
        {
            Image img = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/no-audio.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                LayoutTransform = new RotateTransform(90)             
            };
            FieldGrid.Children.Insert(10*y+x,img);
            Grid.SetRow(img, y);
            Grid.SetColumn(img, x);
        }

        private void FieldCell_Click(object sender, MouseButtonEventArgs e)
        {
            Image im = (Image)sender;
            int i = Grid.GetColumn(im);
            int j = Grid.GetRow(im);
            FieldGrid.Children.RemoveAt(10 * j + i);
            SetCell(i, j);
        }
        public static bool isis = false;
        private void Up(object sender, MouseButtonEventArgs e)
        {
            if(isis)
            {
                Image im = (Image)sender;
                im.Source = new BitmapImage(new Uri("/Resources/audio.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache };
                im.Opacity = 100;
            }
        }
    }
}
