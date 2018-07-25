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

            walkAndShowCollectedData(currentListOfFiles);
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
                    tbUsedBy.Text = node.Text + " IS USED BY:\r\n\r\n";
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
                w.Dispose();

            w = new Walker(a);
            w.parent = this;
            w.includeGAC = cbShowGac.Checked;
            w.runIt();

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
            ofd.Title = "Please select .NET assembly to investigate dependencies";
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

        private void searchTxt_TextChanged(object sender, EventArgs e)
        {

        }

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

        }
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

        private void searchBtn_Click(object sender, EventArgs e)
        {
            if (tvReferencesTree.Nodes.Count == 0)
            {
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
                

                node.BackColor = SystemColors.Highlight; ;
                node.ForeColor = SystemColors.HighlightText; ;
                treeView.SelectedNode = node;
                treeView.Focus();
                if ( firstNode == default(TreeNode))
                {
                    firstNode = node;
                }
            });


            if (firstNode == default(TreeNode))
            {
                isColored = false;
            }else
            {
                tvReferencesTree.SelectedNode = firstNode;
                tvReferencesTree.Focus();
                isColored = true;

            }

            prviousSearchString = searchString;


        }
    }
    public static class Extensions
    {
        public static bool CaseInsensitiveContains(this string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}
