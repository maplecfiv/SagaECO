using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.DualJob
{
    public class DualJobInfoFactory : Singleton<DualJobInfoFactory>
    {
        public Dictionary<byte, DualJobInfo> items = new Dictionary<byte, DualJobInfo>();

        public void Init(string path, Encoding encoding)
        {
            using (var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding))
            {
                string[] paras;
                while (!sr.EndOfStream)
                {
                    string line;
                    line = sr.ReadLine();

                    try
                    {
                        if (line == "") continue;
                        if (line.Substring(0, 1) == "#") continue;

                        paras = line.Split(',');
                        var item = new DualJobInfo();
                        item.DualJobID = byte.Parse(paras[0]);
                        item.DualJobName = paras[1];
                        item.BaseJobID = byte.Parse(paras[2]);
                        item.ExperJobID = byte.Parse(paras[3]);
                        item.TechnicalJobID = byte.Parse(paras[4]);
                        item.ChronicleJobID = byte.Parse(paras[5]);
                        item.Description = paras[6];

                        if (!items.ContainsKey(item.DualJobID))
                            items.Add(item.DualJobID, item);
                        else
                            items[item.DualJobID] = item;
                    }
                    catch (Exception ex)
                    {
                        Logger.getLogger().Error(ex, ex.Message);
                    }
                }

                sr.Close();
                sr.Dispose();
            }
        }
    }
}