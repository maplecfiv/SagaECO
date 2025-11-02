using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SagaLib {
    public class EventParser {
        public static EventData Find(uint eventId) {
            string path = $"{ConfigLoader.LoadEventPath()}/{eventId}.yaml";
            if (!File.Exists(path)) {
                return null;
            }

            IDeserializer d = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance) // see height_in_inches in sample yml 
                .Build();
            return d.Deserialize<EventData>(File.ReadAllText(path));
        }
    }

    public class EventData {
        public static readonly string KeyEventEntrypoint = "entrypoint";
        public Dictionary<string, EventProgress> EventProgresses { get; set; }
    }

    public class EventProgress {
        public EventProgressType Type { get; set; }
        public Dictionary<string, string[]> Data { get; set; }
        public string Next { get; set; }
    }

    public enum EventProgressType : uint {
        DIALOG = 1,
        TELEPORT = 2,
        REWARD = 3
    }
}