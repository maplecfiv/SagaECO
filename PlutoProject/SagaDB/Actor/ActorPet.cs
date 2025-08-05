using SagaDB.Mob;

namespace SagaDB.Actor
{
    public class ActorPet : ActorMob, IStats
    {
        public ActorPet()
        {
        }

        public ActorPet(uint mobID, Item.Item pet)
        {
            type = ActorType.PET;
            baseData = MobFactory.Instance.GetPetData(mobID);
            Limits = MobFactory.Instance.GetPetLimit(mobID);
            if (pet.HP > Limits.hp)
                pet.HP = (short)Limits.hp;
            MaxHP = (uint)(baseData.hp + pet.HP);
            HP = MaxHP;
            MaxMP = (uint)(baseData.mp + pet.MP);
            MP = MaxMP;
            MaxSP = (uint)(baseData.sp + pet.SP);
            SP = MaxSP;
            Name = baseData.name;
            Speed = baseData.speed;
            Status.attackType = baseData.attackType;
            if (pet.ASPD > Limits.aspd)
                pet.ASPD = Limits.aspd;
            Status.aspd = (short)(baseData.aspd + pet.ASPD);
            if (pet.CSPD > Limits.cspd)
                pet.CSPD = Limits.cspd;
            Status.cspd = (short)(baseData.cspd + pet.CSPD);
            Status.def = baseData.def;
            if (pet.Def > Limits.def_add)
                pet.Def = (short)Limits.def_add;
            Status.def_add = (short)(baseData.def_add + pet.Def);
            Status.mdef = baseData.mdef;
            if (pet.MDef > Limits.mdef_add)
                pet.MDef = (short)Limits.mdef_add;
            Status.mdef_add = (short)(baseData.mdef_add + pet.MDef);
            if (pet.Atk1 > Limits.atk_max)
                pet.Atk1 = (short)Limits.atk_max;
            Status.min_atk1 = (ushort)(baseData.atk_min + pet.Atk1);
            Status.max_atk1 = (ushort)(baseData.atk_max + pet.Atk1);
            Status.min_atk2 = (ushort)(baseData.atk_min + pet.Atk2);
            Status.max_atk2 = (ushort)(baseData.atk_max + pet.Atk2);
            Status.min_atk3 = (ushort)(baseData.atk_min + pet.Atk3);
            Status.max_atk3 = (ushort)(baseData.atk_max + pet.Atk3);
            if (pet.MAtk > Limits.matk_max)
                pet.MAtk = (short)Limits.matk_max;
            Status.min_matk = (ushort)(baseData.matk_min + pet.MAtk);
            Status.max_matk = (ushort)(baseData.matk_max + pet.MAtk);

            if (pet.HitMelee > Limits.hit_melee)
                pet.HitMelee = (short)Limits.hit_melee;
            Status.hit_melee = (ushort)(baseData.hit_melee + pet.HitMelee);
            if (pet.HitRanged > Limits.hit_ranged)
                pet.HitRanged = (short)Limits.hit_ranged;
            Status.hit_ranged = (ushort)(baseData.hit_ranged + pet.HitRanged);
            if (pet.AvoidMelee > Limits.avoid_melee)
                pet.AvoidMelee = (short)Limits.avoid_melee;
            Status.avoid_melee = (ushort)(baseData.avoid_melee + pet.AvoidMelee);
            if (pet.AvoidRanged > Limits.avoid_ranged)
                pet.AvoidRanged = (short)Limits.avoid_ranged;

            Status.avoid_ranged = (ushort)(baseData.avoid_ranged + pet.AvoidRanged);
            sightRange = 1500;

            PictID = pet.PictID;
        }
        /*
        public ActorPet(uint mobID)
        {
            this.type = ActorType.PET;
            this.baseData = Mob.MobFactory.Instance.GetPetData(mobID);
            this.limits = Mob.MobFactory.Instance.GetPetLimit(mobID);
            this.MaxHP = this.baseData.hp;
            this.HP = this.MaxHP;
            this.MaxMP = this.baseData.mp;
            this.MP = this.MaxMP;
            this.MaxSP = this.baseData.sp;
            this.SP = this.MaxSP;
            this.Name = this.baseData.name;
            this.Speed = this.baseData.speed;
            this.Status.attackType = this.baseData.attackType;
            this.Status.aspd = this.baseData.aspd;
            this.Status.cspd = this.baseData.cspd;
            this.Status.def = this.baseData.def;
            this.Status.def_add = (short)this.baseData.def_add;
            this.Status.mdef = this.baseData.mdef;
            this.Status.mdef_add = (short)this.baseData.mdef_add;
            this.Status.min_atk1 = this.baseData.atk_min;
            this.Status.max_atk1 = this.baseData.atk_max;
            this.Status.min_atk2 = this.baseData.atk_min;
            this.Status.max_atk2 = this.baseData.atk_max;
            this.Status.min_atk3 = this.baseData.atk_min;
            this.Status.max_atk3 = this.baseData.atk_max;
            this.Status.min_matk = this.baseData.matk_min;
            this.Status.max_matk = this.baseData.matk_max;

            this.Status.hit_melee = this.baseData.hit_melee;
            this.Status.hit_ranged = this.baseData.hit_ranged;
            this.Status.avoid_melee = this.baseData.avoid_melee;
            this.Status.avoid_ranged = this.baseData.avoid_ranged;
            this.sightRange = 1500;
        }*/

        public ActorPC Owner { get; set; }

        public bool Ride { get; set; }

        public uint PetID => BaseData.id;

        public MobData Limits { get; }

        /// <summary>
        ///     是否为联合宠物
        /// </summary>
        public bool IsUnion { get; }
    }
}