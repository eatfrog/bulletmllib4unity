using System;

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
					return new BulletNode(NodeName.ChangeSpeed);
				}
				case NodeName.Accel:
				{
					return new BulletNode(NodeName.Accel);
				}
				case NodeName.Wait:
				{
                    return new BulletNode(NodeName.Wait);
				}
				case NodeName.Repeat:
				{
                    return new BulletNode(NodeName.Repeat);
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
                    return new BulletNode(NodeName.Vanish);
				}
				case NodeName.Horizontal:
				{
					return new BulletNode(NodeName.Horizontal);
				}
				case NodeName.Vertical:
				{
                    return new BulletNode(NodeName.Vertical);
				}
				case NodeName.Term:
				{
                    return new BulletNode(NodeName.Term);
				}
				case NodeName.Times:
				{
                    return new BulletNode(NodeName.Times);
				}
				case NodeName.Direction:
				{
					return new DirectionNode();
				}
				case NodeName.Speed:
				{
                    return new BulletNode(NodeName.Speed);
				}
				case NodeName.Param:
				{
					return new BulletNode(NodeName.Param);
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
