using System;
namespace Bloomreach
{
	public class AttributedObject
	{
        
        public Dictionary<string, object?> Attributes { get; set; } = new ();

        public object? this[string key]
        {
            get => Attributes.TryGetValue(key, out var value) ? value : null;
            set => Attributes[key] = value;
        }
    }
}

