using Microsoft.Maui.Handlers;
using UIKit;

namespace Bloomreach.Platforms.iOS;

public class AppInboxButtonIosHandler : ButtonHandler
{
    protected override UIButton CreatePlatformView()
    {
        return BloomreachSDK.GetAppInboxNativeButton() as UIButton ?? CreateFallbackButtonInstance();
    }

    private UIButton CreateFallbackButtonInstance()
    {
        var fallbackButtonInstance = new UIButton();
        fallbackButtonInstance.TitleLabel.Text = "AppInbox not ready";
        return fallbackButtonInstance;
    }
}