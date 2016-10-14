using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SindaSoft.DependencyWalker
{
    public partial class MainWindow : Form
    {
        public List<string> args;
        Walker w = null;

        /// <summary>
        /// Form constructor
        /// </summary>
        /// <param name="a"></param>
        public MainWindow(string[] a)
        {
            args = new List<string>(a);
            InitializeComponent();
        }

        /// <summary>
        /// OnLoad event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (args.Count == 0)
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

            walkAndShowCollectedData(args);
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
            walkAndShowCollectedData(args);  
        }

        /// <summary>
        /// Run analysis and display data from it
        /// </summary>
        private void walkAndShowCollectedData(List<string> a)
        {
            Cursor.Current = Cursors.WaitCursor;

            this.Text += " [" + String.Join(",", a.ToArray()) + "]";


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
            ofd.Filter = "Images (*.exe;*.dll)|*.exe;*.dll|" +
                         "All files (*.*)|*.*";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                args = new List<string>(ofd.FileNames);
                walkAndShowCollectedData(args);
            }
        }
    }
}
