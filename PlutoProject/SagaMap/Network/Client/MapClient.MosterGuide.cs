using System.Collections.Generic;
using System.Linq;
using SagaDB.Mob;
using SagaLib;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void OnNewMosterDiscover(uint mobID)
        {
            if (MobFactory.Instance.Mobs.ContainsKey(mobID))
            {
                var mob = MobFactory.Instance.Mobs[mobID];
                if (mob.guideFlag == 1)
                {
                    var p = new SSMG_MOSTERGUIDE_NEW_RECORD();
                    p.guideID = mob.guideID;
                    netIO.SendPacket(p);
                }
            }
        }

        public void SendMosterGuide()
        {
            var MobList =
                (from m in MobFactory.Instance.Mobs.Values where m.guideFlag == 3 orderby m.guideID select m)
                .ToDictionary(m => m.guideID, m => m.id);

            //switch m.guideFlag to 1 when enabled
            var boolstates = new bool[MobList.Keys.Max()];

            for (short i = 0; i < boolstates.Length; i++)
            {
                var state = false;
                if (MobList.ContainsKey(i))
                    if (Character.MosterGuide.ContainsKey(MobList[i]))
                        state = Character.MosterGuide[MobList[i]];
                boolstates[i] = state;
            }

            var masks = new List<BitMask>();
            byte index = 0;
            var BitmaskSize = 32;
            var skip = BitmaskSize * index;
            while (skip < boolstates.Length)
            {
                var items = boolstates.Select(x => x).Skip(skip).Take(BitmaskSize).ToArray();
                masks.Add(new BitMask(items));
                index++;
                skip = BitmaskSize * index;
            }

            var p = new SSMG_MOSTERGUIDE_RECORDS();
            p.Records = masks;
            netIO.SendPacket(p);
        }
    }
}