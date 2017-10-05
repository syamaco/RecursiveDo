using System;
using System.Collections.Generic;
using System.IO;

namespace RecursiveDo
{
    class Program
    {
        Dictionary<string, string> dic;
        Program()
        {
            dic = new Dictionary<string, string>();

            using (var file = new StreamReader(@"dict.txt"))
            {
                string line = "";
                while ((line = file.ReadLine()) != null)
                {
                    var kv = line.Replace('/', Path.DirectorySeparatorChar).Split('\t');
                    if (kv.Length == 2)
                    {
                        this.dic.Add(kv[0], kv[1]);
                    }
                    else
                    {
                        Console.WriteLine("SkipLine : '" + line + "'");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                new Program().DirSearch(args[0], args.Length > 1 && args[1] == "-v");
            }
            else
            {
                Console.WriteLine("needs root dir...");
            }
        }
        void DirSearch(string dir, bool isVarbose = false)
        {
            try
            {
                foreach (var d in Directory.GetDirectories(dir))
                {
                    foreach (var f in Directory.GetFiles(d, "*.*"))
                    {
                        if (this.dic.ContainsKey(f))
                        {
                            log(f + " -> " + this.dic[f], isVarbose);
                            try
                            {
                                File.Move(f, this.dic[f]);
                            }
                            catch (Exception e)
                            {
                                log(e.Message, isVarbose);
                            }
                        }
                    }
                    DirSearch(d, isVarbose);
                }
            }
            catch (Exception e)
            {
                log(e.Message, isVarbose);
            }
        }

        void log(string message, bool withConsole = false)
        {
            if (withConsole) Console.WriteLine(message);
        }
    }

}
