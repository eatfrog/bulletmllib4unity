using System;

namespace BulletMLLib
{
	/// <summary>
	/// This task changes the direction a little bit every frame
	/// </summary>
	public class ChangeDirectionTask : BulletMLTask
	{

        /// <summary>
        /// The amount pulled out of the node
        /// </summary>
        private float NodeDirection;

        /// <summary>
        /// the type of direction change, pulled out of the node
        /// </summary>
        private NodeType ChangeType;

        /// <summary>
        /// How long to run this task... measured in frames
        /// </summary>
        private float Duration { get; set; }

        /// <summary>
        /// How many frames this dude has ran
        /// </summary>
        private float RunDelta { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public ChangeDirectionTask(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{

		}

		/// <summary>
		/// this sets up the task to be run.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		protected override void SetupTask(Bullet bullet)
		{
            RunDelta = 0;

            //set the time length to run this dude
            Duration = Node.GetChildValue(NodeName.Term, this);

            //check for divide by 0
            if (Math.Abs(Duration) < 0.01)
            {
                Duration = 1.0f;
            }

            //Get the amount to change direction from the nodes
            DirectionNode dirNode = Node.GetChild(NodeName.Direction) as DirectionNode;
		    if (dirNode != null)
		    {
		        NodeDirection = dirNode.GetValue(this) * (float)Math.PI / 180.0f; //also make sure to convert to radians

		        //How do we want to change direction?
		        ChangeType = dirNode.NodeType;
		    }
		}
		
		public override RunStatus Run(Bullet bullet)
		{
            //change the direction of the bullet by the correct amount
            bullet.Direction += GetDirection(bullet);

            //decrement the amount if time left to run and return End when this task is finished
            RunDelta += 1.0f * bullet.TimeSpeed;
            if (Duration <= RunDelta)
            {
                TaskFinished = true;
                return RunStatus.End;
            }
            
            //since this task isn't finished, run it again next time
            return RunStatus.Continue;
            		    
		}

        private float GetDirection(Bullet bullet)
        {
            //How do we want to change direction?
            float direction;
            switch (ChangeType)
            {
                case NodeType.Sequence:
                    {
                        //We are going to add this amount to the direction every frame
                        direction = NodeDirection;
                    }
                    break;

                case NodeType.Absolute:
                    {
                        //We are going to go in the direction we are given, regardless of where we are pointing right now
                        direction = NodeDirection - bullet.Direction;
                    }
                    break;

                case NodeType.Relative:
                    {
                        //The direction change will be relative to our current direction
                        direction = NodeDirection;
                    }
                    break;

                default:
                    {
                        //the direction change is to aim at the enemy
                        direction = ((NodeDirection + bullet.GetAngleTowardsPlayer()) - bullet.Direction);
                    }
                    break;
            }

            //keep the direction between -180 and 180
            direction = WrapAngle(direction);

            //The sequence type of change direction is unaffected by the duration
            if (ChangeType == NodeType.Absolute)
            {
                //divide by the amount fo time remaining
                direction /= Duration - RunDelta;
            }
            else if (ChangeType != NodeType.Sequence)
            {
                //Divide by the duration so we ease into the direction change
                direction /= Duration;
            }

            return direction;
        }

	    private static float WrapAngle(float direction)
	    {
	        float ret = direction;            

            //keep the direction between 0-360
            if (ret > 2 * Math.PI)
            {
                ret -= (float)(2 * Math.PI);
            }
            else if (ret < 0)
            {
                ret += (float)(2 * Math.PI);
            }
	        return ret;
	    }

	}
}