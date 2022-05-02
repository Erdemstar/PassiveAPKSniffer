﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassiveAPKSniffer.Class
{
    class Option
    {
        [Option("jadx_path", Required = false, HelpText = "--jadx_path C:\\Users\\user\\jadx-1.3.5\\bin")]
        public string jadx_path { get; set; }

        [Option("download_jadx", Required = false, HelpText = "--download_jadx yes")]
        public string download_jadx { get; set; }
        
        [Option("apk_path", Required = false, HelpText = "--apk_path C:\\Users\\user\\vuln.apk")]
        public string apk_path { get; set; }

        [Option("rule_path", Required = false, HelpText = "--rule_path C:\\Users\\user\\PassiveAPKSniffer\\Class\\Rules.json")]
        public string rule_path { get; set; }
    }
} 