using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PassiveAPKSniffer.Class
{
    class Core
    {
        /// <summary>
        /// Control file is exist or not. If file exist it return true otherwise return false
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
        /// Take command and run it OS level.
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
        /// Take jadxPath and apkPath data for preparing OS Command. [OS Command should be checked this step and give dynamic data for filename]
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
        /// take folder path information after Decompile process is finished and gather all files under folder path. [There is a hardcoded extension control maybe this control can be dynamic with reading external file]
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public IEnumerable<string> fileList(string folderPath)
        {
            try
            {
                var allfiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".java") || s.EndsWith(".xml") || s.EndsWith(".kt"));
                return allfiles;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// it writ 
        /// </summary>
        /// <returns></returns>
        public static async Task DownloadFile()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;

            var content = await GetUrlContent(Strings.jadxDownloadLink);
            if (content != null)
            {
                try
                {
                    await File.WriteAllBytesAsync(exePath + "/jadx.zip", content);
                    Console.WriteLine(Strings.info + Strings.successJadxDownload);
                    Environment.Exit(-1);
                }
                catch
                {
                    Console.WriteLine(Strings.info + Strings.errorJadxDownload);
                    Environment.Exit(-1);
                }
            }
            else
            {
                Console.WriteLine(Strings.info + Strings.errorJadxDownload);
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// it take url and try to download content
        /// </summary>
        /// <returns></returns>
        public static async Task<byte[]> GetUrlContent(string url)
        {
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(url))
                return result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;
        }

        /// <summary>
        /// This is like a main function. All function is called from here. After Compile and fileList function it calls Sniffer class to find secret
        /// </summary>
        /// <param name="jadxPath"></param>
        /// <param name="apkPath"></param>
        /// <param name="rulePath"></param>
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
