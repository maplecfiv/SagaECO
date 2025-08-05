using SagaLogin.Manager;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Server;

namespace SagaLogin.Network.Client
{
    public partial class LoginClient : SagaLib.Client
    {
        public void OnChatWhisper(CSMG_CHAT_WHISPER p)
        {
            if (selectedChar == null) return;
            var client = LoginClientManager.Instance.FindClient(p.Receiver);
            if (client != null)
            {
                var p1 = new SSMG_CHAT_WHISPER();
                p1.Sender = selectedChar.Name;
                p1.Content = p.Content;
                client.netIO.SendPacket(p1);
            }
            else
            {
                var p1 = new SSMG_CHAT_WHISPER_FAILED();
                p1.Receiver = p.Receiver;
                p1.Result = 0xFFFFFFFF;
                netIO.SendPacket(p1);
            }
        }
    }
}