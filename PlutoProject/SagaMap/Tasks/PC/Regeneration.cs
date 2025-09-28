using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC
{
    public class Regeneration : MultiRunTask
    {
        private readonly MapClient client;

        public Regeneration(MapClient client)
        {
            dueTime = 5000;
            period = 5000;
            this.client = client;
        }

        public override void CallBack()
        {
            ClientManager.EnterCriticalArea();
            try
            {
                if (client.Character.Mode == PlayerMode.KNIGHT_EAST) //除夕活动
                {
                    Deactivate();
                    client.Character.Tasks.Remove("Regeneration");
                }

                //if (this.client != null)
                {
                    client.Character.HP += (uint)(0.1f * client.Character.MaxHP);
                    ;
                    if (client.Character.HP > client.Character.MaxHP)
                        client.Character.HP = client.Character.MaxHP;
                    client.Character.MP += (uint)(0.1f * client.Character.MaxMP);
                    if (client.Character.MP > client.Character.MaxMP)
                        client.Character.MP = client.Character.MaxMP;
                    client.Character.SP += (uint)(0.1f * client.Character.MaxSP);
                    if (client.Character.SP > client.Character.MaxSP)
                        client.Character.SP = client.Character.MaxSP;
                    client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, client.Character,
                        true);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                Deactivate();
                client.Character.Tasks.Remove("Regeneration");
            }

            ClientManager.LeaveCriticalArea();
        }
    }
}