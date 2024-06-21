using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics;
using System.IO;
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
            var settingsPath = @".\user\setting\4.28.2.1\YukkuriMovieMaker.Settings.YMMSettings.json";
            if (JsonDocument.Parse(File.ReadAllText(settingsPath)).RootElement.TryGetProperty("IsKawaiiMode", out JsonElement nameElement))
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
            }

            // 標準出力のリダイレクト
            IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, ownsHandle: false);
            FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
            StreamWriter standardOutput = new StreamWriter(fileStream) { AutoFlush = true };
            Console.SetOut(standardOutput);
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private const int STD_OUTPUT_HANDLE = -11;

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
