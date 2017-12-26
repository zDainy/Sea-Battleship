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
        int countAlive;
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
        public int CountAlive { get => countAlive; set => countAlive = value; }

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

        public bool isHere(Image image, PlayField field) //ДА ЧТО ЗА НАХУЙ
        {
            foreach (Image im in images)
            {
                if (im == image)
                {
                    countAlive--;
                    if (countAlive == 0)
                    {
                        if (isHorizontal)
                        {
                            for (int i = X - 1; i < x + size + 1; i++)
                            {
                                if (i >= 0 && i < 10 && y + 1 >= 0 && y + 1 < 10)
                                    PlayField.SetCell(i, y + 1, field.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                if (i >= 0 && i < 10 && y - 1 >= 0 && y - 1 < 10)
                                    PlayField.SetCell(i, y - 1, field.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                            }
                            if (x - 1 >= 0 && x - 1 < 10)
                                PlayField.SetCell(x - 1, y, field.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                            if (x + size >= 0 && x + size < 10)
                                PlayField.SetCell(x + size, y, field.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                        }
                        else
                        {
                            for (int i = y - 1; i < y + size + 1; i++)
                            {
                                if (i >= 0 && i < 10 && x + 1 >= 0 && x + 1 < 10)
                                    PlayField.SetCell(x + 1, i, field.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                                if (i >= 0 && i < 10 && x - 1 >= 0 && x - 1 < 10)
                                    PlayField.SetCell(x - 1, i, field.FieldGrid, new Image()
                                    {
                                        Stretch = Stretch.Fill,
                                        Opacity = 100,
                                        Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                    });
                            }
                            if (y - 1 >= 0 && y - 1 < 10)
                                PlayField.SetCell(x, y - 1, field.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                            if (y + size >= 0 && y + size < 10)
                                PlayField.SetCell(x, y + size, field.FieldGrid, new Image()
                                {
                                    Stretch = Stretch.Fill,
                                    Opacity = 100,
                                    Source = new BitmapImage(new Uri("/Resources/waterCrushed.png", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache }
                                });
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
