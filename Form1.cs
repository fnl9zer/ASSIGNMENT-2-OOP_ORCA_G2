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

        PictureBox background;
        Image[] bgImages;

        int score;
        int level;
        int dificulty;
        bool pause;
        bool gameOver;

        int playerHealth;
        int playerMaxHealth;
        int playerLives;
        PictureBox[] hearts;

        int hitCount;
        int hitsToLoseLife;

        bool isInvincible;
        int invincibleTimer;

        //power ups
        PictureBox[] powerUps;
        Image[] powerUpImages;
        string[] powerUpTypes;
        bool doublePoints;
        bool rapidFire;
        int normalAttackSpeed;

        //final boss
        PictureBox finalBoss;
        PictureBox bossHealthBar;
        PictureBox bossHealthFill;
        int bossHealth;
        int bossMaxHealth;
        bool bossActive;
        bool bossMovingRight;
        int bossAttackCounter;

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

            hitCount = 0;
            hitsToLoseLife = 3;

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

            isInvincible = false;
            invincibleTimer = 0;

            attacks = new PictureBox[3];

            //load background images
            bgImages = new Image[5];
            bgImages[0] = Image.FromFile(@"assets\L1.png");
            bgImages[1] = Image.FromFile(@"assets\L2.png");
            bgImages[2] = Image.FromFile(@"assets\L3.png");
            bgImages[3] = Image.FromFile(@"assets\L4.png");
            bgImages[4] = Image.FromFile(@"assets\L5.png");

            //load images
            Image munition = Image.FromFile(@"assets\munition.png");
            Image heartImg = Image.FromFile(@"assets\heart.png");
            Image meteor = Image.FromFile(@"assets\meteor.png");
            Image flamingMeteor = Image.FromFile(@"assets\flaming_meteor.png");

            Image enemy1 = Image.FromFile("assets\\E1.png");
            Image enemy2 = Image.FromFile("assets\\E2.png");
            Image enemy3 = Image.FromFile("assets\\E3.png");
            Image boss1 = Image.FromFile("assets\\TIE fighter.png");
            Image boss2 = Image.FromFile("assets\\TIE fighter 2.png");

            Image deathStar = Image.FromFile(@"assets\death star.png");

            powerUpImages = new Image[3];
            powerUpImages[0] = Image.FromFile(@"assets\restore.png");
            powerUpImages[1] = Image.FromFile(@"assets\double attack.png");
            powerUpImages[2] = Image.FromFile(@"assets\double point.png");

            finalBoss = new PictureBox();
            finalBoss.Size = new Size(120, 120);
            finalBoss.SizeMode = PictureBoxSizeMode.Zoom;
            finalBoss.BorderStyle = BorderStyle.None;
            finalBoss.BackColor = Color.Transparent;
            finalBoss.Image = deathStar;
            finalBoss.Visible = false;
            finalBoss.Location = new Point(this.Width / 2 - 60, -130);
            this.Controls.Add(finalBoss);

            bossHealthBar = new PictureBox();
            bossHealthBar.Size = new Size(200, 15);
            bossHealthBar.BackColor = Color.DarkRed;
            bossHealthBar.Visible = false;
            bossHealthBar.Location = new Point(this.Width / 2 - 100, 10);
            this.Controls.Add(bossHealthBar);

            bossHealthFill = new PictureBox();
            bossHealthFill.Size = new Size(200, 15);
            bossHealthFill.BackColor = Color.LimeGreen;
            bossHealthFill.Visible = false;
            bossHealthFill.Location = new Point(this.Width / 2 - 100, 10);
            this.Controls.Add(bossHealthFill);

            bossHealth = 100;
            bossMaxHealth = 100;
            bossActive = false;
            bossMovingRight = true;
            bossAttackCounter = 0;

            powerUpTypes = new string[] { "restore", "rapidfire", "doublepoint" };
            doublePoints = false;
            rapidFire = false;
            normalAttackSpeed = attackSpeed;

            powerUps = new PictureBox[3];
            for (int i = 0; i < powerUps.Length; i++)
            {
                powerUps[i] = new PictureBox();
                powerUps[i].Size = new Size(25, 25);
                powerUps[i].SizeMode = PictureBoxSizeMode.Zoom;
                powerUps[i].BorderStyle = BorderStyle.None;
                powerUps[i].BackColor = Color.Transparent;
                powerUps[i].Visible = false;
                powerUps[i].Image = powerUpImages[i];
                powerUps[i].Tag = powerUpTypes[i];
                powerUps[i].Location = new Point(-50, -50);
                this.Controls.Add(powerUps[i]);
            }

            enemies = new PictureBox[10];

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();

                if (i == 0 || i == 9) 
                {
                    enemies[i].Size = new Size(60, 60);  // Boss size
                }
                else
                {
                    enemies[i].Size = new Size(40, 40);  // Regular enemy size
                }

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
                hearts[i].Size = new Size(45, 45);
                hearts[i].SizeMode = PictureBoxSizeMode.Zoom;
                hearts[i].BackColor = Color.Transparent;
                hearts[i].BorderStyle = BorderStyle.None;
                hearts[i].Image = heartImg;
                hearts[i].Location = new Point(this.Width - 130 + i * 35, this.Height - 85);
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
                // tag as boss or basic based on which enemy fires it
                enemiesAttacks[i].Tag = (x == 0 || x == 9) ? "boss" : "basic";
                this.Controls.Add(enemiesAttacks[i]);
            }

            gameMedia.controls.play();
        }

        private void TakeDamage(int hitsRequired)
        {
            if (isInvincible)
            {
                System.Diagnostics.Debug.WriteLine("Damage ignored - invincible");
                return;
            }

            hitCount++;
            System.Diagnostics.Debug.WriteLine($"Hit! Current hitCount: {hitCount}, Required: {hitsRequired}");

            if (hitCount >= hitsRequired)
            {
                System.Diagnostics.Debug.WriteLine("Losing a life!");
                hitCount = 0;
                playerLives -= 1;

                if (playerLives == 2) hearts[2].Visible = false;
                else if (playerLives == 1) hearts[1].Visible = false;
                else if (playerLives <= 0) hearts[0].Visible = false;

                if (playerLives <= 0)
                {
                    Player.Visible = false;
                    GameOver("Game Over");
                }
                else
                {
                    isInvincible = true;
                    InvincibleTimer.Start();
                    System.Diagnostics.Debug.WriteLine("Invincibility started");
                }
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
            CollectPowerUp();
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
            if (bossActive)
            {
                MoveFinalBoss();
                return;
            }
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

                    score += doublePoints ? 2 : 1;
                    scorelbl.Text = "Score: " + (score < 10 ? "0" + score.ToString() : score.ToString());

                    // score threshold increases each level
                    int[] levelThresholds = { 20, 45, 75, 110 };

                    if (level <= 4 && score >= levelThresholds[level - 1])
                    {
                        level += 1;
                        levellbl.Text = "Level: " + level.ToString();

                        if (enemySpeed <= 10 && enemiesAttacksSpeed <= 10 && dificulty >= 0)
                        {
                            dificulty--;
                            enemySpeed++;
                            enemiesAttacksSpeed++;
                        }

                        if (level == 5)
                            ActivateFinalBoss();
                    }
                    enemies[i].Location = new Point((i + 1) * 50, -100);
                    SpawnPowerUp(enemies[i].Location.X, enemies[i].Location.Y);
                }

                // direct enemy collision = instant lose life
                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    LoseLife();
                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }
            }
        }

        private void LoseLife()
        {
            if (isInvincible) return;

            playerLives -= 1;

            if (playerLives == 2) hearts[2].Visible = false;
            else if (playerLives == 1) hearts[1].Visible = false;
            else if (playerLives <= 0) hearts[0].Visible = false;

            if (playerLives <= 0)
            {
                Player.Visible = false;
                GameOver("Game Over");
            }
            else
            {
                // Start invincibility after losing a life
                isInvincible = true;
                InvincibleTimer.Start();
            }
        }

        private void MoveObstaclesTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                if (bossActive) return;

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
                        TakeDamage(2);
                    else
                        TakeDamage(3);
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

            lbl1.AutoSize = true;
            lbl1.Location = new Point((this.Width - lbl1.Width) / 2, (this.Height - lbl1.Height) / 2 - 80);

            lbl1.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;

            ReplayBtn.Location = new Point((this.Width - ReplayBtn.Width) / 2, (this.Height / 2) + 20);
            ExitBtn.Location = new Point((this.Width - ExitBtn.Width) / 2, (this.Height / 2) + 70);

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
            PowerUpTimer.Stop();
        }

        private void StartTimer()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveAttackTimer.Start();
            EnemiesAttackTimer.Start();
            MoveObstaclesTimer.Start();
            PowerUpTimer.Start();
        }

        private void EnemiesAttacksTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < enemiesAttacks.Length; i++)
            {
                if (enemiesAttacks[i].Visible)
                {
                    enemiesAttacks[i].Top += enemiesAttacksSpeed;

                    if (enemiesAttacks[i].Top > this.Height)
                    {
                        enemiesAttacks[i].Visible = false;
                        enemiesAttacks[i].Location = new Point(-100, -100);
                    }
                }
                else if (!bossActive)
                {
                    int x = rand.Next(0, 10);
                    enemiesAttacks[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
            CollisionWithEnemiesAttacks();
        }

        private void CollisionWithEnemiesAttacks()
        {
            bool hasHitThisFrame = false;

            for (int i = 0; i < enemiesAttacks.Length; i++)
            {
                if (enemiesAttacks[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    // Hide the bullet
                    enemiesAttacks[i].Visible = false;
                    enemiesAttacks[i].Location = new Point(-100, -100);

                    // Only process ONE bullet per frame
                    if (!hasHitThisFrame && !isInvincible)
                    {
                        explosion.settings.volume = 30;
                        explosion.controls.play();

                        // Increment hit counter
                        hitCount++;
                        System.Diagnostics.Debug.WriteLine($"Hit! Current hitCount: {hitCount}");

                        // Determine how many hits needed
                        int hitsNeeded = (enemiesAttacks[i].Tag != null && enemiesAttacks[i].Tag.ToString() == "boss") ? 2 : 3;

                        if (hitCount >= hitsNeeded)
                        {
                            System.Diagnostics.Debug.WriteLine("Losing a life!");
                            hitCount = 0;
                            playerLives--;

                            // Update hearts
                            if (playerLives == 2) hearts[2].Visible = false;
                            else if (playerLives == 1) hearts[1].Visible = false;
                            else if (playerLives <= 0) hearts[0].Visible = false;

                            if (playerLives <= 0)
                            {
                                Player.Visible = false;
                                GameOver("Game Over");
                            }
                            else
                            {
                                isInvincible = true;
                                InvincibleTimer.Start();
                                System.Diagnostics.Debug.WriteLine("Invincibility started");
                            }
                        }

                        hasHitThisFrame = true;
                    }
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

        private void InvincibleTimer_Tick(object sender, EventArgs e)
        {
            isInvincible = false;
            InvincibleTimer.Stop();
        }
        private void SpawnPowerUp(int x, int y)
        {
            // 30% chance to spawn a powerup
            if (rand.Next(0, 100) > 30) return;

            // pick random powerup
            int type = rand.Next(0, 3);

            // restore only spawns if player has lost a life
            if (type == 0 && playerLives == 3) return;

            powerUps[type].Location = new Point(x, y);
            powerUps[type].Visible = true;
        }

        private void CollectPowerUp()
        {
            for (int i = 0; i < powerUps.Length; i++)
            {
                if (!powerUps[i].Visible) continue;

                // move powerup down
                powerUps[i].Top += 2;

                // disappears off screen
                if (powerUps[i].Top > this.Height)
                {
                    powerUps[i].Visible = false;
                    powerUps[i].Location = new Point(-50, -50);
                }

                // player collects it
                if (powerUps[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    powerUps[i].Visible = false;
                    powerUps[i].Location = new Point(-50, -50);

                    string type = powerUps[i].Tag.ToString();

                    if (type == "restore")
                    {
                        // restore 1 heart
                        playerLives += 1;
                        if (playerLives == 2) hearts[2].Visible = true;
                        else if (playerLives == 3) { hearts[1].Visible = true; hearts[2].Visible = true; }
                    }
                    else if (type == "rapidfire")
                    {
                        // double attack speed for 5 seconds
                        rapidFire = true;
                        attackSpeed = normalAttackSpeed * 2;
                        PowerUpTimer.Start();
                    }
                    else if (type == "doublepoint")
                    {
                        // double points for 5 seconds
                        doublePoints = true;
                        PowerUpTimer.Start();
                    }
                }
            }
        }

        private void PowerUpTimer_Tick(object sender, EventArgs e)
        {
            // reset powerup effects after 5 seconds
            rapidFire = false;
            doublePoints = false;
            attackSpeed = normalAttackSpeed;
            PowerUpTimer.Stop();
        }

        private void ActivateFinalBoss()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Visible = false;
                enemies[i].Location = new Point(-100, -100);
            }
            for (int i = 0; i < enemiesAttacks.Length; i++)
            {
                enemiesAttacks[i].Visible = false;
                enemiesAttacks[i].Location = new Point(-100, -100);
            }
            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].Visible = false;
                obstacles[i].Location = new Point(-100, -100);
            }
            bossHealthFill.BringToFront();
            bossHealthBar.BringToFront();

            bossHealth = bossMaxHealth;
            bossActive = true;
            finalBoss.Visible = true;
            bossHealthBar.Visible = true;
            bossHealthFill.Visible = true;
            finalBoss.Location = new Point(this.Width / 2 - 60, 30);
        }
        private void MoveFinalBoss()
        {
            if (!bossActive) return;

            if (bossMovingRight)
            {
                finalBoss.Left += 2;
                if (finalBoss.Right >= this.Width - 10) bossMovingRight = false;
            }
            else
            {
                finalBoss.Left -= 2;
                if (finalBoss.Left <= 10) bossMovingRight = true;
            }
            int fillWidth = Math.Max(0, (int)((float)bossHealth / bossMaxHealth * 200));
            bossHealthFill.Width = fillWidth;
            bossHealthFill.BackColor = bossHealth > 40 ? Color.LimeGreen : Color.OrangeRed;
            bossHealthBar.BringToFront();
            bossHealthFill.BringToFront();

            bossAttackCounter++;
            if (bossAttackCounter % 20 == 0)
            {
                FireBossBullet();
                if (bossAttackCounter >= 100) bossAttackCounter = 0;
            }
            for (int j = 0; j < attacks.Length; j++)
            {
                if (attacks[j].Bounds.IntersectsWith(finalBoss.Bounds) && attacks[j].Visible)
                {
                    explosion.controls.play();
                    attacks[j].Visible = false;
                    attacks[j].Location = new Point(Player.Location.X + 20, Player.Location.Y);
                    bossHealth--;
                    if (bossHealth % 20 == 0)
                        SpawnPowerUp(finalBoss.Location.X + 50, finalBoss.Location.Y + 120);

                    if (bossHealth <= 0)
                    {
                        bossActive = false;
                        finalBoss.Visible = false;
                        bossHealthBar.Visible = false;
                        bossHealthFill.Visible = false;
                        GameOver("CONGRATULATIONS!");
                    }
                }
            }
            if (finalBoss.Bounds.IntersectsWith(Player.Bounds))
            {
                explosion.settings.volume = 30;
                explosion.controls.play();
                LoseLife();
            }
        }
        private void FireBossBullet()
        {
            int bulletsSpawned = 0;
            int[] offsets = { 10, 50, 90 }; // 3 bullets spread across boss width

            for (int i = 0; i < enemiesAttacks.Length && bulletsSpawned < 3; i++)
            {
                if (!enemiesAttacks[i].Visible)
                {
                    enemiesAttacks[i].Location = new Point(
                        finalBoss.Location.X + offsets[bulletsSpawned],
                        finalBoss.Location.Y + 120);
                    enemiesAttacks[i].Tag = "boss";
                    enemiesAttacks[i].Visible = true;
                    bulletsSpawned++;
                }
            }
        }
    }
}