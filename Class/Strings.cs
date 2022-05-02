using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassiveAPKSniffer.Class
{
    class Strings
    {
        public static string info = "[*]";
        public static string warning = "[!]";

        public static string jadxDownloadLink = "https://github.com/skylot/jadx/releases/download/v1.3.5/jadx-1.3.5.zip";
        public static string jadxNotFound = "PassiveAPKSniffer didn't find a jadx executible file";
        public static string jadxRun = "Everything is looking cool. The Jadx is started";
        public static string jadxFinish = "The Jadx is finished time to static anaylze";
        public static string jadxRunProblem = "There is a problem when decompiling";

        public static string apkNotFound = "PassiveAPKSniffer didn't find APK";
        public static string windowsCommand = "cmd.exe /c ";

        public static string fileListError = "There is an error while file list process";

        public static string rulesNotFound = "Rules.json is not found";
        public static string rulesReadFilesProblem = "There is a problem while reading rules.json";
    }
}
