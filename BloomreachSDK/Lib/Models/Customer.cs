using System;

namespace Bloomreach
{
	public class Customer
    {

        public Dictionary<string, string> CustomerIds { get; set; } = new ();
        public Dictionary<string, object?> Properties { get; set; } = new ();

        public Customer(string registered) : this("registered", registered) { }

        public Customer(string key, string value) : this(new Dictionary<string, string>(){{ key, value }}) { }

        public Customer(IDictionary<string, string> customerIds) {
            foreach (var each in customerIds)
            {
                CustomerIds[each.Key] = each.Value;
            }
        }

        public Customer WithProperty(string key, object? value)
        {
            return WithProperties(new Dictionary<string, object?>() {
                { key, value }
            });
        }

        public Customer WithProperties(IDictionary<string, object?> properties)
        {
            foreach (var each in properties)
            {
                Properties[each.Key] = each.Value;
            }
            return this;
        }
    }
}

