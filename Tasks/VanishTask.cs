using System.Diagnostics;
using BulletMLLib4Unity;

namespace BulletMLLib
{
	/// <summary>
	/// This task removes a bullet from the game.
	/// </summary>
	public class VanishTask : BulletMLTask
	{
		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="BulletMLLib.BulletMLTask"/> class.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="owner">Owner.</param>
		public VanishTask(BulletMLNode node, BulletMLTask owner) : base(node, owner)
		{

		}

		/// <summary>
		/// Run this task and all subtasks against a bullet
		/// This is called once a frame during runtime.
		/// </summary>
		/// <returns>RunStatus: whether this task is done, paused, or still running</returns>
		/// <param name="bullet">The bullet to update this task against.</param>
		public override RunStatus Run(Bullet bullet)
		{
			//remove the bullet via the bullet manager interface
			IBulletManager manager = bullet.MyBulletManager;

			manager.RemoveBullet(bullet);
			return RunStatus.End;
		}

		#endregion //Methods
	}
}