using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_ACTOR_APPEAR : Packet
    {
        public SSMG_ITEM_ACTOR_APPEAR()
        {
            if (Configuration.Configuration.Instance.Version < Version.Saga9_Iris)
                data = new byte[26];
            else
                data = new byte[29];
            offset = 2;
            ID = 0x07D5;
        }

        public ActorItem Item
        {
            set
            {
                var info = MapManager.Instance.GetMap(value.MapID).Info;
                PutUInt(value.ActorID, 2);
                PutUInt(value.Item.ItemID, 6);
                PutByte(Global.PosX16to8(value.X, info.width), 10);
                PutByte(Global.PosY16to8(value.Y, info.height), 11);
                PutUShort(value.Item.Stack, 12);
                PutUInt(10, 14); //Unknown
                if (value.PossessionItem)
                    PutByte(1, 22); //type, possession item is 1, otherwise 0
                else
                    PutByte(0, 22); //type, possession item is 1, otherwise 0
                var buf = Global.Unicode.GetBytes(value.Comment + "\0");
                var count = (byte)buf.Length;
                var buff = new byte[29 + count];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte(count, 23);
                PutBytes(buf, 24);
                PutByte(value.Item.identified, (ushort)(27 + count));
                byte fusion = 0;
                if (value.Item.PictID != 0)
                    fusion = 1;
                PutByte(fusion, (ushort)(28 + count));
            }
        }
    }
}