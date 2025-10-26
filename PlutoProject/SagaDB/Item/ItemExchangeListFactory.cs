using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.Item {
    public class ItemExchangeListFactory : Singleton<ItemExchangeListFactory> {
        public Dictionary<uint, ItemExchangeList> ExchangeList = new Dictionary<uint, ItemExchangeList>();

        public void Init(string path, Encoding encoding) {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);

            string[] paras;
            while (!sr.EndOfStream) {
                string line;
                line = sr.ReadLine();
                try {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    var exl = new ItemExchangeList();
                    exl.ItemID = uint.Parse(paras[0]);
                    exl.OriItemID = uint.Parse(paras[1]);

                    if (!ExchangeList.ContainsKey(exl.ItemID))
                        ExchangeList.Add(exl.ItemID, exl);
                }
                catch (Exception ex) {
                    Logger.ShowError(ex);
                }
            }

            sr.Close();
        }
    }
}