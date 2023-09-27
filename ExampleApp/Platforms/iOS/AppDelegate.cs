using Bloomreach;
using Foundation;
using UIKit;
using UserNotifications;

namespace ExampleApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate, IUNUserNotificationCenterDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
	{
		UNUserNotificationCenter.Current.Delegate = this;
		Console.WriteLine("APNS-BR notif center set");
		return base.FinishedLaunching(application, launchOptions);
	}
	
	[Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
	public void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
	{
        BloomreachSDK.HandlePushToken(deviceToken);
    }

	[Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
	public void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
	{
		Console.WriteLine("APNS-BR Push notification action pure ");
		BloomreachSDK.HandlePushNotificationOpened(NotificationAction.Parse(userInfo));
		completionHandler(UIBackgroundFetchResult.NewData);
	}

	[Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
	public void WillPresentNotification(UNUserNotificationCenter notificationCenter, UNNotification notification,
		Action<UNNotificationPresentationOptions> completionHandler)
	{
		completionHandler(
			UNNotificationPresentationOptions.Sound
			| UNNotificationPresentationOptions.Alert
			| UNNotificationPresentationOptions.Badge
		);
	}

	[Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
	public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
	{
		Console.WriteLine("APNS-BR Push notification action click");
		BloomreachSDK.HandlePushNotificationOpened(NotificationAction.Parse(response));
		completionHandler();
	}

	public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity,
		UIApplicationRestorationHandler completionHandler)
	{
		var campaignUrl = userActivity.WebPageUrl?.ToString();
		if (userActivity.ActivityType == "NSUserActivityTypeBrowsingWeb" && campaignUrl != null)
		{
			BloomreachSDK.HandleCampaignClick(new Uri(campaignUrl));
			return userActivity.WebPageUrl.Host == "old.panaxeo.com";
		}

		return false;
	}

	public override bool OpenUrl(UIApplication application, NSUrl url, NSDictionary options)
	{
		BloomreachSDK.HandleCampaignClick(url);
		return url.Host == "old.panaxeo.com";
	}
}

