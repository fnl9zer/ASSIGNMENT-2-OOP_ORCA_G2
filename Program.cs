using System;
using System.Windows.Forms;

namespace SPACESHOOTER_ORCA
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("=== SPACE SHOOTER: OBJECT INTERACTION ===\n");

            // --- ENCAPSULATION DEMO ---
            Console.WriteLine("--- ENCAPSULATION ---");
            Player player = new Player(100, 100, 3, 4);
            Console.WriteLine("Player created — Health: " + player.Health + " | Lives: " + player.Lives + " | Score: " + player.Score);

            // --- INHERITANCE DEMO ---
            Console.WriteLine("\n--- INHERITANCE ---");
            BasicEnemy basic = new BasicEnemy();
            HardEnemy hard = new HardEnemy();
            BossEnemy boss = new BossEnemy();

            Console.WriteLine("BasicEnemy inherits from Enemy: " + (basic is Enemy));
            Console.WriteLine("HardEnemy inherits from Enemy: " + (hard is Enemy));
            Console.WriteLine("BossEnemy inherits from Enemy: " + (boss is Enemy));

            // --- POLYMORPHISM DEMO ---
            Console.WriteLine("\n--- POLYMORPHISM ---");
            Console.WriteLine("Same Attack() method, different behaviour per enemy:\n");

            Enemy[] enemyList = new Enemy[]
            {
                new BasicEnemy(),
                new HardEnemy(),
                new BossEnemy()
            };

            foreach (Enemy enemy in enemyList)
            {
                enemy.Attack(player);
                Console.WriteLine("Player health after attack: " + player.Health + "\n");
            }

            // boss phase change
            Console.WriteLine("--- BOSS PHASE CHANGE ---");
            boss.NextPhase();
            boss.Attack(player);
            Console.WriteLine("Player health: " + player.Health);

            // --- OBSTACLE INTERACTION ---
            Console.WriteLine("\n--- OBSTACLE ---");
            Obstacle meteor = new Obstacle("basic", 3);
            Obstacle flamingMeteor = new Obstacle("flaming", 3);
            Console.WriteLine("Meteor damage: " + meteor.CollisionDamage + " | Flaming: " + meteor.IsFlaming());
            Console.WriteLine("Flaming meteor damage: " + flamingMeteor.CollisionDamage + " | Flaming: " + flamingMeteor.IsFlaming());

            flamingMeteor.Collide(player);
            Console.WriteLine("Player health after flaming meteor: " + player.Health);

            // player score
            player.AddScore(basic.PointsAwarded);
            Console.WriteLine("\nPlayer destroys BasicEnemy! Score: " + player.Score);

            // check destruction
            Console.WriteLine("Is player destroyed? " + player.IsDestroyed());
            Console.WriteLine("\n=== END OF DEMONSTRATION ===");

            // launch game
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form = new Form1();
            form.Show();
            Application.Run();
        }
    }
}