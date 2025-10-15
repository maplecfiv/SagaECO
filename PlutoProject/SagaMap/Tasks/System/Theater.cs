using System;
using SagaDB.Actor;
using SagaDB.Item;
using SagaDB.Theater;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Packets.Server.Theater;

namespace SagaMap.Tasks.System
{
    public class Theater : MultiRunTask
    {
        private static Theater instance;

        public Theater()
        {
            Period = 60000;
            DueTime = 0;
        }

        public static Theater Instance
        {
            get
            {
                if (instance == null)
                    instance = new Theater();
                return instance;
            }
        }

        public override void CallBack()
        {
            try
            {
                foreach (var j in TheaterFactory.Instance.Items.Keys)
                {
                    var nextMovie = TheaterFactory.Instance.GetNextMovie(j);
                    var map = MapManager.Instance.GetMap(j);
                    var actors = new Actor[map.Actors.Count];
                    map.Actors.Values.CopyTo(actors, 0);
                    var now = new DateTime(1970, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    if (nextMovie != null)
                    {
                        var span = nextMovie.StartTime - now;
                        switch ((int)span.TotalMinutes)
                        {
                            case 10:
                            case 7:
                            case 5:
                            case 3:
                            case 2:
                            case 1:
                                Logger.getLogger().Information(string.Format(
                                    "{0} is going to play <{1}> in {2:0} minutes", map.Name,
                                    nextMovie.Name, span.TotalMinutes));
                                foreach (var i in actors)
                                {
                                    if (i.type != ActorType.PC)
                                        continue;
                                    var pc = (ActorPC)i;
                                    if (pc.Online)
                                    {
                                        if ((int)span.TotalMinutes == 10)
                                        {
                                            var item = pc.Inventory.GetItem(nextMovie.Ticket,
                                                Inventory.SearchType.ITEM_ID);
                                            if (item.Stack == 0)
                                            {
                                                var lobby = j - 10000;
                                                map.SendActorToMap(pc, lobby, 10, 1);
                                            }
                                            else
                                            {
                                                var p3 = new SSMG_THEATER_INFO();
                                                p3.MessageType = SSMG_THEATER_INFO.Type.MESSAGE;
                                                p3.Message = LocalManager.Instance.Strings.THEATER_WELCOME;
                                                MapClient.FromActorPC(pc).NetIo.SendPacket(p3);
                                                p3 = new SSMG_THEATER_INFO();
                                                p3.MessageType = SSMG_THEATER_INFO.Type.MOVIE_ADDRESS;
                                                p3.Message = nextMovie.URL;
                                                MapClient.FromActorPC(pc).NetIo.SendPacket(p3);
                                            }
                                        }

                                        var p = new SSMG_THEATER_INFO();
                                        p.MessageType = SSMG_THEATER_INFO.Type.MESSAGE;
                                        p.Message = string.Format(LocalManager.Instance.Strings.THEATER_COUNTDOWN,
                                            nextMovie.Name, (int)span.TotalMinutes);
                                        MapClient.FromActorPC(pc).NetIo.SendPacket(p);
                                        if ((int)span.TotalMinutes == 1)
                                        {
                                            var p1 = new SSMG_THEATER_INFO();
                                            p1.MessageType = SSMG_THEATER_INFO.Type.STOP_BGM;
                                            MapClient.FromActorPC(pc).NetIo.SendPacket(p1);
                                        }
                                    }
                                }

                                break;
                            case 0:
                                Logger.getLogger().Information(string.Format("{0} is now playing <{1}>", map.Name,
                                    nextMovie.Name));
                                foreach (var i in actors)
                                {
                                    if (i.type != ActorType.PC)
                                        continue;
                                    var pc = (ActorPC)i;
                                    if (pc.Online)
                                    {
                                        MapClient.FromActorPC(pc).DeleteItemID(nextMovie.Ticket, 1, true);
                                        var p = new SSMG_THEATER_INFO();
                                        p.MessageType = SSMG_THEATER_INFO.Type.PLAY;
                                        p.Message = "";
                                        MapClient.FromActorPC(pc).NetIo.SendPacket(p);
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.getLogger().Error(ex, ex.Message);
            }
        }
    }
}