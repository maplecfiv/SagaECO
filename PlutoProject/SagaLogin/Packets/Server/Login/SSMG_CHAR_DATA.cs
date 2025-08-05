using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SagaDB.Actor;
using SagaLib;
using Version = SagaLib.Version;

namespace SagaLogin.Packets.Server.Login
{
    public class SSMG_CHAR_DATA : Packet
    {
        public SSMG_CHAR_DATA()
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                data = new byte[156];
            else if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                data = new byte[131];
            else if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                data = new byte[113];
            else
                data = new byte[86];
            offset = 2;
            ID = 0x28;
        }

        public List<ActorPC> Chars
        {
            set
            {
                if (value.Count == 0)
                {
                    SetName("", 0);
                    SetRace(0, 0);
                    if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                        SetForm(0, 0);
                    SetGender(0, 0);
                    SetHairStyle(0, 0);
                    SetHairColor(0, 0);
                    SetWig(0, 0);
                    SetIfExist(false, 0);
                    SetFace(0, 0);
                    SetRebirthFlag(0, 0);
                    SetTail(0, 0);
                    SetWing(0, 0);
                    SetWingColor(0, 0);
                    SetJob(0, 0);
                    SetMap(0, 0);
                    SetLv(0, 0);
                    SetJob1(0, 0);
                    SetQuestRemaining(0, 0);
                    SetJob2X(0, 0);
                    SetJob2T(0, 0);
                    SetJob3(0, 0);
                }

                int count;
                if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                    count = 4;
                else
                    count = 3;
                for (var i = 0; i < count; i++)
                {
                    var pcs =
                        from p in value
                        where p.Slot == i
                        select p;
                    if (pcs.Count() == 0)
                        continue;
                    var pc = pcs.First();
                    SetName(pc.Name, (ushort)i);
                    SetRace((byte)pc.Race, (ushort)i);
                    if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                        SetForm((byte)pc.Form, (ushort)i);
                    SetGender((byte)pc.Gender, (ushort)i);
                    SetHairStyle(pc.HairStyle, (ushort)i);
                    SetHairColor(pc.HairColor, (ushort)i);
                    SetWig(pc.Wig, (ushort)i);
                    SetIfExist(true, (ushort)i);
                    SetFace(pc.Face, (ushort)i);
                    byte rblv = 0x00;
                    if (pc.Rebirth)
                        rblv = 0x64;
                    else
                        rblv = 0x00;
                    SetRebirthFlag(rblv, (ushort)i);
                    SetTail(pc.TailStyle, (ushort)i);
                    SetWing(pc.WingStyle, (ushort)i);
                    SetWingColor(pc.WingColor, (ushort)i);
                    SetJob((byte)pc.Job, (ushort)i);
                    SetMap(pc.MapID, (ushort)i);
                    SetLv(pc.Level, (ushort)i);
                    SetJob1(pc.JobLevel1, (ushort)i);
                    SetQuestRemaining(pc.QuestRemaining, (ushort)i);
                    SetJob2X(pc.JobLevel2X, (ushort)i);
                    SetJob2T(pc.JobLevel2T, (ushort)i);
                    SetJob3(pc.JobLevel3, (ushort)i); //Job3 level
                }
            }
        }

        private void SetName(string name, int index)
        {
            var current = 0;
            var offset = 3;
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                PutByte(4, 2);
            else
                PutByte(3, 2);
            while (current < index)
            {
                byte size;
                size = GetByte((ushort)offset);
                offset = offset + size + 1;
                current++;
            }

            byte[] buf;
            byte[] buff;
            buf = Encoding.UTF8.GetBytes(name);
            PutByte((byte)buf.Length, (ushort)offset);
            offset++;
            buff = new byte[data.Length + buf.Length];
            Array.Copy(data, 0, buff, 0, offset);
            Array.Copy(data, offset, buff, offset + buf.Length, data.Length - offset);
            data = buff;
            PutBytes(buf, (ushort)offset);
            //this.SetUnkown();
        }

        private ushort GetDataOffset()
        {
            ushort offset = 3;
            int count;
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                count = 4;
            else
                count = 3;
            for (var i = 0; i < count; i++)
            {
                byte size;
                size = GetByte(offset);
                offset = (ushort)(offset + size + 1);
            }

            return offset;
        }

        private void SetRace(byte race, ushort index)
        {
            var offset = GetDataOffset();
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                PutByte(4, offset);
            else
                PutByte(3, offset);
            PutByte(race, (ushort)(offset + index + 1));
        }

        private void SetForm(byte form, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = (ushort)(GetDataOffset() + 5);
                PutByte(4, offset);
                PutByte(form, (ushort)(offset + index + 1));
            }
        }

        private void SetGender(byte gender, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = (ushort)(GetDataOffset() + 10);
                PutByte(4, offset);
                PutByte(gender, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 4);
                PutByte(3, offset);
                PutByte(gender, (ushort)(offset + index + 1));
            }
        }

        private void SetHairStyle(ushort hair, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = (ushort)(GetDataOffset() + 15);
                PutByte(4, offset);
                if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                    PutUShort(hair, (ushort)(offset + index * 2 + 1));
                else
                    PutUShort(hair, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 8);
                PutByte(3, offset);
                PutUShort(hair, (ushort)(offset + index + 1));
            }
        }

        private void SetHairColor(byte color, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 24)
                    : (ushort)(GetDataOffset() + 20);
                PutByte(4, offset);
                PutByte(color, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 12);
                PutByte(3, offset);
                PutByte(color, (ushort)(offset + index + 1));
            }
        }

        //Default by 0xff
        private void SetWig(ushort wig, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 29)
                    : (ushort)(GetDataOffset() + 25);
                PutByte(4, offset);
                if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                    PutUShort(wig, (ushort)(offset + index * 2 + 1));
                else
                    PutByte((byte)wig, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 16);
                PutByte(3, offset);
                PutByte((byte)wig, (ushort)(offset + index + 1));
            }
        }

        private void SetIfExist(bool exist, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 38)
                    : (ushort)(GetDataOffset() + 30);
                PutByte(4, offset);
                if (exist)
                    PutByte(0xff, (ushort)(offset + index + 1));
                else
                    PutByte(0x00, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 20);
                PutByte(3, offset);
                if (exist)
                    PutByte(0xff, (ushort)(offset + index + 1));
                else
                    PutByte(0x00, (ushort)(offset + index + 1));
            }
        }

        private void SetFace(ushort face, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 43)
                    : (ushort)(GetDataOffset() + 35);
                PutByte(4, offset);
                PutUShort(face, (ushort)(offset + index * 2 + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 24);
                PutByte(3, offset);
                PutUShort(face, (ushort)(offset + index + 1));
            }
        }

        private void SetRebirthFlag(byte unknown, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga11)
            {
                var offset = (ushort)(GetDataOffset() + 53);
                PutByte(4, offset);
                PutByte(unknown, (ushort)(offset + index + 1));
            }
        }

        private void SetTail(byte tail, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
            {
                var offset = (ushort)(GetDataOffset() + 53);
                PutByte(4, offset);
                PutByte(tail, (ushort)(offset + index + 1));
            }
        }

        private void SetWing(byte wing, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
            {
                var offset = (ushort)(GetDataOffset() + 58);
                PutByte(4, offset);
                PutByte(wing, (ushort)(offset + index + 1));
            }
        }

        private void SetWingColor(byte wingcolor, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
            {
                var offset = (ushort)(GetDataOffset() + 63);
                PutByte(4, offset);
                PutByte(wingcolor, (ushort)(offset + index + 1));
            }
        }

        private void SetUnkown()
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 53)
                    : (ushort)(GetDataOffset() + 40);
                PutByte(4, offset);
                PutUInt(0, (ushort)(offset + 1));
                PutByte(4, (ushort)(offset + 5));
                PutUInt(0, (ushort)(offset + 6));
                PutByte(4, (ushort)(offset + 10));
                PutUInt(0, (ushort)(offset + 11));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 28);
                PutUInt(0x03000000, offset);
                PutUInt(0x03000000, (ushort)(offset + 4));
                PutUInt(0x03000000, (ushort)(offset + 8));
            }
        }

        private void SetJob(byte job, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 68)
                    : (ushort)(GetDataOffset() + 55);
                PutByte(4, offset);
                PutByte(job, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 40);
                PutByte(3, offset);
                PutByte(job, (ushort)(offset + index + 1));
            }
        }

        private void SetMap(uint map, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 73)
                    : (ushort)(GetDataOffset() + 60);
                PutByte(4, offset);
                PutUInt(map, (ushort)(offset + index * 4 + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 44);
                PutByte(3, offset);
                PutUInt(map, (ushort)(offset + index * 4 + 1));
            }
        }

        private void SetLv(byte lv, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 90)
                    : (ushort)(GetDataOffset() + 77);
                PutByte(4, offset);
                PutByte(lv, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 57);
                PutByte(3, offset);
                PutByte(lv, (ushort)(offset + index + 1));
            }
        }

        private void SetJob1(byte job1, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 95)
                    : (ushort)(GetDataOffset() + 82);
                PutByte(4, offset);
                PutByte(job1, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 61);
                PutByte(3, offset);
                PutByte(job1, (ushort)(offset + index + 1));
            }
        }

        private void SetQuestRemaining(ushort quest, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 100)
                    : (ushort)(GetDataOffset() + 87);
                PutByte(4, offset);
                PutUShort(quest, (ushort)(offset + index * 2 + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 65);
                PutByte(3, offset);
                PutUShort(quest, (ushort)(offset + index * 2 + 1));
            }
        }

        private void SetJob2X(byte job2x, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 109)
                    : (ushort)(GetDataOffset() + 96);
                PutByte(4, offset);
                PutByte(job2x, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 72);
                PutByte(3, offset);
                PutByte(job2x, (ushort)(offset + index + 1));
            }
        }

        private void SetJob2T(byte job2t, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga10)
            {
                var offset = Configuration.Configuration.Instance.Version >= Version.Saga11
                    ? (ushort)(GetDataOffset() + 114)
                    : (ushort)(GetDataOffset() + 101);
                PutByte(4, offset);
                PutByte(job2t, (ushort)(offset + index + 1));
            }
            else
            {
                var offset = (ushort)(GetDataOffset() + 76);
                PutByte(3, offset);
                PutByte(job2t, (ushort)(offset + index + 1));
            }
        }

        private void SetJob3(byte job3, ushort index)
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga11)
            {
                var offset = (ushort)(GetDataOffset() + 119);
                PutByte(4, offset);
                PutByte(job3, (ushort)(offset + index + 1));
            }
        }
    }
}