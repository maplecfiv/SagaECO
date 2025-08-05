using System;

namespace SagaDB.Theater
{
    /// <summary>
    ///     电影类
    /// </summary>
    public class Movie
    {
        /// <summary>
        ///     电影的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     影院地图ID
        /// </summary>
        public uint MapID { get; set; }

        /// <summary>
        ///     电影票ID
        /// </summary>
        public uint Ticket { get; set; }

        /// <summary>
        ///     电影网址，mms流地址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        ///     上映时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     电影长度
        /// </summary>
        public int Duration { get; set; }
    }
}