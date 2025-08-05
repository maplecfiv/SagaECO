using System.Collections.Generic;

namespace SagaMap.Localization.Languages
{
    public class Japanese : Strings
    {
        public Japanese()
        {
            VISIT_OUR_HOMEPAGE = "SagaECO日本語バージョンの公式サイトは http://jp.sagaeco.com";

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

            ATCOMMAND_COMMANDLIST = "あなたが使えるコマンドは：";
            ATCOMMAND_MODE_PARA = "使用方法: !mode 1-2";
            ATCOMMAND_PK_MODE_INFO = "PK状態になりました";
            ATCOMMAND_NORMAL_MODE_INFO = "PK状態が解除されました";
            ATCOMMAND_WARP_PARA = "使用方法: !warp MapID x y";
            ATCOMMAND_NO_ACCESS = "このコマンドを実行する権限はありません！";
            ATCOMMAND_ITEM_PARA = "使用方法: !item アイテムID";
            ATCOMMAND_ITEM_NO_SUCH_ITEM = "このアイテムはありません！";
            ATCOMMAND_ANNOUNCE_PARA = "使用方法: !announce 内容";
            ATCOMMAND_HEAL_MESSAGE = "HP/MP/SP 全回復";
            ATCOMMAND_LEVEL_PARA = "使用方法: !level 数値";
            ATCOMMAND_GOLD_PARA = "使用方法: !gold 数量";
            ATCOMMAND_SHOPPOINT_PARA = "使用方法: !shoppoint 数量";
            ATCOMMAND_HAIR_PAEA = "使用方法: !hair Style数値 Wig数値 Color数値";
            ATCOMMAND_HAIR_ERROR = "変更できませんでした";
            ATCOMMAND_HAIRSTYLE_PARA = "使用方法: !hairstyle 1-15";
            ATCOMMAND_HAIRCOLOR_PARA = "使用方法: !haircolor 1-22";
            ATCOMMAND_HAIRCOLOR_ERROR = "髪色を変更できませんでした";
            ATCOMMAND_PLAYERSIZE_PARA = "使用方法: !playersize 数値 (標準:1000)";
            ATCOMMAND_SPAWN_PARA = "使用方法: !spawn MobID 数 範囲 時間";
            ATCOMMAND_SPAWN_SUCCESS = "Spawn:{0} amount:{1} range:{2} delay:{3} added";

            PLAYER_LOG_IN = "プレイヤー：{0} ログインしています";
            PLAYER_LOG_OUT = "プレイヤー：{0} ログアウトしています";
            CLIENT_CONNECTING = "クライアント(バージョン:{0}) 接続中...";
            NEW_CLIENT = "新しいクライアント： {0}";
            INITIALIZATION = "初期化を始めます……";
            ACCEPTING_CLIENT = "接続開始します。";
            ATCOMMAND_KICK_PARA = "使用方法: !kick キャラ名";
            ATCOMMAND_KICKALL_PARA = "使用方法: !kickall";
            ATCOMMAND_SPEED_PARA = "使用方法: !speed 数値 (標準速度:420)";
            ATCOMMAND_JUMP_PARA = "使用方法: !jump キャラ名";
            ATCOMMAND_HAIREXT_PARA = "使用方法: !hairext 1-52";
            ITEM_ADDED = "{0}を{1}個入手しました";
            ITEM_DELETED = "{0}を{1}個失いました";

            ITEM_WARE_GET = "[{0}]を{1}個取り出しました";
            ITEM_WARE_PUT = "[{0}]を{1}個預けました";

            GET_EXP = "基本経験値 {0}、職業経験値 {1}を取得しました";
            ATCOMMAND_ONLINE_PLAYER_INFO = "当オンラインのプレイヤー:";

            NPC_INPUT_BANK = "金額を入力 現在{0}Gold";
            NPC_BANK_NOT_ENOUGH_GOLD = "所持金が足りません";
            NPC_EventID_NotFound = "ＮＰＣ未実装!$RＳｃｒｉｐｔ(Eventid={0})がありません";
            NPC_EventID_NotFound_Msg = "...";
            ATCOMMAND_MOB_ERROR = "そのMobはありません!";
            ATCOMMAND_WARP_ERROR = "そのMapIDはありません";

            PET_FRIENDLY_DOWN = "{0}の新密度が減りました";

            POSSESSION_EXP = "(憑依経験値)基本経験値 {0}、職業経験値 {1}を取得しました";
            POSSESSION_DONE = "{0}に憑依しました";
            POSSESSION_RIGHT = "右手";
            POSSESSION_LEFT = "左手";
            POSSESSION_NECK = "胸アクセサリー";
            POSSESSION_ARMOR = "鎧";

            QUEST_HOW_TO_DO = "どの仕事にしますか？";
            QUEST_NOT_CANCEL = "取り消しません";
            QUEST_CANCEL = "クエストを取り消します";
            QUEST_CANCELED = "クエストをキャンセルした……。$R;";
            QUEST_REWARDED = "報酬を受け取りました！$R;";
            QUEST_FAILED = "クエスト失敗……$R;";
            QUEST_IF_TAKE_QUEST = "仕事を受ける？";
            QUEST_TAKE = "はい";
            QUEST_NOT_TAKE = "いいえ";
            QUEST_TRANSPORT_GET = "アイテムを渡します";
            QUEST_TRANSPORT_GIVE = "アイテムを渡しました";

            PARTY_NEW_NAME = "新しいパーティー";

            SKILL_ACTOR_DELETE = "{0}が消滅しました";
            SKILL_STATUS_ENTER = "{0}状態になりました";
            SKILL_STATUS_LEAVE = "{0}状態が解除されました";
            SKILL_DECOY = "分身：";

            ITEM_TREASURE_OPEN = "開けたい物を選んで下さい";
            ITEM_TREASURE_NO_NEED = "開錠出来るアイテムがありません";
            ITEM_IDENTIFY = "鑑定したい物を選んで下さい";
            ITEM_IDENTIFY_NO_NEED = "鑑定するアイテムがありません";
            ITEM_IDENTIFY_RESULT = "鑑定結果: {0} -> {1}";
            ITEM_UNIDENTIFIED_NONE = "雑貨";
            ITEM_UNIDENTIFIED_HELM = "かぶる物";
            ITEM_UNIDENTIFIED_ACCE_HEAD = "装飾品";
            ITEM_UNIDENTIFIED_ACCE_FACE0 = "装飾品";
            ITEM_UNIDENTIFIED_ACCE_FACE1 = "装飾品";
            ITEM_UNIDENTIFIED_ACCE_FACE2 = "装飾品";
            ITEM_UNIDENTIFIED_ACCE_NECK = "装飾品";
            ITEM_UNIDENTIFIED_ACCE_FINGER = "装飾品";
            ITEM_UNIDENTIFIED_ARMOR_UPPER = "着る物";
            ITEM_UNIDENTIFIED_ARMOR_LOWER = "着る物";
            ITEM_UNIDENTIFIED_ONEPIECE = "着る物";
            ITEM_UNIDENTIFIED_COSTUME = "着る物";
            ITEM_UNIDENTIFIED_OVERALLS = "着る物";
            ITEM_UNIDENTIFIED_BODYSUIT = "着る物";
            ITEM_UNIDENTIFIED_FACEBODYSUIT = "着る物";
            ITEM_UNIDENTIFIED_BACKPACK = "背負い袋";
            ITEM_UNIDENTIFIED_COAT = "着る物";
            ITEM_UNIDENTIFIED_SOCKS = "靴下";
            ITEM_UNIDENTIFIED_BOOTS = "靴";
            ITEM_UNIDENTIFIED_SLACKS = "着る物";
            ITEM_UNIDENTIFIED_LONGBOOTS = "靴";
            ITEM_UNIDENTIFIED_HALFBOOTS = "靴";
            ITEM_UNIDENTIFIED_FULLFACE = "かぶる物";
            ITEM_UNIDENTIFIED_SHORT_SWORD = "短剣";
            ITEM_UNIDENTIFIED_SWORD = "剣";
            ITEM_UNIDENTIFIED_RAPIER = "細剣";
            ITEM_UNIDENTIFIED_CLAW = "爪";
            ITEM_UNIDENTIFIED_KNUCKLE = "槌";
            ITEM_UNIDENTIFIED_SHIELD = "盾";
            ITEM_UNIDENTIFIED_HAMMER = "鈍器";
            ITEM_UNIDENTIFIED_AXE = "斧";
            ITEM_UNIDENTIFIED_SPEAR = "槍";
            ITEM_UNIDENTIFIED_STAFF = "杖";
            ITEM_UNIDENTIFIED_THROW = "投げる物";
            ITEM_UNIDENTIFIED_BOW = "弓";
            ITEM_UNIDENTIFIED_ARROW = "矢";
            ITEM_UNIDENTIFIED_GUN = "銃";
            ITEM_UNIDENTIFIED_BULLET = "弾";
            ITEM_UNIDENTIFIED_HANDBAG = "手提げ袋";
            ITEM_UNIDENTIFIED_LEFT_HANDBAG = "手提げ袋";
            ITEM_UNIDENTIFIED_BOOK = "本";
            ITEM_UNIDENTIFIED_INSTRUMENT = "楽器";
            ITEM_UNIDENTIFIED_ROPE = "鞭";
            ITEM_UNIDENTIFIED_CARD = "投げる物";
            ITEM_UNIDENTIFIED_ETC_WEAPON = "???";
            ITEM_UNIDENTIFIED_SHOES = "靴";
            ITEM_UNIDENTIFIED_MONEY = "お金";
            ITEM_UNIDENTIFIED_FOOD = "食物";
            ITEM_UNIDENTIFIED_POTION = "飲み物";
            ITEM_UNIDENTIFIED_MARIONETTE = "マリオネット";
            ITEM_UNIDENTIFIED_GOLEM = "ゴーレム";
            ITEM_UNIDENTIFIED_TREASURE_BOX = "宝箱";
            ITEM_UNIDENTIFIED_CONTAINER = "コンテナ";
            ITEM_UNIDENTIFIED_TIMBER_BOX = "木箱";
            ITEM_UNIDENTIFIED_SEED = "種";
            ITEM_UNIDENTIFIED_SCROLL = "スクロール";
            ITEM_UNIDENTIFIED_SKILLBOOK = "スキル";
            ITEM_UNIDENTIFIED_PET = "ペット";
            ITEM_UNIDENTIFIED_PET_NEKOMATA = "背負い魔";
            ITEM_UNIDENTIFIED_PET_YOUHEI = "衛兵";
            ITEM_UNIDENTIFIED_BACK_DEMON = "背負い魔";
            ITEM_UNIDENTIFIED_RIDE_PET = "乗り物";
            ITEM_UNIDENTIFIED_USE = "道具";
            ITEM_UNIDENTIFIED_PETFOOD = "ペットフード";
            ITEM_UNIDENTIFIED_STAMP = "スタンプ";
            ITEM_UNIDENTIFIED_FG_FURNITURE = "家具";
            ITEM_UNIDENTIFIED_FG_BASEBUILD = "家";
            ITEM_UNIDENTIFIED_FG_ROOM_WALL = "壁紙";
            ITEM_UNIDENTIFIED_FG_ROOM_FLOOR = "床";
            ITEM_UNIDENTIFIED_ITDGN = "変なもの";
            ITEM_UNIDENTIFIED_ROBOT_GROW = "ロボット部品";

            FG_NAME = "{0}さんの飛空庭";
            FG_NOT_FOUND = "飛空庭がありません";
            FG_ALREADY_CALLED = "既に飛空庭を呼び出しています";
            FG_CANNOT = "ここでは飛空庭を呼び出せません";
            FG_FUTNITURE_SETUP = "{0}を設置しました ({1}/{2}個)";
            FG_FUTNITURE_REMOVE = "{0}を撤去しました ({1}/{2}個)";
            FG_FUTNITURE_MAX = "設置最大数を超えています";

            ITD_HOUR = "時間";
            ITD_MINUTE = "分";
            ITD_SECOND = "秒";
            ITD_CRASHING = "あと {0} 秒後に崩壊します";
            ITD_CREATED = "さんのダンジョンが作成されました";
            ITD_PARTY_DISMISSED = "パーティが解散されました";
            ITD_QUEST_CANCEL = "ダンジョン作成者がクエストをキャンセルしました";
            ITD_SELECT_DUUNGEON = "どのダンジョンに行きますか？";
            ITD_DUNGEON_NAME = " さんのダンジョン";

            THEATER_WELCOME = "シアタースクリーンへようこそ！";
            THEATER_COUNTDOWN = "{0} は {1} 分後に始まります";

            NPC_SHOP_CP_GET = "{0} CP を入手しました";
            NPC_SHOP_ECOIN_GET = "{0} ecoin を入手しました";
            NPC_SHOP_CP_LOST = "{0} CP を失いました";
            NPC_SHOP_ECOIN_LOST = "{0} ecoin を失いました";

            WRP_ENTER = "チャンプバトル参戦中";
            WRP_GOT = "WRP {0}を取得しました";
            WRP_LOST = "WRP {0}失いました";
            DEATH_PENALTY = "デスペナルティーにより経験値が奪われました";

            ODWAR_PREPARE = "DEM正在向{0}进军，预计{1}分钟后到达";
            ODWAR_PREPARE2 = "请有能力的勇者们前往支援！";
            ODWAR_START = "都市攻防戦が開始されました！";
            ODWAR_SYMBOL_DOWN = "シンボル·{0}号機が破壊されました";
            ODWAR_SYMBOL_ACTIVATE = "シンボル·{0}号機が起動しました";
            ODWAR_LOSE = "西部要塞城被DEM攻陷了！！";
            ODWAR_WIN = "敵が撤退するぞ！我々の勝利だ！！";
            ODWAR_WIN2 = "西部要塞的象征开始展开防御力场！";
            ODWAR_WIN3 = "敵軍がウェストフォートから撤退しました";
            ODWAR_WIN4 = "敵が撤退するぞ！我々の勝利だ！！";
            ODWAR_CAPTURE = "勇者们成功地夺回了西部要塞城！！";

            EP_INCREASE = "あと、{0}時間後にEPが増加します";
            EP_INCREASED = "EP{0}ポイント増加しました";

            NPC_ITEM_FUSION_RECHOOSE = "選びなおす！";
            NPC_ITEM_FUSION_CANCEL = "やっぱりやーめた";
            NPC_ITEM_FUSION_CONFIRM = "成功率{1}％ {0}G";
        }

        public override string EnglishName => "Japanese";

        public override string LocalName => "日本語";
    }
}