using SagaDB.Actor;
using SagaDB.FictitiousActors;
using SagaDB.Item;
using SagaDB.Mob;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;

namespace SagaMap.FictitiousActors
{
    public class FictitiousActorsManager : Singleton<FictitiousActorsManager>
    {
        public void regionFictitiousSingleActor(Actor item)
        {
            var map = MapManager.Instance.GetMap(item.MapID);
            if (item.type == ActorType.GOLEM)
            {
                item.X = Global.PosX8to16(item.X2, map.Width);
                item.Y = Global.PosY8to16(item.Y2, map.Height);
                if (((ActorGolem)item).AIMode >= 1)
                {
                    var eh = new MobEventHandler((ActorGolem)item);
                    item.e = eh;
                    byte ai = 0;
                    if (((ActorGolem)item).AIMode == 2)
                    {
                        var date = MobFactory.Instance.GetMobData(((ActorGolem)item).MobID);
                        item.MaxHP = date.hp;
                        item.HP = date.hp;
                        item.Status.aspd = date.aspd;
                        item.Status.cspd = date.cspd;
                        item.Status.def = date.def;
                        item.Status.def_add = (short)date.def_add;
                        item.Status.mdef = date.mdef;
                        item.Status.mdef_add = (short)date.mdef_add;
                        item.Status.min_atk1 = date.atk_min;
                        item.Status.max_atk1 = date.atk_max;
                        item.Status.min_atk2 = date.atk_min;
                        item.Status.max_atk2 = date.atk_max;
                        item.Status.min_atk3 = date.atk_min;
                        item.Status.max_atk3 = date.atk_max;
                        item.Status.min_matk = date.matk_min;
                        item.Status.max_matk = date.matk_max;
                        ai = 1;
                        item.Range = 2;
                    }
                    else if (((ActorGolem)item).AIMode == 4)
                    {
                        ai = 4;
                    }

                    item.Speed = 600;
                    eh.AI = new MobAI(item);
                    if (MobAIFactory.Instance.Items.ContainsKey(((ActorGolem)item).MobID))
                        eh.AI.Mode = MobAIFactory.Instance.Items[((ActorGolem)item).MobID];
                    else
                        eh.AI.Mode = new AIMode(ai);
                    eh.AI.X_Ori = item.X;
                    eh.AI.Y_Ori = item.Y;
                    eh.AI.X_Spawn = item.X;
                    eh.AI.Y_Spawn = item.Y;
                    eh.AI.Mode.MobID = ((ActorGolem)item).MobID;

                    eh.AI.Start();
                }

                map.RegisterActor(item);
                item.invisble = false;
                map.OnActorVisibilityChange(item);
            }
        }

        public void regionFictitiousActors()
        {
            foreach (var Mapid in FictitiousActorsFactory.Instance.FictitiousActorsList.Keys)
            {
                var map = MapManager.Instance.GetMap(Mapid);
                if (map == null)
                    continue;

                foreach (var item in FictitiousActorsFactory.Instance.FictitiousActorsList[Mapid])
                    if (item.type == ActorType.PC)
                    {
                        var a = (ActorPC)item;
                        a.Inventory.Equipments[EnumEquipSlot.HEAD] = ItemFactory.Instance.GetItem(a.Equips[0]);
                        a.Inventory.Equipments[EnumEquipSlot.FACE] = ItemFactory.Instance.GetItem(a.Equips[1]);
                        a.Inventory.Equipments[EnumEquipSlot.CHEST_ACCE] = ItemFactory.Instance.GetItem(a.Equips[2]);
                        a.Inventory.Equipments[EnumEquipSlot.UPPER_BODY] = ItemFactory.Instance.GetItem(a.Equips[3]);
                        a.Inventory.Equipments[EnumEquipSlot.LOWER_BODY] = ItemFactory.Instance.GetItem(a.Equips[4]);
                        a.Inventory.Equipments[EnumEquipSlot.BACK] = ItemFactory.Instance.GetItem(a.Equips[5]);
                        a.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND] = ItemFactory.Instance.GetItem(a.Equips[6]);
                        a.Inventory.Equipments[EnumEquipSlot.LEFT_HAND] = ItemFactory.Instance.GetItem(a.Equips[7]);
                        a.Inventory.Equipments[EnumEquipSlot.SHOES] = ItemFactory.Instance.GetItem(a.Equips[8]);
                        a.Inventory.Equipments[EnumEquipSlot.SOCKS] = ItemFactory.Instance.GetItem(a.Equips[9]);
                        a.Inventory.Equipments[EnumEquipSlot.PET] = ItemFactory.Instance.GetItem(a.Equips[10]);
                        a.Inventory.Equipments[EnumEquipSlot.EFFECT] = ItemFactory.Instance.GetItem(a.Equips[11]);
                        a.Fictitious = true;
                        item.X = Global.PosX8to16(item.X2, map.Width);
                        item.Y = Global.PosY8to16(item.Y2, map.Height);
                        item.e = new PCEventHandler();
                        map.RegisterActor(item);
                        item.invisble = false;
                        map.OnActorVisibilityChange(item);
                    }
                    else if (item.type == ActorType.FURNITURE)
                    {
                        var fi = (ActorFurniture)item;
                        fi.e = new NullEventHandler();
                        map.RegisterActor(item);
                        item.invisble = false;
                        map.OnActorVisibilityChange(item);
                    }
                    else if (item.type == ActorType.GOLEM)
                    {
                        item.X = Global.PosX8to16(item.X2, map.Width);
                        item.Y = Global.PosY8to16(item.Y2, map.Height);
                        var eh = new MobEventHandler((ActorGolem)item);
                        item.e = eh;
                        if (((ActorGolem)item).AIMode >= 1)
                        {
                            byte ai = 0;
                            if (((ActorGolem)item).AIMode == 2)
                            {
                                var date = MobFactory.Instance.GetMobData(((ActorGolem)item).MobID);
                                item.MaxHP = date.hp;
                                item.HP = date.hp;
                                item.Status.aspd = date.aspd;
                                item.Status.cspd = date.cspd;
                                item.Status.def = date.def;
                                item.Status.def_add = (short)date.def_add;
                                item.Status.mdef = date.mdef;
                                item.Status.mdef_add = (short)date.mdef_add;
                                item.Status.min_atk1 = date.atk_min;
                                item.Status.max_atk1 = date.atk_max;
                                item.Status.min_atk2 = date.atk_min;
                                item.Status.max_atk2 = date.atk_max;
                                item.Status.min_atk3 = date.atk_min;
                                item.Status.max_atk3 = date.atk_max;
                                item.Status.min_matk = date.matk_min;
                                item.Status.max_matk = date.matk_max;
                                ai = 1;
                                item.Range = 2;
                            }
                            else if (((ActorGolem)item).AIMode == 4)
                            {
                                ai = 4;
                            }

                            item.Speed = 600;
                            eh.AI = new MobAI(item);
                            if (MobAIFactory.Instance.Items.ContainsKey(((ActorGolem)item).MobID))
                                eh.AI.Mode = MobAIFactory.Instance.Items[((ActorGolem)item).MobID];
                            else
                                eh.AI.Mode = new AIMode(ai);
                            eh.AI.X_Ori = item.X;
                            eh.AI.Y_Ori = item.Y;
                            eh.AI.X_Spawn = item.X;
                            eh.AI.Y_Spawn = item.Y;
                            eh.AI.Mode.MobID = ((ActorGolem)item).MobID;

                            eh.AI.Start();
                        }

                        map.RegisterActor(item);
                        item.invisble = false;
                        map.OnActorVisibilityChange(item);
                    }
            }
        }
    }
}