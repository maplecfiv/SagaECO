using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.DualJob {
    public class DualJobSkillFactory : Singleton<DualJobSkillFactory> {
        public Dictionary<ushort, List<DualJobSkill>> items = new Dictionary<ushort, List<DualJobSkill>>();

        public void Init(string path, Encoding encoding) {
            using (var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding)) {
                string[] paras;
                while (!sr.EndOfStream) {
                    string line;
                    line = sr.ReadLine();

                    try {
                        if (line == "") continue;
                        if (line.Substring(0, 1) == "#") continue;

                        paras = line.Split(',');
                        for (var i = 0; i < paras.Length; i++)
                            if (paras[i] == "")
                                paras[i] = "0";
                        var item = new DualJobSkill();
                        item.DualJobID = byte.Parse(paras[0]);
                        item.SkillID = ushort.Parse(paras[1]);
                        item.SkillName = paras[2];
                        item.SkillJobID = byte.Parse(paras[3]);
                        for (var i = 4; i <= 13; i++) item.LearnSkillLevel.Add(byte.Parse(paras[i]));

                        if (items.ContainsKey(item.DualJobID)) {
                            if (items[item.DualJobID] == null)
                                items[item.DualJobID] = new List<DualJobSkill>();

                            items[item.DualJobID].Add(item);
                        }
                        else {
                            items.Add(item.DualJobID, new List<DualJobSkill> { item });
                        }
                    }
                    catch (Exception ex) {
                        Logger.ShowError(ex);
                    }
                }

                sr.Close();
                sr.Dispose();
            }
        }
    }
}