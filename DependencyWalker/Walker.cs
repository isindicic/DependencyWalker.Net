using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace SindaSoft.DependencyWalker
{
    public class Walker : IDisposable
    {
        public MainWindow parent;
        public bool includeGAC = true;
        public List<string> listOfFilenames2Check;
        public Dictionary<string, string> refass;
        public Dictionary<string, TreeNode> ref2node;
        public Dictionary<string, string> refass2filename;
        public Dictionary<string, bool> refass2isGAC;
        public Dictionary<string, string> errors;

        public Dictionary<string, string> type2ass;

        public string file2inspect = null;

        public Walker(List<string> args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

            listOfFilenames2Check = args;
        }

        Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            string assemblyPath;
            if (file2inspect != null)
                assemblyPath = Path.Combine(Path.GetDirectoryName(file2inspect), new AssemblyName(e.Name).Name + ".dll");
            else
                assemblyPath = new AssemblyName(e.Name).Name + ".dll";

            if (!File.Exists(assemblyPath))
                return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        public void Dispose()
        {
            try
            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.AssemblyResolve -= new ResolveEventHandler(currentDomain_AssemblyResolve);
            }
            catch
            { 
            }
        }


        /// <summary>
        /// Start dependacy walking !!!!
        /// </summary>
        public void runIt()
        {
            // Recreate data
            refass = new Dictionary<string, string>();
            ref2node = new Dictionary<string, TreeNode>();
            refass2filename = new Dictionary<string, string>();
            refass2isGAC = new Dictionary<string, bool>();
            type2ass = new Dictionary<string, string>();
            errors = new Dictionary<string, string>();
            
            // Clear tree control
            parent.tvReferencesTree.Nodes.Clear();

            // Get real filelist
            List<string> lof = expandListOfFiles();

            foreach (String n in lof)   // For each specified file .. 
            {
                FileInfo fi = new FileInfo(n);
                file2inspect = fi.FullName;

                inspectAssembly(n);     // .. check it
            }
        }

        /// <summary>
        /// Load assembly specified with filename and 
        /// walk through its dependencies
        /// </summary>
        /// <param name="name"></param>
        private void inspectAssembly(string name)
        {
            try
            {
                Assembly a = Assembly.LoadFrom(name);
                TreeNode tn = parent.tvReferencesTree.Nodes.Add(a.GetName().Name);
                AssemblyName[] anames = a.GetReferencedAssemblies();
                tn.Tag = a;
                foreach (AssemblyName an in anames)
                {
                    if (!shouldWeIncludeIt(an))
                        continue;

                    TreeNode tn2 = tn.Nodes.Add(an.Name);
                    tn2.Tag = an;

                    if (!refass.ContainsKey(an.Name))
                    {
                        refass[an.Name] = name;
                        inspectAssembly(tn2, an);

                        ref2node[an.Name] = tn2;
                    }
                    else
                    {
                        refass[an.Name] += "\n" + name;
                        if (!isItInGlobalAssemblyCache(an))
                            CloneNodes(ref2node[an.Name], tn2);
                    }
                }

                foreach(Type t in a.GetTypes())
                    type2ass[getCSharpFromType(t)] = name;
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                TreeNode tn = parent.tvReferencesTree.Nodes.Add(name);
                tn.Text += " - Error loading";
                tn.ForeColor = Color.Red;
                errors[tn.Text] = String.Join("--->", treeNode2refList(tn).ToArray()) + "\r\n" + ex.ToString();

                errors[tn.Text] += "\r\n--- Loader exceptions -----\r\n";
                foreach (Exception eee in ex.LoaderExceptions)
                    errors[tn.Text] += "\t" + eee.ToString() + "\r\n";

                expandTree2Node(tn);
            }
            catch (Exception ex)
            {
                TreeNode tn = parent.tvReferencesTree.Nodes.Add(name);
                tn.Text += " - Error loading";
                tn.ForeColor = Color.Red;
                errors[tn.Text] = String.Join("--->", treeNode2refList(tn).ToArray()) + "\r\n" + ex.ToString();
                expandTree2Node(tn);
            }
        }

        /// <summary>
        /// Load assembly specified with assembly name and 
        /// walk through its dependencies
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="anr"></param>
        private void inspectAssembly(TreeNode tn, AssemblyName anr)
        {
            try
            {
                Assembly a = Assembly.Load(anr.FullName);

                refass2filename[anr.Name] = a.CodeBase; // Save assembly file location... 
                refass2isGAC[anr.Name] = a.GlobalAssemblyCache; // Is it GAC ? 

                AssemblyName[] anames = a.GetReferencedAssemblies();
                foreach (AssemblyName an in anames)
                {
                    if (!shouldWeIncludeIt(an))
                        continue;

                    TreeNode tn2 = tn.Nodes.Add(an.Name);
                    tn2.Tag = an;

                    if (!refass.ContainsKey(an.Name))
                    {
                        refass[an.Name] = anr.Name;
                        inspectAssembly(tn2, an);       // Go deeply ....

                        ref2node[an.Name] = tn2;
                    }
                    else
                    {
                        refass[an.Name] += "\n" + anr.Name;
                        if (!isItInGlobalAssemblyCache(an))
                            CloneNodes(ref2node[an.Name], tn2);
                    }
                }

                foreach (Type t in a.GetTypes())
                    type2ass[getCSharpFromType(t)] = anr.Name;

            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                tn.Text += " - Error loading";
                tn.ForeColor = Color.Red;

                errors[tn.Text] = String.Join("--->", treeNode2refList(tn).ToArray()) + "\r\n" + ex.ToString();

                errors[tn.Text] += "\r\n--- Loader exceptions -----\r\n";
                foreach(Exception eee in ex.LoaderExceptions)
                    errors[tn.Text] += "\t" + eee.ToString() + "\r\n";

                expandTree2Node(tn);
            }
            catch (Exception ex)
            {
                tn.Text += " - Error loading";
                tn.ForeColor = Color.Red;

                errors[tn.Text] = String.Join("--->", treeNode2refList(tn).ToArray()) + "\r\n" + ex.ToString();
                expandTree2Node(tn);
            }
        }


        private string getCSharpFromType(Type t)
        {
            string name = t.FullName;
            if (t.IsGenericType)
            {
                if (t.IsNested && t.DeclaringType.IsGenericType)
                    return name; // This is invalid !!! But let's use short name ... 
                string className = name.Substring(0, t.Name.IndexOf('`'));
                List<string> args = new List<string>();
                foreach (Type arg in t.GetGenericArguments())
                    args.Add( getCSharpFromType(arg) );
                return className + "<" + String.Join(",", args.ToArray()) + ">";
            }
            else
                return name;
        }


        /// <summary>
        /// Check if this assembly should be included in report.
        /// In short this function check if we need to include GAC based 
        /// assemblies
        /// </summary>
        /// <param name="an"></param>
        /// <returns></returns>
        private bool shouldWeIncludeIt(AssemblyName an)
        {
            if (this.includeGAC)
                return true;
            else
                return !isItInGlobalAssemblyCache(an);
        }


        private bool isItInGlobalAssemblyCache(AssemblyName an)
        {
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
        }

        private void CloneNodes(TreeNode src, TreeNode dest)
        {
            foreach (TreeNode tn in src.Nodes)
                dest.Nodes.Add(tn.Clone() as TreeNode);
        }

        private List<string> treeNode2refList(TreeNode tn)
        {
            List<string> retval = new List<string>();
            retval.Add( tn.Text );
            TreeNode t = tn.Parent;

            while (t != null)
            {
                retval.Insert(0,  t.Text);
                t = t.Parent;
            }
            return retval;
        }

        /// <summary>
        /// Expand tree up to desired node
        /// </summary>
        /// <param name="tn"></param>
        private void expandTree2Node(TreeNode tn)
        {
            TreeNode t = tn.Parent;

            while (t != null)
            {
                t.Expand();
                t = t.Parent;
            }
        }

        /// <summary>
        /// Check if there are some jocker signs in 
        /// specified filelist and replace that 
        /// entry with real filenames
        /// </summary>
        /// <returns></returns>
        private List<string> expandListOfFiles()
        {
            List<string> retval = new List<string>();
            foreach (string s in listOfFilenames2Check)
            {
                if (s.Contains("*") || s.Contains("?"))
                {
                    string path = Path.GetDirectoryName(s);
                    string fn = Path.GetFileName(s);

                    if (String.IsNullOrEmpty(path))
                        path = Directory.GetCurrentDirectory();

                    string[] ss = Directory.GetFiles(path, fn);
                    foreach (string f in ss)
                        retval.Add(f);
                }
                else
                    retval.Add(s);
            }
            return retval;
        }

    }
}

