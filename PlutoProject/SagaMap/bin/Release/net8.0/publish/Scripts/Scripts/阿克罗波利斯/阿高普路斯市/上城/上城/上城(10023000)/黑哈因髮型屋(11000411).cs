using System;
using System.Collections.Generic;
using System.Text;

using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Scripting;
using System.Linq;
using SagaScript.Chinese.Enums;
using SagaLib;
//所在地圖:上城(10023000) NPC基本信息:黑哈因髮型屋(11000411) X:103 Y:74		JP:ヘルヘイム
namespace SagaScript.M10023000
{

    public class S11000411 : Event
    {

        public S11000411()
        {
            this.EventID = 11000411;
        }

        public override void OnEvent(ActorPC pc)
        {
            BitMask<Acropolisut_01> mask = new BitMask<Acropolisut_01>(pc.CMask["Acropolisut_01"]);

            List<uint> hairlist = gethairlist(pc);
            List<string> hair_name = new List<string>();
            List<uint> hairticket_id = new List<uint>();

            if (pc.Gender == PC_GENDER.FEMALE)
            {
                Say(pc, 11000115, 131, "很抱歉，$R;" +
                    "我對女生不感興趣…$R;" +
                    "請找別的髮型屋吧！", "黑哈因髮型屋");
                return;
            }


            if (pc.Marionette != null)
            {
                Say(pc, 11000115, 131, "請先解除木偶，$R;" +
                                       "然後再來找我吧！$R;"
                                       , "黑哈因髮型屋");
                return;
            }

            if (!mask.Test(Acropolisut_01.已經與尼貝諾特髮型屋進行第一次對話))
            {
                ニーベルング会話初回(pc);
                return;
            }

            Say(pc, 131, "哦！你好。$R;" +
                "歡迎光臨黑哈因髮型屋！$R;" +
                "$R你可以在這裡を$R;" +
                "改變你喜歡的髮型。$R;" +
                "$P你準備好改變你的髮型了嗎？$R;", "黑哈因髮型屋");

            if (hairlist.Count() >= 1)
            {

                Say(pc, 11000115, 131, "哦!$R;" +
                                       "$R竟然帶來了介紹書?$R;" +
                                       "$P這回子…$R;" +
                                       "是個大人物來的呢!$R;" +
                                       "$P我的身體跟我說、$R;" +
                                       "很想為你做個美好的髮型!$R;" +
                                       "$P如何呢?$R;" +
                                       "$R你要使用介紹書$R;" +
                                       "來改變你的髮型嗎$R;", "尼貝諾特髮型屋");


                hair_name.Add("不使用");

                foreach (uint v in hairlist)
                {
                    SagaDB.Item.Item.ItemData item = ItemFactory.Instance.Items[v];
                    hair_name.Add(item.name);
                    hairticket_id.Add(v);
                }

                string[] name_arr = hair_name.ToArray();
                uint[] id_arr = hairticket_id.ToArray();

                int selected = Select(pc, "要使用哪一個介紹書？", "", name_arr);

                if (selected <= 1)
                {
                    Say(pc, 11000115, 131, "好吧、$R;" +
                       "如果想改變髮型的話，隨時再來吧！$R;", "尼貝諾特髮型屋");
                    return;
                }
                else
                {
                    uint seleced_id = id_arr[selected - 2];
                    var res = from hair in HairFactory.Instance.Hairs where ((CountItem(pc, hair.ItemID) > 0) && (hair.ItemID == seleced_id) && (hair.Gender == 0 || hair.Gender == 2)) select hair;

                    List<Hair> hrs = res.ToList();

                    List<string> style = new List<string>();
                    style.Add("不用了");

                    foreach (Hair hh in hrs)
                    {
                        style.Add(hh.HairName + "," + hh.WigName);
                    }

                    int selected2 = Select(pc, "要哪一種髮型？", "", style.ToArray());



                    if (selected2 <= 1)
                    {
                        Say(pc, 11000115, 131, "好吧、$R;" +
                           "如果想改變髮型的話，隨時再來吧！$R;", "尼貝諾特髮型屋");
                        return;
                    }

                    string[] tokens = style.ToArray()[selected2 - 1].Split(',');


                    Hair ha2 = hrs.Find(x => x.HairName == tokens[0] && x.WigName == tokens[1]);

                    Say(pc, 131, "你確認要改變這髮型嗎？$R;髮型: " + ha2.HairName + "$R;選項: " + ha2.WigName);

                    switch (Select(pc, "要改變嗎？", "", "不用了", "要"))
                    {
                        case 1:
                            Say(pc, 11000115, 131, "好吧、$R;" +
                               "如果想改變髮型的話，隨時再來吧！$R;", "尼貝諾特髮型屋");
                            return;
                        case 2:

                            TakeItem(pc, ha2.ItemID, 1);
                            Say(pc, 11000115, 131, "我們來開始吧！$R;", "尼貝諾特髮型屋");
                            ShowEffect(pc, 4112);
                            if (ha2.HairWig <= 0)
                            {
                                pc.Wig = 255;
                            }
                            else
                            {
                                pc.Wig = (ushort)ha2.HairWig;
                            }
                            pc.HairStyle = (ushort)ha2.HairStyle;
                            Complete(pc);
                            break;
                    }



                }
            }
            else
            {
                基本髮型目錄(pc);

            }


        }
        void 基本髮型目錄(ActorPC pc)
        {
            switch (Select(pc, "要改變髮型嗎？", "", "改變", "不用了"))
            {
                case 1:
                    if (pc.HairStyle == 10)
                    {
                        Say(pc, 11000115, 131, "ああ，惜しいな!$R;" +
                                               "$R君の髪が帽子に隠れちゃってる，$R;" +
                                               "帽子を外したらまた来てくれるかな！，$R;", "黑哈因髮型屋");
                        return;
                    }

                    if (pc.Gender == PC_GENDER.MALE)
                    {
                        男性髮型處理判斷(pc);
                        return;
                    }

                    break;

                case 2:
                    Cancel(pc);
                    break;
            }
        }
        private void Start(ActorPC pc)
        {
            Say(pc, 11000115, 131, "我們來開始吧！$R;", "黑哈因髮型屋");
        }

        private void Complete(ActorPC pc)
        {
            Say(pc, 11000411, 131, "完成了！$R;" +
                                   "$R哦！！$R;" +
                                   "很好呢！！跟你很配呢！$R;" +
                                   "請再光臨哦！！$R;", "黑哈因髮型屋");
        }

        private void Cancel(ActorPC pc)
        {
            Say(pc, 11000411, 131, "好吧、$R;" +
               "如果想改變髮型的話，隨時再來吧！$R;", "黑哈因髮型屋");
            return;
        }

        void ニーベルング会話初回(ActorPC pc)
        {
            BitMask<Acropolisut_01> Acropolisut_01_mask = new BitMask<Acropolisut_01>(pc.CMask["Acropolisut_01"]);

            int Gift;

            Acropolisut_01_mask.SetValue(Acropolisut_01.已經與尼貝諾特髮型屋進行第一次對話, true);

            Say(pc, 131, "哦，您好!$R;" +
                                   "歡迎來到「尼貝隆肯髮型屋」。$R;" +
                                   "$R這裡可以做您喜歡的髮型。$R;" +
                                   "$P那麼就來試試啊?$R;", "尼貝諾特髮型屋");

            Say(pc, 131, "……?$R;" +
                                   "$P哇!!$R;" +
                                   "$R看樣子您應該是第一次來這裡吧?$R;" +
                                   "$P來，先送您個代表友誼的禮物吧，$R;" +
                                   "很漂亮的唷!$R;", "尼貝諾特髮型屋");

            Gift = Global.Random.Next(1, 12);

            switch (Gift)
            {
                case 1:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031301, 1);
                    Say(pc, 0, 131, "『ヘアカラー・チェリーレッド』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 2:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031302, 1);
                    Say(pc, 0, 131, "『ヘアカラー・ジェードパープル』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 3:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031303, 1);
                    Say(pc, 0, 131, "『ヘアカラー・ブルー』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 4:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031304, 1);
                    Say(pc, 0, 131, "『ヘアカラー・アイスブルー』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 5:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031305, 1);
                    Say(pc, 0, 131, "『ヘアカラー・グリーン』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 6:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031306, 1);
                    Say(pc, 0, 131, "『ヘアカラー・ライトグリーン』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 7:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031307, 1);
                    Say(pc, 0, 131, "『ヘアカラー・キャンディーイエロー』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 8:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031308, 1);
                    Say(pc, 0, 131, "『ヘアカラー・オレンジ』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 9:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031309, 1);
                    Say(pc, 0, 131, "『ヘアカラー・マットブラック』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 10:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031310, 1);
                    Say(pc, 0, 131, "『ヘアカラー・ダルグレー』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 11:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031311, 1);
                    Say(pc, 0, 131, "『ヘアカラー・シルバー』$R;" + "を手に入れた！$R;", " ");
                    break;

                case 12:
                    PlaySound(pc, 2040, false, 100, 50);
                    GiveItem(pc, 10031312, 1);
                    Say(pc, 0, 131, "『ヘアカラー・モイストシルバー』$R;" + "を手に入れた！$R;", " ");
                    break;
            }
            Say(pc, 131, "それは『ヘアカラー』といって$R;" +
                        "君の素敵な髪の色を、もっともーっと$R;" +
                        "素敵な色に染めてくれるアイテムさ！$R;" +
                        "$P使い方は簡単！$R;" +
                        "ヘアカラーをダブルクリックするだけ。$R;" +
                        "$R１度染めた髪色は戻らないから$R;" +
                        "染めるときは気をつけてね。$R;", "尼貝諾特髮型屋");

        }

        void 男性髮型處理判斷(ActorPC pc)
        {
            if (pc.HairStyle == 7)
            {
                Say(pc, 11000115, 131, "……$R;" +
                                       "$Pなんてことだ!$R;" +
                                       "$Rキミには髪がないじゃないか！$R;" +
                                       "$R髪が伸びたらまたおいで！$R;", "黑哈因髮型屋");
                return;
            }

            if (pc.HairStyle == 4)
            {
                男性剪頭髮光頭髮型處理(pc);
                return;
            }

            if (pc.HairStyle == 11 ||
                pc.HairStyle == 12)
            {
                男性剪頭髮破壞髮型處理(pc);
                return;
            }

            Say(pc, 131, "カットするかい？$Rそれともスタイリングかい？$R;" +
                        "$Rカットだと３０００ゴールド$Rスタイリングだと$R１００００ゴールドになるよ。$R;", "黑哈因髮型屋");

            switch (Select(pc, "どうする?", "", "カットする", "セットする", "やめる"))
            {
                case 1:
                    男性髮型剪頭髮處理(pc);
                    break;

                case 2:
                    男性髮型燙頭髮處理(pc);
                    break;

                case 3:
                    Say(pc, 11000115, 131, "そう?$R;" +
                                           "またおいでよ!$R;", "黑哈因髮型屋");
                    break;
            }
        }

        void 男性髮型剪頭髮處理(ActorPC pc)
        {
            if (pc.Gold >= 3000)
            {
                if (pc.HairStyle == 0 ||
                    pc.HairStyle == 2 ||
                    pc.HairStyle == 5 ||
                    pc.HairStyle == 8)
                {
                    男性剪頭髮平頭髮型處理(pc);
                    return;
                }

                if (pc.HairStyle == 1 ||
                    pc.HairStyle == 3 ||
                    pc.HairStyle == 6 ||
                    pc.HairStyle == 13 ||
                    pc.HairStyle == 14 ||
                    pc.HairStyle == 15 ||
                    pc.HairStyle == 16 ||
                    pc.HairStyle == 20 ||
                    pc.HairStyle == 21 ||
                    pc.HairStyle == 22)
                {
                    男性剪頭髮自然髮型處理(pc);
                    return;
                }
            }
            else
            {
                Say(pc, 131, "……。$R;" +
                            "$Pあ～ん、残念～$R;" +
                            "君は持ち合わせが足りないようだね？$R;" +
                            "$Rまたおいでよ。$R;", "黑哈因髮型屋");
            }
        }

        void 男性剪頭髮平頭髮型處理(ActorPC pc)
        {
            Say(pc, 11000115, 131, "カットするんだね?$R;" +
                                   "$R丸刈りになるから$R;" +
                                   "$P他の髪型に$R;" +
                                   "這樣也沒關係嗎?$R;", "黑哈因髮型屋");

            switch (Select(pc, "どうする?", "", "カットする", "やめる"))
            {
                case 1:
                    pc.Gold -= 3000;

                    Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

                    ShowEffect(pc, 4112);
                    pc.Wig = 255;
                    pc.HairStyle = 4;

                    Complete(pc);
                    break;

                case 2:
                    Say(pc, 11000115, 131, "セットしようか!$R;", "黑哈因髮型屋");

                    switch (Select(pc, "どうする?", "", "セットする", "やめる"))
                    {
                        case 1:
                            男性髮型燙頭髮處理(pc);
                            break;

                        case 2:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            break;
                    }
                    break;
            }
        }

        void 男性剪頭髮自然髮型處理(ActorPC pc)
        {
            Say(pc, 11000115, 131, "わかった，カットだね?$R;" +
                                   "自然な感じに仕上げるよ。$R;", "黑哈因髮型屋");

            switch (Select(pc, "どうする?", "", "カットする", "やめる"))
            {
                case 1:
                    pc.Gold -= 3000;

                    Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

                    ShowEffect(pc, 4112);
                    pc.Wig = 255;
                    pc.HairStyle = 0;

                    Complete(pc);
                    break;

                case 2:
                    Say(pc, 11000115, 131, "そう?$R;" +
                                           "またおいでよ!$R;", "黑哈因髮型屋");
                    break;
            }
        }

        void 男性剪頭髮光頭髮型處理(ActorPC pc)
        {
            Say(pc, 11000115, 131, "髪が短すぎてセットは無理だね。$R;" +
                                   "$Rカットだけになるよ?$R;", "黑哈因髮型屋");

            switch (Select(pc, "どうする?", "", "カットする", "やめる"))
            {
                case 1:
                    Say(pc, 11000115, 131, "本当にする?$R;", "黑哈因髮型屋");

                    switch (Select(pc, "どうする?", "", "カットする", "やめる"))
                    {
                        case 1:
                            if (pc.Gold >= 3000)
                            {
                                pc.Gold -= 3000;

                                Say(pc, 11000115, 131, "……$R;" +
                                                       "$P…じゃあ。$R;" +
                                                       "$R君の髪にははさみが使えないから，$R;" +
                                                       "このバリカンで……$R;" +
                                                       "$Pはじめるよ～!$R;", "黑哈因髮型屋");

                                ShowEffect(pc, 4112);
                                pc.Wig = 255;
                                pc.HairStyle = 7;

                                Complete(pc);
                                return;
                            }
                            Say(pc, 131, "……。$R;" +
                                        "$Pあ～ん、残念～$R;" +
                                        "君は持ち合わせが足りないようだね？$R;" +
                                        "$Rまたおいでよ。$R;", "黑哈因髮型屋");

                            return;

                        case 2:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            break;
                    }
                    break;

                case 2:
                    Say(pc, 11000115, 131, "じゃあ，$R;" +
                                           "髪が伸びたら、またおいでよ！$R;", "黑哈因髮型屋");
                    break;
            }
        }

        void 男性剪頭髮破壞髮型處理(ActorPC pc)
        {
            Say(pc, 11000115, 131, "んー，髪は素敵なんだけど，$R;" +
                                   "すごく強ばってるね。$R;" +
                                   "$R全部刈っちゃうことになるね?$R;", "黑哈因髮型屋");

            switch (Select(pc, "全部刈ってしまう?", "", "やめます", "やっちゃってください"))
            {
                case 1:
                    break;

                case 2:
                    ShowEffect(pc, 4112);
                    pc.Wig = 255;
                    pc.HairStyle = 7;

                    Say(pc, 11000115, 131, "できたよ!$R;", "黑哈因髮型屋");
                    break;
            }
        }

        void 男性髮型燙頭髮處理(ActorPC pc)
        {
            if (pc.Gold >= 10000)
            {
                Say(pc, 11000115, 131, "わかった，セットだね?$R;", "黑哈因髮型屋");

                if (pc.HairStyle == 0)
                {
                    switch (Select(pc, "どんなふうにする?", "", "カッコよく決めて", "さわやかに決めて", "おとなしめにして", "やめる"))
                    {
                        case 1:
                            男性燙頭髮ワイルド處理(pc);
                            return;

                        case 2:
                            男性燙頭髮ボーイズ處理(pc);
                            return;

                        case 3:
                            男性燙頭髮コンサバティブ處理(pc);
                            return;

                        case 4:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            return;
                    }
                }

                if (pc.HairStyle == 1 ||
                    pc.HairStyle == 20)
                {
                    switch (Select(pc, "どんなふうにする?", "", "ワイルドに決めて", "クールに決めて", "やめる"))
                    {
                        case 1:
                            男性燙頭髮オールバック處理(pc);
                            return;

                        case 2:
                            男性燙頭髮ワンレングス處理(pc);
                            return;

                        case 3:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            return;
                    }
                }

                if (pc.HairStyle == 2)
                {
                    switch (Select(pc, "どんなふうにする?", "", "ナチュラルにして", "さわやかに決めて", "おとなしめにして", "やめる"))
                    {
                        case 1:
                            男性燙頭髮ロング處理(pc);
                            return;

                        case 2:
                            男性燙頭髮ボーイズ處理(pc);
                            return;

                        case 3:
                            男性燙頭髮コンサバティブ處理(pc);
                            return;

                        case 4:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            return;
                    }
                }

                if (pc.HairStyle == 3)
                {
                    switch (Select(pc, "どんなふうにする?", "", "ナチュラルにして", "ワイルドに決めて", "やめる"))
                    {
                        case 1:
                            男性燙頭髮ロング處理(pc);
                            return;

                        case 2:
                            男性燙頭髮オールバック處理(pc);
                            return;

                        case 3:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            return;
                    }
                }

                if (pc.HairStyle == 5 ||
                    pc.HairStyle == 8)
                {
                    switch (Select(pc, "どんなふうにする?", "", "ナチュラルにして", "カッコよく決めて", "おとなしめにして", "やめる"))
                    {
                        case 1:
                            男性燙頭髮ロング處理(pc);
                            return;

                        case 2:
                            男性燙頭髮ワイルド處理(pc);
                            return;

                        case 3:
                            男性燙頭髮コンサバティブ處理(pc);
                            return;

                        case 4:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            return;
                    }
                }

                if (pc.HairStyle == 6)
                {
                    switch (Select(pc, "どんなふうにする?", "", "ナチュラルにして", "クールに決めて", "やめる"))
                    {
                        case 1:
                            男性燙頭髮ロング處理(pc);
                            return;

                        case 2:
                            男性燙頭髮ワンレングス處理(pc);
                            return;

                        case 3:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            return;
                    }
                }

                if (pc.HairStyle == 13 ||
                    pc.HairStyle == 14 ||
                    pc.HairStyle == 15 ||
                    pc.HairStyle == 16 ||
                    pc.HairStyle == 21 ||
                    pc.HairStyle == 22)
                {
                    switch (Select(pc, "どんなふうにする?", "", "ワイルドに決めて", "クールに決めて", "ナチュラルにして", "やめる"))
                    {
                        case 1:
                            男性燙頭髮オールバック處理(pc);
                            return;

                        case 2:
                            男性燙頭髮ワンレングス處理(pc);
                            return;

                        case 3:
                            男性燙頭髮ロング處理(pc);
                            return;

                        case 4:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "黑哈因髮型屋");
                            return;
                    }
                }
            }
            else
            {
                Say(pc, 131, "……。$R;" +
                            "$Pあ～ん、残念～$R;" +
                            "君は持ち合わせが足りないようだね？$R;" +
                            "$Rまたおいでよ。$R;", "黑哈因髮型屋");

            }
        }

        void 男性燙頭髮ワイルド處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

            ShowEffect(pc, 4112);
            pc.Wig = 255;
            pc.HairStyle = 2;

            Complete(pc);
        }

        void 男性燙頭髮ボーイズ處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

            ShowEffect(pc, 4112);
            pc.Wig = 255;
            pc.HairStyle = 5;

            Complete(pc);
        }

        void 男性燙頭髮コンサバティブ處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

            ShowEffect(pc, 4112);
            pc.Wig = 255;
            pc.HairStyle = 8;

            Complete(pc);
        }

        void 男性燙頭髮ロング處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

            ShowEffect(pc, 4112);
            pc.Wig = 255;
            pc.HairStyle = 1;

            Complete(pc);
        }

        void 男性燙頭髮オールバック處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

            ShowEffect(pc, 4112);
            pc.Wig = 255;
            pc.HairStyle = 6;

            Complete(pc);
        }

        void 男性燙頭髮ワンレングス處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!!$R;", "黑哈因髮型屋");

            ShowEffect(pc, 4112);
            pc.Wig = 255;
            pc.HairStyle = 3;

            Complete(pc);
        }
    }
}
