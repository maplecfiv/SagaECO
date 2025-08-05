using System;
using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_AI_DETAIL : Packet
    {
        private readonly byte ainum = 10;
        private readonly byte cubes_action_num;
        private readonly byte cubes_activeskill_num;
        private readonly byte cubes_condition_num;
        private readonly byte cubes_passiveskill_num;

        public SSMG_PARTNER_AI_DETAIL(byte cubes_condition_num, byte cubes_action_num, byte cubes_activeskill_num,
            byte cubes_passiveskill_num)
        {
            data = new byte[187 + 2 * cubes_condition_num + 2 * cubes_action_num + 2 * cubes_activeskill_num +
                            2 * cubes_passiveskill_num];
            offset = 2;
            this.cubes_condition_num = cubes_condition_num;
            this.cubes_action_num = cubes_action_num;
            this.cubes_activeskill_num = cubes_activeskill_num;
            this.cubes_passiveskill_num = cubes_passiveskill_num;
            ID = 0x2183;
            //seperators
            PutByte(10, 7);
            PutByte(10, 28);
            PutByte(10, 39);
            PutByte(10, 50);
            PutByte(10, 71);
            PutByte(10, 95);
            PutByte(10, 116);
            PutByte(10, 127);
            PutByte(10, 138);
            PutByte(10, 159);
        }

        public byte Unknown0
        {
            set => PutByte(0, 2);
        }

        public uint PartnerInventorySlot
        {
            set => PutUInt(value, 3);
        }

        /// <summary>
        ///     set cube unique ids
        /// </summary>
        public Dictionary<byte, ushort> Conditions_ID
        {
            set
            {
                var conditions = value;
                for (uint i = 0; i < ainum; i++)
                    if (conditions.ContainsKey((byte)i))
                        PutUShort(conditions[(byte)i], (ushort)(i * 2 + 8));
            }
        }

        /// <summary>
        ///     set cube unique ids
        /// </summary>
        public Dictionary<byte, ushort> Reactions_ID
        {
            set
            {
                var reactions = value;
                for (uint i = 0; i < ainum; i++)
                    if (reactions.ContainsKey((byte)i))
                        PutUShort(reactions[(byte)i], (ushort)(i * 2 + 51));
            }
        }

        /// <summary>
        ///     seconds
        /// </summary>
        public Dictionary<byte, ushort> Time_Intervals
        {
            set
            {
                var intervals = value;
                for (uint i = 0; i < ainum; i++)
                    if (intervals.ContainsKey((byte)i))
                        PutUShort(intervals[(byte)i], (ushort)(i * 2 + 72));
            }
        }

        /// <summary>
        ///     Set On/Off States of AIs
        /// </summary>
        public Dictionary<byte, bool> AI_states
        {
            set
            {
                var states = value;
                ushort off_states_sum = 0;
                for (uint i = 9; i > 0; i--)
                    if (states.ContainsKey((byte)i))
                        if (!states[(byte)i])
                            off_states_sum = (ushort)(off_states_sum + Math.Pow(2, i));

                PutUShort(off_states_sum, 92);
            }
        }

        /// <summary>
        ///     AI思考设定
        /// </summary>
        public byte BasicAI
        {
            set => PutByte(value, 94);
        }

        public List<ushort> Cubes_Condition
        {
            set
            {
                var cubes = value;
                PutByte(cubes_condition_num, 183);
                for (var i = 0; i < cubes_condition_num; i++) PutUShort(cubes[i], (ushort)(184 + i * 2));
            }
        }

        public List<ushort> Cubes_Action
        {
            set
            {
                var cubes = value;
                PutByte(cubes_action_num, 184 + 2 * cubes_condition_num);
                for (var i = 0; i < cubes_action_num; i++)
                    PutUShort(cubes[i], (ushort)(185 + 2 * cubes_condition_num + i * 2));
            }
        }

        public List<ushort> Cubes_Activeskill
        {
            set
            {
                var cubes = value;
                PutByte(cubes_activeskill_num, 185 + 2 * cubes_condition_num + 2 * cubes_action_num);
                for (var i = 0; i < cubes_activeskill_num; i++)
                    PutUShort(cubes[i], (ushort)(186 + 2 * cubes_condition_num + 2 * cubes_action_num + i * 2));
            }
        }

        public List<ushort> Cubes_Passiveskill
        {
            set
            {
                var cubes = value;
                PutByte(cubes_passiveskill_num,
                    186 + 2 * cubes_condition_num + 2 * cubes_action_num + 2 * cubes_activeskill_num);
                for (var i = 0; i < cubes_passiveskill_num; i++)
                    PutUShort(cubes[i],
                        (ushort)(187 + 2 * cubes_condition_num + 2 * cubes_action_num + 2 * cubes_activeskill_num +
                                 i * 2));
            }
        }
    }
}