using CommandLine;
using PassiveAPKSniffer.Class;
using System;

namespace PassiveAPKSniffer
{
    class Program
    {
        //https://github.com/shivsahni/APKEnum
        //https://github.com/dwisiswant0/apkleaks
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Option>(args)
               .WithParsed(o =>
               {
                   if (o.download_jadx is true)
                   {
                       Core.DownloadFile().GetAwaiter().GetResult();
                   }
                   else if (o.jadx_path is not null)
                   {
                       if (o.apk_path is not null)
                       {
                           if (o.rule_path is not null)
                           {
                               Core core = new Core(o.jadx_path, o.apk_path, o.rule_path);
                           }
                           else
                           {
                               Console.WriteLine(Strings.rulesNotFound);
                               Environment.Exit(-1);
                           }
                       }
                       else
                       {
                           Console.WriteLine(Strings.apkNotFound);
                           Environment.Exit(-1);
                       }

                   }
                   else
                   {
                       Console.WriteLine(Strings.helpMessage);
                       Environment.Exit(-1);
                   }

               });
        }
    }
}
