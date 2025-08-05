using SagaDB.Actor;
using SagaDB.Mob;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Packets.Server.NPC;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     寶物搜查（トレジャースキャンニング）
    /// </summary>
    public class SearchTreasure : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            int[] range = { 0, 5000, 7000, 10000 };
            var actor = new ActorSkill(args.skill, sActor);
            var lifetime = 10000 * level;
            var skill = new DefaultBuff(args.skill, sActor, "SearchTreasure", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);

            //Define MapClient
            var client = MapClient.FromActorPC((ActorPC)sActor);

            //Locate Map
            var map = MapManager.Instance.GetMap(sActor.MapID);

            //Search Range
            var affected = map.GetActorsArea(sActor, (short)range[level], false);

            var i = 0;


            byte arrX = 255;
            byte arrY = 255;
            double length = 255;

            //Search Mob
            foreach (var act in affected)
                if (act.type == ActorType.MOB)
                {
                    var m = (ActorMob)act;
                    if (m.BaseData.mobType == MobType.TREASURE_BOX_MATERIAL ||
                        m.BaseData.mobType == MobType.CONTAINER_MATERIAL ||
                        m.BaseData.mobType == MobType.TIMBER_BOX_MATERIAL)
                    {
                        if (m.HP <= 0) continue;
                        i++;
                        if (SagaLib.Global.PosX16to8(act.X, map.Width) <= arrX &&
                            SagaLib.Global.PosY16to8(act.Y, map.Width) <= arrY)
                            if (map.GetLengthD(actor.X, actor.Y, SagaLib.Global.PosX16to8(act.X, map.Width),
                                    SagaLib.Global.PosY16to8(act.Y, map.Width)) <= length)
                            {
                                length = map.GetLengthD(actor.X, actor.Y, SagaLib.Global.PosX16to8(act.X, map.Width),
                                    SagaLib.Global.PosY16to8(act.Y, map.Width));
                                arrX = SagaLib.Global.PosX16to8(act.X, map.Width);
                                arrY = SagaLib.Global.PosY16to8(act.Y, map.Width);
                            }
                        //break;
                    }
                }

            //If Not Mob found
            if (i <= 0)
            {
                client.SendSystemMessage("沒有對象於搜索範圍內。");
            }
            else
            {
                var p = new SSMG_NPC_NAVIGATION();
                p.X = SagaLib.Global.PosX16to8(arrX, map.Width);
                p.Y = SagaLib.Global.PosY16to8(arrY, map.Width);
                p.Type = 0;
                client.netIO.SendPacket(p);

                client.SendSystemMessage("已進入搜索狀態。");
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //Define MapClient
            var client = MapClient.FromActorPC((ActorPC)actor);

            client.SendSystemMessage("已解除搜索狀態。");
            var p = new SSMG_NPC_NAVIGATION_CANCEL();
            client.netIO.SendPacket(p);
        }

        #endregion
    }
}