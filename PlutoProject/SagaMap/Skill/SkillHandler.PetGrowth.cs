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
}