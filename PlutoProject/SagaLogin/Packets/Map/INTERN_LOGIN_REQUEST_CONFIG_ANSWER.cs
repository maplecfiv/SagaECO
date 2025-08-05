using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SagaDB.Actor;
using SagaDB.Config;
using SagaLib;

namespace SagaLogin.Packets.Map
{
    public class INTERN_LOGIN_REQUEST_CONFIG_ANSWER : Packet
    {
        public INTERN_LOGIN_REQUEST_CONFIG_ANSWER()
        {
            data = new byte[8];
            offset = 2;
            ID = 0xFFF2;
        }

        public bool AuthOK
        {
            set
            {
                if (value)
                    PutByte(1, 2);
                else
                    PutByte(0, 2);
            }
        }

        public Dictionary<PC_RACE, StartupSetting> StartupSetting
        {
            set
            {
                var ms = new MemoryStream();
                var bf = new BinaryFormatter();
                bf.Serialize(ms, value);
                ms.Flush();
                var buf = new byte[8 + ms.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutUInt((uint)ms.Length, 3);
                PutBytes(ms.ToArray(), 7);
            }
        }
    }
}