using System.Collections.Generic;
using System.Linq;
using SagaDB.FlyingCastle;
using SagaLib;

namespace SagaMap.Manager
{
    public class FFGardenManager : Singleton<FFGardenManager>
    {
        private List<FlyingCastle> items = new List<FlyingCastle>();

        public List<FlyingCastle> GetFlyingCastles(int page, out int maxPage)
        {
            var res =
                from r in MapServer.charDB.GetFlyingCastles()
                select r;
            var list = MapServer.charDB.GetFlyingCastles();
            if (list.Count % 15 == 0)
                maxPage = list.Count / 15;
            else
                maxPage = list.Count / 15 + 1;
            res =
                from r in list
                where list.IndexOf(r) >= page * 15 && list.IndexOf(r) < (page + 1) * 15
                select r;
            list = res.ToList();
            return list;
        }
    }
}