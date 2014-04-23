using System;
using System.Xml;
using System.Diagnostics;
using BulletMLLib4Unity;

namespace BulletMLLib
{
	public class FireNode : BulletMLNode
	{
		#region Members

		/// <summary>
		/// A bullet node this task will use to set any bullets shot from this task
		/// </summary>
		/// <value>The bullet node.</value>
		public BulletNode BulletDescriptionNode { get; set; }

		#endregion //Members

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.FireNode"/> class.
		/// </summary>
		public FireNode() : this(NodeName.Fire)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.FireNode"/> class.
		/// this is the constructor used by sub classes
		/// </summary>
		/// <param name="nodeType">the node type.</param>
		public FireNode(NodeName nodeType) : base(nodeType)
		{
		}

		/// <summary>
		/// Validates the node.
		/// Overloaded in child classes to validate that each type of node follows the correct business logic.
		/// This checks stuff that isn't validated by the XML validation
		/// </summary>
		public override void ValidateNode()
		{
			base.ValidateNode();

			//check for a bullet node
			BulletDescriptionNode = GetChild(NodeName.Bullet) as BulletNode;
			
		    if (null != BulletDescriptionNode) return;

		    //make sure that dude knows what he's doing
		    BulletRefNode refNode = GetChild(NodeName.BulletRef) as BulletRefNode;
		    if (refNode != null)
		    {
		        refNode.FindMyBulletNode();
		        BulletDescriptionNode = refNode.ReferencedBulletNode;
		    }
		}

		#endregion Methods
	}
}
