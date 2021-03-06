﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletMLLib
{
	/// <summary>
	/// This is the bullet class that outside assemblies will interact with.
	/// Just inherit from this class and override the abstract functions!
	/// </summary>
	public abstract class Bullet
	{
		/// <summary>
		/// A bullet manager that manages this bullet.
		/// </summary>
		/// <value>My bullet manager.</value>
		private readonly IBulletManager _bulletManager;

		/// <summary>
		/// The tree node that describes this bullet.  These are shared between multiple bullets
		/// </summary>
		public BulletMLNode Node { get; private set; }

		/// <summary>
		/// How fast time moves in this game.
		/// Can be used to do slowdown, speedup, etc.
		/// </summary>
		/// <value>The time speed.</value>
		public float TimeSpeed { get; set; }

		/// <summary>
		/// Change the size of this bulletml script
		/// If you want to reuse a script for a game but the size is wrong, this can be used to resize it
		/// </summary>
		/// <value>The scale.</value>
		public float Scale { get; set; }

		/// <summary>
		/// The acceleration of this bullet
		/// </summary>
		/// <value>The accel, in pixels/frame^2</value>
		public Vector2 Acceleration { get; set; }

		/// <summary>
		/// Gets or sets the speed
		/// </summary>
		/// <value>The speed, in pixels/frame</value>
		public virtual float Speed { get; set; }

		/// <summary>
		/// A list of tasks that will define this bullets behavior
		/// </summary>
		public List<BulletMLTask> Tasks { get; private set; }

		// X/Y position
		public abstract float X { get; set; }
		public abstract float Y { get; set; }

		/// <summary>
		/// Gets my bullet manager.
		/// </summary>
		/// <value>My bullet manager.</value>
		public IBulletManager MyBulletManager
		{
			get
			{
				return _bulletManager;
			}
		}

        // who is firing this? Needs to be set by the implementation of CreateBullet
        public Emitter Emitter { get; set; }


        private float _direction;
		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction in radians.</value>
		public virtual float Direction
		{
			get
			{
				return _direction;
			}
			set
			{
				_direction = value;

				//keep the direction between 0-360
				if (_direction > 2 * Math.PI)
				{
					_direction -= (float)(2 * Math.PI);
				}
				else if (_direction < 0)
				{
					_direction += (float)(2 * Math.PI);
				}
			}
		}

		/// <summary>
		/// Convenience property to get the label of a bullet.
		/// </summary>
		/// <value>The label.</value>
		public string Label
		{
			get
			{
				return Node.Label;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.Bullet"/> class.
		/// </summary>
		/// <param name="myBulletManager">My bullet manager.</param>
		protected Bullet(IBulletManager myBulletManager)
		{			
			_bulletManager = myBulletManager;

			Acceleration = Vector2.zero;

			Tasks = new List<BulletMLTask>();

			//init these to the default
			TimeSpeed = 1.0f;
			Scale = 1.0f;
		}

		
		/// <summary>
		/// This bullet is fired from another bullet, initialize it from the node that fired it
		/// </summary>
		/// <param name="subNode">Sub node that defines this bullet</param>
		public void InitNode(BulletMLNode subNode)
		{
			//clear everything out
			Tasks.Clear();
			
			//Grab that top level node
			Node = subNode;

			//found a top num node, add a task for it
			BulletMLTask task = new BulletMLTask(subNode, null);

			//parse the nodes into the task list
			task.ParseTasks(this);

			//initialize all the tasks
			task.InitTask(this);

			Tasks.Add(task);
		}

		/// <summary>
        /// Update this bullet.
        /// If you set timespeed to consider Time.deltaTime you can update as much as you want
		/// </summary>
		public virtual void Update()
		{
            if (Emitter != null) // when emitter is gone, let the bullet fly away without further actions            
                Tasks.ForEach(x => x.Run(this));
            
            float speed = (Speed * TimeSpeed) * Scale;
			X += Acceleration.x + (float)(Math.Sin(Direction) * speed);
			Y += Acceleration.y + (float)(-Math.Cos(Direction) * speed);
		}

		/// <summary>
		/// Get player direction if we're aiming towards him
		/// </summary>
		/// <returns>angle to target the bullet</returns>
		public float GetAngleTowardsPlayer()
		{		
			Vector2 shipPos = MyBulletManager.PlayerPosition(this);
		    return (float)Math.Atan2((shipPos.x - X), -(shipPos.y - Y));
		}

		/// <summary>
		/// Finds the task by label.
		/// This recurses into child tasks to find the taks with the correct label
		/// Used only for unit testing!
		/// </summary>
		/// <returns>The task by label.</returns>
		/// <param name="strLabel">String label.</param>
		public BulletMLTask FindTaskByLabel(string strLabel)
		{
			//check if any of the child tasks have a task with that label
			foreach (BulletMLTask childTask in Tasks)
			{
				BulletMLTask foundTask = childTask.FindTaskByLabel(strLabel);
				if (null != foundTask)
				{
					return foundTask;
				}
			}

			return null;
		}

		/// <summary>
		/// Given a label and name, find the task that matches
		/// </summary>
		/// <returns>The task by label and name.</returns>
		/// <param name="strLabel">String label of the task</param>
		/// <param name="name">The name of the node the task should be attached to</param>
		public BulletMLTask FindTaskByLabelAndName(string strLabel, NodeName name)
		{
			//check if any of teh child tasks have a task with that label
			foreach (BulletMLTask childTask in Tasks)
			{
				BulletMLTask foundTask = childTask.FindTaskByLabelAndName(strLabel, name);
				if (null != foundTask)
				{
					return foundTask;
				}
			}

			return null;
		}


        // Needs to be run after node init
        public abstract void BulletSpawned();
	}
}
