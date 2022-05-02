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
                   if (o.jadx_path is not null)
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
                           }
                       }
                       else
                       {
                           Console.WriteLine(Strings.apkNotFound);
                       }

                   }
                   else
                   {
                       Console.WriteLine(Strings.jadxNotFound);
                   }
                   if (o.download_jadx == "yes")
                   {
                       //jadx download
                   }

               });
        }
    }
}
