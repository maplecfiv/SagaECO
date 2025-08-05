using System.Collections.Generic;
using SagaDB.Actor;

namespace SagaDB.PProtect
{
    public class PProtect
    {
        public byte MaxMember = 4;


        /// <summary>
        ///     队伍序列ID
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        ///     副本ID
        /// </summary>
        public uint TaskID { get; set; }

        /// <summary>
        ///     队伍名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     召集人
        /// </summary>
        public ActorPC Leader { get; set; }

        /// <summary>
        ///     队伍成员
        /// </summary>
        public List<ActorPC> Members { get; } = new List<ActorPC>();

        /// <summary>
        ///     取得成员人数
        /// </summary>
        public int MemberCount => Members.Count;

        /// <summary>
        ///     招募信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     是否加密
        /// </summary>
        public bool IsPassword => !string.IsNullOrEmpty(Password);

        /// <summary>
        ///     状态 0为招募中 1为游戏中
        /// </summary>
        public byte IsRun { get; set; }
    }
}