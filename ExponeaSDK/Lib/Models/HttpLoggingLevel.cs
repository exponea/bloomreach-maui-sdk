using System.Runtime.Serialization;

namespace Exponea
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