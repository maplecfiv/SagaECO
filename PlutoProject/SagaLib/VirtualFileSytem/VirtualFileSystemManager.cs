namespace SagaLib.VirtualFileSystem
{
    public enum FileSystems
    {
        Real,
        LPK
    }

    public class VirtualFileSystemManager : Singleton<VirtualFileSystemManager>
    {
        public IFileSystem FileSystem { get; private set; }

        public bool Init(FileSystems type, string path)
        {
            if (FileSystem != null)
                FileSystem.Close();
            switch (type)
            {
                case FileSystems.Real:
                    FileSystem = new RealFileSystem();
                    break;
                case FileSystems.LPK:
                    FileSystem = new LPKFileSystem();
                    break;
            }

            return FileSystem.Init(path);
        }
    }
}