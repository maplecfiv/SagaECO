using System;
using System.Collections.Generic;
using SagaDB.Partner;
using SagaLib;

namespace SagaDB.Actor
{
    public class ActorPartner : Actor
    {
        public Dictionary<byte, ushort> ai_conditions = new Dictionary<byte, ushort>();
        public Dictionary<byte, ushort> ai_intervals = new Dictionary<byte, ushort>();
        public byte ai_mode, basic_ai_mode, basic_ai_mode_2;
        public Dictionary<byte, ushort> ai_reactions = new Dictionary<byte, ushort>();
        public Dictionary<byte, bool> ai_states = new Dictionary<byte, bool>();
        public bool AutoAttack = false;
        protected PartnerData basedata;
        public byte battleStatus;
        public List<ushort> equipcubes_action = new List<ushort>();
        public List<ushort> equipcubes_activeskill = new List<ushort>();
        public List<ushort> equipcubes_condition = new List<ushort>();
        public List<ushort> equipcubes_passiveskill = new List<ushort>();

        public Dictionary<EnumPartnerEquipSlot, Item.Item> equipments =
            new Dictionary<EnumPartnerEquipSlot, Item.Item>();

        public ulong exp, rankexp, reliabilityexp; //to be added into sql
        public bool Fictitious;
        public List<Item.Item> foods = new List<Item.Item>();
        public uint LastAttackActorID;
        private byte lv;
        public bool motion_loop, online;
        public DateTime nextfeedtime; //to be added into sql
        public uint partnerid;
        public byte perk0, perk1, perk2, perk3, perk4, perk5;
        public ushort perkpoint;
        public byte rank;
        public bool rebirth;
        public byte reliability;
        public ushort reliabilityuprate; //to be added into sql

        public Dictionary<uint, Skill.Skill> Skills = new Dictionary<uint, Skill.Skill>();

        public ActorPartner(uint partnerid, Item.Item partneritem)
        {
            type = ActorType.PARTNER;
            basedata = PartnerFactory.Instance.GetPartnerData(partneritem.BaseData.petID);
            Name = basedata.name;
            Speed = 780; //this.basedata.speed;
            Status.attackType = basedata.attackType;
            sightRange = 1500;
            PictID = partneritem.PictID;
        }

        public PartnerData BaseData
        {
            get => basedata;
            set => basedata = value;
        }

        /// <summary>
        ///     存在于数据库的ActorPartnerID
        /// </summary>
        public uint ActorPartnerID { get; set; }

        public ActorPC Owner { get; set; }

        /// <summary>
        ///     等级
        /// </summary>
        public override byte Level
        {
            get => lv;
            set
            {
                lv = value;
                if (e != null)
                    e.PropertyUpdate(UpdateEvent.LEVEL, 0);
            }
        }

        /// <summary>
        ///     动作
        /// </summary>
        public MotionType Motion { get; set; }

        public bool MotionLoop
        {
            get => motion_loop;
            set => motion_loop = value;
        }

        //public EmotionType Emotion { get { return this.emotion; } set { this.emotion = value; }}
    }
}