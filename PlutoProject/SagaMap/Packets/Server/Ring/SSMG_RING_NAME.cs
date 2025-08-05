using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_RING_NAME : Packet
    {
        public SSMG_RING_NAME()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x1AD1;
        }

        public ActorPC Player
        {
            set
            {
                PutUInt(value.ActorID, 2);
                if (value.Ring != null)
                    PutUInt(value.Ring.ID);
                else
                    PutUInt(0);
                byte[] buf;
                if (value.PlayerTitle != "" && value.PlayerTitle != " ")
                    buf = Global.Unicode.GetBytes(value.PlayerTitle + "\0");
                else
                    buf = Global.Unicode.GetBytes("未装备称号" + "\0");
                var buff = new byte[12 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutByte((byte)buf.Length);
                PutBytes(buf);
                if (value.Ring != null)
                    if (value == value.Ring.Leader)
                        PutByte(1);
                /*if (value.Ring != null)
                    this.PutUInt(value.Ring.ID);
                else
                    this.PutUInt(0);

                byte[] buf;
                if (value.Ring != null)
                    buf = Global.Unicode.GetBytes(value.Ring.Name + "\0");
                else
                    buf = new byte[1];
                byte[] buff = new byte[12 + buf.Length];
                this.data.CopyTo(buff, 0);
                this.data = buff;
                this.PutByte((byte)buf.Length);
                this.PutBytes(buf);
                if (value.Ring != null)
                    if (value == value.Ring.Leader)
                        this.PutByte(1);*/
            }
        }
    }
}