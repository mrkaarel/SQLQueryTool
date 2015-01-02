namespace SqlQueryTool.Forms
{
	partial class DatabaseObjectBrowser
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splDatabaseObjects = new System.Windows.Forms.SplitContainer();
			this.chkSearchSPContents = new System.Windows.Forms.CheckBox();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.trvDatabaseObjects = new System.Windows.Forms.TreeView();
			this.lblTableFieldsTitle = new System.Windows.Forms.Label();
			this.dgvTableFields = new System.Windows.Forms.DataGridView();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDescription = new System.Windows.Forms.DataGridViewImageColumn();
			this.searchTimer = new System.Windows.Forms.Timer(this.components);
			this.cmnTableCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnStoredProcCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnTableCommandsGlobal = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmnViewCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.btnRefresh = new System.Windows.Forms.Button();
			this.mniCreateSelectQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectAllRows = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectRowCount = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectTopRows = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectBottomRows = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCreateInsertQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCreateUpdateQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCreateDeleteQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCopyStoredProcName = new System.Windows.Forms.ToolStripMenuItem();
			this.mniGrantExecuteOnSP = new System.Windows.Forms.ToolStripMenuItem();
			this.mniBuildTableRowCountsQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSetTableRowFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.mniFindColumns = new System.Windows.Forms.ToolStripMenuItem();
			this.mniShowViewDefinition = new System.Windows.Forms.ToolStripMenuItem();
			this.splDatabaseObjects.Panel1.SuspendLayout();
			this.splDatabaseObjects.Panel2.SuspendLayout();
			this.splDatabaseObjects.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvTableFields)).BeginInit();
			this.cmnTableCommands.SuspendLayout();
			this.cmnStoredProcCommands.SuspendLayout();
			this.cmnTableCommandsGlobal.SuspendLayout();
			this.cmnViewCommands.SuspendLayout();
			this.SuspendLayout();
			// 
			// splDatabaseObjects
			// 
			this.splDatabaseObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splDatabaseObjects.Location = new System.Drawing.Point(3, 3);
			this.splDatabaseObjects.Name = "splDatabaseObjects";
			this.splDatabaseObjects.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splDatabaseObjects.Panel1
			// 
			this.splDatabaseObjects.Panel1.Controls.Add(this.chkSearchSPContents);
			this.splDatabaseObjects.Panel1.Controls.Add(this.btnRefresh);
			this.splDatabaseObjects.Panel1.Controls.Add(this.txtSearch);
			this.splDatabaseObjects.Panel1.Controls.Add(this.trvDatabaseObjects);
			// 
			// splDatabaseObjects.Panel2
			// 
			this.splDatabaseObjects.Panel2.Controls.Add(this.lblTableFieldsTitle);
			this.splDatabaseObjects.Panel2.Controls.Add(this.dgvTableFields);
			this.splDatabaseObjects.Size = new System.Drawing.Size(224, 382);
			this.splDatabaseObjects.SplitterDistance = 212;
			this.splDatabaseObjects.TabIndex = 8;
			// 
			// chkSearchSPContents
			// 
			this.chkSearchSPContents.AutoSize = true;
			this.chkSearchSPContents.Location = new System.Drawing.Point(4, 29);
			this.chkSearchSPContents.Name = "chkSearchSPContents";
			this.chkSearchSPContents.Size = new System.Drawing.Size(185, 17);
			this.chkSearchSPContents.TabIndex = 3;
			this.chkSearchSPContents.Text = "search stored proc/view contents";
			this.chkSearchSPContents.UseVisualStyleBackColor = true;
			this.chkSearchSPContents.CheckedChanged += new System.EventHandler(this.chkSearchSPContents_CheckedChanged);
			// 
			// txtSearch
			// 
			this.txtSearch.AllowDrop = true;
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.ForeColor = System.Drawing.Color.Gray;
			this.txtSearch.Location = new System.Drawing.Point(4, 3);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(183, 20);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.Text = "search…";
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			this.txtSearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtSearch_DragDrop);
			this.txtSearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtSearch_DragEnter);
			this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
			this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
			// 
			// trvDatabaseObjects
			// 
			this.trvDatabaseObjects.AllowDrop = true;
			this.trvDatabaseObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trvDatabaseObjects.Indent = 15;
			this.trvDatabaseObjects.Location = new System.Drawing.Point(4, 52);
			this.trvDatabaseObjects.Name = "trvDatabaseObjects";
			this.trvDatabaseObjects.Size = new System.Drawing.Size(217, 157);
			this.trvDatabaseObjects.TabIndex = 0;
			this.trvDatabaseObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvDatabaseObjects_AfterSelect);
			this.trvDatabaseObjects.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvDatabaseObjects_NodeMouseClick);
			this.trvDatabaseObjects.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvDatabaseObjects_NodeMouseDoubleClick);
			this.trvDatabaseObjects.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvDatabaseObjects_DragDrop);
			this.trvDatabaseObjects.DragOver += new System.Windows.Forms.DragEventHandler(this.trvDatabaseObjects_DragOver);
			// 
			// lblTableFieldsTitle
			// 
			this.lblTableFieldsTitle.AutoSize = true;
			this.lblTableFieldsTitle.Enabled = false;
			this.lblTableFieldsTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTableFieldsTitle.Name = "lblTableFieldsTitle";
			this.lblTableFieldsTitle.Size = new System.Drawing.Size(50, 13);
			this.lblTableFieldsTitle.TabIndex = 1;
			this.lblTableFieldsTitle.Text = "Columns:";
			// 
			// dgvTableFields
			// 
			this.dgvTableFields.AllowUserToAddRows = false;
			this.dgvTableFields.AllowUserToDeleteRows = false;
			this.dgvTableFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvTableFields.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvTableFields.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dgvTableFields.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dgvTableFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvTableFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colInfo,
            this.colDescription});
			this.dgvTableFields.Location = new System.Drawing.Point(4, 19);
			this.dgvTableFields.Name = "dgvTableFields";
			this.dgvTableFields.ReadOnly = true;
			this.dgvTableFields.RowHeadersVisible = false;
			this.dgvTableFields.Size = new System.Drawing.Size(217, 144);
			this.dgvTableFields.TabIndex = 0;
			// 
			// colName
			// 
			dataGridViewCellStyle1.NullValue = "null";
			this.colName.DefaultCellStyle = dataGridViewCellStyle1;
			this.colName.FillWeight = 137.8173F;
			this.colName.HeaderText = "Name";
			this.colName.MinimumWidth = 60;
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colInfo
			// 
			this.colInfo.FillWeight = 137.8173F;
			this.colInfo.HeaderText = "Definition";
			this.colInfo.MinimumWidth = 60;
			this.colInfo.Name = "colInfo";
			this.colInfo.ReadOnly = true;
			// 
			// colDescription
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.colDescription.DefaultCellStyle = dataGridViewCellStyle2;
			this.colDescription.FillWeight = 24.36548F;
			this.colDescription.HeaderText = "";
			this.colDescription.Name = "colDescription";
			this.colDescription.ReadOnly = true;
			// 
			// searchTimer
			// 
			this.searchTimer.Interval = 200;
			this.searchTimer.Tick += new System.EventHandler(this.searchTimer_Tick);
			// 
			// cmnTableCommands
			// 
			this.cmnTableCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCreateSelectQuery,
            this.mniCreateInsertQuery,
            this.mniCreateUpdateQuery,
            this.mniCreateDeleteQuery});
			this.cmnTableCommands.Name = "cmnDatabaseObjects";
			this.cmnTableCommands.Size = new System.Drawing.Size(189, 92);
			// 
			// cmnStoredProcCommands
			// 
			this.cmnStoredProcCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCopyStoredProcName,
            this.mniGrantExecuteOnSP});
			this.cmnStoredProcCommands.Name = "cmnStoredProcCommands";
			this.cmnStoredProcCommands.Size = new System.Drawing.Size(230, 48);
			// 
			// cmnTableCommandsGlobal
			// 
			this.cmnTableCommandsGlobal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniBuildTableRowCountsQuery,
            this.mniSetTableRowFilter,
            this.mniFindColumns});
			this.cmnTableCommandsGlobal.Name = "cmnTableCommandsGlobal";
			this.cmnTableCommandsGlobal.Size = new System.Drawing.Size(233, 70);
			// 
			// cmnViewCommands
			// 
			this.cmnViewCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniShowViewDefinition});
			this.cmnViewCommands.Name = "cmnViewCommands";
			this.cmnViewCommands.Size = new System.Drawing.Size(185, 48);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Image = global::SqlQueryTool.Properties.Resources.refresh;
			this.btnRefresh.Location = new System.Drawing.Point(193, 1);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(28, 23);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// mniCreateSelectQuery
			// 
			this.mniCreateSelectQuery.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniSelectAllRows,
            this.mniSelectRowCount,
            this.mniSelectTopRows,
            this.mniSelectBottomRows});
			this.mniCreateSelectQuery.Image = global::SqlQueryTool.Properties.Resources.table;
			this.mniCreateSelectQuery.Name = "mniCreateSelectQuery";
			this.mniCreateSelectQuery.Size = new System.Drawing.Size(188, 22);
			this.mniCreateSelectQuery.Text = "Create SELECT query";
			// 
			// mniSelectAllRows
			// 
			this.mniSelectAllRows.Image = global::SqlQueryTool.Properties.Resources.table;
			this.mniSelectAllRows.Name = "mniSelectAllRows";
			this.mniSelectAllRows.Size = new System.Drawing.Size(152, 22);
			this.mniSelectAllRows.Text = "All rows";
			this.mniSelectAllRows.Click += new System.EventHandler(this.mniSelectAllRows_Click);
			// 
			// mniSelectRowCount
			// 
			this.mniSelectRowCount.Image = global::SqlQueryTool.Properties.Resources.table_count;
			this.mniSelectRowCount.Name = "mniSelectRowCount";
			this.mniSelectRowCount.Size = new System.Drawing.Size(152, 22);
			this.mniSelectRowCount.Text = "Row count";
			this.mniSelectRowCount.Click += new System.EventHandler(this.mniSelectRowCount_Click);
			// 
			// mniSelectTopRows
			// 
			this.mniSelectTopRows.Image = global::SqlQueryTool.Properties.Resources.table_top;
			this.mniSelectTopRows.Name = "mniSelectTopRows";
			this.mniSelectTopRows.Size = new System.Drawing.Size(152, 22);
			this.mniSelectTopRows.Text = "First 100 rows";
			this.mniSelectTopRows.Click += new System.EventHandler(this.mniSelectTopRows_Click);
			// 
			// mniSelectBottomRows
			// 
			this.mniSelectBottomRows.Image = global::SqlQueryTool.Properties.Resources.table_bottom;
			this.mniSelectBottomRows.Name = "mniSelectBottomRows";
			this.mniSelectBottomRows.Size = new System.Drawing.Size(152, 22);
			this.mniSelectBottomRows.Text = "Last 100 rows";
			this.mniSelectBottomRows.Click += new System.EventHandler(this.mniSelectBottomRows_Click);
			// 
			// mniCreateInsertQuery
			// 
			this.mniCreateInsertQuery.Image = global::SqlQueryTool.Properties.Resources.table_row_insert;
			this.mniCreateInsertQuery.Name = "mniCreateInsertQuery";
			this.mniCreateInsertQuery.Size = new System.Drawing.Size(188, 22);
			this.mniCreateInsertQuery.Text = "Create INSERT query";
			this.mniCreateInsertQuery.Click += new System.EventHandler(this.mniCreateInsertQuery_Click);
			// 
			// mniCreateUpdateQuery
			// 
			this.mniCreateUpdateQuery.Image = global::SqlQueryTool.Properties.Resources.table_edit;
			this.mniCreateUpdateQuery.Name = "mniCreateUpdateQuery";
			this.mniCreateUpdateQuery.Size = new System.Drawing.Size(188, 22);
			this.mniCreateUpdateQuery.Text = "Create UPDATE query";
			this.mniCreateUpdateQuery.Click += new System.EventHandler(this.mniCreateUpdateQuery_Click);
			// 
			// mniCreateDeleteQuery
			// 
			this.mniCreateDeleteQuery.Image = global::SqlQueryTool.Properties.Resources.table_row_delete;
			this.mniCreateDeleteQuery.Name = "mniCreateDeleteQuery";
			this.mniCreateDeleteQuery.Size = new System.Drawing.Size(188, 22);
			this.mniCreateDeleteQuery.Text = "Create DELETE query";
			this.mniCreateDeleteQuery.Click += new System.EventHandler(this.mniCreateDeleteQuery_Click);
			// 
			// mniCopyStoredProcName
			// 
			this.mniCopyStoredProcName.Image = global::SqlQueryTool.Properties.Resources.page_copy;
			this.mniCopyStoredProcName.Name = "mniCopyStoredProcName";
			this.mniCopyStoredProcName.Size = new System.Drawing.Size(229, 22);
			this.mniCopyStoredProcName.Text = "Copy name to clipboard";
			this.mniCopyStoredProcName.Click += new System.EventHandler(this.mniCopyStoredProcName_Click);
			// 
			// mniGrantExecuteOnSP
			// 
			this.mniGrantExecuteOnSP.Image = global::SqlQueryTool.Properties.Resources.key_add;
			this.mniGrantExecuteOnSP.Name = "mniGrantExecuteOnSP";
			this.mniGrantExecuteOnSP.Size = new System.Drawing.Size(229, 22);
			this.mniGrantExecuteOnSP.Text = "Grant EXECUTE permissions…";
			this.mniGrantExecuteOnSP.Click += new System.EventHandler(this.mniGrantExecuteOnSP_Click);
			// 
			// mniBuildTableRowCountsQuery
			// 
			this.mniBuildTableRowCountsQuery.Image = global::SqlQueryTool.Properties.Resources.table_count;
			this.mniBuildTableRowCountsQuery.Name = "mniBuildTableRowCountsQuery";
			this.mniBuildTableRowCountsQuery.Size = new System.Drawing.Size(232, 22);
			this.mniBuildTableRowCountsQuery.Text = "Create table row counts query";
			this.mniBuildTableRowCountsQuery.Click += new System.EventHandler(this.mniBuildTableRowCountsQuery_Click);
			// 
			// mniSetTableRowFilter
			// 
			this.mniSetTableRowFilter.Image = global::SqlQueryTool.Properties.Resources.table_lightning;
			this.mniSetTableRowFilter.Name = "mniSetTableRowFilter";
			this.mniSetTableRowFilter.Size = new System.Drawing.Size(232, 22);
			this.mniSetTableRowFilter.Text = "Filter tables by row count…";
			this.mniSetTableRowFilter.Click += new System.EventHandler(this.mniSetTableRowFilter_Click);
			// 
			// mniFindColumns
			// 
			this.mniFindColumns.Image = global::SqlQueryTool.Properties.Resources.find;
			this.mniFindColumns.Name = "mniFindColumns";
			this.mniFindColumns.Size = new System.Drawing.Size(232, 22);
			this.mniFindColumns.Text = "Find columns…";
			this.mniFindColumns.Click += new System.EventHandler(this.mniFindColumns_Click);
			// 
			// mniShowViewDefinition
			// 
			this.mniShowViewDefinition.Image = global::SqlQueryTool.Properties.Resources.script_code;
			this.mniShowViewDefinition.Name = "mniShowViewDefinition";
			this.mniShowViewDefinition.Size = new System.Drawing.Size(184, 22);
			this.mniShowViewDefinition.Text = "Show view definition";
			this.mniShowViewDefinition.Click += new System.EventHandler(this.mniShowViewDefinition_Click);
			// 
			// DatabaseObjectBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splDatabaseObjects);
			this.Name = "DatabaseObjectBrowser";
			this.Size = new System.Drawing.Size(230, 388);
			this.splDatabaseObjects.Panel1.ResumeLayout(false);
			this.splDatabaseObjects.Panel1.PerformLayout();
			this.splDatabaseObjects.Panel2.ResumeLayout(false);
			this.splDatabaseObjects.Panel2.PerformLayout();
			this.splDatabaseObjects.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvTableFields)).EndInit();
			this.cmnTableCommands.ResumeLayout(false);
			this.cmnStoredProcCommands.ResumeLayout(false);
			this.cmnTableCommandsGlobal.ResumeLayout(false);
			this.cmnViewCommands.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splDatabaseObjects;
		private System.Windows.Forms.CheckBox chkSearchSPContents;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.TreeView trvDatabaseObjects;
		private System.Windows.Forms.Label lblTableFieldsTitle;
		private System.Windows.Forms.Timer searchTimer;
		private System.Windows.Forms.ContextMenuStrip cmnTableCommands;
		private System.Windows.Forms.ToolStripMenuItem mniCreateSelectQuery;
		private System.Windows.Forms.ToolStripMenuItem mniSelectAllRows;
		private System.Windows.Forms.ToolStripMenuItem mniSelectRowCount;
		private System.Windows.Forms.ToolStripMenuItem mniSelectTopRows;
		private System.Windows.Forms.ToolStripMenuItem mniSelectBottomRows;
		private System.Windows.Forms.ToolStripMenuItem mniCreateInsertQuery;
		private System.Windows.Forms.ToolStripMenuItem mniCreateUpdateQuery;
		private System.Windows.Forms.ToolStripMenuItem mniCreateDeleteQuery;
		private System.Windows.Forms.ContextMenuStrip cmnStoredProcCommands;
		private System.Windows.Forms.ToolStripMenuItem mniCopyStoredProcName;
		private System.Windows.Forms.ToolStripMenuItem mniGrantExecuteOnSP;
		private System.Windows.Forms.ContextMenuStrip cmnTableCommandsGlobal;
		private System.Windows.Forms.ToolStripMenuItem mniBuildTableRowCountsQuery;
		private System.Windows.Forms.ToolStripMenuItem mniSetTableRowFilter;
		private System.Windows.Forms.ToolStripMenuItem mniFindColumns;
		private System.Windows.Forms.ContextMenuStrip cmnViewCommands;
		private System.Windows.Forms.ToolStripMenuItem mniShowViewDefinition;
		private System.Windows.Forms.DataGridView dgvTableFields;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colInfo;
		private System.Windows.Forms.DataGridViewImageColumn colDescription;


	}
}
