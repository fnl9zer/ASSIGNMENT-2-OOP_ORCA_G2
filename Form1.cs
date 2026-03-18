using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace SPACESHOOTER_ORCA
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootMedia;
        WindowsMediaPlayer explosion;

        PictureBox[] enemiesAttacks;
        int enemiesAttacksSpeed;

        PictureBox[] stars;
        int playerSpeed;

        PictureBox[] attacks;
        int attackSpeed;

        PictureBox[] enemies;
        int enemySpeed;

        PictureBox[] obstacles;
        int obstacleSpeed;

        int backgroundSpeed;
        Random rand;

        int score;
        int level;
        int dificulty;
        bool pause;
        bool gameOver;

        int playerHealth;
        int playerMaxHealth;
        int playerLives;
        PictureBox[] hearts;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pause = false;
            gameOver = false;
            score = 0;
            level = 1;
            dificulty = 9;

            playerHealth = 100;
            playerMaxHealth = 100;
            playerLives = 3;

            ReplayBtn.Visible = false;
            ExitBtn.Visible = false;
            lbl1.Visible = false;

            scorelbl.Text = "Score: 00";
            levellbl.Text = "Level: 1";

            backgroundSpeed = 4;
            playerSpeed = 4;
            enemySpeed = 4;
            attackSpeed = 20;
            enemiesAttacksSpeed = 4;
            obstacleSpeed = 3;

            attacks = new PictureBox[3];

            //load images
            Image munition = Image.FromFile(@"asserts\munition.png");
            Image heartImg = Image.FromFile(@"asserts\heart.png");
            Image meteor = Image.FromFile(@"asserts\meteor.png");
            Image flamingMeteor = Image.FromFile(@"asserts\flaming_meteor.png");

            Image enemy1 = Image.FromFile("asserts\\E1.png");
            Image enemy2 = Image.FromFile("asserts\\E2.png");
            Image enemy3 = Image.FromFile("asserts\\E3.png");
            Image boss1 = Image.FromFile("asserts\\Boss1.png");
            Image boss2 = Image.FromFile("asserts\\Boss2.png");

            enemies = new PictureBox[10];

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }

            enemies[0].Image = boss1;
            enemies[1].Image = enemy2;
            enemies[2].Image = enemy3;
            enemies[3].Image = enemy3;
            enemies[4].Image = enemy1;
            enemies[5].Image = enemy3;
            enemies[6].Image = enemy2;
            enemies[7].Image = enemy3;
            enemies[8].Image = enemy2;
            enemies[9].Image = boss2;

            for (int i = 0; i < attacks.Length; i++)
            {
                attacks[i] = new PictureBox();
                attacks[i].Size = new Size(8, 8);
                attacks[i].Image = munition;
                attacks[i].SizeMode = PictureBoxSizeMode.Zoom;
                attacks[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(attacks[i]);
            }

            obstacles = new PictureBox[6];
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i] = new PictureBox();
                obstacles[i].Size = new Size(35, 35);
                obstacles[i].SizeMode = PictureBoxSizeMode.Zoom;
                obstacles[i].BorderStyle = BorderStyle.None;
                obstacles[i].BackColor = Color.Transparent;
                obstacles[i].Visible = false;
                obstacles[i].Image = (i % 2 == 0) ? meteor : flamingMeteor;
                obstacles[i].Tag = (i % 2 == 0) ? "basic" : "flaming";
                obstacles[i].Location = new Point((i + 1) * 80, -50);
                this.Controls.Add(obstacles[i]);
            }

            hearts = new PictureBox[3];
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i] = new PictureBox();
                hearts[i].Size = new Size(30, 30);
                hearts[i].SizeMode = PictureBoxSizeMode.Zoom;
                hearts[i].BackColor = Color.Transparent;
                hearts[i].BorderStyle = BorderStyle.None;
                hearts[i].Image = heartImg;
                hearts[i].Location = new Point(this.Width - 110 + i * 35, this.Height - 60);
                hearts[i].Visible = true;
                this.Controls.Add(hearts[i]);
            }

            //create WMP
            gameMedia = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //load audio/bg
            gameMedia.URL = "songs\\GameSong.mp3";
            shootMedia.URL = "songs\\shoot.mp3";
            explosion.URL = "songs\\boom.mp3";

            //settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootMedia.settings.volume = 1;
            explosion.settings.volume = 6;

            stars = new PictureBox[15];
            rand = new Random();

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rand.Next(20, 580), rand.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }
                this.Controls.Add(stars[i]);
            }

            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].Location = new Point(rand.Next(20, 560), rand.Next(-300, -30));
            }

            enemiesAttacks = new PictureBox[10];

            for (int i = 0; i < enemiesAttacks.Length; i++)
            {
                enemiesAttacks[i] = new PictureBox();
                enemiesAttacks[i].Size = new Size(2, 25);
                enemiesAttacks[i].Visible = false;
                enemiesAttacks[i].BackColor = Color.Yellow;
                int x = rand.Next(0, 10);
                enemiesAttacks[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemiesAttacks[i]);
            }

            gameMedia.controls.play();
        }

        private void TakeDamage(int damage)
        {
            playerHealth -= damage;

            if (playerHealth > 66)
            {
                hearts[0].Visible = true;
                hearts[1].Visible = true;
                hearts[2].Visible = true;
            }
            else if (playerHealth > 33)
            {
                hearts[2].Visible = false;
                hearts[1].Visible = true;
                hearts[0].Visible = true;
            }
            else if (playerHealth > 0)
            {
                hearts[2].Visible = false;
                hearts[1].Visible = false;
                hearts[0].Visible = true;
            }
            else
            {
                hearts[2].Visible = false;
                hearts[1].Visible = false;
                hearts[0].Visible = false;
                Player.Visible = false;
                GameOver("Game Over");
            }
        }

        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                stars[i].Top += backgroundSpeed;
                if (stars[i].Top >= this.Height)
                    stars[i].Top = -stars[i].Height;
            }
            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgroundSpeed - 2;
                if (stars[i].Top >= this.Height)
                    stars[i].Top = -stars[i].Height;
            }
        }

        private void Right_Timer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
                Player.Left += playerSpeed;
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
                Player.Left -= playerSpeed;
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
                Player.Left += playerSpeed;
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10)
                Player.Top -= playerSpeed;
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
                Player.Top += playerSpeed;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause)
            {
                if (e.KeyCode == Keys.Right) RightMoveTimer.Start();
                if (e.KeyCode == Keys.Left) LeftMoveTimer.Start();
                if (e.KeyCode == Keys.Down) DownMoveTimer.Start();
                if (e.KeyCode == Keys.Up) UpMoveTimer.Start();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();

            if (e.KeyCode == Keys.Space)
            {
                if (!gameOver)
                {
                    if (pause)
                    {
                        StartTimer();
                        lbl1.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        lbl1.Location = new Point(this.Width / 2 - 120, 150);
                        lbl1.Text = "PAUSED";
                        lbl1.Visible = true;
                        gameMedia.controls.pause();
                        StopTimer();
                        pause = true;
                    }
                }
            }
        }

        private void MoveAttackTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < attacks.Length; i++)
            {
                if (attacks[i].Top > 0)
                {
                    attacks[i].Visible = true;
                    attacks[i].Top -= attackSpeed;
                    Collision();
                }
                else
                {
                    shootMedia.controls.play();
                    attacks[i].Visible = false;
                    attacks[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemySpeed);
        }

        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                    array[i].Location = new Point((i + 1) * 50, -200);
            }
        }

        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (attacks[0].Bounds.IntersectsWith(enemies[i].Bounds) ||
                    attacks[1].Bounds.IntersectsWith(enemies[i].Bounds) ||
                    attacks[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.controls.play();

                    score += 1;
                    scorelbl.Text = "Score: " + (score < 10 ? "0" + score.ToString() : score.ToString());

                    if (score % 30 == 0)
                    {
                        level += 1;
                        levellbl.Text = "Level: " + level.ToString();

                        if (enemySpeed <= 10 && enemiesAttacksSpeed <= 10 && dificulty >= 0)
                        {
                            dificulty--;
                            enemySpeed++;
                            enemiesAttacksSpeed++;
                        }

                        if (level == 10)
                            GameOver("CONGRATULATIONS!");
                    }
                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    TakeDamage(100);
                }
            }
        }

        private void MoveObstaclesTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].Visible = true;
                obstacles[i].Top += obstacleSpeed;

                if (obstacles[i].Top > this.Height)
                {
                    obstacles[i].Visible = false;
                    obstacles[i].Location = new Point(rand.Next(20, 560), rand.Next(-300, -30));
                }

                if (obstacles[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    explosion.controls.play();
                    obstacles[i].Location = new Point(rand.Next(20, 560), rand.Next(-300, -30));

                    if (obstacles[i].Tag.ToString() == "flaming")
                        TakeDamage(60);
                    else
                        TakeDamage(30);
                }

                for (int j = 0; j < attacks.Length; j++)
                {
                    if (attacks[j].Bounds.IntersectsWith(obstacles[i].Bounds))
                    {
                        explosion.controls.play();
                        obstacles[i].Location = new Point(rand.Next(20, 560), rand.Next(-300, -30));
                        score += 1;
                        scorelbl.Text = "Score: " + (score < 10 ? "0" + score.ToString() : score.ToString());
                    }
                }
            }
        }

        private void GameOver(string str)
        {
            gameOver = true;
            lbl1.Text = str;
            lbl1.Location = new Point(120, 120);
            lbl1.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;

            gameMedia.controls.stop();
            StopTimer();
        }

        private void StopTimer()
        {
            MoveBgTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveAttackTimer.Stop();
            EnemiesAttackTimer.Stop();
            MoveObstaclesTimer.Stop();
        }

        private void StartTimer()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveAttackTimer.Start();
            EnemiesAttackTimer.Start();
            MoveObstaclesTimer.Start();
        }

        private void EnemiesAttacksTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemiesAttacks.Length - dificulty); i++)
            {
                if (enemiesAttacks[i].Top < this.Height)
                {
                    enemiesAttacks[i].Visible = true;
                    enemiesAttacks[i].Top += enemiesAttacksSpeed;
                    CollisionWithEnemiesAttacks();
                }
                else
                {
                    enemiesAttacks[i].Visible = false;
                    int x = rand.Next(0, 10);
                    enemiesAttacks[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
        }

        private void CollisionWithEnemiesAttacks()
        {
            for (int i = 0; i < enemiesAttacks.Length; i++)
            {
                if (enemiesAttacks[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesAttacks[i].Visible = false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    TakeDamage(100);
                }
            }
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            Form1 newGame = new Form1();
            newGame.Show();
            this.Close();
        }
    }
}