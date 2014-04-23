using System;
using System.Xml;

namespace BulletMLLib
{
	/// <summary>
	/// Action reference node.
	/// This node type references another Action node.
	/// </summary>
	public class ActionRefNode : ActionNode
	{
		#region Members

		public ActionNode ReferencedActionNode { get; private set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.ActionRefNode"/> class.
		/// </summary>
		public ActionRefNode() : base(NodeName.ActionRef)
		{
		}

		/// <summary>
		/// Validates the node.
		/// Overloaded in child classes to validate that each type of node follows the correct business logic.
		/// This checks stuff that isn't validated by the XML validation
		/// </summary>
		public override void ValidateNode()
		{
			//do any base class validation
			base.ValidateNode();

			//Find the action node this dude references
			BulletMLNode refNode = GetRootNode().FindLabelNode(Label, NodeName.Action);

			//make sure we foud something
			if (null == refNode)
			{
				throw new NullReferenceException("Couldn't find the action node \"" + Label + "\"");
			}

			ReferencedActionNode = refNode as ActionNode;
			if (null == ReferencedActionNode)
			{
				throw new NullReferenceException("The BulletMLNode \"" + Label + "\" isn't an action node");
			}
		}

		#endregion //Methods
	}
}
