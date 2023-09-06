using System;
using System.Runtime.Serialization;

namespace Exponea
{
	public enum FlushMode
	{
		[EnumMember(Value = "unknown")]
        Unknown,
        [EnumMember(Value = "app_close")]
        AppClose,
        [EnumMember(Value = "immediate")]
        Immediate,
        [EnumMember(Value = "manual")]
        Manual,
        [EnumMember(Value = "period")]
        Period,
    }
}

