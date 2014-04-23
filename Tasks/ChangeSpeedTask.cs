using System;


namespace BulletMLLib
{
	/// <summary>
	/// This task changes the speed a little bit every frame.
	/// </summary>
	public class ChangeSpeedTask : BulletMLTask
	{
		#region Members

		/// <summary>
		/// The amount to change speed every frame
		/// </summary>
		private float SpeedChange { get; set; }

		/// <summary>
		/// How long to run this task... measured in frames
		/// </summary>
		private float Duration { get; set; }

		#endregion //Members

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

			switch (Node.GetChild(NodeName.Speed).NodeType)
			{
				case NodeType.Sequence:
				{
					SpeedChange = Node.GetChildValue(NodeName.Speed, this);
				}
				break;

				case NodeType.Relative:
				{
					SpeedChange = Node.GetChildValue(NodeName.Speed, this) / Duration;
				}
				break;

				default:
				{
					SpeedChange = (Node.GetChildValue(NodeName.Speed, this) - bullet.Speed) / Duration;
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
			bullet.Speed += SpeedChange;

			Duration -= 1.0f * bullet.TimeSpeed;
			if (Duration <= 0.0f)
			{
				TaskFinished = true;
				return RunStatus.End;
			}
			else
			{
				return RunStatus.Continue;
			}
		}

		#endregion //Methods
	}
}