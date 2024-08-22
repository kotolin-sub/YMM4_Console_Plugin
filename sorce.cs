using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Text.Json;
using YukkuriMovieMaker.Plugin;

namespace YmmConsole
{
    public class YmmDebugWindowPlugin : IPlugin
    {
        static YmmDebugWindowPlugin()
        {
            AllocConsole();

            RedirectOutput();
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE = -12;

        private static void RedirectOutput()
        {
            IntPtr stdOutHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            SafeFileHandle safeStdOutHandle = new SafeFileHandle(stdOutHandle, ownsHandle: false);
            FileStream stdOutStream = new FileStream(safeStdOutHandle, FileAccess.Write);
            StreamWriter stdOutWriter = new StreamWriter(stdOutStream) { AutoFlush = true };
            Console.SetOut(stdOutWriter);

            IntPtr stdErrHandle = GetStdHandle(STD_ERROR_HANDLE);
            SafeFileHandle safeStdErrHandle = new SafeFileHandle(stdErrHandle, ownsHandle: false);
            FileStream stdErrStream = new FileStream(safeStdErrHandle, FileAccess.Write);
            StreamWriter stdErrWriter = new StreamWriter(stdErrStream) { AutoFlush = true };
            Console.SetError(stdErrWriter);
        }

        public static void ConBridge(object text)
        {
            Console.WriteLine(text);
        }

        public string Name => "DebugWindow";
    }

    public class YmmCon
    {
        public static void DebugPrint(object text)
        {
            YmmDebugWindowPlugin.ConBridge(text);
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
    }
    
}
