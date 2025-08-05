using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.Network.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;

namespace SagaMap.Skill
{
    public enum PetGrowthReason
    {
        PhysicalBeenHit,
        MagicalBeenHit,
        UseSkill,
        PhysicalHit,
        SkillHit,
        CriticalHit,
        ItemRecover
    }

    public partial class SkillHandler : Singleton<SkillHandler>
    {
        private void SendPetGrowth(Actor actor, SSMG_ACTOR_PET_GROW.GrowType growType, uint value)
        {
            if (actor.type != ActorType.PET)
                return;
            var pet = (ActorPet)actor;
            if (pet.Owner == null)
                return;
            if (!pet.Owner.Online)
                return;

            var p = new SSMG_ACTOR_PET_GROW();
            if (pet.Ride)
                p.PetActorID = pet.Owner.ActorID;
            else
                p.PetActorID = pet.ActorID;
            p.OwnerActorID = pet.Owner.ActorID;
            p.Type = growType;
            p.Value = value;
            MapClient.FromActorPC(pet.Owner).netIO.SendPacket(p);
        }

        public void ProcessPetGrowth(Actor actor, PetGrowthReason reason)
        {
            if (actor.type != ActorType.PET)
                return;
            var pet = (ActorPet)actor;
            if (!pet.Owner.Online)
                return;
            var growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
            if (pet.Owner.Inventory.Equipments.ContainsKey(EnumEquipSlot.PET))
            {
                var item = pet.Owner.Inventory.Equipments[EnumEquipSlot.PET];
                int rate;
                switch (reason)
                {
                    case PetGrowthReason.CriticalHit:
                    case PetGrowthReason.UseSkill:
                    case PetGrowthReason.SkillHit:
                        rate = 10;
                        break;
                    case PetGrowthReason.PhysicalHit:
                    case PetGrowthReason.ItemRecover:
                    case PetGrowthReason.MagicalBeenHit:
                    case PetGrowthReason.PhysicalBeenHit:
                        rate = 3;
                        break;
                }

                if (pet.Ride)
                    rate = 15;
                else
                    rate = 5;
                if (Global.Random.Next(0, 99) < rate)
                {
                    item.Refine = 1;
                    if (!pet.Ride)
                    {
                        int type;
                        switch (reason)
                        {
                            case PetGrowthReason.PhysicalBeenHit:
                            case PetGrowthReason.MagicalBeenHit:
                                type = Global.Random.Next(0, 8);
                                switch (type)
                                {
                                    case 0:
                                        if (pet.Limits.hp > item.HP)
                                        {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.atk_max > item.Atk1)
                                        {
                                            item.Atk1++;
                                            item.Atk2++;
                                            item.Atk3++;

                                            pet.Status.min_atk1++;
                                            pet.Status.max_atk1++;
                                            pet.Status.min_atk2++;
                                            pet.Status.max_atk2++;
                                            pet.Status.min_atk3++;
                                            pet.Status.max_atk3++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ATK1;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.hit_melee > item.HitMelee)
                                        {
                                            item.HitMelee++;
                                            pet.Status.hit_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMelee;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.hit_ranged > item.HitRanged)
                                        {
                                            item.HitRanged++;
                                            pet.Status.hit_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitRanged;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.aspd > item.ASPD)
                                        {
                                            item.ASPD++;
                                            pet.Status.aspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ASPD;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.def_add > item.Def)
                                        {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 6:
                                        if (pet.Limits.mdef_add > item.MDef)
                                        {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 7:
                                        if (pet.Limits.avoid_melee > item.AvoidMelee)
                                        {
                                            item.AvoidMelee++;
                                            pet.Status.avoid_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMelee;
                                        }

                                        break;
                                    case 8:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged)
                                        {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.UseSkill:
                                type = Global.Random.Next(0, 2);
                                switch (type)
                                {
                                    case 0:
                                        if (pet.Limits.matk_max > item.MAtk)
                                        {
                                            item.MAtk++;
                                            pet.Status.min_matk++;
                                            pet.Status.max_matk++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MATK;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.hit_ranged > item.HitMagic)
                                        {
                                            item.HitMagic++;
                                            pet.Status.hit_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMagic;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.cspd > item.CSPD)
                                        {
                                            item.CSPD++;
                                            pet.Status.cspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.CSPD;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.PhysicalHit:
                                type = Global.Random.Next(0, 3);
                                switch (type)
                                {
                                    case 0:
                                        if (pet.Limits.hp > item.HP)
                                        {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.def_add > item.Def)
                                        {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.avoid_melee > item.AvoidMelee)
                                        {
                                            item.AvoidMelee++;
                                            pet.Status.avoid_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMelee;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged)
                                        {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.SkillHit:
                                type = Global.Random.Next(0, 5);
                                switch (type)
                                {
                                    case 0:
                                        if (pet.Limits.hp > item.HP)
                                        {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.mdef_add > item.MDef)
                                        {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.mp > item.MPRecover)
                                        {
                                            item.MPRecover++;
                                            pet.Status.mp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MPRecover;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged)
                                        {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.def_add > item.Def)
                                        {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.avoid_magic > item.AvoidMagic)
                                        {
                                            item.AvoidMagic++;
                                            pet.Status.avoid_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMagic;
                                        }

                                        break;
                                }

                                break;
                        }
                    }
                    else
                    {
                        int type;
                        switch (reason)
                        {
                            case PetGrowthReason.PhysicalBeenHit:
                                type = Global.Random.Next(0, 14);
                                switch (type)
                                {
                                    case 0:
                                        if (pet.Limits.hp > item.HP)
                                        {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.hit_melee > item.HitMelee)
                                        {
                                            item.HitMelee++;
                                            pet.Status.hit_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMelee;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.hit_ranged > item.HitRanged)
                                        {
                                            item.HitRanged++;
                                            pet.Status.hit_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitRanged;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.aspd > item.ASPD)
                                        {
                                            item.ASPD++;
                                            pet.Status.aspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ASPD;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.matk_max > item.MAtk)
                                        {
                                            item.MAtk++;
                                            pet.Status.min_matk++;
                                            pet.Status.max_matk++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MATK;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.hit_ranged > item.HitMagic)
                                        {
                                            item.HitMagic++;
                                            pet.Status.hit_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMagic;
                                        }

                                        break;
                                    case 6:
                                        if (pet.Limits.cspd > item.CSPD)
                                        {
                                            item.CSPD++;
                                            pet.Status.cspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.CSPD;
                                        }

                                        break;
                                    case 7:
                                        if (pet.Limits.aspd > item.ASPD)
                                        {
                                            item.ASPD++;
                                            pet.Status.aspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ASPD;
                                        }

                                        break;
                                    case 8:
                                        if (pet.Limits.atk_max > item.Atk1)
                                        {
                                            item.Atk1++;
                                            item.Atk2++;
                                            item.Atk3++;

                                            pet.Status.min_atk1++;
                                            pet.Status.max_atk1++;
                                            pet.Status.min_atk2++;
                                            pet.Status.max_atk2++;
                                            pet.Status.min_atk3++;
                                            pet.Status.max_atk3++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.ATK1;
                                        }

                                        break;
                                    case 9:
                                        if (pet.Limits.cri > item.HitCritical)
                                        {
                                            item.HitCritical++;
                                            pet.Status.hit_critical++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Critical;
                                        }

                                        break;
                                    case 10:
                                        if (pet.Limits.criavd > item.AvoidCritical)
                                        {
                                            item.AvoidCritical++;
                                            pet.Status.avoid_critical++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidCri;
                                        }

                                        break;
                                    case 11:
                                        if (pet.Limits.def_add > item.Def)
                                        {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 12:
                                        if (pet.Limits.mdef_add > item.MDef)
                                        {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 13:
                                        if (pet.Limits.avoid_melee > item.AvoidMelee)
                                        {
                                            item.AvoidMelee++;
                                            pet.Status.avoid_melee++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMelee;
                                        }

                                        break;
                                    case 14:
                                        if (pet.Limits.avoid_ranged > item.AvoidRanged)
                                        {
                                            item.AvoidRanged++;
                                            pet.Status.avoid_ranged++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidRanged;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.UseSkill:
                                type = Global.Random.Next(0, 6);
                                switch (type)
                                {
                                    case 0:
                                        if (pet.Limits.hit_ranged > item.HitMagic)
                                        {
                                            item.HitMagic++;
                                            pet.Status.hit_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HitMagic;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.matk_max > item.MAtk)
                                        {
                                            item.MAtk++;
                                            pet.Status.min_matk++;
                                            pet.Status.max_matk++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MATK;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.cspd > item.CSPD)
                                        {
                                            item.CSPD++;
                                            pet.Status.cspd++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.CSPD;
                                        }

                                        break;
                                    case 3:
                                        if (pet.Limits.mdef_add > item.MDef)
                                        {
                                            item.MDef++;
                                            pet.Status.mdef_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MDef;
                                        }

                                        break;
                                    case 4:
                                        if (pet.Limits.mp > item.MPRecover)
                                        {
                                            item.MPRecover++;
                                            pet.Status.mp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MPRecover;
                                        }

                                        break;
                                    case 5:
                                        if (pet.Limits.def_add > item.Def)
                                        {
                                            item.Def++;
                                            pet.Status.def_add++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Def;
                                        }

                                        break;
                                    case 6:
                                        if (pet.Limits.avoid_magic > item.AvoidMagic)
                                        {
                                            item.AvoidMagic++;
                                            pet.Status.avoid_magic++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.AvoidMagic;
                                        }

                                        break;
                                }

                                break;
                            case PetGrowthReason.ItemRecover:
                                type = Global.Random.Next(0, 2);
                                switch (type)
                                {
                                    case 0:
                                        if (pet.Limits.hp > item.HP)
                                        {
                                            item.HP++;
                                            pet.MaxHP++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.HP;
                                        }

                                        break;
                                    case 1:
                                        if (pet.Limits.mp > item.MPRecover)
                                        {
                                            item.MPRecover++;
                                            pet.Status.mp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.MPRecover;
                                        }

                                        break;
                                    case 2:
                                        if (pet.Limits.hp > item.HPRecover)
                                        {
                                            item.HPRecover++;
                                            pet.Status.hp_recover_skill++;
                                            growType = SSMG_ACTOR_PET_GROW.GrowType.Recover;
                                        }

                                        break;
                                }

                                break;
                        }
                    }

                    if (pet.Owner.Online)
                    {
                        if (pet.Ride)
                        {
                            StatusFactory.Instance.CalcStatus(pet.Owner);
                            MapClient.FromActorPC(pet.Owner).SendPlayerInfo();
                        }

                        MapClient.FromActorPC(pet.Owner).SendItemInfo(item);
                    }

                    SendPetGrowth(actor, growType, 1);
                }
            }
        }
    }
}