namespace ExponeaSDK
{
    public enum TokenTrackFrequency
    {
        Daily,
        EveryLaunch,
        OnTokenChange

    }

    internal enum TokenTrackFrequencyInternal
    {
        /** Tracked once on days where the user opens the app */
        DAILY,
        /** Tracked every time the app is launched */
        EVERY_LAUNCH,
        /** Tracked on the first launch or if the token changes */
        ON_TOKEN_CHANGE
    }
}