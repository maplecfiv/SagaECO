using System;
using System.Collections.Generic;
using SagaDB.Actor;

namespace SagaDB
{
    public class Account
    {
        /// <summary>
        ///     上次登录时间
        /// </summary>
        public DateTime lastLoginTime;

        /// <summary>
        ///     任务点下次重置时间
        /// </summary>
        public DateTime questNextTime;

        /// <summary>
        ///     帐号名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     人物删除密码
        /// </summary>
        public string DeletePassword { get; set; }

        /// <summary>
        ///     帐号ID
        /// </summary>
        public int AccountID { get; set; } = -1;

        /// <summary>
        ///     帐号所有人物
        /// </summary>
        public List<ActorPC> Characters { get; set; } = new List<ActorPC>();

        /// <summary>
        ///     GM权限
        /// </summary>
        public byte GMLevel { get; set; }

        /// <summary>
        ///     银行余额
        /// </summary>
        public uint Bank { get; set; }

        /// <summary>
        ///     帐号是否被封
        /// </summary>
        public bool Banned { get; set; }

        /// <summary>
        ///     上次登录IP
        /// </summary>
        public string LastIP { get; set; } = "";

        /// <summary>
        ///     补偿IP
        /// </summary>
        public string LastIP2 { get; set; } = "";

        /// <summary>
        ///     物理地址
        /// </summary>
        public string MacAddress { get; set; } = "";

        /// <summary>
        ///     该账号所有角色名
        /// </summary>
        public string PlayerNames { get; set; } = "";
    }
}