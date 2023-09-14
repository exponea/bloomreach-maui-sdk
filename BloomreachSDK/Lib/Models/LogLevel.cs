using System;
using System.Runtime.Serialization;

namespace Bloomreach
{
	public enum LogLevel
	{
		[EnumMember(Value = "unknown")]
        Unknown,
        [EnumMember(Value = "off")]
        Off,
        [EnumMember(Value = "error")]
        Error,
        [EnumMember(Value = "warning")]
        Warn,
        [EnumMember(Value = "info")]
        Info,
        [EnumMember(Value = "debug")]
        Debug,
        [EnumMember(Value = "verbose")]
        Verbose,
    }
}

