namespace Bloomreach;

public class AppInboxMessage
{
    public AppInboxMessage(
        string id,
        AppInboxMessageType type,
        bool isRead,
        double receivedTime,
        IDictionary<string, object> content)
    {
        Id = id;
        this.Type = type;
        IsRead = isRead;
        ReceivedTime = receivedTime;
        this.Content = content;
    }

    public string Id { get; set; }
    public AppInboxMessageType Type { get; set; }
    public bool IsRead { get; set; }
    public double ReceivedTime { get; set; }
    public IDictionary<string, object> Content { get; set; }
}