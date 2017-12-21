using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    public class Ships
    {
        List<Ship4> _shipList4;
        List<Ship3> _shipList3;
        int _shipCount3 = 2;
        int _shipCount4 = 1;
        PlayField _playField;

        public PlayField PlayField { get => _playField; set => _playField = value; }
        public int ShipCount3 { get => _shipCount3; set => _shipCount3 = value; }
        public int ShipCount4 { get => _shipCount4; set => _shipCount4 = value; }
        public List<Ship3> ShipList3 { get => _shipList3; set => _shipList3 = value; }
        public List<Ship4> ShipList4 { get => _shipList4; set => _shipList4 = value; }

        public Ships() //чтоб было
        {
        }

        public Ships(PlayField playField) //расширить
        {
            _playField = playField;
            ShipList3 = new List<Ship3>();
            ShipList4 = new List<Ship4>();
            for (int i = 0; i < _shipCount4; i++)
            {
                ShipList4.Add(new Ship4(_playField));

            }
            for (int i = 0; i < _shipCount3; i++)
            {
                ShipList3.Add(new Ship3(_playField));
            }
        }

        public static bool CheckCellToPlace(AShip ship, int x, int y, Ships list)
        {
            if (ship.Length == 1)
            {
                if (CheckSell(x + 1, y, list) && CheckSell(x + 1, y + 1, list) && CheckSell(x + 1, y - 1, list) && CheckSell(x, y + 1, list) && CheckSell(x, y - 1, list) && CheckSell(x - 1, y + 1, list) && CheckSell(x - 1, y, list) && CheckSell(x - 1, y - 1, list)) return true;
            }
            else if (ship.Length == 2)
            {
                if (ship.IsHorizontal)
                {
                    if (CheckSell(x + 2, y, list) && CheckSell(x + 2, y - 1, list) && CheckSell(x + 2, y + 1, list) && CheckSell(x + 1, y + 1, list) && CheckSell(x + 1, y - 1, list) && CheckSell(x, y + 1, list) && CheckSell(x, y - 1, list) && CheckSell(x - 1, y + 1, list) && CheckSell(x - 1, y, list) && CheckSell(x - 1, y - 1, list)) return true;
                }
                else
                {
                    if (CheckSell(x + 1, y, list) && CheckSell(x + 1, y + 1, list) && CheckSell(x + 1, y - 1, list) && CheckSell(x, y + 2, list) && CheckSell(x - 1, y + 2, list) && CheckSell(x + 1, y + 2, list) && CheckSell(x, y - 1, list) && CheckSell(x - 1, y + 1, list) && CheckSell(x - 1, y, list) && CheckSell(x - 1, y - 1, list)) return true;
                }
            }
            else if (ship.Length == 3)
            {
                if (ship.IsHorizontal)
                {
                    if (CheckSell(x + 3, y, list) && CheckSell(x + 3, y + 1, list) && CheckSell(x + 3, y - 1, list) && CheckSell(x + 2, y - 1, list) && CheckSell(x + 2, y + 1, list) && CheckSell(x + 1, y + 1, list) && CheckSell(x + 1, y - 1, list) && CheckSell(x, y + 1, list) && CheckSell(x, y - 1, list) && CheckSell(x - 1, y + 1, list) && CheckSell(x - 1, y, list) && CheckSell(x - 1, y - 1, list)) return true;
                }
                else
                {
                    if (CheckSell(x + 1, y, list) && CheckSell(x + 1, y + 1, list) && CheckSell(x + 1, y - 1, list) && CheckSell(x, y + 3, list) && CheckSell(x - 1, y + 3, list) && CheckSell(x + 1, y + 3, list) && CheckSell(x - 1, y + 2, list) && CheckSell(x + 1, y + 2, list) && CheckSell(x, y - 1, list) && CheckSell(x - 1, y + 1, list) && CheckSell(x - 1, y, list) && CheckSell(x - 1, y - 1, list)) return true;
                }
            }
            else if (ship.Length == 4)
            {
                if (ship.IsHorizontal)
                {
                    if (CheckSell(x + 4, y, list) && CheckSell(x + 4, y - 1, list) && CheckSell(x + 4, y + 1, list) && CheckSell(x + 3, y + 1, list) && CheckSell(x + 3, y - 1, list) && CheckSell(x + 2, y - 1, list) && CheckSell(x + 2, y + 1, list) && CheckSell(x + 1, y + 1, list) && CheckSell(x + 1, y - 1, list) && CheckSell(x, y + 1, list) && CheckSell(x, y - 1, list) && CheckSell(x - 1, y + 1, list) && CheckSell(x - 1, y, list) && CheckSell(x - 1, y - 1, list)) return true;
                }
                else
                {
                    if (CheckSell(x + 1, y, list) && CheckSell(x + 1, y + 1, list) && CheckSell(x + 1, y - 1, list) && CheckSell(x, y + 4, list) && CheckSell(x - 1, y + 4, list) && CheckSell(x + 1, y + 4, list) && CheckSell(x - 1, y + 3, list) && CheckSell(x + 1, y + 3, list) && CheckSell(x - 1, y + 2, list) && CheckSell(x + 1, y + 2, list) && CheckSell(x, y - 1, list) && CheckSell(x - 1, y + 1, list) && CheckSell(x - 1, y, list) && CheckSell(x - 1, y - 1, list)) return true;
                }
            }
            return false;
        } //отлажено. Страшно, но работает

        public static bool CheckSell(int x, int y, Ships list) //отлажено
        {
            if (x >= 0 && y >= 0 && x < 10 && y < 10)
            {
                foreach (AShip ship in list.ShipList4)
                {
                    if (ship.IsPlaced)
                        foreach (Image image in ship.Images)
                        {
                            if ((Grid.GetRow(image) == y) && (Grid.GetColumn(image) == x)) return false;
                        }
                }
                foreach (AShip ship in list.ShipList3)
                {
                    if (ship.IsPlaced)
                        foreach (Image image in ship.Images)
                        {
                            if ((Grid.GetRow(image) == y) && (Grid.GetColumn(image) == x)) return false;
                        }
                }
                //foreach (AShip ship in list.ShipList4)
                //{
                //    foreach (Image image in ship.Images)
                //    {
                //        if ((Grid.GetRow(image) == y) && (Grid.GetColumn(image) == x)) return false;
                //    }
                //}
                //foreach (AShip ship in list.ShipList4)
                //{
                //    foreach (Image image in ship.Images)
                //    {
                //        if ((Grid.GetRow(image) == y) && (Grid.GetColumn(image) == x)) return false;
                //    }
                //}
            }
            return true;
        }

    }
}
