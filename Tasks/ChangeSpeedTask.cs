using System;
namespace BulletMLLib
{
	/// <summary>
	/// This task changes the speed a little bit every frame.
	/// </summary>
	public class ChangeSpeedTask : BulletMLTask
	{
        private float _nodeSpeed;

        /// <summary>
        /// the type of speed change, pulled out of the node
        /// </summary>
        private NodeType _changeType;

        /// <summary>
        /// How long to run this task... measured in frames
        /// </summary>
        private float Duration { get; set; }

        /// <summary>
        /// How many frames this dude has ran
        /// </summary>
        private float RunDelta { get; set; }

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public ChangeSpeedTask(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{

		}

		/// <summary>
		/// this sets up the task to be run.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		protected override void SetupTask(Bullet bullet)
		{
            //set the length of time to run this dude
            Duration = Node.GetChildValue(NodeName.Term, this);

            //check for divide by 0
            if (Math.Abs(Duration) < 0.01)
            {
                Duration = 1.0f;
            }

            _nodeSpeed = Node.GetChildValue(NodeName.Speed, this);
            _changeType = Node.GetChild(NodeName.Speed).NodeType;
		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>RunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public override RunStatus Run(Bullet bullet)
		{
            bullet.Speed += GetSpeed(bullet);

            RunDelta += 1.0f * bullet.TimeSpeed;
            if (Duration <= RunDelta)
            {
                TaskFinished = true;
                return RunStatus.End;
            }

            //since this task isn't finished, run it again next time
            return RunStatus.Continue;

		}

        private float GetSpeed(Bullet bullet)
        {
            switch (_changeType)
            {
                case NodeType.Sequence:                    
                    return _nodeSpeed;                   
                case NodeType.Relative:                    
                    return _nodeSpeed / Duration;                    

                default:                    
                    return ((_nodeSpeed - bullet.Speed) / (Duration - RunDelta));                    
            }
        }

		#endregion //Methods
	}
}