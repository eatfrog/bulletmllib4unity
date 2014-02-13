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
        public Vector2 PlayerPosition(Bullet targettedBullet)
        {
            return new Vector2(0, 0);
        }

        public void RemoveBullet(Bullet deadBullet)
        {
            Console.WriteLine("Bullet removed");
            return;
        }

        public Bullet CreateBullet()
        {
            Console.WriteLine("New bullet created");
            return new FakeBullet(this);
        }
    }
}
