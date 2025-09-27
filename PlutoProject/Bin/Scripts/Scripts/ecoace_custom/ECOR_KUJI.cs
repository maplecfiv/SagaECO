using System;
using System.Collections.Generic;
using System.Text;
using SagaLib;
using SagaDB;
using SagaDB.Item;
using SagaDB.Actor;
using SagaMap.Network.Client;
using SagaMap.Scripting;
using SagaMap.Manager;
using SagaScript.Chinese.Enums;
namespace SagaScript
{
    public class RKUJI : SagaMap.Scripting.Item

    {
        public RKUJI()
        {
            
            Init(85000001, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ1");
                TakeItem(pc, 22000104, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000002, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ2");
                TakeItem(pc, 22000105, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000003, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ3");
                TakeItem(pc, 22000106, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000004, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ4");
                TakeItem(pc, 22000107, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000005, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ5");
                TakeItem(pc, 22000108, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000006, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ6");
                TakeItem(pc, 22000109, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000007, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ7");
                TakeItem(pc, 22000110, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000008, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ8");
                TakeItem(pc, 22000111, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000009, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ9");
                TakeItem(pc, 22000112, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
            Init(85000010, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ10");
                TakeItem(pc, 22000113, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000045, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "���������ر�ƪ!");
                TakeItem(pc, 22000180, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000011, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ11");
                TakeItem(pc, 22000114, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000012, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ12");
                TakeItem(pc, 22000115, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000046, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "�����ر�ƪ!");
                TakeItem(pc, 22000181, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000013, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ13");
                TakeItem(pc, 22000116, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000047, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "���ġ��ر�ƪ!");
                TakeItem(pc, 22000182, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000014, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ14");
                TakeItem(pc, 22000117, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000015, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ15");
                TakeItem(pc, 22000118, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000016, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ16");
                TakeItem(pc, 22000119, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000048, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯDX");
                TakeItem(pc, 22000183, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });  
	    Init(85000017, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ17");
                TakeItem(pc, 22000122, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });          
	    Init(85000049, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "�´���ѡ");
                TakeItem(pc, 22000184, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });
	    Init(85000018, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ18");
                TakeItem(pc, 22000125, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            }); 
	    Init(85000050, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯDX�ڶ���");
                TakeItem(pc, 22000185, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });  
	    Init(85000019, delegate(ActorPC pc)
            {
                GiveRandomTreasure(pc, "������ȯ19");
                TakeItem(pc, 22000123, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });








        Init(85000051, delegate(ActorPC pc)//����
            {
                GiveRandomTreasure(pc, "Ů����С����ǵ������");
                TakeItem(pc, 22010100, 1);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "���������������!", "������ȯ");
            });


        Init(85100050, delegate(ActorPC pc)//����
        {
            GiveRandomTreasure(pc, "ȫ��ͨ��������װ");
            TakeItem(pc, 22110100, 1);
            PlaySound(pc, 2040, false, 100, 50);
            Say(pc, 0, "���������������!", "������ȯ");
        });


	    Init(85002045, delegate(ActorPC pc)//��ECO֮��
            {
                //GiveRandomTreasure(pc, "������ȯ10");
                TakeItem(pc, 22000103, 1);
		GiveItem(pc, 16000300, 10);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "�õ���10��ʱ��֮ԿE!", "ECO֮��");
            });
	    Init(90000210, delegate(ActorPC pc)//��10��װ
            {
                //GiveRandomTreasure(pc, "������ȯ10");
                TakeItem(pc, 16000301, 1);
		GiveItem(pc, 16000300, 10);
                PlaySound(pc, 2040, false, 100, 50);
                Say(pc, 0, "�õ���10��ʱ��֮ԿE!", "ECO֮��");
            });
        }
    }
}