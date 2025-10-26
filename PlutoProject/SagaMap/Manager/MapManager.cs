using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Map;
using SagaDB.Ring;
using SagaLib;

namespace SagaMap.Manager {
    public sealed class MapManager : Singleton<MapManager> {
        public int InstanceMapLifeHour = 4;
        private Dictionary<uint, MapInfo> mapInfo;

        public MapManager() {
            Maps = new Dictionary<uint, Map>();
            mapInfo = new Dictionary<uint, MapInfo>();
        }

        public Dictionary<uint, Map> Maps { get; }

        public Dictionary<uint, MapInfo> MapInfos {
            set => mapInfo = value;
        }

        public string GetMapName(uint mapID) {
            if (mapInfo.ContainsKey(mapID))
                return mapInfo[mapID].name;
            return "MAP_NAME_NOT_FOUND";
        }

        public uint GetMapId(string mapName) {
            foreach (var kv in mapInfo)
                if (kv.Value.name.ToLower() == mapName.ToLower()) //make the map name case insensitive
                    return kv.Key;
            return 0xFFFFFFFF;
        }

        public void LoadMaps() {
            foreach (var mapID in Configuration.Configuration.Instance.HostedMaps)
                if (mapInfo.ContainsKey(mapID))
                    if (!AddMap(new Map(mapInfo[mapID])))
                        Logger.ShowError("Cannot load map " + mapID, null);
        }

        public uint CreateMapInstance(ActorPC creator, uint template, uint exitMap, byte exitX, byte exitY) {
            return CreateMapInstance(creator, template, exitMap, exitX, exitY, false);
        }

        public uint CreateMapInstance(ActorPC creator, uint template, uint exitMap, byte exitX, byte exitY,
            bool autoDispose) {
            return CreateMapInstance(creator, template, exitMap, exitX, exitY, false, 999);
        }

        public uint CreateMapInstance(ActorPC creator, uint template, uint exitMap, byte exitX, byte exitY,
            bool autoDispose, uint ResurrectionLimit) {
            return CreateMapInstance(creator, template, exitMap, exitX, exitY, false, 999, false);
        }

        public uint CreateMapInstance(ActorPC creator, uint template, uint exitMap, byte exitX, byte exitY,
            bool autoDispose, uint ResurrectionLimit, bool returnori) {
            if (!Maps.ContainsKey(template))
                return 0;
            var templateMap = Maps[template];
            var newMap = new Map(templateMap.Info);
            /*if(returnori)
            for (int i = 0; i < 999; i++)
            {
                if (!this.maps.ContainsKey((uint)(template * 100 + i)))
                {
                    newMap.ID = (uint)(template * 100 + i);
                    SagaLib.Logger.getLogger().Information(newMap.ID.ToString());
                    break;
                }
            }
            else*/
            if (template == 70000000 || template == 75000000) {
                for (var i = (int)(template % 1000) + 1; i < 999; i++)
                    if (!Maps.ContainsKey((uint)(template / 1000 * 1000 + template % 1000 + i))) {
                        newMap.ID = (uint)(template / 1000 * 1000 + template % 1000 + i);
                        break;
                    }
            }
            else {
                for (var i = (int)(template % 1000) + 1; i < 999; i++)
                    if (!Maps.ContainsKey((uint)(template / 1000 * 1000 + template % 1000 + i))) {
                        newMap.ID = (uint)(template / 1000 * 1000 + template % 1000 + i);
                        Logger.GetLogger().Information(newMap.ID + "副本创建者：" + creator.Name);
                        break;
                    }
            }

            newMap.IsMapInstance = true;
            newMap.ClientExitMap = exitMap;
            newMap.ClientExitX = exitX;
            newMap.ClientExitY = exitY;
            newMap.AutoDispose = autoDispose;
            newMap.Creator = creator;
            newMap.ResurrectionLimit = ResurrectionLimit;
            if (returnori) newMap.returnori = true;
            newMap.OriID = template;
            Configuration.Configuration.Instance.HostedMaps.Add(newMap.ID);
            Maps.Add(newMap.ID, newMap);
            return newMap.ID;
        }

        public uint CreateMapInstance(Ring ring, uint template, uint exitMap, byte exitX, byte exitY,
            bool autoDispose) {
            if (!Maps.ContainsKey(template))
                return 0;
            var templateMap = Maps[template];
            var newMap = new Map(templateMap.Info);
            for (var i = (int)(template % 1000) + 1; i < 999; i++)
                if (!Maps.ContainsKey((uint)(template / 1000 * 1000 + template % 1000 + i))) {
                    newMap.ID = (uint)(template / 1000 * 1000 + template % 1000 + i);
                    break;
                }

            newMap.IsMapInstance = true;
            newMap.ClientExitMap = exitMap;
            newMap.ClientExitX = exitX;
            newMap.ClientExitY = exitY;
            newMap.AutoDispose = autoDispose;
            newMap.Ring = ring;
            Configuration.Configuration.Instance.HostedMaps.Add(newMap.ID);
            Maps.Add(newMap.ID, newMap);
            return newMap.ID;
        }

        public void CreateFFInstanceOfSer() {
            var templateMap = Maps[90001000];
            var newMap = new Map(templateMap.Info);
            newMap.ID = 90001999;
            newMap.IsMapInstance = false;
            Configuration.Configuration.Instance.HostedMaps.Add(newMap.ID);
            Maps.Add(newMap.ID, newMap);

            templateMap = Maps[91000000];
            newMap = new Map(templateMap.Info);
            newMap.ID = 91000999;
            newMap.IsMapInstance = false;
            Configuration.Configuration.Instance.HostedMaps.Add(newMap.ID);
            Maps.Add(newMap.ID, newMap);

            templateMap = Maps[70000000];
            newMap = new Map(templateMap.Info);
            newMap.ID = 70000999;
            newMap.IsMapInstance = false;
            Configuration.Configuration.Instance.HostedMaps.Add(newMap.ID);
            Maps.Add(newMap.ID, newMap);
        }

        public void DisposeMapInstanceOnLogout(uint charID) {
            try {
                var keys = new uint[Maps.Count];
                Maps.Keys.CopyTo(keys, 0);
                foreach (var i in keys)
                    if (Maps.ContainsKey(i))
                        if (Maps[i].AutoDispose && Maps[i].IsMapInstance)
                            //临时救火，但会导致部分副本地图不会被回收！请尽快优化。
                            if (Maps[i].Creator != null)
                                if (Maps[i].Creator.CharID == charID)
                                    DeleteMapInstance(i);
                /*if(maps[i].Creator == null)
                                DeleteMapInstance(i);
                            else if(!maps[i].Creator.Online)
                                DeleteMapInstance(i);
                            else if (maps[i].Creator.CharID == charID)
                                DeleteMapInstance(i);*/
            }
            catch (Exception ex) {
                Logger.ShowError(ex);
            }
        }

        public bool DeleteMapInstance(uint id) {
            if (!Maps.ContainsKey(id))
                return false;
            var map = Maps[id];
            map.OnDestrory();
            Maps.Remove(id);
            Configuration.Configuration.Instance.HostedMaps.Remove(id);
            return true;
        }

        public bool AddMap(Map addMap) {
            foreach (var map in Maps.Values)
                if (addMap.ID == map.ID)
                    return false;

            Maps.Add(addMap.ID, addMap);
            return true;
        }

        public Map GetMap(uint mapID) {
            if (Maps.ContainsKey(mapID)) return Maps[mapID];

            Logger.ShowDebug("Requesting unknown mapID:" + mapID, null);
            return null;
        }
    }
}