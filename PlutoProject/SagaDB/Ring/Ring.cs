using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;

namespace SagaDB.Ring
{
    public enum RingRight
    {
        RingMaster = 0x1,
        Ring2ndMaster = 0x2,
        AddRight = 0x4,
        KickRight = 0x8,
        FFRight = 0x10
    }

    public class Ring
    {
        /// <summary>
        ///     军团的飞空城名称
        /// </summary>
        public string FFName { get; set; }

        /// <summary>
        ///     军团的ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     军团名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     军团声望
        /// </summary>
        public uint Fame { get; set; }

        /// <summary>
        ///     军团飞空城的ID
        /// </summary>
        public uint FF_ID { get; set; }

        /// <summary>
        ///     军团的飞空城
        /// </summary>
        public FlyingCastle.FlyingCastle FFGarden { get; set; }

        /// <summary>
        ///     取得指定军团成员
        /// </summary>
        /// <param name="index">索引ID</param>
        /// <returns>成员玩家</returns>
        public ActorPC this[int index]
        {
            get
            {
                if (Members.ContainsKey(index))
                    return Members[index];
                return null;
            }
        }

        /// <summary>
        ///     队长
        /// </summary>
        public ActorPC Leader { get; set; }

        /// <summary>
        ///     军团成员
        /// </summary>
        public Dictionary<int, ActorPC> Members { get; } = new Dictionary<int, ActorPC>();

        /// <summary>
        ///     成员权限
        /// </summary>
        public Dictionary<int, BitMask<RingRight>> Rights { get; } = new Dictionary<int, BitMask<RingRight>>();

        /// <summary>
        ///     军团最大人数
        /// </summary>
        public int MaxMemberCount
        {
            get
            {
                var i = 1;
                while (RingFameTable.Instance.Items.ContainsKey((uint)i))
                {
                    if (Fame < RingFameTable.Instance.Items[(uint)i].Fame)
                        break;
                    i++;
                }

                return i - 1;
            }
        }

        /// <summary>
        ///     取得成员人数
        /// </summary>
        public int MemberCount => Members.Count;

        public ActorPC GetMember(uint char_id)
        {
            var chr =
                from c in Members.Values
                where c.CharID == char_id
                select c;
            if (chr.Count() == 0)
                return null;
            return chr.First();
        }

        /// <summary>
        ///     检查某个玩家是否是军团成员
        /// </summary>
        /// <param name="char_id">玩家的CharID</param>
        /// <returns>是否是军团成员</returns>
        public bool IsMember(uint char_id)
        {
            var chr =
                from c in Members.Values
                where c.CharID == char_id
                select c;
            return chr.Count() != 0;
        }

        /// <summary>
        ///     检查某个玩家是否是军团成员
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>是否是军团成员</returns>
        public bool IsMember(ActorPC pc)
        {
            return IsMember(pc.CharID);
        }

        /// <summary>
        ///     取得某个玩家成员ID
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>成员ID，如果不是军团成员则返回-1</returns>
        public int IndexOf(ActorPC pc)
        {
            foreach (byte i in Members.Keys)
                if (Members[i].CharID == pc.CharID)
                    return i;
            return -1;
        }

        /// <summary>
        ///     取得某个玩家成员ID
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>成员ID，如果不是军团成员则返回-1</returns>
        public int IndexOf(uint pc)
        {
            foreach (byte i in Members.Keys)
                if (Members[i].CharID == pc)
                    return i;
            return -1;
        }

        /// <summary>
        ///     成员上线，替换离线Actor
        /// </summary>
        /// <param name="newPC">新Actor</param>
        public void MemberOnline(ActorPC newPC)
        {
            if (!IsMember(newPC))
                return;
            var index = IndexOf(newPC);
            Members[index] = newPC;
            if (Leader.CharID == newPC.CharID)
                Leader = newPC;
        }

        /// <summary>
        ///     添加新的成员
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>军团中的索引</returns>
        public int NewMember(ActorPC pc)
        {
            if (IsMember(pc))
                return (byte)IndexOf(pc);
            var max = MaxMemberCount;
            for (var i = 8; i < max + 8; i++)
                if (!Members.ContainsKey(i))
                {
                    Members.Add(i, pc);
                    Rights.Add(i, new BitMask<RingRight>(new BitMask()));
                    return i;
                }

            return -1;
        }

        /// <summary>
        ///     删除成员
        /// </summary>
        /// <param name="pc">玩家</param>
        public void DeleteMemeber(ActorPC pc)
        {
            Rights.Remove(IndexOf(pc));
            Members.Remove(IndexOf(pc));
        }

        /// <summary>
        ///     删除成员
        /// </summary>
        /// <param name="pc">玩家</param>
        public void DeleteMemeber(uint pc)
        {
            Members.Remove(IndexOf(pc));
        }
    }
}