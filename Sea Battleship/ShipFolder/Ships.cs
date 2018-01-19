using Sea_Battleship.ShipFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace Sea_Battleship
{
    public class Ships
    {
        List<Ship1> _shipList1;
        List<Ship2> _shipList2;
        List<Ship4> _shipList4;
        List<Ship3> _shipList3;
        int _shipCount1 = 4;
        int _shipCount2 = 3;
        int _shipCount3 = 2;
        int _shipCount4 = 1;
        PlayField _playField;
        public int IsDeadCount { get; set; }

        public PlayField PlayField { get => _playField; set => _playField = value; }
        public int ShipCount3 { get => _shipCount3; set => _shipCount3 = value; }
        public int ShipCount4 { get => _shipCount4; set => _shipCount4 = value; }
        public int ShipCount1 { get => _shipCount1; set => _shipCount1 = value; }
        public int ShipCount2 { get => _shipCount2; set => _shipCount2 = value; }
        public List<Ship3> ShipList3 { get => _shipList3; set => _shipList3 = value; }
        public List<Ship4> ShipList4 { get => _shipList4; set => _shipList4 = value; }
        public List<Ship1> ShipList1 { get => _shipList1; set => _shipList1 = value; }
        public List<Ship2> ShipList2 { get => _shipList2; set => _shipList2 = value; }


        public Ships()
        {
            IsDeadCount = 0;
        }

        public Ships(PlayField playField)
        {
            _playField = playField;
            ShipList1 = new List<Ship1>();
            ShipList2 = new List<Ship2>();
            ShipList3 = new List<Ship3>();
            ShipList4 = new List<Ship4>();
        }

        public void Init()
        {
            for (int i = 0; i < _shipCount4; i++)
            {
                ShipList4.Add(new Ship4());
            }
            for (int i = 0; i < _shipCount3; i++)
            {
                ShipList3.Add(new Ship3());
            }
            for (int i = 0; i < _shipCount2; i++)
            {
                Ship2 ship = new Ship2();
                ShipList2.Add(ship);
            }
            for (int i = 0; i < _shipCount1; i++)
            {
                ShipList1.Add(new Ship1());
            }
        }

        public void PlaceAll()
        {
            foreach (AShip ship in ShipList1)
            {
                ship.Place(PlayField);
            }
            foreach (AShip ship in ShipList2)
            {
                ship.Place(PlayField);
            }
            foreach (AShip ship in ShipList3)
            {
                ship.Place(PlayField);
            }
            foreach (AShip ship in ShipList4)
            {
                ship.Place(PlayField);
            }
        }

        public bool Check(int X, int Y, PlayPage z, bool isOnline)
        {
            bool isDead = false;
            bool was = false;

            foreach (AShip sh in ShipList1)
            {
                if (sh.isHere(X, Y, z, isOnline, out isDead))
                {
                    was = true;
                    break;
                }
            }
            if (!was)
                foreach (AShip sh in ShipList2)
                {
                    if (sh.isHere(X, Y, z, isOnline, out isDead))
                    {
                        was = true;
                        break;
                    }
                }
            if (!was)
                foreach (AShip sh in ShipList3)
                {
                    if (sh.isHere(X, Y, z, isOnline, out isDead))
                    {
                        break;
                    }
                }
            if (!was)
                foreach (AShip sh in ShipList4)
                {
                    if (sh.isHere(X, Y, z, isOnline, out isDead))
                    {
                        was = true;
                        break;
                    }
                }
            if (isDead)
                IsDeadCount++;
            return was;
        }

        public bool Check(Image im, PlayPage z, bool isOnline)
        {
            bool isDead = false;
            bool was = false;
            int X = Grid.GetColumn(im);
            int Y = Grid.GetRow(im);
            foreach (AShip sh in ShipList1)
            {
                if (sh.isHere(im, z, isOnline, out isDead))
                {
                    was = true;
                    break;
                }
            }
            if (!was)
                foreach (AShip sh in ShipList2)
                {
                    if (sh.isHere(im, z, isOnline, out isDead))
                    {
                        was = true;
                        break;
                    }
                }
            if (!was)
                foreach (AShip sh in ShipList3)
                {
                    if (sh.isHere(im, z, isOnline, out isDead))
                    {
                        was = true;
                        break;
                    }
                }
            if (!was)
                foreach (AShip sh in ShipList4)
                {
                    if (sh.isHere(im, z, isOnline, out isDead))
                    {
                        was = true;
                        break;
                    }
                }
            if (isDead)
                IsDeadCount++;
            return was;
        }

        public void CheckEnemy(Point p, PlayPage z, bool isOnline)
        {
            bool isDead = false;
            Image image = (Image)z.MyField.FieldGrid.Children[10 * (int)p.Y + (int)p.X];
            bool was = false;
            foreach (AShip sh in z.MyField.Ships.ShipList1)
            {
                if (sh.isHere(image, z, isOnline, out isDead))
                {
                    z.MyField.ShipHitted(image);
                    //PlayField.SetCell((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                    //{
                    //    Stretch = Stretch.Fill,
                    //    Opacity = 100,
                    //    Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                    //});
                    was = true;
                    break;
                }
            }
            if (!was)
                foreach (AShip sh in z.MyField.Ships.ShipList2)
                {
                    if (sh.isHere(image, z, isOnline, out isDead))
                    {
                        z.MyField.ShipHitted(image);
                        //PlayField.SetCell((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                        //{
                        //    Stretch = Stretch.Fill,
                        //    Opacity = 100,
                        //    Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        //});
                        was = true;
                        break;
                    }
                }
            if (!was)
                foreach (AShip sh in z.MyField.Ships.ShipList3)
                {
                    if (sh.isHere(image, z, isOnline, out isDead))
                    {
                        z.MyField.ShipHitted(image);
                        //PlayField.SetCell((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                        //{
                        //    Stretch = Stretch.Fill,
                        //    Opacity = 100,
                        //    Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        //});
                        was = true;
                        break;
                    }
                }
            if (!was)
                foreach (AShip sh in z.MyField.Ships.ShipList4)
                {
                    if (sh.isHere(image, z, isOnline, out isDead))
                    {
                        z.MyField.ShipHitted(image);
                        //PlayField.SetCell((int)p.X, (int)p.Y, z.MyField.FieldGrid, new Image()
                        //{
                        //    Stretch = Stretch.Fill,
                        //    Opacity = 100,
                        //    Source = new BitmapImage(new Uri("/Resources/shipCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                        //});
                        was = true;
                        break;
                    }
                }

        }

        public bool IsAllDead()
        {
            int shipCount = 0;
            int count = 0;
            foreach (AShip sh in ShipList1)
            {
                shipCount++;
                if (sh.IsDead)
                    count++;
            }
            foreach (AShip sh in ShipList2)
            {
                shipCount++;
                if (sh.IsDead)
                    count++;
            }
            foreach (AShip sh in ShipList3)
            {
                shipCount++;
                if (sh.IsDead)
                    count++;
            }
            foreach (AShip sh in ShipList4)
            {
                shipCount++;
                if (sh.IsDead)
                    count++;
            }
            if (count == shipCount) return true;
            return false;
        }

        public void SetAroundDead()
        {
            Grid z =PlayField.FieldGrid; 
            foreach (AShip ship in ShipList1)
            {
                ship.SetAroundDead(z);
            }
            foreach (AShip ship in ShipList2)
            {
                ship.SetAroundDead(z);
            }
            foreach (AShip ship in ShipList3)
            {
                ship.SetAroundDead(z);
            }
            foreach (AShip ship in ShipList4)
            {
                ship.SetAroundDead(z);
            }
        }
    }
}