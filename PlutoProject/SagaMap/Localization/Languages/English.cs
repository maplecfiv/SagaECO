//#define Thai

using System.Collections.Generic;

namespace SagaMap.Localization.Languages
{
    public class English : Strings
    {
        public English()
        {
#if Thai
            this.VISIT_OUR_HOMEPAGE = "ยินดีต้อนรับสู่ ECO-THAI ( www.eco-thai.com)";
            //this.VISIT_OUR_HOMEPAGE = "ยินดีต้อนรับสู่ ECO-WASABI ( www.ecowasabi.com)";
            //this.VISIT_OUR_HOMEPAGE = " ยินดีต้อนรับสู่ ECO-KITTY (www.eco-kitty.net)";
#else
            VISIT_OUR_HOMEPAGE = "Plese visit this Emulator,SagaECO's homepage: http://www.sagaeco.com";
#endif
            ATCOMMAND_DESC = new Dictionary<string, string>();
            ATCOMMAND_DESC.Add("/who", "Displays the current number of players online");
            ATCOMMAND_DESC.Add("/motion", "To do routine expressions");
            ATCOMMAND_DESC.Add("/vcashshop", "Opens the Mall Shop");
            ATCOMMAND_DESC.Add("/user", "Displays the current online players, names and map names");
            ATCOMMAND_DESC.Add("/commandlist", "Shows all the available commands");
            ATCOMMAND_DESC.Add("!warp", "Teleports to the specified map and coordinates");
            ATCOMMAND_DESC.Add("!announce", "Announces a global message");
            ATCOMMAND_DESC.Add("!heal", "Recover HP/MP/SP");
            ATCOMMAND_DESC.Add("!level", "Adjust the level to a specified value");
            ATCOMMAND_DESC.Add("!gold", "Adjust the player's gold");
            ATCOMMAND_DESC.Add("!shoppoint", "Adjust the player's mall points");
            ATCOMMAND_DESC.Add("!hairstyle", "Adjust the players hairstyle");
            ATCOMMAND_DESC.Add("!haircolor", "Adjust the players haircolor");
            ATCOMMAND_DESC.Add("!job", "Change the players job profession");
            ATCOMMAND_DESC.Add("!joblevel", "Adjust the job level to a specified value");
            ATCOMMAND_DESC.Add("!statpoints", "Adjust the player stat points to the specified value");
            ATCOMMAND_DESC.Add("!skillpoints",
                "Adjust the player skill points to the specified value，!skillpoints [job] value，job has 1,2-1,2-2");
            ATCOMMAND_DESC.Add("!event", "Calls the specified EventID script");
            ATCOMMAND_DESC.Add("!hairext", "Adjust the current players attached hair");
            ATCOMMAND_DESC.Add("!playersize", "Adjust the current players size");
            ATCOMMAND_DESC.Add("!item", "Summons a specifed item");
            ATCOMMAND_DESC.Add("!speed", "Adjust the players speed");
            ATCOMMAND_DESC.Add("!revive", "Resurrect the player");
            ATCOMMAND_DESC.Add("!kick", "Kick away the designated player");
            ATCOMMAND_DESC.Add("!kickall", "Kicked away all online players");
            ATCOMMAND_DESC.Add("!jump", "Teleport to a specific player");
            ATCOMMAND_DESC.Add("!recall", "Call a specific player");
            ATCOMMAND_DESC.Add("!mob", "Brush out the specified number of the designated monster");
            ATCOMMAND_DESC.Add("!summon", "Summon a monster as a pet");
            ATCOMMAND_DESC.Add("!summonme", "Summon yourself as a pet");
            ATCOMMAND_DESC.Add("!spawn", "Strange point of making brushes");
            ATCOMMAND_DESC.Add("!effect", "Show the specified effect");
            ATCOMMAND_DESC.Add("!skill", "Acquisition of a skill");
            ATCOMMAND_DESC.Add("!skillclear", "Clear all the skills");
            ATCOMMAND_DESC.Add("!who", "Shows the number of current online players");
            ATCOMMAND_DESC.Add("!who2", "Shows the number and name of the current online players");
            ATCOMMAND_DESC.Add("!go", "Teleport to a city");
            ATCOMMAND_DESC.Add("!info", "Displays the current map element nformation");
            ATCOMMAND_DESC.Add("!cash", "Adjust the players cash");
            ATCOMMAND_DESC.Add("!reloadscript", "Reload the scripts");
            ATCOMMAND_DESC.Add("!reloadconfig", "Reload the configs(ECOShop,ShopDB,monster,Quests,Treasure,Theater)");
            ATCOMMAND_DESC.Add("!raw", "Send Packet");


            ATCOMMAND_COMMANDLIST = "Usage： /commandlist";
            ATCOMMAND_MODE_PARA = "Usage: !mode 1-2";
            ATCOMMAND_PK_MODE_INFO = "PK Mode:On";
            ATCOMMAND_NORMAL_MODE_INFO = "PK Mode:Off";
            ATCOMMAND_WARP_PARA = "Usage: !warp mapID x y";
            ATCOMMAND_NO_ACCESS = "You don't have access to this command!";
            ATCOMMAND_ITEM_PARA = "Usage: !item itemID";
            ATCOMMAND_ITEM_NO_SUCH_ITEM = "No such item!";
            ATCOMMAND_ANNOUNCE_PARA = "Usage: !announce xxoo";
            ATCOMMAND_HEAL_MESSAGE = "HP/MP/SP recovered";
            ATCOMMAND_LEVEL_PARA = "Usage: !level xxoo";
            ATCOMMAND_GOLD_PARA = "Usage: !gold xxoo";
            ATCOMMAND_SHOPPOINT_PARA = "Usage: !shoppoint xxoo";
            ATCOMMAND_HAIR_PAEA = "Use: !hair StyleByte WigByte ColorByte";
            ATCOMMAND_HAIR_ERROR = "Cannot change color for current hair style";
            ATCOMMAND_HAIRSTYLE_PARA = "Usage: !hairstyle 1-15";
            ATCOMMAND_HAIRCOLOR_PARA = "Usage: !haircolor 1-22";
            ATCOMMAND_HAIRCOLOR_ERROR = "Cannot change color for current hair style";
            ATCOMMAND_PLAYERSIZE_PARA = "Usage: !playersize size (Defaultsize:1000)";
            ATCOMMAND_SPAWN_PARA = "Usage: !spawn mobid amount range delay";
            ATCOMMAND_SPAWN_SUCCESS = "Spawn:{0} amount:{1} range:{2} delay:{3} added";

            PLAYER_LOG_IN = "Player:{0} logged in.";
            PLAYER_LOG_OUT = "Player:{0} logged out.";
            CLIENT_CONNECTING = "Client(Version:{0}) is trying to connect...";
            NEW_CLIENT = "New client from: {0}";
            INITIALIZATION = "Starting Initialization...";
            ACCEPTING_CLIENT = "Accepting clients.";
            ATCOMMAND_KICK_PARA = "Usage: !kick name";
            ATCOMMAND_KICKALL_PARA = "Usage: !kickall";
            ATCOMMAND_SPEED_PARA = "Usage: !speed xxoo (Defaultspeed:420)";
            ATCOMMAND_JUMP_PARA = "Usage: !jump playername";
            ATCOMMAND_HAIREXT_PARA = "Usage: !hairext 1-52";
            ITEM_ADDED = "Got {1} [{0}]";
            ITEM_DELETED = "Lost {1} [{0}]";
            ITEM_WARE_GET = "Get {1} [{0}] from ware house";
            ITEM_WARE_PUT = "Put {1} [{0}] into ware house";

            GET_EXP = "Get BaseEXP {0}  JobEXP {1}";
            ATCOMMAND_ONLINE_PLAYER_INFO = "Online Player:";

            NPC_EventID_NotFound = "EVENTID {0} not found";
            NPC_EventID_NotFound_Msg = "...";
            NPC_INPUT_BANK = "Enter amount. Current balance:{0}";
            NPC_BANK_NOT_ENOUGH_GOLD = "Not Enough Gold！";

            ATCOMMAND_MOB_ERROR = "Your Command is error";
            ATCOMMAND_WARP_ERROR = "Don't Warp this Map";

            PET_FRIENDLY_DOWN = "{0}'s friendly towards you is decreased.";

            POSSESSION_EXP = "Get (Possession) BaseEXP {0}  JobEXP {1} ";
            POSSESSION_DONE = "Possessioned to [{0}]";
            POSSESSION_RIGHT = "Right hand";
            POSSESSION_LEFT = "Left hand";
            POSSESSION_NECK = "Necklace";
            POSSESSION_ARMOR = "Armor";

            QUEST_HOW_TO_DO = "How to do?";
            QUEST_NOT_CANCEL = "Nothing will be canceled";
            QUEST_CANCEL = "Cancel quest";
            QUEST_CANCELED = "Quest is canceled......$R;";
            QUEST_REWARDED = "Got Reward!$R;";
            QUEST_FAILED = "Quest failed.....$R;";
            QUEST_IF_TAKE_QUEST = "Do you want to take the quest?";
            QUEST_TAKE = "Take it";
            QUEST_NOT_TAKE = "Don't take it";
            QUEST_TRANSPORT_GET = "Got courier luggage.";
            QUEST_TRANSPORT_GIVE = "Gave courier luggage.";

            PARTY_NEW_NAME = "New Party";

            SKILL_ACTOR_DELETE = "[{0}] destoried!!!";
            SKILL_STATUS_ENTER = "Became {0} Status";
            SKILL_STATUS_LEAVE = "{0} Status canceled";
            SKILL_DECOY = "Decoy:";

            ITEM_TREASURE_OPEN = "Please select the box";
            ITEM_TREASURE_NO_NEED = "There is no need to open the box";
            ITEM_IDENTIFY = "Please select the items to be identified";
            ITEM_IDENTIFY_NO_NEED = "There is no need to identify the items";
            ITEM_IDENTIFY_RESULT = "Identification Results: {0} -> {1}";
            ITEM_UNIDENTIFIED_NONE = "None";
            ITEM_UNIDENTIFIED_HELM = "Helmet";
            ITEM_UNIDENTIFIED_ACCE_HEAD = "Head Accessory";
            ITEM_UNIDENTIFIED_ACCE_FACE0 = "Face Accessory 1";
            ITEM_UNIDENTIFIED_ACCE_FACE1 = "Face Accessory 2";
            ITEM_UNIDENTIFIED_ACCE_FACE2 = "Face Accessory 3";
            ITEM_UNIDENTIFIED_ACCE_NECK = "Necklace";
            ITEM_UNIDENTIFIED_ACCE_FINGER = "Ring";
            ITEM_UNIDENTIFIED_ARMOR_UPPER = "Upper Body Armor";
            ITEM_UNIDENTIFIED_ARMOR_LOWER = "Lower Body Armor";
            ITEM_UNIDENTIFIED_ONEPIECE = "Dress";
            ITEM_UNIDENTIFIED_OVERALLS = "Work Pants";
            ITEM_UNIDENTIFIED_BODYSUIT = "Body suit";
            ITEM_UNIDENTIFIED_FACEBODYSUIT = "Face Body suit";
            ITEM_UNIDENTIFIED_BACKPACK = "Backpack";
            ITEM_UNIDENTIFIED_COAT = "Coat";
            ITEM_UNIDENTIFIED_SOCKS = "Socks";
            ITEM_UNIDENTIFIED_BOOTS = "Boots";
            ITEM_UNIDENTIFIED_SLACKS = "Slacks";
            ITEM_UNIDENTIFIED_LONGBOOTS = "Long Boots";
            ITEM_UNIDENTIFIED_HALFBOOTS = "Half Boots";
            ITEM_UNIDENTIFIED_FULLFACE = "Full Face";
            ITEM_UNIDENTIFIED_SHORT_SWORD = "Dagger";
            ITEM_UNIDENTIFIED_SWORD = "Sword";
            ITEM_UNIDENTIFIED_RAPIER = "Rapier";
            ITEM_UNIDENTIFIED_CLAW = "Claw";
            ITEM_UNIDENTIFIED_KNUCKLE = "Knucle";
            ITEM_UNIDENTIFIED_SHIELD = "Shield";
            ITEM_UNIDENTIFIED_HAMMER = "Hammer";
            ITEM_UNIDENTIFIED_AXE = "Axe";
            ITEM_UNIDENTIFIED_SPEAR = "Spear";
            ITEM_UNIDENTIFIED_STAFF = "Staff";
            ITEM_UNIDENTIFIED_THROW = "Throwing Weapon";
            ITEM_UNIDENTIFIED_BOW = "Bow";
            ITEM_UNIDENTIFIED_ARROW = "Arrow";
            ITEM_UNIDENTIFIED_GUN = "Gun";
            ITEM_UNIDENTIFIED_BULLET = "Bullet";
            ITEM_UNIDENTIFIED_HANDBAG = "Handbag";
            ITEM_UNIDENTIFIED_LEFT_HANDBAG = "Left Handbag";
            ITEM_UNIDENTIFIED_BOOK = "Book";
            ITEM_UNIDENTIFIED_INSTRUMENT = "Instrument";
            ITEM_UNIDENTIFIED_ROPE = "Rope";
            ITEM_UNIDENTIFIED_CARD = "Card";
            ITEM_UNIDENTIFIED_ETC_WEAPON = "???";
            ITEM_UNIDENTIFIED_SHOES = "Shoes";
            ITEM_UNIDENTIFIED_MONEY = "Money";
            ITEM_UNIDENTIFIED_FOOD = "Food";
            ITEM_UNIDENTIFIED_POTION = "Potion";
            ITEM_UNIDENTIFIED_MARIONETTE = "Marionette";
            ITEM_UNIDENTIFIED_GOLEM = "Golem";
            ITEM_UNIDENTIFIED_TREASURE_BOX = "Treasure Box";
            ITEM_UNIDENTIFIED_CONTAINER = "Container";
            ITEM_UNIDENTIFIED_TIMBER_BOX = "Timer Box";
            ITEM_UNIDENTIFIED_SEED = "Seed";
            ITEM_UNIDENTIFIED_SCROLL = "Scroll";
            ITEM_UNIDENTIFIED_SKILLBOOK = "Skill Book";
            ITEM_UNIDENTIFIED_PET = "Pet";
            ITEM_UNIDENTIFIED_PET_NEKOMATA = "Pet Nekomata";
            ITEM_UNIDENTIFIED_PET_YOUHEI = "Pet Youhei";
            ITEM_UNIDENTIFIED_BACK_DEMON = "Kay Tsutsumi";
            ITEM_UNIDENTIFIED_RIDE_PET = "Riding Pet";
            ITEM_UNIDENTIFIED_USE = "Props";
            ITEM_UNIDENTIFIED_PETFOOD = "Pet Food";
            ITEM_UNIDENTIFIED_STAMP = "Stamp";
            ITEM_UNIDENTIFIED_FG_FURNITURE = "Furniture";
            ITEM_UNIDENTIFIED_FG_BASEBUILD = "Parts";
            ITEM_UNIDENTIFIED_FG_ROOM_WALL = "Wallpaper";
            ITEM_UNIDENTIFIED_FG_ROOM_FLOOR = "Flooring";
            ITEM_UNIDENTIFIED_ITDGN = "Unknown thing";
            ITEM_UNIDENTIFIED_ROBOT_GROW = "Robot";
            ITEM_UNIDENTIFIED_COSTUME = "Special Clothing";

            FG_NAME = "This is {0}'s Flying garden";
            FG_NOT_FOUND = "You don't own a flying garden";
            FG_ALREADY_CALLED = "You've already called the flying garden";
            FG_CANNOT = "You can't call flying garden here";
            FG_FUTNITURE_SETUP = "{0} Placed ({1}/{2})";
            FG_FUTNITURE_REMOVE = "{0} Removed ({1}/{2})";
            FG_FUTNITURE_MAX = "Cannot place furniture any more";


            ITD_HOUR = "Hour";
            ITD_MINUTE = "Minute";
            ITD_SECOND = "Second";
            ITD_CRASHING = "This dungeon will collapse in {0}.";
            ITD_CREATED = "'s dungeon is created";
            ITD_PARTY_DISMISSED = "Party disbanded. This dungeon will collapse";
            ITD_QUEST_CANCEL = "Players canceled the quest. The dungeon will collapse";
            ITD_SELECT_DUUNGEON = "Select Dungeon";
            ITD_DUNGEON_NAME = " Dungeon";

            THEATER_WELCOME = "Welcome to cinema!";
            THEATER_COUNTDOWN = "{0} will be showed in {1} minutes";
            NPC_SHOP_CP_GET = "Got {0} CP.";
            NPC_SHOP_ECOIN_GET = "Got {0} ecoin";
            NPC_SHOP_CP_LOST = "Lost {0} CP";
            NPC_SHOP_ECOIN_LOST = "Lost {0} ecoin";

            WRP_ENTER = "You are now in the Battle of Champion";
            WRP_GOT = "You've got {0} WRP";
            WRP_LOST = "You've lost {0} WRP";
            DEATH_PENALTY = "You've lost EXP due to death penalty";

            ODWAR_PREPARE = "DEMs are marching towards {0}, will arrive in about {1} minutes";
            ODWAR_PREPARE2 = "Please reinforce immediately";
            ODWAR_START = "City Defence War started";
            ODWAR_SYMBOL_DOWN = "Symobl·Nr.{0} has been destoried!!!";
            ODWAR_SYMBOL_ACTIVATE = "Symbol·Nr.{0} has been activated!!!";
            ODWAR_LOSE = "West Fort is captured by DEMs!!!";
            ODWAR_WIN = "West Fort's Defence War was successful!";
            ODWAR_WIN2 = "West Fort's Symbols are now generation Defence Field!";
            ODWAR_WIN3 = "Enemies are retreating from West Fort!";
            ODWAR_WIN4 = "Enemies retreated! We won!";
            ODWAR_CAPTURE = "We successfully captured West Fort City!!";

            EP_INCREASE = "EP will increase in {0} hours";
            EP_INCREASED = "EP increased {0} points";

            NPC_ITEM_FUSION_RECHOOSE = "I want to choose again";
            NPC_ITEM_FUSION_CANCEL = "I want to cancel it";
            NPC_ITEM_FUSION_CONFIRM = "Success Rate{1}% {0}G";
        }

        public override string EnglishName => "English";

        public override string LocalName => "English";
    }
}