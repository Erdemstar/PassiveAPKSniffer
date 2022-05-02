using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PassiveAPKSniffer.Class
{
    class Sniffer
    {
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rules"></param>
        public void readFiles(IEnumerable<string> filePath, List<Rules> rules)
        {
            try
            {
                foreach (var file in filePath)
                {
                    using (StreamReader ReaderObject = new StreamReader(file))
                    {
                        string Line;
                        while ((Line = ReaderObject.ReadLine()) != null)
                        {
                            controlLine(Line, rules);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeline"></param>
        /// <param name="rules"></param>
        public void controlLine(string codeline, List<Rules> rules)
        {
            //regex loop
            foreach (var regex in rules)
            {
                //find regex in file loop
                foreach (Match match in Regex.Matches(codeline, regex.value, RegexOptions.IgnoreCase))
                {
                    if (match.Success && match.Groups.Count > 0)
                    {
                        // Value may be more than maxResponseSize and it false positive. So i write a little controll
                        if (codeline.Length < 300) {

                            Console.WriteLine("Rule :" + regex.key + " " + "Data : " + codeline);

                        }

                    }
                }

            }
        }

        /// <summary>
        /// 
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

            readFiles(filePath,rules);

        }


    }
}
