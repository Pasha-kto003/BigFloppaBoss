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
using System.Windows.Threading;

namespace WpfApp25
{
    /// <summary>
    /// Логика взаимодействия для Boss.xaml
    /// </summary>
    public partial class Boss : Window
    {
        bool goLeft, goRight;
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        List<Rectangle> bulletsForBossToRemove = new List<Rectangle>();
        int ImageOfBoss = 0;
        int bulletTimer = 0;
        int bulletTimerLimit = 90;
        int bossSpeed = 6;
        int totalBosses = 1;
        int bossHealth = 1000;
        bool gameOver = false;
        DispatcherTimer gameTimer = new DispatcherTimer();
        ImageBrush myCanvasSkin = new ImageBrush();
        ImageBrush playerSkin = new ImageBrush();

        public Boss()
        {
            InitializeComponent();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(30);
            gameTimer.Start();
            //gameTimer.Stop();
            playerSkin.ImageSource = new BitmapImage(new Uri("Images/MyShip_-3000.png", UriKind.Relative));
            myCanvas.Background = myCanvasSkin;
            myCanvasSkin.ImageSource = new BitmapImage(new Uri("Images/SpaceForGame.png", UriKind.Relative));
            player.Fill = playerSkin;
            myCanvas.Focus();
            MakeBoss(1);

        }

        private void GameLoop(object sender, EventArgs e)
        {
            Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            liveBoss.Content = "Осталось всего пришельцев: " + bossHealth;
            if (goLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - 10);
            }
            if (goRight == true && Canvas.GetLeft(player) + 80 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + 10);
            }
            bulletTimer -= 3;
            if (bulletTimer < 0)
            {
                BossBulletMaker(Canvas.GetLeft(player) + 20, 10);
                Random random = new Random();
                BossBulletMaker(random.Next(0, 500), random.Next(3, 10));
                BossBulletMaker(random.Next(0, 500), random.Next(3, 10));
                BossBulletMaker(random.Next(0, 500), random.Next(3, 10));
                BossBulletMaker(random.Next(0, 500), random.Next(3, 10));
                bulletTimer = bulletTimerLimit;
            }
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    foreach (var y in myCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "boss")
                        {
                            Rect bossHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bullet.IntersectsWith(bossHit))
                            {
                                itemsToRemove.Add(x);
                                itemsToRemove.Add(y);
                                bossHealth -= 10;
                            }

                        }
                    }
                }
                if (x is Rectangle && (string)x.Tag == "boss")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + bossSpeed);

                    if (Canvas.GetLeft(x) > 820)
                    {
                        Canvas.SetLeft(x, -80);
                        Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10));
                    }
                    Rect bossHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (playerHitBox.IntersectsWith(bossHitBox))
                    {
                        ShowGameOver("Пришельцы захватили мир!!");
                    }
                }
                if (x is Rectangle && (string)x.Tag == "bossBullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10); //скорость пули босса
                    if (Canvas.GetTop(x) > 480)
                    {
                        itemsToRemove.Add(x);
                    }
                    Rect bossBulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (playerHitBox.IntersectsWith(bossBulletHitBox))
                    {
                        ShowGameOver("Пришельцы испепелили вас!!");
                    }
                }
            }

            if (bossHealth < 900)
            {
                bossSpeed = 10;
            }
            if (bossHealth < 800)
            {
                bossSpeed = 12;
            }
            if (bossHealth < 700)
            {
                bossSpeed = 13;
            }
            if (bossHealth < 400)
            {
                bossSpeed = 14;
            }

            if (bossHealth < 1)
            {
                ShowGameOver("Вы прошли игру");
            }

            foreach (Rectangle i in itemsToRemove) //решить с ним проблему
            {
                if (bossHealth < 1)
                {
                    myCanvas.Children.Remove(i);
                }

            }
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }

            if (e.Key == Key.Right)
            {
                goRight = true;
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }

            if (e.Key == Key.Right)
            {
                goRight = false;
            }
            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle { Tag = "bullet", Height = 20, Width = 5, Fill = Brushes.Orange, Stroke = Brushes.Red };
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                myCanvas.Children.Add(newBullet);
            }
            if (e.Key == Key.Enter && gameOver == true)
            {
                Menu menu = new Menu();
                menu.Show();
                Close();
            }
        }
        private void BossBulletMaker(double x, double y)
        {
            Rectangle bossBullet = new Rectangle { Tag = "bossBullet", Height = 40, Width = 15, Fill = Brushes.Red, Stroke = Brushes.OrangeRed, StrokeThickness = 5 };
            Canvas.SetTop(bossBullet, y);
            Canvas.SetLeft(bossBullet, x);
            myCanvas.Children.Add(bossBullet);
        }

        private void MakeBoss(int limit)
        {
            int left = 0;
            totalBosses = limit;
            for (int i = 0; i < limit; i++)
            {
                ImageBrush bossSkin = new ImageBrush();
                Rectangle newBoss = new Rectangle { Tag = "boss", Height = 100, Width = 200, Fill = bossSkin };
                Canvas.SetTop(newBoss, 1);
                Canvas.SetLeft(newBoss, left);
                myCanvas.Children.Add(newBoss);
                left -= 1;
                bossSkin.ImageSource = new BitmapImage(new Uri("Images/bigfloppa.png", UriKind.Relative));
            }

        }
        private void ShowGameOver(string message)
        {
            
            gameOver = true;
            gameTimer.Stop();
            liveBoss.Content = " " + message + " Нажмите Enter чтобы снова играть";
        }
    }
}
