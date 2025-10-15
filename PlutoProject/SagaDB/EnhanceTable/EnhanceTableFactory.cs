using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.EnhanceTable
{
    public class EnhanceTableFactory : Singleton<EnhanceTableFactory>
    {
        public Dictionary<int, EnhanceTable> table = new Dictionary<int, EnhanceTable>();

        public Dictionary<int, EnhanceTable> Table => table;


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
                    var tmp = new EnhanceTable();
                    tmp.Count = int.Parse(paras[0]);
                    tmp.BaseRate = int.Parse(paras[1]);
                    tmp.Crystal = int.Parse(paras[2]);
                    tmp.EnhanceCrystal = int.Parse(paras[3]);
                    tmp.SPCrystal = int.Parse(paras[4]);
                    tmp.KingCrystal = int.Parse(paras[5]);
                    tmp.ExplodeProtect = int.Parse(paras[6]);
                    tmp.ResetProtect = int.Parse(paras[7]);
                    tmp.Okugi = int.Parse(paras[8]);
                    tmp.Shinzui = int.Parse(paras[9]);
                    tmp.Matsuri = int.Parse(paras[10]);
                    tmp.Recycle = int.Parse(paras[11]);


                    table.Add(tmp.Count, tmp);
                }
                catch (Exception ex)
                {
                    Logger.GetLogger().Error(ex, ex.Message);
                }
            }

            sr.Close();
        }
    }
}