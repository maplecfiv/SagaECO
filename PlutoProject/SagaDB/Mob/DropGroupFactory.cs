using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.Mob
{
    public class DropGroupFactory : Singleton<DropGroupFactory>
    {
        private readonly Dictionary<ushort, string[]> Dropgroup = new Dictionary<ushort, string[]>();

        public string[] GetDropRate(ushort groupid)
        {
            if (Dropgroup.ContainsKey(groupid))
                return Dropgroup[groupid];
            return new[] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "什么也不掉" };
        }

        public void Init(string path, Encoding encoding)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count = 0;

#if !Web
            var label = "load dropgroup data...";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            var time = DateTime.Now;
            string[] paras;
            var line = "";

            while (!sr.EndOfStream)
                try
                {
                    line = sr.ReadLine();
                    if (line == "")
                        continue;
                    if (line.Substring(0, 1) == "#")
                        continue;

                    paras = line.Split(',');

                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";

                    var data = new string[paras.Length - 1];
                    for (var i = 1; i < paras.Length; i++) data[i - 1] = paras[i];
                    if (Dropgroup.ContainsKey(ushort.Parse(paras[0])))
                        Logger.GetLogger().Error("the group id:" + paras[0] + "is exist, skip it");
                    else
                        Dropgroup.Add(ushort.Parse(paras[0]), data);

#if !Web
                    if ((DateTime.Now - time).TotalMilliseconds > 40)
                    {
                        time = DateTime.Now;
                        Logger.ProgressBarShow((uint)sr.BaseStream.Position, (uint)sr.BaseStream.Length, label);
                    }
#endif

                    count++;
                }
                catch (Exception ex)
                {
#if !Web
                    Logger.GetLogger().Error("Error on parsing dropgroup db!\r\nat line:" + line);
                    Logger.GetLogger().Error(ex, ex.Message);
#endif
                }
#if !Web
            Logger.ProgressBarHide(count + " DropGroup loaded.");
#endif
            sr.Close();
        }
    }
}