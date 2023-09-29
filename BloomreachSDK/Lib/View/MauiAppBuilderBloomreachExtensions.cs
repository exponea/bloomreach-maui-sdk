#if ANDROID
using Bloomreach.Platforms.Android;
#endif

namespace Bloomreach.View;

public static class MauiAppBuilderBloomreachExtensions
{
    public static MauiAppBuilder RegisterBloomreachUi(
        this MauiAppBuilder builder)
    {
        builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            handlers.AddHandler(typeof(AppInboxButton), typeof(AppInboxButtonAndroidHandler));
#elif IOS

#endif
        });
        return builder;
    }
}