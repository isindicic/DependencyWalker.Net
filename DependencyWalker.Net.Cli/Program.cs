using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SindaSoft.DependencyWalker
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> a = new List<string>(args);

            if (a.FirstOrDefault(x => x.StartsWith("--help")) != null)
            {
                Console.WriteLine("Usage:\n");
                Console.WriteLine("\nDependencyWalker.Cli directoryorfile2explore [--json]  [--gac] \n");
                Console.WriteLine("Where is\n");
                Console.WriteLine("\t--json   - Write data in JSON format");
                Console.WriteLine("\t--gac   - Include GAC");
                return;
            }

            using (Worker w = new Worker())
            {
                if (a.FirstOrDefault(x => x.StartsWith("--json")) != null)
                    w.json = true;
                if (a.FirstOrDefault(x => x.StartsWith("--gac")) != null)
                    w.gac = true;

                a.Remove("--json");
                a.Remove("--gac");
                w.Start(a.ToArray());
            }

        }
    }
}
