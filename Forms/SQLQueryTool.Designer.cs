using SqlQueryTool.Utils;
namespace SqlQueryTool.Forms
{
	partial class SQLQueryTool
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
			if (disposing && (components != null)) {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLQueryTool));
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.lblStatusbarInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.splMainContent = new System.Windows.Forms.SplitContainer();
			this.grpDatabaseObjects = new System.Windows.Forms.GroupBox();
			this.databaseObjectBrowser = new SqlQueryTool.Forms.DatabaseObjectBrowser();
			this.pnlConnection = new System.Windows.Forms.GroupBox();
			this.connectionManager = new SqlQueryTool.Forms.ConnectionManager();
			this.btnCloseQuery = new System.Windows.Forms.Button();
			this.btnAddQuery = new System.Windows.Forms.Button();
			this.btnRunQuery = new System.Windows.Forms.Button();
			this.grpQueries = new System.Windows.Forms.GroupBox();
			this.tabQueries = new SqlQueryTool.Utils.DraggableTabControl();
			this.appImages = new System.Windows.Forms.ImageList(this.components);
			this.cmnTabpage = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniCloseTabpage = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip.SuspendLayout();
			this.splMainContent.Panel1.SuspendLayout();
			this.splMainContent.Panel2.SuspendLayout();
			this.splMainContent.SuspendLayout();
			this.grpDatabaseObjects.SuspendLayout();
			this.pnlConnection.SuspendLayout();
			this.grpQueries.SuspendLayout();
			this.cmnTabpage.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusbarInfo});
			this.statusStrip.Location = new System.Drawing.Point(0, 573);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(892, 22);
			this.statusStrip.TabIndex = 1;
			this.statusStrip.Text = "statusStrip1";
			// 
			// lblStatusbarInfo
			// 
			this.lblStatusbarInfo.Name = "lblStatusbarInfo";
			this.lblStatusbarInfo.Size = new System.Drawing.Size(0, 17);
			// 
			// splMainContent
			// 
			this.splMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splMainContent.Location = new System.Drawing.Point(0, 0);
			this.splMainContent.Name = "splMainContent";
			// 
			// splMainContent.Panel1
			// 
			this.splMainContent.Panel1.Controls.Add(this.grpDatabaseObjects);
			this.splMainContent.Panel1.Controls.Add(this.pnlConnection);
			this.splMainContent.Panel1MinSize = 235;
			// 
			// splMainContent.Panel2
			// 
			this.splMainContent.Panel2.Controls.Add(this.btnCloseQuery);
			this.splMainContent.Panel2.Controls.Add(this.btnAddQuery);
			this.splMainContent.Panel2.Controls.Add(this.btnRunQuery);
			this.splMainContent.Panel2.Controls.Add(this.grpQueries);
			this.splMainContent.Panel2.Enabled = false;
			this.splMainContent.Size = new System.Drawing.Size(892, 573);
			this.splMainContent.SplitterDistance = 268;
			this.splMainContent.TabIndex = 7;
			// 
			// grpDatabaseObjects
			// 
			this.grpDatabaseObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpDatabaseObjects.Controls.Add(this.databaseObjectBrowser);
			this.grpDatabaseObjects.Enabled = false;
			this.grpDatabaseObjects.Location = new System.Drawing.Point(3, 88);
			this.grpDatabaseObjects.Name = "grpDatabaseObjects";
			this.grpDatabaseObjects.Size = new System.Drawing.Size(258, 482);
			this.grpDatabaseObjects.TabIndex = 15;
			this.grpDatabaseObjects.TabStop = false;
			this.grpDatabaseObjects.Text = "Database objects";
			// 
			// databaseObjectBrowser
			// 
			this.databaseObjectBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.databaseObjectBrowser.Location = new System.Drawing.Point(3, 16);
			this.databaseObjectBrowser.Name = "databaseObjectBrowser";
			this.databaseObjectBrowser.Size = new System.Drawing.Size(252, 463);
			this.databaseObjectBrowser.TabIndex = 0;
			// 
			// pnlConnection
			// 
			this.pnlConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlConnection.Controls.Add(this.connectionManager);
			this.pnlConnection.Location = new System.Drawing.Point(3, 3);
			this.pnlConnection.Name = "pnlConnection";
			this.pnlConnection.Size = new System.Drawing.Size(258, 79);
			this.pnlConnection.TabIndex = 13;
			this.pnlConnection.TabStop = false;
			this.pnlConnection.Text = "Connection settings";
			// 
			// connectionManager
			// 
			this.connectionManager.Dock = System.Windows.Forms.DockStyle.Fill;
			this.connectionManager.Location = new System.Drawing.Point(3, 16);
			this.connectionManager.Name = "connectionManager";
			this.connectionManager.Size = new System.Drawing.Size(252, 60);
			this.connectionManager.TabIndex = 0;
			// 
			// btnCloseQuery
			// 
			this.btnCloseQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCloseQuery.Enabled = false;
			this.btnCloseQuery.Image = global::SqlQueryTool.Properties.Resources.script_delete;
			this.btnCloseQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCloseQuery.Location = new System.Drawing.Point(102, 543);
			this.btnCloseQuery.Name = "btnCloseQuery";
			this.btnCloseQuery.Size = new System.Drawing.Size(90, 26);
			this.btnCloseQuery.TabIndex = 3;
			this.btnCloseQuery.Text = "Close query";
			this.btnCloseQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnCloseQuery.UseVisualStyleBackColor = true;
			this.btnCloseQuery.Click += new System.EventHandler(this.btnCloseQuery_Click);
			// 
			// btnAddQuery
			// 
			this.btnAddQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddQuery.Image = global::SqlQueryTool.Properties.Resources.script_add;
			this.btnAddQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnAddQuery.Location = new System.Drawing.Point(3, 543);
			this.btnAddQuery.Name = "btnAddQuery";
			this.btnAddQuery.Size = new System.Drawing.Size(93, 26);
			this.btnAddQuery.TabIndex = 2;
			this.btnAddQuery.Text = "New query…";
			this.btnAddQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAddQuery.UseVisualStyleBackColor = true;
			this.btnAddQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
			// 
			// btnRunQuery
			// 
			this.btnRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRunQuery.Enabled = false;
			this.btnRunQuery.Image = global::SqlQueryTool.Properties.Resources.script_go;
			this.btnRunQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRunQuery.Location = new System.Drawing.Point(531, 543);
			this.btnRunQuery.Name = "btnRunQuery";
			this.btnRunQuery.Size = new System.Drawing.Size(83, 26);
			this.btnRunQuery.TabIndex = 1;
			this.btnRunQuery.Text = "Run query";
			this.btnRunQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnRunQuery.UseVisualStyleBackColor = true;
			this.btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
			// 
			// grpQueries
			// 
			this.grpQueries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grpQueries.Controls.Add(this.tabQueries);
			this.grpQueries.Location = new System.Drawing.Point(3, 3);
			this.grpQueries.Name = "grpQueries";
			this.grpQueries.Size = new System.Drawing.Size(614, 536);
			this.grpQueries.TabIndex = 0;
			this.grpQueries.TabStop = false;
			this.grpQueries.Text = "Queries";
			// 
			// tabQueries
			// 
			this.tabQueries.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabQueries.AllowDrop = true;
			this.tabQueries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabQueries.ImageList = this.appImages;
			this.tabQueries.Location = new System.Drawing.Point(3, 16);
			this.tabQueries.Name = "tabQueries";
			this.tabQueries.SelectedIndex = 0;
			this.tabQueries.Size = new System.Drawing.Size(608, 520);
			this.tabQueries.TabIndex = 0;
			this.tabQueries.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.tabQueries_TabCountChanged);
			this.tabQueries.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.tabQueries_TabCountChanged);
			this.tabQueries.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabQueries_MouseClick);
			this.tabQueries.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tabQueries_MouseDoubleClick);
			// 
			// appImages
			// 
			this.appImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("appImages.ImageStream")));
			this.appImages.TransparentColor = System.Drawing.Color.Transparent;
			this.appImages.Images.SetKeyName(0, "script");
			// 
			// cmnTabpage
			// 
			this.cmnTabpage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCloseTabpage});
			this.cmnTabpage.Name = "cmnTabpage";
			this.cmnTabpage.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.cmnTabpage.Size = new System.Drawing.Size(137, 26);
			// 
			// mniCloseTabpage
			// 
			this.mniCloseTabpage.Image = global::SqlQueryTool.Properties.Resources.cross;
			this.mniCloseTabpage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.mniCloseTabpage.Name = "mniCloseTabpage";
			this.mniCloseTabpage.Size = new System.Drawing.Size(136, 22);
			this.mniCloseTabpage.Text = "Close query";
			this.mniCloseTabpage.Click += new System.EventHandler(this.mniCloseTabpage_Click);
			// 
			// SQLQueryTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 595);
			this.Controls.Add(this.splMainContent);
			this.Controls.Add(this.statusStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "SQLQueryTool";
			this.Text = "SQL query tool";
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.splMainContent.Panel1.ResumeLayout(false);
			this.splMainContent.Panel2.ResumeLayout(false);
			this.splMainContent.ResumeLayout(false);
			this.grpDatabaseObjects.ResumeLayout(false);
			this.pnlConnection.ResumeLayout(false);
			this.grpQueries.ResumeLayout(false);
			this.cmnTabpage.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel lblStatusbarInfo;
		private System.Windows.Forms.SplitContainer splMainContent;
		private System.Windows.Forms.GroupBox grpDatabaseObjects;
		private System.Windows.Forms.GroupBox pnlConnection;
		private System.Windows.Forms.GroupBox grpQueries;
		private System.Windows.Forms.Button btnAddQuery;
		private System.Windows.Forms.Button btnRunQuery;
		private System.Windows.Forms.Button btnCloseQuery;
		private System.Windows.Forms.ImageList appImages;
		private System.Windows.Forms.ContextMenuStrip cmnTabpage;
		private System.Windows.Forms.ToolStripMenuItem mniCloseTabpage;
		private ConnectionManager connectionManager;
		private DatabaseObjectBrowser databaseObjectBrowser;
		private DraggableTabControl tabQueries;


	}
}

