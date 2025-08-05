using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;

namespace SagaDB.Group
{
    public class Group
    {
        public uint MaxMember = 8;

        /// <summary>
        ///     队伍随机分配开关（0为开，1为关）
        /// </summary>
        public byte Roll;

        /// 临时字符串变量集
        /// </summary>
        public VariableHolder<string, string> TStr { get; } = new VariableHolder<string, string>("");

        /// <summary>
        ///     临时整数变量集
        /// </summary>
        public VariableHolder<string, int> TInt { get; } = new VariableHolder<string, int>(0);

        /// <summary>
        ///     临时标识变量集
        /// </summary>
        public VariableHolderA<string, BitMask> TMask { get; } = new VariableHolderA<string, BitMask>();

        public VariableHolderA<string, DateTime> TTime { get; } = new VariableHolderA<string, DateTime>();

        /// <summary>
        ///     队伍的ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     队伍名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     取得指定队伍成员
        /// </summary>
        /// <param name="index">索引ID</param>
        /// <returns>成员玩家</returns>
        public ActorPC this[byte index]
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
        ///     队伍成员
        /// </summary>
        public Dictionary<byte, ActorPC> Members { get; } = new Dictionary<byte, ActorPC>();

        /// <summary>
        ///     取得成员人数
        /// </summary>
        public int MemberCount => Members.Count;

        /// <summary>
        ///     检查某个玩家是否是队伍成员
        /// </summary>
        /// <param name="char_id">玩家的CharID</param>
        /// <returns>是否是队伍成员</returns>
        public bool IsMember(uint char_id)
        {
            var chr =
                from c in Members.Values
                where c.CharID == char_id
                select c;
            return chr.Count() != 0;
        }

        /// <summary>
        ///     检查某个玩家是否是队伍成员
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>是否是队伍成员</returns>
        public bool IsMember(ActorPC pc)
        {
            return IsMember(pc.CharID);
        }

        /// <summary>
        ///     取得某个玩家成员ID
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>成员ID，如果不是队伍成员则返回-1</returns>
        public byte IndexOf(ActorPC pc)
        {
            foreach (var i in Members.Keys)
                if (Members[i].CharID == pc.CharID)
                    return i;
            return 255;
        }

        /// <summary>
        ///     取得某个玩家成员ID
        /// </summary>
        /// <param name="pc">玩家</param>
        /// <returns>成员ID，如果不是队伍成员则返回-1</returns>
        public byte IndexOf(uint pc)
        {
            foreach (var i in Members.Keys)
                if (Members[i].CharID == pc)
                    return i;
            return 255;
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
        /// <returns>队伍中的索引</returns>
        public byte NewMember(ActorPC pc)
        {
            if (IsMember(pc))
                return IndexOf(pc);
            for (byte i = 0; i < 8; i++)
                if (!Members.ContainsKey(i))
                {
                    Members.Add(i, pc);
                    return i;
                }

            return 255;
        }

        /// <summary>
        ///     删除成员
        /// </summary>
        /// <param name="pc">玩家</param>
        public void DeleteMemeber(ActorPC pc)
        {
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

        /// <summary>
        ///     根据CharID查找成员
        /// </summary>
        /// <param name="pc">玩家</param>
        public ActorPC SearchMemeber(uint pc)
        {
            foreach (var i in Members.Keys)
                if (Members[i].CharID == pc)
                    return Members[i];
            return null;
        }
    }
}