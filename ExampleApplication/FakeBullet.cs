using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletMLLib;

namespace ExampleApplication
{
    public class FakeBullet : Bullet
    {

        public FakeBullet(IBulletManager myBulletManager)
            : base(myBulletManager)
        {
            TimeSpeed = 1;
            Scale = 1;
        }


        public FakeBullet(IBulletManager myBulletManager, Emitter e) : base(myBulletManager)
        {
            Emitter = e;
        }
        
        public bool IsActive { get; set; }

        public override void BulletSpawned()
        {
           
            Console.WriteLine("New bullet spawned. PatterName: " + Node.GetPatternName());

            // instantiate prefab or something..
        }

        public override float X { get; set; }
        public override float Y { get; set; }

    }
}
