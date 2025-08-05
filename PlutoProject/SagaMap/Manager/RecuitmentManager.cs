using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Manager
{
    public enum RecruitmentType
    {
        Party = 1,
        Item,
        Info,
        Team
    }

    public class Recruitment
    {
        public ActorPC Creator { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public RecruitmentType Type { get; set; }
    }

    public class RecruitmentManager : Singleton<RecruitmentManager>
    {
        private readonly List<Recruitment> items = new List<Recruitment>();

        public void CreateRecruiment(Recruitment rec)
        {
            var res =
                from r in items
                where r.Creator == rec.Creator
                select r;

            if (res.Count() != 0)
            {
                items.Remove(res.First());
                items.Add(rec);
            }
            else
            {
                items.Add(rec);
            }
        }

        public void DeleteRecruitment(ActorPC creator)
        {
            var res =
                from r in items
                where r.Creator == creator
                select r;

            if (res.Count() != 0) items.Remove(res.First());
        }

        public List<Recruitment> GetRecruitments(RecruitmentType type, int page, out int maxPage)
        {
            var res =
                from r in items
                where r.Type == type
                select r;
            var list = res.ToList();
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

        public List<Recruitment> GetRecruitments(RecruitmentType type)
        {
            var res =
                from r in items
                where r.Type == type
                select r;
            var list = res.ToList();
            return list;
        }
    }
}