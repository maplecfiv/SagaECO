using System;
using System.Collections.Generic;
using System.Text;

using SagaDB.Actor;
using SagaMap.Scripting;
using System.Linq;
using System.Collections;
//���ڵ؈D:�³�(10024000) NPC������Ϣ:�y(11001172) X:132 Y:97
namespace SagaScript.M80060050
{
    public class S20010020 : Event
    {
        public S20010020()
        {
            this.EventID = 20010020;
        }

        public override void OnEvent(ActorPC pc)
        {
            PlaySound(pc, 2030, false, 100, 50);
            Say(pc, 20010020, 131, "����ECO֮����" +
                                    "$R�ռ����㹻�����ֶ����ҾͿ����������ˣ�$R;", "ңԶ���̾���");
            Say(pc, 20010020, 111, "��Ȼ������" +
                                    "$R����������̫һ����������$R;", "ңԶ���̾���");
            if (CountItem(pc, 22000103) >= 5)
            {
                Say(pc, 20010020, 111, "���������Ȥ���ҽ��ף�" +
                                    "$R̫���ˣ������ڲ���ȥ�״�½���ˣ�" +
                                     "��Щ���ҵĴ����$R;" +
                                     "������Щ������ʱ�ò���,�ɱ�˵�ҿ���Ŷ~$R;", "������");

                KujiTrade trade = new KujiTrade();
                List<string> arrname = trade.tradelist.Keys.ToList();
                arrname.Add("û��Ȥ");
                int lastoption = arrname.Count;
                int option = Select(pc, "��Ҫʲô", "", arrname.ToArray());
                if (option == arrname.Count)
                    return;
                string changenum = InputBox(pc, "Ҫ������?", InputType.Bank);
                int num = 0;
                if (!int.TryParse(changenum, out num))
                {
                    Say(pc, 20010020, 111, "�국!����������Ҫ������!;", "ңԶ���̾���");
                    return;
                }
                KujiTradeInfo info = trade.tradelist[arrname[option]];
                if (CountItem(pc, info.HeartID) < info.HeartNum * num)
                {
                    Say(pc, 20010020, 111, "�국!��Ѿ��������ô����!?!;", "ңԶ���̾���");
                    return;
                }
                PlaySound(pc, 2040, false, 100, 50);
                TakeItem(pc,info.HeartID,info.HeartNum);
                GiveItem(pc, info.KujiBoxID, info.KujiBoxNum);
                Say(pc, 20010020, 111, "$R�ݰ�~~~~~~~~~$R;", "ңԶ���̾���");
            }
            else
            {
                Say(pc, 20010020, 111, "$R�������Ȥ�������Ұɡ�$R;", "ңԶ���̾���");
            }
        }
    }
    public class KujiTradeInfo
    {
        private uint heartid;
        /// <summary>
        /// �ĵ�ID
        /// </summary>
        public uint HeartID
        {
            get { return heartid; }
            set { heartid = value; }
        }
        private byte heartnum;

        /// <summary>
        /// �ĵ�����
        /// </summary>
        public byte HeartNum
        {
            get { return heartnum; }
            set { heartnum = value; }
        }

        private uint kujiboxid;
        /// <summary>
        /// kuji���ӵ�ID
        /// </summary>
        public uint KujiBoxID
        {
            get { return kujiboxid; }
            set { kujiboxid = value; }
        }

        private byte kuijiboxnum;
        /// <summary>
        /// kuji���ӵ�����
        /// </summary>
        public byte KujiBoxNum
        {
            get { return kuijiboxnum; }
            set { kuijiboxnum = value; }
        }

        /// <summary>
        /// ��ʼ���һ�����
        /// </summary>
        /// <param name="heartid">�һ����ID</param>
        /// <param name="heartnum">����һ�������</param>
        /// <param name="kujiboxid">�һ�kuji���ӵ�ID</param>
        /// <param name="kujiboxnum">�һ�kuji���ӵ�����</param>
        public KujiTradeInfo(uint heartid, byte heartnum, uint kujiboxid, byte kujiboxnum)
        {
            this.heartid = heartid;
            this.heartnum = heartnum;
            this.kujiboxid = kujiboxid;
            this.kuijiboxnum = kujiboxnum;
        }
    }

    public class KujiTrade
    {

        public Dictionary<string, KujiTradeInfo> tradelist = new Dictionary<string, KujiTradeInfo>();
        public KujiTrade()
        {
            tradelist.Add("��?", new KujiTradeInfo(123, 1, 321, 1));
            tradelist.Add("123", new KujiTradeInfo(100, 1, 1005, 1));
            tradelist.Add("234", new KujiTradeInfo(100, 1, 1005, 1));
            tradelist.Add("345", new KujiTradeInfo(100, 1, 1005, 1));
        }

    }
}
