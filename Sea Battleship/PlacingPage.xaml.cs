using Core;
using Sea_Battleship.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Network;
using GameStatus = Core.GameStatus;
using ShipArrangement = Core.ShipArrangement;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlacingPage.xaml
    /// </summary>
    public partial class PlacingPage : Page
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
        private double tmpX4 = 582;
        private double tmpY4 = 80;
        private double tmpX3 = 582;
        private double tmpY3 = 140;
        private double tmpX2 = 582;
        private double tmpY2 = 200;
        private double tmpX1 = 582;
        private double tmpY1 = 260;
        private StackPanel tmpShip;
        private List<ShipListItem> shipList = new List<ShipListItem>();
        private ShipArrangement _arrangementClient;
        private GameConfig _gameConfig;
        private OnlineGame _onlineGame;

        public PlacingPage(ShipArrangement arrangementClient, GameConfig game)
        {
            WindowConfig.PlacingPage = this;
            ArrangementClient = arrangementClient;
            GameConfig = game;
            InitializeComponent();
            WindowConfig.GetCurrentAudioImg(AudioImg);
            Init();
        }

        public PlacingPage(OnlineGame game)
        {
            WindowConfig.PlacingPage = this;
            OnlineGame = game;
            InitializeComponent();
            //ExitButton.IsEnabled = false;
            WindowConfig.GetCurrentAudioImg(AudioImg);
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
                for (int i = x - 1; i < x + tmpShip.Children.Count + 1; i++)
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
                for (int i = y - 1; i < y + tmpShip.Children.Count + 1; i++)
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
                tmpShip.Margin = new Thickness(e.GetPosition(null).X-20 , e.GetPosition(null).Y-20, 0, 0);//!!
            }
        }

        private void ship_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Yes = false;
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

        public OnlineGame OnlineGame { get => _onlineGame; set => _onlineGame = value; }
        public GameConfig GameConfig { get => _gameConfig; set => _gameConfig = value; }
        public ShipArrangement ArrangementClient { get => _arrangementClient; set => _arrangementClient = value; }

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
            MessageBoxResult res = MessageBox.Show("Вы уверены, что хотите выйти в главное меню?", "", MessageBoxButton.YesNo);
            switch (res)
            {
                case MessageBoxResult.Yes:
                    Exit();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        public void Exit(bool lastPlayer = false)
        {
            if (_onlineGame != null)
            {
                if (OnlineGame.PlayerRole == PlayerRole.Server)
                {
                    OnlineGame.Connect.Server.SendResponse(OpearationTypes.GameStatus,
                        new Network.GameStatus(GameStatus.Break));
                    OnlineGame.Connect.Server.Stop();
                }
                else
                {
                    OnlineGame.Connect.Client.SendRequest(OpearationTypes.GameStatus,
                        new Network.GameStatus(GameStatus.Break));
                    OnlineGame.Connect.Client.Close();
                }
            }
            WindowConfig.OnlineGame = null;
            WindowConfig.game = null;
            WindowConfig.IsLoaded = false;
            NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
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
            if (Yes)
            {
                ChangeOrientation();
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((Grid)sender).Width = ((Grid)sender).ActualHeight * 632 / 449;
            gr.Width = gr.ActualHeight;
            foreach (ShipListItem sh in shipList)
            {
                if (sh.PointList == null)
                {
                    sh.ShipPanel.Margin = new Thickness(100 + gr.ActualHeight, sh.ShipPanel.Margin.Top, 0, 0);
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
                if (!(OnlineGame is null))
                {
                    OnlineGame.CreateGame(CreateShipArrangement());
                    // new PlayWindow(_onlineGame) { Owner = Owner }.Show();
                    PlayPage page = new PlayPage(OnlineGame);
                    NavigationService.Navigate(page, UriKind.Relative);
                }
                else
                {
                    arr = CreateShipArrangement();
                    WindowConfig.game = new Game(arr, ArrangementClient, GameConfig);
                    PlayPage page = new PlayPage(WindowConfig.game);
                    NavigationService.Navigate(page, UriKind.Relative);
                    //new PlayWindow(new Game(arr, _arrangementClient, _gameConfig)) { Owner = Owner }.Show();
                }
                // Close();
            }
            catch (CookieException)
            {
                MessageBox.Show("Нет подключения", "Соединение разорвано", MessageBoxButton.OK, MessageBoxImage.Warning);
                WindowConfig.OnlineGame = null;
                WindowConfig.game = null;
                NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
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
                new SaveArrangementWindow(arr).Show();
            }
            catch
            {
                MessageBox.Show("Остались нерасставленные корабли");
            }
        }

        private void LoadArrItem_Click(object sender, RoutedEventArgs e)
        {
            new LoadArrangementWindow(ArrangementClient, GameConfig, OnlineGame).Show();
        }

        public void ArrangeLoad(ShipArrangement arrangement)//доделать
        {
            CellStatе[,] cells = arrangement.GetArrangement();
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
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (cells[i, j] == CellStatе.Ship)
                        PluckShip(i, j, cells);
                }
            }
        }

        private void RuleItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Spravka.html");
            }
            catch
            {
                MessageBox.Show("Справка отсутствует");
            }
        }

        private void PluckShip(int i, int j, CellStatе[,] cells)
        {
            int len = 1;
            cells[i, j] = CellStatе.Water;
            if (i + 1 < 10 && cells[i + 1, j] == CellStatе.Ship)
            {
                len++;
                cells[i + 1, j] = CellStatе.Water;
                if (i + 2 < 10 && cells[i + 2, j] == CellStatе.Ship)
                {
                    len++;
                    cells[i + 2, j] = CellStatе.Water;
                    if (i + 3 < 10 && cells[i + 3, j] == CellStatе.Ship)
                    {
                        len++;
                        cells[i + 3, j] = CellStatе.Water;
                        ShipListItem listItem = null;
                        foreach (ShipListItem sh in shipList)
                        {
                            if (sh.ShipPanel.BindingGroup.Name == "ship4" && (Grid)sh.ShipPanel.Parent != gr)
                            {
                                listItem = sh;
                                break;
                            }
                        }
                        Grid.SetZIndex(listItem.ShipPanel, 1);
                        Grid parent;
                        parent = (Grid)listItem.ShipPanel.Parent;
                        parent.Children.Remove(listItem.ShipPanel);
                        listItem.ShipPanel.Margin = new Thickness();
                        gr.Children.Add(listItem.ShipPanel);
                        Grid.SetColumn(listItem.ShipPanel, i);
                        Grid.SetRow(listItem.ShipPanel, j);
                        //if (listItem.ShipPanel.Orientation == Orientation.Vertical)
                        //{
                        Grid.SetRowSpan(listItem.ShipPanel, 4);
                        listItem.PointList = new List<Point>();
                        listItem.PointList.Add(new Point(i, j));
                        listItem.PointList.Add(new Point(i, j + 1));
                        listItem.PointList.Add(new Point(i, j + 2));
                        listItem.PointList.Add(new Point(i, j + 3));
                        //else
                        //{
                        //    Grid.SetColumnSpan(listItem.ShipPanel, 4);
                        //    listItem.PointList = new List<Point>();
                        //    listItem.PointList.Add(new Point(i, j));
                        //    listItem.PointList.Add(new Point(i + 1, j));
                        //    listItem.PointList.Add(new Point(i + 2, j));
                        //    listItem.PointList.Add(new Point(i + 3, j));
                        //}

                    }
                    else
                    {
                        ShipListItem listItem = null;
                        foreach (ShipListItem sh in shipList)
                        {
                            if (sh.ShipPanel.BindingGroup.Name == "ship3" && (Grid)sh.ShipPanel.Parent != gr)
                            {
                                listItem = sh;
                                break;
                            }
                        }
                        Grid.SetZIndex(listItem.ShipPanel, 1);
                        Grid parent;
                        parent = (Grid)listItem.ShipPanel.Parent;
                        parent.Children.Remove(listItem.ShipPanel);
                        listItem.ShipPanel.Margin = new Thickness();
                        gr.Children.Add(listItem.ShipPanel);
                        Grid.SetColumn(listItem.ShipPanel, i);
                        Grid.SetRow(listItem.ShipPanel, j);
                        //if (listItem.ShipPanel.Orientation == Orientation.Vertical)
                        //{
                        Grid.SetRowSpan(listItem.ShipPanel, 3);
                        listItem.PointList = new List<Point>();
                        listItem.PointList.Add(new Point(i, j));
                        listItem.PointList.Add(new Point(i, j + 1));
                        listItem.PointList.Add(new Point(i, j + 2));
                    }
                }
                else
                {
                    ShipListItem listItem = null;
                    foreach (ShipListItem sh in shipList)
                    {
                        if (sh.ShipPanel.BindingGroup.Name == "ship2" && (Grid)sh.ShipPanel.Parent != gr)
                        {
                            listItem = sh;
                            break;
                        }
                    }
                    Grid.SetZIndex(listItem.ShipPanel, 1);
                    Grid parent;
                    parent = (Grid)listItem.ShipPanel.Parent;
                    parent.Children.Remove(listItem.ShipPanel);
                    listItem.ShipPanel.Margin = new Thickness();
                    gr.Children.Add(listItem.ShipPanel);
                    Grid.SetColumn(listItem.ShipPanel, i);
                    Grid.SetRow(listItem.ShipPanel, j);
                    //if (listItem.ShipPanel.Orientation == Orientation.Vertical)
                    //{
                    Grid.SetRowSpan(listItem.ShipPanel, 2);
                    listItem.PointList = new List<Point>();
                    listItem.PointList.Add(new Point(i, j));
                    listItem.PointList.Add(new Point(i, j + 1));
                }
            }
            else if (j + 1 < 10 && cells[i, j + 1] == CellStatе.Ship)
            {
                len++;
                cells[i, j + 1] = CellStatе.Water;
                if (j + 2 < 10 && cells[i, j + 2] == CellStatе.Ship)
                {
                    len++;
                    cells[i, j + 2] = CellStatе.Water;
                    if (j + 3 < 10 && cells[i, j + 3] == CellStatе.Ship)
                    {
                        len++;
                        cells[i, j + 3] = CellStatе.Water;
                        ShipListItem listItem = null;
                        foreach (ShipListItem sh in shipList)
                        {
                            if (sh.ShipPanel.BindingGroup.Name == "ship4" && (Grid)sh.ShipPanel.Parent != gr)
                            {
                                listItem = sh;
                                break;
                            }
                        }
                        Grid.SetZIndex(listItem.ShipPanel, 1);
                        Grid parent;
                        parent = (Grid)listItem.ShipPanel.Parent;
                        parent.Children.Remove(listItem.ShipPanel);
                        listItem.ShipPanel.Margin = new Thickness();
                        gr.Children.Add(listItem.ShipPanel);
                        Grid.SetColumn(listItem.ShipPanel, i);
                        Grid.SetRow(listItem.ShipPanel, j);
                        tmpShip = listItem.ShipPanel;
                        ChangeOrientation();
                        tmpShip = null;
                        Grid.SetColumnSpan(listItem.ShipPanel, 4);
                        listItem.PointList = new List<Point>();
                        listItem.PointList.Add(new Point(i, j));
                        listItem.PointList.Add(new Point(i + 1, j));
                        listItem.PointList.Add(new Point(i + 2, j));
                        listItem.PointList.Add(new Point(i + 3, j));
                    }
                    else
                    {
                        ShipListItem listItem = null;
                        foreach (ShipListItem sh in shipList)
                        {
                            if (sh.ShipPanel.BindingGroup.Name == "ship3" && (Grid)sh.ShipPanel.Parent != gr)
                            {
                                listItem = sh;
                                break;
                            }
                        }
                        Grid.SetZIndex(listItem.ShipPanel, 1);
                        Grid parent;
                        parent = (Grid)listItem.ShipPanel.Parent;
                        parent.Children.Remove(listItem.ShipPanel);
                        listItem.ShipPanel.Margin = new Thickness();
                        gr.Children.Add(listItem.ShipPanel);
                        Grid.SetColumn(listItem.ShipPanel, i);
                        Grid.SetRow(listItem.ShipPanel, j);
                        tmpShip = listItem.ShipPanel;
                        ChangeOrientation();
                        tmpShip = null;
                        Grid.SetColumnSpan(listItem.ShipPanel, 3);
                        listItem.PointList = new List<Point>();
                        listItem.PointList.Add(new Point(i, j));
                        listItem.PointList.Add(new Point(i + 1, j));
                        listItem.PointList.Add(new Point(i + 2, j));
                    }
                }
                else
                {
                    ShipListItem listItem = null;
                    foreach (ShipListItem sh in shipList)
                    {
                        if (sh.ShipPanel.BindingGroup.Name == "ship2" && (Grid)sh.ShipPanel.Parent != gr)
                        {
                            listItem = sh;
                            break;
                        }
                    }
                    Grid.SetZIndex(listItem.ShipPanel, 1);
                    Grid parent;
                    parent = (Grid)listItem.ShipPanel.Parent;
                    parent.Children.Remove(listItem.ShipPanel);
                    listItem.ShipPanel.Margin = new Thickness();
                    gr.Children.Add(listItem.ShipPanel);
                    Grid.SetColumn(listItem.ShipPanel, i);
                    Grid.SetRow(listItem.ShipPanel, j);
                    tmpShip = listItem.ShipPanel;
                    ChangeOrientation();
                    tmpShip = null;
                    Grid.SetColumnSpan(listItem.ShipPanel, 2);
                    listItem.PointList = new List<Point>();
                    listItem.PointList.Add(new Point(i, j));
                    listItem.PointList.Add(new Point(i + 1, j));
                }
            }
            else
            {
                ShipListItem listItem = null;
                foreach (ShipListItem sh in shipList)
                {
                    if (sh.ShipPanel.BindingGroup.Name == "ship1" && (Grid)sh.ShipPanel.Parent != gr)
                    {
                        listItem = sh;
                        break;
                    }
                }
                Grid.SetZIndex(listItem.ShipPanel, 1);
                Grid parent;
                parent = (Grid)listItem.ShipPanel.Parent;
                parent.Children.Remove(listItem.ShipPanel);
                listItem.ShipPanel.Margin = new Thickness();
                gr.Children.Add(listItem.ShipPanel);
                Grid.SetColumn(listItem.ShipPanel, i);
                Grid.SetRow(listItem.ShipPanel, j);
                listItem.PointList = new List<Point>();
                listItem.PointList.Add(new Point(i, j));
            }
        }

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Игру создали студенты группы 6403:\nКотов Алексей\nОнисич Степан\nШибаева Александра", "Об авторах", MessageBoxButton.OK);
        }
    }
}
