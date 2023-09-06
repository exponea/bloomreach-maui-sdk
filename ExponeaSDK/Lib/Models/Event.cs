namespace Exponea;

public class Event : AttributedObject
{
    public Event(string eventName)
    {
        Name = eventName;
    }

    public string Name { get; set; }
}