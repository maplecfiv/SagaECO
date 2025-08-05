using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Actor
{
    public class CSMG_PLAYER_MOVE : Packet
    {
        public CSMG_PLAYER_MOVE()
        {
            data = new byte[10];
            ID = 0x11F8;
            offset = 2;
        }

        public short X
        {
            get => GetShort(2);
            set => PutShort(value, 2);
        }

        public short Y
        {
            get => GetShort(4);
            set => PutShort(value, 4);
        }

        public ushort Dir
        {
            get => GetUShort(6);
            set => PutUShort(value, 6);
        }

        public MoveType MoveType
        {
            get => (MoveType)GetUShort(8);
            set => PutUShort((ushort)value, 8);
        }


        public override Packet New()
        {
            return new CSMG_PLAYER_MOVE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnMove(this);
        }
    }
}