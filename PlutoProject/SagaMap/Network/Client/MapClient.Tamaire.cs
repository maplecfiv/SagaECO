using System;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using SagaMap.PC;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void OnTamaireRentalRequest(CSMG_TAMAIRE_RENTAL_REQUEST p)
        {
            var lender = MapServer.charDB.GetChar(p.Lender);
            TamaireRentalManager.Instance.ProcessRental(Character, lender);
            SendTamaire();
            StatusFactory.Instance.CalcStatus(Character);
            SendPlayerInfo();
        }

        public void SendTamaire()
        {
            if (Character.TamaireRental == null)
                return;
            if (Character.TamaireRental.CurrentLender == 0)
                return;
            var p = new SSMG_TAMAIRE_RENTAL();
            var lender = MapServer.charDB.GetChar(Character.TamaireRental.CurrentLender);
            p.JobType = lender.TamaireLending.JobType;
            p.RentalDue = Character.TamaireRental.RentDue - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            p.Factor = (short)((1f -
                                TamaireRentalManager.Instance.CalcFactor(lender.TamaireLending.Baselv -
                                                                         Character.Level)) * 1000);
            netIO.SendPacket(p);
        }

        public void OnTamaireRentalTerminateRequest(CSMG_TAMAIRE_RENTAL_TERMINATE_REQUEST p)
        {
            OnTamaireRentalTerminate(1);
        }

        public void OnTamaireRentalTerminate(byte reason)
        {
            var p = new SSMG_TAMAIRE_RENTAL_TERMINATE();
            p.Reason = reason;
            netIO.SendPacket(p);
        }

        public void OpenTamaireListUI()
        {
            var p = new SSMG_TAMAIRE_LIST_UI();
            netIO.SendPacket(p);
        }
    }
}