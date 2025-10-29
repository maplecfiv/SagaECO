using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SagaLib {
    public enum PcRace {
        EMIL,
        TITANIA,
        DOMINION,
        DEM,
        NONE
    }

    [Serializable]
    public class StartupSetting {
        public uint StartMap;
        public ushort Str, Dex, Int, Vit, Agi, Mag;
        public byte X, Y;

        public override string ToString() {
            return string.Format("Stats:[S:{0},D:{1},I:{2},V:{3},A:{4},M:{5}\r\n       StartPoint:[{6}({7},{8})]",
                Str, Dex, Int, Vit, Agi, Mag, StartMap, X, Y);
        }
    }

    public class ConfigLoader {
        public static Dictionary<PcRace, StartupSetting> StartupSetting { get; set; } =
            GetStartupSetting();

        private static Dictionary<PcRace, StartupSetting> GetStartupSetting() {
            Dictionary<PcRace, StartupSetting> startupSetting =
                new Dictionary<PcRace, StartupSetting>();

            startupSetting.Add(PcRace.EMIL, new StartupSetting {
                Str = 8,
                Dex = 3,
                Int = 3,
                Vit = 10,
                Agi = 4,
                Mag = 3,
                StartMap = 30204000,
                X = 15,
                Y = 16,
            });

            startupSetting.Add(PcRace.TITANIA, new StartupSetting {
                Str = 6,
                Dex = 3,
                Int = 6,
                Vit = 4,
                Agi = 2,
                Mag = 10,
                StartMap = 30204000,
                X = 15,
                Y = 16,
            });

            startupSetting.Add(PcRace.DOMINION, new StartupSetting {
                Str = 10,
                Dex = 5,
                Int = 1,
                Vit = 1,
                Agi = 5,
                Mag = 8,
                StartMap = 30204000,
                X = 15,
                Y = 16,
            });

            startupSetting.Add(PcRace.DEM, new StartupSetting {
                Str = 5,
                Dex = 5,
                Int = 5,
                Vit = 5,
                Agi = 5,
                Mag = 5,
                StartMap = 30203003,
                X = 13,
                Y = 15,
            });

            return startupSetting;
        }

        public static bool ShouldShutdown() {
            string path = $"{ConfigLoader.LoadConfigPath()}/doShutdown";
            return File.Exists(path);
        }

        public static XmlElement LoadConfig(string projectName) {
            XmlDocument xml = new XmlDocument();
            string path = $"{ConfigLoader.LoadConfigPath()}/{projectName}.xml";
            // SagaLib.Logger.ShowInfo($"load config {path} for {projectName}");
            xml.Load(
                path);
            return xml[projectName];
        }

        public static string LoadPacketPath() {
            string path = "/home/maple/projects/SagaECO/PlutoProject/SagaMap/Packets";
            // SagaLib.Logger.ShowInfo($"load packet config from {path}");
            return path;
        }

        public static string LoadDbPath() {
            string path = "/home/maple/projects/SagaECO/PlutoProject/Bin/DB";
            // SagaLib.Logger.ShowInfo($"load db config from {path}");
            return path;
        }

        public static string LoadConfigPath() {
            string path = $"/home/maple/projects/SagaECO/PlutoProject/Bin/Config";
            // SagaLib.Logger.ShowInfo($"load config from {path}");
            return path;
        }

        public static string LoadScriptPath() {
            string path = $"/home/maple/projects/SagaECO/PlutoProject/Bin/Script";
            // SagaLib.Logger.ShowInfo($"load config from {path}");
            return path;
        }
    }
}