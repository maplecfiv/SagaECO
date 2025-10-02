using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using SagaLib;

using SagaDB.Actor;

namespace SagaLogin.Packets.Server
{
    public class SSMG_SERVER_LST_STAER : Packet
    {
        public SSMG_SERVER_LST_STAER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x32;
        }
    }
}
