using System.Runtime.Serialization;

namespace Bloomreach
{
    public enum HttpLoggingLevel
    {
        [EnumMember(Value = "none")]
        NONE,
        [EnumMember(Value = "basic")]
        BASIC,
        [EnumMember(Value = "headers")]
        HEADERS,
        [EnumMember(Value = "body")]
        BODY
    }
}