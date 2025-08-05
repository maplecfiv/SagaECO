using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Tamaire;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Server;

namespace SagaLogin.Network.Client
{
    public partial class LoginClient : SagaLib.Client
    {
        public void OnTamaireListRequest(CSMG_TAMAIRE_LIST_REQUEST p)
        {
            var p1 = new SSMG_TAMAIRE_LIST();
            int a;
            var data = GetLendings(p.JobType, false, false, p.minlevel, p.maxlevel, p.page, out a);
            p1.PutData(data, selectedChar.Level);
            netIO.SendPacket(p1);
        }

        public List<TamaireLending> GetLendings(byte jobtype, bool isFriendOnly, bool isRingOnly, byte minlevel,
            byte maxlevel, int page, out int maxPage)
        {
            var items = LoginServer.charDB.GetTamaireLendings();
            var query = from lending in items
                where lending.Baselv >= minlevel && lending.Baselv <= maxlevel
                                                 && LoginServer.charDB.GetAccountID(lending.Lender) != account.AccountID
                                                 && DateTime.Now < lending.PostDue
                                                 && lending.MaxLendings > lending.Renters.Count
                select lending;
            var list = query.ToList();
            if (selectedChar.TamaireRental != null)
                list = (from lending in list
                    where lending.Lender != selectedChar.TamaireRental.LastLender
                    select lending).ToList();
            if (jobtype != 0xFF)
                list = (from lending in list where lending.JobType == jobtype select lending).ToList();
            if (isFriendOnly)
                list = (from lending in list
                    where LoginServer.charDB.GetFriendList(selectedChar)
                        .Contains(LoginServer.charDB.GetChar(lending.Lender))
                    select lending).ToList();
            if (isRingOnly)
                list = (from lending in list
                    where LoginServer.charDB.GetChar(lending.Lender).Ring == selectedChar.Ring
                    select lending).ToList();
            if (list.Count % 10 == 0)
                maxPage = list.Count / 10;
            else
                maxPage = list.Count / 10 + 1;

            var rentals = (from lending in list
                where list.IndexOf(lending) >= page * 10 && list.IndexOf(lending) < (page + 1) * 10
                select lending).ToList();
            return rentals;
        }
    }
}