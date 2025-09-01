namespace HeroEngine.Util
{
    public class FileLogger
    {
        public static bool WriteFile = true;
        public static FileLogger Instance = new FileLogger("HeroEngine", "execution") { WriteConsole = true };

        private string _prefix;
        private readonly string _logs;
        private readonly long _start = UnixTime.Now();

        public string Prefix;
        public string FileName;
        public bool WriteConsole = false;
        public FileLogger(string prefix, string fileName)
        {
            Prefix = prefix;
            FileName = fileName;

            _logs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(_logs);
        }

        public void Info(params string[] messages)
        {
            Console.ForegroundColor = ConsoleColor.White;
            foreach (string message in messages)
            {
                foreach (string subMessage in message.Split("\n"))
                {
                    string msg = subMessage.Replace("\n", "");
                    if (string.IsNullOrEmpty(msg) || string.IsNullOrWhiteSpace(msg)) continue;

                    if (WriteConsole)
                    {
                        Console.WriteLine($"[{FormatTime()}/INFO] [{Prefix.ToUpper()}]: {msg}");
                    }

                    AppendLogToFile($"[Thread{Thread.CurrentThread.ManagedThreadId} @ {FormatTime()}] [{Prefix.ToUpper()}/INFO]: {msg}");
                }
            }
            Console.ResetColor();
        }

        public void Warn(params string[] messages)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (string message in messages)
            {
                foreach (string subMessage in message.Split("\n"))
                {
                    string msg = subMessage.Replace("\n", "");
                    if (string.IsNullOrEmpty(msg) || string.IsNullOrWhiteSpace(msg)) continue;

                    if (WriteConsole)
                    {
                        Console.WriteLine($"[{FormatTime()}/WARN] [{Prefix.ToUpper()}]: {msg}");
                    }

                    AppendLogToFile($"[Thread{Thread.CurrentThread.ManagedThreadId} @ {FormatTime()}] [{Prefix.ToUpper()}/WARN]: {msg}");
                }
            }
            Console.ResetColor();
        }

        private void AppendLogToFile(string message)
        {
            if (!WriteFile) return;

            try
            {
                var fileName = Path.Combine(_logs, $"{FileName}.log");

                using (StreamWriter writer = new StreamWriter(fileName, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception)
            {

            }
        }

        private static string FormatTime()
        {
            string formattedTime = DateTime.Now.ToString("HH:mm:ss");
            return formattedTime;
        }
    }
}
