namespace PassportReaderNS
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
            
            _sc.prDisconnect();
            _sc.fpsDisconnect();
            
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelError = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.PictureBox4 = new System.Windows.Forms.PictureBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttonThumbs = new System.Windows.Forms.Button();
            this.buttonRightHand = new System.Windows.Forms.Button();
            this.buttonLeftHand = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fpPictureBox11 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox10 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox8 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox7 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox6 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox5 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox1 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox2 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox3 = new System.Windows.Forms.PictureBox();
            this.fpPictureBox4 = new System.Windows.Forms.PictureBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button5 = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.TextBoxID = new System.Windows.Forms.TextBox();
            this.buttonReadFingers = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox4)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 593);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(843, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelError
            // 
            this.toolStripStatusLabelError.Name = "toolStripStatusLabelError";
            this.toolStripStatusLabelError.Size = new System.Drawing.Size(726, 17);
            this.toolStripStatusLabelError.Spring = true;
            this.toolStripStatusLabelError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(298, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 100);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.HideSelection = false;
            this.textBox1.Location = new System.Drawing.Point(6, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(259, 349);
            this.textBox1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 558);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 23);
            this.button1.TabIndex = 7;
            this.button1.Tag = "";
            this.button1.Text = "Read barcode/MRZ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(819, 527);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(811, 501);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "BRC";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Location = new System.Drawing.Point(410, 359);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(388, 126);
            this.textBox3.TabIndex = 11;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(298, 403);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Get Party By GTIN";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(299, 359);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Get Item By GTIN";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 385);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Info";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(811, 501);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "MRZ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(19, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 385);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.HideSelection = false;
            this.textBox2.Location = new System.Drawing.Point(6, 19);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(259, 349);
            this.textBox2.TabIndex = 5;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(298, 16);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(500, 100);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.Panel1);
            this.tabPage3.Controls.Add(this.checkBox10);
            this.tabPage3.Controls.Add(this.checkBox11);
            this.tabPage3.Controls.Add(this.checkBox5);
            this.tabPage3.Controls.Add(this.checkBox6);
            this.tabPage3.Controls.Add(this.checkBox7);
            this.tabPage3.Controls.Add(this.checkBox8);
            this.tabPage3.Controls.Add(this.checkBox4);
            this.tabPage3.Controls.Add(this.checkBox3);
            this.tabPage3.Controls.Add(this.checkBox2);
            this.tabPage3.Controls.Add(this.checkBox1);
            this.tabPage3.Controls.Add(this.buttonThumbs);
            this.tabPage3.Controls.Add(this.buttonRightHand);
            this.tabPage3.Controls.Add(this.buttonLeftHand);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.fpPictureBox11);
            this.tabPage3.Controls.Add(this.fpPictureBox10);
            this.tabPage3.Controls.Add(this.fpPictureBox8);
            this.tabPage3.Controls.Add(this.fpPictureBox7);
            this.tabPage3.Controls.Add(this.fpPictureBox6);
            this.tabPage3.Controls.Add(this.fpPictureBox5);
            this.tabPage3.Controls.Add(this.fpPictureBox1);
            this.tabPage3.Controls.Add(this.fpPictureBox2);
            this.tabPage3.Controls.Add(this.fpPictureBox3);
            this.tabPage3.Controls.Add(this.fpPictureBox4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(811, 501);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Fingerprints";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Panel1
            // 
            this.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.Panel1.Controls.Add(this.PictureBox4);
            this.Panel1.Location = new System.Drawing.Point(3, 398);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(100, 100);
            this.Panel1.TabIndex = 77;
            this.Panel1.Visible = false;
            // 
            // PictureBox4
            // 
            this.PictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox4.Location = new System.Drawing.Point(0, 0);
            this.PictureBox4.Name = "PictureBox4";
            this.PictureBox4.Size = new System.Drawing.Size(100, 100);
            this.PictureBox4.TabIndex = 0;
            this.PictureBox4.TabStop = false;
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(274, 461);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(15, 14);
            this.checkBox10.TabIndex = 38;
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Location = new System.Drawing.Point(474, 461);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(15, 14);
            this.checkBox11.TabIndex = 37;
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(74, 319);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(15, 14);
            this.checkBox5.TabIndex = 36;
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(274, 319);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(15, 14);
            this.checkBox6.TabIndex = 35;
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(474, 319);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(15, 14);
            this.checkBox7.TabIndex = 34;
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(674, 319);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(15, 14);
            this.checkBox8.TabIndex = 33;
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(74, 174);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(15, 14);
            this.checkBox4.TabIndex = 32;
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(274, 174);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(15, 14);
            this.checkBox3.TabIndex = 31;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(474, 174);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 30;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(674, 174);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 29;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttonThumbs
            // 
            this.buttonThumbs.Location = new System.Drawing.Point(214, 375);
            this.buttonThumbs.Name = "buttonThumbs";
            this.buttonThumbs.Size = new System.Drawing.Size(52, 23);
            this.buttonThumbs.TabIndex = 28;
            this.buttonThumbs.Text = "Start";
            this.buttonThumbs.UseVisualStyleBackColor = true;
            this.buttonThumbs.Click += new System.EventHandler(this.buttonThumbs_Click);
            // 
            // buttonRightHand
            // 
            this.buttonRightHand.Location = new System.Drawing.Point(15, 233);
            this.buttonRightHand.Name = "buttonRightHand";
            this.buttonRightHand.Size = new System.Drawing.Size(52, 23);
            this.buttonRightHand.TabIndex = 27;
            this.buttonRightHand.Text = "Start";
            this.buttonRightHand.UseVisualStyleBackColor = true;
            this.buttonRightHand.Click += new System.EventHandler(this.buttonRightHand_Click);
            // 
            // buttonLeftHand
            // 
            this.buttonLeftHand.Location = new System.Drawing.Point(15, 88);
            this.buttonLeftHand.Name = "buttonLeftHand";
            this.buttonLeftHand.Size = new System.Drawing.Size(52, 23);
            this.buttonLeftHand.TabIndex = 26;
            this.buttonLeftHand.Text = "Start";
            this.buttonLeftHand.UseVisualStyleBackColor = true;
            this.buttonLeftHand.Click += new System.EventHandler(this.buttonLeftHand_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(402, 346);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 26);
            this.label14.TabIndex = 25;
            this.label14.Text = "Thumbs";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(389, 204);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 26);
            this.label13.TabIndex = 24;
            this.label13.Text = "Right hand";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(396, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 26);
            this.label10.TabIndex = 23;
            this.label10.Text = "Left hand";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(513, 359);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Right thumb";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(313, 359);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Left thumb";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(726, 217);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Little";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(523, 217);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Ring";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(323, 217);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Middle";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(126, 217);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Index";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(722, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Index";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(522, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Middle";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Ring";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Little";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(198, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 29);
            this.label1.TabIndex = 11;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fpPictureBox11
            // 
            this.fpPictureBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox11.Location = new System.Drawing.Point(495, 375);
            this.fpPictureBox11.Name = "fpPictureBox11";
            this.fpPictureBox11.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox11.TabIndex = 10;
            this.fpPictureBox11.TabStop = false;
            // 
            // fpPictureBox10
            // 
            this.fpPictureBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox10.Location = new System.Drawing.Point(295, 375);
            this.fpPictureBox10.Name = "fpPictureBox10";
            this.fpPictureBox10.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox10.TabIndex = 9;
            this.fpPictureBox10.TabStop = false;
            // 
            // fpPictureBox8
            // 
            this.fpPictureBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox8.Location = new System.Drawing.Point(695, 233);
            this.fpPictureBox8.Name = "fpPictureBox8";
            this.fpPictureBox8.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox8.TabIndex = 7;
            this.fpPictureBox8.TabStop = false;
            // 
            // fpPictureBox7
            // 
            this.fpPictureBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox7.Location = new System.Drawing.Point(495, 233);
            this.fpPictureBox7.Name = "fpPictureBox7";
            this.fpPictureBox7.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox7.TabIndex = 6;
            this.fpPictureBox7.TabStop = false;
            // 
            // fpPictureBox6
            // 
            this.fpPictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox6.Location = new System.Drawing.Point(295, 233);
            this.fpPictureBox6.Name = "fpPictureBox6";
            this.fpPictureBox6.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox6.TabIndex = 5;
            this.fpPictureBox6.TabStop = false;
            // 
            // fpPictureBox5
            // 
            this.fpPictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox5.Location = new System.Drawing.Point(95, 233);
            this.fpPictureBox5.Name = "fpPictureBox5";
            this.fpPictureBox5.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox5.TabIndex = 4;
            this.fpPictureBox5.TabStop = false;
            // 
            // fpPictureBox1
            // 
            this.fpPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox1.Location = new System.Drawing.Point(695, 88);
            this.fpPictureBox1.Name = "fpPictureBox1";
            this.fpPictureBox1.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox1.TabIndex = 3;
            this.fpPictureBox1.TabStop = false;
            // 
            // fpPictureBox2
            // 
            this.fpPictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox2.Location = new System.Drawing.Point(495, 88);
            this.fpPictureBox2.Name = "fpPictureBox2";
            this.fpPictureBox2.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox2.TabIndex = 2;
            this.fpPictureBox2.TabStop = false;
            // 
            // fpPictureBox3
            // 
            this.fpPictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox3.Location = new System.Drawing.Point(295, 88);
            this.fpPictureBox3.Name = "fpPictureBox3";
            this.fpPictureBox3.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox3.TabIndex = 1;
            this.fpPictureBox3.TabStop = false;
            // 
            // fpPictureBox4
            // 
            this.fpPictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpPictureBox4.Location = new System.Drawing.Point(95, 88);
            this.fpPictureBox4.Name = "fpPictureBox4";
            this.fpPictureBox4.Size = new System.Drawing.Size(100, 100);
            this.fpPictureBox4.TabIndex = 0;
            this.fpPictureBox4.TabStop = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button5);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.textBox5);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.pictureBox3);
            this.tabPage4.Controls.Add(this.textBox4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(811, 501);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Generate Barcode Image";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(20, 150);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "Print";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(66, 94);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 13);
            this.label16.TabIndex = 4;
            this.label16.Text = "points";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(23, 91);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(37, 20);
            this.textBox5.TabIndex = 3;
            this.textBox5.Text = "24";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(20, 18);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(97, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Enter barcode text:";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(206, 42);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(571, 207);
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(20, 42);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(158, 20);
            this.textBox4.TabIndex = 0;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(253, 559);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(133, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "Scan fingers";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // TextBoxID
            // 
            this.TextBoxID.Location = new System.Drawing.Point(698, 559);
            this.TextBoxID.Name = "TextBoxID";
            this.TextBoxID.Size = new System.Drawing.Size(124, 20);
            this.TextBoxID.TabIndex = 18;
            // 
            // buttonReadFingers
            // 
            this.buttonReadFingers.Location = new System.Drawing.Point(502, 558);
            this.buttonReadFingers.Name = "buttonReadFingers";
            this.buttonReadFingers.Size = new System.Drawing.Size(133, 23);
            this.buttonReadFingers.TabIndex = 17;
            this.buttonReadFingers.Text = "Read fingers";
            this.buttonReadFingers.UseVisualStyleBackColor = true;
            this.buttonReadFingers.Click += new System.EventHandler(this.buttonReadFingers_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(673, 562);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(19, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "Id:";
            // 
            // Form1
            // 
            this.AcceptButton = this.buttonReadFingers;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 615);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.TextBoxID);
            this.Controls.Add(this.buttonReadFingers);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Passport Reader & Scanner App";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox4)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelError;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox fpPictureBox4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox fpPictureBox1;
        private System.Windows.Forms.PictureBox fpPictureBox2;
        private System.Windows.Forms.PictureBox fpPictureBox3;
        private System.Windows.Forms.PictureBox fpPictureBox11;
        private System.Windows.Forms.PictureBox fpPictureBox10;
        private System.Windows.Forms.PictureBox fpPictureBox8;
        private System.Windows.Forms.PictureBox fpPictureBox7;
        private System.Windows.Forms.PictureBox fpPictureBox6;
        private System.Windows.Forms.PictureBox fpPictureBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button buttonThumbs;
        private System.Windows.Forms.Button buttonRightHand;
        private System.Windows.Forms.Button buttonLeftHand;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button button5;
        internal System.Windows.Forms.TextBox TextBoxID;
        private System.Windows.Forms.Button buttonReadFingers;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.PictureBox PictureBox4;
        private System.Windows.Forms.Label label17;
    }
}

