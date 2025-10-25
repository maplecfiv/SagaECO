using System;
using SagaLib;
using SqlSugar;

namespace SagaDB;

public class SqlSugarHelper // Cannot be a generic class
{
    // Instructions for multi-database usage:
    // For fixed multiple databases, you can pass new SqlSugarScope(List<ConnectionConfig>, db => {}). See the documentation on multi-tenancy.
    // For variable multiple databases, refer to the SaaS sub-database documentation.
    // Use singleton pattern
    public static SqlSugarScope Db = new SqlSugarScope(new ConnectionConfig() {
            ConnectionString = $"datasource={ConfigLoader.LoadDbPath()}/eco.db", // Connection string
            DbType = DbType.Sqlite, // Database type
            IsAutoCloseConnection = true, // If not set to true, manual close is required
            LanguageType = LanguageType.Default
        },
        db => {
            db.Ado.ExecuteCommand("PRAGMA journal_mode = WAL;");
            // (A) Global effect configuration point, generally used for AOP and program startup configurations, effective for all contexts
            // Debugging SQL event, can be removed
            db.Aop.OnLogExecuting = (sql, pars) => {
                // Recommended to get native SQL, performance is OK for version 5.1.4.63
                SagaLib.Logger.ShowInfo(UtilMethods.GetNativeSql(sql, pars));

                // Get non-parameterized SQL, affects performance especially with large SQL and many parameters, use for debugging
                // Console.WriteLine(UtilMethods.GetSqlString(DbType.SqlServer, sql, pars))
            };

            // Add multiple configurations below
            // db.Ado.IsDisableMasterSlaveSeparation = true;

            // Note: For multi-tenancy, configure as many as needed
            // db.GetConnection(i).Aop
        });
}