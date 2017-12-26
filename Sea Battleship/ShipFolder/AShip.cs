using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    public abstract class AShip
    {
        int size;
        Image[] images;
        bool isHorizontal = true;
        int x = -1;
        int y = -1;
        bool iDead = false;

        public Image[] Images { get => images; set => images = value; }
        public bool IsHorizontal { get => isHorizontal; set => isHorizontal = value; }
        public int Size { get => size; set => size = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public void ShipInit()
        {
            for (int i = 0; i < Size; i++)
            {
                Images[i].MouseLeftButtonDown += Ship_MouseLeftButtonDown;
            }
        }

        private void Ship_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //
        }

       public void Place(PlayField field, int x, int y, bool isHorizontal)
        {
            X = x;
            Y = y;
            IsHorizontal = isHorizontal;
            if (isHorizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    PlayField.SetCell(x + i, y, field.FieldGrid, Images[i]);
                }
            }
            else
            {
                ChangeOrientation();
                for (int i = 0; i < size; i++)
                {

                    PlayField.SetCell(x, y + i, field.FieldGrid, Images[i]);
                }
            }
        }

        public void Place(PlayField field)
        {
            if (IsHorizontal)
            {
                for (int i = 0; i < size; i++)
                {
                    PlayField.SetCell(x + i, y, field.FieldGrid, Images[i]);
                }
            }
            else
            {
                ChangeOrientation();
                for (int i = 0; i < size; i++)
                {

                    PlayField.SetCell(x, y + i, field.FieldGrid, Images[i]);
                }
            }
        }

        private void ChangeOrientation()
        {
            for (int i = 0; i < size; i++)
            {
                Images[i].LayoutTransform = new RotateTransform(90);
            }
        }
    }
}
