namespace SindaSoft.DependencyWalker
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tvReferencesTree = new System.Windows.Forms.TreeView();
            this.tbUsedBy = new System.Windows.Forms.TextBox();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpReferenceTree = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tpListOfReferences = new System.Windows.Forms.TabPage();
            this.tbListOfReferences = new System.Windows.Forms.TextBox();
            this.tpTypes = new System.Windows.Forms.TabPage();
            this.lblAssemblyWithType = new System.Windows.Forms.Label();
            this.searchTypeTxt = new System.Windows.Forms.TextBox();
            this.lbTypes = new System.Windows.Forms.ListBox();
            this.tpErrors = new System.Windows.Forms.TabPage();
            this.tbErrors = new System.Windows.Forms.TextBox();
            this.cbShowGac = new System.Windows.Forms.CheckBox();
            this.btnSelectFiles = new System.Windows.Forms.Button();
            this.btnRescan = new System.Windows.Forms.Button();
            this.searchTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssMatchCounter = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLoadedInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tcMain.SuspendLayout();
            this.tpReferenceTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tpListOfReferences.SuspendLayout();
            this.tpTypes.SuspendLayout();
            this.tpErrors.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvReferencesTree
            // 
            this.tvReferencesTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvReferencesTree.Location = new System.Drawing.Point(0, 0);
            this.tvReferencesTree.Margin = new System.Windows.Forms.Padding(4);
            this.tvReferencesTree.Name = "tvReferencesTree";
            this.tvReferencesTree.Size = new System.Drawing.Size(331, 527);
            this.tvReferencesTree.TabIndex = 0;
            this.tvReferencesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvReferencesTree_AfterSelect);
            // 
            // tbUsedBy
            // 
            this.tbUsedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbUsedBy.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbUsedBy.Location = new System.Drawing.Point(0, 0);
            this.tbUsedBy.Margin = new System.Windows.Forms.Padding(4);
            this.tbUsedBy.Multiline = true;
            this.tbUsedBy.Name = "tbUsedBy";
            this.tbUsedBy.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbUsedBy.Size = new System.Drawing.Size(660, 293);
            this.tbUsedBy.TabIndex = 1;
            this.tbUsedBy.WordWrap = false;
            // 
            // tcMain
            // 
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tpReferenceTree);
            this.tcMain.Controls.Add(this.tpListOfReferences);
            this.tcMain.Controls.Add(this.tpTypes);
            this.tcMain.Controls.Add(this.tpErrors);
            this.tcMain.Location = new System.Drawing.Point(0, 87);
            this.tcMain.Margin = new System.Windows.Forms.Padding(4);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1012, 568);
            this.tcMain.TabIndex = 2;
            // 
            // tpReferenceTree
            // 
            this.tpReferenceTree.Controls.Add(this.splitContainer1);
            this.tpReferenceTree.Location = new System.Drawing.Point(4, 29);
            this.tpReferenceTree.Margin = new System.Windows.Forms.Padding(4);
            this.tpReferenceTree.Name = "tpReferenceTree";
            this.tpReferenceTree.Padding = new System.Windows.Forms.Padding(4);
            this.tpReferenceTree.Size = new System.Drawing.Size(1004, 535);
            this.tpReferenceTree.TabIndex = 0;
            this.tpReferenceTree.Text = "References Tree";
            this.tpReferenceTree.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvReferencesTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(996, 527);
            this.splitContainer1.SplitterDistance = 331;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tbUsedBy);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(660, 527);
            this.splitContainer2.SplitterDistance = 293;
            this.splitContainer2.TabIndex = 3;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(660, 230);
            this.propertyGrid1.TabIndex = 2;
            // 
            // tpListOfReferences
            // 
            this.tpListOfReferences.Controls.Add(this.tbListOfReferences);
            this.tpListOfReferences.Location = new System.Drawing.Point(4, 29);
            this.tpListOfReferences.Margin = new System.Windows.Forms.Padding(4);
            this.tpListOfReferences.Name = "tpListOfReferences";
            this.tpListOfReferences.Padding = new System.Windows.Forms.Padding(4);
            this.tpListOfReferences.Size = new System.Drawing.Size(1004, 535);
            this.tpListOfReferences.TabIndex = 1;
            this.tpListOfReferences.Text = "List of references";
            this.tpListOfReferences.UseVisualStyleBackColor = true;
            // 
            // tbListOfReferences
            // 
            this.tbListOfReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbListOfReferences.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbListOfReferences.Location = new System.Drawing.Point(4, 4);
            this.tbListOfReferences.Margin = new System.Windows.Forms.Padding(4);
            this.tbListOfReferences.Multiline = true;
            this.tbListOfReferences.Name = "tbListOfReferences";
            this.tbListOfReferences.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbListOfReferences.Size = new System.Drawing.Size(996, 527);
            this.tbListOfReferences.TabIndex = 2;
            this.tbListOfReferences.WordWrap = false;
            // 
            // tpTypes
            // 
            this.tpTypes.Controls.Add(this.lblAssemblyWithType);
            this.tpTypes.Controls.Add(this.searchTypeTxt);
            this.tpTypes.Controls.Add(this.lbTypes);
            this.tpTypes.Location = new System.Drawing.Point(4, 29);
            this.tpTypes.Name = "tpTypes";
            this.tpTypes.Size = new System.Drawing.Size(1004, 535);
            this.tpTypes.TabIndex = 3;
            this.tpTypes.Text = "List of types";
            this.tpTypes.UseVisualStyleBackColor = true;
            // 
            // lblAssemblyWithType
            // 
            this.lblAssemblyWithType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAssemblyWithType.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblAssemblyWithType.Location = new System.Drawing.Point(8, 482);
            this.lblAssemblyWithType.Name = "lblAssemblyWithType";
            this.lblAssemblyWithType.Size = new System.Drawing.Size(986, 26);
            this.lblAssemblyWithType.TabIndex = 2;
            this.lblAssemblyWithType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchTypeTxt
            // 
            this.searchTypeTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTypeTxt.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.searchTypeTxt.Location = new System.Drawing.Point(8, 14);
            this.searchTypeTxt.Name = "searchTypeTxt";
            this.searchTypeTxt.Size = new System.Drawing.Size(986, 27);
            this.searchTypeTxt.TabIndex = 1;
            this.searchTypeTxt.TextChanged += new System.EventHandler(this.searchTypeTxt_TextChanged);
            // 
            // lbTypes
            // 
            this.lbTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTypes.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lbTypes.FormattingEnabled = true;
            this.lbTypes.ItemHeight = 20;
            this.lbTypes.Location = new System.Drawing.Point(8, 55);
            this.lbTypes.Name = "lbTypes";
            this.lbTypes.Size = new System.Drawing.Size(986, 424);
            this.lbTypes.TabIndex = 0;
            this.lbTypes.SelectedIndexChanged += new System.EventHandler(this.lbTypes_SelectedIndexChanged);
            // 
            // tpErrors
            // 
            this.tpErrors.Controls.Add(this.tbErrors);
            this.tpErrors.Location = new System.Drawing.Point(4, 29);
            this.tpErrors.Name = "tpErrors";
            this.tpErrors.Size = new System.Drawing.Size(1004, 535);
            this.tpErrors.TabIndex = 2;
            this.tpErrors.Text = "Errors";
            this.tpErrors.UseVisualStyleBackColor = true;
            // 
            // tbErrors
            // 
            this.tbErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbErrors.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tbErrors.Location = new System.Drawing.Point(0, 0);
            this.tbErrors.Margin = new System.Windows.Forms.Padding(4);
            this.tbErrors.Multiline = true;
            this.tbErrors.Name = "tbErrors";
            this.tbErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbErrors.Size = new System.Drawing.Size(1004, 535);
            this.tbErrors.TabIndex = 3;
            this.tbErrors.WordWrap = false;
            // 
            // cbShowGac
            // 
            this.cbShowGac.AutoSize = true;
            this.cbShowGac.Checked = true;
            this.cbShowGac.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowGac.Location = new System.Drawing.Point(6, 56);
            this.cbShowGac.Name = "cbShowGac";
            this.cbShowGac.Size = new System.Drawing.Size(204, 24);
            this.cbShowGac.TabIndex = 3;
            this.cbShowGac.Text = "Show .NET assemblies";
            this.cbShowGac.UseVisualStyleBackColor = true;
            this.cbShowGac.CheckedChanged += new System.EventHandler(this.cbShowGac_CheckedChanged);
            // 
            // btnSelectFiles
            // 
            this.btnSelectFiles.Location = new System.Drawing.Point(6, 15);
            this.btnSelectFiles.Name = "btnSelectFiles";
            this.btnSelectFiles.Size = new System.Drawing.Size(154, 37);
            this.btnSelectFiles.TabIndex = 4;
            this.btnSelectFiles.Text = "Select file";
            this.btnSelectFiles.UseVisualStyleBackColor = true;
            this.btnSelectFiles.Click += new System.EventHandler(this.btnSelectFiles_Click);
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRescan.Location = new System.Drawing.Point(844, 15);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(154, 37);
            this.btnRescan.TabIndex = 5;
            this.btnRescan.Text = "Rescan";
            this.btnRescan.UseVisualStyleBackColor = true;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // searchTxt
            // 
            this.searchTxt.Location = new System.Drawing.Point(258, 22);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(250, 26);
            this.searchTxt.TabIndex = 6;
            this.searchTxt.TextChanged += new System.EventHandler(this.searchTxt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Search:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssMatchCounter,
            this.tssLoadedInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 633);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1010, 25);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssMatchCounter
            // 
            this.tssMatchCounter.Name = "tssMatchCounter";
            this.tssMatchCounter.Size = new System.Drawing.Size(37, 20);
            this.tssMatchCounter.Text = "xxxx";
            // 
            // tssLoadedInfo
            // 
            this.tssLoadedInfo.Name = "tssLoadedInfo";
            this.tssLoadedInfo.Size = new System.Drawing.Size(958, 20);
            this.tssLoadedInfo.Spring = true;
            this.tssLoadedInfo.Text = "yyy";
            this.tssLoadedInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 658);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchTxt);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.btnSelectFiles);
            this.Controls.Add(this.cbShowGac);
            this.Controls.Add(this.tcMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(700, 45);
            this.Name = "MainWindow";
            this.Text = "DependencyWalker.Net";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.tcMain.ResumeLayout(false);
            this.tpReferenceTree.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tpListOfReferences.ResumeLayout(false);
            this.tpListOfReferences.PerformLayout();
            this.tpTypes.ResumeLayout(false);
            this.tpTypes.PerformLayout();
            this.tpErrors.ResumeLayout(false);
            this.tpErrors.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TreeView tvReferencesTree;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpReferenceTree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tpListOfReferences;
        private System.Windows.Forms.TextBox tbUsedBy;
        private System.Windows.Forms.TextBox tbListOfReferences;
        private System.Windows.Forms.TabPage tpErrors;
        private System.Windows.Forms.TextBox tbErrors;
        private System.Windows.Forms.CheckBox cbShowGac;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnSelectFiles;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.TextBox searchTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssMatchCounter;
        private System.Windows.Forms.ToolStripStatusLabel tssLoadedInfo;
        private System.Windows.Forms.TabPage tpTypes;
        private System.Windows.Forms.ListBox lbTypes;
        private System.Windows.Forms.TextBox searchTypeTxt;
        private System.Windows.Forms.Label lblAssemblyWithType;
    }
}

