using System;
using System.Diagnostics;

namespace BulletMLLib
{
	/// <summary>
	/// This task sets the direction of a bullet
	/// </summary>
	public class SetDirectionTask : BulletMLTask
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public SetDirectionTask(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{

		}

	}
}