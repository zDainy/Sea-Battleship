﻿using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sea_Battleship.Engine;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlacingWindow.xaml
    /// </summary>
    public partial class PlacingWindow : Window
    {
        private class ShipListItem
        {
            private StackPanel _shipPanel;
            private List<Point> _pointList;

            public ShipListItem()
            {

            }

            public StackPanel ShipPanel { get => _shipPanel; set => _shipPanel = value; }
            public List<Point> PointList { get => _pointList; set => _pointList = value; }
        }
        private double tmpX4 = 500;
        private double tmpY4 = 80;
        private double tmpX3 = 500;
        private double tmpY3 = 140;
        private double tmpX2 = 500;
        private double tmpY2 = 200;
        private double tmpX1 = 500;
        private double tmpY1 = 260;
        private StackPanel tmpShip;
        private List<ShipListItem> shipList = new List<ShipListItem>();
        private ShipArrangement _arrangementClient;
        private GameConfig _gameConfig;
        private OnlineGame _onlineGame;

        public PlacingWindow(ShipArrangement arrangementClient, GameConfig game)
        {
            _arrangementClient = arrangementClient;
            _gameConfig = game;
            InitializeComponent();
            Init();
        }

        public PlacingWindow(OnlineGame game)
        {
            _onlineGame = game;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Image img = new Image
                    {
                        Stretch = Stretch.Fill,
                        Source = new BitmapImage(new Uri("/Resources/Water.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache },
                        Opacity = 0
                    };
                    img.MouseLeftButtonUp += FieldCell_Click;
                    gr.Children.Add(img);
                    Grid.SetColumn(img, x);
                    Grid.SetRow(img, y);
                    Grid.SetZIndex(img, 1);
                }
            }
            foreach (Object ship in CurGrid.Children)
            {
                if (ship.GetType().ToString() == "System.Windows.Controls.StackPanel")
                    shipList.Add(new ShipListItem { ShipPanel = (StackPanel)ship });
            }
        }

        private bool CheckSell(int x, int y)
        {
            if (tmpShip.Orientation == Orientation.Horizontal)
            {
                for (int i = x - 1; i < x + tmpShip.Children.Count+1; i++)
                {
                    for (int j = y - 1; j < y + 2; j++)
                    {
                        foreach (ShipListItem sh in shipList)
                        {
                            if (i >= 0 && i < 10 && j >= 0 && j < 10 && sh.PointList != null && sh.PointList.Contains(new Point(i, j)))
                                return false;
                        }
                    }
                }
            }
            else
            {
                for (int i = y - 1; i < y + tmpShip.Children.Count+1; i++)
                {
                    for (int j = x - 1; j < x + 2; j++)
                    {
                        foreach (ShipListItem sh in shipList)
                        {
                            if (i >= 0 && i < 10 && j >= 0 && j < 10 && sh.PointList != null && sh.PointList.Contains(new Point(j, i)))
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        private void FieldCell_Click(object sender, MouseButtonEventArgs e)
        {
            if (Yes)
            {
                Yes = !Yes;
                Image image = (Image)sender;
                int X = Grid.GetColumn(image);
                int Y = Grid.GetRow(image);
                ShipListItem listItem = null;
                foreach (ShipListItem sh in shipList)
                {
                    if (sh.ShipPanel == tmpShip)
                    {
                        listItem = sh;
                        break;
                    }
                }
                Grid.SetZIndex(tmpShip, 1);
                Grid parent;
                switch (tmpShip.BindingGroup.Name)
                {
                    case "ship4":
                        if (tmpShip.Orientation == Orientation.Horizontal && X > 6) X = 6;
                        if (tmpShip.Orientation == Orientation.Vertical && Y > 6) Y = 6;
                        if (CheckSell(X, Y))
                        {
                            parent = (Grid)tmpShip.Parent;
                            parent.Children.Remove(tmpShip);
                            tmpShip.Margin = new Thickness();
                            gr.Children.Add(tmpShip);
                            Grid.SetColumn(tmpShip, X);
                            Grid.SetRow(tmpShip, Y);
                            if (tmpShip.Orientation == Orientation.Vertical)
                            {
                                Grid.SetRowSpan(tmpShip, 4);
                                listItem.PointList = new List<Point>();
                                listItem.PointList.Add(new Point(X, Y));
                                listItem.PointList.Add(new Point(X, Y + 1));
                                listItem.PointList.Add(new Point(X, Y + 2));
                                listItem.PointList.Add(new Point(X, Y + 3));
                            }
                            else
                            {
                                Grid.SetColumnSpan(tmpShip, 4);
                                listItem.PointList = new List<Point>();
                                listItem.PointList.Add(new Point(X, Y));
                                listItem.PointList.Add(new Point(X + 1, Y));
                                listItem.PointList.Add(new Point(X + 2, Y));
                                listItem.PointList.Add(new Point(X + 3, Y));
                            }
                        }
                        else
                        {
                            Grid.SetZIndex(tmpShip, 0);
                            if (tmpShip.Orientation == Orientation.Vertical)
                            {
                                ChangeOrientation();
                            }
                            tmpShip.Margin = new Thickness(tmpX4, tmpY4, 0, 0);
                        }
                        break;
                    case "ship3":
                        if (tmpShip.Orientation == Orientation.Horizontal && X > 7) X = 7;
                        if (tmpShip.Orientation == Orientation.Vertical && Y > 7) Y = 7;
                        if (CheckSell(X, Y))
                        {
                            parent = (Grid)tmpShip.Parent;
                            parent.Children.Remove(tmpShip);
                            tmpShip.Margin = new Thickness();
                            gr.Children.Add(tmpShip);
                            Grid.SetColumn(tmpShip, X);
                            Grid.SetRow(tmpShip, Y);
                            if (tmpShip.Orientation == Orientation.Vertical)
                            {
                                Grid.SetRowSpan(tmpShip, 3);
                                listItem.PointList = new List<Point>();
                                listItem.PointList.Add(new Point(X, Y));
                                listItem.PointList.Add(new Point(X, Y + 1));
                                listItem.PointList.Add(new Point(X, Y + 2));
                            }
                            else
                            {
                                Grid.SetColumnSpan(tmpShip, 3);
                                listItem.PointList = new List<Point>();
                                listItem.PointList.Add(new Point(X, Y));
                                listItem.PointList.Add(new Point(X + 1, Y));
                                listItem.PointList.Add(new Point(X + 2, Y));
                            }
                        }
                        else
                        {
                            Grid.SetZIndex(tmpShip, 0);
                            if (tmpShip.Orientation == Orientation.Vertical)
                            {
                                ChangeOrientation();
                            }
                            tmpShip.Margin = new Thickness(tmpX3, tmpY3, 0, 0);
                        }
                        break;
                    case "ship2":
                        if (tmpShip.Orientation == Orientation.Horizontal && X > 8) X = 8;
                        if (tmpShip.Orientation == Orientation.Vertical && Y > 8) Y = 8;
                        if (CheckSell(X, Y))
                        {
                            parent = (Grid)tmpShip.Parent;
                            parent.Children.Remove(tmpShip);
                            tmpShip.Margin = new Thickness();
                            gr.Children.Add(tmpShip);
                            Grid.SetColumn(tmpShip, X);
                            Grid.SetRow(tmpShip, Y);
                            if (tmpShip.Orientation == Orientation.Vertical)
                            {
                                Grid.SetRowSpan(tmpShip, 2);
                                listItem.PointList = new List<Point>();
                                listItem.PointList.Add(new Point(X, Y));
                                listItem.PointList.Add(new Point(X, Y + 1));
                            }
                            else
                            {
                                Grid.SetColumnSpan(tmpShip, 2);
                                listItem.PointList = new List<Point>();
                                listItem.PointList.Add(new Point(X, Y));
                                listItem.PointList.Add(new Point(X + 1, Y));
                            }
                        }
                        else
                        {
                            Grid.SetZIndex(tmpShip, 0);
                            if (tmpShip.Orientation == Orientation.Vertical)
                            {
                                ChangeOrientation();
                            }
                            tmpShip.Margin = new Thickness(tmpX2, tmpY2, 0, 0);
                        }
                        break;
                    case "ship1":
                        if (CheckSell(X, Y))
                        {
                            parent = (Grid)tmpShip.Parent;
                            parent.Children.Remove(tmpShip);
                            tmpShip.Margin = new Thickness();
                            gr.Children.Add(tmpShip);
                            Grid.SetColumn(tmpShip, X);
                            Grid.SetRow(tmpShip, Y);
                            listItem.PointList = new List<Point>();
                            listItem.PointList.Add(new Point(X, Y));
                        }
                        else
                        {
                            Grid.SetZIndex(tmpShip, 0);
                            if (tmpShip.Orientation == Orientation.Vertical)
                            {
                                ChangeOrientation();
                            }
                            tmpShip.Margin = new Thickness(tmpX1, tmpY1, 0, 0);
                        }
                        break;
                }
            }
        }

        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (Yes)
            {
                tmpShip.Margin = new Thickness(e.GetPosition(null).X-20, e.GetPosition(null).Y-20, 0, 0);
            }
        }

        private void ship_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Yes = !Yes;
            Grid.SetZIndex(tmpShip, 0);
            if (tmpShip.Orientation == Orientation.Vertical)
            {
                ChangeOrientation();
            }
            switch (tmpShip.BindingGroup.Name)
            {
                case "ship4":
                    tmpShip.Margin = new Thickness(tmpX4, tmpY4, 0, 0);
                    break;
                case "ship3":
                    tmpShip.Margin = new Thickness(tmpX3, tmpY3, 0, 0);
                    break;
                case "ship2":
                    tmpShip.Margin = new Thickness(tmpX2, tmpY2, 0, 0);
                    break;
                case "ship1":
                    tmpShip.Margin = new Thickness(tmpX1, tmpY1, 0, 0);
                    break;
            }
        }

        bool Yes = false;

        private void ship_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Yes = !Yes;
            StackPanel ship = (StackPanel)sender;
            tmpShip = ship;
            Grid.SetZIndex(tmpShip, 0);
            ShipListItem listItem = null;
            foreach (ShipListItem sh in shipList)
            {
                if (sh.ShipPanel == tmpShip)
                {
                    listItem = sh;
                    break;
                }
            }
            if (listItem != null)
                if (listItem.PointList != null)
                    listItem.PointList = null;
            if (ship.Parent == gr)
            {
                Grid.SetZIndex(ship, 0);
                gr.Children.Remove(ship);
                CurGrid.Children.Add(ship);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

        private void ChangeOrientation()
        {
            if (tmpShip.Orientation == Orientation.Horizontal)
            {
                tmpShip.Orientation = Orientation.Vertical;
                double tmp = tmpShip.Width;
                tmpShip.Width = tmpShip.Height;
                tmpShip.Height = tmp;
                foreach (Image img in tmpShip.Children)
                {
                    img.LayoutTransform = new RotateTransform(90);
                }
            }
            else
            {
                tmpShip.Orientation = Orientation.Horizontal;
                double tmp = tmpShip.Width;
                tmpShip.Width = tmpShip.Height;
                tmpShip.Height = tmp;
                foreach (Image img in tmpShip.Children)
                {
                    img.LayoutTransform = new RotateTransform(0);
                }
            }
        }

        private void ship_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(Yes)
            {
                ChangeOrientation();
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Grid)sender).Width = ((Grid)sender).ActualHeight * 632 / 449;
            gr.Width = gr.ActualHeight;
            foreach(ShipListItem sh in shipList)
            {
                if(sh.PointList==null)
                {
                    sh.ShipPanel.Margin = new Thickness(100+ gr.ActualHeight, sh.ShipPanel.Margin.Top, 0,0);
                }
            }
        }

        private bool Check(int x, int y)
        {
            foreach (ShipListItem sh in shipList)
            {
                if (sh.PointList.Contains(new Point(x, y)))
                    return true;
            }
            return false;
        }

        private ShipArrangement CreateShipArrangement()
        {
            ShipArrangement arr = new ShipArrangement();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Check(i, j))
                    {
                        arr.SetCellState(CellStatе.Ship, i, j);
                    }
                }
            }
            return arr;
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            ShipArrangement arr;
            try
            {
                if (!(_onlineGame is null))
                {
                    _onlineGame.CreateGame(CreateShipArrangement());
                    new PlayWindow(_onlineGame) { Owner = Owner }.Show();

                }
                else
                {
                    arr = CreateShipArrangement();
                    new PlayWindow(new Game(arr, _arrangementClient, _gameConfig)) { Owner = Owner }.Show();
                }
                Close();
            }
            catch
            {
                MessageBox.Show("Остались нерасставленные корабли");
            }
        }

        private void SaveArrItem_Click(object sender, RoutedEventArgs e)
        {
            ShipArrangement arr;
            try
            {
                arr = CreateShipArrangement();
                new SaveArrangementWindow(arr) { Owner = this}.Show();
            }
            catch
            {
                MessageBox.Show("Остались нерасставленные корабли");
            }
        }

        private void LoadArrItem_Click(object sender, RoutedEventArgs e)
        {
            new LoadArrangementWindow() { Owner = this}.Show();
        }

        public void ArrangeLoad(ShipArrangement arrangement)//доделать
        {
            foreach (ShipListItem ship in shipList)
            {
                if (ship.ShipPanel.Orientation == Orientation.Vertical)
                {
                    tmpShip = ship.ShipPanel;
                    ChangeOrientation();
                }
                Grid.SetZIndex(ship.ShipPanel, 0);
                ShipListItem listItem = null;
                foreach (ShipListItem sh in shipList)
                {
                    if (sh.ShipPanel == ship.ShipPanel)
                    {
                        listItem = sh;
                        break;
                    }
                }
                if (listItem != null)
                    if (listItem.PointList != null)
                        listItem.PointList = null;
                if (ship.ShipPanel.Parent == gr)
                {
                    Grid.SetZIndex(ship.ShipPanel, 0);
                    gr.Children.Remove(ship.ShipPanel);
                    CurGrid.Children.Add(ship.ShipPanel);
                }
            }

        }

        private void RuleItem_Click(object sender, RoutedEventArgs e)
        {
            ArrangeLoad(null);
        }
    }
}
