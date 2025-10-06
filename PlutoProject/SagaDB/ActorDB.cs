using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.BBS;
using SagaDB.Tamaire;

namespace SagaDB
{

    public class GetRingEmblemResult
    {
        public byte[] Data { get; }
        public bool NeedUpdate { get; }

        public DateTime NewTime { get; }

        public GetRingEmblemResult(byte[] data, bool needUpdate, DateTime newTime)
        {
            Data = data;
            NeedUpdate = needUpdate;
            NewTime = newTime;
        }
    }
    public interface ActorDB
    {
        void AJIClear();

        /// <summary>
        ///     Write the given character to the database.
        /// </summary>
        /// <param name="aChar">Character that needs to be writen.</param>
        void SaveChar(ActorPC aChar);

        void SavePaper(ActorPC aChar);
        void SaveChar(ActorPC aChar, bool fullinfo);

        void SaveChar(ActorPC aChar, bool itemInfo, bool fullinfo);

        void CreateChar(ActorPC aChar, int account_id);

        void SaveVar(ActorPC aChar);

        void DeleteChar(ActorPC aChar);

        ActorPC GetChar(uint charID);

        ActorPC GetChar(uint charID, bool fullinfo);

        bool CharExists(string name);

        uint GetAccountID(ActorPC pc);

        uint GetAccountID(uint charID);

        uint[] GetCharIDs(int account_id);

        string GetCharName(uint id);

        bool Connect();

        bool isConnected();

        /// <summary>
        ///     取得指定玩家的好友列表
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>好友列表</returns>
        List<ActorPC> GetFriendList(ActorPC pc);

        /// <summary>
        ///     取得添加指定玩家为好友的玩家列表
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>玩家列表</returns>
        List<ActorPC> GetFriendList2(ActorPC pc);

        void AddFriend(ActorPC pc, uint charID);

        bool IsFriend(uint char1, uint char2);

        void DeleteFriend(uint char1, uint char2);

        Party.Party GetParty(uint id);

        void NewParty(Party.Party party);

        void SaveParty(Party.Party party);

        void DeleteParty(Party.Party party);

        Ring.Ring GetRing(uint id);

        void NewRing(Ring.Ring ring);

        void SaveRing(Ring.Ring ring, bool saveMembers);

        void DeleteRing(Ring.Ring ring);

        void RingEmblemUpdate(Ring.Ring ring, byte[] buf);

        GetRingEmblemResult GetRingEmblem(uint ring_id, DateTime date);

        List<Post> GetBBS(uint bbsID);

        List<Post> GetBBSPage(uint bbsID, int page);

        List<Mail> GetMail(ActorPC pc);

        bool BBSNewPost(ActorPC poster, uint bbsID, string title, string content);

        ActorPC LoadServerVar();

        void SaveServerVar(ActorPC fakepc);

        void GetVShop(ActorPC pc);

        void SaveSkill(ActorPC pc);

        void GetSkill(ActorPC pc);

        void SaveVShop(ActorPC pc);

        uint CreatePartner(Item.Item partnerItem);

        void SavePartner(ActorPartner ap);

        void SavePartnerEquip(ActorPartner ap);

        void SavePartnerCube(ActorPartner ap);

        void SavePartnerAI(ActorPartner ap);

        ActorPartner GetActorPartner(uint ActorPartnerID, Item.Item partneritem);

        void GetPartnerEquip(ActorPartner ap);

        void GetPartnerCube(ActorPartner ap);

        void GetPartnerAI(ActorPartner ap);

        void SaveWRP(ActorPC pc);

        List<ActorPC> GetWRPRanking();

        List<FlyingCastle.FlyingCastle> GetFlyingCastles();

        void SaveFlyCastle(Ring.Ring ring);

        void SaveSerFF(Server.Server ser);

        void GetSerFFurniture(Server.Server ser);

        void SaveFlyCastleCopy(Dictionary<FlyingCastle.FurniturePlace, List<ActorFurniture>> Furnitures);

        void CreateFF(ActorPC pc);

        void GetFlyCastle(ActorPC pc);

        uint GetFlyCastleRindID(uint ffid);

        void GetFlyingCastleFurniture(Ring.Ring ring);

        void GetFlyingCastleFurnitureCopy(Dictionary<FlyingCastle.FurniturePlace, List<ActorFurniture>> Furnitures);

        void SavaLevelLimit();

        void GetLevelLimit();

        List<Gift> GetGifts(ActorPC pc);

        bool DeleteGift(Gift gift);

        bool DeleteMail(Mail mail);

        uint AddNewGift(Gift gift);

        List<TamaireLending> GetTamaireLendings();

        void GetTamaireLending(ActorPC pc);

        void CreateTamaireLending(TamaireLending tamaireLending);

        void DeleteTamaireLending(TamaireLending tamaireLending);

        void SaveTamaireLending(TamaireLending tamaireLending);

        void GetTamaireRental(ActorPC pc);

        void CreateTamaireRental(TamaireRental tamaireRental);

        void SaveTamaireRental(TamaireRental tamaireRental);

        void DeleteTamaireRental(TamaireRental tamaireRental);

        void SaveNPCState(ActorPC pc, uint npcID);

        void SaveStamp(ActorPC pc, StampGenre genre);

        void SaveMosterGuide(ActorPC pc, uint mobID, bool state);

        void GetMosterGuide(ActorPC pc);


        //#region 副职相关

        void GetDualJobInfo(ActorPC pc);

        void SaveDualJobInfo(ActorPC pc, bool allinfo);

        void GetDualJobSkill(ActorPC pc);

        void SaveDualJobSkill(ActorPC pc);

        //#endregion
    }
}