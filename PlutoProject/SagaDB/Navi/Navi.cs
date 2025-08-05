using System.Collections.Generic;

namespace SagaDB.Navi
{
    /// <summary>
    ///     导航
    /// </summary>
    public class Navi
    {
        public Navi()
        {
            Categories = new Dictionary<uint, Category>();
        }

        public Navi(Navi navi)
        {
            //this.categories = new Dictionary<uint, Category>(navi.categories);
        }

        public Dictionary<uint, Category> Categories { get; }

        public Dictionary<uint, Step> UniqueSteps { get; }
    }

    public class Category
    {
        public Category(uint id)
        {
            this.ID = id;
        }

        public uint ID { get; }

        public Dictionary<uint, Event> Events { get; } = new Dictionary<uint, Event>();
    }

    public class Event
    {
        //bool show = true;
        //bool finished = false;
        //bool notRewarded = false;

        private byte state = 1;

        public Event(uint id)
        {
            this.ID = id;
        }

        public uint ID { get; }

        /// <summary>
        ///     Event状态，最低位为是否显示（1为显示），次低位为是否完成（1为完成），三低位为是否已领取奖励（1为未领取）
        /// </summary>
        public byte State
        {
            get => state;
            set
            {
                if (value > 7)
                    state = 7;
                else
                    state = value;
            }
        }

        /// <summary>
        ///     设定和获取Event是否显示（1为显示）
        /// </summary>
        public bool Show
        {
            get => toBool(state &= 1);
            set
            {
                if (value)
                    state |= 1;
                else
                    state &= 0xFE;
            }
        }

        /// <summary>
        ///     设定和获取Event是否完成（1为完成）
        /// </summary>
        public bool Finished
        {
            get => toBool(state &= 2);
            set
            {
                if (value)
                    state |= 2;
                else
                    state &= 0xFD;
            }
        }

        /// <summary>
        ///     设定和获取Event是否已领取奖励（1为未领取）
        /// </summary>
        public bool NotRewarded
        {
            get => toBool(state &= 4);
            set
            {
                if (value)
                    state |= 4;
                else
                    state &= 0xFB;
            }
        }

        /// <summary>
        ///     设定和获取Event的每个步骤是否在导航开始后显示
        /// </summary>
        public int DisplaySteps { get; set; }

        /// <summary>
        ///     设定和获取Event的每个步骤是否完成
        /// </summary>
        public int FinishedSteps { get; set; }

        public Dictionary<uint, Step> Steps { get; } = new Dictionary<uint, Step>();

        private bool toBool(byte b)
        {
            if (b == 0) return false;
            return true;
        }
    }

    public class Step
    {
        //bool display = false;
        //bool finished = false;

        public Step(uint id, uint uniqueId, Event belongEvent)
        {
            this.ID = id;
            this.UniqueId = uniqueId;
            this.BelongEvent = belongEvent;
        }

        public uint ID { get; }

        public uint UniqueId { get; }

        public bool Display
        {
            get => toBool(BelongEvent.DisplaySteps &= 1 << ((int)ID - 1));
            set
            {
                if (value)
                    BelongEvent.DisplaySteps |= 1 << ((int)ID - 1);
                else
                    BelongEvent.DisplaySteps &= ~(1 << ((int)ID - 1));
            }
        }

        public bool Finished
        {
            get => toBool(BelongEvent.FinishedSteps &= 1 << ((int)ID - 1));
            set
            {
                if (value)
                    BelongEvent.FinishedSteps |= 1 << ((int)ID - 1);
                else
                    BelongEvent.FinishedSteps &= ~(1 << ((int)ID - 1));
            }
        }

        public Event BelongEvent { get; set; }

        private bool toBool(int i)
        {
            if (i == 0) return false;
            return true;
        }
    }
}