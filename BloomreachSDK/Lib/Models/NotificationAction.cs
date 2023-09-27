using System.Collections.Generic;
#if IOS
using Bloomreach.Utils;
using Foundation;
using UserNotifications;
#endif

namespace Bloomreach;

public class NotificationAction
{
    public NotificationAction(string actionType, string actionName, string url)
    {
        ActionType = actionType;
        ActionName = actionName;
        Url = url;
    }

    public string ActionType { get; set; }
    public string ActionName { get; set; }
    public string Url { get; set; }
    
    public string? ActionIdentifier { get; set; }
    
    public Dictionary<string, object?> Attributes { get; set; } = new ();
    
    public NotificationAction WithAttribute(string key, object value)
    {
        return WithAttributes(new Dictionary<string, object>() {
            { key, value }
        });
    }

    public NotificationAction WithAttributes(IDictionary<string, object> properties)
    {
        foreach (var each in properties)
        {
            Attributes[each.Key] = each.Value;
        }
        return this;
    }

#if IOS
    public static NotificationAction Parse(UNNotificationResponse response)
    {
#pragma warning disable CA1416
        var payload = response.Notification.Request.Content.UserInfo;
        var identifier = response.ActionIdentifier.ToString();
#pragma warning restore CA1416
        var target = new NotificationAction(
            identifier, identifier, identifier
        ).WithAttributes(ConverterUtils.NormalizeDictionary(payload));
        target.ActionIdentifier = identifier;
        return target;
    }
    
    public static NotificationAction Parse(NSDictionary userInfo)
    {
        return new NotificationAction(
            "", "", ""
        ).WithAttributes(ConverterUtils.NormalizeDictionary(userInfo));
    }
#endif
}