//Comment this out to deactivate the dead lock check!

#define DeadLockCheck

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Network.Client;
using SagaMap.Packets.Client;
using SagaMap.Packets.Client.AbyssTeam;
using SagaMap.Packets.Client.Actor;
using SagaMap.Packets.Client.Another;
using SagaMap.Packets.Client.Bond;
using SagaMap.Packets.Client.Chat;
using SagaMap.Packets.Client.Community;
using SagaMap.Packets.Client.DEM;
using SagaMap.Packets.Client.DualJob;
using SagaMap.Packets.Client.FFarden;
using SagaMap.Packets.Client.FGarden;
using SagaMap.Packets.Client.Fish;
using SagaMap.Packets.Client.Golem;
using SagaMap.Packets.Client.InfiniteCorridor;
using SagaMap.Packets.Client.IrisCard;
using SagaMap.Packets.Client.Item;
using SagaMap.Packets.Client.Login;
using SagaMap.Packets.Client.Navi;
using SagaMap.Packets.Client.NCShop;
using SagaMap.Packets.Client.NPC;
using SagaMap.Packets.Client.Partner;
using SagaMap.Packets.Client.Party;
using SagaMap.Packets.Client.Possession;
using SagaMap.Packets.Client.PProtect;
using SagaMap.Packets.Client.Quest;
using SagaMap.Packets.Client.Ring;
using SagaMap.Packets.Client.Skill;
using SagaMap.Packets.Client.Tamaire;
using SagaMap.Packets.Client.Trade;
using SagaMap.Packets.Client.VShop;

namespace SagaMap.Manager
{
    public sealed class MapClientManager : ClientManager
    {
        public Thread check;

        private MapClientManager()
        {
            Clients = new List<MapClient>();
            commandTable = new Dictionary<ushort, Packet>();
            //this.commandTable.Add(0x6000, new Packets.Client.TEST_YUMEMI_1());
            commandTable.Add(0x205B, new CSMG_FF_FURNITURE_USE());


            commandTable.Add(0x0690, new CSMG_CHAT_GIFT_TAKE());

            commandTable.Add(0x1F76, new CSMG_DAILYDUNGEON_OPEN());
            commandTable.Add(0x1F78, new CSMG_DAILYDUNGEON_JOIN());

            /*稱號系統*/
            commandTable.Add(0x2418, new CSMG_PLAYER_SETTITLE());
            commandTable.Add(0x254C, new CSMG_PLAYER_TITLE_CANCLENEW());
            commandTable.Add(0x241B, new CSMG_PLAYER_TITLE_REQUIRE());
            /*稱號系統*/

            /*Another系统*/
            commandTable.Add(0x23AA, new CSMG_ANO_PAPER_EQUIP());
            commandTable.Add(0x23AC, new CSMG_ANO_PAPER_TAKEOFF());
            commandTable.Add(0x23A4, new CSMG_ANO_UI_OPEN());
            commandTable.Add(0x23A6, new CSMG_ANO_PAPER_USE());
            commandTable.Add(0x23A8, new CSMG_ANO_PAPER_COMPOUND());
            /*Another系统*/

            /*潜在强化*/
            commandTable.Add(0x1F59, new CSMG_ITEM_MASTERENHANCE_CLOSE());
            commandTable.Add(0x1F57, new CSMG_ITEM_MASTERENHANCE_CONFIRM());
            commandTable.Add(0x1F55, new CSMG_ITEM_MASTERENHANCE_SELECT());
            /*潜在强化*/

            /*新道具界面類*/
            commandTable.Add(0x1CF2, new CSMG_ITEM_FACEVIEW());
            commandTable.Add(0x1CF4, new CSMG_ITEM_FACECHANGE());
            commandTable.Add(0x1CF6, new CSMG_ITEM_FACEVIEW_CLOSE());
            /*新道具界面類*/

            /*飛空城*/
            commandTable.Add(0x200D, new CSMG_FF_FURNITURE_ROOM_APPEAR());
            commandTable.Add(0x200E, new Packet());
            commandTable.Add(0x205D, new CSMG_FF_FURNITURE_REMOVE());
            commandTable.Add(0x2010, new CSMG_FFGARDEN_JOIN());
            commandTable.Add(0x2012, new CSMG_FFGARDEN_JOIN_OTHER());
            commandTable.Add(0x203A, new CSMG_FF_CASTLE_SETUP());
            commandTable.Add(0x203C, new CSMG_FF_FURNITURE_REMOVE_CASTLE());
            commandTable.Add(0x2046, new CSMG_FF_FURNITURE_ROOM_ENTER());
            commandTable.Add(0x2059, new CSMG_FF_FURNITURE_SETUP());
            commandTable.Add(0x20DE, new CSMG_FF_UNIT_SETUP());
            /*飛空城*/

            commandTable.Add(0x2061, new CSMG_FF_FURNITURE_RESET());

            //钓鱼
            commandTable.Add(0x216C, new CSMG_FF_FISHBAIT_EQUIP()); //装备鱼饵

            //心憑依 たまいれ
            commandTable.Add(0x22B1, new Packet());
            commandTable.Add(0x22B2, new CSMG_TAMAIRE_RENTAL_REQUEST());
            commandTable.Add(0x22B4, new CSMG_TAMAIRE_RENTAL_TERMINATE_REQUEST());

            commandTable.Add(0x000A, new CSMG_SEND_VERSION());
            commandTable.Add(0x0010, new CSMG_LOGIN());
            commandTable.Add(0x001E, new Packet()); //dummy packet
            commandTable.Add(0x001F, new CSMG_LOGOUT());
            commandTable.Add(0x001C, new CSMG_SSO_LOGOUT());
            commandTable.Add(0x0032, new CSMG_PING());
            commandTable.Add(0x00B8, new Packet()); //dummy packet ClientCheck
            commandTable.Add(0x01FD, new CSMG_CHAR_SLOT());
            commandTable.Add(0x0208, new CSMG_PLAYER_STATS_UP());
            commandTable.Add(0x0222, new CSMG_PLAYER_ELEMENTS());
            commandTable.Add(0x0227, new CSMG_SKILL_LEARN());
            commandTable.Add(0x022B, new CSMG_SKILL_LEVEL_UP());
            commandTable.Add(0x020C, new CSMG_ACTOR_REQUEST_PC_INFO());
            commandTable.Add(0x0258, new CSMG_PLAYER_STATS_PRE_CALC());
            commandTable.Add(0x02B2, new CSMG_PLAYER_MIRROR_OPEN());
            commandTable.Add(0x02BE, new CSMG_NPC_JOB_SWITCH());
            commandTable.Add(0x03E8, new CSMG_CHAT_PUBLIC());
            commandTable.Add(0x0406, new CSMG_CHAT_PARTY());
            commandTable.Add(0x0410, new CSMG_CHAT_RING());
            commandTable.Add(0x041A, new CSMG_CHAT_SIGN());
            commandTable.Add(0x05E6, new CSMG_NPC_EVENT_START());
            commandTable.Add(0x05F5, new CSMG_NPC_INPUTBOX());
            commandTable.Add(0x05F7, new CSMG_NPC_SELECT());
            commandTable.Add(0x0602, new CSMG_NPC_SHOP_BUY());

            commandTable.Add(0x0604, new CSMG_NPC_SHOP_SELL());
            commandTable.Add(0x0605, new CSMG_NPC_SHOP_CLOSE());
            commandTable.Add(0x0611, new CSMG_ITEM_EXCHANGE_CLOSE());
            commandTable.Add(0x060F, new CSMG_ITEM_EXCHANGE_CONFIRM());
            commandTable.Add(0x0637, new CSMG_DEM_CHIP_CLOSE());
            commandTable.Add(0x0638, new CSMG_DEM_CHIP_CATEGORY());
            commandTable.Add(0x063C, new CSMG_DEM_CHIP_BUY());
            commandTable.Add(0x062D, new CSMG_NCSHOP_CLOSE());
            commandTable.Add(0x062E, new CSMG_NCSHOP_CATEGORY_REQUEST());
            commandTable.Add(0x0632, new CSMG_NCSHOP_BUY());
            commandTable.Add(0x0641, new CSMG_VSHOP_CLOSE());
            commandTable.Add(0x064A, new CSMG_VSHOP_CATEGORY_REQUEST());
            commandTable.Add(0x0654, new CSMG_VSHOP_BUY());
            commandTable.Add(0x07D0, new CSMG_ITEM_DROP());
            commandTable.Add(0x07E4, new CSMG_ITEM_GET());
            commandTable.Add(0x09C4, new CSMG_ITEM_USE());
            commandTable.Add(0x09E2, new CSMG_ITEM_MOVE());
            commandTable.Add(0x09E7, new CSMG_ITEM_EQUIPT());
            commandTable.Add(0x09EA, new CSMG_ITEM_CHANGE_SLOT());
            commandTable.Add(0x09F7, new CSMG_ITEM_WARE_CLOSE());
            commandTable.Add(0x09FA, new CSMG_ITEM_WARE_GET());
            commandTable.Add(0x09FC, new CSMG_ITEM_WARE_PUT());
            commandTable.Add(0x09FE, new CSMG_ITEM_WARE_PAGE());
            commandTable.Add(0x0A0A, new CSMG_TRADE_REQUEST());
            commandTable.Add(0x0A14, new CSMG_TRADE_CONFIRM());
            commandTable.Add(0x0A15, new CSMG_TRADE_PERFORM());
            commandTable.Add(0x0A16, new CSMG_TRADE_CANCEL());
            commandTable.Add(0x0A1B, new CSMG_TRADE_ITEM());
            commandTable.Add(0x0A0D, new CSMG_TRADE_REQUEST_ANSWER());
            commandTable.Add(0x0FAA, new CSMG_SKILL_RANGE_ATTACK());
            commandTable.Add(0x0F96,
                new Packet()); //Dummy packet, dunno what this packet does, if someone knows, tell me    -by liiir1985
            commandTable.Add(0x0F9F, new CSMG_SKILL_ATTACK());
            commandTable.Add(0x0FA3, new CSMG_PLAYER_RETURN_HOME());
            commandTable.Add(0x0FA5, new CSMG_SKILL_CHANGE_BATTLE_STATUS());
            commandTable.Add(0x0FD2,
                new Packet()); //Dummy packet, player dead, dunno why should client tell server,that player is dead
            commandTable.Add(0x11F8, new CSMG_PLAYER_MOVE());
            commandTable.Add(0x11FE, new CSMG_PLAYER_MAP_LOADED());
            commandTable.Add(0x121D, new CSMG_CHAT_WAITTYPE());
            commandTable.Add(0x1D0B, new CSMG_CHAT_EXPRESSION());
            commandTable.Add(0x1216, new CSMG_CHAT_EMOTION());
            commandTable.Add(0x121B, new CSMG_CHAT_MOTION());
            commandTable.Add(0x12CB, new CSMG_NPC_PET_SELECT());
            commandTable.Add(0x1387, new CSMG_SKILL_CAST());
            commandTable.Add(0x13B6, new CSMG_NPC_SYNTHESE());
            commandTable.Add(0x13B9, new CSMG_NPC_SYNTHESE_FINISH());
            commandTable.Add(0x13BA, new CSMG_CHAT_SIT());
            commandTable.Add(0x13C0, new CSMG_ITEM_EQUIPT_REPAIR());
            commandTable.Add(0x13C5, new CSMG_ITEM_ENHANCE_CONFIRM());
            commandTable.Add(0x13C7, new CSMG_ITEM_ENHANCE_CLOSE());
            commandTable.Add(0x13D9, new CSMG_ITEM_FUSION());
            commandTable.Add(0x13DD, new CSMG_ITEM_FUSION_CANCEL());
            //this.commandTable.Add(0x13E3, new Packets.Client.CSMG_IRIS_ADD_SLOT_ITEM_SELECT());
            commandTable.Add(0x13E3, new CSMG_IRIS_ADD_SLOT_CONFIRM()); //从13E5改为13E3
            commandTable.Add(0x13E5, new CSMG_IRIS_ADD_SLOT_CANCEL()); //从13E7改为 13C7和取消强化合并了.
            commandTable.Add(0x140B, new CSMG_IRIS_CARD_ASSEMBLE_CONFIRM());
            commandTable.Add(0x140D, new CSMG_IRIS_CARD_ASSEMBLE_CANCEL());

            /*憑依系統*/
            commandTable.Add(0x177A, new CSMG_POSSESSION_REQUEST());
            commandTable.Add(0x177F, new CSMG_POSSESSION_CANCEL());
            commandTable.Add(0x178E, new CSMG_POSSESSION_CATALOG_REQUEST());
            commandTable.Add(0x1791, new CSMG_POSSESSION_CATALOG_ITEM_INFO_REQUEST());
            commandTable.Add(0x17A2, new CSMG_POSSESSION_PARTNER_REQUEST());
            commandTable.Add(0x17A4, new CSMG_POSSESSION_PARTNER_CANCEL());
            /*憑依系統*/

            commandTable.Add(0x17E8, new CSMG_GOLEM_SHOP_SELL());
            commandTable.Add(0x17EA, new CSMG_GOLEM_SHOP_SELL_CLOSE());
            commandTable.Add(0x17EB, new CSMG_GOLEM_SHOP_SELL_SETUP());
            commandTable.Add(0x17F2, new CSMG_GOLEM_WAREHOUSE());
            commandTable.Add(0x17F4, new CSMG_GOLEM_WAREHOUSE_SET());
            commandTable.Add(0x17F8, new CSMG_GOLEM_WAREHOUSE_GET());
            commandTable.Add(0x17FC, new CSMG_GOLEM_SHOP_OPEN());
            commandTable.Add(0x17FF, new Packet());
            commandTable.Add(0x1803, new CSMG_GOLEM_SHOP_SELL_BUY());
            commandTable.Add(0x181A, new CSMG_GOLEM_SHOP_BUY());
            commandTable.Add(0x181C, new CSMG_GOLEM_SHOP_BUY_CLOSE());
            commandTable.Add(0x181D, new CSMG_GOLEM_SHOP_BUY_SETUP());
            commandTable.Add(0x1825, new Packet());
            commandTable.Add(0x1827, new CSMG_GOLEM_SHOP_BUY_SELL());
            commandTable.Add(0x1991, new CSMG_QUEST_DETAIL_REQUEST());
            commandTable.Add(0x1965, new CSMG_QUEST_SELECT());

            commandTable.Add(0x191A, new CSMG_PLAYER_SHOP_SELL_BUY()); //商人开店
            commandTable.Add(0x1915, new Packet());
            commandTable.Add(0x190A, new CSMG_PLAYER_SETSHOP_OPEN()); //商人开店
            commandTable.Add(0x190C, new CSMG_PLAYER_SETSHOP_CLOSE()); //商人开店
            commandTable.Add(0x190D, new CSMG_PLAYER_SETSHOP_SETUP()); //商人开店
            commandTable.Add(0x1900, new CSMG_PLAYER_SHOP_OPEN()); //商人开店

            commandTable.Add(0x19C9, new CSMG_PARTY_INVITE());
            commandTable.Add(0x19CB, new CSMG_PARTY_INVITE_ANSWER());
            commandTable.Add(0x19CD, new CSMG_PARTY_QUIT());
            commandTable.Add(0x19D2, new CSMG_PARTY_KICK());
            commandTable.Add(0x19D7, new CSMG_PARTY_NAME());
            commandTable.Add(0x19FF, new CSMG_PARTY_ROLL());
            commandTable.Add(0x1AAE, new CSMG_RING_INVITE());
            commandTable.Add(0x1AB7, new CSMG_RING_INVITE_ANSWER(false));
            commandTable.Add(0x1AB8, new CSMG_RING_INVITE_ANSWER(true));
            commandTable.Add(0x1ABD, new CSMG_RING_QUIT());
            commandTable.Add(0x1AC2, new CSMG_RING_KICK());
            commandTable.Add(0x1AD6, new CSMG_RING_RIGHT_SET());
            commandTable.Add(0x1ADB, new CSMG_RING_EMBLEM_UPLOAD());
            commandTable.Add(0x1AF5, new CSMG_COMMUNITY_BBS_CLOSE());
            commandTable.Add(0x1AFE, new CSMG_COMMUNITY_BBS_POST());
            commandTable.Add(0x1B08, new CSMG_COMMUNITY_BBS_REQUEST_PAGE());
            commandTable.Add(0x1B8A, new CSMG_COMMUNITY_RECRUIT_CREATE());
            commandTable.Add(0x1B94, new CSMG_COMMUNITY_RECRUIT_DELETE());
            commandTable.Add(0x1B9E, new CSMG_COMMUNITY_RECRUIT());
            commandTable.Add(0x1BA8, new CSMG_COMMUNITY_RECRUIT_JOIN());
            commandTable.Add(0x1BAE, new CSMG_COMMUNITY_RECRUIT_REQUEST_ANS());
            commandTable.Add(0x1BF8, new CSMG_FGARDEN_EQUIPT());
            commandTable.Add(0x1C02, new CSMG_FGARDEN_FURNITURE_SETUP());
            commandTable.Add(0x1C07, new CSMG_FGARDEN_FURNITURE_USE());
            commandTable.Add(0x1C0C, new CSMG_FGARDEN_FURNITURE_REMOVE());
            commandTable.Add(0x1C11, new CSMG_FGARDEN_FURNITURE_RECONFIG());
            commandTable.Add(0x1C21, new CSMG_FG_WARE_CLOSE());
            commandTable.Add(0x1C2A, new CSMG_FG_WARE_GET());
            commandTable.Add(0x1C2F, new CSMG_FG_WARE_PUT());
            commandTable.Add(0x1D4C, new CSMG_PLAYER_GREETINGS());
            commandTable.Add(0x1DB0, new CSMG_IRIS_CARD_OPEN());
            commandTable.Add(0x1DB2, new CSMG_IRIS_CARD_CLOSE());
            commandTable.Add(0x1DB6, new CSMG_IRIS_CARD_INSERT());
            commandTable.Add(0x1DBB, new CSMG_IRIS_CARD_REMOVE());
            commandTable.Add(0x1DC9, new CSMG_IRIS_CARD_LOCK());
            commandTable.Add(0x1DCB, new CSMG_IRIS_CARD_UNLOCK());
            commandTable.Add(0x1DD9, new CSMG_IRIS_GACHA_DRAW());
            commandTable.Add(0x1DDB, new CSMG_IRIS_GACHA_CANCEL());
            commandTable.Add(0x1E47, new CSMG_DEM_DEMIC_CLOSE());
            commandTable.Add(0x1E4C, new CSMG_DEM_DEMIC_INITIALIZE());
            commandTable.Add(0x1E4E, new CSMG_DEM_DEMIC_CONFIRM());
            commandTable.Add(0x1E50, new CSMG_DEM_STATS_PRE_CALC());
            commandTable.Add(0x1E5B, new CSMG_DEM_COST_LIMIT_CLOSE());
            commandTable.Add(0x1E5C, new CSMG_DEM_COST_LIMIT_BUY());
            commandTable.Add(0x1E7D, new CSMG_DEM_FORM_CHANGE());
            commandTable.Add(0x1E83, new CSMG_DEM_PARTS_CLOSE());
            commandTable.Add(0x1E87, new CSMG_DEM_PARTS_EQUIP());
            commandTable.Add(0x1E88, new CSMG_DEM_PARTS_UNEQUIP());

            //Navi
            commandTable.Add(0x1EAA, new CSMG_NAVI_OPEN());

            commandTable.Add(0x1EBE, new CSMG_ITEM_CHANGE()); //110武器人形
            commandTable.Add(0x1EC0, new CSMG_ITEM_CHANGE_CANCEL()); //110武器人形

            commandTable.Add(0x1EDC, new CSMG_PLAYER_REQUIRE_REBIRTHREWARD()); // 3转特典选择窗口打开请求
            commandTable.Add(0x1EDE, new CSMG_CHAR_FORM()); // 3转特典外观确定
            commandTable.Add(0x1EDF, new Packet()); // 3转特典道具获取
            commandTable.Add(0x0064, new CSMG_0064()); //未知封包

            //Partner system
            commandTable.Add(0x217C, new CSMG_PARTNER_PERK_PREVIEW());
            commandTable.Add(0x217E, new CSMG_PARTNER_PERK_CONFIRM());
            commandTable.Add(0x2180, new CSMG_PARTNER_AI_MODE_SELECTION());
            commandTable.Add(0x2182, new CSMG_PARTNER_AI_DETAIL_OPEN());
            commandTable.Add(0x2184, new CSMG_PARTNER_AI_DETAIL_SETUP());
            commandTable.Add(0x2186, new CSMG_PARTNER_AI_DETAIL_CLOSE());
            commandTable.Add(0x218A, new CSMG_PARTNER_SETEQUIP());
            commandTable.Add(0x2199, new CSMG_PARTNER_SETFOOD());
            commandTable.Add(0x219B, new CSMG_PARTNER_AUTOFEED());
            commandTable.Add(0x219D, new CSMG_PARTNER_UPDATE_REQUEST());
            commandTable.Add(0x21A0, new CSMG_PARTNER_TALK());
            commandTable.Add(0x21A1, new CSMG_PARTNER_PARTNER_MOTION());
            commandTable.Add(0x2188, new CSMG_PARTNER_CUBE_DELETE());

            //查看装备
            commandTable.Add(0x0262, new CSMG_PLAYER_EQUIP_OPEN());

            //家具联动
            commandTable.Add(0x1C35, new CSMG_PLAYER_FURNITURE_SIT());
            commandTable.Add(0x2064, new CSMG_PLAYER_FURNITURE_SIT());

            //無限迴廊中層, 下層
            commandTable.Add(0x229C, new CSMG_INFINITECORRIDOR_TRAP());
            commandTable.Add(0x2294, new CSMG_INFINITECORRIDOR_WARP());

            //奈落隊伍
            commandTable.Add(0x22E3, new CSMG_ABYSSTEAM_LIST_REQUEST());
            commandTable.Add(0x22E5, new CSMG_ABYSSTEAM_LIST_CLOSE());
            commandTable.Add(0x22E6, new CSMG_ABYSSTEAM_SET_OPEN_REQUEST());
            commandTable.Add(0x22E8, new Packet());
            commandTable.Add(0x22E9, new CSMG_ABYSSTEAM_SET_CREATE_REQUEST());
            commandTable.Add(0x22EC, new CSMG_ABYSSTEAM_REGIST_REQUEST());
            commandTable.Add(0x22EF, new CSMG_ABYSSTEAM_REGIST_APPROVAL());
            commandTable.Add(0x22F2, new CSMG_ABYSSTEAM_LEAVE_REQUEST());
            commandTable.Add(0x22F4, new CSMG_ABYSSTEAM_BREAK_REQUEST());

            //寵物保護
            commandTable.Add(0x235B, new CSMG_PPROTECT_LIST_OPEN());
            commandTable.Add(0x235E, new CSMG_PPROTECT_CREATED_INITI());
            commandTable.Add(0x2361, new CSMG_PPROTECT_CREATED_INFO());
            commandTable.Add(0x2363, new CSMG_PPROTECT_CREATED_REVISE());
            commandTable.Add(0x2365, new CSMG_PPROTECT_ADD_1());
            commandTable.Add(0x2367, new CSMG_PPROTECT_ADD());
            commandTable.Add(0x2369, new CSMG_PPROTECT_CREATED_OUT());
            commandTable.Add(0x236B, new CSMG_PPROTECT_CREATED_OUT());
            commandTable.Add(0x2374, new CSMG_PPROTECT_READY());

            //師徒
            commandTable.Add(0x1FE0, new CSMG_BOND_REQUEST_FROM_MASTER());
            commandTable.Add(0x1FE3, new CSMG_BOND_REQUEST_PUPILIN_ANSWER());
            commandTable.Add(0x1FE4, new CSMG_BOND_REQUEST_FROM_PUPILIN());
            commandTable.Add(0x1FE7, new CSMG_BOND_REQUEST_MASTER_ANSWER());
            commandTable.Add(0x1FE8, new CSMG_BOND_CANCEL());

            //角色設定
            commandTable.Add(0x1A5E, new CSMG_PLAYER_SETOPTION());

            //副职相关
            commandTable.Add(0x22CF, new CSMG_DUALJOB_CHANGE_CANCEL());
            commandTable.Add(0x22D0, new CSMG_DUALJOB_CHANGE_CONFIRM());
            //this.commandTable.Add(0x003c, new Packets.Client.CSMG_NO_NYASHIELD());//没装喵盾


            //Daily Stamp

            commandTable.Add(0x1F73, new CSMG_DAILY_STAMP_OPEN());
            commandTable.Add(0x1F74, new CSMG_DAILY_STAMP_OPEN());

            waitressQueue = new AutoResetEvent(true);

            //deadlock check
            check = new Thread(checkCriticalArea);
            check.Name = string.Format("DeadLock checker({0})", check.ManagedThreadId);
#if DeadLockCheck
            check.Start();
#endif
        }

        public static MapClientManager Instance => Nested.instance;

        public List<MapClient> Clients { get; }

        public List<MapClient> OnlinePlayer
        {
            get
            {
                var list = new List<MapClient>();
                foreach (var i in Clients)
                {
                    if (i.netIO.Disconnected)
                        continue;
                    if (i.Character == null)
                        continue;
                    if (!i.Character.Online)
                        continue;
                    list.Add(i);
                }

                return list;
            }
        }

        public List<MapClient> OnlinePlayerOnlyIP
        {
            get
            {
                var ips = new List<string>();
                var list = new List<MapClient>();
                foreach (var i in Clients)
                {
                    if (i.netIO.Disconnected)
                        continue;
                    if (i.Character == null)
                        continue;
                    if (!i.Character.Online)
                        continue;
                    if (!ips.Contains(i.Character.Account.LastIP))
                    {
                        ips.Add(i.Character.Account.LastIP);
                        list.Add(i);
                    }
                }

                return list;
            }
        }

        public void Abort()
        {
            check.Abort();
            packetCoordinator.Abort();
        }

        public void Announce(string txt)
        {
            try
            {
                foreach (var i in OnlinePlayer)
                    try
                    {
                        i.SendAnnounce(txt);
                    }
                    catch
                    {
                    }
            }
            catch
            {
            }
        }


        /// <summary>
        ///     Connects new clients
        /// </summary>
        public override void NetworkLoop(int maxNewConnections)
        {
            for (var i = 0; listener.Pending() && i < maxNewConnections; i++)
            {
                var sock = listener.AcceptSocket();
                var ip = sock.RemoteEndPoint.ToString().Substring(0, sock.RemoteEndPoint.ToString().IndexOf(':'));
                Logger.ShowInfo(string.Format(LocalManager.Instance.Strings.NEW_CLIENT, sock.RemoteEndPoint), null);
                var client = new MapClient(sock, commandTable);
                Clients.Add(client);
            }
        }

        public override void OnClientDisconnect(Client client_t)
        {
            Clients.Remove((MapClient)client_t);
        }

        public MapClient FindClient(ActorPC pc)
        {
            return FindClient(pc.CharID);
        }

        public override Client GetClient(uint actorID)
        {
            var chr = from c in OnlinePlayer
                where c.Character.ActorID == actorID
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        public override Client GetClientForName(string actorName)
        {
            var chr = from c in OnlinePlayer
                where c.Character.Name == actorName
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        public MapClient FindClient(uint charID)
        {
            var chr = from c in OnlinePlayer
                where c.Character.CharID == charID
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        private class Nested
        {
            internal static readonly MapClientManager instance = new MapClientManager();

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
        }
    }
}