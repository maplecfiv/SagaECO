using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Shaman_精灵使_
{
    /// <summary>
    ///     精靈祈願（精霊への祈り）
    /// </summary>
    public class PrayerToTheElf : ISkill
    {
        private Elements MapElement = Elements.Neutral;

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.CheckActorSkillInRange(SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height), 250)) return -17;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var value = 0;
            foreach (var item in sActor.AttackElements)
                if (value < SkillHandler.Instance.fieldelements(map, SagaLib.Global.PosX16to8(sActor.X, map.Width),
                        SagaLib.Global.PosY16to8(sActor.Y, map.Height), item.Key))
                {
                    value = SkillHandler.Instance.fieldelements(map, SagaLib.Global.PosX16to8(sActor.X, map.Width),
                        SagaLib.Global.PosY16to8(sActor.Y, map.Height), item.Key);
                    MapElement = item.Key;
                }

            if (MapElement != Elements.Neutral)
            {
                var skill = new PrayerToTheElfBuff(args.skill, sActor, MapElement + "PrayerToTheElf", lifetime,
                    MapElement, SagaLib.Global.PosX16to8(sActor.X, map.Width),
                    SagaLib.Global.PosY16to8(sActor.Y, map.Height));
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        public class PrayerToTheElfBuff : DefaultBuff
        {
            private readonly byte centerX;
            private readonly byte centerY;
            private readonly Map map;
            private readonly Elements MapElement;
            private readonly string prefix;

            public PrayerToTheElfBuff(SagaDB.Skill.Skill skill, Actor actor, string AdditionName, int lifetime,
                Elements e, byte x, byte y)
                : base(skill, actor, AdditionName, lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                MapElement = e;
                centerX = x;
                centerY = y;
                prefix = MapElement + "PrayerToTheElf";
                map = MapManager.Instance.GetMap(actor.MapID);
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                var elevalue = (byte)(skill.skill.Level * 10);
                switch (MapElement)
                {
                    case Elements.Dark:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                    {
                                        if (skill.Variable.ContainsKey(getVariableKey(x, y)))
                                            skill.Variable.Remove(getVariableKey(x, y));
                                        skill.Variable.Add(getVariableKey(x, y), map.Info.dark[x, y]);
                                        map.Info.dark[x, y] += elevalue;
                                    }

                        break;
                    case Elements.Holy:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                    {
                                        if (skill.Variable.ContainsKey(getVariableKey(x, y)))
                                            skill.Variable.Remove(getVariableKey(x, y));
                                        skill.Variable.Add(getVariableKey(x, y), map.Info.holy[x, y]);
                                        map.Info.holy[x, y] += elevalue;
                                    }

                        break;
                    case Elements.Fire:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                    {
                                        if (skill.Variable.ContainsKey(getVariableKey(x, y)))
                                            skill.Variable.Remove(getVariableKey(x, y));
                                        skill.Variable.Add(getVariableKey(x, y), map.Info.fire[x, y]);
                                        map.Info.fire[x, y] += elevalue;
                                    }

                        break;
                    case Elements.Water:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                    {
                                        if (skill.Variable.ContainsKey(getVariableKey(x, y)))
                                            skill.Variable.Remove(getVariableKey(x, y));
                                        skill.Variable.Add(getVariableKey(x, y), map.Info.water[x, y]);
                                        map.Info.water[x, y] += elevalue;
                                    }

                        break;
                    case Elements.Wind:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                    {
                                        if (skill.Variable.ContainsKey(getVariableKey(x, y)))
                                            skill.Variable.Remove(getVariableKey(x, y));
                                        skill.Variable.Add(getVariableKey(x, y), map.Info.wind[x, y]);
                                        map.Info.wind[x, y] += elevalue;
                                    }

                        break;
                    case Elements.Earth:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                    {
                                        if (skill.Variable.ContainsKey(getVariableKey(x, y)))
                                            skill.Variable.Remove(getVariableKey(x, y));
                                        skill.Variable.Add(getVariableKey(x, y), map.Info.earth[x, y]);
                                        map.Info.earth[x, y] += elevalue;
                                    }

                        break;
                }
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                switch (MapElement)
                {
                    case Elements.Dark:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                        map.Info.dark[x, y] = (byte)skill.Variable[getVariableKey(x, y)];

                        break;
                    case Elements.Holy:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                        map.Info.holy[x, y] = (byte)skill.Variable[getVariableKey(x, y)];

                        break;
                    case Elements.Fire:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                        map.Info.fire[x, y] = (byte)skill.Variable[getVariableKey(x, y)];

                        break;
                    case Elements.Water:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                        map.Info.water[x, y] = (byte)skill.Variable[getVariableKey(x, y)];

                        break;
                    case Elements.Wind:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                        map.Info.wind[x, y] = (byte)skill.Variable[getVariableKey(x, y)];

                        break;
                    case Elements.Earth:
                        for (var x = centerX - 2; x <= centerX + 2; x++)
                            for (var y = centerY - 2; y <= centerY + 2; y++)
                                if (x >= 0 && x <= 255)
                                    if (y >= 0 && y <= 255)
                                        map.Info.earth[x, y] = (byte)skill.Variable[getVariableKey(x, y)];

                        break;
                }
            }

            private string getVariableKey(int x, int y)
            {
                return prefix + string.Format("{0:000}", x) + string.Format("{0:000}", y);
            }
        }

        //#endregion
    }
}