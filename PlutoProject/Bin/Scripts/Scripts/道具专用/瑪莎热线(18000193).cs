using System;
using System.Collections.Generic;
using System.Text;

using SagaDB.Actor;
using SagaMap.Scripting;

using SagaLib;
using SagaScript.Chinese.Enums;
namespace SagaScript.瑪莎热线
{
    public class S18000193 : Event
    {
        public S18000193()
        {
            this.EventID = 18000193;
        }

        public override void OnEvent(ActorPC pc)
        {
            BitMask<Beginner_02> Beginner_02_mask = new BitMask<Beginner_02>(pc.CMask["Beginner_02"]);
            int selection;
            if (!Beginner_02_mask.Test(Beginner_02.第一次使用瑪莎熱線))
            {
               第一次瑪莎熱線(pc);
               return;
            }
            else
            {
                selection = Select(pc, "何をしますか？", "", "マーシャホットライン　《マーシャと話す》", "マルチチャンネル　　　《通信機能を使う》", "通信を切る");

                while (selection != 3)
                {
                    switch (selection)
                    {
                        case 1:
                            Say(pc, 0, 0, "もしもし～？$R;" +
                            "こちらは$R;" +
                            "『マーシャの冒険者お助け$R;" +
                            "　ホットライン』です♪$R;" +
                            "" + pc.Name + "さん！ こんにちは！$R;" +
                            "$Pあ、まだあの島にいるのね。$R;" +
                            "その島に着いた時$R;" +
                            "近くに誰かいなかった？$R;" +
                            "もし誰かいたなら、$R;" +
                            "まずは話しかけてみて！$R;" +
                            "$Pとにかく人と話して$R;" +
                            "情報収集をすること！$R;" +
                            "それが冒険者としての$R;" +
                            "第一歩よ。$R;" +
                            "$Pもし、それでも判らなかったら、$R;" +
                            "世界中の冒険者と繋がる$R;" +
                            "マルチチャンネルの$R;" +
                            "初心者掲示板で$R;" +
                            "調べてみるといいわ。$R;" +
                            "$P何も情報がなければ、$R;" +
                            "自分で$R;" +
                            "初心者掲示板に$R;" +
                            "書き込んで、$R;" +
                            "質問をしてみてね！$R;" +
                            "$Pきっと経験豊かな$R;" +
                            "冒険者の誰かが$R;" +
                            "助けてくれるわ。$R;", "マーシャ");
                            break;

                        case 2:
                            通信機能(pc);
                            return;
                    }
                    selection = Select(pc, "何をしますか？", "", "マーシャホットライン　《マーシャと話す》", "マルチチャンネル　　　《通信機能を使う》", "通信を切る");
                }
                }

        }
        void 第一次瑪莎熱線(ActorPC pc)
        {
            BitMask<Beginner_02> Beginner_02_mask = new BitMask<Beginner_02>(pc.CMask["Beginner_02"]);

            switch (Select(pc, "何をしますか？", "", "マーシャホットライン　《マーシャと話す》", "マルチチャンネル　　　《通信機能を使う》", "通信を切る"))
            {
                case 1:
                    Beginner_02_mask.SetValue(Beginner_02.第一次使用瑪莎熱線, true);
                    Say(pc, 0, 0, "もしもし～？$R;" +
                    "こちらは、$R;" +
                    "『マーシャの冒険者$R;" +
                    "　お助けホットライン』です♪$R;" +
                    "$Pうふふ。$R;" +
                    "そうそう、そうやって使うの。$R;" +
                    "飛空庭を降りたら$R;" +
                    "まず、マーシャホットラインを$R;" +
                    "使ってみてね♪$R;" +
                    "$Pもし、マーシャホットラインを$R;" +
                    "なくしてしまったら$R;" +
                    "アクロポリスの可動橋って所に$R;" +
                    "初心者案内人さんがいるから$R;" +
                    "彼にお願いをしてみてね。$R;" +
                    "$P代わりのマーシャホットラインを$R;" +
                    "渡してくれるように$R;" +
                    "お願いしておいたから！$R;" +
                    "$Pそれと、もう一つ。$R;" +
                    "これもどうぞ！$R;", "マーシャ");
                    Say(pc, 0, 0, "それじゃあ、$R;" +
                    "アクロポリスシティまで$R;" +
                    "全速力で行くわよー！$R;" +
                    "振り落とされないように$R;" +
                    "気をつけてね！$R;", "マーシャ");
                    Warp(pc, 10071000, 245, 82);
                    break;

                case 2:
                    通信機能(pc);
                    return;
            }
        }
        void 通信機能(ActorPC pc)
        {
            Select(pc, "使いたい機能を選んでください。", "", "ヘルプ表示　　　《困ったときはヘルプ》", "初心者掲示板　　《質問はここで》", "パーティー募集　《仲間を探そう》", "アイテム募集　　《宝物を交換しよう》", "情報募集　　　　《情報収集はここで》", "チャットルーム　《楽しいお話広場》", "メール　　　　　《手紙を渡そう》", "その他の情報　  《世界の情報など》", "通信を切る");
            Say(pc, 0, 0, "坏了....;", "マーシャ");
        }
        }
    }
