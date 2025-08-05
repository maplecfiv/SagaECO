using System.Collections.Generic;

namespace SagaMap.Localization.Languages
{
    public class TChinese : Strings
    {
        public TChinese()
        {
            VISIT_OUR_HOMEPAGE = "請訪問糢擬器SagaECO的主頁：http://www.sagaeco.com";

            ATCOMMAND_DESC = new Dictionary<string, string>();
            ATCOMMAND_DESC.Add("/who", "显示当前在线玩家数量");
            ATCOMMAND_DESC.Add("/motion", "做指定动作表情");
            ATCOMMAND_DESC.Add("/vcashshop", "打开道具商城");
            ATCOMMAND_DESC.Add("/user", "显示当前在线玩家名字以及地图名");
            ATCOMMAND_DESC.Add("/commandlist", "显示所有当前玩家能用的命令列表");
            ATCOMMAND_DESC.Add("!warp", "瞬移到指定地图的指定坐标");
            ATCOMMAND_DESC.Add("!announce", "发布全服公告");
            ATCOMMAND_DESC.Add("!heal", "瞬间恢复当前HP/MP/SP");
            ATCOMMAND_DESC.Add("!level", "调整人物等级到指定数值");
            ATCOMMAND_DESC.Add("!gold", "调整当前玩家的现金");
            ATCOMMAND_DESC.Add("!shoppoint", "调整当前帐号的商城点数");
            ATCOMMAND_DESC.Add("!hairstyle", "调整玩家发型");
            ATCOMMAND_DESC.Add("!haircolor", "调整玩家发色");
            ATCOMMAND_DESC.Add("!job", "调整玩家职业");
            ATCOMMAND_DESC.Add("!joblevel", "调整人物职业等级到指定数值");
            ATCOMMAND_DESC.Add("!statpoints", "调整玩家剩余属性点数到指定数值");
            ATCOMMAND_DESC.Add("!skillpoints", "调整玩家剩余技能点，!skillpoints [job] 数值，job有1，2－1，2－2");
            ATCOMMAND_DESC.Add("!event", "调用指定EventID的脚本");
            ATCOMMAND_DESC.Add("!hairext", "调整当前人物的附加头发");
            ATCOMMAND_DESC.Add("!playersize", "调整当前人物的人物大小");
            ATCOMMAND_DESC.Add("!item", "制造指定数量的指定道具");
            ATCOMMAND_DESC.Add("!speed", "调整当前人物移动速度");
            ATCOMMAND_DESC.Add("!revive", "复活当前人物");
            ATCOMMAND_DESC.Add("!kick", "踢掉指定玩家");
            ATCOMMAND_DESC.Add("!kickall", "踢掉所有在线玩家");
            ATCOMMAND_DESC.Add("!jump", "瞬移到指定玩家处");
            ATCOMMAND_DESC.Add("!recall", "瞬移指定玩家到此玩家处");
            ATCOMMAND_DESC.Add("!mob", "刷出指定数量指定怪物");
            ATCOMMAND_DESC.Add("!summon", "召唤某怪物为宠物");
            ATCOMMAND_DESC.Add("!summonme", "召唤自己为宠物");
            ATCOMMAND_DESC.Add("!spawn", "制作刷怪点");
            ATCOMMAND_DESC.Add("!effect", "显示指定特效");
            ATCOMMAND_DESC.Add("!skill", "习得某技能");
            ATCOMMAND_DESC.Add("!skillclear", "忘记所有技能");
            ATCOMMAND_DESC.Add("!who", "显示当前在线玩家的名字");
            ATCOMMAND_DESC.Add("!who2", "显示当前在线玩家的名字，所在地图以及坐标");
            ATCOMMAND_DESC.Add("!go", "快速移动到某城市");
            ATCOMMAND_DESC.Add("!info", "显示当前地图单元的元素信息");
            ATCOMMAND_DESC.Add("!cash", "调整当前人物的现金");
            ATCOMMAND_DESC.Add("!reloadscript", "重新读取脚本");
            ATCOMMAND_DESC.Add("!reloadconfig", "重新读取某设定(ECOShop,ShopDB,monster,Quests,Treasure,Theater)");
            ATCOMMAND_DESC.Add("!raw", "发送封包");

            ATCOMMAND_COMMANDLIST = "您能使用的命令有：";
            ATCOMMAND_MODE_PARA = "用法: !mode 1-2";
            ATCOMMAND_PK_MODE_INFO = "PK模式已開啟";
            ATCOMMAND_NORMAL_MODE_INFO = "PK模式已關閉";
            ATCOMMAND_WARP_PARA = "用法: !warp 地圖ID x y";
            ATCOMMAND_NO_ACCESS = "你沒有訪問這條命令的權限！";
            ATCOMMAND_ITEM_PARA = "用法: !item 物品ID";
            ATCOMMAND_ITEM_NO_SUCH_ITEM = "沒有這件物品！";
            ATCOMMAND_ANNOUNCE_PARA = "用法: !announce 內容";
            ATCOMMAND_HEAL_MESSAGE = "HP/MP/SP 全滿";
            ATCOMMAND_LEVEL_PARA = "用法: !level 等級";
            ATCOMMAND_GOLD_PARA = "用法: !gold 數量";
            ATCOMMAND_SHOPPOINT_PARA = "用法: !shoppoint 數量";
            ATCOMMAND_HAIR_PAEA = "用法: !hair Style數值 Wig數值 Color數值";
            ATCOMMAND_HAIR_ERROR = "當前髮型無法更換顏色";
            ATCOMMAND_HAIRSTYLE_PARA = "用法: !hairstyle 1-15";
            ATCOMMAND_HAIRCOLOR_PARA = "用法: !haircolor 1-22";
            ATCOMMAND_HAIRCOLOR_ERROR = "當前髮型無法更換顏色";
            ATCOMMAND_PLAYERSIZE_PARA = "用法: !playersize 數值 (默認大小:1000)";
            ATCOMMAND_SPAWN_PARA = "用法: !spawn 怪物ID 數量 範圍 刷怪時間";
            ATCOMMAND_SPAWN_SUCCESS = "Spawn:{0} amount:{1} range:{2} delay:{3} added";

            PLAYER_LOG_IN = "玩家：{0} 已經登錄";
            PLAYER_LOG_OUT = "玩家：{0} 已登出";
            CLIENT_CONNECTING = "客户端(版本號:{0}) 正在嘗試連接中...";
            NEW_CLIENT = "新客户端： {0}";
            INITIALIZATION = "開啟初始化……";
            ACCEPTING_CLIENT = "開始接受客户端。";
            ATCOMMAND_KICK_PARA = "用法: !kick 名字";
            ATCOMMAND_KICKALL_PARA = "用法: !kickall";
            ATCOMMAND_SPEED_PARA = "用法: !speed 數值 (默認速度:420)";
            ATCOMMAND_JUMP_PARA = "用法: !jump 玩家名字";
            ATCOMMAND_HAIREXT_PARA = "用法: !hairext 1-52";

            ITEM_ADDED = "得到{1}個[{0}]";
            ITEM_DELETED = "失去{1}個[{0}]";
            ITEM_WARE_GET = "取出[{0}] {1}個";
            ITEM_WARE_PUT = "存放[{0}] {1}個";
            GET_EXP = "得到基本經驗值 {0}  職業經驗值 {1}";
            ATCOMMAND_ONLINE_PLAYER_INFO = "當前在線玩家:";
            NPC_EventID_NotFound = "NPC未實裝!$R無法找到腳本(Eventid={0})";
            NPC_EventID_NotFound_Msg = "..噓..別嘈";
            ATCOMMAND_MOB_ERROR = "你的召喚怪物命令使用錯誤!";
            ATCOMMAND_WARP_ERROR = "無法轉移到指定地圖";

            PET_FRIENDLY_DOWN = "{0}的親密度減少了";

            POSSESSION_EXP = "得到了(憑依)基本經驗值 {0}  職業經驗值 {1} ";
            POSSESSION_DONE = "憑依在[{0}]了";
            POSSESSION_RIGHT = "右手";
            POSSESSION_LEFT = "左手";
            POSSESSION_NECK = "項鏈";
            POSSESSION_ARMOR = "盔甲";

            NPC_INPUT_BANK = "輸入金額 目前有{0}個金幣";
            NPC_BANK_NOT_ENOUGH_GOLD = "所持金額不足！";

            QUEST_HOW_TO_DO = "怎麼做好呢？";
            QUEST_NOT_CANCEL = "什麼也不取消";
            QUEST_CANCEL = "取消任務";
            QUEST_CANCELED = "任務取消了……$R;";
            QUEST_REWARDED = "收到了報酬！$R;";
            QUEST_FAILED = "任務失敗了……$R;";
            QUEST_IF_TAKE_QUEST = "要接受委託嗎";
            QUEST_TAKE = "接受";
            QUEST_NOT_TAKE = "不接受";
            QUEST_TRANSPORT_GET = "收到行李";
            QUEST_TRANSPORT_GIVE = "東西已經轉交了";

            PARTY_NEW_NAME = "新的隊伍";

            SKILL_ACTOR_DELETE = "[{0}] 消滅了!!!";
            SKILL_STATUS_ENTER = "成了{0}狀態";
            SKILL_STATUS_LEAVE = "解除了{0}狀態";
            SKILL_DECOY = "分身：";

            ITEM_TREASURE_OPEN = "請選擇要開封的箱子";
            ITEM_TREASURE_NO_NEED = "没有需要打开的箱子";
            ITEM_IDENTIFY = "請選擇要鑑定的物品";
            ITEM_IDENTIFY_NO_NEED = "没有需要鑑定的物品";
            ITEM_IDENTIFY_RESULT = "鑑定結果: {0} -> {1}";
            ITEM_UNIDENTIFIED_NONE = "雜貨";
            ITEM_UNIDENTIFIED_HELM = "頭盔";
            ITEM_UNIDENTIFIED_ACCE_HEAD = "頭飾";
            ITEM_UNIDENTIFIED_ACCE_FACE0 = "臉部裝飾 1";
            ITEM_UNIDENTIFIED_ACCE_FACE1 = "臉部裝飾 2";
            ITEM_UNIDENTIFIED_ACCE_FACE2 = "臉部裝飾 3";
            ITEM_UNIDENTIFIED_ACCE_NECK = "項鏈";
            ITEM_UNIDENTIFIED_ACCE_FINGER = "戒指";
            ITEM_UNIDENTIFIED_ARMOR_UPPER = "上身防具";
            ITEM_UNIDENTIFIED_ARMOR_LOWER = "下身防具";
            ITEM_UNIDENTIFIED_ONEPIECE = "連身裙";
            ITEM_UNIDENTIFIED_OVERALLS = "工作褲";
            ITEM_UNIDENTIFIED_BODYSUIT = "全身套裝";
            ITEM_UNIDENTIFIED_FACEBODYSUIT = "全身套裝";
            ITEM_UNIDENTIFIED_BACKPACK = "背囊";
            ITEM_UNIDENTIFIED_COAT = "外套";
            ITEM_UNIDENTIFIED_SOCKS = "襪子";
            ITEM_UNIDENTIFIED_BOOTS = "靴子";
            ITEM_UNIDENTIFIED_SLACKS = "下衣";
            ITEM_UNIDENTIFIED_LONGBOOTS = "長靴";
            ITEM_UNIDENTIFIED_HALFBOOTS = "半長靴";
            ITEM_UNIDENTIFIED_FULLFACE = "面具";
            ITEM_UNIDENTIFIED_SHORT_SWORD = "短刀";
            ITEM_UNIDENTIFIED_SWORD = "劍";
            ITEM_UNIDENTIFIED_RAPIER = "幼長劍";
            ITEM_UNIDENTIFIED_CLAW = "棒子";
            ITEM_UNIDENTIFIED_KNUCKLE = "關節";
            ITEM_UNIDENTIFIED_SHIELD = "盾牌";
            ITEM_UNIDENTIFIED_HAMMER = "鐵鎚";
            ITEM_UNIDENTIFIED_AXE = "斧頭";
            ITEM_UNIDENTIFIED_SPEAR = "槍";
            ITEM_UNIDENTIFIED_STAFF = "魔杖";
            ITEM_UNIDENTIFIED_THROW = "投擲物";
            ITEM_UNIDENTIFIED_BOW = "弓";
            ITEM_UNIDENTIFIED_ARROW = "箭";
            ITEM_UNIDENTIFIED_GUN = "槍";
            ITEM_UNIDENTIFIED_BULLET = "子彈";
            ITEM_UNIDENTIFIED_HANDBAG = "手提包";
            ITEM_UNIDENTIFIED_LEFT_HANDBAG = "手提包(左)";
            ITEM_UNIDENTIFIED_BOOK = "書";
            ITEM_UNIDENTIFIED_INSTRUMENT = "樂器";
            ITEM_UNIDENTIFIED_ROPE = "繩子";
            ITEM_UNIDENTIFIED_CARD = "卡";
            ITEM_UNIDENTIFIED_ETC_WEAPON = "???";
            ITEM_UNIDENTIFIED_SHOES = "鞋";
            ITEM_UNIDENTIFIED_MONEY = "金幣";
            ITEM_UNIDENTIFIED_FOOD = "食物";
            ITEM_UNIDENTIFIED_POTION = "藥水";
            ITEM_UNIDENTIFIED_MARIONETTE = "活動木偶";
            ITEM_UNIDENTIFIED_GOLEM = "石像";
            ITEM_UNIDENTIFIED_TREASURE_BOX = "寶物箱";
            ITEM_UNIDENTIFIED_CONTAINER = "集裝箱";
            ITEM_UNIDENTIFIED_TIMBER_BOX = "木箱";
            ITEM_UNIDENTIFIED_SEED = "種子";
            ITEM_UNIDENTIFIED_SCROLL = "捲軸";
            ITEM_UNIDENTIFIED_SKILLBOOK = "技能書";
            ITEM_UNIDENTIFIED_PET = "寵物";
            ITEM_UNIDENTIFIED_PET_NEKOMATA = "凱堤";
            ITEM_UNIDENTIFIED_PET_YOUHEI = "傭兵";
            ITEM_UNIDENTIFIED_BACK_DEMON = "凱堤";
            ITEM_UNIDENTIFIED_RIDE_PET = "騎乘寵物";
            ITEM_UNIDENTIFIED_USE = "道具";
            ITEM_UNIDENTIFIED_PETFOOD = "寵物食物";
            ITEM_UNIDENTIFIED_STAMP = "印章";
            ITEM_UNIDENTIFIED_FG_FURNITURE = "傢俱";
            ITEM_UNIDENTIFIED_FG_BASEBUILD = "部件";
            ITEM_UNIDENTIFIED_FG_ROOM_WALL = "牆紙";
            ITEM_UNIDENTIFIED_FG_ROOM_FLOOR = "地板";
            ITEM_UNIDENTIFIED_ITDGN = "不知名的東西";
            ITEM_UNIDENTIFIED_ROBOT_GROW = "強化部件";
            ITEM_UNIDENTIFIED_COSTUME = "特殊服裝";


            FG_NAME = "這是{0}的飛空庭";
            FG_NOT_FOUND = "沒有飛空庭";
            FG_ALREADY_CALLED = "已經召喚了飛空庭";
            FG_CANNOT = "在這裡無法召喚飛空庭";
            FG_FUTNITURE_SETUP = "{0} 已放置 ({1}/{2}个)";
            FG_FUTNITURE_REMOVE = "{0} 已拆除 ({1}/{2}个)";
            FG_FUTNITURE_MAX = "不能再設置道具";

            ITD_HOUR = "小時";
            ITD_MINUTE = "分鍾";
            ITD_SECOND = "秒";
            ITD_CRASHING = "這個地牢 {0} 後會崩潰";
            ITD_CREATED = "的地牢形成了";
            ITD_PARTY_DISMISSED = "隊伍解散了，這個地牢會消失";
            ITD_QUEST_CANCEL = "製作者取消了任務，這個地牢會消失";
            ITD_SELECT_DUUNGEON = "請選擇要進去的地牢";
            ITD_DUNGEON_NAME = " 的地牢";

            THEATER_WELCOME = "歡迎光臨電影院！";
            THEATER_COUNTDOWN = "{0} 將于 {1} 分鐘后開始播放";

            NPC_SHOP_CP_GET = "得到 {0} CP.";
            NPC_SHOP_ECOIN_GET = "得到 {0} ecoin";
            NPC_SHOP_CP_LOST = "失去 {0} CP";
            NPC_SHOP_ECOIN_LOST = "失去 {0} ecoin";

            WRP_ENTER = "冠軍戰場參戰中";
            WRP_GOT = "取得 {0}點WRP";
            WRP_LOST = "失去 {0}點WRP";
            DEATH_PENALTY = "由于死亡懲罰經驗值等降低了";

            ODWAR_PREPARE = "DEM正在向{0}进军，预计{1}分钟后到达";
            ODWAR_PREPARE2 = "请有能力的勇者们前往支援！";
            ODWAR_START = "都市防御战开始了！";
            ODWAR_SYMBOL_DOWN = "象征·{0}号机被破坏！！！";
            ODWAR_SYMBOL_ACTIVATE = "象征·{0}号机成功地展开了！！！";
            ODWAR_LOSE = "西部要塞城被DEM攻陷了！！";
            ODWAR_WIN = "西部要塞的防御战胜利了！";
            ODWAR_WIN2 = "西部要塞的象征开始展开防御力场！";
            ODWAR_WIN3 = "敌军开始从西部要塞撤退！";
            ODWAR_WIN4 = "敌军撤退了！我们胜利了！";
            ODWAR_CAPTURE = "勇者们成功地夺回了西部要塞城！！";

            EP_INCREASE = "离增加EP還有{0}小時";
            EP_INCREASED = "EP增加了{0}点";

            NPC_ITEM_FUSION_RECHOOSE = "重新選！";
            NPC_ITEM_FUSION_CANCEL = "還是算了";
            NPC_ITEM_FUSION_CONFIRM = "成功率{1}％ {0}G";
        }

        public override string EnglishName => "TChinese";

        public override string LocalName => "繁體中文";
    }
}