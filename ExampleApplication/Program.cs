using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletMLLib;

namespace ExampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var bm = new FakeBulletManager();

            BulletPattern pattern = new BulletPattern("../../1.xml");
            var bullet = new FakeBullet(bm);
            var emitter = new FakeEmitter(bm, pattern, bullet);

            for (int i = 0; i < 5000; i++)
            {
                emitter.Update(0, 0);                
            }
            Console.ReadKey();
        }
    }
}
