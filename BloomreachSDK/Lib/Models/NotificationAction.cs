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
    
    public NotificationAction WithAttribute(string key, object? value)
    {
        return WithAttributes(new Dictionary<string, object?>() {
            { key, value }
        });
    }

    public NotificationAction WithAttributes(IDictionary<string, object?> properties)
    {
        foreach (var each in properties)
        {
            Attributes[each.Key] = each.Value;
        }
        return this;
    }
}