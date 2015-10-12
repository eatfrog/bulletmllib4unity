using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BulletMLLib;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var bm = new FakeBulletManager();
            
            GameManager.GameDifficulty = () => 1f;

            BulletPattern pattern = new BulletPattern("../../example.xml");
            var fakeBullet = new FakeBullet(bm);
            var emitter = new FakeEmitter(bm, pattern, fakeBullet);

            for (int i = 0; i < 500; i++)
            {
                emitter.Update(0, 0);
                for (int ii = 0; ii < bm.Bullets.Count; ii++)
                {
                    bm.Bullets[ii].Update();
                }
                bm.Bullets.ForEach(bullet => Console.WriteLine("X: {0} Y: {1} Aim: {2} Direction: {3}", bullet.X, bullet.Y, bullet.GetAngleTowardsPlayer(), bullet.Direction));                
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q) break;
                else Console.WriteLine("--- Press Q for break or any other key for next step ---");
            }
            Console.WriteLine("End");
            Console.ReadKey();
        }
    }
}
