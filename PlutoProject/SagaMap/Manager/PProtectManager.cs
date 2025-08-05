using System.Collections.Generic;
using SagaDB.PProtect;
using SagaLib;

namespace SagaMap.Manager
{
    public class PProtectManager : Singleton<PProtectManager>
    {
        private readonly Dictionary<uint, PProtect> pprotects = new Dictionary<uint, PProtect>();
        private readonly List<PProtect> pprotects_l = new List<PProtect>();

        private uint nowID = 1;

        public void ADD(PProtect pp)
        {
            pp.ID = newID();
            pprotects.Add(pp.ID, pp);
            pprotects_l.Add(pp);
        }

        public PProtect GetPProtect(uint id)
        {
            if (pprotects.ContainsKey(id))
                return pprotects[id];
            return null;
        }

        public void Remove(uint id)
        {
            if (pprotects.ContainsKey(id))
            {
                if (pprotects_l.Contains(pprotects[id]))
                    pprotects_l.Remove(pprotects[id]);
                pprotects.Remove(id);
            }
        }

        public List<PProtect> GetPProtectsOfPage(ushort page, out ushort max, int search = 0)
        {
            var temp = new List<PProtect>();
            if (pprotects_l.Count == 0)
            {
                max = 0;
                return temp;
            }

            var pprotects_t = pprotects_l;
            if (search > 0) pprotects_t = pprotects_l.FindAll(x => x.TaskID == search);


            int p = page;
            if ((page + 1) * 15 > pprotects_t.Count)
                p = pprotects_t.Count / 15;

            for (var i = p * 15; i < pprotects_t.Count; i++) temp.Add(pprotects_t[i]);

            max = (ushort)(pprotects_t.Count / 15 + 1);
            return temp;
        }

        public ushort GetPProtectsPageMax()
        {
            return (ushort)(pprotects_l.Count / 15 + 1);
        }

        private uint newID()
        {
            while (pprotects.ContainsKey(nowID)) nowID++;
            return nowID;
        }
    }
}