using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_OPEN_MIRROR_WINDOW : Packet
    {
        public SSMG_PLAYER_OPEN_MIRROR_WINDOW()
        {
            data = new byte[100];
            ID = 0x02B3;
            offset = 2;
        }

        public List<ushort> SetFace
        {
            set
            {
                PutByte(20);

                for (var i = 0; i < 20; i++)
                    if (i < value.Count)
                        PutUShort(value[i]);
                    else
                        PutUShort(0xFFFF);
            }
        }

        public List<ushort> SetHairStyle
        {
            set
            {
                PutByte(20);
                for (var i = 0; i < 20; i++)
                    if (i < value.Count)
                        PutUShort(value[i]);
                    else
                        PutUShort(0xFFFF);
            }
        }

        public List<ushort> SetHairColor
        {
            set
            {
                PutByte(20);
                for (var i = 0; i < 20; i++)
                    if (i < value.Count)
                        PutUShort(value[i]);
                    else
                        PutUShort(0xFFFF);
            }
        }

        public List<uint> SetUnknow
        {
            set
            {
                PutByte(20);
                for (var i = 0; i < 20; i++)
                    if (i < value.Count)
                        PutUInt(0xFFFFFFFF);
                    else
                        PutUInt(0x00000000);
            }
        }

        public List<byte> SetHairColorStorageSlot
        {
            set
            {
                PutByte(20);
                for (var i = 0; i < 20; i++)
                    if (i < value.Count)
                        PutByte(0x32);
                    else
                        PutByte(0xFF);
            }
        }
    }
}