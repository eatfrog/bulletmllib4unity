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

        public FakeBullet(FakeBulletManager myBulletManager)
            : base(myBulletManager)
        {

        }


        public FakeBullet(FakeBulletManager myBulletManager, Emitter e) : base(myBulletManager)
        {
            Emitter = e;
        }

        public Emitter Emitter { get; set; }
        public override float X
        {
            get
            {
                return 0f;
            }
            set
            {
                
            }
        }

        public override float Y
        {
            get
            {
                return 0f;
            }
            set
            {                
            }
        }

        public override void BulletSpawned()
        {
           
            Console.WriteLine("New bullet spawned. PatterName: " + Node.GetPatternName());

            // instantiate prefab or something..
        }
    }
}
