using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Npc;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Scripting
{
    public delegate void MobAttackCallback(MobEventHandler eh, ActorPC pc);

    public delegate void MobCallback(MobEventHandler eh, ActorPC pc);

    public delegate void PartnerCallback(PartnerEventHandler eh, ActorPC pc);

    public delegate void TimerCallback(Timer timer, ActorPC pc);

    /// <summary>
    ///     所有脚本文件的基类
    /// </summary>
    public abstract partial class Event
    {
        /// <summary>
        ///     如果玩家已经领了任务，NPC回复
        /// </summary>
        protected string alreadyHasQuest = "";

        /// <summary>
        ///     玩家领取一般任务(非搬运任务）后的回复
        /// </summary>
        protected string gotNormalQuest = "";

        /// <summary>
        ///     玩家领取搬运任务后的回复
        /// </summary>
        protected string gotTransportQuest = "";

        /// <summary>
        ///     领取任务需要最少任务点
        /// </summary>
        protected int leastQuestPoint = 1;

        /// <summary>
        ///     任务点不够时回复
        /// </summary>
        protected string notEnoughQuestPoint = "";

        /// <summary>
        ///     任务取消后回复
        /// </summary>
        protected string questCanceled = "";

        /// <summary>
        ///     任务完成后的回复
        /// </summary>
        protected string questCompleted = "";

        /// <summary>
        ///     任务失败的回复
        /// </summary>
        protected string questFailed = "";

        /// <summary>
        ///     任务太简单的回复
        /// </summary>
        protected string questTooEasy = "";

        /// <summary>
        ///     任务太困难的回复
        /// </summary>
        protected string questTooHard = "";

        /// <summary>
        ///     搬运完成后目标NPC回复
        /// </summary>
        protected string questTransportCompleteDest = "";

        /// <summary>
        ///     搬运完成后起始NPC回复
        /// </summary>
        protected string questTransportCompleteSrc = "";

        /// <summary>
        ///     搬运任务目标NPC回复
        /// </summary>
        protected string questTransportDest = "";

        /// <summary>
        ///     搬运任务起始NPC回复
        /// </summary>
        protected string questTransportSource = "";

        /// <summary>
        ///     如果是搬运任务，问候语
        /// </summary>
        protected string transport = "";

        /// <summary>
        ///     触发当前脚本的玩家
        /// </summary>
        public ActorPC CurrentPC { get; set; }

        /// <summary>
        ///     当前脚本所设置的默认商店物品列表
        /// </summary>
        public List<uint> Goods { get; } = new List<uint>();

        /// <summary>
        ///     NPC所收购的物品价值的上限
        ///     <remarks>默认为2000</remarks>
        /// </summary>
        protected uint BuyLimit { get; set; } = 2000;

        /// <summary>
        ///     服务器专有字符串变量集
        /// </summary>
        protected VariableHolder<string, string> SStr => ScriptManager.Instance.VariableHolder.AStr;

        /// <summary>
        ///     服务器专有整数变量集
        /// </summary>
        protected VariableHolder<string, int> SInt => ScriptManager.Instance.VariableHolder.AInt;

        /// <summary>
        ///     服务器专有标识变量集
        /// </summary>
        protected VariableHolderA<string, BitMask> SMask => ScriptManager.Instance.VariableHolder.AMask;

        /// <summary>
        ///     服务器专有列表变量集
        /// </summary>
        protected VariableHolderA<string, VariableHolderA<string, int>> SDict =>
            ScriptManager.Instance.VariableHolder.Adict;

        /// <summary>
        ///     当前脚本的EventID
        /// </summary>
        public uint EventID { get; set; }

        private MapClient GetMapClient(ActorPC pc)
        {
            var eh = (PCEventHandler)pc.e;
            return eh.Client;
        }

        public override string ToString()
        {
            if (NPCFactory.Instance.Items.ContainsKey(EventID))
                return NPCFactory.Instance.Items[EventID].Name + "(" + EventID + ")";

            return base.ToString();
        }
    }
}