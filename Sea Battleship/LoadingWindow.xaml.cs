using Core;
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

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public int z;
        public LoadingWindow()
        {
            InitializeComponent();
            Label label;
            Button button;
            int i = 1;
            List<string> list =  FileSystem.SavedGameList();
            if(list!=null)
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
            Owner.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game game = FileSystem.LoadGame(((Button)sender).Content.ToString());
            WindowConfig.game = game;
            WindowConfig.IsLoaded = true;
            new PlayWindow(game) { Owner = Owner}.Show();
            Close();
        }
    }
}
