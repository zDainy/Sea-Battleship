using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    public class Ship4 : AShip
    {
        public Ship4()
        {
            Size = 4;
            CountAlive = 4;
            Images = new Image[Size];
            Images[0] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/1n.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
            Images[1] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/2n.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },

            };
            Images[2] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/3n.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
            Images[3] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/4n.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
        }
    }
}
