namespace BiometricsTest
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.textBoxChunk = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarLabel = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "FillCache";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(28, 73);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Identify";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox.Location = new System.Drawing.Point(139, 12);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.ReadOnly = true;
            this.richTextBox.Size = new System.Drawing.Size(236, 361);
            this.richTextBox.TabIndex = 3;
            this.richTextBox.TabStop = false;
            this.richTextBox.Text = "";
            this.richTextBox.WordWrap = false;
            // 
            // textBoxChunk
            // 
            this.textBoxChunk.Location = new System.Drawing.Point(28, 142);
            this.textBoxChunk.Name = "textBoxChunk";
            this.textBoxChunk.Size = new System.Drawing.Size(75, 20);
            this.textBoxChunk.TabIndex = 4;
            this.textBoxChunk.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Number of chunks";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "1 chunk = 10 WSQ images";
            // 
            // trackBarLabel
            // 
            this.trackBarLabel.AutoSize = true;
            this.trackBarLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.trackBarLabel.Location = new System.Drawing.Point(112, 211);
            this.trackBarLabel.Name = "trackBarLabel";
            this.trackBarLabel.Size = new System.Drawing.Size(13, 13);
            this.trackBarLabel.TabIndex = 68;
            this.trackBarLabel.Text = "0";
            this.trackBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackBar1
            // 
            this.trackBar1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.trackBar1.LargeChange = 10;
            this.trackBar1.Location = new System.Drawing.Point(15, 236);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 45);
            this.trackBar1.SmallChange = 5;
            this.trackBar1.TabIndex = 66;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 70;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(10, 211);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 13);
            this.label11.TabIndex = 67;
            this.label11.Text = "Matching Threshold:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(5, 277);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(132, 17);
            this.checkBox1.TabIndex = 69;
            this.checkBox1.Text = "Show matching results";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 385);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.trackBarLabel);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxChunk);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.TextBox textBoxChunk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label trackBarLabel;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

