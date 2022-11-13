using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YonatanMankovich.ComputerTasksTimer
{
    public static class ComputerTaskExecuter
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        public static void Execute(ComputerTask task)
        {
            switch (task)
            {
                case ComputerTask.Shutdown: Shutdown(); break;
                case ComputerTask.Restart: Restart(); break;
                case ComputerTask.Sleep: Sleep(); break;
                case ComputerTask.ScreenOff: ScreenOff(); break;
                case ComputerTask.ScreenOffAndLock: Lock(); ScreenOff(); break;
                case ComputerTask.Lock: Lock(); break;
                case ComputerTask.Hibernate: Hibernate(); break;
                case ComputerTask.SignOut: SignOut(); break;
                default: throw new InvalidEnumArgumentException("Unknown computer task", (int)task, typeof(ComputerTask));
            }
        }

        public static void SignOut()
        {
            ProcessStartNoShell("shutdown.exe", "/l");
        }

        public static void Hibernate()
        {
            ProcessStartNoShell("shutdown.exe", "/h");
        }

        public static void Lock()
        {
            Process.Start("Rundll32.exe", "User32.dll,LockWorkStation");
        }

        public static void ScreenOff()
        {
            SendMessage(0xFFFF, 0x112, 0xF170, 2);
        }

        public static void Sleep()
        {
            Process.Start("rundll32.exe", "powrprof.dll,SetSuspendState 0,1,0");
        }

        public static void Restart()
        {
            ProcessStartNoShell("shutdown.exe", "/r /t 0");
        }

        public static void Shutdown()
        {
            ProcessStartNoShell("shutdown.exe", "/s /t 0");
        }

        private static void ProcessStartNoShell(string fileName, string arguments)
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = fileName,
                Arguments = arguments,
            });
        }
    }
}