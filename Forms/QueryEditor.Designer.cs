namespace SqlQueryTool.Forms
{
	partial class QueryEditor
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
			this.splQuery = new System.Windows.Forms.SplitContainer();
			this.pnlHelper = new System.Windows.Forms.Panel();
			this.dgResults = new System.Windows.Forms.DataGridView();
			this.cmnQueryResultsCommands = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mniCreateRowUpdateQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.mniCreateRowDeleteQuery = new System.Windows.Forms.ToolStripMenuItem();
			this.splQuery.Panel2.SuspendLayout();
			this.splQuery.SuspendLayout();
			this.pnlHelper.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgResults)).BeginInit();
			this.cmnQueryResultsCommands.SuspendLayout();
			this.SuspendLayout();
			// 
			// splQuery
			// 
			this.splQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splQuery.Location = new System.Drawing.Point(0, 0);
			this.splQuery.Name = "splQuery";
			this.splQuery.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splQuery.Panel2
			// 
			this.splQuery.Panel2.Controls.Add(this.pnlHelper);
			this.splQuery.Size = new System.Drawing.Size(450, 508);
			this.splQuery.SplitterDistance = 150;
			this.splQuery.SplitterWidth = 8;
			this.splQuery.TabIndex = 0;
			// 
			// pnlHelper
			// 
			this.pnlHelper.Controls.Add(this.dgResults);
			this.pnlHelper.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlHelper.Location = new System.Drawing.Point(0, 0);
			this.pnlHelper.Name = "pnlHelper";
			this.pnlHelper.Size = new System.Drawing.Size(450, 350);
			this.pnlHelper.TabIndex = 0;
			// 
			// dgResults
			// 
			this.dgResults.AllowUserToAddRows = false;
			this.dgResults.AllowUserToDeleteRows = false;
			this.dgResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgResults.Location = new System.Drawing.Point(0, 0);
			this.dgResults.Name = "dgResults";
			this.dgResults.ReadOnly = true;
			this.dgResults.RowHeadersVisible = false;
			this.dgResults.Size = new System.Drawing.Size(450, 350);
			this.dgResults.TabIndex = 0;
			this.dgResults.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgResults_CellMouseClick);
			this.dgResults.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgResults_CellMouseDown);
			this.dgResults.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgResults_DataError);
			this.dgResults.SelectionChanged += new System.EventHandler(this.dgResults_SelectionChanged);
			this.dgResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgResults_KeyDown);
			this.dgResults.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgResults_PreviewKeyDown);
			// 
			// cmnQueryResultsCommands
			// 
			this.cmnQueryResultsCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mniCreateRowUpdateQuery,
            this.mniCreateRowDeleteQuery});
			this.cmnQueryResultsCommands.Name = "cmnQueryResultsCommands";
			this.cmnQueryResultsCommands.Size = new System.Drawing.Size(189, 70);
			// 
			// mniCreateRowUpdateQuery
			// 
			this.mniCreateRowUpdateQuery.Image = global::SqlQueryTool.Properties.Resources.table_edit;
			this.mniCreateRowUpdateQuery.Name = "mniCreateRowUpdateQuery";
			this.mniCreateRowUpdateQuery.Size = new System.Drawing.Size(188, 22);
			this.mniCreateRowUpdateQuery.Text = "Create UPDATE query";
			this.mniCreateRowUpdateQuery.Click += new System.EventHandler(this.mniCreateRowUpdateQuery_Click);
			// 
			// mniCreateRowDeleteQuery
			// 
			this.mniCreateRowDeleteQuery.Image = global::SqlQueryTool.Properties.Resources.table_row_delete;
			this.mniCreateRowDeleteQuery.Name = "mniCreateRowDeleteQuery";
			this.mniCreateRowDeleteQuery.Size = new System.Drawing.Size(188, 22);
			this.mniCreateRowDeleteQuery.Text = "Create DELETE query";
			this.mniCreateRowDeleteQuery.Click += new System.EventHandler(this.mniCreateRowDeleteQuery_Click);
			// 
			// QueryEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splQuery);
			this.Name = "QueryEditor";
			this.Size = new System.Drawing.Size(450, 508);
			this.splQuery.Panel2.ResumeLayout(false);
			this.splQuery.ResumeLayout(false);
			this.pnlHelper.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgResults)).EndInit();
			this.cmnQueryResultsCommands.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splQuery;
		private System.Windows.Forms.Panel pnlHelper;
		private System.Windows.Forms.DataGridView dgResults;
		private System.Windows.Forms.ContextMenuStrip cmnQueryResultsCommands;
		private System.Windows.Forms.ToolStripMenuItem mniCreateRowUpdateQuery;
		private System.Windows.Forms.ToolStripMenuItem mniCreateRowDeleteQuery;

	}
}
