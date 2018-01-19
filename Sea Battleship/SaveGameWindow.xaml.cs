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
    /// Логика взаимодействия для SaveGameWindow.xaml
    /// </summary>
    public partial class SaveGameWindow : Window
    {
        public SaveGameWindow()
        {
            InitializeComponent();
        }

        private void RetButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxSave.Text != "")
            {
                if (FileSystem.GameExists(textBoxSave.Text))
                {
                    MessageBox.Show("Игра с таким названием существует,\nпожалуйста, введите другое название");
                }
                else
                {
                    if (WindowConfig.GameState == WindowConfig.State.Offline)
                    {
                        FileSystem.SaveGame(textBoxSave.Text, WindowConfig.game);
                    }
                    else
                    {
                        FileSystem.SaveGame(textBoxSave.Text, WindowConfig.OnlineGame.Game);
                    }
                    MessageBox.Show("Игра сохранена");
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Введите название игры");
            }
        }
    }
}
