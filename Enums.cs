using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulletMLLib
{
    /// <summary>
    /// Different types of bullet patterns
    /// </summary>
    public enum PatternType
    {
        Vertical,
        Horizontal,
        None
    }

    public enum NodeType
    {
        None,
        Aim,
        Absolute,
        Relative,
        Sequence
    };

    public enum NodeName
    {
        Bullet,
        Action,
        Fire,
        ChangeDirection,
        ChangeSpeed,
        Accel,
        Wait,
        Repeat,
        BulletRef,
        ActionRef,
        FireRef,
        Vanish,
        Horizontal,
        Vertical,
        Term,
        Times,
        Direction,
        Speed,
        Param,
        Bulletml
    };

    /// <summary>
	/// Theese are used for tasks during runtime...
	/// </summary>
	public enum RunStatus
	{
		Continue, //keep parsing this task
		End, //this task is finished parsing
		Stop //this task is paused
	}
}
