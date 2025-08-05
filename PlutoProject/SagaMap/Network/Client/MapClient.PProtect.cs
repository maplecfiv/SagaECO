using SagaDB.PProtect;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        private PProtect pp;

        public void OnPProtectCreatedIniti(CSMG_PPROTECT_CREATED_INITI p)
        {
            var p1 = new SSMG_PPROTECT_CREATED_INITI();
            netIO.SendPacket(p1);
        }

        /// <summary>
        ///     打开列表
        /// </summary>
        public void OnPProtectListOpen(CSMG_PPROTECT_LIST_OPEN p)
        {
            ushort max = 0;
            var pp = PProtectManager.Instance.GetPProtectsOfPage(p.Page, out max, p.Search);
            //pp.Add(new PProtect { ID = 0xffff, Leader = this.chara, Name = "123", MaxMember = 5, Message = "233", TaskID = 12000 });
            //pp.Add(new PProtect { ID = 0xffee, Leader = this.chara, Name = "987", MaxMember = 1, Message = "000", TaskID = 12000 });
            var p1 = new SSMG_PPROTECT_LIST();

            p1.PageMax = max;
            if (p.Page < max)
                p1.Page = p.Page;
            else
                p1.Page = max;
            p1.List = pp;
            var ss = p1.DumpData();
            netIO.SendPacket(p1);
        }

        /// <summary>
        ///     创建招募
        /// </summary>
        public void OnPProtectCreated(CSMG_PPROTECT_CREATED_INFO p)
        {
            pp = new PProtect();
            pp.Leader = Character;
            pp.Members.Add(pp.Leader);
            pp.Name = p.name;
            pp.Password = p.password;
            pp.Message = p.message;
            pp.MaxMember = p.maxMember;
            pp.TaskID = p.taskID;

            PProtectManager.Instance.ADD(pp);

            var p1 = new SSMG_PPROTECT_CREATED_RESULT();

            netIO.SendPacket(p1);

            var p2 = new SSMG_PPROTECT_CHAT_INFO();
            p2.SetData(Character, 0, 0, 0, 0, 0, 0);
            netIO.SendPacket(p2);
        }


        /// <summary>
        ///     修改招募信息
        /// </summary>
        public void OnPProtectCreatedRevise(CSMG_PPROTECT_CREATED_REVISE p)
        {
            if (pp == null)
                return;
            pp.Name = p.name;
            pp.Password = p.password;
            pp.Message = p.message;
            pp.MaxMember = p.maxMember;
            pp.TaskID = p.taskID;


            var p1 = new SSMG_PPROTECT_CREATED_REVISE_RESULT();
            p1.SetData(p.name, p.message, p.taskID, p.maxMember, 0, 0);
            //string ss = p1.DumpData();
            netIO.SendPacket(p1);

            for (var i = 0; i < pp.Members.Count; i++)
            {
                var client = MapClientManager.Instance.FindClient(pp.Members[i]);
                if (client != null && client.Character != Character)
                {
                    p1 = new SSMG_PPROTECT_CREATED_REVISE_RESULT();
                    p1.SetData(p.name, p.message, p.taskID, p.maxMember, 0, 0);
                    client.netIO.SendPacket(p1);
                }
            }
        }

        /// <summary>
        ///     加入
        /// </summary>
        public void OnPProtectADD(CSMG_PPROTECT_ADD p)
        {
            addPProtect(p.PPID, p.Password);
        }

        public void OnPProtectADD1(CSMG_PPROTECT_ADD_1 p)
        {
            var ppt = PProtectManager.Instance.GetPProtect(p.PPID);
            if (ppt != null)
            {
                var p1 = new SSMG_PPROTECT_CREATED_ADD_RESULT_1();
                p1.List = ppt.Members;
                netIO.SendPacket(p1);
            }
        }

        private void addPProtect(uint ppid, string password)
        {
            if (pp != null) OnPProtectCreatedOut(null);

            var ppt = PProtectManager.Instance.GetPProtect(ppid);
            if (ppt != null)
            {
                if (!ppt.IsPassword || ppt.Password == password)
                {
                    SSMG_PPROTECT_CHAT_INFO p2;
                    for (var i = 0; i < ppt.Members.Count; i++)
                    {
                        var client = MapClientManager.Instance.FindClient(ppt.Members[i]);
                        if (client != null)
                        {
                            p2 = new SSMG_PPROTECT_CHAT_INFO();
                            p2.SetData(Character, (byte)ppt.Members.Count, 0, 0, 0, 0, 0); //
                            client.netIO.SendPacket(p2);

                            var p3 = new SSMG_PPROTECT_CHAT_INFO();
                            p3.SetData(ppt.Members[i], (byte)i, 0, 0, 0, 0, 0);
                            netIO.SendPacket(p3);
                        }
                    }

                    p2 = new SSMG_PPROTECT_CHAT_INFO();
                    p2.SetData(Character, (byte)ppt.Members.Count, 0, 0, 0, 0, 0); //

                    netIO.SendPacket(p2);
                    ppt.Members.Add(Character);
                    var p1 = new SSMG_PPROTECT_CREATED_ADD_RESULT();
                    p1.SetData(ppt.Name, password, 0, 1, 0);
                    netIO.SendPacket(p1);
                    pp = ppt;
                }
                else
                {
                    var p1 = new SSMG_PPROTECT_CREATED_ADD_RESULT();
                    p1.SetData("", "", 0xFB, 0, 0xFF);
                    netIO.SendPacket(p1);
                    //密码错误
                }
            }
        }

        /// <summary>
        ///     修改状态
        /// </summary>
        public void OnPProtectReady(CSMG_PPROTECT_READY p)
        {
            if (pp != null && pp.Leader == Character)
                //队长进入房间操作
                return;
            var p1 = new SSMG_PPROTECT_READY_RESULT();
            SSMG_PPROTECT_READY p2;
            switch (p.State)
            {
                case 1: //准备
                    if (true)
                    {
                        //条件符合
                        p1.Code = 1;

                        for (var i = 0; i < pp.Members.Count; i++)
                        {
                            var client = MapClientManager.Instance.FindClient(pp.Members[i]);
                            if (client != null && client.Character != Character)
                            {
                                p2 = new SSMG_PPROTECT_READY();
                                p2.Index = (byte)pp.Members.IndexOf(Character);
                                p2.Code = 1;
                                client.netIO.SendPacket(p2);
                            }
                        }
                    }
                    else
                    {
                        //条件不符
                        p1.Code = 0xFE;
                    }

                    break;
                case 0: //取消
                {
                    for (var i = 0; i < pp.Members.Count; i++)
                    {
                        var client = MapClientManager.Instance.FindClient(pp.Members[i]);
                        if (client != null && client.Character != Character)
                        {
                            p2 = new SSMG_PPROTECT_READY();
                            p2.Index = (byte)pp.Members.IndexOf(Character);
                            p2.Code = 0;
                            client.netIO.SendPacket(p2);
                        }
                    }
                }
                    p1.Code = 0;
                    break;
            }

            netIO.SendPacket(p1);
        }


        /// <summary>
        ///     退出招募
        /// </summary>
        public void OnPProtectCreatedOut(CSMG_PPROTECT_CREATED_OUT p)
        {
            if (pp == null)
                return;
            if (Character == pp.Leader)
            {
                //招募人退出
                PProtectManager.Instance.Remove(pp.ID);

                for (var i = 0; i < pp.Members.Count; i++)
                {
                    var client = MapClientManager.Instance.FindClient(pp.Members[i]);
                    if (client != null)
                    {
                        var p1 = new SSMG_PPROTECT_CREATED_OUT_RESULT();
                        p1.SetName(pp.Name);
                        client.netIO.SendPacket(p1);
                    }
                }

                pp.Members.Clear();
            }
            else
            {
                //成员退出
                for (var i = 0; i < pp.Members.Count; i++)
                {
                    var client = MapClientManager.Instance.FindClient(pp.Members[i]);
                    if (client != null && client.Character != Character)
                    {
                        var p1 = new SSMG_PPROTECT_CREATED_OUT();
                        //int iii = pp.Members.IndexOf(this.Character);
                        p1.Index = (byte)pp.Members.IndexOf(Character);
                        client.netIO.SendPacket(p1);
                    }
                }

                var p2 = new SSMG_PPROTECT_CREATED_OUT_RESULT_1();
                p2.SetName(pp.Name);
                netIO.SendPacket(p2);

                pp.Members.Remove(Character);
            }

            pp = null;
        }
    }
}