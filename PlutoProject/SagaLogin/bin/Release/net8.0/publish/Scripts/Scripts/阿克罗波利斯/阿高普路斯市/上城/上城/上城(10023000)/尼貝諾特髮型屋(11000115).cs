using System;
using System.Collections.Generic;
using System.Text;

using SagaLib;
using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Scripting;
using SagaScript.Chinese.Enums;
using System.Linq;

namespace SagaScript.M10023000
{

    public class S11000115 : Event
    {
        private List<HairStyleList> stylelist;
        private List<String> stylenames;

        public S11000115()
        {
            this.EventID = 11000115;
        }

        public override void OnEvent(ActorPC pc)
        {

            BitMask<Acropolisut_01> mask = new BitMask<Acropolisut_01>(pc.CMask["Acropolisut_01"]);

            List<uint> hairlist = gethairlist(pc);
            List<string> hair_name = new List<string>();
            List<uint> hairticket_id = new List<uint>();


            if (pc.Gender == PC_GENDER.MALE)
            {
                Say(pc, 11000115, 131, "很抱歉，$R;" +
                    "我對男生不感興趣…$R;" +
                    "請找別的髮型屋吧！", "尼貝諾特髮型屋");
                return;
            }


            if (!mask.Test(Acropolisut_01.已經與尼貝諾特髮型屋進行第一次對話))
            {
                ニーベルング会話初回(pc);
                return;
            }


            if (pc.Marionette != null)
            {
                Say(pc, 11000115, 131, "請先解除木偶，$R;" +
                                       "然後再來找我吧！$R;"
                                       , "尼貝諾特髮型屋");
                return;
            }

            /*
            if (!mask.Test(Acropolisut_01.已經與尼貝諾特髮型屋進行第一次對話))
            {
                ニーベルング会話初回(pc);
                return;
            }
            */


            Say(pc, 131, "哦！你好。$R;" +
                "歡迎光臨尼貝諾特髮型屋！$R;" +
                "$R你可以在這裡を$R;" +
                "改變你喜歡的髮型。$R;" +
                "$P你準備好改變你的髮型了嗎？$R;", "尼貝諾特髮型屋");

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
                    var res = from hair in HairFactory.Instance.Hairs where ((CountItem(pc, hair.ItemID) > 0) && (hair.ItemID == seleced_id) && (hair.Gender == 1 || hair.Gender == 2)) select hair;

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

            return;
        }

        private void Complete(ActorPC pc)
        {
            Say(pc, 11000115, 131, "完成了！$R;" +
                                   "$R哦！！$R;" +
                                   "很好呢！！跟你很配呢！$R;" +
                                   "請再光臨哦！！$R;", "尼貝諾特髮型屋");
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

        void 基本髮型目錄(ActorPC pc)
        {
            switch (Select(pc, "要改變髮型嗎？", "", "改變", "不用了"))
            {
                case 1:
                    if (pc.HairStyle == 10)
                    {
                        Say(pc, 11000115, 131, "ああ，惜しいな!$R;" +
                                               "$R君の髪が帽子に隠れちゃってる，$R;" +
                                               "帽子を外したらまた来てくれるかな！，$R;", "尼貝諾特髮型屋");
                        return;
                    }

                    if (pc.Gender == PC_GENDER.FEMALE)
                    {
                        女性髮型處理判斷(pc);
                        return;
                    }

                    break;

                case 2:
                    Say(pc, 11000115, 131, "そっかぁ，$R;" +
                                           "髪型を変えたくなったら，$R;" +
                                           "また私のところへおいでよ！$R;", "尼貝諾特髮型屋");
                    break;
            }
        }


        void 女性髮型處理判斷(ActorPC pc)
        {
            if (pc.HairStyle == 14)		//	ベリーショート
            {

                Say(pc, 11000115, 131, "……$R;" +
                                       "$Pなんてことだ!$R;" +
                                       "$Rキミの髪は短すぎて，$R;" +
                                       "どんな髪型にも出来ないよ!$R;" +
                                       "$R髪が伸びたらまたおいで！$R;", "尼貝諾特髮型屋");
                return;
            }

            Say(pc, 131, "カットするかい？$Rそれともスタイリングかい？$R;" +
                        "$Rカットだと３０００ゴールド$Rスタイリングだと$R１００００ゴールドになるよ。$R;", "尼貝諾特髮型屋");

            switch (Select(pc, "どうする？", "", "カットする", "スタイリングする", "やめておく"))
            {
                case 1:
                    女性髮型カット処理(pc);
                    break;

                case 2:
                    女性髮型セット処理(pc);
                    break;

                case 3:
                    Say(pc, 11000115, 131, "そっか$R;" +
                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                    break;
            }
        }

        void 女性髮型カット処理(ActorPC pc)
        {
            if (pc.Gold >= 3000)
            {
                if (pc.HairStyle == 0 ||		//ショート
                    pc.HairStyle == 16 ||
                    pc.HairStyle == 18)
                {
                    女性剪頭髮平頭髮型處理(pc);
                    return;
                }

                if (pc.HairStyle == 1 ||		//レイヤー
                    pc.HairStyle == 7 ||		//セミロング
                    pc.HairStyle == 9 ||		//アンティーク
                    pc.HairStyle == 17)
                {
                    女性剪頭髮短髮髮型處理(pc);
                    return;
                }

                if (pc.HairStyle == 2 ||		//ロング
                    pc.HairStyle == 3 ||
                    pc.HairStyle == 4 ||
                    pc.HairStyle == 5 ||
                    pc.HairStyle == 6 ||
                    pc.HairStyle == 8 ||
                    pc.HairStyle == 19 ||
                    pc.HairStyle == 20 ||
                    pc.HairStyle == 21 ||
                    pc.HairStyle == 22 ||
                    pc.HairStyle == 23 ||
                    pc.HairStyle == 24 ||
                    pc.HairStyle == 25)
                {
                    女性剪頭髮半長髮型處理(pc);
                    return;
                }
            }
            else
            {
                Say(pc, 131, "……。$R;" +
                            "$Pあ～ん、残念～$R;" +
                            "君は持ち合わせが足りないようだね？$R;" +
                            "$Rまたおいでよ。$R;", "尼貝諾特髮型屋");
            }
        }

        void 女性剪頭髮平頭髮型處理(ActorPC pc)
        {
            Say(pc, 11000115, 131, "うん，カットだね?$R;" +
                                   "$Rカットすると，かなり短くなってしまうね。$R;" +
                                   "$Pそうなると後で$R;" +
                                   "他の髪型にかえることが，$R;" +
                                   "できなくなるよ。$R;" +
                                   "それでもいいかな?$R;", "尼貝諾特髮型屋");

            switch (Select(pc, "どうする?", "", "カットする", "やめる"))
            {
                case 1:
                    pc.Gold -= 3000;

                    Say(pc, 11000115, 131, "じゃあ，はじめるよ!$R;", "尼貝諾特髮型屋");

                    ShowEffect(pc, 4112);
                    pc.Wig = 255;
                    pc.HairStyle = 14;		// ベリーショート

                    Complete(pc);
                    break;

                case 2:
                    Say(pc, 11000115, 131, "そう?$R;" +
                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                    break;
            }
        }

        void 女性剪頭髮短髮髮型處理(ActorPC pc)
        {
            Say(pc, 11000115, 131, "うん，カットだね?$R;" +
                                   "$Rかなり短くなってしまうから$R;" +
                                   "$Pあとでカットできる髪型が$R;" +
                                   "限られちゃうね。$R;" +
                                   "それでもいいかな?$R;", "尼貝諾特髮型屋");

            switch (Select(pc, "どうする?", "", "カットする", "やめる"))
            {
                case 1:
                    pc.Gold -= 3000;

                    Say(pc, 11000115, 131, "じゃあ，はじめるよ!$R;", "尼貝諾特髮型屋");

                    ShowEffect(pc, 4112);
                    pc.Wig = 255;
                    pc.HairStyle = 0;		// ショート

                    Complete(pc);
                    break;

                case 2:
                    Say(pc, 11000115, 131, "そう?$R;" +
                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                    break;
            }
        }

        void 女性剪頭髮半長髮型處理(ActorPC pc)
        {

            Say(pc, 11000115, 131, "あとでカットできる髪型が$R;" +
                                   "限られちゃうけど、$R;" +
                                   "それでもいいかな?$R;", "尼貝諾特髮型屋");


            switch (Select(pc, "どうする?", "", "カットする", "やめる"))
            {
                case 1:
                    pc.Gold -= 3000;

                    Say(pc, 11000115, 131, "じゃあ，はじめるよ!$R;", "尼貝諾特髮型屋");

                    ShowEffect(pc, 4112);
                    pc.Wig = 255;
                    pc.HairStyle = 7;

                    Complete(pc);
                    break;

                case 2:
                    Say(pc, 11000115, 131, "そう?$R;" +
                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                    break;
            }
        }

        void 女性髮型セット処理(ActorPC pc)
        {
            if (pc.Gold >= 10000)
            {
                if (pc.HairStyle == 0 ||
                    pc.HairStyle == 16 ||
                    pc.HairStyle == 18)
                {
                    Say(pc, 11000115, 131, "髪が短すぎるから$R" +
                                            "カットだけになるよ$R;" +
                                            "$Rそれでいいかい?$R;", "尼貝諾特髮型屋");

                    switch (Select(pc, "どうする?", "", "カットする", "やめる"))
                    {
                        case 1:
                            pc.Gold -= 3000;

                            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

                            ShowEffect(pc, 4112);
                            pc.Wig = 255;
                            pc.HairStyle = 14;

                            Complete(pc);
                            return;

                        case 2:
                            Say(pc, 11000115, 131, "そう?$R;" +
                                                   "またおいでよ!$R;", "尼貝諾特髮型屋");
                            return;
                    }
                }

                Say(pc, 11000115, 131, "どんな感じにしようか?$R;" +
                                       "$R今の長さに合わせた$R;" +
                                       "髪型になるけど?$R;", "尼貝諾特髮型屋");

                switch (Select(pc, "どうする?", "", "考える", "やめる"))
                {
                    case 1:
                        if (pc.HairStyle == 1)
                        {
                            switch (Select(pc, "どうする?", "", "肩と同じくらいに揃えて", "前髪ぱっつんにして", "やめる"))
                            {
                                case 1:
                                    女性造型設計セミロング處理(pc);
                                    return;

                                case 2:
                                    女性造型設計アンティーク處理(pc);
                                    return;

                                case 3:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }

                        if (pc.HairStyle == 2)
                        {
                            switch (Select(pc, "どうする?", "", "内巻きパーマをかけて", "外巻きパーマをかけて", "ちょっと大人っぽく", "やめる"))
                            {
                                case 1:
                                    女性造型設内巻きヘア處理(pc);
                                    return;

                                case 2:
                                    女性造型設計外巻きヘア處理(pc);
                                    return;

                                case 3:
                                    女性造型設計ナチュラル處理(pc);
                                    return;

                                case 4:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }

                        if (pc.HairStyle == 3)
                        {
                            switch (Select(pc, "どうする?", "", "ストレートパーマをかけて", "内巻きパーマをかけて", "ちょっと大人っぽく", "やめる"))
                            {
                                case 1:
                                    女性造型設計ロング處理(pc);
                                    return;

                                case 2:
                                    女性造型設内巻きヘア處理(pc);
                                    return;

                                case 3:
                                    女性造型設計ナチュラル處理(pc);
                                    return;

                                case 4:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }

                        if (pc.HairStyle == 4)
                        {
                            switch (Select(pc, "どうする?", "", "ストレートパーマをかけて", "外巻きパーマをかけて", "ちょっと大人っぽく", "やめる"))
                            {
                                case 1:
                                    女性造型設計ロング處理(pc);
                                    return;

                                case 2:
                                    女性造型設計外巻きヘア處理(pc);
                                    return;

                                case 3:
                                    女性造型設計ナチュラル處理(pc);
                                    return;

                                case 4:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }

                        if (pc.HairStyle == 7)
                        {
                            switch (Select(pc, "どうする?", "", "全体的に軽くする", "前髪ぱっつんにする", "やめる吧"))
                            {
                                case 1:
                                    女性造型設計レイヤー處理(pc);
                                    return;

                                case 2:
                                    女性造型設計アンティーク處理(pc);
                                    return;

                                case 3:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }

                        if (pc.HairStyle == 8)
                        {
                            switch (Select(pc, "どうする?", "", "ストレートパーマをかけて", "内巻きパーマをかけて", "外巻きパーマをかけて", "やめる"))
                            {
                                case 1:
                                    女性造型設計ロング處理(pc);
                                    return;

                                case 2:
                                    女性造型設内巻きヘア處理(pc);
                                    return;

                                case 3:
                                    女性造型設計外巻きヘア處理(pc);
                                    return;

                                case 4:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }

                        if (pc.HairStyle == 9)
                        {
                            switch (Select(pc, "どうする?", "", "肩と同じくらいに揃えて", "全体的に軽くする", "やめる"))
                            {
                                case 1:
                                    女性造型設計セミロング處理(pc);
                                    return;

                                case 2:
                                    女性造型設計レイヤー處理(pc);
                                    return;

                                case 3:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }

                        if (pc.HairStyle == 5 ||
                            pc.HairStyle == 6 ||
                            pc.HairStyle == 17 ||
                            pc.HairStyle == 19 ||
                            pc.HairStyle == 20 ||
                            pc.HairStyle == 21 ||
                            pc.HairStyle == 22 ||
                            pc.HairStyle == 23 ||
                            pc.HairStyle == 24 ||
                            pc.HairStyle == 25 ||
                            pc.HairStyle > 26)
                        {
                            switch (Select(pc, "どうする?", "", "ストレートパーマをかけて", "内巻きパーマをかけて", "外巻きパーマをかけて", "ちょっと大人っぽく", "やめる"))
                            {
                                case 1:
                                    女性造型設計ロング處理(pc);
                                    return;

                                case 2:
                                    女性造型設内巻きヘア處理(pc);
                                    return;

                                case 3:
                                    女性造型設計外巻きヘア處理(pc);
                                    return;

                                case 4:
                                    女性造型設計ナチュラル處理(pc);
                                    return;

                                case 5:
                                    Say(pc, 11000115, 131, "そう?$R;" +
                                                           "またおいでよ!$R;", "尼貝諾特髮型屋");
                                    return;
                            }
                        }
                        return;

                    case 2:
                        Say(pc, 11000115, 131, "そう?$R;" +
                                               "またおいでよ!$R;", "尼貝諾特髮型屋");
                        return;
                }
            }
            else
            {
                Say(pc, 11000115, 131, "……$R;" +
                                    "$Pあ～ん、残念～$R;" +
                                    "君は持ち合わせが足りないようだね？$R;" +
                                    "$Rまたおいでよ。$R;", "尼貝諾特髮型屋");
            }
        }

        void 女性造型設計レイヤー處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

            ShowEffect(pc, 4112);
            pc.HairStyle = 1;
            pc.Wig = 255;

            Complete(pc);
        }

        void 女性造型設計ロング處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

            ShowEffect(pc, 4112);
            pc.HairStyle = 2;
            pc.Wig = 255;

            Complete(pc);
        }

        void 女性造型設計外巻きヘア處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

            ShowEffect(pc, 4112);
            pc.HairStyle = 3;
            pc.Wig = 255;

            Complete(pc);
        }

        void 女性造型設内巻きヘア處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

            ShowEffect(pc, 4112);
            pc.HairStyle = 4;
            pc.Wig = 255;

            Complete(pc);
        }

        void 女性造型設計セミロング處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

            ShowEffect(pc, 4112);
            pc.HairStyle = 7;
            pc.Wig = 255;

            Complete(pc);
        }

        void 女性造型設計ナチュラル處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

            ShowEffect(pc, 4112);
            pc.HairStyle = 8;
            pc.Wig = 255;

            Complete(pc);
        }

        void 女性造型設計アンティーク處理(ActorPC pc)
        {
            pc.Gold -= 10000;

            Say(pc, 11000115, 131, "じゃあ、はじめるよ!$R;", "尼貝諾特髮型屋");

            ShowEffect(pc, 4112);
            pc.HairStyle = 9;
            pc.Wig = 255;

            Complete(pc);
        }


    }
}