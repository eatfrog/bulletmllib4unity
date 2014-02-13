using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulletMLLib;

namespace ExampleApplication
{
    public class FakeEmitter : Emitter
    {
        IBulletManager _bulletManager;
        Bullet rootBullet;
        public FakeEmitter(IBulletManager bulletManager, BulletPattern pattern, Bullet bl) : base(bulletManager, pattern, bl)
        {
            rootBullet = bl;
            _bulletManager = bulletManager;
        }
        
    }
}
