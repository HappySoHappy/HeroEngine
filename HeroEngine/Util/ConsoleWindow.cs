using System.Runtime.InteropServices;

namespace HeroEngine.Util
{
    public static class ConsoleWindow
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        private static int VISIBILITY_STATE = SW_HIDE;

        public static void ShowConsole()
        {
            IntPtr handle = GetConsoleWindow();
            if (handle == IntPtr.Zero)
            {
                AllocConsole();
                handle = GetConsoleWindow();
                FileLogger.Instance.Info("Allocated console window to application");
            }

            if (handle != IntPtr.Zero)
            {
                ShowWindow(handle, SW_SHOW);
                VISIBILITY_STATE = SW_SHOW;
            }
               
        }

        public static void HideConsole()
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero){
                ShowWindow(handle, SW_HIDE);
                VISIBILITY_STATE = SW_HIDE;
            }
        }

        public static void KillConsole()
        {
            IntPtr handle = GetConsoleWindow();
            if (handle != IntPtr.Zero)
            {
                FreeConsole();
                VISIBILITY_STATE = SW_HIDE;
                FileLogger.Instance.Info("Freed console window from application");
            }
        }

        public static bool IsConsoleVisible()
        {
            return GetConsoleWindow() != IntPtr.Zero && VISIBILITY_STATE == SW_SHOW;
        }
    }
}
