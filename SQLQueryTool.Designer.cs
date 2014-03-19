namespace SqlQueryTool
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLQueryTool));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblStatusbarInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.splMainContent = new System.Windows.Forms.SplitContainer();
			this.grpDatabaseObjects = new System.Windows.Forms.GroupBox();
			this.splDatabaseObjects = new System.Windows.Forms.SplitContainer();
			this.chkSearchSPContents = new System.Windows.Forms.CheckBox();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.trvDatabaseObjects = new System.Windows.Forms.TreeView();
			this.lblTableFieldsTitle = new System.Windows.Forms.Label();
			this.dgvTableFields = new System.Windows.Forms.DataGridView();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDescription = new System.Windows.Forms.DataGridViewImageColumn();
			this.pnlConnection = new System.Windows.Forms.GroupBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnAddConnection = new System.Windows.Forms.Button();
			this.btnDeleteSelectedConnection = new System.Windows.Forms.Button();
			this.selPreviousConnections = new System.Windows.Forms.ComboBox();
			this.btnDeleteQuery = new System.Windows.Forms.Button();
			this.btnAddQuery = new System.Windows.Forms.Button();
			this.btnRunQuery = new System.Windows.Forms.Button();
			this.grpQueries = new System.Windows.Forms.GroupBox();
			this.tabQueries = new System.Windows.Forms.TabControl();
			this.appImages = new System.Windows.Forms.ImageList(this.components);
			this.cmnTableCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniCreateSelectQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectAllRows = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectRowCount = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectTopRows = new System.Windows.Forms.ToolStripMenuItem();
			this.mniSelectBottomRows = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCreateInsertQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCreateUpdateQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCreateDeleteQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnTabpage = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniCloseTabpage = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnStoredProcCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniCopyStoredProcName = new System.Windows.Forms.ToolStripMenuItem();
			this.mniGrantExecuteOnSP = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnTableCommandsGlobal = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniShowTableRowCounts = new System.Windows.Forms.ToolStripMenuItem();
			this.mniHideEmptyTables = new System.Windows.Forms.ToolStripMenuItem();
			this.mniFindColumns = new System.Windows.Forms.ToolStripMenuItem();
			this.cmnViewCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniShowViewDefinition = new System.Windows.Forms.ToolStripMenuItem();
			this.searchTimer = new System.Windows.Forms.Timer(this.components);
			this.statusStrip1.SuspendLayout();
			this.splMainContent.Panel1.SuspendLayout();
			this.splMainContent.Panel2.SuspendLayout();
			this.splMainContent.SuspendLayout();
			this.grpDatabaseObjects.SuspendLayout();
			this.splDatabaseObjects.Panel1.SuspendLayout();
			this.splDatabaseObjects.Panel2.SuspendLayout();
			this.splDatabaseObjects.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvTableFields)).BeginInit();
			this.pnlConnection.SuspendLayout();
			this.grpQueries.SuspendLayout();
			this.cmnTableCommands.SuspendLayout();
			this.cmnTabpage.SuspendLayout();
			this.cmnStoredProcCommands.SuspendLayout();
			this.cmnTableCommandsGlobal.SuspendLayout();
			this.cmnViewCommands.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusbarInfo});
			this.statusStrip1.Location = new System.Drawing.Point(0, 573);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(892, 22);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
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
			this.splMainContent.Panel2.Controls.Add(this.btnDeleteQuery);
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
			this.grpDatabaseObjects.Controls.Add(this.splDatabaseObjects);
			this.grpDatabaseObjects.Enabled = false;
			this.grpDatabaseObjects.Location = new System.Drawing.Point(3, 88);
			this.grpDatabaseObjects.Name = "grpDatabaseObjects";
			this.grpDatabaseObjects.Size = new System.Drawing.Size(258, 482);
			this.grpDatabaseObjects.TabIndex = 15;
			this.grpDatabaseObjects.TabStop = false;
			this.grpDatabaseObjects.Text = "Andmebaasi objektid";
			// 
			// splDatabaseObjects
			// 
			this.splDatabaseObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splDatabaseObjects.Location = new System.Drawing.Point(6, 19);
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
			this.splDatabaseObjects.Size = new System.Drawing.Size(246, 457);
			this.splDatabaseObjects.SplitterDistance = 255;
			this.splDatabaseObjects.TabIndex = 7;
			// 
			// chkSearchSPContents
			// 
			this.chkSearchSPContents.AutoSize = true;
			this.chkSearchSPContents.Location = new System.Drawing.Point(4, 29);
			this.chkSearchSPContents.Name = "chkSearchSPContents";
			this.chkSearchSPContents.Size = new System.Drawing.Size(197, 17);
			this.chkSearchSPContents.TabIndex = 3;
			this.chkSearchSPContents.Text = "otsi ka protseduuride/vaadete sisust";
			this.chkSearchSPContents.UseVisualStyleBackColor = true;
			this.chkSearchSPContents.CheckedChanged += new System.EventHandler(this.chkSearchSPContents_CheckedChanged);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Image = global::SqlQueryTool.Properties.Resources.refresh;
			this.btnRefresh.Location = new System.Drawing.Point(219, 0);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(28, 23);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.AllowDrop = true;
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.ForeColor = System.Drawing.Color.Gray;
			this.txtSearch.Location = new System.Drawing.Point(1, 2);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(212, 20);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.Text = "otsi...";
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
			this.trvDatabaseObjects.Location = new System.Drawing.Point(0, 52);
			this.trvDatabaseObjects.Name = "trvDatabaseObjects";
			this.trvDatabaseObjects.Size = new System.Drawing.Size(246, 200);
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
			this.lblTableFieldsTitle.Size = new System.Drawing.Size(70, 13);
			this.lblTableFieldsTitle.TabIndex = 1;
			this.lblTableFieldsTitle.Text = "Tabeli väljad:";
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
			this.dgvTableFields.Location = new System.Drawing.Point(0, 16);
			this.dgvTableFields.Name = "dgvTableFields";
			this.dgvTableFields.ReadOnly = true;
			this.dgvTableFields.RowHeadersVisible = false;
			this.dgvTableFields.Size = new System.Drawing.Size(246, 182);
			this.dgvTableFields.TabIndex = 0;
			// 
			// colName
			// 
			dataGridViewCellStyle3.NullValue = "null";
			this.colName.DefaultCellStyle = dataGridViewCellStyle3;
			this.colName.FillWeight = 137.8173F;
			this.colName.HeaderText = "Väli";
			this.colName.MinimumWidth = 60;
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colInfo
			// 
			this.colInfo.FillWeight = 137.8173F;
			this.colInfo.HeaderText = "Info";
			this.colInfo.MinimumWidth = 60;
			this.colInfo.Name = "colInfo";
			this.colInfo.ReadOnly = true;
			// 
			// colDescription
			// 
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.colDescription.DefaultCellStyle = dataGridViewCellStyle4;
			this.colDescription.FillWeight = 24.36548F;
			this.colDescription.HeaderText = "";
			this.colDescription.Name = "colDescription";
			this.colDescription.ReadOnly = true;
			// 
			// pnlConnection
			// 
			this.pnlConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlConnection.Controls.Add(this.btnConnect);
			this.pnlConnection.Controls.Add(this.btnAddConnection);
			this.pnlConnection.Controls.Add(this.btnDeleteSelectedConnection);
			this.pnlConnection.Controls.Add(this.selPreviousConnections);
			this.pnlConnection.Location = new System.Drawing.Point(3, 3);
			this.pnlConnection.Name = "pnlConnection";
			this.pnlConnection.Size = new System.Drawing.Size(258, 79);
			this.pnlConnection.TabIndex = 13;
			this.pnlConnection.TabStop = false;
			this.pnlConnection.Text = "Ühenduse seaded";
			// 
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.Image = global::SqlQueryTool.Properties.Resources.database_go;
			this.btnConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnConnect.Location = new System.Drawing.Point(182, 46);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(70, 25);
			this.btnConnect.TabIndex = 17;
			this.btnConnect.Text = "Ühenda";
			this.btnConnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnAddConnection
			// 
			this.btnAddConnection.Image = global::SqlQueryTool.Properties.Resources.database_add;
			this.btnAddConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnAddConnection.Location = new System.Drawing.Point(6, 46);
			this.btnAddConnection.Name = "btnAddConnection";
			this.btnAddConnection.Size = new System.Drawing.Size(62, 25);
			this.btnAddConnection.TabIndex = 16;
			this.btnAddConnection.Text = "Uus...";
			this.btnAddConnection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAddConnection.UseVisualStyleBackColor = true;
			this.btnAddConnection.Click += new System.EventHandler(this.btnAddConnection_Click);
			// 
			// btnDeleteSelectedConnection
			// 
			this.btnDeleteSelectedConnection.Image = global::SqlQueryTool.Properties.Resources.database_delete;
			this.btnDeleteSelectedConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDeleteSelectedConnection.Location = new System.Drawing.Point(74, 46);
			this.btnDeleteSelectedConnection.Name = "btnDeleteSelectedConnection";
			this.btnDeleteSelectedConnection.Size = new System.Drawing.Size(69, 25);
			this.btnDeleteSelectedConnection.TabIndex = 13;
			this.btnDeleteSelectedConnection.Text = "Kustuta";
			this.btnDeleteSelectedConnection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDeleteSelectedConnection.UseVisualStyleBackColor = true;
			this.btnDeleteSelectedConnection.Click += new System.EventHandler(this.btnDeleteSelectedConnection_Click);
			// 
			// selPreviousConnections
			// 
			this.selPreviousConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.selPreviousConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selPreviousConnections.FormattingEnabled = true;
			this.selPreviousConnections.Location = new System.Drawing.Point(6, 19);
			this.selPreviousConnections.Name = "selPreviousConnections";
			this.selPreviousConnections.Size = new System.Drawing.Size(246, 21);
			this.selPreviousConnections.TabIndex = 11;
			this.selPreviousConnections.SelectedIndexChanged += new System.EventHandler(this.selPreviousConnections_SelectedIndexChanged);
			// 
			// btnDeleteQuery
			// 
			this.btnDeleteQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDeleteQuery.Image = global::SqlQueryTool.Properties.Resources.script_delete;
			this.btnDeleteQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDeleteQuery.Location = new System.Drawing.Point(102, 543);
			this.btnDeleteQuery.Name = "btnDeleteQuery";
			this.btnDeleteQuery.Size = new System.Drawing.Size(94, 26);
			this.btnDeleteQuery.TabIndex = 3;
			this.btnDeleteQuery.Text = "Sulge päring";
			this.btnDeleteQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDeleteQuery.UseVisualStyleBackColor = true;
			this.btnDeleteQuery.Click += new System.EventHandler(this.btnDeleteQuery_Click);
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
			this.btnAddQuery.Text = "Uus päring...";
			this.btnAddQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAddQuery.UseVisualStyleBackColor = true;
			this.btnAddQuery.Click += new System.EventHandler(this.btnAddQuery_Click);
			// 
			// btnRunQuery
			// 
			this.btnRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRunQuery.Image = global::SqlQueryTool.Properties.Resources.script_go;
			this.btnRunQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRunQuery.Location = new System.Drawing.Point(518, 543);
			this.btnRunQuery.Name = "btnRunQuery";
			this.btnRunQuery.Size = new System.Drawing.Size(96, 26);
			this.btnRunQuery.TabIndex = 1;
			this.btnRunQuery.Text = "Käivita päring";
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
			this.grpQueries.Text = "Päringud";
			// 
			// tabQueries
			// 
			this.tabQueries.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabQueries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabQueries.ImageList = this.appImages;
			this.tabQueries.Location = new System.Drawing.Point(3, 16);
			this.tabQueries.Name = "tabQueries";
			this.tabQueries.SelectedIndex = 0;
			this.tabQueries.Size = new System.Drawing.Size(608, 520);
			this.tabQueries.TabIndex = 0;
			this.tabQueries.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabQueries_MouseClick);
			// 
			// appImages
			// 
			this.appImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("appImages.ImageStream")));
			this.appImages.TransparentColor = System.Drawing.Color.Transparent;
			this.appImages.Images.SetKeyName(0, "script");
			// 
			// cmnTableCommands
			// 
			this.cmnTableCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCreateSelectQuery,
            this.mniCreateInsertQuery,
            this.mniCreateUpdateQuery,
            this.mniCreateDeleteQuery});
			this.cmnTableCommands.Name = "cmnDatabaseObjects";
			this.cmnTableCommands.Size = new System.Drawing.Size(197, 92);
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
			this.mniCreateSelectQuery.Size = new System.Drawing.Size(196, 22);
			this.mniCreateSelectQuery.Text = "Koosta SELECT-päring";
			// 
			// mniSelectAllRows
			// 
			this.mniSelectAllRows.Image = global::SqlQueryTool.Properties.Resources.table;
			this.mniSelectAllRows.Name = "mniSelectAllRows";
			this.mniSelectAllRows.Size = new System.Drawing.Size(167, 22);
			this.mniSelectAllRows.Text = "Kõik read";
			this.mniSelectAllRows.Click += new System.EventHandler(this.mniCreateSelectQuery_Click);
			// 
			// mniSelectRowCount
			// 
			this.mniSelectRowCount.Image = global::SqlQueryTool.Properties.Resources.table_count;
			this.mniSelectRowCount.Name = "mniSelectRowCount";
			this.mniSelectRowCount.Size = new System.Drawing.Size(167, 22);
			this.mniSelectRowCount.Text = "Ridade arv";
			this.mniSelectRowCount.Click += new System.EventHandler(this.mniSelectRowCount_Click);
			// 
			// mniSelectTopRows
			// 
			this.mniSelectTopRows.Image = global::SqlQueryTool.Properties.Resources.table_top;
			this.mniSelectTopRows.Name = "mniSelectTopRows";
			this.mniSelectTopRows.Size = new System.Drawing.Size(167, 22);
			this.mniSelectTopRows.Text = "Esimesed 100 rida";
			this.mniSelectTopRows.Click += new System.EventHandler(this.mniSelectTopRows_Click);
			// 
			// mniSelectBottomRows
			// 
			this.mniSelectBottomRows.Image = global::SqlQueryTool.Properties.Resources.table_bottom;
			this.mniSelectBottomRows.Name = "mniSelectBottomRows";
			this.mniSelectBottomRows.Size = new System.Drawing.Size(167, 22);
			this.mniSelectBottomRows.Text = "Viimased 100 rida";
			this.mniSelectBottomRows.Click += new System.EventHandler(this.mniSelectBottomRows_Click);
			// 
			// mniCreateInsertQuery
			// 
			this.mniCreateInsertQuery.Image = global::SqlQueryTool.Properties.Resources.table_row_insert;
			this.mniCreateInsertQuery.Name = "mniCreateInsertQuery";
			this.mniCreateInsertQuery.Size = new System.Drawing.Size(196, 22);
			this.mniCreateInsertQuery.Text = "Koosta INSERT-päring";
			this.mniCreateInsertQuery.Click += new System.EventHandler(this.mniCreateInsertQuery_Click);
			// 
			// mniCreateUpdateQuery
			// 
			this.mniCreateUpdateQuery.Image = global::SqlQueryTool.Properties.Resources.table_edit;
			this.mniCreateUpdateQuery.Name = "mniCreateUpdateQuery";
			this.mniCreateUpdateQuery.Size = new System.Drawing.Size(196, 22);
			this.mniCreateUpdateQuery.Text = "Koosta UPDATE-päring";
			this.mniCreateUpdateQuery.Click += new System.EventHandler(this.mniCreateUpdateQuery_Click);
			// 
			// mniCreateDeleteQuery
			// 
			this.mniCreateDeleteQuery.Image = global::SqlQueryTool.Properties.Resources.table_row_delete;
			this.mniCreateDeleteQuery.Name = "mniCreateDeleteQuery";
			this.mniCreateDeleteQuery.Size = new System.Drawing.Size(196, 22);
			this.mniCreateDeleteQuery.Text = "Koosta DELETE-päring";
			this.mniCreateDeleteQuery.Click += new System.EventHandler(this.mniCreateDeleteQuery_Click);
			// 
			// cmnTabpage
			// 
			this.cmnTabpage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCloseTabpage});
			this.cmnTabpage.Name = "cmnTabpage";
			this.cmnTabpage.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.cmnTabpage.Size = new System.Drawing.Size(141, 26);
			// 
			// mniCloseTabpage
			// 
			this.mniCloseTabpage.Image = global::SqlQueryTool.Properties.Resources.cross;
			this.mniCloseTabpage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.mniCloseTabpage.Name = "mniCloseTabpage";
			this.mniCloseTabpage.Size = new System.Drawing.Size(140, 22);
			this.mniCloseTabpage.Text = "Sulge päring";
			this.mniCloseTabpage.Click += new System.EventHandler(this.mniCloseTabpage_Click);
			// 
			// cmnStoredProcCommands
			// 
			this.cmnStoredProcCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCopyStoredProcName,
            this.mniGrantExecuteOnSP});
			this.cmnStoredProcCommands.Name = "cmnStoredProcCommands";
			this.cmnStoredProcCommands.Size = new System.Drawing.Size(239, 48);
			// 
			// mniCopyStoredProcName
			// 
			this.mniCopyStoredProcName.Image = global::SqlQueryTool.Properties.Resources.page_copy;
			this.mniCopyStoredProcName.Name = "mniCopyStoredProcName";
			this.mniCopyStoredProcName.Size = new System.Drawing.Size(238, 22);
			this.mniCopyStoredProcName.Text = "Kopeeri nimi";
			this.mniCopyStoredProcName.Click += new System.EventHandler(this.mniCopyStoredProcName_Click);
			// 
			// mniGrantExecuteOnSP
			// 
			this.mniGrantExecuteOnSP.Image = global::SqlQueryTool.Properties.Resources.key_add;
			this.mniGrantExecuteOnSP.Name = "mniGrantExecuteOnSP";
			this.mniGrantExecuteOnSP.Size = new System.Drawing.Size(238, 22);
			this.mniGrantExecuteOnSP.Text = "Anna kasutajale käivitusõigus...";
			this.mniGrantExecuteOnSP.Click += new System.EventHandler(this.mniGrantExecuteOnSP_Click);
			// 
			// cmnTableCommandsGlobal
			// 
			this.cmnTableCommandsGlobal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniShowTableRowCounts,
            this.mniHideEmptyTables,
            this.mniFindColumns});
			this.cmnTableCommandsGlobal.Name = "cmnTableCommandsGlobal";
			this.cmnTableCommandsGlobal.Size = new System.Drawing.Size(232, 70);
			// 
			// mniShowTableRowCounts
			// 
			this.mniShowTableRowCounts.Image = global::SqlQueryTool.Properties.Resources.table_count;
			this.mniShowTableRowCounts.Name = "mniShowTableRowCounts";
			this.mniShowTableRowCounts.Size = new System.Drawing.Size(231, 22);
			this.mniShowTableRowCounts.Text = "Kõigi tabelite ridade arvud";
			this.mniShowTableRowCounts.Click += new System.EventHandler(this.mniShowTableRowCounts_Click);
			// 
			// mniHideEmptyTables
			// 
			this.mniHideEmptyTables.Image = global::SqlQueryTool.Properties.Resources.table_lightning;
			this.mniHideEmptyTables.Name = "mniHideEmptyTables";
			this.mniHideEmptyTables.Size = new System.Drawing.Size(231, 22);
			this.mniHideEmptyTables.Text = "Näita tabeleid ridade arvuga…";
			this.mniHideEmptyTables.Click += new System.EventHandler(this.mniHideEmptyTables_Click);
			// 
			// mniFindColumns
			// 
			this.mniFindColumns.Image = global::SqlQueryTool.Properties.Resources.find;
			this.mniFindColumns.Name = "mniFindColumns";
			this.mniFindColumns.Size = new System.Drawing.Size(231, 22);
			this.mniFindColumns.Text = "Otsi tulpasid…";
			this.mniFindColumns.Click += new System.EventHandler(this.mniFindColumns_Click);
			// 
			// cmnViewCommands
			// 
			this.cmnViewCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniShowViewDefinition});
			this.cmnViewCommands.Name = "cmnViewCommands";
			this.cmnViewCommands.Size = new System.Drawing.Size(203, 26);
			// 
			// mniShowViewDefinition
			// 
			this.mniShowViewDefinition.Image = global::SqlQueryTool.Properties.Resources.script_code;
			this.mniShowViewDefinition.Name = "mniShowViewDefinition";
			this.mniShowViewDefinition.Size = new System.Drawing.Size(202, 22);
			this.mniShowViewDefinition.Text = "Näita vaate definitsiooni";
			this.mniShowViewDefinition.Click += new System.EventHandler(this.mniShowViewDefinition_Click);
			// 
			// searchTimer
			// 
			this.searchTimer.Interval = 200;
			this.searchTimer.Tick += new System.EventHandler(this.searchTimer_Tick);
			// 
			// SQLQueryTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 595);
			this.Controls.Add(this.splMainContent);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SQLQueryTool";
			this.Text = "SQL query tool";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splMainContent.Panel1.ResumeLayout(false);
			this.splMainContent.Panel2.ResumeLayout(false);
			this.splMainContent.ResumeLayout(false);
			this.grpDatabaseObjects.ResumeLayout(false);
			this.splDatabaseObjects.Panel1.ResumeLayout(false);
			this.splDatabaseObjects.Panel1.PerformLayout();
			this.splDatabaseObjects.Panel2.ResumeLayout(false);
			this.splDatabaseObjects.Panel2.PerformLayout();
			this.splDatabaseObjects.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvTableFields)).EndInit();
			this.pnlConnection.ResumeLayout(false);
			this.grpQueries.ResumeLayout(false);
			this.cmnTableCommands.ResumeLayout(false);
			this.cmnTabpage.ResumeLayout(false);
			this.cmnStoredProcCommands.ResumeLayout(false);
			this.cmnTableCommandsGlobal.ResumeLayout(false);
			this.cmnViewCommands.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusbarInfo;
        private System.Windows.Forms.SplitContainer splMainContent;
        private System.Windows.Forms.SplitContainer splDatabaseObjects;
        private System.Windows.Forms.TreeView trvDatabaseObjects;
        private System.Windows.Forms.Label lblTableFieldsTitle;
        private System.Windows.Forms.DataGridView dgvTableFields;
        private System.Windows.Forms.GroupBox grpDatabaseObjects;
        private System.Windows.Forms.GroupBox pnlConnection;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnAddConnection;
        private System.Windows.Forms.Button btnDeleteSelectedConnection;
        private System.Windows.Forms.ComboBox selPreviousConnections;
        private System.Windows.Forms.GroupBox grpQueries;
        private System.Windows.Forms.TabControl tabQueries;
        private System.Windows.Forms.Button btnAddQuery;
        private System.Windows.Forms.Button btnRunQuery;
        private System.Windows.Forms.Button btnDeleteQuery;
        private System.Windows.Forms.ImageList appImages;
        private System.Windows.Forms.ContextMenuStrip cmnTabpage;
        private System.Windows.Forms.ToolStripMenuItem mniCloseTabpage;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ContextMenuStrip cmnTableCommands;
        private System.Windows.Forms.ToolStripMenuItem mniCreateSelectQuery;
        private System.Windows.Forms.ToolStripMenuItem mniCreateInsertQuery;
        private System.Windows.Forms.ToolStripMenuItem mniCreateDeleteQuery;
        private System.Windows.Forms.ToolStripMenuItem mniCreateUpdateQuery;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfo;
        private System.Windows.Forms.DataGridViewImageColumn colDescription;
        private System.Windows.Forms.ToolStripMenuItem mniSelectTopRows;
        private System.Windows.Forms.ToolStripMenuItem mniSelectBottomRows;
        private System.Windows.Forms.ToolStripMenuItem mniSelectAllRows;
        private System.Windows.Forms.ContextMenuStrip cmnStoredProcCommands;
        private System.Windows.Forms.ToolStripMenuItem mniCopyStoredProcName;
        private System.Windows.Forms.ToolStripMenuItem mniGrantExecuteOnSP;
        private System.Windows.Forms.CheckBox chkSearchSPContents;
        private System.Windows.Forms.ToolStripMenuItem mniSelectRowCount;
		private System.Windows.Forms.ContextMenuStrip cmnTableCommandsGlobal;
		private System.Windows.Forms.ToolStripMenuItem mniShowTableRowCounts;
		private System.Windows.Forms.ToolStripMenuItem mniHideEmptyTables;
		private System.Windows.Forms.ToolStripMenuItem mniFindColumns;
		private System.Windows.Forms.ContextMenuStrip cmnViewCommands;
		private System.Windows.Forms.ToolStripMenuItem mniShowViewDefinition;
		private System.Windows.Forms.Timer searchTimer;


    }
}

