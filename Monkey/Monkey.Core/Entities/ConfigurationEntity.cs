using Monkey.Core.Constants;

namespace Monkey.Core.Entities
{
    public class ConfigurationEntity : Entity
    {
        public string Group { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public Configuration.KeyType Type { get; set; } = Configuration.KeyType.String;

        public string Description { get; set; }
    }
}