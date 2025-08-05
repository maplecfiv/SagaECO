using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Partner;
using SagaLib;

namespace SagaMap.Partner.AICommands
{
    public class Chase : AICommand
    {
        private readonly Actor dest;
        private readonly PartnerAI partnerai;
        private int index;

        private List<MapNode> path;

        public short x, y;

        public Chase(PartnerAI partnerai, Actor dest)
        {
            this.partnerai = partnerai;
            this.dest = dest;
            x = dest.X;
            y = dest.Y;
            if (partnerai.Partner.MapID != dest.MapID || !partnerai.CanMove)
            {
                Status = CommandStatus.FINISHED;
                return;
            }

            if (partnerai.Mode.RunAway)
            {
                if (PartnerAI.GetLengthD(partnerai.Partner.X, partnerai.Partner.Y, dest.X, dest.Y) < 2000)
                {
                    if (Global.Random.Next(0, 99) < 20)
                    {
                        var range = Global.Random.Next(100, 1000);
                        x = (short)(partnerai.Partner.X - dest.X);
                        y = (short)(partnerai.Partner.Y - dest.Y);
                        x = (short)(partnerai.Partner.X + x / PartnerAI.GetLengthD(0, 0, x, y) * range);
                        y = (short)(partnerai.Partner.Y + y / PartnerAI.GetLengthD(0, 0, x, y) * range);
                    }
                    else
                    {
                        Status = CommandStatus.FINISHED;
                        return;
                    }
                }
                else
                {
                    Status = CommandStatus.FINISHED;
                    return;
                }
            }

            path = partnerai.FindPath(Global.PosX16to8(partnerai.Partner.X, partnerai.map.Width),
                Global.PosY16to8(partnerai.Partner.Y, partnerai.map.Height), Global.PosX16to8(x, partnerai.map.Width),
                Global.PosY16to8(y, partnerai.map.Height));
            Status = CommandStatus.INIT;
        }

        public string GetName()
        {
            return "Chase";
        }

        public void Update(object para)
        {
            try
            {
                ActorPartner partner = null;
                if (partnerai.Partner.type == ActorType.PARTNER)
                    partner = (ActorPartner)partnerai.Partner;
                MapNode node;
                if (partnerai.Mode.NoMove || !partnerai.CanMove)
                    return;
                if (dest.Status == null)
                {
                    Status = CommandStatus.FINISHED;
                    return;
                }

                //if (mob.Mode.isAnAI && mob.CannotMove)
                //    return;
                if (dest.Status.Additions.ContainsKey("Hiding")) Status = CommandStatus.FINISHED;
                if (dest.Status.Additions.ContainsKey("Through")) Status = CommandStatus.FINISHED;
                if (Status == CommandStatus.FINISHED)
                    return;
                var chasedest = dest;
                if (partner.ai_mode == 1 && partner.Owner != null)
                    chasedest = partner.Owner;
                float size;
                if (partnerai.Mode.isAnAI)
                    size = partnerai.needlen;
                else
                    size = ((ActorPartner)partnerai.Partner).BaseData.range;
                var ifNeko = false;
                if (partnerai.Partner.type == ActorType.PET)
                    if (((ActorPartner)partnerai.Partner).BaseData.partnertype == PartnerType.MAGIC_CREATURE)
                        ifNeko = true;

                if (PartnerAI.GetLengthD(partnerai.Partner.X, partnerai.Partner.Y, chasedest.X, chasedest.Y) <=
                    size * 150 && !ifNeko)
                    if (!partnerai.Mode.RunAway || Global.Random.Next(0, 99) < 70)
                    {
                        partnerai.map.FindFreeCoord(partnerai.Partner.X, partnerai.Partner.Y, out x, out y,
                            partnerai.Partner);
                        if ((partnerai.Partner.X == x && partnerai.Partner.Y == y) || partnerai.Mode.RunAway)
                        {
                            Status = CommandStatus.FINISHED;
                            return;
                        }

                        var dst = new short[2] { x, y };
                        partnerai.map.MoveActor(Map.MOVE_TYPE.START, partnerai.Partner, dst,
                            PartnerAI.GetDir((short)(dst[0] - x), (short)(dst[1] - y)),
                            (ushort)(partnerai.Partner.Speed / 20), true);
                        return;
                    }

                if (index + 1 < path.Count && !partnerai.Partner.Status.Additions.ContainsKey("SkillCast"))
                {
                    node = path[index];
                    var dst = new short[2]
                    {
                        Global.PosX8to16(node.x, partnerai.map.Width), Global.PosY8to16(node.y, partnerai.map.Height)
                    };
                    partnerai.map.MoveActor(Map.MOVE_TYPE.START, partnerai.Partner, dst,
                        PartnerAI.GetDir((short)(dst[0] - x), (short)(dst[1] - y)),
                        (ushort)(partnerai.Partner.Speed / 20), true);
                    if (partnerai.Mode.isAnAI)
                        partnerai.CannotAttack = DateTime.Now.AddMilliseconds(1500);
                }
                else
                {
                    if (path.Count == 0)
                    {
                        Status = CommandStatus.FINISHED;
                        return;
                    }

                    node = path[path.Count - 1];
                    var dst = new short[2]
                    {
                        Global.PosX8to16(node.x, partnerai.map.Width), Global.PosY8to16(node.y, partnerai.map.Height)
                    };
                    if (partnerai.map.GetActorsArea(dst[0], dst[1], 50).Count > 0 && !ifNeko)
                    {
                        partnerai.map.FindFreeCoord(chasedest.X, chasedest.Y, out x, out y, partnerai.Partner);
                        path = partnerai.FindPath(Global.PosX16to8(partnerai.Partner.X, partnerai.map.Width),
                            Global.PosY16to8(partnerai.Partner.Y, partnerai.map.Height),
                            Global.PosX16to8(x, partnerai.map.Width), Global.PosY16to8(y, partnerai.map.Height));
                        index = 0;
                        return;
                    }

                    partnerai.map.MoveActor(Map.MOVE_TYPE.START, partnerai.Partner, dst,
                        PartnerAI.GetDir((short)(dst[0] - x), (short)(dst[1] - y)),
                        (ushort)(partnerai.Partner.Speed / 20), true);
                    if (PartnerAI.GetLengthD(partnerai.Partner.X, partnerai.Partner.Y, chasedest.X, chasedest.Y) >
                        50 + size * 100 && !partnerai.Mode.RunAway)
                    {
                        if (partnerai.Partner.MapID != chasedest.MapID)
                        {
                            Status = CommandStatus.FINISHED;
                            return;
                        }

                        path = partnerai.FindPath(Global.PosX16to8(partnerai.Partner.X, partnerai.map.Width),
                            Global.PosY16to8(partnerai.Partner.Y, partnerai.map.Height),
                            Global.PosX16to8(chasedest.X, partnerai.map.Width),
                            Global.PosY16to8(chasedest.Y, partnerai.map.Height));
                        index = -1;
                    }
                    else
                    {
                        Status = CommandStatus.FINISHED;
                        return;
                    }
                }

                index++;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex, null);
                Status = CommandStatus.FINISHED;
            }
        }

        public CommandStatus Status { get; set; }

        public void Dispose()
        {
            Status = CommandStatus.FINISHED;
        }

        public void FindPath()
        {
            path = partnerai.FindPath(Global.PosX16to8(partnerai.Partner.X, partnerai.map.Width),
                Global.PosY16to8(partnerai.Partner.Y, partnerai.map.Height), Global.PosX16to8(x, partnerai.map.Width),
                Global.PosY16to8(y, partnerai.map.Height));
            index = 0;
        }

        public List<MapNode> GetPath()
        {
            return path;
        }
    }
}