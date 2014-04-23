using System;
using UnityEngine;

namespace BulletMLLib
{
	/// <summary>
	/// This task adds acceleration to a bullet.
	/// </summary>
	public class AccelTask : BulletMLTask
	{
	    /// <summary>
		/// How long to run this task... measured in frames
		/// </summary>
		public float Duration { get; private set; }

		/// <summary>
		/// The direction to accelerate in 
		/// </summary>
		private Vector2 _acceleration = Vector2.zero;
		
		/// <summary>
		/// Gets or sets the acceleration.
		/// </summary>
		/// <value>The acceleration.</value>
		public Vector2 Acceleration 
		{ 
			get
			{
				return _acceleration;
			}
		}

	    /// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public AccelTask(BulletNode node, BulletMLTask owner) : base(node, owner)
		{
			
		}

		/// <summary>
		/// this sets up the task to be run.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		protected override void SetupTask(Bullet bullet)
		{
			//set the accelerataion we are gonna add to the bullet
			Duration = Node.GetChildValue(NodeName.Term, this);

			//check for divide by 0
			if (Math.Abs(Duration) < 0.0f)
			{
				Duration = 1.0f;
			}

			//Get the horizontal node
            var horiz = Node.GetChild(NodeName.Horizontal) as BulletNode;
			if (null != horiz)
			{
				//Set the x component of the acceleration
				switch (horiz.NodeType)
				{
					case NodeType.Sequence:
					{
						//Sequence in an acceleration node means "add this amount every frame"
						_acceleration.x = horiz.GetValue(this);
					}
					break;

					case NodeType.Relative:
					{
						//accelerate by a certain amount
						_acceleration.x = horiz.GetValue(this) / Duration;
					}
					break;

					default:
					{
						//accelerate to a specific value
						_acceleration.x = (horiz.GetValue(this) - bullet.Acceleration.x) / Duration;
					}
					break;
				}
			}

			//Get the vertical node
            BulletNode vert = Node.GetChild(NodeName.Vertical) as BulletNode;
		    if (null == vert) return;

		    //set teh y component of the acceleration
		    switch (vert.NodeType)
		    {
		        case NodeType.Sequence:
		        {
		            //Sequence in an acceleration node means "add this amount every frame"
		            _acceleration.y = vert.GetValue(this);
		        }
		            break;

		        case NodeType.Relative:
		        {
		            //accelerate by a certain amount
		            _acceleration.y = vert.GetValue(this) / Duration;
		        }
		            break;

		        default:
		        {
		            //accelerate to a specific value
		            _acceleration.y = (vert.GetValue(this) - bullet.Acceleration.y) / Duration;
		        }
		            break;
		    }
		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>RunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public override RunStatus Run(Bullet bullet)
		{
			//Add the acceleration to the bullet
			bullet.Acceleration += Acceleration;

			//decrement the amount if time left to run and return End when this task is finished
			Duration -= 1.0f * bullet.TimeSpeed;
			if (Duration <= 0.0f)
			{
				TaskFinished = true;
				return RunStatus.End;
			}
			else 
			{
				//since this task isn't finished, run it again next time
				return RunStatus.Continue;
			}
		}
	}
}