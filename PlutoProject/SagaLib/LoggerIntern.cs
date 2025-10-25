using System.Collections.Concurrent;
using System.Threading;

namespace SagaLib {
    public struct LogData {
        public Level LogLevel;
        public string Text;
        public Serilog.Core.Logger Logger;
    }

    public enum Level {
        Debug,
        Info,
        Warn,
        Error,
        SQL
    }

    public class LoggerIntern {
        private static readonly ConcurrentQueue<LogData> queue = new ConcurrentQueue<LogData>();
        private static Thread thread;
        private static readonly AutoResetEvent waiter = new AutoResetEvent(false);
        public static bool Ready;

        public static void Init() {
            if (thread == null) {
                thread = new Thread(MainLoop);
                thread.Start();
                Ready = true;
            }
        }

        private static void MainLoop() {
            while (true) {
                while (queue.TryDequeue(out var data))
                    switch (data.LogLevel) {
                        case Level.Debug:
                            data.Logger.Information(data.Text);
                            break;
                        case Level.Info:
                            data.Logger.Information(data.Text);
                            break;
                        case Level.Warn:
                            data.Logger.Warning(data.Text);
                            break;
                        case Level.Error:
                            data.Logger.Error(data.Text);
                            break;
                        case Level.SQL:
                            data.Logger.Information(data.Text);
                            break;
                    }

                waiter.WaitOne();
            }
        }

        public static void EnqueueMsg(Level level, string msg, Serilog.Core.Logger logger) {
            var data = new LogData();
            data.LogLevel = level;
            data.Text = msg;
            data.Logger = logger;
            queue.Enqueue(data);
            waiter.Set();
        }
    }
}