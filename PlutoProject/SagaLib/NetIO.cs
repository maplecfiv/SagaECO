using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SagaLib
{
    public class NetIO
    {
        public delegate void PacketEventArg(Packet p);

        public enum Mode
        {
            Server,
            Client
        }

        private readonly byte[] buffer = new byte[4];
        private readonly AsyncCallback callbackData;
        private readonly AsyncCallback callbackKeyExchange;
        private readonly AsyncCallback callbackSend;

        private readonly AsyncCallback callbackSize;

        /// <summary>
        ///     Command table contains the commands that need to be called when a
        ///     packet is received. Key will be the packet type
        /// </summary>
        private readonly Dictionary<ushort, Packet> commandTable;

        private readonly NetworkStream stream;
        private int alreadyReceived;

        /// <summary>
        ///     绑定的客户端
        /// </summary>
        public Client client;

        /// <summary>
        ///     加密算法实例
        /// </summary>
        public Encryption Crypt;

        private bool disconnecting;

        private int keyAlreadyReceived;

        private int lastSize;
        private int receivedBytes;
        private DateTime receiveStamp = DateTime.Now;
        private byte[] restBuffer;
        private int restBufferLength;
        private DateTime sendStamp = DateTime.Now;
        private int sentBytes;

        public Socket sock;

        internal int waitCounter;

        /// <summary>
        ///     是新的连接
        /// </summary>
        /// <param name="sock"></param>
        /// <param name="commandTable"></param>
        /// <param name="client"></param>
        public NetIO(Socket sock, Dictionary<ushort, Packet> commandTable, Client client)
        {
            this.sock = sock;
            stream = new NetworkStream(sock);
            this.commandTable = commandTable;
            this.client = client;
            Crypt = new Encryption();


            callbackSize = ReceiveSize;
            callbackData = ReceiveData;
            callbackKeyExchange = ReceiveKeyExchange;
            callbackSend = OnSent;
            // Use the static key untill the keys have been exchanged

            Disconnected = false;
        }

        /// <summary>
        ///     已断开的
        /// </summary>
        public bool Disconnected { get; private set; }

        /// <summary>
        ///     封包头的长度
        /// </summary>
        public ushort FirstLevelLength { get; set; } = 4;

        /// <summary>
        ///     当前平均使用的上行带宽，以字节为单位
        /// </summary>
        public int UpStreamBand { get; private set; }

        /// <summary>
        ///     当前平均使用的下行带宽，以字节为单位
        /// </summary>
        public int DownStreamBand { get; private set; }

        public event PacketEventArg OnReceivePacket;
        public event PacketEventArg OnSendPacket;

        private void StartPacketParsing()
        {
            if (sock.Connected)
            {
                client.OnConnect();
                try
                {
                    stream.BeginRead(buffer, 0, 4, callbackSize, null);
                }
                catch (Exception ex)
                {
                    Logger.GetLogger().Error(ex, ex.Message);
                    try //this could crash the gateway somehow,so better ignore the Exception
                    {
                        Disconnect();
                    }
                    catch (Exception exception)
                    {
                        Logger.GetLogger().Error(exception, null);
                    }

                    Logger.GetLogger().Warning("Invalid packet head from:" + sock.RemoteEndPoint, null);
                }
            }
            else
            {
                Disconnect();
            }
        }

        /// <summary>
        ///     设置当前网络层模式，客户端或服务器端
        /// </summary>
        /// <param name="mode">需要设定的模式</param>
        public void SetMode(Mode mode)
        {
            byte[] data;
            switch (mode)
            {
                case Mode.Server:
                    try
                    {
                        data = new byte[8];

                        keyAlreadyReceived = 8;

                        stream.BeginRead(data, 0, 8, callbackKeyExchange, data);
                    }
                    catch (Exception exception)
                    {
                        Logger.GetLogger().Error(exception, null);
                        ClientManager.EnterCriticalArea();
                        Disconnect();
                        ClientManager.LeaveCriticalArea();
                    }

                    break;
                case Mode.Client:
                    try
                    {
                        data = new byte[529];
                        int size;
                        if (sock.Available < 529)
                            size = sock.Available;
                        else
                            size = 529;

                        keyAlreadyReceived = size;
                        stream.BeginRead(data, 0, size, callbackKeyExchange, data);
                    }
                    catch (Exception exception)
                    {
                        Logger.GetLogger().Error(exception, null);
                        ClientManager.EnterCriticalArea();
                        Disconnect();
                        ClientManager.LeaveCriticalArea();
                    }

                    break;
            }
        }

        private void ReceiveKeyExchange(IAsyncResult ar)
        {
            try
            {
                if (Disconnected) return;
                if (!sock.Connected)
                {
                    ClientManager.EnterCriticalArea();
                    Disconnect();
                    ClientManager.LeaveCriticalArea();
                    return;
                }

                try
                {
                    stream.EndRead(ar);
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                    ClientManager.EnterCriticalArea();
                    Disconnect();
                    ClientManager.LeaveCriticalArea();
                    return;
                }

                var raw = (byte[])ar.AsyncState;

                if (keyAlreadyReceived < raw.Length)
                {
                    var left = raw.Length - keyAlreadyReceived;
                    if (left > 1024)
                        left = 1024;
                    if (left > sock.Available) left = sock.Available;
                    try
                    {
                        stream.BeginRead(raw, keyAlreadyReceived, left, callbackKeyExchange, raw);
                    }
                    catch (Exception exception)
                    {
                        Logger.GetLogger().Error(exception, null);
                        //Logger.getLogger().Error(ex, ex.Message);
                        ClientManager.EnterCriticalArea();
                        Disconnect();
                        ClientManager.LeaveCriticalArea();
                        return;
                    }

                    keyAlreadyReceived += left;
                    return;
                }

                if (raw.Length == 8)
                {
                    var p1 = new Packet(529);
                    p1.PutUInt(1, 4);
                    p1.PutByte(0x32, 8);
                    p1.PutUInt(0x100, 9);
                    Crypt.MakePrivateKey();
                    var bufstring = Conversions.bytes2HexString(Encryption.Module.getBytes());
                    p1.PutBytes(Encoding.ASCII.GetBytes(bufstring.ToLower()), 13);
                    p1.PutUInt(0x100, 269);
                    bufstring = Conversions.bytes2HexString(Crypt.GetKeyExchangeBytes());
                    p1.PutBytes(Encoding.ASCII.GetBytes(bufstring), 273);
                    SendPacket(p1, true, true);
                    try
                    {
                        var data = new byte[260];
                        int size;
                        if (sock.Available < 260)
                            size = sock.Available;
                        else
                            size = 260;

                        keyAlreadyReceived = size;
                        stream.BeginRead(data, 0, size, callbackKeyExchange, data);
                    }
                    catch (Exception exception)
                    {
                        Logger.GetLogger().Error(exception, null);
                        ClientManager.EnterCriticalArea();
                        Disconnect();
                        ClientManager.LeaveCriticalArea();
                    }
                }
                else if (raw.Length == 260)
                {
                    var p1 = new Packet();
                    p1.data = raw;
                    var keyBuf = p1.GetBytes(256, 4);
                    Crypt.MakeAESKey(Encoding.ASCII.GetString(keyBuf));
                    StartPacketParsing();
                }
                else if (raw.Length == 529)
                {
                    var p1 = new Packet();
                    p1.data = raw;
                    var keyBuf = p1.GetBytes(256, 273);
                    Crypt.MakePrivateKey();
                    var p2 = new Packet(260);
                    p2.PutUInt(0x100, 0);
                    var bufstring = Conversions.bytes2HexString(Crypt.GetKeyExchangeBytes());
                    p2.PutBytes(Encoding.ASCII.GetBytes(bufstring), 4);
                    SendPacket(p2, true, true);
                    Crypt.MakeAESKey(Encoding.ASCII.GetString(keyBuf));
                    StartPacketParsing();
                }
            }
            catch (Exception exception)
            {
                Logger.GetLogger().Error(exception, null);
                Disconnect();
                //Logger.getLogger().Error(ex, ex.Message);
            }
        }

        /// <summary>
        ///     断开连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (Disconnected) return;
                Disconnected = true;
                try
                {
                    if (!disconnecting)
                        client.OnDisconnect();
                }
                catch (Exception e)
                {
                    Logger.GetLogger().Error(e, e.Message);
                }

                disconnecting = true;
                try
                {
                    Logger.GetLogger().Information(sock.RemoteEndPoint + " disconnected", null);
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                }

                try
                {
                    stream.Close();
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                }

                try
                {
                    sock.Close();
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                }
            }
            catch (Exception e)
            {
                Logger.GetLogger().Error(e, e.Message);
                try
                {
                    stream.Close();
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                }

                //try { sock.Disconnect(true); }
                try
                {
                    sock.Close();
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                }
                //Logger.getLogger().Information(sock.RemoteEndPoint.ToString() + " disconnected", null);
            }
            //this.nlock.ReleaseWriterLock(); 
        }


        private void ReceiveSize(IAsyncResult ar)
        {
            try
            {
                if (Disconnected) return;

                if (buffer[0] == 0xFF && (buffer[1] == 0xFF) & (buffer[2] == 0xFF) && buffer[3] == 0xFF)
                {
                    // if the buffer is marked as "empty", there was an error during reading
                    // normally happens if the client disconnects
                    // note: this is required as sock.Connected still can be true, even the client
                    // is already disconnected
                    ClientManager.EnterCriticalArea();
                    Disconnect();
                    ClientManager.LeaveCriticalArea();
                    return;
                }
                /*if (!sock.Connected)
                {
                    ClientManager.EnterCriticalArea();
                    this.Disconnect();
                    ClientManager.LeaveCriticalArea();
                    return;
                }*/

                try
                {
                    stream.EndRead(ar);
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                    ClientManager.EnterCriticalArea();
                    Disconnect();
                    ClientManager.LeaveCriticalArea();
                    return;
                }

                Array.Reverse(buffer);
                var size = BitConverter.ToUInt32(buffer, 0) + 4;


                if (size < 4)
                {
                    Logger.GetLogger().Warning(sock.RemoteEndPoint + " error: packet size is < 4", null);
                    return;
                }

                /*while (sock.Available+4 < size)
                {
                    //Logger.getLogger().Warning(sock.RemoteEndPoint.ToString() + string.Format(" error: packet data is too short, should be {0:G}", size - 2), null);
                    Thread.Sleep(100);
                }*/

                var data = new byte[size + 4];

                // mark buffer as "empty"
                buffer[0] = 0xFF;
                buffer[1] = 0xFF;
                buffer[2] = 0xFF;
                buffer[3] = 0xFF;

                lastSize = (int)size;
                if (sock.Available < lastSize)
                    size = (uint)sock.Available;
                else
                    size = (uint)lastSize;
                if (size > 1024)
                {
                    size = 1024;
                    alreadyReceived = 1024;
                }
                else
                {
                    alreadyReceived = (int)size;
                }

                // Receive the data from the packet and call the receivedata function
                // The packet is stored in AsyncState
                //_logger.Debug("New packet with size " + p.size);
                try
                {
                    stream.BeginRead(data, 4, (int)size, callbackData, data);
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                    ClientManager.EnterCriticalArea();
                    Disconnect();
                    ClientManager.LeaveCriticalArea();
                }
            }

            catch (Exception e)
            {
                Logger.GetLogger().Error(e, e.Message);
            }
        }

        /// <summary>
        ///     分析封包
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveData(IAsyncResult ar)
        {
            try
            {
                if (Disconnected) return;
                /*if (!sock.Connected)
                {
                    ClientManager.EnterCriticalArea();
                    this.Disconnect();
                    ClientManager.LeaveCriticalArea();
                    return;
                }*/
                try
                {
                    stream.EndRead(ar);
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                    ClientManager.EnterCriticalArea();
                    Disconnect();
                    ClientManager.LeaveCriticalArea();
                    return;
                }

                var raw = (byte[])ar.AsyncState;
                if (alreadyReceived < lastSize && lastSize > 0)
                {
                    var left = lastSize - alreadyReceived;
                    if (left > 1024)
                        left = 1024;
                    if (sock.Available == 0)
                    {
                        waitCounter = 0;
                        while (sock.Available == 0)
                        {
                            if (waitCounter > 300)
                            {
                                Logger.GetLogger().Warning("Receive Timeout for client:" + client);
                                ClientManager.EnterCriticalArea();
                                Disconnect();
                                ClientManager.LeaveCriticalArea();
                                return;
                            }

                            Thread.Sleep(100);
                            waitCounter++;
                        }
                    }

                    if (left > sock.Available) left = sock.Available;
                    alreadyReceived += left;
                    try
                    {
                        stream.BeginRead(raw, 4 + alreadyReceived - left, left, callbackData, raw);
                    }
                    catch (Exception ex)
                    {
                        Logger.GetLogger().Error(ex, ex.Message);
                        ClientManager.EnterCriticalArea();
                        Disconnect();
                        ClientManager.LeaveCriticalArea();
                    }

                    return;
                }

                var a = new Packet();
                a.data = raw;
                raw = Crypt.Decrypt(raw, 8);
                var now = DateTime.Now;

                receivedBytes += raw.Length;
                if ((now - receiveStamp).TotalSeconds > 10)
                {
                    DownStreamBand = (int)(receivedBytes / (now - receiveStamp).TotalSeconds);
                    receivedBytes = 0;
                    receiveStamp = now;
                }

                var p = new Packet();
                p.data = raw;
                var length = p.GetUInt(4);
                uint offset = 0;
                if (length > 0 && length < 1024000)
                    while (offset < length)
                    {
                        uint size;
                        if (restBuffer != null && restBufferLength > 0)
                        {
                            var p3 = new Packet((uint)(restBufferLength + restBuffer.Length));
                            p3.PutBytes(restBuffer, 0);
                            p3.PutBytes(p.GetBytes((ushort)restBufferLength, (ushort)(8 + offset)));
                            offset += (uint)restBufferLength;
                            restBuffer = null;
                            restBufferLength = 0;
                            ProcessPacket(p3);
                        }

                        if (FirstLevelLength == 4)
                            size = p.GetUInt((ushort)(8 + offset));
                        else
                            size = p.GetUShort((ushort)(8 + offset));

                        offset += FirstLevelLength;
                        if (size + offset > length)
                        {
                            restBuffer = new byte[length - offset];
                            Array.Copy(p.data, offset + 8, restBuffer, 0, restBuffer.Length);
                            restBufferLength = (int)(size - restBuffer.Length);
                            break;
                        }

                        var p2 = new Packet();
                        p2.data = p.GetBytes((ushort)size, (ushort)(8 + offset));
                        offset += size;
                        ProcessPacket(p2);
                    }

                try
                {
                    stream.BeginRead(buffer, 0, 4, callbackSize, null);
                }
                catch (Exception exception)
                {
                    Logger.GetLogger().Error(exception, null);
                    ClientManager.EnterCriticalArea();
                    Disconnect();
                    ClientManager.LeaveCriticalArea();
                }
            }
            catch (Exception e)
            {
                Logger.GetLogger().Error(e, e.Message);
            }
        }

        /// <summary>
        ///     当拆分完封包后呼叫，调用此方法后将自动根据封包分派函数交由事先指定的处理函数处理该封包
        /// </summary>
        /// <param name="p">需要处理的封包</param>
        private void ProcessPacket(Packet p)
        {
            if (p.data.Length < 2) return;
            ClientManager.AddThread(
                $"PacketParser({Thread.CurrentThread.ManagedThreadId}),Opcode:0x{p.ID:X4}",
                Thread.CurrentThread);
            Packet command;
            commandTable.TryGetValue(p.ID, out command);
            if (command != null)
            {
                var p1 = command.New();
                p1.data = p.data;
                p1.size = (ushort)p.data.Length;
                ClientManager.EnterCriticalArea();
                try
                {
                    p1.Parse(client);
                }
                catch (Exception ex)
                {
                    Logger.GetLogger().Error(ex, ex.Message);
                }

                ClientManager.LeaveCriticalArea();
                if (OnSendPacket != null)
                    OnReceivePacket(p1);
            }
            else
            {
                if (commandTable.ContainsKey(0xFFFF))
                {
                    var p1 = commandTable[0xFFFF].New();
                    p1.data = p.data;
                    p1.size = (ushort)p.data.Length;
                    ClientManager.EnterCriticalArea();
                    try
                    {
                        p1.Parse(client);
                    }
                    catch (Exception ex)
                    {
                        Logger.GetLogger().Error(ex, ex.Message);
                    }

                    ClientManager.LeaveCriticalArea();
                }
                else
                {
                    Logger.ShowDebug(string.Format("Unknown Packet:0x{0:X4}\r\n       Data:{1}", p.ID, DumpData(p)),
                        Logger.CurrentLogger);
                }
            }

            ClientManager.RemoveThread(string.Format("PacketParser({0}),Opcode:0x{1:X4}",
                Thread.CurrentThread.ManagedThreadId, p.ID));
        }

        public string DumpData(Packet p)
        {
            var tmp2 = "";
            for (var i = 0; i < p.data.Length; i++)
            {
                tmp2 += string.Format("{0:X2} ", p.data[i]);
                if ((i + 1) % 16 == 0 && i != 0) tmp2 += "\r\n            ";
            }

            return tmp2;
        }

        /// <summary>
        ///     发送封包
        /// </summary>
        /// <param name="p">需要发送的封包</param>
        /// <param name="nolength">Unknow是干嘛的233</param>
        /// <param name="noWarper">是否不需要封装封包头，仅用于交换密钥，其余时候不建议使用</param>
        public void SendPacket(Packet p, bool nolength, bool noWarper)
        {
            //if (sendCounter >= 50)
            //    Logger.ShowDebug("Recurssion over 50 times!", Logger.defaultlogger);
            //Debug.Assert(sendCounter < 50, "Recurssion over 50 times!");
            //sendCounter++;
            if (Disconnected)
                return;
            if (OnSendPacket != null)
                OnSendPacket(p);
            if (!noWarper)
            {
                var buf = new byte[p.data.Length + FirstLevelLength];
                Array.Copy(p.data, 0, buf, FirstLevelLength, p.data.Length);
                p.data = buf;
                if (FirstLevelLength == 4)
                    p.SetLength();
                else
                    p.PutUShort((ushort)(p.data.Length - 2), 0);
                buf = new byte[p.data.Length + 4];
                Array.Copy(p.data, 0, buf, 4, p.data.Length);
                p.data = buf;
                p.SetLength();
                buf = new byte[p.data.Length + 4];
                Array.Copy(p.data, 0, buf, 4, p.data.Length);
                p.data = buf;
            }

            if (!nolength)
            {
                var mod = 16 - (p.data.Length - 8) % 16;
                if (mod != 0)
                {
                    var buf = new byte[p.data.Length + mod];
                    Array.Copy(p.data, 0, buf, 0, p.data.Length);
                    p.data = buf;
                }

                p.PutUInt((uint)(p.data.Length - 8), 0);
            }

            sentBytes += p.data.Length;
            var now = DateTime.Now;
            if ((now - sendStamp).TotalSeconds > 10)
            {
                UpStreamBand = (int)(sentBytes / (now - sendStamp).TotalSeconds);
                sentBytes = 0;
                sendStamp = now;
            }

            try
            {
                byte[] data;
                data = Crypt.Encrypt(p.data, 8);
                stream.BeginWrite(data, 0, data.Length, callbackSend, null);
            }
            catch (Exception ex)
            {
                if (client != null)
                {
                    Logger.GetLogger().Error(ex, ex.Message);
                    Disconnect();
                    client = null;
                }
            }
            //sendCounter--;
        }

        private void OnSent(IAsyncResult ar)
        {
            try
            {
                stream.EndWrite(ar);
            }
            catch
            {
            }
        }

        public void SendPacket(Packet p, bool noWarper)
        {
            SendPacket(p, false, noWarper);
        }

        public void SendPacket(Packet p)
        {
            SendPacket(p, false);
        }
    }
}