using SagaDB.Actor;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Breeder
{
    /// <summary>
    ///     メタモルフォーゼ（メタモルフォーゼ）
    /// </summary>
    public class Metamorphosis : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            var pc = (ActorPC)sActor;
            var client = MapClient.FromActorPC(pc);
            if (pc.TranceID != 0)
            {
                pc.TranceID = 0;
            }
            else
            {
                if (pet != null)
                    if (!SkillHandler.Instance.IsRidePet(pet))
                        pc.TranceID = pet.BaseData.pictid;
            }

            client.SendCharInfoUpdate();
        }

        #endregion
    }
}