using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SindaSoft.DependencyWalker
{
    public partial class MainWindow : Form
    {
        private Walker w = null;
        public List<string> currentListOfFiles = null;
        public List<string> listOfTypes = null;

        /// <summary>
        /// Form constructor
        /// </summary>
        /// <param name="a"></param>
        public MainWindow(string[] a)
        {
            currentListOfFiles = new List<string>(a);
            InitializeComponent();
        }

        /// <summary>
        /// OnLoad event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
#if NETCOREAPP
            cbShowGac.Visible = false;  // No GAC in .NET Core
#endif
            tvReferencesTree.AllowDrop = true;
            tvReferencesTree.DragEnter += new DragEventHandler(tvReferencesTree_DragEnter);
            tvReferencesTree.DragDrop += new DragEventHandler(tvReferencesTree_DragDrop);




            if (currentListOfFiles.Count == 0)
            {
                /*
                MessageBox.Show("Usage:\n\n\tDepends.Net.exe file1 [file2 [file3 [.. etc..]]]\n\nFor example:\n\n\tDepends.Net.exe program.exe lib1.dll lib2.dll", 
                                this.Text, 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                Close();
                */
                return;
            }

            tssLoadedInfo.Text = 
            tssMatchCounter.Text = "";
            walkAndShowCollectedData(currentListOfFiles);
        }

        void tvReferencesTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        void tvReferencesTree_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            tssLoadedInfo.Text =
            tssMatchCounter.Text = "";
            walkAndShowCollectedData(files.ToList());
        }


        /// <summary>
        /// When someone click on treeview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvReferencesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = tvReferencesTree.SelectedNode;
            if (node != null)
            {
                if (w.refass.ContainsKey(node.Text))
                {
                    tbUsedBy.Text = node.Text;

                    if(String.IsNullOrEmpty(w.refass2dotnetversion[node.Text]))
                        tbUsedBy.Text +=  " ("+w.refass2dotnetversion[node.Text]+")";

                    tbUsedBy.Text += " IS USED BY:\r\n\r\n";
                    tbUsedBy.Text += w.refass[node.Text].Replace("\n", "\r\n");
                }
                else if (w.errors.ContainsKey(node.Text))
                {
                    tbUsedBy.Text = w.errors[node.Text].Replace("\n", "\r\n");
                }
                else
                    tbUsedBy.Text = String.Empty;

                propertyGrid1.SelectedObject = node.Tag;
                propertyGrid1.Enabled = false;
            }
        }

        /// <summary>
        /// When someone change "Show .NET" checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbShowGac_CheckedChanged(object sender, EventArgs e)
        {
            walkAndShowCollectedData(currentListOfFiles);
        }

        /// <summary>
        /// Run analysis and display data from it
        /// </summary>
        private void walkAndShowCollectedData(List<string> a)
        {
            Cursor.Current = Cursors.WaitCursor;

            this.Text = "DependencyWalker.Net - [" + String.Join(",", a.Select(x => Path.GetFileName(x)).ToArray()) + "]";

            if (w != null)
            {
                w.Dispose();
                w = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();  

            w = new Walker(a);
            w.parent = this;
            w.includeGAC = cbShowGac.Checked;
            w.runIt();
            
            tssLoadedInfo.Text =   (w.refass.Count  + w.listOfFilenames2Check.Count) +  " modules/" + w.type2ass.Count + " types";
            
            this.listOfTypes = w.type2ass.Keys.ToList();
            this.listOfTypes.Sort();
            this.searchTypeTxt_TextChanged(this, null);

            if (w.refass.Count > 0)
            {
                if (!tcMain.TabPages.Contains(tpListOfReferences))
                    tcMain.TabPages.Add(tpListOfReferences);

                tbUsedBy.Text = "";
                this.tbListOfReferences.Text = "";
                foreach (string s in w.refass.Keys)
                    this.tbListOfReferences.Text += s + "\r\n";

                this.tbListOfReferences.Text += "\r\n\r\n\r\n";
                foreach (string s in w.refass2filename.Keys)
                {
                    this.tbListOfReferences.Text += (new Uri(w.refass2filename[s])).AbsolutePath
                                                    + "\r\n";
                }
            }
            else
                tcMain.TabPages.Remove(tpListOfReferences);

            if (w.errors.Count > 0)
            {
                if (!tcMain.TabPages.Contains(tpErrors))
                    tcMain.TabPages.Add(tpErrors);

                tbErrors.Text = "";
                foreach (string s in w.errors.Values)
                    tbErrors.Text += "\r\n\r\n" + s;
            }
            else
                tcMain.TabPages.Remove(tpErrors);

            Cursor.Current = Cursors.Default;

        }

        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Title = "Please select one or more .NET assemblies to investigate dependencies";
            ofd.Filter = ".NET assemblies (*.exe;*.dll)|*.exe;*.dll|" +
                         "All files (*.*)|*.*";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentListOfFiles = new List<string>(ofd.FileNames);
                walkAndShowCollectedData(currentListOfFiles);
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            if (currentListOfFiles.Count > 0)
                walkAndShowCollectedData(currentListOfFiles);
        }

        /// <summary>
        /// Search TreeView for give text & perform the action
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="treeView"></param>
        /// <param name="action"></param>
        void SearchAndDo(string searchText,TreeView treeView,Action<TreeView,TreeNode> action)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                if (node.Text.CaseInsensitiveContains(searchText))
                {
                    action(treeView,node);
                }
                SearchAndDo(searchText, treeView,node, action);
            }

        }/// <summary>
        /// Search the subnode under given tree node recursively & perform the action
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="treeView"></param>
        /// <param name="treeNode"></param>
        /// <param name="action"></param>
        void SearchAndDo(string searchText, TreeView treeView, TreeNode treeNode, Action<TreeView,TreeNode> action)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                if (node.Text.CaseInsensitiveContains(searchText))
                {
                    action(treeView,node);
                }
                SearchAndDo(searchText, treeView, node,action);

            }
        }

        private TreeNode firstNode;
        private bool isColored = default(bool);
        private string prviousSearchString = string.Empty;

        private void searchTxt_TextChanged(object sender, EventArgs e)
        {
            int matchCounter = 0;
            tssMatchCounter.Text = "";
            if (tvReferencesTree.Nodes.Count == 0)
            {
                searchTxt.Focus();
                return;
            }

            firstNode = default(TreeNode);
            var searchString = searchTxt.Text;
            
            // ClearColor
            if ( isColored )
            {
                SearchAndDo(prviousSearchString, tvReferencesTree, (treeView,node) =>
                 {
                     node.BackColor = Color.Empty;
                     node.ForeColor = Color.Empty;
                 });
            }
            
            SearchAndDo(searchString, tvReferencesTree, (treeView, node) =>
            {
                matchCounter++;
                node.BackColor = Color.Yellow; // SystemColors.Highlight;
                node.ForeColor = Color.Black; // SystemColors.HighlightText; ;
                if ( firstNode == default(TreeNode))
                {
                    firstNode = node;
                }
            });


            if (firstNode == default(TreeNode))
            {
                isColored = false;
            }
            else
            {
                tvReferencesTree.ExpandAll();
                tvReferencesTree.SelectedNode = firstNode;
                tvReferencesTree.Focus();
                isColored = true;
            }

            prviousSearchString = searchString;
            tssMatchCounter.Text = !String.IsNullOrEmpty(searchString) ? String.Format("{0} matches found", matchCounter) 
                                                                       : String.Empty;
            searchTxt.Focus();
        }

        private void searchTypeTxt_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(searchTypeTxt.Text))
                lbTypes.DataSource = listOfTypes;
            else
                lbTypes.DataSource = listOfTypes.Where( x=> x.ToLower().Contains(searchTypeTxt.Text.ToLower())).ToList();
        }

        private void lbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTypes.SelectedItem == null)
                lblAssemblyWithType.Text = "";
            else
                lblAssemblyWithType.Text = w.type2ass[lbTypes.SelectedItem as string];
        }
    }

    public static class Extensions
    {
        public static bool CaseInsensitiveContains(this string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            if (String.IsNullOrEmpty(value))
                return false;
            else
                return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}
