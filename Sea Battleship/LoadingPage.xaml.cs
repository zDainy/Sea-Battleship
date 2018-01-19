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
    /// Логика взаимодействия для LoadingPage.xaml
    /// </summary>
    public partial class LoadingPage : Page
    {
        public int z;
        public LoadingPage()
        {
            InitializeComponent();
            WindowConfig.GetCurrentAudioImg(AudioImg);
            Label label;
            Button button;
            int i = 1;
            List<string> list = FileSystem.SavedGameList();
            if (list != null)
            {
                foreach (string str in list)
                {
                    string[] tmp = str.Split('\\', '.');
                    string str1 = "";

                    str1 += tmp[1];

                    str1 = str1.TrimEnd('.');
                    label = new Label();
                    label.Content = str1;
                    button = new Button();
                    button.Content = str1;  
                    button.Click += Button_Click;
                    LoadGrid.Children.Add(label);
                    LoadGrid.Children.Add(button);
                    Grid.SetRow(label, i);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(label, 0);
                    Grid.SetColumn(button, 0);
                    i++;
                }
            }
        }
        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);
        }

        private void BackClick_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game game = FileSystem.LoadGame(((Button)sender).Content.ToString());
            if (game.GameConfig.IsOnline)
            {
                OnlineGame onlineGame = new OnlineGame(PlayerRole.Server, PlacementState.Loaded);
                onlineGame.Game = game;
                WindowConfig.IsLoaded = true;
                WaitingWindow window = new WaitingWindow(onlineGame, null, PlacementState.Loaded);
                window.SetNavigationService(NavigationService);
                window.Show();
                window.Wait();
            }
            else
            {
                WindowConfig.game = game;
                WindowConfig.IsLoaded = true;
                PlayPage playPage = new PlayPage(WindowConfig.game);
                NavigationService.Navigate(playPage, UriKind.Relative);
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

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Игру создали студенты группы 6403:\nКотов Алексей\nОнисич Степан\nШибаева Александра", "Об авторах", MessageBoxButton.OK);
        }
    }
}
