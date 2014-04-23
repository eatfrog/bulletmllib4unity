﻿namespace BulletMLLib
{
	/// <summary>
	/// An action task, this dude contains a list of tasks that are repeated
	/// </summary>
	public class ActionTask : BulletMLTask
	{
		#region Members

		/// <summary>
		/// The max number of times to repeat this action
		/// </summary>
		public int RepeatNumMax { get; private set; }

		/// <summary>
		/// The number of times this task has been run.
		/// This starts at 0 and the task will repeat until it hits the "max"
		/// </summary>
		public int RepeatNum { get; private set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.ActionTask"/> class.
		/// </summary>		
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public ActionTask(ActionNode node, BulletMLTask owner) : base(node, owner)
		{

			//set the number of times to repeat this action
			RepeatNumMax = node.RepeatNum(this);
		}

		/// <summary>
		/// Parse a specified node and bullet into this task
		/// </summary>		
		/// <param name="bullet">the bullet this dude is controlling</param>
		public override void ParseTasks(Bullet bullet)
		{
			//is this an actionref task?
			if (NodeName.ActionRef == Node.Name)
			{
				//add a sub task under this one for the referenced action
				ActionRefNode myActionRefNode = Node as ActionRefNode;

				//create the action task
				ActionTask actionTask = new ActionTask(myActionRefNode.ReferencedActionNode, this);

				//parse the children of the action node into the task
				actionTask.ParseTasks(bullet);

				//store the task
				ChildTasks.Add(actionTask);
			}

			//call the base class
			base.ParseTasks(bullet);
		}

		/// <summary>
		/// this sets up the task to be run.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		protected override void SetupTask(Bullet bullet)
		{
			RepeatNum = 0;
		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>RunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public override RunStatus Run(Bullet bullet)
		{
			//run the action until we hit the limit
			while (RepeatNum < RepeatNumMax)
			{
				RunStatus runStatus = base.Run(bullet);

				//What was the return value from running all teh child actions?
				switch (runStatus)
				{
					case RunStatus.End:
					{
						//The actions completed successfully, initialize everything and run it again
						RepeatNum++;

						//reset all the child tasks
						foreach (BulletMLTask task in ChildTasks)
						{
							task.InitTask(bullet);
						}
					}
					break;

					case RunStatus.Stop:
					{
						//Something in the child tasks paused this action
						return runStatus;
					}

					default:
					{
						//One of the child tasks needs to keep running next frame
						return RunStatus.Continue;
					}
				}
			}

			//if it gets here, all the child tasks have been run the correct number of times
			TaskFinished = true;
			return RunStatus.End;
		}

		#endregion //Methods
	}
}