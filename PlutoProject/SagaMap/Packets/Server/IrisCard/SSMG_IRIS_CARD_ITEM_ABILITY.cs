using System.Collections.Generic;
using SagaDB.Iris;
using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_CARD_ITEM_ABILITY : Packet
    {
        public enum Types
        {
            Deck,
            Total,
            Max
        }

        public SSMG_IRIS_CARD_ITEM_ABILITY()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x1DC5;
        }

        public Types Type
        {
            set => PutByte((byte)value, 2);
        }

        public List<AbilityVector> AbilityVectors
        {
            set
            {
                var buff = new byte[data.Length + 4 * value.Count];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count, 3);
                foreach (var i in value) PutUInt(i.ID);
            }
        }

        public List<int> VectorValues
        {
            set
            {
                var buff = new byte[data.Length + 2 * value.Count];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count);
                foreach (var i in value) PutShort((short)i);
            }
        }

        public List<int> VectorLevels
        {
            set
            {
                var buff = new byte[data.Length + value.Count];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count);
                foreach (var i in value) PutByte((byte)i);
            }
        }

        public List<ReleaseAbility> ReleaseAbilities
        {
            set
            {
                var buff = new byte[data.Length + 4 * value.Count];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count);
                foreach (var i in value) PutInt((int)i);
            }
        }

        public List<int> AbilityValues
        {
            set
            {
                var buff = new byte[data.Length + 4 * value.Count];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count);
                foreach (var i in value) PutInt(i);
            }
        }

        public Dictionary<Elements, int> ElementsAttack
        {
            set
            {
                var buff = new byte[data.Length + 4 * value.Count + 1];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte(0);
                PutByte((byte)value.Count);
                for (var i = 0; i < value.Count; i++) PutUShort((ushort)value[(Elements)i]);
            }
        }

        public Dictionary<Elements, int> ElementsDefence
        {
            set
            {
                var buff = new byte[data.Length + 4 * value.Count];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count);
                for (var i = 0; i < value.Count; i++) PutUShort((ushort)value[(Elements)i]);
            }
        }
    }
}