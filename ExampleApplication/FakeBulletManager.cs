using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletMLLib;
using UnityEngine;

namespace ExampleApplication
{
    public class FakeBulletManager : IBulletManager
    {
        public List<FakeBullet> bullets = new List<FakeBullet>();
        public Vector2 PlayerPosition(Bullet targettedBullet)
        {
            return new Vector2(0, 0);
        }

        public void RemoveBullet(Bullet deadBullet)
        {
            Console.WriteLine("Bullet removed");
            return;
        }

        public Bullet CreateBullet(Emitter e)
        {
            Console.WriteLine("New bullet created");
            var bullet = new FakeBullet(this, e);
            bullets.Add(bullet);
            return bullet;
        }
    }
}
