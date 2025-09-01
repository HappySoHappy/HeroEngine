using HeroEngine.Desktop.Interface;
using HeroEngine.Desktop.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HeroEngine.Desktop
{
    internal static class Program
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandlerDelegate handler, bool add);

        public static int BuildNumber = 0;

        public static ProgramSettings Settings { get; set; } = null!;
        public static FileLogger CrashLog { get; set; } = new FileLogger("CRASH", "crash-report");


        private delegate bool ConsoleCtrlHandlerDelegate(CtrlType ctrlType);
        private static ConsoleCtrlHandlerDelegate handler;
        static void Main()
        {
            handler = new ConsoleCtrlHandlerDelegate(ConsoleCtrlHandler);
            SetConsoleCtrlHandler(handler, true);

            if (IsAlreadyRunning())
            {
                MessageBox.Show("Running multiple instances may lead to issues, such as race conditions, rate limiting or I/O errors. Proceed with caution", "HeroEngine", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ApplicationConfiguration.Initialize();

            SetThreadExecutionState(0x80000000 | 0x00000001);

            LoadConfiguration();


            var color = Settings.Highlight;
            int r = Math.Max(color.R - 30, 0);
            int g = Math.Max(color.G - 30, 0);
            int b = Math.Max(color.B - 30, 0);
            color = Color.FromArgb(r, g, b);

            Theme.Dark.HighlightColor = Settings.Highlight;
            Theme.Dark.HighlightBorderColor = color;

            Theme.Light.HighlightColor = Settings.Highlight;
            Theme.Light.HighlightBorderColor = color;
            switch (Settings.ThemeIndex)
            {
                case 0:
                    Settings.Theme = Theme.Dark;
                    break;

                case 1:
                    Settings.Theme = Theme.Light;
                    break;
            }

            try
            {
                using (var frontend = new FrontendForm())
                {
                    Application.Run(frontend);
                    if (frontend.Success)
                    {
                        using (var bot = new BotForm())
                        {
                            bot.License = frontend.License!;
                            bot.StartPosition = FormStartPosition.Manual;
                            bot.Location = frontend.Location;

                            Application.Run(bot);
                        }
                    }
                }
            } catch (Exception ex)
            {
                // encode stacktrace and message, and send to server for debugging
                CrashLog.Info(ex.Message, ex.StackTrace ?? "<missing stacktrace>");
            }

            SetThreadExecutionState(0x80000000);
        }

        public static void LoadConfiguration()
        {
            try
            {
                string jsonString = File.ReadAllText("settings.json");
                var jsonSettings = new JsonSerializerSettings
                {
                    Converters = { new Interface.ColorConverter() },
                };

                Settings = JsonConvert.DeserializeObject<ProgramSettings>(jsonString)!;
            }
            catch
            {
                Settings = new ProgramSettings()
                {

                };

                SaveConfiguration();
            }
        }

        public static void SaveConfiguration()
        {
            if (Settings == null) return;

            try
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    Converters = { new Interface.ColorConverter() },
                };
                string jsonString = JsonConvert.SerializeObject(Settings, Formatting.Indented, jsonSettings);
                File.WriteAllText("settings.json", jsonString);
            }
            catch
            {

            }
        }

        public static bool IsAlreadyRunning()
        {
            Process currentRunningProcess = Process.GetCurrentProcess();
            Process[] listOfProcs = Process.GetProcessesByName(currentRunningProcess.ProcessName);
            foreach (Process proc in listOfProcs)
            {
                try
                {
                    if ((proc.MainModule!.FileName == currentRunningProcess.MainModule!.FileName) && (proc.Id != currentRunningProcess.Id))
                        return true;
                } catch { }
            }
            return false;
        }

        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(uint esFlags);

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool ConsoleCtrlHandler(CtrlType ctrlType)
        {
            return false;
        }
    }
}
