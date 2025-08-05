using SagaDB.Mob;

namespace SagaDB.Actor
{
    public class ActorShadow : ActorPet
    {
        public ActorShadow(ActorPC creator)
        {
            baseData = new MobData();
            baseData.level = creator.Level;
            Status.attackType = creator.Status.attackType;
            Status.aspd = creator.Status.aspd;
            Status.def = creator.Status.def;
            Status.def_add = creator.Status.def_add;
            Status.mdef = creator.Status.mdef;
            Status.mdef_add = creator.Status.mdef_add;
            Status.min_atk1 = creator.Status.min_atk1;
            Status.max_atk1 = creator.Status.max_atk1;
            Status.min_atk2 = creator.Status.min_atk2;
            Status.max_atk2 = creator.Status.max_atk2;
            Status.min_atk3 = creator.Status.min_atk3;
            Status.max_atk3 = creator.Status.max_atk3;
            Status.min_matk = creator.Status.min_matk;
            Status.max_matk = creator.Status.max_matk;

            Status.hit_melee = creator.Status.hit_melee;
            Status.hit_ranged = creator.Status.hit_ranged;
            Status.avoid_melee = creator.Status.avoid_melee;
            Status.avoid_ranged = creator.Status.avoid_ranged;
            MaxHP = 1;
            HP = 1;
            type = ActorType.SHADOW;
            sightRange = 1500;
            Owner = creator;
            Speed = 100;
        }
    }
}