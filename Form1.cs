using System;
using System.Drawing;
using System.Windows.Forms;
using WMPLib;

namespace SPACESHOOTER_ORCA
{
    public partial class Form1 : Form
    {
        // Media (bg sounds)
        private readonly WindowsMediaPlayer gameMedia;
        private readonly WindowsMediaPlayer shootMedia;
        private readonly WindowsMediaPlayer explosion;

        // Player
        private int playerSpeed;
        private int playerHealth;
        private int playerMaxHealth;
        private int playerLives;
        private PictureBox[] hearts;
        private int hitCount;
        private int hitsToLoseLife;
        private bool isInvincible;
        private int invincibleTimer;

        // Attack
        private PictureBox[] attacks;
        private int attackSpeed;
        private int normalAttackSpeed;

        // Enemy 
        private PictureBox[] enemies;
        private int enemySpeed;
        private PictureBox[] enemiesAttacks;
        private int enemiesAttacksSpeed;

        // Obstacle 
        private PictureBox[] obstacles;
        private int obstacleSpeed;

        // PowerUp 
        private PictureBox[] powerUps;
        private Image[] powerUpImages;
        private string[] powerUpTypes;
        private bool doublePoints;
        private bool rapidFire;

        // Boss 
        private PictureBox finalBoss;
        private PictureBox bossHealthBar;
        private PictureBox bossHealthFill;
        private int bossHealth;
        private int bossMaxHealth;
        private bool bossActive;
        private bool bossMovingRight;
        private int bossAttackCounter;

        // Effects
        private PictureBox[] stars;
        private int backgroundSpeed;
        private Image[] bgImages;

        // Game State
        private int score;
        private int level;
        private int difficulty;
        private bool pause;
        private bool gameOver;
        private readonly Random rand;

        public Form1()
        {
            InitializeComponent();

            // Initialize media
            gameMedia = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            rand = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeGameState();
            LoadImages();
            InitializeMedia();
            InitializeStars();
            InitializeEnemies();
            InitializeAttacks();
            InitializeObstacles();
            InitializeHearts();
            InitializePowerUps();
            InitializeBoss();
            StartGame();
        }

        // Initialization Methods
        private void InitializeGameState()
        {
            pause = false;
            gameOver = false;
            score = 0;
            level = 1;
            difficulty = 9;

            playerHealth = 100;
            playerMaxHealth = 100;
            playerLives = 3;

            hitCount = 0;
            hitsToLoseLife = 3;

            backgroundSpeed = 4;
            playerSpeed = 4;
            enemySpeed = 4;
            attackSpeed = 20;
            normalAttackSpeed = attackSpeed;
            enemiesAttacksSpeed = 4;
            obstacleSpeed = 3;

            isInvincible = false;
            invincibleTimer = 0;

            doublePoints = false;
            rapidFire = false;

            ReplayBtn.Visible = false;
            ExitBtn.Visible = false;
            lbl1.Visible = false;

            scorelbl.Text = "Score: 00";
            levellbl.Text = "Level: 1";
        }

        private void LoadImages()
        {
            bgImages = new Image[5];
            bgImages[0] = Image.FromFile(@"assets\L1.png");
            bgImages[1] = Image.FromFile(@"assets\L2.png");
            bgImages[2] = Image.FromFile(@"assets\L3.png");
            bgImages[3] = Image.FromFile(@"assets\L4.png");
            bgImages[4] = Image.FromFile(@"assets\L5.png");

            powerUpImages = new Image[3];
            powerUpImages[0] = Image.FromFile(@"assets\restore.png");
            powerUpImages[1] = Image.FromFile(@"assets\double attack.png");
            powerUpImages[2] = Image.FromFile(@"assets\double point.png");
        }

        private void InitializeMedia()
        {
            gameMedia.URL = "songs\\GameSong.mp3";
            shootMedia.URL = "songs\\shoot.mp3";
            explosion.URL = "songs\\boom.mp3";

            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 5;
            shootMedia.settings.volume = 1;
            explosion.settings.volume = 6;
        }

        private void InitializeStars()
        {
            stars = new PictureBox[15];
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox
                {
                    BorderStyle = BorderStyle.None,
                    Location = new Point(rand.Next(20, 580), rand.Next(-10, 400))
                };

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
        }

        private void InitializeEnemies()
        {
            enemies = new PictureBox[10];
            Image enemy1 = Image.FromFile("assets\\E1.png");
            Image enemy2 = Image.FromFile("assets\\E2.png");
            Image enemy3 = Image.FromFile("assets\\E3.png");
            Image boss1 = Image.FromFile("assets\\TIE fighter.png");
            Image boss2 = Image.FromFile("assets\\TIE fighter 2.png");

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.None,
                    Visible = false,
                    Size = (i == 0 || i == 9) ? new Size(60, 60) : new Size(40, 40),
                    Location = new Point((i + 1) * 50, -50)
                };
                this.Controls.Add(enemies[i]);
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

            enemiesAttacks = new PictureBox[10];
            for (int i = 0; i < enemiesAttacks.Length; i++)
            {
                enemiesAttacks[i] = new PictureBox
                {
                    Size = new Size(2, 25),
                    Visible = false,
                    BackColor = Color.Yellow,
                    Location = new Point(enemies[i % 10].Location.X, enemies[i % 10].Location.Y - 20),
                    Tag = (i == 0 || i == 9) ? "boss" : "basic"
                };
                this.Controls.Add(enemiesAttacks[i]);
            }
        }

        private void InitializeAttacks()
        {
            attacks = new PictureBox[3];
            Image munition = Image.FromFile(@"assets\munition.png");

            for (int i = 0; i < attacks.Length; i++)
            {
                attacks[i] = new PictureBox
                {
                    Size = new Size(8, 8),
                    Image = munition,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.None
                };
                this.Controls.Add(attacks[i]);
            }
        }

        private void InitializeObstacles()
        {
            obstacles = new PictureBox[6];
            Image meteor = Image.FromFile(@"assets\meteor.png");
            Image flamingMeteor = Image.FromFile(@"assets\flaming_meteor.png");

            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i] = new PictureBox
                {
                    Size = new Size(35, 35),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.Transparent,
                    Visible = false,
                    Image = (i % 2 == 0) ? meteor : flamingMeteor,
                    Tag = (i % 2 == 0) ? "basic" : "flaming",
                    Location = new Point(rand.Next(20, 560), rand.Next(-300, -30))
                };
                this.Controls.Add(obstacles[i]);
            }
        }

        private void InitializeHearts()
        {
            hearts = new PictureBox[3];
            Image heartImg = Image.FromFile(@"assets\heart.png");

            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i] = new PictureBox
                {
                    Size = new Size(45, 45),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    BorderStyle = BorderStyle.None,
                    Image = heartImg,
                    Location = new Point(this.Width - 130 + i * 35, this.Height - 85),
                    Visible = true
                };
                this.Controls.Add(hearts[i]);
            }
        }

        private void InitializePowerUps()
        {
            powerUpTypes = new string[] { "restore", "rapidfire", "doublepoint" };
            powerUps = new PictureBox[3];

            for (int i = 0; i < powerUps.Length; i++)
            {
                powerUps[i] = new PictureBox
                {
                    Size = new Size(25, 25),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.Transparent,
                    Visible = false,
                    Image = powerUpImages[i],
                    Tag = powerUpTypes[i],
                    Location = new Point(-50, -50)
                };
                this.Controls.Add(powerUps[i]);
            }
        }

        private void InitializeBoss()
        {
            Image deathStar = Image.FromFile(@"assets\death star.png");

            finalBoss = new PictureBox
            {
                Size = new Size(120, 120),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Transparent,
                Image = deathStar,
                Visible = false,
                Location = new Point(this.Width / 2 - 60, -130)
            };
            this.Controls.Add(finalBoss);

            bossHealthBar = new PictureBox
            {
                Size = new Size(200, 15),
                BackColor = Color.DarkRed,
                Visible = false,
                Location = new Point(this.Width / 2 - 100, 10)
            };
            this.Controls.Add(bossHealthBar);

            bossHealthFill = new PictureBox
            {
                Size = new Size(200, 15),
                BackColor = Color.LimeGreen,
                Visible = false,
                Location = new Point(this.Width / 2 - 100, 10)
            };
            this.Controls.Add(bossHealthFill);

            bossHealth = 100;
            bossMaxHealth = 100;
            bossActive = false;
            bossMovingRight = true;
            bossAttackCounter = 0;
        }

        private void StartGame()
        {
            gameMedia.controls.play();
        }

        // Player Movement
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

            if (e.KeyCode == Keys.Space && !gameOver)
            {
                if (pause)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        private void Right_Timer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580) Player.Left += playerSpeed;
        }

        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10) Player.Left -= playerSpeed;
        }

        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580) Player.Left += playerSpeed;
        }

        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10) Player.Top -= playerSpeed;
        }

        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400) Player.Top += playerSpeed;
        }

        // Game State Management
        private void PauseGame()
        {
            lbl1.Location = new Point(this.Width / 2 - 120, 150);
            lbl1.Text = "PAUSED";
            lbl1.Visible = true;
            gameMedia.controls.pause();
            StopTimer();
            pause = true;
        }

        private void ResumeGame()
        {
            StartTimer();
            lbl1.Visible = false;
            gameMedia.controls.play();
            pause = false;
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

        private void GameOver(string message)
        {
            gameOver = true;
            lbl1.Text = message;
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

        private void LoseLife()
        {
            if (isInvincible) return;

            playerLives -= 1;
            UpdateHearts();

            if (playerLives <= 0)
            {
                Player.Visible = false;
                GameOver("Game Over");
            }
            else
            {
                isInvincible = true;
                InvincibleTimer.Start();
            }
        }

        private void TakeDamage(int hitsRequired)
        {
            if (isInvincible) return;

            hitCount++;
            if (hitCount >= hitsRequired)
            {
                hitCount = 0;
                playerLives -= 1;
                UpdateHearts();

                if (playerLives <= 0)
                {
                    Player.Visible = false;
                    GameOver("Game Over");
                }
                else
                {
                    isInvincible = true;
                    InvincibleTimer.Start();
                }
            }
        }

        private void UpdateHearts()
        {
            if (playerLives == 2) hearts[2].Visible = false;
            else if (playerLives == 1) hearts[1].Visible = false;
            else if (playerLives <= 0) hearts[0].Visible = false;
        }

        private void InvincibleTimer_Tick(object sender, EventArgs e)
        {
            isInvincible = false;
            InvincibleTimer.Stop();
        }

        // Background and Visuals
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

        // Attack System
        private void MoveAttackTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < attacks.Length; i++)
            {
                if (attacks[i].Top > 0)
                {
                    attacks[i].Visible = true;
                    attacks[i].Top -= attackSpeed;
                    CheckCollisions();
                }
                else
                {
                    shootMedia.controls.play();
                    attacks[i].Visible = false;
                    attacks[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        // Collision System
        private void CheckCollisions()
        {
            CheckEnemyCollisions();
            CheckObstacleCollisions();
        }

        private void CheckEnemyCollisions()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (IsAttackCollidingWithEnemy(i))
                {
                    HandleEnemyHit(i);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    HandlePlayerEnemyCollision(i);
                }
            }
        }

        private bool IsAttackCollidingWithEnemy(int enemyIndex)
        {
            for (int i = 0; i < attacks.Length; i++)
            {
                if (attacks[i].Bounds.IntersectsWith(enemies[enemyIndex].Bounds))
                    return true;
            }
            return false;
        }

        private void HandleEnemyHit(int enemyIndex)
        {
            explosion.controls.play();

            score += doublePoints ? 2 : 1;
            scorelbl.Text = "Score: " + (score < 10 ? "0" + score.ToString() : score.ToString());

            CheckLevelProgression();

            enemies[enemyIndex].Location = new Point((enemyIndex + 1) * 50, -100);
            SpawnPowerUp(enemies[enemyIndex].Location.X, enemies[enemyIndex].Location.Y);
        }

        private void HandlePlayerEnemyCollision(int enemyIndex)
        {
            explosion.settings.volume = 30;
            explosion.controls.play();
            LoseLife();
            enemies[enemyIndex].Location = new Point((enemyIndex + 1) * 50, -100);
        }

        private void CheckObstacleCollisions()
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                if (obstacles[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    HandleObstacleHit(i);
                }

                for (int j = 0; j < attacks.Length; j++)
                {
                    if (attacks[j].Bounds.IntersectsWith(obstacles[i].Bounds))
                    {
                        HandleObstacleDestroyed(i);
                        attacks[j].Visible = false;
                    }
                }
            }
        }

        private void HandleObstacleHit(int obstacleIndex)
        {
            explosion.controls.play();
            obstacles[obstacleIndex].Location = new Point(rand.Next(20, 560), rand.Next(-300, -30));

            if (obstacles[obstacleIndex].Tag.ToString() == "flaming")
                TakeDamage(2);
            else
                TakeDamage(3);
        }

        private void HandleObstacleDestroyed(int obstacleIndex)
        {
            explosion.controls.play();
            obstacles[obstacleIndex].Location = new Point(rand.Next(20, 560), rand.Next(-300, -30));
            score += 1;
            scorelbl.Text = "Score: " + (score < 10 ? "0" + score.ToString() : score.ToString());
        }

        private void CheckLevelProgression()
        {
            int[] levelThresholds = { 20, 45, 75, 110 };

            if (level <= 4 && score >= levelThresholds[level - 1])
            {
                level += 1;
                levellbl.Text = "Level: " + level.ToString();

                if (enemySpeed <= 10 && enemiesAttacksSpeed <= 10 && difficulty >= 0)
                {
                    difficulty--;
                    enemySpeed++;
                    enemiesAttacksSpeed++;
                }

                if (level == 5)
                    ActivateFinalBoss();
            }
        }

        // Enemy System
        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            if (bossActive)
            {
                MoveFinalBoss();
                return;
            }

            MoveEnemies();
        }

        private void MoveEnemies()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].Visible = true;
                enemies[i].Top += enemySpeed;

                if (enemies[i].Top > this.Height)
                    enemies[i].Location = new Point((i + 1) * 50, -200);
            }
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
            CheckEnemyAttackCollisions();
        }

        private void CheckEnemyAttackCollisions()
        {
            bool hasHitThisFrame = false;

            for (int i = 0; i < enemiesAttacks.Length; i++)
            {
                if (enemiesAttacks[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesAttacks[i].Visible = false;
                    enemiesAttacks[i].Location = new Point(-100, -100);

                    if (!hasHitThisFrame && !isInvincible)
                    {
                        explosion.settings.volume = 30;
                        explosion.controls.play();

                        hitCount++;
                        int hitsNeeded = (enemiesAttacks[i].Tag != null && enemiesAttacks[i].Tag.ToString() == "boss") ? 2 : 3;

                        if (hitCount >= hitsNeeded)
                        {
                            hitCount = 0;
                            playerLives--;
                            UpdateHearts();

                            if (playerLives <= 0)
                            {
                                Player.Visible = false;
                                GameOver("Game Over");
                            }
                            else
                            {
                                isInvincible = true;
                                InvincibleTimer.Start();
                            }
                        }
                        hasHitThisFrame = true;
                    }
                }
            }
        }

        // Obstacle System
        private void MoveObstaclesTimer_Tick(object sender, EventArgs e)
        {
            if (bossActive) return;

            for (int i = 0; i < obstacles.Length; i++)
            {
                obstacles[i].Visible = true;
                obstacles[i].Top += obstacleSpeed;

                if (obstacles[i].Top > this.Height)
                {
                    obstacles[i].Visible = false;
                    obstacles[i].Location = new Point(rand.Next(20, 560), rand.Next(-300, -30));
                }
            }
        }

        // PowerUp System
        private void SpawnPowerUp(int x, int y)
        {
            if (rand.Next(0, 100) > 30) return;

            int type = rand.Next(0, 3);
            if (type == 0 && playerLives == 3) return;

            powerUps[type].Location = new Point(x, y);
            powerUps[type].Visible = true;
        }

        private void CollectPowerUp()
        {
            for (int i = 0; i < powerUps.Length; i++)
            {
                if (!powerUps[i].Visible) continue;

                powerUps[i].Top += 2;

                if (powerUps[i].Top > this.Height)
                {
                    powerUps[i].Visible = false;
                    powerUps[i].Location = new Point(-50, -50);
                }

                if (powerUps[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    powerUps[i].Visible = false;
                    powerUps[i].Location = new Point(-50, -50);

                    string type = powerUps[i].Tag.ToString();

                    if (type == "restore")
                    {
                        playerLives += 1;
                        if (playerLives == 2) hearts[2].Visible = true;
                        else if (playerLives == 3) { hearts[1].Visible = true; hearts[2].Visible = true; }
                    }
                    else if (type == "rapidfire")
                    {
                        rapidFire = true;
                        attackSpeed = normalAttackSpeed * 2;
                        PowerUpTimer.Start();
                    }
                    else if (type == "doublepoint")
                    {
                        doublePoints = true;
                        PowerUpTimer.Start();
                    }
                }
            }
        }

        private void PowerUpTimer_Tick(object sender, EventArgs e)
        {
            rapidFire = false;
            doublePoints = false;
            attackSpeed = normalAttackSpeed;
            PowerUpTimer.Stop();
        }

        // Boss System
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

            UpdateBossPosition();
            UpdateBossHealthBar();
            FireBossBullets();
            CheckBossCollisions();
        }

        private void UpdateBossPosition()
        {
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
        }

        private void UpdateBossHealthBar()
        {
            int fillWidth = Math.Max(0, (int)((float)bossHealth / bossMaxHealth * 200));
            bossHealthFill.Width = fillWidth;
            bossHealthFill.BackColor = bossHealth > 40 ? Color.LimeGreen : Color.OrangeRed;
            bossHealthBar.BringToFront();
            bossHealthFill.BringToFront();
        }

        private void FireBossBullets()
        {
            bossAttackCounter++;
            if (bossAttackCounter % 20 == 0)
            {
                FireBossBullet();
                if (bossAttackCounter >= 100) bossAttackCounter = 0;
            }
        }

        private void FireBossBullet()
        {
            int bulletsSpawned = 0;
            int[] offsets = { 10, 50, 90 };

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

        private void CheckBossCollisions()
        {
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

        // UI Events
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

        // i dunno...
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
    }
}