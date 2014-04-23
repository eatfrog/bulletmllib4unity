using System;
using BulletMLLib4Unity;

namespace BulletMLLib
{
	/// <summary>
	/// This is a simple class used to create different types of nodes.
	/// </summary>
	public static class NodeFactory
	{
		/// <summary>
		/// Given a node type, create the correct node.
		/// </summary>
		/// <returns>An instance of the correct node type</returns>
		/// <param name="nodeType">Node type that we want.</param>
		public static BulletMLNode CreateNode(NodeName nodeType)
		{
			switch (nodeType)
			{
				case NodeName.Bullet:
				{
					return new BulletNode();
				}
				case NodeName.Action:
				{
					return new ActionNode();
				}
				case NodeName.Fire:
				{
					return new FireNode();
				}
				case NodeName.ChangeDirection:
				{
					return new ChangeDirectionNode();
				}
				case NodeName.ChangeSpeed:
				{
					return new ChangeSpeedNode();
				}
				case NodeName.Accel:
				{
					return new AccelNode();
				}
				case NodeName.Wait:
				{
					return new WaitNode();
				}
				case NodeName.Repeat:
				{
					return new RepeatNode();
				}
				case NodeName.BulletRef:
				{
					return new BulletRefNode();
				}
				case NodeName.ActionRef:
				{
					return new ActionRefNode();
				}
				case NodeName.FireRef:
				{
					return new FireRefNode();
				}
				case NodeName.Vanish:
				{
					return new VanishNode();
				}
				case NodeName.Horizontal:
				{
					return new HorizontalNode();
				}
				case NodeName.Vertical:
				{
					return new VerticalNode();
				}
				case NodeName.Term:
				{
					return new TermNode();
				}
				case NodeName.Times:
				{
					return new TimesNode();
				}
				case NodeName.Direction:
				{
					return new DirectionNode();
				}
				case NodeName.Speed:
				{
					return new SpeedNode();
				}
				case NodeName.Param:
				{
					return new ParamNode();
				}
				case NodeName.Bulletml:
				{
					return new BulletMLNode(NodeName.Bulletml);
				}
				default:
				{
					throw new Exception("Unhandled type of NodeName: \"" + nodeType.ToString() + "\"");
				}
			}
		}
	}
}
