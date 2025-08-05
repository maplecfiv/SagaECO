using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_PET_GROW : Packet
    {
        public enum GrowType
        {
            HP,
            SP,
            MP,
            Speed,
            ATK1,
            ATK2,
            ATK3,
            MATK,
            Def,
            MDef,
            HitMelee,
            HitRanged,
            HitMagic,
            AvoidMelee,
            AvoidRanged,
            AvoidMagic,
            Critical,
            AvoidCri,
            Recover,
            MPRecover,
            Stamina,
            ASPD,
            CSPD
        }

        public SSMG_ACTOR_PET_GROW()
        {
            data = new byte[18];
            offset = 2;
            ID = 0x12C0;
        }

        public uint PetActorID
        {
            set => PutUInt(value, 2);
        }

        public uint OwnerActorID
        {
            set => PutUInt(value, 6);
        }

        public GrowType Type
        {
            set => PutUInt((uint)value, 10);
        }

        public uint Value
        {
            set => PutUInt(value, 14);
        }
    }
}