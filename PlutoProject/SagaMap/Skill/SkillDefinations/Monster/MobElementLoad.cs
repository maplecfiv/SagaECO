using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     螺旋風！、燃燒的路、凍結的路、私家路、死神
    /// </summary>
    public class MobElementLoad : ISkill
    {
        private readonly uint NextSkillID;

        public MobElementLoad(uint Next_SkillID)
        {
            NextSkillID = Next_SkillID;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var posX = new byte[3];
            var posY = new byte[3];
            SkillHandler.Instance.GetRelatedPos(sActor, 0, 0, out posX[0], out posY[0]);
            switch (SkillHandler.Instance.GetDirection(sActor))
            {
                case SkillHandler.ActorDirection.North:
                case SkillHandler.ActorDirection.NorthEast:
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 3, out posX[1], out posY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, 6, out posX[2], out posY[2]);
                    break;
                case SkillHandler.ActorDirection.South:
                case SkillHandler.ActorDirection.SouthEast:
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -3, out posX[1], out posY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 0, -6, out posX[2], out posY[2]);
                    break;
                case SkillHandler.ActorDirection.West:
                case SkillHandler.ActorDirection.NorthWest:
                    SkillHandler.Instance.GetRelatedPos(sActor, -3, 0, out posX[1], out posY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, -6, 0, out posX[2], out posY[2]);
                    break;
                case SkillHandler.ActorDirection.East:
                case SkillHandler.ActorDirection.SouthWest:
                    SkillHandler.Instance.GetRelatedPos(sActor, 3, 0, out posX[1], out posY[1]);
                    SkillHandler.Instance.GetRelatedPos(sActor, 6, 0, out posX[2], out posY[2]);
                    break;
            }

            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, 1, 0, posX[0], posY[0]));
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, 1, 0, posX[1], posY[1]));
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, 1, 0, posX[2], posY[2]));
        }

        #endregion
    }
}