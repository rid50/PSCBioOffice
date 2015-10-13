namespace PSCBioVerificationFace.Common
{
	partial class VerticalZoomSlider
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
			((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// trackZoom
			// 
			this.trackZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.trackZoom.Location = new System.Drawing.Point(0, 21);
			this.trackZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.trackZoom.Size = new System.Drawing.Size(45, 109);
			// 
			// numUpDown
			// 
			this.numUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.numUpDown.Location = new System.Drawing.Point(2, 136);
			this.numUpDown.Size = new System.Drawing.Size(43, 20);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(182, 4);
			// 
			// VerticalZoomSlider
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.Name = "VerticalZoomSlider";
			this.Size = new System.Drawing.Size(47, 159);
			((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
	}
}
