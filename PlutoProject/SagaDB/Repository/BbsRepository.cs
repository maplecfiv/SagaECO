using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Entities;

namespace SagaDB.Repository;

public class BbsRepository {
    public static bool Insert(ActorPC poster, uint bbsId, string title, string content) {
        try {
            SqlSugarHelper.Db.BeginTran();

            SqlSugarHelper.Db.Insertable<Bbs>(new Bbs {
                BbsId = bbsId,
                PostDate = DateTime.Now.ToUniversalTime(),
                CharId = poster.CharID,
                Name = poster.Name,
                Title = title,
                Content = content
            }).ExecuteCommand();

            SqlSugarHelper.Db.CommitTran();
            return true;
        }
        catch (Exception e) {
            SqlSugarHelper.Db.RollbackTran();
            SagaLib.Logger.ShowError(e);
            return false;
        }
    }

    public static List<Bbs> GetBbs(uint bbsId) {
        return GetBbs(bbsId, -1, -1);
    }

    public static List<Bbs> GetBbs(uint bbsId, int paging, int offset) {
        if (paging == -1) {
            return SqlSugarHelper.Db.Queryable<Bbs>().Where(item => item.BbsId == bbsId)
                .OrderByDescending(item => item.BbsId).ToList();
        }

        return SqlSugarHelper.Db.Queryable<Bbs>().Where(item => item.BbsId == bbsId)
            .OrderByDescending(item => item.BbsId).ToPageList(paging, offset);
    }
}