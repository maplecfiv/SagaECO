using System;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Server;

namespace SagaLogin.Network.Client
{
    public partial class LoginClient : SagaLib.Client
    {
        public void OnRingEmblemNew(CSMG_RING_EMBLEM_NEW p)
        {
            bool needUpdate;
            DateTime newDate;
            byte[] data;
            data = LoginServer.charDB.GetRingEmblem(p.RingID, new DateTime(1970, 1, 1), out needUpdate, out newDate);
            SendRingEmblem(p.RingID, data, needUpdate, newDate);
        }

        public void OnRingEmblem(CSMG_RING_EMBLEM p)
        {
            bool needUpdate;
            DateTime newDate;
            byte[] data;
            data = LoginServer.charDB.GetRingEmblem(p.RingID, p.UpdateTime, out needUpdate, out newDate);
            SendRingEmblem(p.RingID, data, needUpdate, newDate);
        }

        private void SendRingEmblem(uint ringid, byte[] data, bool needUpdate, DateTime newDate)
        {
            var p = new SSMG_RING_EMBLEM();
            if (needUpdate)
                p.Result = 0;
            else
                p.Result = 1;
            p.RingID = ringid;
            if (data != null)
            {
                p.Result2 = 0;
                if (needUpdate)
                    p.Data = data;
                p.UpdateTime = newDate;
            }
            else
            {
                p.Result2 = 1;
            }

            netIO.SendPacket(p);
        }
    }
}