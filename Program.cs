using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ProxyChanger
{
    class Program
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static bool settingsReturn, refreshReturn;

        static void Main(string[] args)
        {
            string proxy = String.Empty;
            int enabled = 0;
            if (args.Contains("--enable"))
            {
                var arguments = args.ToList();
                proxy = arguments[arguments.IndexOf("--enable") + 1];
                enabled = 1;
            }
            else if (args.Contains("--disable") || args.Contains("-d")) { }
            else
            {
                Console.WriteLine("Usage: --enable [IP:PORT] ] or -e");
                Console.WriteLine("       --disable or -d");
            }

            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", enabled);
            registry.SetValue("ProxyServer", proxy);

            // These lines implement the Interface in the beginning of program 
            // They cause the OS to refresh the settings, causing IP to realy update
            settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);

            Console.WriteLine("Proxy server successfully " + ((enabled == 1) ? ("set to " + proxy) : "Disabled"));
        }
    }
}
