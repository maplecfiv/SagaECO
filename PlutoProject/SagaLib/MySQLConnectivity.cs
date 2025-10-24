using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using MySql.Data.MySqlClient;

namespace SagaLib {
    public class SqlExecuteScalarResult {
        public bool IsSuccess { set; get; }

        public uint Index { set; get; }

        public SqlExecuteScalarResult(bool isSuccess, uint index) {
            IsSuccess = isSuccess;
            Index = index;
        }
    }

    public abstract class MySQLConnectivity {
        private readonly Thread mysqlPool;

        private readonly List<MySqlCommand> waitQueue = new List<MySqlCommand>();
        internal int cuurentCount;
        protected MySqlConnection db;
        protected MySqlConnection dbinactive;

        public MySQLConnectivity() {
            mysqlPool = new Thread(ProcessMysql);
            mysqlPool.Start();
        }

        public bool CanClose {
            get {
                lock (waitQueue) {
                    return waitQueue.Count == 0 && cuurentCount == 0;
                }
            }
        }

        private void ProcessMysql() {
            while (true)
                try {
                    MySqlCommand[] cmds;
                    lock (waitQueue) {
                        if (waitQueue.Count > 0) {
                            cmds = waitQueue.ToArray();
                            waitQueue.Clear();
                            cuurentCount = cmds.Length;
                        }
                        else {
                            cmds = new MySqlCommand[0];
                        }
                    }

                    if (cmds.Length > 0) {
                        var pending = new List<MySqlCommand>();
                        DatabaseWaitress.EnterCriticalArea();

                        foreach (var i in cmds)
                            try {
                                i.Command.Connection = db;
                                switch (i.Type) {
                                    case MySqlCommand.CommandType.NonQuery:
                                        i.Command.ExecuteNonQuery();
                                        break;
                                    case MySqlCommand.CommandType.Query:
                                        var adapter = new MySqlDataAdapter(i.Command);
                                        var set = new DataSet();
                                        adapter.Fill(set);
                                        i.DataRows = set.Tables[0].Rows;
                                        break;
                                    case MySqlCommand.CommandType.Scalar:
                                        i.Scalar = Convert.ToUInt32(i.Command.ExecuteScalar());
                                        break;
                                }
                            }
                            catch (Exception ex) {
                                Logger.ShowSQL("Error on query:" + command2String(i.Command), Logger.defaultlogger);
                                Logger.ShowSQL(ex, Logger.defaultlogger);
                                i.ErrorCount++;
                                if (i.ErrorCount > 10)
                                    Logger.ShowSQL("Error to many times, dropping command", Logger.defaultlogger);
                                else
                                    pending.Add(i);
                            }

                        DatabaseWaitress.LeaveCriticalArea();
                        if (pending.Count > 0)
                            lock (waitQueue) {
                                foreach (var i in pending) waitQueue.Add(i);
                            }

                        pending = null;
                    }

                    cmds = null;
                    cuurentCount = 0;
                    Thread.Sleep(10);
                }
                catch (ThreadAbortException) {
                    DatabaseWaitress.LeaveCriticalArea();
                }
        }

        public bool SQLExecuteNonQuery(string sqlstr) {
            lock (waitQueue) {
                var cmd = new MySqlCommand(new MySql.Data.MySqlClient.MySqlCommand(sqlstr));
                waitQueue.Add(cmd);
            }

            return true;
        }

        private string command2String(MySql.Data.MySqlClient.MySqlCommand cmd) {
            string output;
            output = cmd.CommandText;
            if (cmd.Parameters.Count > 0) {
                var para = "";
                foreach (MySqlParameter i in cmd.Parameters)
                    para += string.Format("{0}={1},", i.ParameterName, value2String(i.Value));
                para = para.Substring(0, para.Length - 1);
                output = string.Format("{0} VALUES({1})", output, para);
            }

            return output;
        }

        private string value2String(object val) {
            if (val.GetType() == typeof(byte[])) {
                var tmp = (byte[])val;
                return "0x" + Conversions.bytes2HexString(tmp);
            }

            return val.ToString();
        }

        public bool SqlExecuteNonQuery(MySql.Data.MySqlClient.MySqlCommand cmd) {
            lock (waitQueue) {
                waitQueue.Add(new MySqlCommand(cmd));
            }

            return true;
        }

        public SqlExecuteScalarResult SqlExecuteScalar(string sqlstr) {
            uint index = 0;
            var criticalarea = ClientManager.Blocked;
            var result = false;
            if (criticalarea) {
                ClientManager.LeaveCriticalArea();
            }

            try {
                if (sqlstr.Substring(sqlstr.Length - 1) != ";") {
                    sqlstr += ";";
                }

                sqlstr += "SELECT LAST_INSERT_ID();";
                var cmd = new MySqlCommand(new MySql.Data.MySqlClient.MySqlCommand(sqlstr),
                    MySqlCommand.CommandType.Scalar);
                lock (waitQueue) {
                    waitQueue.Add(cmd);
                }

                while (cmd.Scalar == uint.MaxValue) Thread.Sleep(10);
                index = cmd.Scalar;
                result = true;
            }
            catch (Exception ex) {
                Logger.ShowSQL(ex, Logger.defaultlogger);
            }

            if (criticalarea) {
                ClientManager.EnterCriticalArea();
            }

            return new SqlExecuteScalarResult(result, index);
        }

        public DataRowCollection SqlExecuteQuery(string sqlstr) {
            DataRowCollection result;
            DataSet tmp;
            var criticalarea = ClientManager.Blocked;
            if (criticalarea)
                ClientManager.LeaveCriticalArea();
            try {
                var cmd = new MySqlCommand(new MySql.Data.MySqlClient.MySqlCommand(sqlstr),
                    MySqlCommand.CommandType.Query);
                lock (waitQueue) {
                    waitQueue.Add(cmd);
                }

                while (cmd.DataRows == null) Thread.Sleep(10);
                result = cmd.DataRows;
                if (criticalarea)
                    ClientManager.EnterCriticalArea();
                return result;
            }
            catch (Exception ex) {
                Logger.ShowSQL("Error on query:" + sqlstr, Logger.defaultlogger);
                Logger.ShowSQL(ex, Logger.defaultlogger);
                if (criticalarea)
                    ClientManager.EnterCriticalArea();
                return null;
            }
        }

        public string ToSqlDateTime(DateTime date) {
            return string.Format("{0}-{1}-{2} {3}:{4}:{5}", date.Year, date.Month, date.Day, date.Hour, date.Minute,
                date.Second);
        }


        public string CheckSqlString(string str) {
            return str.Replace("\\", "").Replace("'", "\\'");
        }

        private class MySqlCommand {
            public enum CommandType {
                NonQuery,
                Query,
                Scalar
            }

            public MySqlCommand(MySql.Data.MySqlClient.MySqlCommand cmd) {
                Command = cmd;
                Type = CommandType.NonQuery;
            }

            public MySqlCommand(MySql.Data.MySqlClient.MySqlCommand cmd, CommandType type) {
                Command = cmd;
                Type = type;
            }

            public MySql.Data.MySqlClient.MySqlCommand Command { get; }

            public DataRowCollection DataRows { get; set; }

            public CommandType Type { get; }

            public uint Scalar { get; set; } = uint.MaxValue;

            public int ErrorCount { get; set; }
        }
    }
}