namespace Bloomreach;

public class AppInboxAction
{
    public AppInboxAction(AppInboxActionType type, string title, string url)
    {
        Type = type;
        Title = title;
        Url = url;
    }

    public AppInboxActionType Type { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
}