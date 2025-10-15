using System;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.Map
{
    public class MapNameFactory : Singleton<MapNameFactory>
    {
        public void Init(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);

            string[] paras;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    if (!MapInfoFactory.Instance.MapInfo.ContainsKey(uint.Parse(paras[0])))
                        continue;

                    MapInfoFactory.Instance.MapInfo[uint.Parse(paras[0])].name = paras[1];
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                }
            }
        }
    }
}