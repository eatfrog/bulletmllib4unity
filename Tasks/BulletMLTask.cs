using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace BulletMLLib
{
	/// <summary>
	/// This is a task..each task is the action from a single xml node, for one bullet.
	/// basically each bullet makes a tree of these to match its pattern
	/// </summary>
	public class BulletMLTask
	{

		/// <summary>
		/// A list of child tasks of this dude
		/// </summary>
		public List<BulletMLTask> ChildTasks { get; private set; }

		/// <summary>
		/// The parameter list for this task
		/// </summary>
		public List<float> ParamList { get; private set; }

		/// <summary>
		/// the parent task of this dude in the tree
		/// Used to fetch param values.
		/// </summary>
		public BulletMLTask Owner { get; set; }

		/// <summary>
		/// The bullet ml node that this dude represents
		/// </summary>
		public BulletMLNode Node { get; private set; }

		/// <summary>
		/// whether or not this task has finished running
		/// </summary>
		public bool TaskFinished { get; protected set; }

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public BulletMLTask(BulletMLNode node, BulletMLTask owner)
		{
			if (null == node)
			{
				throw new NullReferenceException("node argument cannot be null");
			}

			ChildTasks = new List<BulletMLTask>();
			ParamList = new List<float>();
			TaskFinished = false;
			Owner = owner;
			Node = node; 
		}

		/// <summary>
		/// Parse a specified node and bullet into this task
		/// </summary>
		/// <param name="myNode">the node for this dude</param>
		/// <param name="bullet">the bullet this dude is controlling</param>
		public virtual void ParseTasks(Bullet bullet)
		{
			if (null == bullet)
			{
				throw new NullReferenceException("bullet argument cannot be null");
			}

			foreach (BulletMLNode childNode in Node.ChildNodes)
			{
				ParseChildNode(childNode, bullet);
			}
		}

	    /// <summary>
	    /// Parse a specified node and bullet into this task
	    /// </summary>	    
	    /// <param name="childNode"></param>
	    /// <param name="bullet">the bullet this dude is controlling</param>
	    public virtual void ParseChildNode(BulletMLNode childNode, Bullet bullet)
		{
			
			switch (childNode.Name)
			{
				case NodeName.Repeat:
				{

					//create a placeholder bulletmltask for the repeat node
                    RepeatTask repeatTask = new RepeatTask(childNode, this);

					//parse the child nodes into the repeat task
					repeatTask.ParseTasks(bullet);

					//store the task
					ChildTasks.Add(repeatTask);
				}
				break;
			
				case NodeName.Action:
				{
					//convert the node to an ActionNode
					ActionNode myActionNode = childNode as ActionNode;

					//create the action task
					ActionTask actionTask = new ActionTask(myActionNode, this);

					//parse the children of the action node into the task
					actionTask.ParseTasks(bullet);

					//store the task
					ChildTasks.Add(actionTask);
				}
				break;
		
				case NodeName.ActionRef:
				{
					//convert the node to an ActionNode
					ActionRefNode myActionNode = childNode as ActionRefNode;

					//create the action task
					ActionTask actionTask = new ActionTask(myActionNode, this);

					//add the params to the action task
					foreach (BulletMLNode node in childNode.ChildNodes)
					{
					    actionTask.ParamList.Add(node.GetValue(this));
					}

				    //parse the children of the action node into the task
					actionTask.ParseTasks(bullet);

					//store the task
					ChildTasks.Add(actionTask);
				}
				break;
	
				case NodeName.ChangeSpeed:
				{
                    ChildTasks.Add(new ChangeSpeedTask(childNode as BulletNode, this));
				}
				break;
	
				case NodeName.ChangeDirection:
				{
                    ChildTasks.Add(new ChangeDirectionTask(childNode as BulletNode, this));
				}
				break;

				case NodeName.Fire:
				{
					//convert the node to a fire node
					FireNode myFireNode = childNode as FireNode;

					//create the fire task
					FireTask fireTask = new FireTask(myFireNode, this);

					//parse the children of the fire node into the task
					fireTask.ParseTasks(bullet);

					//store the task
					ChildTasks.Add(fireTask);
				}
				break;

				case NodeName.FireRef:
				{
					//convert the node to a fireref node
					FireRefNode myFireNode = childNode as FireRefNode;

					//create the fire task
					FireTask fireTask = new FireTask(myFireNode.ReferencedFireNode, this);

					//add the params to the fire task
					for (int i = 0; i < childNode.ChildNodes.Count; i++)
					{
						fireTask.ParamList.Add(childNode.ChildNodes[i].GetValue(this));
					}

					//parse the children of the action node into the task
					fireTask.ParseTasks(bullet);

					//store the task
					ChildTasks.Add(fireTask);
				}
				break;

				case NodeName.Wait:
				{
                    ChildTasks.Add(new WaitTask(childNode as BulletNode, this));
				}
				break;

				case NodeName.Vanish:
				{
                    ChildTasks.Add(new VanishTask(childNode as BulletNode, this));
				}
				break;

				case NodeName.Accel:
				{
                    ChildTasks.Add(new AccelTask(childNode as BulletNode, this));
				}
				break;
			}
		}

		/// <summary>
		/// This gets called when nested repeat nodes get initialized.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		public virtual void HardReset(Bullet bullet)
		{
			TaskFinished = false;

			foreach (BulletMLTask task in ChildTasks)
			{
				task.HardReset(bullet);
			}

			SetupTask(bullet);
		}

		/// <summary>
		/// Init this task and all its sub tasks.  
		/// This method should be called AFTER the nodes are parsed, but BEFORE run is called.
		/// </summary>
		/// <param name="bullet">the bullet this dude is controlling</param>
		public virtual void InitTask(Bullet bullet)
		{
			TaskFinished = false;

			foreach (BulletMLTask task in ChildTasks)
			{
				task.InitTask(bullet);
			}

			SetupTask(bullet);
		}

		/// <summary>
		/// this sets up the task to be run.
		/// </summary>
		/// <param name="bullet">Bullet.</param>
		protected virtual void SetupTask(Bullet bullet)
		{
			//overload in child classes
		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>RunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public virtual RunStatus Run(Bullet bullet)
		{
			//run all the child tasks
			TaskFinished = true;
			foreach (BulletMLTask t in ChildTasks)
			{
                //is the child task finished running?
			    if (!t.TaskFinished)
			    {
			        //Run the child task...
			        RunStatus childStatus = t.Run(bullet);
			        if (childStatus == RunStatus.Stop)
			        {
			            //The child task is paused, so it is not finished
			            TaskFinished = false;
			            return childStatus;
			        }

			        if (childStatus == RunStatus.Continue)
			        {
			            //child task needs to do some more work
			            TaskFinished = false;
			        }
			    }
			}

		    return (TaskFinished ?  RunStatus.End : RunStatus.Continue);
		}

		/// <summary>
		/// Get the value of a parameter of this task.
		/// </summary>
		/// <returns>The parameter value.</returns>
		/// <param name="iParamNumber">the index of the parameter to get</param>
		public float GetParamValue(int iParamNumber)
		{
			//if that task doesn't have any params, go up until we find one that does
			if (ParamList.Count < iParamNumber)
			{
				//the current task doens't have enough params to solve this value
				return null != Owner ? Owner.GetParamValue(iParamNumber) : 0.0f;
			}
			
			//the value of that param is the one we want
			return ParamList[iParamNumber - 1];
		}

		/// <summary>
		/// Gets the node value.
		/// </summary>
		/// <returns>The node value.</returns>
		public float GetNodeValue()
		{
			return Node.GetValue(this);
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
			//check if this is the corretc task
			if (strLabel == Node.Label)
			{
				return this;
			}

			//check if any of teh child tasks have a task with that label
			foreach (BulletMLTask childTask in ChildTasks)
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
		/// given a label and name, find the task that matches
		/// </summary>
		/// <returns>The task by label and name.</returns>
		/// <param name="strLabel">String label of the task</param>
		/// <param name="name">the name of the node the task should be attached to</param>
		public BulletMLTask FindTaskByLabelAndName(string strLabel, NodeName name)
		{
			//check if this is the corretc task
			if ((strLabel == Node.Label) && (name == Node.Name))
			{
				return this;
			}

			//check if any of teh child tasks have a task with that label
			foreach (BulletMLTask childTask in ChildTasks)
			{
				BulletMLTask foundTask = childTask.FindTaskByLabelAndName(strLabel, name);
				if (null != foundTask)
				{
					return foundTask;
				}
			}

			return null;
		}

		#endregion //Methods
	}
}