using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Serilog;

namespace SagaLib
{
    public class Logger
    {
        private static readonly Serilog.Core.Logger _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        public static Serilog.Core.Logger GetLogger()
        {
            return _logger;
        }

        public static Serilog.Core.Logger InitLogger<T>()
        {
            {
                // instantiate and configure logging. Using serilog here, to log to console and a text-file.
                return _logger;
            }
        }

        public enum EventType
        {
            ItemGolemGet = 1,
            ItemLootGet = 2,
            ItemWareGet = 4,
            ItemNPCGet = 8,
            ItemVShopGet = 16,
            ItemTradeGet = 32,
            ItemGMGet = 64,
            ItemGolemLost = 128,
            ItemUseLost = 256,
            ItemWareLost = 512,
            ItemNPCLost = 1024,
            ItemTradeLost = 2048,
            ItemDropLost = 4096,
            GoldChange = 8192,
            WarehouseGet = 16384,
            WarehousePut = 32768,
            GMCommand = 65536
        }

        public static MySQLConnectivity defaultSql;

        public static BitMask<EventType> SQLLogLevel = new BitMask<EventType>(new BitMask(68712));

        private static string GetStackTrace()
        {
            var trace = new StackTrace().ToString().Split('\n');
            var Stacktrace = "";
            foreach (var i in trace)
            {
                if (i.Contains(" System."))
                    continue;
                Stacktrace += i.Replace("\r", "\r\n");
            }

            var size = 1024;
            if (size > Stacktrace.Length)
                size = Stacktrace.Length;
            Stacktrace.Substring(0, size);
            return Stacktrace;
        }

        public static void LogItemGet(EventType type, string pc, string item, string detail, bool stack)
        {
            if (type >= EventType.ItemGolemGet && type <= EventType.ItemGMGet)
                if (defaultSql != null && SQLLogLevel.Test(type))
                {
                    var content = detail;
                    if (stack)
                        content += "\r\n" + GetStackTrace();
                    SQLLog(type, pc, item, content);
                }
        }

        public static void LogItemLost(EventType type, string pc, string item, string detail, bool stack)
        {
            if (type >= EventType.ItemGolemLost && type <= EventType.ItemDropLost)
                if (defaultSql != null && SQLLogLevel.Test(type))
                {
                    var content = detail;
                    if (stack)
                        content += "\r\n" + GetStackTrace();
                    SQLLog(type, pc, item, content);
                }
        }

        public static void LogGoldChange(string pc, int amount)
        {
            if (defaultSql != null && SQLLogLevel.Test(EventType.GoldChange))
                SQLLog(EventType.GoldChange, pc, amount.ToString(), GetStackTrace());
        }

        public static void LogWarehouseGet(string pc, string item, string detail)
        {
            if (defaultSql != null && SQLLogLevel.Test(EventType.WarehouseGet))
                SQLLog(EventType.WarehouseGet, pc, item, detail);
        }

        public static void LogWarehousePut(string pc, string item, string detail)
        {
            if (defaultSql != null && SQLLogLevel.Test(EventType.WarehouseGet))
                SQLLog(EventType.WarehousePut, pc, item, detail);
        }

        public static void LogGMCommand(string pc, string item, string detail)
        {
            if (defaultSql != null && SQLLogLevel.Test(EventType.GMCommand))
                SQLLog(EventType.GMCommand, pc, item, detail);
        }

        private static void SQLLog(EventType type, string src, string dst, string detail)
        {
            var time = DateTime.Now;
            src = defaultSql.CheckSQLString(src);
            var sql = string.Format(
                "INSERT INTO `log`(`eventType`,`eventTime`,`src`,`dst`,`detail`) VALUES ('{0}','{1}','{2}','{3}','{4}');",
                type, defaultSql.ToSQLDateTime(time), defaultSql.CheckSQLString(src), defaultSql.CheckSQLString(dst),
                defaultSql.CheckSQLString(detail));
            try
            {
                defaultSql.SQLExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public enum LogContent
        {
            Info = 1,
            Warning = 2,
            Error = 4,
            SQL = 8,
            Debug = 16,
            Custom = 32
        }

        public static Logger defaultlogger;
        public static Logger CurrentLogger = defaultlogger;
        public static Serilog.Core.Logger DefaultLogger = null;
        private readonly string filename;

        public LogContent LogLevel = (LogContent)31;
        private string path;

        // public Logger(string filename)
        // {
        //     this.filename = filename;
        //     path = GetLogFile();
        //     if (!File.Exists(path))
        //     {
        //         var f = File.Create(path);
        //         f.Close();
        //     }
        // }


        /*public Logger(string path)
        {
            if (!System.IO.Directory.Exists("Log"))
                System.IO.Directory.CreateDirectory("Log");
            this.path = path;
            if (!File.Exists(path))
            {
                System.IO.File.Create(path);
            }
        }*/

        public void WriteLog(string p)
        {
            try
            {
                // path = GetLogFile();
                // var file = new FileStream(path, FileMode.Append);
                // var sw = new StreamWriter(file);
                var final = GetDate() + "|" +
                            p; // Add character to make exploding string easier for reading specific log entry by ReadLog()
                // sw.WriteLine(final);
                // sw.Close();
                _logger.Information(final);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public void WriteLog(string prefix, string p)
        {
            try
            {
                // path = GetLogFile();
                p = $"{prefix}->{p}";
                // var file = new FileStream(path, FileMode.Append);
                // var sw = new StreamWriter(file);
                var final = GetDate() + "|" +
                            p; // Add character to make exploding string easier for reading specific log entry by ReadLog()
                // sw.WriteLine(final);
                // sw.Close();
                _logger.Information(final);
            }
            catch (Exception exception)
            {
                ShowError(exception, null);
            }
        }

        public static void ShowInfo(Exception ex, Logger log)
        {
            // if ((defaultlogger.LogLevel | LogContent.Info) != defaultlogger.LogLevel)
            //     return;
            ////Console.ForegroundColor = ConsoleColor.Green;
            _logger.Debug($"[Info] {ex.Message}\r\n {ex.StackTrace}");
            ////Console.ResetColor();
            // _logger.Debug(ex.Message + "\r\n" + ex.StackTrace);
            // if (log != null) log.WriteLog(ex.Message);
        }

        public static void ShowInfo(string ex)
        {
            // if ((defaultlogger.LogLevel | LogContent.Info) != defaultlogger.LogLevel)
            //     return;
            ////Console.ForegroundColor = ConsoleColor.Green;
            _logger.Debug($"[Info] {ex}");
            ////Console.ResetColor();
            // _logger.Debug(ex);
        }

        public static void ShowInfo(string ex, Logger log)
        {
            // if ((defaultlogger.LogLevel | LogContent.Info) != defaultlogger.LogLevel)
            //     return;
            ////Console.ForegroundColor = ConsoleColor.Green;
            _logger.Debug($"[Info] {ex}");
            ////Console.ResetColor();
            // _logger.Debug(ex);
            // if (log != null) log.WriteLog(ex);
        }

        public static void ShowSQL(Exception ex)
        {
            LoggerIntern.EnqueueMsg(Level.SQL, ex.ToString(), DefaultLogger);
        }

        public static void ShowSQL(string ex)
        {
            LoggerIntern.EnqueueMsg(Level.SQL, ex, DefaultLogger);
        }

        public static void ShowWarning(Exception ex)
        {
            // if ((defaultlogger.LogLevel | LogContent.Warning) != defaultlogger.LogLevel)
            //     return;
            ShowWarning(ex, defaultlogger);
        }

        public static void ShowWarning(string ex)
        {
            // if ((defaultlogger.LogLevel | LogContent.Warning) != defaultlogger.LogLevel)
            //     return;
            ShowWarning(ex, defaultlogger);
        }

        public static void ShowDebug(Exception ex, Logger log)
        {
            // if ((defaultlogger.LogLevel | LogContent.Debug) != defaultlogger.LogLevel)
            //     return;
            ////Console.ForegroundColor = ConsoleColor.Cyan;
            _logger.Debug($"[Debug] {ex.Message} \r\n {ex.StackTrace}");
            ////Console.ForegroundColor = ConsoleColor.White;
            // _logger.Debug(ex.Message + "\r\n" + ex.StackTrace);
            ////Console.ResetColor();
            // if (log != null)
            //     log.WriteLog("[Debug]" + ex.Message + "\r\n" + ex.StackTrace);
        }

        public static void ShowDebug(string ex, Logger log)
        {
            // if ((defaultlogger.LogLevel | LogContent.Debug) != defaultlogger.LogLevel)
            //     return;
            ////Console.ForegroundColor = ConsoleColor.Cyan;
            // _logger.Debug("[Debug]");
            ////Console.ForegroundColor = ConsoleColor.White;
            var Stacktrace = new StackTrace(1, true);
            var txt = ex;
            foreach (var i in Stacktrace.GetFrames())
                txt = txt + "\r\n      at " + i.GetMethod().ReflectedType.FullName + "." + i.GetMethod().Name + " " +
                      i.GetFileName() + ":" + i.GetFileLineNumber();
            txt = FilterSQL(txt);
            // _logger.Debug(txt);
            _logger.Debug($"[Debug] {txt}");
            ////Console.ResetColor();
            // if (log != null) log.WriteLog("[Debug]" + txt);
        }

        public static void ShowSQL(Exception ex, Logger log)
        {
            // if ((defaultlogger.LogLevel | LogContent.SQL) != defaultlogger.LogLevel)
            //     return;
            ////Console.ForegroundColor = ConsoleColor.Magenta;
            _logger.Debug($"[SQL] {ex.Message} \r\n {FilterSQL(ex.StackTrace)}");
            ////Console.ForegroundColor = ConsoleColor.White;
            // _logger.Debug(ex.Message + "\r\n" + FilterSQL(ex.StackTrace));
            ////Console.ResetColor();
            // if (log != null)
            //     log.WriteLog("[SQL]" + ex.Message + "\r\n" + FilterSQL(ex.StackTrace));
        }

        private static string FilterSQL(string input)
        {
            var tmp = input.Split('\n');
            var tmp2 = "";
            foreach (var i in tmp)
                if (!i.Contains(" MySql.") && !i.Contains(" System."))
                    tmp2 = tmp2 + i + "\n";
            return tmp2;
        }

        public static void ShowSQL(string ex, Logger log)
        {
            // if ((defaultlogger.LogLevel | LogContent.SQL) != defaultlogger.LogLevel)
            //     return;
            ////Console.ForegroundColor = ConsoleColor.Magenta;
            _logger.Debug($"[SQL] {ex}");
            ////Console.ForegroundColor = ConsoleColor.White;
            // _logger.Debug(ex);
            ////Console.ResetColor();
            // if (log != null)
            //     log.WriteLog("[SQL]" + ex);
        }

        public static void ShowWarning(Exception ex, Logger log)
        {
            ShowError(ex, log);
        }

        public static void ShowWarning(string ex, Logger log)
        {
            _logger.Warning(ex);
        }

        public static void ShowError(Exception ex, Logger log)
        {
            ShowError(ex);
        }

        public static void ShowError(string ex, Logger log)
        {
            ShowError(ex);
        }

        public static void ShowError(string ex)
        {
            _logger.Error(ex);
        }

        public static void ShowError(Exception ex)
        {
            _logger.Error(ex, ex.Message);
        }

        public string GetLogFile()
        {
            // Read in from XML here if needed.
            if (!Directory.Exists("Log"))
                Directory.CreateDirectory("Log");

            return "Log/[" +
                   string.Format("{0}-{1}-{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day) + "]" +
                   filename;
        }

        public string GetDate()
        {
            return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        public static void ProgressBarShow(uint progressPos, uint progressTotal, string label)
        {
            ////Console.ForegroundColor = ConsoleColor.Green;
            _logger.Debug("\r[Info]");
            ////Console.ResetColor();
            _logger.Debug("{0} [", label);
            var sb = new StringBuilder();
            //sb.AppendFormat("\r{0} [", label);
            var barPos = progressPos * 40 / progressTotal + 1;
            for (uint p = 0; p < barPos; p++) sb.AppendFormat("#");
            for (var p = barPos; p < 40; p++) sb.AppendFormat(" ");
            sb.AppendFormat("] {0}%\r", progressPos * 100 / progressTotal);
            ////Console.ForegroundColor = ConsoleColor.White;
            _logger.Debug(sb.ToString());
            ////Console.ResetColor();
        }

        public static void ProgressBarHide(string label)
        {
            //char[] buffer = new char[80];
            //label.CopyTo(0, buffer, 0, label.Length);
            //_logger.Debug(buffer);
            ////Console.ForegroundColor = ConsoleColor.Green;
            _logger.Debug("\r[Info]");
            ////Console.ResetColor();
            _logger.Debug(
                "{0}                                                                                            \r",
                label);
        }
    }
}