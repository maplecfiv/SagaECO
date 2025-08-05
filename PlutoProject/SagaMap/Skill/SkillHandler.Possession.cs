using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill
{
    public partial class SkillHandler
    {
        private List<Actor> ProcessAttackPossession(Actor dActor)
        {
            var list = new List<Actor>();
            if (dActor.type != ActorType.PC)
                return list;
            var pc = (ActorPC)dActor;
            foreach (var i in pc.PossesionedActors)
            {
                if (!i.Online)
                    continue;
                if (Global.Random.Next(0, 99) < i.Status.possessionCancel)
                    continue;
                switch (i.PossessionPosition)
                {
                    case PossessionPosition.NECK:
                        if (Global.Random.Next(0, 99) < 8)
                            list.Add(i);
                        break;
                    case PossessionPosition.CHEST:
                        if (Global.Random.Next(0, 99) < 12)
                            list.Add(i);
                        break;
                    case PossessionPosition.RIGHT_HAND:
                        if (Global.Random.Next(0, 99) < 15)
                            list.Add(i);
                        break;
                    case PossessionPosition.LEFT_HAND:
                        if (Global.Random.Next(0, 99) < 18)
                            list.Add(i);
                        break;
                }
            }

            return list;
        }
    }
}