using System.Runtime.InteropServices;
using System.Text.Json;
using YukkuriMovieMaker.Plugin;

namespace YmmConsole
{
    public class YmmDebugWindowPlugin : IPlugin
    {
        static YmmDebugWindowPlugin()
        {
            [DllImport("kernel32.dll")]
            static extern bool AllocConsole();
            AllocConsole();
            if (JsonDocument.Parse(File.ReadAllText(@".\user\setting\4.28.2.1\YukkuriMovieMaker.Settings.YMMSettings.json")).RootElement.TryGetProperty("IsKawaiiMode", out JsonElement nameElement)) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; Console.Clear(); }
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
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
    }
}

