using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Map;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.ODWar;
using SagaMap.Packets.Server;
using SagaMap.Packets.Server.Actor;
using SagaMap.Packets.Server.Chat;
using SagaMap.Packets.Server.FFarden;
using SagaMap.Packets.Server.FGarden;
using SagaMap.Packets.Server.Golem;
using SagaMap.Packets.Server.Item;
using SagaMap.Packets.Server.Party;
using SagaMap.Packets.Server.Possession;
using SagaMap.Packets.Server.Ring;
using SagaMap.Packets.Server.Skill;
using SagaMap.PC;
using SagaMap.Skill;

namespace SagaMap.ActorEventHandlers
{
    public class PCEventHandler : ActorEventHandler
    {
        public MapClient Client;

        public PCEventHandler()
        {
        }

        public PCEventHandler(MapClient client)
        {
            Client = client;
        }

        #region ActorEventHandler Members

        public void OnActorSkillCancel(Actor sActor)
        {
            var p = new SSMG_SKILL_CAST_CANCEL();
            p.ActorID = sActor.ActorID;
            Client.netIO.SendPacket(p);
        }

        public void OnActorReturning(Actor sActor)
        {
        }

        public void OnActorAppears(Actor aActor)
        {
            if (Client == null) return;
            MapInfo info;
            if (!Client.Character.VisibleActors.Contains(aActor.ActorID))
                Client.Character.VisibleActors.Add(aActor.ActorID);
            switch (aActor.type)
            {
                case ActorType.PC:

                    var pc = (ActorPC)aActor;


                    if (!pc.Online && pc.PossessionTarget == 0)
                        if (!pc.Fictitious)
                            return;


                    for (uint i = 1500; i < 1508; i++)
                        if (pc.Skills.ContainsKey(i)) //临时删暴击
                            pc.Skills.Remove(i);

                    var p = new SSMG_ACTOR_PC_APPEAR();
                    p.ActorID = pc.ActorID;
                    p.Dir = (byte)(pc.Dir / 45);
                    p.HP = pc.HP;
                    p.MaxHP = pc.MaxHP;
                    if (pc.PossessionTarget == 0)
                    {
                        p.PossessionActorID = 0xFFFFFFFF;
                        p.PossessionPosition = PossessionPosition.NONE;
                    }
                    else
                    {
                        var actor = Client.Map.GetActor(pc.PossessionTarget);
                        if (actor != null)
                        {
                            if (actor.type != ActorType.ITEM)
                                p.PossessionActorID = pc.PossessionTarget;
                            else
                                p.PossessionActorID = pc.ActorID;
                            p.PossessionPosition = pc.PossessionPosition;
                        }
                        else
                        {
                            p.PossessionActorID = pc.PossessionTarget;
                            p.PossessionPosition = pc.PossessionPosition;
                        }
                    }

                    p.Speed = pc.Speed;
                    p.X = Global.PosX16to8(pc.X, Client.map.Width);
                    p.Y = Global.PosY16to8(pc.Y, Client.map.Height);
                    Client.netIO.SendPacket(p);

                    if (pc.FurnitureID != 255)
                        OnActorFurnitureSit(pc);
                    break;
                case ActorType.ITEM:
                    var p1 = new SSMG_ITEM_ACTOR_APPEAR();
                    p1.Item = (ActorItem)aActor;
                    Client.netIO.SendPacket(p1);
                    break;
                case ActorType.MOB:
                    var mob = (ActorMob)aActor;
                    if (mob.AnotherID == 0)
                    {
                        info = MapManager.Instance.GetMap(mob.MapID).Info;
                        var p2 = new SSMG_ACTOR_MOB_APPEAR();
                        p2.ActorID = mob.ActorID;
                        p2.Dir = (byte)(mob.Dir / 45);
                        p2.HP = mob.HP;
                        p2.MaxHP = mob.MaxHP;
                        p2.MobID = mob.MobID;
                        p2.Speed = mob.BaseData.speed;
                        p2.X = Global.PosX16to8(mob.X, info.width);
                        p2.Y = Global.PosY16to8(mob.Y, info.height);
                        Client.netIO.SendPacket(p2);
                        if (mob.Name != mob.BaseData.name)
                            OnCharInfoUpdate(mob);
                    }
                    else
                    {
                        info = MapManager.Instance.GetMap(mob.MapID).Info;
                        var p2 = new SSMG_ACTOR_ANOTHER_MOB_APPEAR();
                        p2.ActorID = mob.ActorID;
                        p2.Dir = (byte)(mob.Dir / 45);
                        p2.HP = mob.HP;
                        p2.MaxHP = mob.MaxHP;
                        p2.MobID = mob.AnotherID;
                        p2.Speed = mob.BaseData.speed;
                        p2.X = Global.PosX16to8(mob.X, info.width);
                        p2.Y = Global.PosY16to8(mob.Y, info.height);
                        p2.Camp = mob.Camp;
                        Client.netIO.SendPacket(p2);
                        OnCharInfoUpdate(mob);
                    }

                    //this.Client.SendActorSpeed(mob, mob.BaseData.speed);
                    break;
                case ActorType.PET:
                {
                    var pet = (ActorPet)aActor;
                    info = MapManager.Instance.GetMap(pet.MapID).Info;
                    var p3 = new SSMG_ACTOR_PET_APPEAR();
                    p3.ActorID = pet.ActorID;
                    p3.Union = 0x17;
                    //if (pet.IsUnion)
                    if (pet.Ride)
                        p3.Union = 0x17;
                    p3.OwnerActorID = pet.Owner.ActorID;
                    p3.OwnerCharID = pet.Owner.CharID;
                    p3.OwnerLevel = pet.Owner.Level;
                    p3.OwnerWRP = pet.Owner.WRPRanking;
                    p3.Dir = (byte)(pet.Dir / 45);
                    p3.HP = pet.HP;
                    p3.MaxHP = pet.MaxHP;
                    p3.Speed = pet.Speed;
                    p3.X = Global.PosX16to8(pet.X, info.width);
                    p3.Y = Global.PosY16to8(pet.Y, info.height);
                    Client.netIO.SendPacket(p3);
                    Client.SendPetInfo();
                }
                    break;
                case ActorType.PARTNER:
                {
                    var pet = (ActorPartner)aActor;
                    info = MapManager.Instance.GetMap(pet.MapID).Info;
                    var p3 = new SSMG_ACTOR_PET_APPEAR();
                    p3.ActorID = pet.ActorID;
                    p3.Union = 0x17;
                    p3.OwnerActorID = pet.Owner.ActorID;
                    p3.OwnerCharID = pet.Owner.CharID;
                    p3.OwnerLevel = pet.Owner.Level;
                    p3.OwnerWRP = pet.Owner.WRPRanking;
                    p3.Dir = (byte)(pet.Dir / 45);
                    p3.HP = pet.HP;
                    p3.MaxHP = pet.MaxHP;
                    p3.Speed = pet.Speed;
                    p3.X = Global.PosX16to8(pet.X, info.width);
                    p3.Y = Global.PosY16to8(pet.Y, info.height);
                    Client.netIO.SendPacket(p3);
                    Client.SendPetInfo();
                }
                    break;
                case ActorType.SKILL:
                {
                    var skill = (ActorSkill)aActor;
                    info = MapManager.Instance.GetMap(skill.MapID).Info;
                    var p4 = new SSMG_ACTOR_SKILL_APPEAR();
                    p4.ActorID = skill.ActorID;
                    p4.Dir = (byte)(skill.Dir / 45);
                    p4.Speed = skill.Speed;
                    p4.SkillID = (ushort)skill.Skill.ID;
                    p4.SkillLv = skill.Skill.Level;
                    p4.X = Global.PosX16to8(skill.X, info.width);
                    p4.Y = Global.PosY16to8(skill.Y, info.height);
                    Client.netIO.SendPacket(p4);
                }
                    break;
                case ActorType.SHADOW:
                {
                    var pet = (ActorShadow)aActor;
                    info = MapManager.Instance.GetMap(pet.MapID).Info;
                    var p3 = new SSMG_ACTOR_PET_APPEAR();
                    p3.ActorID = pet.ActorID;
                    p3.Union = 0;
                    p3.OwnerActorID = pet.Owner.ActorID;
                    p3.OwnerCharID = pet.Owner.CharID;
                    p3.Dir = (byte)(pet.Dir / 45);
                    p3.HP = pet.HP;
                    p3.MaxHP = pet.MaxHP;
                    p3.Speed = pet.Speed;
                    p3.X = Global.PosX16to8(pet.X, info.width);
                    p3.Y = Global.PosY16to8(pet.Y, info.height);
                    Client.netIO.SendPacket(p3);
                }
                    break;
                case ActorType.EVENT:
                {
                    var actor = (ActorEvent)aActor;
                    var p3 = new SSMG_ACTOR_EVENT_APPEAR();
                    p3.Actor = actor;
                    Client.netIO.SendPacket(p3);
                }
                    break;
                case ActorType.FURNITUREUNIT:
                {
                    var actor = (ActorFurnitureUnit)aActor;
                    var item = ItemFactory.Instance.GetItem(actor.ItemID);
                    if (item.BaseData.itemType == ItemType.FF_CASTLE)
                    {
                        var p3 = new SSMG_FF_CASTLE_APPEAR();
                        p3.ActorID = actor.ActorID;
                        p3.X = 0xF6EE;
                        p3.Z = 0xFF28;
                        p3.Yaxis = 0x64;
                        Client.netIO.SendPacket(p3);
                    }
                    else
                    {
                        var p3 = new SSMG_FF_UNIT_APPEAR();
                        p3.ActorID = actor.ActorID;
                        p3.ItemID = actor.ItemID;
                        p3.PictID = actor.PictID;
                        p3.X = actor.X;
                        p3.Z = actor.Z;
                        p3.Yaxis = actor.Yaxis;
                        Client.netIO.SendPacket(p3);
                    }

                    break;
                }
                case ActorType.FURNITURE:
                {
                    var actor = (ActorFurniture)aActor;
                    var item = ItemFactory.Instance.GetItem(actor.ItemID);
                    if (Client.Map.ID > 90001000)
                    {
                        if (item.BaseData.itemType == ItemType.FF_CASTLE)
                        {
                            var p3 = new SSMG_FF_CASTLE_APPEAR();
                            p3.ActorID = actor.ActorID;
                            p3.X = 0xF6EE;
                            p3.Z = 0xFF28;
                            p3.Yaxis = 0x64;
                            Client.netIO.SendPacket(p3);
                        }
                        else
                        {
                            var p3 = new SSMG_FF_ACTOR_APPEAR(3);
                            p3.ActorID = actor.ActorID;
                            p3.ItemID = actor.ItemID;
                            p3.PictID = actor.PictID;
                            p3.X = actor.X;
                            p3.Y = actor.Y;
                            p3.Z = actor.Z;
                            //p3.Zaxis = actor.Dir;
                            p3.Xaxis = actor.Xaxis;
                            p3.Yaxis = actor.Yaxis;
                            p3.Zaxis = actor.Zaxis;
                            p3.Motion = actor.Motion;
                            p3.Name = actor.Name;
                            Client.netIO.SendPacket(p3);
                        }
                    }
                    else
                    {
                        //byte type = 2;
                        //if (this.Client.map.ID < 70000000)
                        //    type = 1;
                        var p3 = new SSMG_FG_ACTOR_APPEAR(1);
                        p3.ActorID = actor.ActorID;
                        p3.ItemID = actor.ItemID;
                        p3.PictID = actor.PictID;
                        p3.X = actor.X;
                        p3.Y = actor.Y;
                        p3.Z = actor.Z;
                        //p3.Zaxis = actor.Dir;
                        p3.Xaxis = actor.Xaxis;
                        p3.Yaxis = actor.Yaxis;
                        p3.Zaxis = actor.Zaxis;
                        p3.Motion = actor.Motion;
                        p3.Name = actor.Name;
                        Client.netIO.SendPacket(p3);
                    }
                }
                    break;
                case ActorType.GOLEM:
                {
                    var actor = (ActorGolem)aActor;
                    info = MapManager.Instance.GetMap(actor.MapID).Info;
                    var p3 = new SSMG_GOLEM_ACTOR_APPEAR();
                    p3.ActorID = actor.ActorID;
                    //p3.PictID = actor.Item.BaseData.marionetteID;
                    p3.PictID = actor.PictID;
                    p3.X = Global.PosX16to8(actor.X, info.width);
                    p3.Y = Global.PosY16to8(actor.Y, info.height);
                    p3.Speed = actor.Speed;
                    p3.Dir = (byte)(actor.Dir / 45);
                    p3.GolemID = actor.ActorID;
                    p3.GolemType = actor.GolemType;
                    p3.CharName = actor.Name;
                    p3.Title = actor.Title;
                    p3.Unknown = 1;
                    Client.netIO.SendPacket(p3);
                    if (actor.MotionLoop)
                    {
                        var parg = new ChatArg();
                        parg.motion = (MotionType)actor.Motion;
                        parg.loop = 1;
                        var map = MapManager.Instance.GetMap(actor.MapID);
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, parg, actor, false);
                    }
                }
                    break;
            }

            if (aActor.type != ActorType.FURNITURE)
                OnActorChangeBuff(aActor);
        }

        public void OnPlayerShopChange(Actor aActor)
        {
            if (Client == null) return;
            var pc = (ActorPC)aActor;
            var client = MapClient.FromActorPC(pc);
            var p = new SSMG_PLAYER_SHOP_APPEAR();
            p.ActorID = pc.ActorID;
            p.Title = client.Shoptitle; //mark 22
            p.button = 1;
            Client.netIO.SendPacket(p);
        }

        public void OnPlayerShopChangeClose(Actor aActor)
        {
            if (Client == null) return;
            var p = new SSMG_PLAYER_SHOP_APPEAR();
            var pc = (ActorPC)aActor;
            var client = MapClient.FromActorPC(pc);
            p.ActorID = pc.ActorID;
            p.Title = client.Shoptitle;
            p.button = 0;
            Client.netIO.SendPacket(p);
        }

        public void OnActorChangeEquip(Actor sActor, MapEventArgs args)
        {
            if (Client == null) return;
            var pc = (ActorPC)sActor;
            var p = new SSMG_ITEM_ACTOR_EQUIP_UPDATE();
            p.Player = pc;
            Client.netIO.SendPacket(p);
        }

        public void OnActorChat(Actor cActor, MapEventArgs args)
        {
            if (Client == null) return;
            var p = new SSMG_CHAT_PUBLIC();
            p.ActorID = cActor.ActorID;
            p.Message = ((ChatArg)args).content;
            Client.netIO.SendPacket(p);
        }

        public void OnActorDisappears(Actor dActor)
        {
            if (Client == null) return;
            if (Client.Character.VisibleActors.Contains(dActor.ActorID))
                Client.Character.VisibleActors.Remove(dActor.ActorID);
            switch (dActor.type)
            {
                case ActorType.PC:
                    var p = new SSMG_ACTOR_DELETE();
                    p.ActorID = dActor.ActorID;
                    Client.netIO.SendPacket(p);
                    break;
                case ActorType.ITEM:
                    var p1 = new SSMG_ITEM_ACTOR_DISAPPEAR();
                    var item = (ActorItem)dActor;
                    p1.ActorID = item.ActorID;
                    p1.Count = (byte)item.Item.Stack;
                    p1.Looter = item.LootedBy;
                    Client.netIO.SendPacket(p1);
                    break;
                case ActorType.MOB:
                    if (((ActorMob)dActor).AnotherID != 0)
                    {
                        var p2 = new SSMG_ACTOR_ANOTHER_MOB_DELETE();
                        p2.ActorID = dActor.ActorID;
                        Client.netIO.SendPacket(p2);
                    }
                    else
                    {
                        var p2 = new SSMG_ACTOR_MOB_DELETE();
                        p2.ActorID = dActor.ActorID;
                        Client.netIO.SendPacket(p2);
                    }

                    break;
                case ActorType.SHADOW:
                case ActorType.PET:
                case ActorType.PARTNER:
                    var p3 = new SSMG_ACTOR_PET_DELETE();
                    p3.ActorID = dActor.ActorID;
                    Client.netIO.SendPacket(p3);
                    break;
                case ActorType.SKILL:
                    var skill = (ActorSkill)dActor;
                    var p4 = new SSMG_ACTOR_SKILL_DELETE();
                    p4.ActorID = dActor.ActorID;
                    Client.netIO.SendPacket(p4);
                    if (Client.Character == skill.Caster)
                    {
                        var tem = skill.Name;
                        //if(tem!="NOT_SHOW_DISAPPEAR")
                        //this.Client.SendSystemMessage(string.Format(Manager.LocalManager.Instance.Strings.SKILL_ACTOR_DELETE, skill.Skill.Name));
                    }

                    break;
                case ActorType.EVENT:
                    var p5 = new SSMG_ACTOR_EVENT_DISAPPEAR();
                    p5.ActorID = dActor.ActorID;
                    Client.netIO.SendPacket(p5);
                    break;
                case ActorType.FURNITURE:
                    var p6 = new SSMG_FG_ACTOR_DISAPPEAR();
                    p6.ActorID = dActor.ActorID;
                    Client.netIO.SendPacket(p6);
                    break;
                case ActorType.GOLEM:
                    var p7 = new SSMG_GOLEM_ACTOR_DISAPPEAR();
                    p7.ActorID = dActor.ActorID;
                    Client.netIO.SendPacket(p7);
                    break;
            }
        }

        public void OnActorSkillUse(Actor sActor, MapEventArgs args)
        {
            try
            {
                if (Client == null) return;
                var arg = (SkillArg)args;
                switch (arg.argType)
                {
                    case SkillArg.ArgType.Item_Cast:
                        var p2 = new SSMG_ITEM_USE();
                        p2.ItemID = arg.item.ItemID;
                        p2.Form_ActorId = sActor.ActorID;
                        p2.result = arg.result;
                        p2.To_ActorID = arg.dActor;
                        p2.SkillID = arg.item.BaseData.activateSkill;
                        p2.Cast = arg.item.BaseData.cast;
                        p2.SkillLV = 1;
                        p2.X = arg.x;
                        p2.Y = arg.y;
                        Client.netIO.SendPacket(p2);
                        break;
                    case SkillArg.ArgType.Item_Active:
                        if (arg.dActor != 0xFFFFFFFF)
                        {
                            if (arg.dActor == Client.Character.ActorID)
                            {
                                var p3 = new SSMG_ITEM_ACTIVE_SELF((byte)arg.affectedActors.Count);
                                p3.ActorID = sActor.ActorID;
                                p3.AffectedID = arg.affectedActors;
                                p3.AttackFlag(arg.flag);
                                p3.ItemID = arg.item.ItemID;
                                p3.SetHP(arg.hp);
                                p3.SetMP(arg.mp);
                                p3.SetSP(arg.sp);
                                Client.netIO.SendPacket(p3);
                            }
                            else
                            {
                                var p3 = new SSMG_ITEM_ACTIVE((byte)arg.affectedActors.Count);
                                p3.ActorID = sActor.ActorID;
                                p3.AffectedID = arg.affectedActors;
                                p3.AttackFlag(arg.flag);
                                p3.ItemID = arg.item.ItemID;
                                p3.SetHP(arg.hp);
                                p3.SetMP(arg.mp);
                                p3.SetSP(arg.sp);
                                Client.netIO.SendPacket(p3);
                            }
                        }
                        else
                        {
                            var p3 = new SSMG_ITEM_ACTIVE_FLOOR((byte)arg.affectedActors.Count);
                            p3.ActorID = sActor.ActorID;
                            p3.AffectedID = arg.affectedActors;
                            p3.AttackFlag(arg.flag);
                            p3.SetHP(arg.hp);
                            p3.SetMP(arg.mp);
                            p3.SetSP(arg.sp);
                            p3.ItemID = arg.item.ItemID;
                            p3.X = arg.x;
                            p3.Y = arg.y;
                            Client.netIO.SendPacket(p3);
                        }

                        break;
                    case SkillArg.ArgType.Cast:
                        var p = new SSMG_SKILL_CAST_RESULT();
                        p.ActorID = sActor.ActorID;
                        p.Result = (byte)arg.result;
                        p.TargetID = arg.dActor;
                        p.SkillID = (ushort)arg.skill.ID;
                        p.CastTime = arg.delay;
                        p.SkillLv = arg.skill.Level;
                        p.X = arg.x;
                        p.Y = arg.y;
                        Client.netIO.SendPacket(p);
                        break;
                    case SkillArg.ArgType.Active:
                        if (arg.dActor != 0xFFFFFFFF)
                        {
                            var p1 = new SSMG_SKILL_ACTIVE((byte)arg.affectedActors.Count);
                            if (arg.skill != null)
                                p1.SkillID = (ushort)arg.skill.ID;

                            p1.ActorID = sActor.ActorID;
                            p1.TargetID = arg.dActor;
                            p1.AffectedID = arg.affectedActors;
                            p1.X = arg.x;
                            p1.Y = arg.y;
                            p1.SetHP(arg.hp);
                            p1.SetMP(arg.mp);
                            p1.SetSP(arg.sp);
                            p1.AttackFlag(arg.flag);
                            if (arg.skill != null)
                                p1.SkillLv = arg.skill.Level;
                            Client.netIO.SendPacket(p1);
                        }
                        else
                        {
                            var p1 = new SSMG_SKILL_ACTIVE_FLOOR((byte)arg.affectedActors.Count);
                            //Packets.Server.SSMG_SKILL_ACTIVE p1 = new SagaMap.Packets.Server.SSMG_SKILL_ACTIVE((byte)arg.affectedActors.Count);
                            p1.ActorID = sActor.ActorID;
                            p1.AffectedID = arg.affectedActors;
                            p1.AttackFlag(arg.flag);
                            p1.SetHP(arg.hp);
                            p1.SetMP(arg.mp);
                            p1.SetSP(arg.sp);
                            if (arg.skill != null)
                            {
                                p1.SkillID = (ushort)arg.skill.ID;
                                p1.SkillLv = arg.skill.Level;
                            }

                            p1.X = arg.x;
                            p1.Y = arg.y;
                            Client.netIO.SendPacket(p1);
                        }

                        break;
                    case SkillArg.ArgType.Actor_Active:
                        var p4 = new SSMG_SKILL_ACTIVE_ACTOR((byte)arg.affectedActors.Count);
                        p4.ActorID = sActor.ActorID;
                        p4.AffectedID = arg.affectedActors;
                        p4.AttackFlag(arg.flag);
                        p4.SetHP(arg.hp);
                        p4.SetMP(arg.mp);
                        p4.SetSP(arg.sp);
                        Client.netIO.SendPacket(p4);
                        break;
                    case SkillArg.ArgType.Attack:
                        OnAttack(sActor, arg);

                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void OnActorStartsMoving(Actor mActor, short[] pos, ushort dir, ushort speed)
        {
            OnActorStartsMoving(mActor, pos, dir, speed, MoveType.RUN);
        }

        public void OnActorStartsMoving(Actor mActor, short[] pos, ushort dir, ushort speed, MoveType moveType)
        {
            if (Client == null) return;
            if (!Client.Character.VisibleActors.Contains(mActor.ActorID) && mActor.ActorID != Client.Character.ActorID)
                OnActorAppears(mActor);
            if (mActor.type == ActorType.FURNITURE)
            {
                var p = new SSMG_FG_FURNITURE_RECONFIG();
                p.ActorID = mActor.ActorID;
                p.X = pos[0];
                p.Y = pos[1];
                p.Z = pos[2];
                p.Dir = dir;
                Client.netIO.SendPacket(p);
                return;
            }

            if (mActor.type != ActorType.SKILL)
            {
                var p = new SSMG_ACTOR_MOVE();
                p.ActorID = mActor.ActorID;
                p.X = pos[0];
                p.Y = pos[1];
                if (dir <= 360)
                {
                    p.Dir = dir;
                    if (mActor.type != ActorType.MOB && mActor.type != ActorType.SHADOW &&
                        mActor.type != ActorType.GOLEM && mActor.type != ActorType.PC)
                    {
                        p.MoveType = MoveType.RUN;
                    }
                    else if (mActor.type == ActorType.SHADOW)
                    {
                        var eh = (PetEventHandler)mActor.e;
                        if (eh.AI.AIActivity == Activity.BUSY)
                            p.MoveType = MoveType.RUN;
                        else
                            p.MoveType = MoveType.WALK;
                    }
                    else if (mActor.type == ActorType.MOB || mActor.type == ActorType.GOLEM)
                    {
                        var eh = (MobEventHandler)mActor.e;
                        if (eh.AI.AIActivity == Activity.BUSY)
                            p.MoveType = MoveType.RUN;
                        else
                            p.MoveType = MoveType.WALK;
                    }
                    else
                    {
                        var eh = (PCEventHandler)mActor.e;
                        if (eh.Client.AI != null)
                        {
                            if (eh.Client.AI.AIActivity == Activity.BUSY)
                                p.MoveType = MoveType.RUN;
                            else
                                p.MoveType = MoveType.WALK;
                        }
                        else
                        {
                            p.MoveType = MoveType.RUN;
                        }
                    }
                }
                else
                {
                    p.MoveType = MoveType.FORCE_MOVEMENT;
                }

                if (moveType != MoveType.RUN)
                    p.MoveType = moveType;
                Client.netIO.SendPacket(p);
            }
            else
            {
                var p = new SSMG_ACTOR_SKILL_MOVE();
                p.ActorID = mActor.ActorID;
                p.X = pos[0];
                p.Y = pos[1];
                Client.netIO.SendPacket(p);
            }
        }

        public void OnActorStopsMoving(Actor mActor, short[] pos, ushort dir, ushort speed)
        {
            if (Client == null) return;
            if (!Client.Character.VisibleActors.Contains(mActor.ActorID) && mActor.ActorID != Client.Character.ActorID)
                OnActorAppears(mActor);
            if (dir <= 360)
            {
                var p = new SSMG_ACTOR_MOVE();
                p.ActorID = mActor.ActorID;
                p.X = pos[0];
                p.Y = pos[1];
                p.Dir = dir;
                p.MoveType = MoveType.CHANGE_DIR;
                Client.netIO.SendPacket(p);
            }
            else
            {
                var p = new Packet(11);
                p.ID = 0x11fa;
                p.PutByte(0xff, 2);
                p.PutUInt(mActor.MapID, 3);
                p.PutShort(Client.Character.X, 7);
                p.PutShort(Client.Character.Y, 9);
                Client.netIO.SendPacket(p);
            }
        }

        public void OnCreate(bool success)
        {
            if (Client == null) return;
            if (success)
            {
                if (Client.firstLogin)
                {
                    Client.SendActorID();
                    Client.SendActorMode();
                    Client.SendCharOption();
                    //Client.SendGifts();
                    Client.SendItems();
                    Client.SendCharInfo();
                    Client.SendRingFF();
                    Client.SendAnotherButton();
                    Client.firstLogin = false;
                }
                else
                {
                    Client.map = MapManager.Instance.GetMap(Client.Character.MapID);
                    if (Client.map.ID / 1000 == 70000 || Client.map.ID / 1000 == 75000)
                    {
                        Client.SendGotoFG();
                        var p = new Packet();
                        p.data = new byte[3];
                        p.ID = 0x122a;
                        Client.netIO.SendPacket(p);
                    }
                    else if (Client.map.ID / 10 == 9000000 || Client.map.ID / 10 == 9100000)
                    {
                        Client.SendGotoFF();

                        var p = new Packet();
                        p.data = new byte[3];
                        p.ID = 0x122a;
                        Client.netIO.SendPacket(p);
                    }
                    else if (Client.map.ID == 90001999) //|| Client.map.ID == 10054000)//神经病逻辑处理掉
                    {
                        Client.SendGotoFF();

                        var p = new Packet();
                        p.data = new byte[3];
                        p.ID = 0x122a;
                        Client.netIO.SendPacket(p);
                    }
                    else
                    {
                        Client.SendChangeMap();
                        var p = new Packet();
                        p.data = new byte[3];
                        p.ID = 0x122a;
                        Client.netIO.SendPacket(p);
                    }

                    if (Client.Character.Partner != null)
                        Client.DeletePartner();
                    if (Client.Character.Tasks.ContainsKey("Possession"))
                    {
                        Client.Character.Tasks["Possession"].Deactivate();
                        Client.Character.Tasks.Remove("Possession");
                    }
                }

                if (Client.Character.MapID != 90001999 && Client.Character.Playershoplist.Count != 0 &&
                    Client.Character.Account.GMLevel < 200)
                {
                    Client.Character.Playershoplist.Clear();
                    Client.Character.Fictitious = false;
                    Client.Shopswitch = 0;
                    OnPlayerShopChangeClose(Client.Character);
                }

                if (Client.Character.MapID == 90001999)
                    CustomMapManager.Instance.EnterFFOnMapLoaded(Client);
                Client.PartnerTalking(Client.Character.Partner, MapClient.TALK_EVENT.MASTERLOGIN, 100, 0);
            }
        }

        public void OnActorChangeEmotion(Actor aActor, MapEventArgs args)
        {
            if (Client == null) return;
            var arg = (ChatArg)args;
            var p = new SSMG_CHAT_EMOTION();
            p.ActorID = aActor.ActorID;
            p.Emotion = arg.emotion;
            Client.netIO.SendPacket(p);
        }

        public void OnActorChangeMotion(Actor aActor, MapEventArgs args)
        {
            if (Client == null) return;
            var arg = (ChatArg)args;
            if (aActor.type == ActorType.FURNITURE)
            {
                var p = new SSMG_FF_USE();
                p.actorID = aActor.ActorID;
                p.motion = ((ActorFurniture)aActor).Motion;
                Client.netIO.SendPacket(p);
            }
            else
            {
                if (arg.expression != 0)
                {
                    var p = new SSMG_CHAT_EXPRESSION();
                    p.ActorID = aActor.ActorID;
                    p.Motion = arg.expression;
                    p.Loop = arg.loop;
                    Client.netIO.SendPacket(p);
                }
                else
                {
                    var p = new SSMG_CHAT_MOTION();
                    p.ActorID = aActor.ActorID;
                    p.Motion = arg.motion;
                    p.Loop = arg.loop;
                    Client.netIO.SendPacket(p);
                }
            }
        }

        public void OnActorChangeWaitType(Actor aActor)
        {
            if (Client == null) return;
            if (aActor.type != ActorType.PC) return;
            var pc = (ActorPC)aActor;
            var p = new SSMG_CHAT_WAITTYPE();
            p.ActorID = pc.ActorID;
            p.type = pc.WaitType;
            Client.netIO.SendPacket(p);
        }

        public void OnDelete()
        {
            //TODO: add something
        }

        public void OnCharInfoUpdate(Actor aActor)
        {
            if (Client == null) return;
            var p = new SSMG_ACTOR_PC_INFO();
            p.Actor = aActor;
            Client.netIO.SendPacket(p);
            if (aActor.type == ActorType.PC)
                OnActorRingUpdate((ActorPC)aActor);

            if (aActor.type == ActorType.PC)
            {
                var pc = (ActorPC)aActor;
                var p2 = new SSMG_CHAT_EXPRESSION();
                p2.ActorID = aActor.ActorID;
                p2.Motion = pc.WaitType;
                Client.netIO.SendPacket(p2);
            }

            if (aActor.type == ActorType.PC)
            {
                var p3 = new SSMG_ACTOR_CHANGEPAPER();
                p3.ActorID = aActor.ActorID;
                p3.paperID = ((ActorPC)aActor).UsingPaperID;
                Client.netIO.SendPacket(p3);
            }
        }

        public void OnPlayerSizeChange(Actor aActor)
        {
            if (Client == null) return;
            var p = new SSMG_PLAYER_SIZE();
            var pc = (ActorPC)aActor;
            p.ActorID = pc.ActorID;
            p.Size = pc.Size;
            Client.netIO.SendPacket(p);
        }

        public void OnDie()
        {
            if (Client == null) return;
            if (Client.Character.Marionette != null)
                Client.MarionetteDeactivate();
            Client.Character.ClearTaskAddition();

            SkillHandler.Instance.CastPassiveSkills(Client.Character);
            Client.Character.BattleStatus = 0;
            //this.Client.Character.Mode = PlayerMode.NORMAL;
            Client.SendChangeStatus();
            if (Client.Character.TInt["死亡事件ID"] != 0 && Client.Character.TInt["死亡事件地图ID"] != 0)
            {
                if (Client.Character.TInt["死亡事件地图ID"] == Client.map.ID)
                {
                    Client.Character.TInt.Remove("死亡事件地图ID");
                    Client.EventActivate((uint)Client.Character.TInt["死亡事件ID"]);
                    Client.Character.TInt.Remove("死亡事件ID");
                }
            }
            else
            {
                Client.Character.Buff.Dead = true;
                Client.Character.Motion = MotionType.DEAD;
                Client.Character.MotionLoop = true;
                Client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, Client.Character, true);
            }

            ODWarManager.Instance.UpdateScore(Client.map.ID, Client.Character.ActorID, -200);

            Client.PartnerTalking(Client.Character.Partner, MapClient.TALK_EVENT.MASTERDEAD, 100, 0);

            Client.TitleProccess(Client.Character, 26, 1);
            Client.TitleProccess(Client.Character, 27, 1);
            Client.TitleProccess(Client.Character, 28, 1);

            if (Global.Random.Next(0, 99) < Client.Character.Status.autoReviveRate)
            {
                Client.Character.BattleStatus = 0;
                Client.SendChangeStatus();
                Client.Character.TInt["Revive"] = 5;
                Client.EventActivate(0xF1000000);
            }
            //Client.currentEvent = null;
            //this.scriptThread = null;
        }

        public void OnKick()
        {
            throw new NotImplementedException();
        }

        public void OnMapLoaded()
        {
            throw new NotImplementedException();
        }

        public void OnReSpawn()
        {
            throw new NotImplementedException();
        }

        public void OnSendMessage(string from, string message)
        {
            throw new NotImplementedException();
        }

        public void OnSendWhisper(string name, string message, byte flag)
        {
            throw new NotImplementedException();
        }

        public void OnTeleport(short x, short y)
        {
            if (Client == null) return;
            var p = new SSMG_ACTOR_MOVE();
            p.ActorID = Client.Character.ActorID;
            p.Dir = Client.Character.Dir;
            p.X = x;
            p.Y = y;
            p.MoveType = MoveType.VANISH2;
            Client.netIO.SendPacket(p);
        }

        public void OnAttack(Actor aActor, MapEventArgs args)
        {
            if (Client == null) return;
            var arg = (SkillArg)args;
            if (arg.affectedActors.Count == 1)
            {
                var p = new SSMG_SKILL_ATTACK_RESULT();
                p.ActorID = arg.sActor;
                p.AttackFlag = arg.flag[0];
                if (arg.result >= 0)
                    p.AttackType = arg.type;
                else
                    p.AttackType = (ATTACK_TYPE)arg.result;
                p.Delay = arg.delay;
                p.Unknown = arg.delay;
                p.HP = arg.hp[0];
                p.MP = arg.mp[0];
                p.SP = arg.sp[0];
                p.TargetID = arg.affectedActors[0].ActorID;
                Client.netIO.SendPacket(p);
            }
            else
            {
                var p = new SSMG_SKILL_COMBO_ATTACK_RESULT((byte)arg.affectedActors.Count);
                p.ActorID = arg.sActor;
                p.TargetID = arg.affectedActors;
                p.AttackFlag(arg.flag);
                if (arg.result >= 0)
                    p.AttackType = arg.type;
                else
                    p.AttackType = (ATTACK_TYPE)arg.result;
                p.Delay = arg.delay;
                p.Unknown = arg.delay;
                p.SetHP(arg.hp);
                p.SetMP(arg.mp);
                p.SetSP(arg.sp);

                p.Unknown2 = 0;
                if (arg.skill != null)
                {
                    p.SkillID = arg.skill.ID;
                    p.SkillLevel = arg.skill.Level;
                }

                Client.netIO.SendPacket(p);
            }
        }

        public void OnHPMPSPUpdate(Actor sActor)
        {
            if (Client == null) return;
            Client.SendActorHPMPSP(sActor);
        }

        public void OnPlayerChangeStatus(ActorPC aActor)
        {
            if (Client == null) return;
            var p = new SSMG_SKILL_CHANGE_BATTLE_STATUS();
            p.ActorID = aActor.ActorID;
            p.Status = aActor.BattleStatus;
            Client.netIO.SendPacket(p);
        }

        public void OnActorChangeBuff(Actor sActor)
        {
            if (Client == null) return;
            if (sActor.type == ActorType.FURNITURE) return;
            var p = new SSMG_ACTOR_BUFF();
            p.Actor = sActor;
            Client.netIO.SendPacket(p);
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Party != null)
                    if (pc.Party.IsMember(Client.Character))
                    {
                        var p1 = new SSMG_PARTY_MEMBER_BUFF();
                        p1.Actor = pc;
                        Client.netIO.SendPacket(p1);
                    }
            }
        }

        public void OnLevelUp(Actor sActor, MapEventArgs args)
        {
            if (Client == null) return;
            var arg = (SkillArg)args;
            Client.SendLvUP(sActor, arg.x);
        }

        public void OnPlayerMode(Actor aActor)
        {
            if (Client == null) return;
            var p = new SSMG_ACTOR_MODE();
            //Packets.Server.SSMG_ACTOR_PC_INFO p1 = new Packets.Server.SSMG_ACTOR_PC_INFO();
            var pc = (ActorPC)aActor;
            p.ActorID = aActor.ActorID;
            //Logger.ShowInfo("OnPlayerMode");
            switch (pc.Mode)
            {
                case PlayerMode.NORMAL:
                    p.Mode1 = 2;
                    p.Mode2 = 0;
                    break;
                case PlayerMode.COLISEUM_MODE:
                    p.Mode1 = 0x42;
                    p.Mode2 = 1;
                    break;
                case PlayerMode.KNIGHT_WAR:
                    p.Mode1 = 0x22;
                    p.Mode2 = 2;
                    break;
                case PlayerMode.WRP:
                    p.Mode1 = 0x102;
                    p.Mode2 = 4;
                    break;
                case PlayerMode.KNIGHT_EAST:
                    p.Mode1 = 0x22;
                    p.Mode2 = 2;
                    break;
                case PlayerMode.KNIGHT_WEST:
                    p.Mode1 = 0x22;
                    p.Mode2 = 2;
                    break;
                case PlayerMode.KNIGHT_SOUTH:
                    p.Mode1 = 0x22;
                    p.Mode2 = 2;
                    break;
                case PlayerMode.KNIGHT_NORTH:
                    p.Mode1 = 0x22;
                    p.Mode2 = 2;
                    break;
                case PlayerMode.KNIGHT_FLOWER:
                    p.Mode1 = 0x22;
                    p.Mode2 = 2;
                    break;
                case PlayerMode.KNIGHT_ROCK:
                    p.Mode1 = 0x22;
                    p.Mode2 = 2;
                    break;
            }

            Client.netIO.SendPacket(p);
            //p1.Actor = pc;
            //this.Client.netIO.SendPacket(p1);
        }

        public void OnShowEffect(Actor aActor, MapEventArgs args)
        {
            if (Client == null) return;
            var arg = (EffectArg)args;
            Client.SendNPCShowEffect(arg.actorID, arg.x, arg.y, arg.height, arg.effectID, arg.oneTime);
        }

        public void OnActorPossession(Actor aActor, MapEventArgs args)
        {
            if (Client == null) return;
            var arg = (PossessionArg)args;
            if (!arg.cancel)
            {
                var p1 = new SSMG_POSSESSION_RESULT();
                p1.FromID = arg.fromID;
                p1.ToID = arg.toID;
                p1.Result = arg.result;
                Client.netIO.SendPacket(p1);
            }
            else
            {
                var p1 = new SSMG_POSSESSION_CANCEL();
                p1.FromID = arg.fromID;
                p1.ToID = arg.toID;
                p1.Position = (PossessionPosition)arg.result;
                p1.X = arg.x;
                p1.Y = arg.y;
                p1.Dir = arg.dir;
                Client.netIO.SendPacket(p1);
            }
        }

        public void OnActorPartyUpdate(ActorPC aActor)
        {
            if (Client == null) return;
            var p1 = new SSMG_PARTY_NAME();
            p1.Party(aActor.Party, aActor);
            Client.netIO.SendPacket(p1);
        }

        public void OnActorSpeedChange(Actor mActor)
        {
            if (Client == null) return;
            var p = new SSMG_ACTOR_SPEED();
            p.ActorID = mActor.ActorID;
            p.Speed = mActor.Speed;
            Client.netIO.SendPacket(p);
        }

        public void OnSignUpdate(Actor aActor)
        {
            if (Client == null) return;
            if (aActor.type == ActorType.PC)
            {
                var p = new SSMG_CHAT_SIGN();
                p.ActorID = aActor.ActorID;
                p.Message = ((ActorPC)aActor).Sign;
                Client.netIO.SendPacket(p);
            }
            else if (aActor.type == ActorType.EVENT)
            {
                var p = new SSMG_ACTOR_EVENT_TITLE_CHANGE();
                p.Actor = (ActorEvent)aActor;
                Client.netIO.SendPacket(p);
            }
        }

        public void PropertyUpdate(UpdateEvent arg, int para)
        {
            if (Client == null) return;
            switch (arg)
            {
                case UpdateEvent.GOLD:
                    Client.SendGoldUpdate();
                    break;
                case UpdateEvent.ECoin:
                    Client.SendEXP();
                    if (para > 0)
                        Client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.NPC_SHOP_ECOIN_GET, para));
                    else
                        Client.SendSystemMessage(
                            string.Format(LocalManager.Instance.Strings.NPC_SHOP_ECOIN_LOST, -para));
                    break;
                case UpdateEvent.CP:
                    if (para == 0)
                        return;
                    if (para > 0)
                        Client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.NPC_SHOP_CP_GET, para));
                    else
                        Client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.NPC_SHOP_CP_LOST, -para));
                    break;
                case UpdateEvent.EP:
                    if (para > 0)
                        Client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.EP_INCREASED, para));
                    Client.SendActorHPMPSP(Client.Character);
                    break;
                case UpdateEvent.SPEED:
                    Client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SPEED_UPDATE, null, Client.Character,
                        true);
                    break;
                case UpdateEvent.CHAR_INFO:
                    Client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null,
                        Client.Character, true);
                    break;
                case UpdateEvent.LEVEL:
                    StatusFactory.Instance.CalcStatus(Client.Character);
                    Client.SendPlayerInfo();
                    break;
                case UpdateEvent.STAT_POINT:
                    Client.SendPlayerLevel();
                    break;
                case UpdateEvent.MODE:
                    Client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_MODE, null, Client.Character,
                        true);
                    break;
                case UpdateEvent.VCASH_POINT:
                    MapServer.charDB.SaveVShop(Client.Character);
                    break;
                case UpdateEvent.WRP:
                    if (para != 0)
                    {
                        MapServer.charDB.SaveWRP(Client.Character);
                        Client.SendEXP();
                        if (para > 0)
                            Client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.WRP_GOT, para));
                        else
                            Client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.WRP_LOST, -para));
                    }

                    break;
                case UpdateEvent.QUEST_POINT:
                    Client.SendQuestPoints();
                    break;
            }
        }

        public void PropertyRead(UpdateEvent arg)
        {
            if (Client == null) return;
            switch (arg)
            {
                case UpdateEvent.VCASH_POINT:
                    MapServer.charDB.GetVShop(Client.Character);
                    break;
            }
        }

        public void OnActorRingUpdate(ActorPC aActor)
        {
            if (Client == null) return;
            var p = new SSMG_RING_NAME();
            p.Player = aActor;
            Client.netIO.SendPacket(p);
        }

        public void OnActorWRPRankingUpdate(ActorPC aActor)
        {
            if (Client == null) return;

            Client.SendWRPRanking(aActor);
        }

        public void OnActorChangeAttackType(ActorPC aActor)
        {
            if (Client == null || Client.state == MapClient.SESSION_STATE.DISCONNECTED ||
                !Client.Character.Online) return;
            var p = new SSMG_ACTOR_ATTACK_TYPE();
            p.ActorID = aActor.ActorID;
            p.AttackType = aActor.Status.attackType;
            Client.netIO.SendPacket(p);
        }

        public void OnActorFurnitureSit(ActorPC aActor)
        {
            if (Client == null) return;
            if (aActor.FurnitureID != 255)
            {
                var p1 = new SSMG_PLAYER_FURNITURE_SIT_UP();
                p1.ActorID = aActor.CharID;
                p1.FurnitureID = aActor.FurnitureID;
                p1.unknown = (int)aActor.FurnitureID_old;
                Client.netIO.SendPacket(p1);
            }
            else
            {
                var p1 = new SSMG_PLAYER_FURNITURE_SIT_UP();
                p1.ActorID = aActor.CharID;
                p1.FurnitureID = 0;
                p1.unknown = -1;
                Client.netIO.SendPacket(p1);
            }
        }

        public void OnActorFurnitureList(object obj)
        {
            if (Client == null) return;
            if (obj is List<ActorFurniture>)
            {
                var p1 = new SSMG_FF_ACTORS_APPEAR();
                p1.MapID = Client.Map.ID;
                p1.List = obj as List<ActorFurniture>;
                //string ss = p1.DumpData();
                Client.netIO.SendPacket(p1);
            }
        }

        public void OnActorPaperChange(ActorPC aActor)
        {
            if (Client == null) return;
            var p1 = new SSMG_ACTOR_CHANGEPAPER();
            p1.ActorID = aActor.ActorID;
            p1.paperID = aActor.UsingPaperID;
            Client.netIO.SendPacket(p1);
            ;
        }

        public void OnUpdate(Actor aActor)
        {
        }

        #endregion
    }
}