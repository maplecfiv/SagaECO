using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.Item
{
    public class PlayerTitleFactory : Singleton<PlayerTitleFactory>
    {
        public Dictionary<uint, PlayerTitle> PlayerTitles { get; } = new Dictionary<uint, PlayerTitle>();

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
                    var item = new PlayerTitle();
                    item.id = uint.Parse(paras[0]);
                    item.titlename = paras[1];
                    item.firstname = paras[2];
                    item.itemid = uint.Parse(paras[3]);
                    item.str = ushort.Parse(paras[4]);
                    item.mag = ushort.Parse(paras[5]);
                    item.vit = ushort.Parse(paras[6]);
                    item.dex = ushort.Parse(paras[7]);
                    item.agi = ushort.Parse(paras[8]);
                    item.ing = ushort.Parse(paras[9]);
                    item.hp_add = short.Parse(paras[10]);
                    item.mp_add = short.Parse(paras[11]);
                    item.sp_add = short.Parse(paras[12]);
                    item.hprecov_add = short.Parse(paras[13]);
                    item.mprecov_add = short.Parse(paras[14]);
                    item.sprecov_add = short.Parse(paras[15]);
                    item.min_atk_add = ushort.Parse(paras[16]);
                    item.max_atk_add = ushort.Parse(paras[17]);
                    item.min_matk_add = ushort.Parse(paras[18]);
                    item.max_matk_add = ushort.Parse(paras[19]);
                    item.cri_add = ushort.Parse(paras[20]);
                    item.def_add = ushort.Parse(paras[21]);
                    item.mdef_add = ushort.Parse(paras[22]);
                    item.hit_melee_add = ushort.Parse(paras[23]);
                    item.hit_magic_add = ushort.Parse(paras[24]);
                    item.avoid_melee_add = ushort.Parse(paras[25]);
                    item.avoid_magic_add = ushort.Parse(paras[26]);

                    if (!PlayerTitles.ContainsKey(item.id))
                        PlayerTitles.Add(item.id, item);
                }
                catch (Exception ex)
                {
                    Logger.getLogger().Error(ex, ex.Message);
                }
            }

            sr.Close();
        }
    }
}