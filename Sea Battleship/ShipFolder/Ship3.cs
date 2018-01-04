using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    public class Ship3 : AShip
    {
        public Ship3()
        {
            Size = 3;
            CountAlive = 3;
            Images = new Image[Size];
            Images[0] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/1n2.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
            Images[1] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/2n2.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },

            };
            Images[2] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/3n2.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
        }

    }
}