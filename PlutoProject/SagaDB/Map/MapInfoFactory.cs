using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using SagaDB.Marionette;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.Map {
    public class MapInfoFactory : Singleton<MapInfoFactory> {
        public Dictionary<uint, MapInfo> mapInfos = new Dictionary<uint, MapInfo>();

        public Dictionary<uint, MapInfo> MapInfo => mapInfos;

        public Dictionary<string, List<MapObject>> MapObjects { get; private set; }

        public void LoadMapObjects(string path) {
            var fs = VirtualFileSystemManager.Instance.FileSystem.OpenFile(path);
#pragma warning disable SYSLIB0011
            var bf = new BinaryFormatter();
            MapObjects = (Dictionary<string, List<MapObject>>)bf.Deserialize(fs);
        }

        public void ApplyMapObject() {
            if (MapObjects != null)
                foreach (var i in MapObjects.Keys) {
                    var id = uint.Parse(i);
                    if (mapInfos.ContainsKey(id)) {
                        var info = mapInfos[id];
                        foreach (var j in MapObjects[i]) {
                            if (j.Name.IndexOf("kadoukyo") != -1)
                                continue;
                            if (j.Name.IndexOf("hasi") != -1)
                                continue;
                            var matrix = j.PositionMatrix;
                            for (var k = 0; k < j.Width; k++)
                                for (var l = 0; l < j.Height; l++) {
                                    var x = j.X + matrix[k, l][0];
                                    var y = j.Y + matrix[k, l][1];
                                    if (x < info.width && y < info.height && x >= 0 && y >= 0)
                                        info.walkable[x, y] = 0;
                                }
                        }
                    }
                }
        }

        public void LoadGatherInterval(string path, Encoding encoding) {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path), encoding);
            Logger.ShowInfo("Loading Gather database...");
            var count = 0;
            string[] paras;
            while (!sr.EndOfStream) {
                string line;
                line = sr.ReadLine();
                try {
                    MapInfo info = null;
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');
                    SagaLib.Logger.ShowInfo(line);

                    for (var i = 0; i < paras.Length; i++)
                        if (paras[i] == "" || paras[i].ToLower() == "null")
                            paras[i] = "0";
                    var mapID = uint.Parse(paras[0]);
                    if (mapInfos.ContainsKey(mapID))
                        info = mapInfos[mapID];
                    if (info == null)
                        continue;
                    for (var i = 0; i < 8; i++) {
                        var min = int.Parse(paras[1 + i]);
                        if (min > 0)
                            info.gatherInterval.Add((GatherType)i, min);
                    }

                    count++;
                }
                catch (Exception ex) {
                    Logger.ShowError("Error on parsing gather db!\r\nat line:" + line);
                    Logger.ShowError(ex);
                }
            }

            Logger.ShowInfo(count + " gather informations loaded.");
            sr.Close();
        }

        public void LoadMapFish(string path) {
            var xml = new XmlDocument();
            try {
                XmlElement root;
                XmlNodeList list;
                xml.Load(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path));
                root = xml["ECOFish"];
                list = root.ChildNodes;
                foreach (var j in list) {
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement)) continue;
                    i = (XmlElement)j;
                    switch (i.Name.ToLower()) {
                        case "map":
                            var list2 = i.ChildNodes;
                            uint mapid = 0;
                            byte fishtype = 0;
                            byte xS = 0, yS = 0, xE = 0, yE = 0;
                            foreach (var l in list2) {
                                XmlElement k;
                                if (l.GetType() != typeof(XmlElement)) continue;
                                k = (XmlElement)l;
                                switch (k.Name.ToLower()) {
                                    case "mapid":
                                        mapid = uint.Parse(k.InnerText);
                                        break;
                                    case "fishtype":
                                        fishtype = byte.Parse(k.InnerText);
                                        break;
                                    case "xs":
                                        xS = byte.Parse(k.InnerText);
                                        break;
                                    case "ys":
                                        yS = byte.Parse(k.InnerText);
                                        break;
                                    case "xe":
                                        xE = byte.Parse(k.InnerText);
                                        break;
                                    case "ye":
                                        yE = byte.Parse(k.InnerText);
                                        break;
                                }
                            }

                            var info = mapInfos[mapid];
                            info.id = mapid;
                            for (int infoX = xS; infoX < xE; infoX++)
                                for (int infoY = yS; infoY < yE; infoY++)
                                    info.canfish[infoX, infoY] = fishtype;

                            break;
                    }
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        public void LoadFlags(string path) {
            var xml = new XmlDocument();
            try {
                XmlElement root;
                XmlNodeList list;
                xml.Load(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path));
                root = xml["MapFlags"];
                list = root.ChildNodes;
                foreach (var j in list) {
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement)) continue;
                    i = (XmlElement)j;
                    switch (i.Name.ToLower()) {
                        case "map":
                            var mapid = uint.Parse(i.Attributes[0].InnerText);
                            if (mapInfos.ContainsKey(mapid))
                                mapInfos[mapid].Flag.Value = int.Parse(i.InnerText);
                            break;
                    }
                }
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        public List<uint> Init(string path) {
            return Init(path, true);
        }

        public List<uint> Init(string path, bool fullinfo) {
            List<uint> events = new List<uint>();
            var stream = VirtualFileSystemManager.Instance.FileSystem.OpenFile(path);
            var file = new ZipFile(stream);
            var total = (int)file.Count;
            stream.Position = 0;
            var zs = new ZipInputStream(stream);
            ZipEntry entry;
#if !Web
            var label = "Loading MapInfo.zip";
            Logger.ProgressBarShow(0, (uint)total, label);
#endif
            var time = DateTime.Now;

            entry = zs.GetNextEntry();
            var buf = new byte[2048];
            var count = 0;
            while (entry != null) {
                var ms = new MemoryStream((int)entry.Size);
                BinaryReader br;
                int size;
                while (true) {
                    size = zs.Read(buf, 0, 2048);
                    if (size > 0)
                        ms.Write(buf, 0, size);
                    else
                        break;
                }

                ms.Flush();
                br = new BinaryReader(ms);
                ms.Position = 0;
                var info = new MapInfo();
                info.id = br.ReadUInt32();
                info.id = uint.Parse(Path.GetFileNameWithoutExtension(entry.Name));
                info.name = Global.Unicode.GetString(br.ReadBytes(32)).Replace("\0", "");
                info.width = br.ReadUInt16();
                info.height = br.ReadUInt16();

                fullinfo = true;

                if (fullinfo) {
                    byte fire, wind, water, earth, holy, dark, neutral;
                    info.canfish = new uint[info.width, info.height];
                    info.walkable = new byte[info.width, info.height];
                    info.holy = new byte[info.width, info.height];
                    info.dark = new byte[info.width, info.height];
                    info.neutral = new byte[info.width, info.height];
                    info.fire = new byte[info.width, info.height];
                    info.wind = new byte[info.width, info.height];
                    info.water = new byte[info.width, info.height];
                    info.earth = new byte[info.width, info.height];

                    info.unknown = new int[info.width, info.height];
                    info.unknown14 = new byte[info.width, info.height];
                    info.unknown15 = new byte[info.width, info.height];
                    info.unknown16 = new byte[info.width, info.height];


                    byte unknow1, unknow2;

                    holy = br.ReadByte();
                    dark = br.ReadByte();
                    neutral = br.ReadByte();
                    fire = br.ReadByte();
                    wind = br.ReadByte();
                    water = br.ReadByte();
                    earth = br.ReadByte();
                    unknow1 = br.ReadByte();
                    unknow2 = br.ReadByte();
                    //ms.Position += 2;
                    for (var i = 0; i < info.height; i++)
                        for (var j = 0; j < info.width; j++) {
                            var eventid = br.ReadUInt32();
                            if (eventid != 0) {
                                events.Add(eventid);
                                SagaLib.Logger.ShowInfo($"Event {eventid} was found in {info.name}/{info.id}");
                                if (!info.events.ContainsKey(eventid)) {
                                    info.events.Add(eventid, new byte[2] { (byte)j, (byte)i });
                                }
                                else {
                                    var tmp = new byte[info.events[eventid].Length + 2];
                                    info.events[eventid].CopyTo(tmp, 0);
                                    tmp[tmp.Length - 2] = (byte)j;
                                    tmp[tmp.Length - 1] = (byte)i;
                                    info.events[eventid] = tmp;
                                }

                                info.canfish[j, i] = eventid;
                            }

                            info.holy[j, i] = (byte)(br.ReadByte() + holy);
                            info.dark[j, i] = (byte)(br.ReadByte() + dark);
                            info.neutral[j, i] = (byte)(br.ReadByte() + neutral);
                            info.fire[j, i] = (byte)(br.ReadByte() + fire);
                            info.wind[j, i] = (byte)(br.ReadByte() + wind);
                            info.water[j, i] = (byte)(br.ReadByte() + water);
                            info.earth[j, i] = (byte)(br.ReadByte() + earth);
                            info.unknown[j, i] = br.ReadInt16();
                            //ms.Position += 2;
                            info.walkable[j, i] = br.ReadByte();
                            //ms.Position += 3;
                            info.unknown14[j, i] = br.ReadByte();
                            info.unknown15[j, i] = br.ReadByte();
                            info.unknown16[j, i] = br.ReadByte();
                        }
                }

                SagaLib.Logger.ShowInfo(JsonSerializer.Serialize(info));
                mapInfos.Add(info.id, info);
                ms.Close();
                entry = zs.GetNextEntry();
                count++;
#if !Web
                if ((DateTime.Now - time).TotalMilliseconds > 40) {
                    time = DateTime.Now;
                    Logger.ProgressBarShow((uint)count, (uint)total, label);
                }
#endif
            }

            zs.Close();
            file.Close();
#if !Web
            Logger.ProgressBarHide(count + " maps loaded.");
#endif
            return events;
        }
    }
}