using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SPACESHOOTER_ORCA
{
    internal static class Program
    {
        /// <summary>
        /// </summary>
        [STAThread]
        static void Main()
        {
            Player player = new Player(100, 100, 3, 4);
            Console.WriteLine("=== SPACE SHOOTER by Orca ===\n");
            Console.WriteLine("Player created!");
            Console.WriteLine("Health: " + player.Health + " | Lives: " + player.Lives + " | Score: " + player.Score);

            Enemy basicEnemy = new Enemy(10, 4, "basic", 1);
            Enemy bossEnemy = new Enemy(100, 6, "boss", 10);
            Console.WriteLine("\nEnemies created!");
            Console.WriteLine("Basic Enemy — Damage: " + basicEnemy.Damage + "  | Type: " + basicEnemy.Type);
            Console.WriteLine("Boss Enemy  — Damage: " + bossEnemy.Damage + " | Is Boss: " + bossEnemy.IsBoss());

            Obstacle meteor = new Obstacle("basic", 3);
            Obstacle flamingMeteor = new Obstacle("flaming", 3);
            Console.WriteLine("\nObstacles created!");
            Console.WriteLine("Meteor         — Damage: " + meteor.CollisionDamage + " | Flaming: " + meteor.IsFlaming());
            Console.WriteLine("Flaming Meteor — Damage: " + flamingMeteor.CollisionDamage + " | Flaming: " + flamingMeteor.IsFlaming());

            Console.WriteLine("\n=== INTERACTIONS ===\n");

            Console.WriteLine("Basic enemy attacks player...");
            basicEnemy.Attack(player);
            Console.WriteLine("Player Health: " + player.Health);

            Console.WriteLine("\nFlaming meteor hits player...");
            flamingMeteor.Collide(player);
            Console.WriteLine("Player Health: " + player.Health);

            Console.WriteLine("\nPlayer destroys basic enemy!");
            player.AddScore(basicEnemy.PointsAwarded);
            Console.WriteLine("Player Score: " + player.Score);

            Console.WriteLine("\nBoss enemy attacks player...");
            bossEnemy.Attack(player);
            Console.WriteLine("Player Health: " + player.Health);
            if (player.Health <= 0)
            {
                player.LoseLife();
                player.Respawn();
                Console.WriteLine("Player has lost a life! Lives remaining: " + player.Lives);
                Console.WriteLine("Player respawned! Health: " + player.Health);
            }

            Console.WriteLine("\nLevel up! Enemy speed increases...");
            basicEnemy.IncreaseSpeed(2);
            Console.WriteLine("Basic Enemy new speed: " + basicEnemy.Speed);

            Console.WriteLine("\nIs player destroyed? " + player.IsDestroyed());
            Console.WriteLine("\n=== GAME OVER ===");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form = new Form1();
            form.Show();
            Application.Run();
        }
    }
}
