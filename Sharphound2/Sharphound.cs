﻿using CommandLine;
using Sharphound2.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CommandLine.Text;

namespace Sharphound2
{
    class Sharphound
    {
        public class Options
        {
            [Option('d', HelpText = "Domain to enumerate", DefaultValue = null)]
            public string Domain { get; set; }

            [Option('s',HelpText ="Search the entire forest", DefaultValue = false)]
            public bool SearchForest { get; set; }

            [Option('t',HelpText ="Number of Threads to use", DefaultValue =20)]
            public int Threads { get; set; }

            [Option('I',HelpText ="Interval to display progress in milliseconds", DefaultValue =30000)]
            public int Interval { get; set; }

            [Option('S',HelpText ="Skip ping checks for hosts", DefaultValue =false)]
            public bool SkipPing { get; set; }

            [Option('c',"CollectionMethod", HelpText ="Collection Method", DefaultValue = CollectionMethod.Default)]
            public CollectionMethod CollectMethod { get; set; }

            [Option('P',HelpText ="Timeout in milliseconds for ping timeout", DefaultValue =750)]
            public int PingTimeout { get; set; }

            [Option('G', HelpText= "Skip Global Catalog Deconfliction", DefaultValue = false)]
            public bool SkipGCDeconfliction { get; set; }

            [Option('C',HelpText = "Filename for the data cache", DefaultValue = "BloodHound.bin")]
            public string CacheFile { get; set; }

            public string CurrentUser { get; set; }

        }

        static void Main(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            
            var options = new Options();

            if (!Parser.Default.ParseArguments(args, options)) return;
            options.CurrentUser = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            Cache.CreateInstance(options);
            Utils.CreateInstance(options);
            GroupMemberEnumeration x = new GroupMemberEnumeration(options);
            x.StartEnumeration();
            LocalAdminEnumeration y = new LocalAdminEnumeration(options);
            y.StartEnumeration();
            Cache.Instance.SaveCache();
        }

        public static void InvokeBloodHound(string[] args)
        {
            Main(args);
        }
    }
}
