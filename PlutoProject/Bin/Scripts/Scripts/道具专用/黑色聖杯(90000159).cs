using System;
using System.Collections.Generic;
using System.Text;

using SagaDB.Actor;
using SagaMap.Scripting;

using SagaLib;
using SagaScript.Chinese.Enums;
namespace SagaScript.黒の聖杯
{
    public class S90000159 : Event
    {
        public S90000159()
        {
            this.EventID = 90000159;
        }

        public override void OnEvent(ActorPC pc)
        {
        	BitMask<Neko_09> Neko_09_mask = new BitMask<Neko_09>(pc.CMask["Neko_09"]);
        	if (Neko_09_mask.Test(Neko_09.黑暗圣杯入手))
            {
                Say(pc, 0, 0, ""+ pc.Name +"は$R;" +
                "天高く聖杯を掲げた！$R;", "");
                ShowEffect(pc, 5019);
                Wait(pc, 2970);

                if (Neko_09_mask.Test(Neko_09.获得灵魂碎片_01) &&
                    Neko_09_mask.Test(Neko_09.获得灵魂碎片_02) &&
                    Neko_09_mask.Test(Neko_09.获得灵魂碎片_03) &&
                    Neko_09_mask.Test(Neko_09.获得灵魂碎片_04))
                {
                    Say(pc, 0, 0, "聖杯に魂水が注がれた！$R;", "");

                    Say(pc, 0, 0, "聖杯は、魂水で満ち溢れている。$R;" +
                    "$Rこれ以上、魂水を$R;" +
                    "貯めることは出来ないようだ。$R;", "");

                }
                else if (pc.MapID == 12058000 &&
                    !Neko_09_mask.Test(Neko_09.获得灵魂碎片_01))
                {
                    Neko_09_mask.SetValue(Neko_09.获得灵魂碎片_01, true);
                    获得灵魂碎片(pc);
                }
                else if (pc.MapID == 10023000 &&
                    !Neko_09_mask.Test(Neko_09.获得灵魂碎片_02))
                {
                    Neko_09_mask.SetValue(Neko_09.获得灵魂碎片_02, true);
                    获得灵魂碎片(pc);
                }
                else if (pc.MapID == pc.CInt["NEKO_09_MAPs_01"] &&
                    !Neko_09_mask.Test(Neko_09.获得灵魂碎片_03))
                {
                    Neko_09_mask.SetValue(Neko_09.获得灵魂碎片_03, true);
                    获得灵魂碎片(pc);
                }
                else if (pc.MapID == pc.CInt["NEKO_09_MAPs_02"] &&
                    !Neko_09_mask.Test(Neko_09.获得灵魂碎片_04))
                {
                    Neko_09_mask.SetValue(Neko_09.获得灵魂碎片_04, true);
                    获得灵魂碎片(pc);
                }
                else
                {

                    Say(pc, 0, 0, "……しかし、何も起こらなかった！$R;" +
                    "$Rこの場所に$R;" +
                    "ネコマタ魂はいないようだ。$R;", "");
                }
        	}
        }

        void 获得灵魂碎片(ActorPC pc)
        {
            BitMask<Neko_09> Neko_09_mask = new BitMask<Neko_09>(pc.CMask["Neko_09"]);

            ShowEffect(pc, 4145);
            Wait(pc, 1485);
            Wait(pc, 990);
            Wait(pc, 330);

            if (Neko_09_mask.Test(Neko_09.获得灵魂碎片_01) &&
                Neko_09_mask.Test(Neko_09.获得灵魂碎片_02) &&
                Neko_09_mask.Test(Neko_09.获得灵魂碎片_03) &&
                Neko_09_mask.Test(Neko_09.获得灵魂碎片_04))
            {
                Say(pc, 0, 0, "聖杯に魂水が注がれた！$R;", "");

                Say(pc, 0, 0, "聖杯は、魂水で満ち溢れている。$R;" +
                "$Rこれ以上、魂水を$R;" +
                "貯めることは出来ないようだ。$R;", "");

            }
            else
            {
                Say(pc, 0, 0, "聖杯に魂水が注がれた！$R;", "");

                Say(pc, 0, 0, "まだ、聖杯に魂水が入りそうだ。$R;" +
                "別の場所も探そう！$R;", "");
            }
        }
    }
}