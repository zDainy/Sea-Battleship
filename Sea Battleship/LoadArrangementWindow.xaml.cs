using Core;
using Sea_Battleship.Engine;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для LoadArrangementWindow.xaml
    /// </summary>
    public partial class LoadArrangementWindow : Window
    {
        private ShipArrangement _arrangementClient;
        private GameConfig _gameConfig;
        private OnlineGame _onlineGame;

        public LoadArrangementWindow(ShipArrangement arrangementClient, GameConfig gameConfig, OnlineGame onlineGame)
        {
            _arrangementClient = arrangementClient;
            _gameConfig = gameConfig;
            _onlineGame = onlineGame;
            InitializeComponent();
            List<string> list = FileSystem.SavedArrangementList();
            if (list != null)
            {
                int i = 0;
                Button button;
                foreach (string str in list)
                {
                    string[] tmp = str.Split('\\', '.');
                    string str1 = "";

                    str1 += tmp[1];

                    str1 = str1.TrimEnd('.');
                    button = new Button();
                    button.Content = str1;
                    button.Click += SaveButton_Click;
                    LoadGrid.Children.Add(button);
                    Grid.SetRow(button, i);
                    i++;
                }
            }
        } 


        private void RetButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ShipArrangement arr = FileSystem.LoadArrangement(((Button)sender).Content.ToString());
            WindowConfig.game = new Game(arr, _arrangementClient, _gameConfig);
            if (WindowConfig.GameState == WindowConfig.State.Offline)
            {
                PlayPage playPage = new PlayPage(WindowConfig.game);
                WindowConfig.NavigationService.Navigate(playPage, UriKind.Relative);
            }
            else
            {
                _onlineGame.CreateGame(arr);
                PlayPage page = new PlayPage(_onlineGame);
                WindowConfig.NavigationService.Navigate(page, UriKind.Relative);
            }
            Close();
        }
    }
}
