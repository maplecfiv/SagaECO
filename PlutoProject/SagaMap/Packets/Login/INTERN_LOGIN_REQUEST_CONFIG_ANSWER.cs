using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SagaDB.Actor;
using SagaDB.Config;
using SagaLib;
using SagaMap.Network.LoginServer;

namespace SagaMap.Packets.Login {
//     public class INTERN_LOGIN_REQUEST_CONFIG_ANSWER : Packet
//     {
//         public INTERN_LOGIN_REQUEST_CONFIG_ANSWER()
//         {
//             offset = 2;
//         }
//
//         public bool AuthOK => GetByte(2) == 1;
//
//         public Dictionary<SagaLib.PcRace, StartupSetting> StartupSetting
//         {
//             get
//             {
//                 var len = GetUInt(3);
//                 byte[] buf;
//                 buf = GetBytes((ushort)len, 7);
//                 var ms = new MemoryStream(buf);
//                 Dictionary<SagaLib.PcRace, StartupSetting> list;
// #pragma warning disable SYSLIB0011
//                 var bf = new BinaryFormatter();
//                 list = (Dictionary<SagaLib.PcRace, StartupSetting>)bf.Deserialize(ms);
//                 return list;
//             }
//         }
//
//         public override Packet New()
//         {
//             return new INTERN_LOGIN_REQUEST_CONFIG_ANSWER();
//         }
//
//         public override void Parse(SagaLib.Client client)
//         {
//             ((LoginSession)client).OnGetConfig(this);
//         }
//     }
}