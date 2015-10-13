namespace PSCBioVerificationFace.Common
{
	partial class HorizontalZoomSlider
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.labelZoom = new System.Windows.Forms.Label();
            this.numUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // trackZoom
            // 
            this.trackZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackZoom.Location = new System.Drawing.Point(112, 5);
            this.trackZoom.Maximum = 200;
            this.trackZoom.Minimum = 10;
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.Size = new System.Drawing.Size(143, 45);
            this.trackZoom.TabIndex = 0;
            this.trackZoom.TickFrequency = 5;
            this.trackZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackZoom.Value = 100;
            this.trackZoom.ValueChanged += new System.EventHandler(this.tbZoom_ValueChanged);
            // 
            // labelZoom
            // 
            this.labelZoom.AutoSize = true;
            this.labelZoom.Location = new System.Drawing.Point(3, 6);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.labelZoom.Size = new System.Drawing.Size(34, 18);
            this.labelZoom.TabIndex = 1;
            this.labelZoom.Text = "Zoom";
            // 
            // numUpDown
            // 
            this.numUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numUpDown.Location = new System.Drawing.Point(261, 5);
            this.numUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numUpDown.Name = "numUpDown";
            this.numUpDown.Size = new System.Drawing.Size(52, 20);
            this.numUpDown.TabIndex = 3;
            this.numUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numUpDown.ValueChanged += new System.EventHandler(this.numUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(301, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "%";
            // 
            // HorizontalZoomSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.numUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelZoom);
            this.Controls.Add(this.trackZoom);
            this.Name = "HorizontalZoomSlider";
            this.Size = new System.Drawing.Size(316, 29);
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.TrackBar trackZoom;
		protected System.Windows.Forms.Label labelZoom;
		protected System.Windows.Forms.NumericUpDown numUpDown;
		protected System.Windows.Forms.Label label1;

	}
}
