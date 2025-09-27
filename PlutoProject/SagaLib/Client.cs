using System.Collections.Generic;
using System.Net.Sockets;

namespace SagaLib
{
    public class Client
    {
        public NetIO NetIo;
        public uint SessionId;

        public Client()
        {
        }

        public Client(Socket mSock, Dictionary<ushort, Packet> mCommandTable)
        {
            NetIo = new NetIO(mSock, mCommandTable, this);
        }

        public virtual void OnConnect()
        {
        }

        public virtual void OnDisconnect()
        {
        }
    }
}