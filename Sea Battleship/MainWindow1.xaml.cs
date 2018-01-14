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
using Common;
using Core;
using System.Threading;
using System.Windows.Threading;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для MainWindow1.xaml
    /// </summary>
    public partial class MainWindow1 : Window
    {
        private int x;

        public MainWindow1()
        {
            InitializeComponent();
            //  MainFrame.Content = new PlacingPage(null);
            MainFrame.Content = new MainPage();

           // MainFrame.Content = (new Uri(Properties.Resources.hello));
        //    wb.Navigate(new Uri(Properties.Resources.hello));

            LogService.Start();
            LogService.Trace("==============================================");
        }

        //private void audioChanged(object sender, RoutedEventArgs e)
        //{
        //    WindowConfig.AudioChanged((Image)sender);
        //}

        //private void Loading_Click(object sender, RoutedEventArgs e)
        //{
        //    //LoadingWindow lw = new LoadingWindow
        //    //{
        //    //    Owner = this
        //    //};
        //    //lw.Show();
        //    //Hide();
        //}

        //private void NewGame_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        //{
        //    WindowConfig.GameState = WindowConfig.State.Offline;
        //    ConfigOfflineWindow window = new ConfigOfflineWindow() { Owner = this};
        //    window.Show();
        //    Hide();
        //}

        //private void MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        System.Diagnostics.Process.Start("C:/Users/Пользователь/Desktop/Наиболее морской бой/Sea-Battleship/Sea Battleship/Resources/Spravka.html");
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Справка отсутствует");
        //    }
        //}

        //private void CreateLobby_Click(object sender, RoutedEventArgs e)
        //{
        //    WindowConfig.GameState = WindowConfig.State.Online;
        //    ConfigOnlineHostWindow window = new ConfigOnlineHostWindow();
        //    window.Owner = this;
        //    window.Show();
        //    this.Hide();
        //}



        //private void ConnetToLobbyItem_Click(object sender, RoutedEventArgs e)
        //{
        //    WindowConfig.GameState = WindowConfig.State.Online;
        //    ConfigOnlineNotHostWindow window = new ConfigOnlineNotHostWindow();
        //    window.Owner = this;
        //    window.Show();
        //    this.Hide();
        //}

        //private void AboutItem_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("Игру создали студенты группы 6403:\nКотов Алексей\nОнисич Степан\nШибаева Александра", "Об авторах", MessageBoxButton.OK);
            
        //}
        
    }
}
