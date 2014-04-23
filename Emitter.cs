using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletMLLib;

namespace BulletMLLib
{
    
    /// <summary>
    /// The enemy, or such, that is emitting the bullets
    /// </summary>
    public class Emitter
    {
        // This is not really a bullet, but its pattern holder. needed for x, y and such, for now at least.   
        private readonly Bullet _rootBullet;
        public BulletPattern Pattern { get; private set; }

        private readonly IBulletManager _bulletManager;

        public Emitter(IBulletManager bulletManager, BulletPattern pattern, Bullet rootBullet)
        {
            _bulletManager = bulletManager;
            Pattern = pattern;
            _rootBullet = rootBullet;
            _rootBullet.Emitter = this;
            InitTopNode(pattern.RootNode);
        }

        public void ClearTasks()
        {
            _rootBullet.Tasks.Clear();
        }
        public void Update(float x, float y)
        {
            _rootBullet.X = x;
            _rootBullet.Y = y;
            _rootBullet.Update();
        }

        /// <summary>
        /// Initialize this bullet with a top level node
        /// </summary>
        /// <param name="rootNode">This is a top level node... find the first "top" node and use it to define this bullet</param>
        public void InitTopNode(BulletMLNode rootNode)
        {

            //okay find the item labelled 'top'
            bool bValidBullet = false;
            BulletMLNode topNode = rootNode.FindLabelNode("top", NodeName.Action);
            if (topNode != null)
            {
                //initialize with the top node we found!
                _rootBullet.InitNode(topNode);
                bValidBullet = true;
                _rootBullet.BulletSpawned();
            }
            else
            {
                //ok there is no 'top' node, so that means we have a list of 'top#' nodes
                for (int i = 1; i < 10; i++)
                {
                    topNode = rootNode.FindLabelNode("top" + i, NodeName.Action);
                    if (topNode != null)
                    {
                        if (!bValidBullet)
                        {
                            //Use this bullet!
                            _rootBullet.InitNode(topNode);
                            bValidBullet = true;
                            _rootBullet.BulletSpawned();
                        }
                        else
                        {
                            //Create a new bullet
                            Bullet b = _bulletManager.CreateBullet(this);

                            //set the position to this dude's position
                            b.X = _rootBullet.X;
                            b.Y = _rootBullet.Y;

                            //initialize with the node we found
                            b.InitNode(topNode);
                            b.BulletSpawned();
                        }
                    }
                }
            }

            if (!bValidBullet)
            {
                //We didnt find a "top" node for this dude, remove him from the game.
                _bulletManager.RemoveBullet(_rootBullet);
            }
        }
    }
}
