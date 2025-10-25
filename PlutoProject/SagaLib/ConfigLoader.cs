using System.Xml;

namespace SagaLib;

public class ConfigLoader {
    public static XmlElement LoadConfig(string projectName) {
        XmlDocument xml = new XmlDocument();
        string path = $"/home/maple/projects/SagaECO/PlutoProject/Bin/Config/{projectName}.xml";
        SagaLib.Logger.ShowInfo($"load config {path} for {projectName}");
        xml.Load(
            path);
        return xml[projectName];
    }

    public static string LoadDbPath() {
        string path = "/home/maple/projects/SagaECO/PlutoProject/Bin/DB";
        SagaLib.Logger.ShowInfo($"load db config from {path}");
        return path;
    }
}