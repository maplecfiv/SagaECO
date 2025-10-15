using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.Item
{
    public class FaceFactory : Singleton<FaceFactory>
    {
        /*List<Face> faces = new List<Face>();
        public List<Face> Faces { get { return faces; } }*/

        /// <summary>
        ///     左FACEID 右道具ID
        /// </summary>
        public Dictionary<uint, uint> Faces { get; } = new Dictionary<uint, uint>();

        public List<uint> FaceItemIDList { get; } = new List<uint>();

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
                    var itemID = uint.Parse(paras[0]);
                    var FaceID = uint.Parse(paras[1]);
                    if (!Faces.ContainsKey(FaceID))
                        Faces.Add(FaceID, itemID);
                    if (!FaceItemIDList.Contains(itemID)) FaceItemIDList.Add(itemID);
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