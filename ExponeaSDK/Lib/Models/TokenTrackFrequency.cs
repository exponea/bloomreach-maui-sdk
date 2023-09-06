using System.Runtime.Serialization;

namespace Exponea
{
    public enum TokenTrackFrequency
    {
        [EnumMember(Value = "daily")] 
        Daily,
        [EnumMember(Value = "every_launch")] 
        EveryLaunch,
        [EnumMember(Value = "token_change")] 
        OnTokenChange
    }
}