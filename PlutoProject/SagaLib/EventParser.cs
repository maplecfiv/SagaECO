using System;
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

            SagaLib.Logger.ShowInfo(($"trying to parse {eventId}"));
            try {
                IDeserializer d = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance) // see height_in_inches in sample yml 
                    .Build();
                return d.Deserialize<EventData>(File.ReadAllText(path));
            }
            catch (Exception e) {
                SagaLib.Logger.ShowError(e);
                return null;
            }
        }
    }

    public class EventData {
        public static readonly string KeyEventEntrypoint = "entrypoint";
        public Dictionary<string, EventProgress> EventProgresses { get; set; }
    }

    public class EventProgress {
        public List<Step> Actions { get; set; }
    }

    public class Step {
        public string Action { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public string Next { get; set; }
    }

    public static class EventProgressType {
        public const string MOVE_NPC = "move_npc";
        public const string ROUTINE = "routine";
        public const string GO_SECTION = "go_section";
        public const string DIALOG = "dialog";
        public const string PURCHASE_DIALOG = "purchase_dialog";
        public const string TELEPORT = "teleport";
        public const string PLAY_MOTION = "play_motion";
        public const string STOP_BGM = "stop_bgm";
        public const string CHANGE_BGM = "change_bgm";
        public const string CONTROL_SCREEN = "control_screen";
        public const string WAIT = "wait";
        public const string TAKE_ITEM = "take_item";
        public const string GIVE_ITEM = "give_item";
        public const string PLAY_SE = "play_se";
        public const string PLAY_EFFECT = "play_effect";
        public const string SET_VAR = "set_var";
    }
}