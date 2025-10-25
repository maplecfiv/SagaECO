using System.Xml;

namespace SagaLib;

public class ConfigLoader {
    public static XmlElement LoadConfig(string projectName) {
        XmlDocument xml = new XmlDocument();
        string path = $"{ConfigLoader.LoadConfigPath()}/{projectName}.xml";
        SagaLib.Logger.ShowInfo($"load config {path} for {projectName}");
        xml.Load(
            path);
        return xml[projectName];
    }

    public static string LoadPacketPath() {
        string path = "/home/maple/projects/SagaECO/PlutoProject/SagaMap/Packets";
        SagaLib.Logger.ShowInfo($"load packet config from {path}");
        return path;
    }

    public static string LoadDbPath() {
        string path = "/home/maple/projects/SagaECO/PlutoProject/Bin/DB";
        SagaLib.Logger.ShowInfo($"load db config from {path}");
        return path;
    }

    public static string LoadConfigPath() {
        string path = $"/home/maple/projects/SagaECO/PlutoProject/Bin/Config";
        SagaLib.Logger.ShowInfo($"load config from {path}");
        return path;
    }

    public static string LoadScriptPath() {
        string path = $"/home/maple/projects/SagaECO/PlutoProject/Bin/Script";
        SagaLib.Logger.ShowInfo($"load config from {path}");
        return path;
    }
}