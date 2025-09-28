using System.Collections.Generic;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaMap.Partner
{
    public class PartnerSpawnManager : Singleton<PartnerSpawnManager>
    {
        private Dictionary<uint, List<ActorPartner>> mobs = new Dictionary<uint, List<ActorPartner>>();

        public int LoadAI(string f)
        {
            var total = 0;

            return total;
        }

        public void LoadAnAI(string path)
        {
            var file = VirtualFileSystemManager.Instance.FileSystem.SearchFile(path, "*.xml");
            var total = 0;
            foreach (var f in file) total += LoadAI(f);
            Logger.ShowInfo(total + " 加载新的AI...");
        }
    }
}