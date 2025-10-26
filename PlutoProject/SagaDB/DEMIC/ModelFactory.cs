using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.DEMIC {
    public class ModelFactory : Singleton<ModelFactory> {
        public Dictionary<uint, Model> Models { get; } = new Dictionary<uint, Model>();

        public void Init(string path, Encoding encoding) {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            var count = 0;
#if !Web
            var label = "Loading Chip model database";
            Logger.ProgressBarShow(0, (uint)sr.BaseStream.Length, label);
#endif
            var modelBegin = false;
            uint currentID = 0;
            var y = 0;
            string[] paras;
            while (!sr.EndOfStream) {
                string line;
                line = sr.ReadLine();
                try {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');

                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";

                    if (paras[0] == "model") {
                        modelBegin = true;
                        var model = new Model();
                        model.ID = uint.Parse(paras[1]);
                        currentID = model.ID;
                        Models.Add(model.ID, model);
                        y = 0;
                        count++;
                        continue;
                    }

                    if (modelBegin) {
                        var model = Models[currentID];
                        for (var i = 0; i < paras.Length; i++) {
                            if (paras[i] != "0")
                                model.Cells.Add(new[] { (byte)i, (byte)y });
                            if (byte.Parse(paras[i]) > 100) {
                                model.CenterX = (byte)i;
                                model.CenterY = (byte)y;
                            }
                        }

                        y++;
                    }
                }
                catch (Exception ex) {
#if !Web
                    Logger.ShowError("Error on parsing mob db!\r\nat line:" + line);
                    Logger.ShowError(ex);
#endif
                }
            }
#if !Web
            Logger.ProgressBarHide(count + " models loaded.");
#endif
            sr.Close();
        }
    }
}