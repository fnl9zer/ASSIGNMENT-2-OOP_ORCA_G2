using System;

namespace SPACESHOOTER_ORCA
{
    // Defines understanding for objects that can take damage
    public interface IDamageable
    {
        void TakeDamage(int damage);
        bool IsDestroyed();
    }

    // Player class
    public class Player : IDamageable
    {
        // Encapsulation: Private fields
        private int health;
        private int maxHealth;
        private int lives;
        private int score;
        private int speed;

        // Encapsulation: Public properties provide controlled access
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int Lives { get { return lives; } set { lives = value; } }
        public int Score { get { return score; } set { score = value; } }
        public int Speed { get { return speed; } set { speed = value; } }

        // Constructor
        public Player(int health, int maxHealth, int lives, int speed)
        {
            this.health = health;
            this.maxHealth = maxHealth;
            this.lives = lives;
            this.score = 0;
            this.speed = speed;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health < 0) health = 0;
        }

        public void Heal(int amount)
        {
            health += amount;
            if (health > maxHealth) health = maxHealth;
        }

        public void AddScore(int points)
        {
            score += points;
        }

        public void LoseLife()
        {
            lives -= 1;
        }

        public void Respawn()
        {
            health = maxHealth;
        }

        public bool IsDestroyed()
        {
            return lives <= 0;
        }
    }

    // Enemy base class
    public class Enemy
    {
        // Encapsulation: Private fields
        private int damage;
        private int speed;
        private string type;
        private int pointsAwarded;

        // Encapsulation: Public properties
        public int Damage { get { return damage; } set { damage = value; } }
        public int Speed { get { return speed; } set { speed = value; } }
        public string Type { get { return type; } set { type = value; } }
        public int PointsAwarded { get { return pointsAwarded; } set { pointsAwarded = value; } }

        public Enemy(int damage, int speed, string type, int pointsAwarded)
        {
            this.damage = damage;
            this.speed = speed;
            this.type = type;
            this.pointsAwarded = pointsAwarded;
        }

        // Polymorphism: Different enemies can have different attack behaviors
        public virtual void Attack(Player player)
        {
            player.TakeDamage(damage);
        }

        public bool IsBoss()
        {
            return type == "boss";
        }

        public virtual void IncreaseSpeed(int amount)
        {
            speed += amount;
        }
    }

    // BasicEnemy - Inheritance from Enemy
    public class BasicEnemy : Enemy
    {
        public BasicEnemy() : base(10, 4, "basic", 1) { }
    }

    // HardEnemy - Inheritance from Enemy
    public class HardEnemy : Enemy
    {
        public HardEnemy() : base(20, 5, "hard", 3) { }
    }

    // BossEnemy - Inheritance from Enemy with additional features
    public class BossEnemy : Enemy
    {
        private int phase;
        private bool enraged;

        public int Phase { get { return phase; } set { phase = value; } }
        public bool Enraged { get { return enraged; } set { enraged = value; } }

        public BossEnemy() : base(100, 6, "boss", 10)
        {
            phase = 1;
            enraged = false;
        }

        public void NextPhase()
        {
            phase = 2;
            enraged = true;
            Speed = 8;
            Damage = 150;
        }

        // Polymorphism: Override attack for boss behavior
        public override void Attack(Player player)
        {
            int bonusDamage = enraged ? 50 : 0;
            player.TakeDamage(Damage + bonusDamage);
        }

        // Polymorphism: Override speed increase for boss
        public override void IncreaseSpeed(int amount)
        {
            Speed += amount * 2;
        }
    }

    // Obstacle class
    public class Obstacle
    {
        // Encapsulation: Private fields
        private string type;
        private int collisionDamage;
        private int speed;
        private int positionX;
        private int positionY;

        // Encapsulation: Public properties
        public string Type { get { return type; } set { type = value; } }
        public int CollisionDamage { get { return collisionDamage; } set { collisionDamage = value; } }
        public int Speed { get { return speed; } set { speed = value; } }
        public int PositionX { get { return positionX; } set { positionX = value; } }
        public int PositionY { get { return positionY; } set { positionY = value; } }

        public Obstacle(string type, int speed)
        {
            this.type = type;
            this.speed = speed;
            this.collisionDamage = (type == "flaming") ? 60 : 30;
        }

        public void Collide(Player player)
        {
            player.TakeDamage(collisionDamage);
        }

        public bool IsFlaming()
        {
            return type == "flaming";
        }

        public void Move()
        {
            positionY += speed;
        }
    }

    // PowerUp class
    public class PowerUp
    {
        private string type;
        private int value;
        private float duration;
        private bool isPermanent;

        public string Type { get { return type; } }
        public int Value { get { return value; } }
        public float Duration { get { return duration; } }
        public bool IsPermanent { get { return isPermanent; } }

        public PowerUp(string type, int value, float duration, bool isPermanent)
        {
            this.type = type;
            this.value = value;
            this.duration = duration;
            this.isPermanent = isPermanent;
        }

        public void ApplyTo(Player player)
        {
            if (type == "restore")
                player.Lives += value;
        }

        public void RemoveFrom(Player player)
        {
            // Remove temporary effects (handled by Form1 timers)
        }
    }

    // Level class
    public class Level
    {
        private int levelNumber;
        private int requiredScore;
        private string difficulty;

        public int LevelNumber { get { return levelNumber; } set { levelNumber = value; } }
        public int RequiredScore { get { return requiredScore; } set { requiredScore = value; } }
        public string Difficulty { get { return difficulty; } set { difficulty = value; } }

        public Level(int levelNumber, int requiredScore, string difficulty)
        {
            this.levelNumber = levelNumber;
            this.requiredScore = requiredScore;
            this.difficulty = difficulty;
        }

        public void IncreaseDifficulty()
        {
            if (difficulty == "easy")
                difficulty = "medium";
            else if (difficulty == "medium")
                difficulty = "hard";
        }
    }
}