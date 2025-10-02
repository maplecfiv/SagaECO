using System.IO;

namespace SagaLib.VirtualFileSytem.IFileSystemImp
{
    public class RealFileSystem : IFileSystem
    {
        private string rootPath = ".";

        //#region IFileSystem Members

        public bool Init(string path)
        {
            if (path != "")
                rootPath = path + "/";
            else
                rootPath = "";
            return true;
        }

        public Stream OpenFile(string path)
        {
            if (path.IndexOf(":") < 0) return new FileStream(rootPath + path, FileMode.Open, FileAccess.Read);

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public string[] SearchFile(string path, string pattern)
        {
            return Directory.GetFiles(rootPath + path, pattern);
        }

        public string[] SearchFile(string path, string pattern, SearchOption option)
        {
            return Directory.GetFiles(rootPath + path, pattern, option);
        }

        public void Close()
        {
        }

        //#endregion
    }
}