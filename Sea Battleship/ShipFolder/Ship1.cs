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
    public class Ship1 : AShip
    {
        public Ship1()
        {
            Size = 1;
            CountAlive = 1;
            Images = new Image[Size];
            Images[0] = new Image
            {
                Stretch = Stretch.Fill,
                Source = new BitmapImage(new Uri("/Resources/n4.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },

            };
        }
    }
}
