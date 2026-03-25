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

    // Enemy class
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
        public void Attack(Player player)
        {
            player.TakeDamage(damage);
        }

        public bool IsBoss()
        {
            return type == "boss";
        }

        public void IncreaseSpeed(int amount)
        {
            speed += amount;
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
}