using System.Runtime.Serialization;

namespace Bloomreach
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