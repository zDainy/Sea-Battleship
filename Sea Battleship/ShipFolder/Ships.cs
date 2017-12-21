using Sea_Battleship.ShipFolder;
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
        List<Ship1> _shipList1;
        List<Ship2> _shipList2;
        List<Ship4> _shipList4;
        List<Ship3> _shipList3;
        int _shipCount1 = 4;
        int _shipCount2 = 3;
        int _shipCount3 = 2;
        int _shipCount4 = 1;
        PlayField _playField;

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
        }

        public Ships(PlayField playField)
        {
            _playField = playField;
            ShipList3 = new List<Ship3>();
            ShipList4 = new List<Ship4>();
            for (int i = 0; i < _shipCount4; i++)
            {
                ShipList4.Add(new Ship4());
            }
            for (int i = 0; i < _shipCount3; i++)
            {
                ShipList3.Add(new Ship3());
            }
            //for (int i = 0; i < _shipCount2; i++)
            //{
            //    Ship2 ship = new Ship2();
            //    ShipList2.Add(ship);
            //}
            //for (int i = 0; i < _shipCount1; i++)
            //{
            //    ShipList1.Add(new Ship1());
            //}
        }


    }
}
