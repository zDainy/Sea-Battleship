using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship.ShipFolder
{
    public class Ship2:AShip
    {
        public Ship2()
        {
            Size = 2;
            CountAlive = 2;
            Images = new Image[Size];
            Images[0] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/1n3.png", UriKind.Relative))
                {
                    CreateOptions = BitmapCreateOptions.IgnoreImageCache
                },
            };
            Images[1] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/2n3.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
            };
        }
    }
}
