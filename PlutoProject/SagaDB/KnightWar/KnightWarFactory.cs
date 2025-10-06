using System;
using System.Linq;
using System.Xml;
using SagaLib;

namespace SagaDB.KnightWar
{
    public class KnightWarFactory : Factory<KnightWarFactory, KnightWar>
    {
        public KnightWarFactory()
        {
            loadingTab = "Loading KnightWar database";
            loadedTab = " KightWars loaded.";
            databaseName = "KnightWaw";
            FactoryType = FactoryType.XML;
        }

        /// <summary>
        ///     下一场要进行的骑士团演习
        /// </summary>
        /// <returns>要上映的电影</returns>
        public KnightWar GetNextKnightWar()
        {
            var time = new DateTime(1970, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            var query =
                from movie in Items.Values
                where movie.StartTime > time
                orderby movie.StartTime
                select movie;
            if (query.Count() != 0)
                return query.First();
            {
                query =
                    from movie in Items.Values
                    orderby movie.StartTime
                    select movie;
                if (query.Count() != 0)
                    return query.First();
                return null;
            }
        }

        /// <summary>
        ///     当前正在进行的骑士团演习
        /// </summary>
        /// <returns></returns>
        public KnightWar GetCurrentMovie()
        {
            var time = new DateTime(1970, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            var query =
                from movie in Items.Values
                where movie.StartTime < time && movie.StartTime + new TimeSpan(0, movie.Duration, 0) > time
                orderby movie.StartTime
                select movie;
            if (query.Count() != 0)
                return query.First();
            return null;
        }


        protected override uint GetKey(KnightWar item)
        {
            return item.ID;
        }

        protected override void ParseCSV(KnightWar item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, KnightWar item)
        {
            switch (root.Name.ToLower())
            {
                case "movie":
                    switch (current.Name.ToLower())
                    {
                        case "id":
                            item.ID = uint.Parse(current.InnerText);
                            break;
                        case "starttime":
                            var buf = current.InnerText.Split(':');
                            var time = new DateTime(1970, 1, 1, int.Parse(buf[0]), int.Parse(buf[1]), 0);
                            item.StartTime = time;
                            break;
                        case "duration":
                            item.Duration = int.Parse(current.InnerText);
                            break;
                    }

                    break;
            }
        }
    }
}