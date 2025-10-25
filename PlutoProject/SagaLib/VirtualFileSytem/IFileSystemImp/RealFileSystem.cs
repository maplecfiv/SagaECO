using System.IO;

namespace SagaLib.VirtualFileSytem.IFileSystemImp {
    public class RealFileSystem : IFileSystem {
        // private string rootPath = ".";
        // private string rootPath = "";

        //#region IFileSystem Members

        public bool Init(string path) {
            // if (path != "")
            //     rootPath = path + "/";
            // else
            //     rootPath = "";
            return true;
        }

        public Stream OpenFile(string path) {
            // if (path.IndexOf(":") < 0) return new FileStream(rootPath + path, FileMode.Open, FileAccess.Read);
            SagaLib.Logger.ShowInfo($"Open file {path}");
            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public string[] SearchFile(string path, string pattern) {
            SagaLib.Logger.ShowInfo($"Search file in {path} with pattern {pattern}");
            return Directory.GetFiles(path, pattern, SearchOption.AllDirectories);
        }

        public string[] SearchFile(string path, string pattern, SearchOption option) {
            SagaLib.Logger.ShowInfo($"Search file with pattern {pattern} in {path} with option {option.ToString()}");
            return Directory.GetFiles(path, pattern, option);
        }

        public void Close() {
        }

        //#endregion
    }
}