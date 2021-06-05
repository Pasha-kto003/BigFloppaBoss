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

namespace WpfApp25
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        ImageBrush myGridSkin = new ImageBrush();

        public Menu()
        {
            InitializeComponent();
            mygrid.Background = myGridSkin;
            myGridSkin.ImageSource = new BitmapImage(new Uri("Images/SpaceForGame.png", UriKind.Relative));
            mygrid.Focus();
        }

        private void baton_Click(object sender, RoutedEventArgs e)
        {
            PlayWithFriend playWithFriend = new PlayWithFriend();
            playWithFriend.Show();
            Close();
        }

        private void SimpleGame_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void BossGame_Click(object sender, RoutedEventArgs e)
        {
            Boss boss = new Boss();
            boss.Show();
            Close();
        }
    }
}
