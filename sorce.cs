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
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        public static void DebugPrint(object text)
        {
            Console.WriteLine(text);
        }
        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public string Name => "DebugWindow";
    }

    public class YmmCon
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);
    }

    public static class YMMConsole
    {
        public static void WriteLine(object text)
        {
            YmmDebugWindowPlugin.DebugPrint(text);
        }
        public static string ReadLine() { 
            return YmmDebugWindowPlugin.ReadLine();
        }
    }
}
