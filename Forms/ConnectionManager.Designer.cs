namespace SqlQueryTool.Forms
{
	partial class ConnectionManager
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
			this.selPreviousConnections = new System.Windows.Forms.ComboBox();
			this.btnDeleteSelectedConnection = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnAddConnection = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// selPreviousConnections
			// 
			this.selPreviousConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.selPreviousConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.selPreviousConnections.FormattingEnabled = true;
			this.selPreviousConnections.Location = new System.Drawing.Point(3, 3);
			this.selPreviousConnections.Name = "selPreviousConnections";
			this.selPreviousConnections.Size = new System.Drawing.Size(218, 21);
			this.selPreviousConnections.TabIndex = 0;
			this.selPreviousConnections.SelectedIndexChanged += new System.EventHandler(this.selPreviousConnections_SelectedIndexChanged);
			// 
			// btnDeleteSelectedConnection
			// 
			this.btnDeleteSelectedConnection.Image = global::SqlQueryTool.Properties.Resources.database_delete;
			this.btnDeleteSelectedConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDeleteSelectedConnection.Location = new System.Drawing.Point(66, 30);
			this.btnDeleteSelectedConnection.Name = "btnDeleteSelectedConnection";
			this.btnDeleteSelectedConnection.Size = new System.Drawing.Size(69, 25);
			this.btnDeleteSelectedConnection.TabIndex = 2;
			this.btnDeleteSelectedConnection.Text = "Kustuta";
			this.btnDeleteSelectedConnection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDeleteSelectedConnection.UseVisualStyleBackColor = true;
			this.btnDeleteSelectedConnection.Click += new System.EventHandler(this.btnDeleteSelectedConnection_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.Image = global::SqlQueryTool.Properties.Resources.database_go;
			this.btnConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnConnect.Location = new System.Drawing.Point(150, 31);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(71, 25);
			this.btnConnect.TabIndex = 3;
			this.btnConnect.Text = "Ühenda";
			this.btnConnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnAddConnection
			// 
			this.btnAddConnection.Image = global::SqlQueryTool.Properties.Resources.database_add;
			this.btnAddConnection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnAddConnection.Location = new System.Drawing.Point(3, 30);
			this.btnAddConnection.Name = "btnAddConnection";
			this.btnAddConnection.Size = new System.Drawing.Size(57, 25);
			this.btnAddConnection.TabIndex = 1;
			this.btnAddConnection.Text = "Uus…";
			this.btnAddConnection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAddConnection.UseVisualStyleBackColor = true;
			this.btnAddConnection.Click += new System.EventHandler(this.btnAddConnection_Click);
			// 
			// ConnectionManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.btnDeleteSelectedConnection);
			this.Controls.Add(this.btnAddConnection);
			this.Controls.Add(this.selPreviousConnections);
			this.Name = "ConnectionManager";
			this.Size = new System.Drawing.Size(224, 61);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox selPreviousConnections;
		private System.Windows.Forms.Button btnAddConnection;
		private System.Windows.Forms.Button btnDeleteSelectedConnection;
		private System.Windows.Forms.Button btnConnect;
	}
}
