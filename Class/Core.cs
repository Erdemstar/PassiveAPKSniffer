using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassiveAPKSniffer.Class
{
    class Core
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool fileControl(string file)
        {
            if (File.Exists(file))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processInfo"></param>
        /// <returns></returns>
        public bool RunCommand(ProcessStartInfo processInfo)
        {
            try
            {
                var process = new Process()
                {
                    StartInfo = processInfo,
                };
                process.Start();
                process.WaitForExit();

                return true;
            }
            catch
            {
                return false;
            }


        }
        /// <summary>
        /// 
        /// </summary>
        ///   /// <summary>
        /// 
        /// </summary>
        /// <param name="jadxPath"></param>
        /// <param name="apkPath"></param>
        /// <returns></returns>
        public string Decompile(string jadxPath, string apkPath)
        {

            var commandResult = RunCommand(new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/c " + jadxPath + " " + apkPath,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false
            });
            if (commandResult is false)
            {
                return null;
            }

            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var apkName = Path.GetFileNameWithoutExtension(apkPath);

            return exePath + apkName;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public IEnumerable<string> fileList(string folderPath)
        {
            try
            {
                var allfiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".java") || s.EndsWith(".xml"));
                return allfiles;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jadxPath"></param>
        /// <param name="apkPath"></param>

        public Core(string jadxPath, string apkPath, string rulePath)
        {
            var jadxPathExist = fileControl(jadxPath);
            if (jadxPathExist is false)
            {

                Console.WriteLine(Strings.jadxNotFound);
                Environment.Exit(-1);
            }

            var apkPathExist = fileControl(apkPath);
            if (apkPathExist is false)
            {
                Console.WriteLine(Strings.apkNotFound);
                Environment.Exit(-1);
            }

            var rulePathExist = fileControl(rulePath);
            if (rulePathExist is false)
            {
                Console.WriteLine(Strings.rulesNotFound);
                Environment.Exit(-1);
            }

            Console.WriteLine(Strings.info + "Jadx Path : " + jadxPath);
            Console.WriteLine(Strings.info + "APK Path : " + apkPath);
            Console.WriteLine(Strings.info + Strings.jadxRun);
            
            var tmpFolder = Decompile(jadxPath, apkPath);
            if (tmpFolder is null)
            {
                Console.WriteLine(Strings.jadxRunProblem);
                Environment.Exit(-1);
            }

            Console.WriteLine(Strings.info + Strings.jadxFinish);

            var tmpFiles = fileList(tmpFolder);
            if (tmpFiles is null)
            {
                Console.Write(Strings.fileListError);
                Environment.Exit(-1);
            }
            

            new Sniffer(tmpFiles,rulePath);

        }


    }
}
