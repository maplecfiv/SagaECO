using SagaDB.Actor;
using SagaLib;
using SagaMap.Mob;

namespace SagaMap.Tasks.System.ActivityTasks.BlueBerryActivity.Mob
{
    public class 活动怪物
    {
        public static ActorMob.MobInfo 活动蓝莓Info()
        {
            var info = new ActorMob.MobInfo();
            info.name = "蓝莓";
            info.level = 100;
            info.maxhp = 5000;
            info.speed = 720;
            info.atk_min = 100;
            info.atk_max = 200;
            info.matk_min = 117;
            info.matk_max = 237;
            info.def = 100;
            info.def_add = 20000;
            info.mdef = 100;
            info.mdef_add = 20000;
            info.hit_critical = 23;
            info.hit_magic = 118;
            info.hit_melee = 118;
            info.hit_ranged = 120;
            info.avoid_critical = 0;
            info.avoid_magic = 0;
            info.avoid_melee = 0;
            info.avoid_ranged = 0;
            info.Aspd = 640;
            info.Cspd = 540;
            info.elements[Elements.Neutral] = 0;
            info.elements[Elements.Fire] = 0;
            info.elements[Elements.Water] = 70;
            info.elements[Elements.Wind] = 0;
            info.elements[Elements.Earth] = 0;
            info.elements[Elements.Holy] = 0;
            info.elements[Elements.Dark] = 0;
            info.abnormalstatus[AbnormalStatus.Confused] = 30;
            info.abnormalstatus[AbnormalStatus.Frosen] = 30;
            info.abnormalstatus[AbnormalStatus.Paralyse] = 30;
            info.abnormalstatus[AbnormalStatus.Poisen] = 30;
            info.abnormalstatus[AbnormalStatus.Silence] = 30;
            info.abnormalstatus[AbnormalStatus.Sleep] = 30;
            info.abnormalstatus[AbnormalStatus.Stone] = 30;
            info.abnormalstatus[AbnormalStatus.Stun] = 30;
            info.abnormalstatus[AbnormalStatus.MoveSpeedDown] = 30;
            info.baseExp = 0;
            info.jobExp = 0;


            return info;
        }

        public static AIMode 活动蓝莓AI()
        {
            var ai = new AIMode(6);
            ai.MobID = 10000000;
            ai.isNewAI = true; //1為主動，0為被動
            ai.MobID = 10960002; //怪物ID
            ai.isNewAI = true; //使用的是TT AI
            ai.Distance = 3; //遠程進程切換距離，與敵人3格距離切換
            ai.ShortCD = 3; //進程技能表最短釋放間隔，3秒一次
            ai.LongCD = 3; //遠程技能表最短釋放間隔，3秒一次
            var skillinfo = new AIMode.SkilInfo();


            return ai;
        }
    }
}