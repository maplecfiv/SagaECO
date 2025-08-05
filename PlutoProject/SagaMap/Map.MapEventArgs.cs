using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Skill;
using SagaLib;

namespace SagaMap
{
    public partial class Map
    {
        internal object RegisterActor()
        {
            throw new NotImplementedException();
        }
    }

    public class ChatArg : MapEventArgs
    {
        public string content;
        public uint emotion;
        public byte expression;
        public byte loop;
        public MotionType motion;
    }

    public class MoveArg : MapEventArgs
    {
        public MoveType type;
        public ushort x, y, dir;
    }

    public class EffectArg : MapEventArgs
    {
        public uint actorID;
        public uint effectID;
        public ushort height = 0xFFFF;
        public bool oneTime = true;
        public byte x = 0xFF, y = 0xFF;
    }

    public class PossessionArg : MapEventArgs
    {
        public bool cancel = false;
        public string comment;
        public byte dir;
        public uint fromID;
        public int result;
        public uint toID;
        public byte x;
        public byte y;
    }

    public class AutoCastInfo
    {
        public int delay;
        public byte level;
        public uint skillID;
        public byte x = 0xff;
        public byte y = 0xff;
    }

    public class SkillArg : MapEventArgs
    {
        public enum ArgType
        {
            Attack,
            Cast,
            Active,
            Item_Cast,
            Item_Active,
            Actor_Active
        }

        public List<Actor> affectedActors = new List<Actor>();

        public ArgType argType = ArgType.Attack;
        public List<AutoCastInfo> autoCast = new List<AutoCastInfo>();
        public uint delay;
        public float delayRate = 1f;
        public List<AttackFlag> flag = new List<AttackFlag>();
        public List<int> hp = new List<int>(), mp = new List<int>(), sp = new List<int>();
        public uint inventorySlot;
        public Item item;
        public short result;
        public uint sActor, dActor;
        public bool showEffect = true;
        public SagaDB.Skill.Skill skill;
        public ATTACK_TYPE type;
        public bool useMPSP = true;
        public byte x, y;

        public SkillArg Clone()
        {
            var arg = new SkillArg();
            arg.sActor = sActor;
            arg.dActor = dActor;
            arg.skill = skill;
            arg.x = x;
            arg.y = y;
            arg.argType = argType;
            arg.affectedActors = affectedActors;
            return arg;
        }

        public SkillArg CloneWithoutSkill()
        {
            var arg = new SkillArg();
            var skill = new SagaDB.Skill.Skill();
            skill.BaseData = new SkillData();
            skill.BaseData.id = 0;
            arg.sActor = sActor;
            arg.dActor = dActor;
            arg.skill = skill;
            arg.x = x;
            arg.y = y;
            arg.argType = argType;
            return arg;
        }

        public void Init()
        {
            hp = new List<int>();
            mp = new List<int>();
            sp = new List<int>();
            flag = new List<AttackFlag>();
            for (var i = 0; i < affectedActors.Count; i++)
            {
                flag.Add(0);
                hp.Add(0);
                mp.Add(0);
                sp.Add(0);
            }
        }

        public void Add(SkillArg arg)
        {
            for (var i = 0; i < arg.affectedActors.Count; i++)
            {
                affectedActors.Add(arg.affectedActors[i]);
                flag.Add(arg.flag[i]);
                hp.Add(arg.hp[i]);
                mp.Add(arg.mp[i]);
                sp.Add(arg.sp[i]);
            }
        }

        public void AddSameActor(SkillArg arg)
        {
            var count = affectedActors.Count;
            for (var i = 0; i < arg.affectedActors.Count; i++)
            for (var j = 0; j < count; j++)
            {
                if (arg.affectedActors[i].ActorID == affectedActors[j].ActorID)
                {
                    hp[j] += arg.hp[i];
                    mp[j] += arg.mp[i];
                    sp[j] += arg.sp[i];
                    break;
                }

                if (j == count - 1)
                {
                    affectedActors.Add(arg.affectedActors[i]);
                    flag.Add(arg.flag[i]);
                    hp.Add(arg.hp[i]);
                    mp.Add(arg.mp[i]);
                    sp.Add(arg.sp[i]);
                }
            }
        }

        public void Remove(Actor actor)
        {
            if (affectedActors.Contains(actor))
            {
                hp.RemoveAt(affectedActors.IndexOf(actor));
                mp.RemoveAt(affectedActors.IndexOf(actor));
                sp.RemoveAt(affectedActors.IndexOf(actor));
                flag.RemoveAt(affectedActors.IndexOf(actor));
                affectedActors.Remove(actor);
            }
        }

        public void Extend(int count)
        {
            for (var i = 0; i < count; i++)
                hp.Add(0);
            for (var i = 0; i < count; i++)
                mp.Add(0);
            for (var i = 0; i < count; i++)
                sp.Add(0);
            for (var i = 0; i < count; i++)
                flag.Add(0);
        }
    }
}