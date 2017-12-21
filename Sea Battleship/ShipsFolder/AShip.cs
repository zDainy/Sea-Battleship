using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sea_Battleship
{
    public abstract class AShip
    {
        int length;
        Image[] images;
        bool isHorizontal = true;
        bool isPlaced = false;
        public PlayField playField;
        public StackPanel ShipStackPanel;

        public Image[] Images { get => images; set => images = value; }
        public int Length { get => length; set => length = value; }
        public bool IsHorizontal { get => isHorizontal; set => isHorizontal = value; }
        public bool IsPlaced { get => isPlaced; set => isPlaced = value; }

        public void ChangeOrientation() //отлажено, меняет в том числе и флаг
        {
            if (isHorizontal)
            {
                IsHorizontal = false;
                for (int i = 0; i < length; i++)
                {
                    Images[i].LayoutTransform = new RotateTransform(90);
                }
            }
            else
            {
                IsHorizontal = true;
                for (int i = 0; i < length; i++)
                {
                    Images[i].LayoutTransform = new RotateTransform(0);
                }
            }
        }

        public void Place(Image sender, PlayField playField) //отлажено?
        {
            if (Ships.CheckCellToPlace(this, Grid.GetColumn(sender), Grid.GetRow(sender), playField.Ships))
            {
                //    if (IsPlaced)
                //    {
                //        for (int i = 0; i < length; i++)
                //        {
                //            PlayField.DeleteCell(Grid.GetColumn(Images[i]), Grid.GetRow(Images[i]), playField);
                //        }
                //    }
                //    else
                //    {
                //        if (!isHorizontal)
                //        {
                //            ChangeOrientation();
                //        }
                //    }
                IsPlaced = true;
                if (isHorizontal)
                {
                    this.playField = playField;
                    int x = Grid.GetColumn(sender);
                    if (x > 10 - length) x = 10 - length;
                    int y = Grid.GetRow(sender);
                    Grid grid = playField.FieldGrid;
                    for (int i = 0; i < length; i++)
                    {
                        PlayField.SetCell(x + i, y, grid, Images[i]);
                    }
                }
                else
                {
                    //ChangeOrientation();
                    int x = Grid.GetColumn(sender);
                    int y = Grid.GetRow(sender);
                    if (y > 10 - length) y = 10 - length;
                    Grid grid = (Grid)sender.Parent;
                    for (int i = 0; i < length; i++)
                    {
                        PlayField.SetCell(x, y + i, grid, Images[i]);
                    }
                }
            }
        }

        public void Place(int x, int y, PlayField playField) //отлажено?
        {
            if (Ships.CheckCellToPlace(this, x, y, playField.Ships))
            {
                //if (IsPlaced)
                //{
                //    for (int i = 0; i < length; i++)
                //    {
                //        PlayField.DeleteCell(Grid.GetColumn(Images[i]), Grid.GetRow(Images[i]), playField);
                //    }
                //}
                //else
                //{
                //    if (!isHorizontal)
                //    {
                //        ChangeOrientation();
                //    }
                //}
                IsPlaced = true;
                if (isHorizontal)
                {
                    this.playField = playField;
                    if (x > 10 - length) x = 10 - length;
                    Grid grid = playField.FieldGrid;
                    for (int i = 0; i < length; i++)
                    {
                        PlayField.SetCell(x + i, y, grid, Images[i]);
                    }
                }
                else
                {
                    if (y > 10 - length) y = 10 - length;
                    Grid grid = playField.FieldGrid;
                    for (int i = 0; i < length; i++)
                    {
                        PlayField.SetCell(x, y + i, grid, Images[i]);
                    }
                }
            }
        }

        public void DeleteShipFromField()
        {
            IsPlaced = false;
            for (int i = 0; i < Length; i++)
            {
                PlayField.DeleteCell(Grid.GetColumn(Images[i]), Grid.GetRow(Images[i]), playField);
            }
        }
    }
}
