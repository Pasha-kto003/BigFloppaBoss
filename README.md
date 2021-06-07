 public partial class Boss : Window
    {
        bool goLeft, goRight;
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        int ImageOfBoss = 0;
        int bulletTimer = 0;
        int bulletTimerLimit = 90;
        int bossSpeed = 6;
        int totalBosses = 1;
        int totalShields = 2;
        int bossHealth = 1000;
        int shieldHealth = 200;
        bool gameOver = false;
        int shieldcount = 1;
        Rect hitbox;
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
            myCanvasSkin.ImageSource = new BitmapImage(new Uri("Images/BigFloppanew.png", UriKind.Relative));
            player.Fill = playerSkin;
            myCanvas.Background = myCanvasSkin;
            myCanvas.Focus(); 
            progres.Maximum = bossHealth;
            progres.Value = bossHealth;
            MakeBoss(1);
            progres.Maximum = bossHealth;
            progres.Value = bossHealth;
            
        }

        private void GameLoop(object sender, EventArgs e)
        {
            Rectangle newShield = MakeShield(2);
            if (newShield is Rectangle && (string)newShield.Tag == "shield")
            {
                Rect shieldHitBox = new Rect(Canvas.GetLeft(newShield), Canvas.GetTop(newShield), newShield.Width, newShield.Height);
                hitbox = shieldHitBox;
            }
                Rect playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            liveBoss.Content = "Осталось здоровья у босса: " + bossHealth;
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
                                if (bossHealth < 1)
                                {
                                    itemsToRemove.Add(y);
                                }
                                itemsToRemove.Add(x);
                                bossHealth -= 10;
                                progres.Value = bossHealth;
                            }

                        }
                    }
                }
                if (x is Rectangle && (string)x.Tag == "boss")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) + bossSpeed);                   
                    if (Canvas.GetLeft(x) > 820) // условие перемещения шлеппы
                    {
                        Canvas.SetLeft(x, -80);
                        //Canvas.SetTop(x, Canvas.GetTop(x) + (x.Height + 10));
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
                   
                        if (hitbox.IntersectsWith(bossBulletHitBox))
                        {
                            itemsToRemove.Add(x);
                            //myCanvas.Children.Remove(x);
                            if (shieldHealth < 1)
                            {
                                //myCanvas.Children.Remove(newShield);
                                itemsToRemove.Add(newShield);
                            }
                            shieldHealth -= 50;
                        }
                    
                    
                }
                //if (x is Rectangle && (string)x.Tag =="shield")
                //{
                //    Rect shieldHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                //    if (shieldHitBox.IntersectsWith(bossBulletHitBox)) // касние пули босса щита
                //    {
                //        MessageBox.Show("выафы");
                //    }
                //}
            }
            foreach(var x in itemsToRemove)
            {
                myCanvas.Children.Remove(x);
            }
            
            if (bossHealth < 900)
            {
                bossSpeed = 11;
            }
            if (bossHealth < 800)
            {
                bossSpeed = 12;
            }
            if (bossHealth < 700)
            {
                bossSpeed = 13;
                bulletTimer -= 3;
                if (bulletTimer < 0)
                {
                    Random random = new Random();
                    BossBulletMaker(random.Next(0, 130), random.Next(3, 10));
                    BossBulletMaker(random.Next(0, 280), random.Next(3, 10));
                    BossBulletMaker(random.Next(0, 300), random.Next(3, 10));
                    BossBulletMaker(random.Next(500, 740), random.Next(3, 10));
                    bulletTimer = bulletTimerLimit;
                }
            }
            if (bossHealth < 400)
            {
                bulletTimer -= 3;
                if (bulletTimer < 0)
                {
                    Random random = new Random();
                    BossBulletMaker(random.Next(100, 400), random.Next(3, 10));
                    BossBulletMaker(random.Next(150, 450), random.Next(3, 10));
                    BossBulletMaker(random.Next(180, 480), random.Next(3, 10));
                    BossBulletMaker(random.Next(250, 500), random.Next(3, 10));
                    BossBulletMaker(random.Next(300, 530), random.Next(3, 10));
                    BossBulletMaker(random.Next(320, 540), random.Next(3, 10));
                    BossBulletMaker(random.Next(500, 700), random.Next(3, 10));
                    bulletTimer = bulletTimerLimit;
                }
            }

            if (bossHealth < 1)
            {
                
                ShowGameOver("Вы прошли игру");
                MessageBox.Show("Вы прошли игру (Не один большой Шлеппа русский кот не пострадал)");
            }

            foreach (Rectangle i in itemsToRemove) //удаление Большого Шлеппы
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
                Rectangle newBullet2 = new Rectangle { Tag = "bullet", Height = 20, Width = 5, Fill = Brushes.Orange, Stroke = Brushes.Red };
                Canvas.SetTop(newBullet2, Canvas.GetTop(player) - newBullet2.Height);
                Canvas.SetLeft(newBullet2, Canvas.GetLeft(player) + player.Width / 2 + 10);
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2 - 10);
                myCanvas.Children.Add(newBullet);
                myCanvas.Children.Add(newBullet2);
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
            ImageBrush newBossBullet = new ImageBrush();
            Rectangle bossBullet = new Rectangle { Tag = "bossBullet", Height = 50, Width = 25, Fill = newBossBullet, StrokeThickness = 5, };
            Canvas.SetTop(bossBullet, y);
            Canvas.SetLeft(bossBullet, x);
            myCanvas.Children.Add(bossBullet);
            newBossBullet.ImageSource = new BitmapImage(new Uri("Images/BossBullet2.png", UriKind.Relative));
        }

        private void MakeBoss(int limit)
        {
            int left = 0;
            totalBosses = limit;
            for (int i = 0; i < limit; i++)
            {
                ImageBrush bossSkin = new ImageBrush();
                Rectangle newBoss = new Rectangle { Tag = "boss", Height = 100, Width = 200, Fill = bossSkin };
                Canvas.SetTop(newBoss, 50);
                Canvas.SetLeft(newBoss, left); //направление большого шлеппы влево
                myCanvas.Children.Add(newBoss);
                left -= 1;
                bossSkin.ImageSource = new BitmapImage(new Uri("Images/bigfloppa.png", UriKind.Relative));
            }
        }
        public Rectangle MakeShield(int limitshield) // попытка создать щит
        {
            
            if (shieldcount < limitshield)
            {
                Random random = new Random();
                double height = random.Next(200, 300);
                double width = random.Next(0, 700);
                Rectangle newShield = new Rectangle { Tag = "shield", Height = 30, Width = 75, Fill = Brushes.Red, Stroke = Brushes.Black };
                Canvas.SetTop(newShield, height);
                Canvas.SetLeft(newShield, width);
                myCanvas.Children.Add(newShield);
                shieldcount++;
                return newShield;
            }
            Rectangle rectangle = new Rectangle { Tag = "shield1", Height = 0, Width = 0, Fill = Brushes.Transparent, Stroke = Brushes.Transparent };
            Canvas.SetTop(rectangle, 0);
            Canvas.SetLeft(rectangle, 0);
            myCanvas.Children.Add(rectangle);
            return rectangle;
        }
        private void ShowGameOver(string message)
        {            
            gameOver = true;
            gameTimer.Stop();
            liveBoss.Content = " " + message + " Нажмите Enter чтобы снова играть";
        }
    }
}
