using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using WindowsInput;
using WindowsInput.Native;

namespace COMSender;

public class Actions
{
    public class ActionExecutor
    {
        
        public static void ActivateWindow(string name) {
            var processes = Process.GetProcessesByName(name);
            if (processes.Length > 0) {
                SetForegroundWindow(processes[0].MainWindowHandle);
            }
        }

        public static void SendKeys(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            InputSimulator sim = new InputSimulator();
            sim.Keyboard.ModifiedKeyStroke(modifierKeyCodes, keyCode);
        }
        
        public static void SendKeysTo(string name, IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            Process current = Process.GetCurrentProcess();
            Console.WriteLine(current.ProcessName);
            Thread.Sleep(20);
            ActivateWindow(name);
            SendKeys(modifierKeyCodes, keyCode);
            Thread.Sleep(20);
            SetForegroundWindow(current.MainWindowHandle);
        }
        
        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, buff, nChars) > 0)
            {
                return buff.ToString();
            }
            return "";
        }
        
        public static string GetActiveProcessFileName()
        {
            IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p.MainModule?.FileName ?? "";
        }
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
        
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
    }
}