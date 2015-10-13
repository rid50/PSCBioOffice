namespace Neurotec.Samples
{
	partial class SettingsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.btnDefaults = new System.Windows.Forms.Button();
			this.tvSettingsPageSelection = new System.Windows.Forms.TreeView();
			this.panelSettingsPage = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(296, 402);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(215, 402);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.Location = new System.Drawing.Point(377, 402);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 2;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnDefaults
			// 
			this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDefaults.Location = new System.Drawing.Point(103, 402);
			this.btnDefaults.Name = "btnDefaults";
			this.btnDefaults.Size = new System.Drawing.Size(106, 23);
			this.btnDefaults.TabIndex = 4;
			this.btnDefaults.Text = "Set default values";
			this.btnDefaults.UseVisualStyleBackColor = true;
			this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
			// 
			// tvSettingsPageSelection
			// 
			this.tvSettingsPageSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.tvSettingsPageSelection.HideSelection = false;
			this.tvSettingsPageSelection.Location = new System.Drawing.Point(0, 0);
			this.tvSettingsPageSelection.Name = "tvSettingsPageSelection";
			this.tvSettingsPageSelection.Size = new System.Drawing.Size(138, 396);
			this.tvSettingsPageSelection.TabIndex = 5;
			this.tvSettingsPageSelection.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSettingsPageSelection_AfterSelect);
			// 
			// panelSettingsPage
			// 
			this.panelSettingsPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.panelSettingsPage.Location = new System.Drawing.Point(145, 29);
			this.panelSettingsPage.Name = "panelSettingsPage";
			this.panelSettingsPage.Size = new System.Drawing.Size(319, 367);
			this.panelSettingsPage.TabIndex = 6;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.Location = new System.Drawing.Point(144, 9);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(40, 17);
			this.lblTitle.TabIndex = 7;
			this.lblTitle.Text = "Title";
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(464, 437);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.panelSettingsPage);
			this.Controls.Add(this.tvSettingsPageSelection);
			this.Controls.Add(this.btnDefaults);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(480, 420);
			this.Name = "SettingsForm";
			this.Text = "Settings";
			this.Shown += new System.EventHandler(this.SettingsForm_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.Button btnDefaults;
		private System.Windows.Forms.TreeView tvSettingsPageSelection;
		private System.Windows.Forms.Panel panelSettingsPage;
		private System.Windows.Forms.Label lblTitle;
	}
}
