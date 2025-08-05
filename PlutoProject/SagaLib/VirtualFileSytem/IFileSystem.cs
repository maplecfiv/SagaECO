using System.IO;

namespace SagaLib.VirtualFileSytem
{
    public interface IFileSystem
    {
        bool Init(string path);

        Stream OpenFile(string path);

        string[] SearchFile(string path, string pattern);

        string[] SearchFile(string path, string pattern, SearchOption option);

        void Close();
    }
}