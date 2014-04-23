using System;
using System.Xml;
using BulletMLLib4Unity;

namespace BulletMLLib
{
	public class BulletNode : BulletMLNode
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletNode"/> class.
		/// </summary>
		public BulletNode() : this(NodeName.Bullet)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletNode"/> class.
		/// this is the constructor used by sub classes
		/// </summary>
		/// <param name="nodeType">the node type.</param>
		public BulletNode(NodeName nodeType) : base(nodeType)
		{
		}
	}
}
