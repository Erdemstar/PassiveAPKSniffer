using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PassiveAPKSniffer.Class
{
    class Sniffer
    {
        /// <summary>
        /// it basicly read Rules file.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public List<Rules> readRules(string Path)
        {
            using (StreamReader stream = new StreamReader(Path))
            {
                string data = stream.ReadToEnd();
                var json = JsonSerializer.Deserialize<List<Rules>>(data);
                return json;
            }
        }
        /// <summary>
        /// it read all file and every fileline is went for anaylze calling controlLine
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rules"></param>
        public List<Output> readFiles(IEnumerable<string> filePath, List<Rules> rules)
        {
            List<Output> list = new List<Output>();
            try
            {
                foreach (var file in filePath)
                {
                    using (StreamReader ReaderObject = new StreamReader(file))
                    {
                        int i = 1;
                        string Line;
                        while ((Line = ReaderObject.ReadLine()) != null)
                        {
                            var data = controlLine(Line, rules);
                            if (data is not null)
                            {
                                list.Add(new Output()
                                {
                                    filename = file,
                                    linenumber = i.ToString(),
                                    rulename = data.Item1,
                                    payload = data.Item2
                                });
                            }
                            
                            i++;
                        }
                    }
                }
            }
            catch
            {

            }


            return list;
        }

        /// <summary>
        /// it basicly take line and rules then control there is a secret or not for using regex.
        /// </summary>
        /// <param name="codeline"></param>
        /// <param name="rules"></param>
        public Tuple<string,string> controlLine(string codeline, List<Rules> rules)
        {
            //regex loop
            foreach (var rule in rules)
            {
                Regex regex = new Regex(rule.value, RegexOptions.IgnoreCase);
                Match match = regex.Match(codeline);

                if (match.Success && match.Groups.Count > 0)
                {
                    // Value may be more than maxResponseSize and it false positive. So i write a little controll
                    if (codeline.Length < 300)
                    {
                        return Tuple.Create(rule.key, codeline);
                    }

                }

            }
            return null;
        }

        public void writeOutput(List<Output> output)
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            string json = JsonSerializer.Serialize(output);
            File.WriteAllText(exePath + "/PassiveAPKSniffer-output.json", json);
        }

        /// <summary>
        /// this is like a main function.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rulePath"></param>
        public Sniffer(IEnumerable<string> filePath, string rulePath)
        {
            var rules = readRules(rulePath);
            if (rules is null)
            {
                Console.WriteLine(Strings.rulesReadFilesProblem);
                Environment.Exit(-1);
            }

            var output = readFiles(filePath, rules);
            if (output is not null)
            {
                writeOutput(output);
            }

            Console.WriteLine(Strings.info + Strings.finish);

        }


    }
}
