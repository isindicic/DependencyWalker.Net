using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace SindaSoft.DependencyWalker
{
    public class Worker : IDisposable
    {
        public bool json = false;
        public bool gac = false;

        public Worker()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);
        }

        #region IDisposable Members

        public void Dispose()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve -= new ResolveEventHandler(currentDomain_AssemblyResolve);
        }

        #endregion

        public string currentFilenameWeCheck = null;

        Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            string assemblyPath;
            if (currentFilenameWeCheck != null)
                assemblyPath = Path.Combine(Path.GetDirectoryName(currentFilenameWeCheck), new AssemblyName(e.Name).Name + ".dll");
            else
                assemblyPath = new AssemblyName(e.Name).Name + ".dll";

            if (!File.Exists(assemblyPath))
                return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        public void Start(string[] args)
        {
            if (args.Length == 1)
            {
                string path = args[0];

                if (File.Exists(path)) // Is it a file ??!
                    this.StartWithFile(path);
                else if (Directory.Exists(path)) // Is it a directory ??!
                    this.StartWithDirectory(path);
            }
            else if (args.Length == 0)
            {
                string path = Directory.GetCurrentDirectory();
                this.StartWithDirectory(path);
            }
        }

        public void StartWithDirectory(string path)
        {
            List<string> files = Directory.GetFiles(path, "*.dll").ToList();
            files.AddRange(Directory.GetFiles(path, "*.exe").ToList());
            files.Sort();

            if (this.json)
            {
                List<AssemblyInfo> ais = new List<AssemblyInfo>();
                foreach (string fn in files)
                {
                    FileInfo fi = new FileInfo(fn);
                    currentFilenameWeCheck = fi.FullName;

                    ais.Add(inspectAssembly(fn));
                }

                Console.WriteLine(QuickJsonSerializer.Serialize(ais));
            }
            else
            {
                foreach (string fn in files)
                {
                    FileInfo fi = new FileInfo(fn);
                    currentFilenameWeCheck = fi.FullName;

                    AssemblyInfo ai = inspectAssembly(fn);
                    string line = ai.ToString();
                    Console.WriteLine(line);
                }
            }
        }

        public void StartWithFile(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            currentFilenameWeCheck = fi.FullName;

            AssemblyInfo ai = inspectAssembly(filename);

            if (this.json)
                Console.WriteLine( QuickJsonSerializer.Serialize(ai) );
            else
                Console.WriteLine(ai.ToString());

        }


        private Dictionary<string, AssemblyInfo> alreadyProcessed;
        /// <summary>
        /// Load assembly specified with filename and 
        /// walk through its dependencies
        /// </summary>
        /// <param name="name"></param>
        private AssemblyInfo inspectAssembly(string aname)
        {
            string name = aname;
            AssemblyInfo retval = new AssemblyInfo();
            alreadyProcessed = new Dictionary<string, AssemblyInfo>();
            try
            {
                Assembly a = Assembly.LoadFrom(aname);
                name = a.GetName().Name;
                retval.FillData(a);

                AssemblyName[] anames = a.GetReferencedAssemblies();
                foreach (AssemblyName an in anames)
                {
                    if (!shouldWeIncludeIt(an))
                        continue;

                    AssemblyInfo ai = new AssemblyInfo
                    {
                        Name = an.Name
                    };
                    retval.References.Add(ai);

                    if (!alreadyProcessed.ContainsKey(an.Name))
                    {
                        alreadyProcessed[an.Name] = ai;
                        inspectAssembly(ai, an);
                    }
                    else
                    {
                        ai.CopyData(alreadyProcessed[an.Name]);
                    }
                }
                return retval;
            }
            catch (Exception ex)
            {
                AssemblyInfo ai = new AssemblyInfo
                {
                    Name = name,
                    Error = ex.Message
                };
                return ai;
            }
        }

        /// <summary>
        /// Load assembly specified with assembly name and 
        /// walk through its dependencies
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="anr"></param>
        private void inspectAssembly(AssemblyInfo tn, AssemblyName anr)
        {
            string name = "";
            try
            {
                name = anr.Name;
                Assembly a = Assembly.Load(anr.FullName);
                tn.FillData(a);

                AssemblyName[] anames = a.GetReferencedAssemblies();
                foreach (AssemblyName an in anames)
                {
                    if (!shouldWeIncludeIt(an))
                        continue;

                    AssemblyInfo tn2 = new AssemblyInfo
                    {
                        Name = an.Name
                    };

                    if (!alreadyProcessed.ContainsKey(an.Name))
                    {
                        alreadyProcessed[an.Name] = tn2;
                        inspectAssembly(tn2, an);       // Go deeply ....
                    }
                    else
                    {
                        tn2.CopyData(alreadyProcessed[an.Name]);  // Get what we already know...
                    }
                }
            }
            catch (Exception /*ex*/)
            {
                //AssemblyInfo ai = new AssemblyInfo
                //{
                //    Name = name,
                //    Error = ex.Message
                //};
            }
        }

        private bool shouldWeIncludeIt(AssemblyName an)
        {
            if (this.gac)
                return true;
            else
                return !isItInGlobalAssemblyCache(an);
        }

        private bool isItInGlobalAssemblyCache(AssemblyName an)
        {
#if NETCOREAPP
            return true;    // No GAC in .NET Core
#else
            try
            {
                Assembly a = Assembly.Load(an.FullName);

                System.Diagnostics.Debug.WriteLine(an.Name + " ---> " + (a.GlobalAssemblyCache ? "GAC" : ""));
                return a.GlobalAssemblyCache;
            }
            catch
            {
                return false;    // Error ?! .. pass it .. 
            }
#endif
        }
    }

    public class AssemblyInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }

        public string Version { get; set; }
        public string Architecture { get; set; }
        public string DotNetVersion { get; set; }

        public bool IsGAC { get; set; }

        public string FileUri { get; set; }
        public List<AssemblyInfo> References { get; set; }

        public string Error { get; set; }

        public AssemblyInfo()
        {
            References = new List<AssemblyInfo>();
        }

        public AssemblyInfo(Assembly a) : this()
        {
            this.FillData(a);
        }

        public void FillData(Assembly a)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(a.Location);
            this.Version = fvi.FileVersion;
            this.DotNetVersion = ".NET CLR " + a.ImageRuntimeVersion;
            this.Architecture = a.GetName().ProcessorArchitecture.ToString();
            this.Name = a.GetName().Name;
            this.FullName = a.GetName().FullName;

            this.FileUri = a.CodeBase; // Save assembly file location... 
            this.IsGAC = a.GlobalAssemblyCache; // Is it GAC ? 

            try
            {
                // This work only for .NET > 4 .. Try it...
                var attribute = a.GetCustomAttributes(true)
                                 .OfType<System.Runtime.Versioning.TargetFrameworkAttribute>()
                                 .First();
                this.DotNetVersion = attribute.FrameworkDisplayName.Trim();
            }
            catch
            {
            }
        }

        public void CopyData(AssemblyInfo src)
        {
            this.Name = src.Name;
            this.FullName = src.FullName;
            this.Architecture = src.Architecture;
            this.DotNetVersion = src.DotNetVersion;
            this.Error = src.Error;
            this.FileUri = src.FileUri;
            this.IsGAC = src.IsGAC;
            this.Version = src.Version;
        }


        public string ToString(int ident)
        {
            string identS = new string(' ', ident * 25);

            string retval;
            if (String.IsNullOrEmpty(this.Error))
            {
                retval = identS + String.Format("{0, -10}\n", this.Name);
                retval += identS + String.Format("{0, -10}    Full Name: {1,-10}\n", "", this.FullName);
                retval += identS + String.Format("{0, -10} File Version: {1,-10}\n", "", this.Version);
                retval += identS + String.Format("{0, -10} Architecture: {1,-10}\n", "", this.Architecture);
                retval += identS + String.Format("{0, -10}    Built for: {1,-10}\n", "", this.DotNetVersion);
                retval += identS + String.Format("{0, -10}   References: [ ", "");
                List<string> refs = new List<string>();
                foreach (AssemblyInfo ai in this.References)
                    refs.Add(ai.Name);
                retval += String.Join(",", refs.ToArray());
                retval += String.Format(" ]\n");

                foreach (AssemblyInfo ai in this.References)
                    retval += ai.ToString(ident + 1);
            }
            else
                retval = identS + String.Format("{0, -10}\n{1, -10}        Error: {2}\n", this.Name, "", this.Error);
            return retval;
        }

        public override string ToString()
        {
            return this.ToString(0);
        }
    }
}
