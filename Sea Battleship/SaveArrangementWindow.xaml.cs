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
    /// Логика взаимодействия для SaveArrangementWindow.xaml
    /// </summary>
    public partial class SaveArrangementWindow : Window
    {
        ShipArrangement arrangement;
        public SaveArrangementWindow(ShipArrangement arr)
        {
            InitializeComponent();
            arrangement = arr;
        }

        private void RetButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowConfig.GameState == WindowConfig.State.Offline)
            {                
                if (textBoxSave.Text != "")
                {
                    if (FileSystem.ArrangementExists(textBoxSave.Text))
                    {
                        MessageBox.Show("Расстановка с таким названием существует,\nпожалуйста, введите другое название");
                    }
                    else
                    {
                        FileSystem.SaveArrangement(textBoxSave.Text, arrangement);
                        MessageBox.Show("Расстановка сохранена");
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Введите название расстановки");
                }
            }
        }
    }
}
