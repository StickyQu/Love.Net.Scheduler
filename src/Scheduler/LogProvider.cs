// Copyright © xu yingting & top001.com. All rights reserved.

using System;
using System.IO;
using System.Text;
using Hangfire.Logging;

namespace Love.Net.Scheduler {
    public class LogProvider : ILogProvider {

        static LogProvider() {
            Logger = new FileLog();
        }

        public static ILog Logger { get; }

        public ILog GetLogger(string name) {
            return Logger;
        }
    }

    public class FileLog : ILog {
        private static string LogPath = Path.Combine(System.AppContext.BaseDirectory, "bin", "Log.txt");

        private static bool _isLogging = false;

        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null) {
            if (_isLogging) {
                return false;
            }

            _isLogging = true;

            try {
                if (messageFunc != null || exception != null) {
                    var fm = FileMode.Append;
                    if (File.Exists(LogPath)) {
                        var fi = new FileInfo(LogPath);
                        // if larger than 2M, discard the old
                        if (fi != null && fi.Length > 2 * 1024 * 1024)
                            fm = FileMode.Truncate;
                    }

                    // write to file
                    using (var fs = new FileStream(LogPath, fm, FileAccess.Write, FileShare.Write)) {
                        var sb = new StringBuilder();
                        sb.AppendLine(string.Format("--------------------- [{0}] ----------- {1} --------------------- ", logLevel, DateTime.Now));
                        if (messageFunc != null) {
                            sb.AppendLine(messageFunc());
                        }
                        if (exception != null) {
                            sb.AppendLine(exception.ToString());
                        }
                        sb.AppendLine();

                        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Flush();
                    }
                }
            }
            catch {
                return false;
            }
            finally {
                _isLogging = false;
            }

            return true;
        }
    }
}