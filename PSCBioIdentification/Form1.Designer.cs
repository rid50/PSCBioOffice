namespace PSCBioIdentification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.nfViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.fingerView1 = new Neurotec.Biometrics.Gui.NFingerView();
            this.fingerView2 = new Neurotec.Biometrics.Gui.NFingerView();
            this.gbMainLog = new System.Windows.Forms.GroupBox();
            this.lblWaitingForImg = new System.Windows.Forms.Label();
            this.rtbMain = new System.Windows.Forms.RichTextBox();
            this.personId = new System.Windows.Forms.TextBox();
            this.lblPersonId = new System.Windows.Forms.Label();
            this.gbResults = new System.Windows.Forms.GroupBox();
            this.radioButtonManAndWoman = new System.Windows.Forms.RadioButton();
            this.radioButtonWoman = new System.Windows.Forms.RadioButton();
            this.radioButtonMan = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonRequest = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblLeft = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblThumbs = new System.Windows.Forms.Label();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.lblRight = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBoxMode = new System.Windows.Forms.GroupBox();
            this.radioButtonIdentify = new System.Windows.Forms.RadioButton();
            this.radioButtonVerify = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelError = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbFinger10 = new System.Windows.Forms.Label();
            this.lbFinger9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbFinger8 = new System.Windows.Forms.Label();
            this.lbFinger7 = new System.Windows.Forms.Label();
            this.lbFinger6 = new System.Windows.Forms.Label();
            this.lbFinger5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFinger1 = new System.Windows.Forms.Label();
            this.lbFinger2 = new System.Windows.Forms.Label();
            this.lbFinger3 = new System.Windows.Forms.Label();
            this.lbFinger4 = new System.Windows.Forms.Label();
            this.scannersListBox = new System.Windows.Forms.ListBox();
            this.manageCacheButton = new System.Windows.Forms.Button();
            this.checkBoxCache4 = new System.Windows.Forms.CheckBox();
            this.labCache4 = new System.Windows.Forms.Label();
            this.labCache3 = new System.Windows.Forms.Label();
            this.checkBoxCache3 = new System.Windows.Forms.CheckBox();
            this.labCache2 = new System.Windows.Forms.Label();
            this.checkBoxCache2 = new System.Windows.Forms.CheckBox();
            this.labCache1 = new System.Windows.Forms.Label();
            this.checkBoxCache1 = new System.Windows.Forms.CheckBox();
            this.labCache8 = new System.Windows.Forms.Label();
            this.checkBoxCache8 = new System.Windows.Forms.CheckBox();
            this.labCache7 = new System.Windows.Forms.Label();
            this.checkBoxCache7 = new System.Windows.Forms.CheckBox();
            this.labCache6 = new System.Windows.Forms.Label();
            this.checkBoxCache6 = new System.Windows.Forms.CheckBox();
            this.labCache5 = new System.Windows.Forms.Label();
            this.checkBoxCache5 = new System.Windows.Forms.CheckBox();
            this.labCache10 = new System.Windows.Forms.Label();
            this.checkBoxCache10 = new System.Windows.Forms.CheckBox();
            this.labCache9 = new System.Windows.Forms.Label();
            this.checkBoxCache9 = new System.Windows.Forms.CheckBox();
            this.labelCacheUnavailable = new System.Windows.Forms.Label();
            this.labelCacheValidationTime = new System.Windows.Forms.Label();
            this.buttonRefreshScannerListBox = new System.Windows.Forms.Button();
            this.pictureBoxCompanyLogo = new System.Windows.Forms.PictureBox();
            this.pictureBoxRed = new PSCBioIdentification.MyPictureBox();
            this.pictureBoxGreen = new PSCBioIdentification.MyPictureBox();
            this.pictureBoxPhoto = new System.Windows.Forms.PictureBox();
            this.pictureBoxCheckMark = new System.Windows.Forms.PictureBox();
            this.fpPictureBox9 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox10 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox6 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox5 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox7 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox8 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox3 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox1 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox2 = new PSCBioIdentification.MyPictureBox();
            this.fpPictureBox4 = new PSCBioIdentification.MyPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nfViewSplitContainer)).BeginInit();
            this.nfViewSplitContainer.Panel1.SuspendLayout();
            this.nfViewSplitContainer.Panel2.SuspendLayout();
            this.nfViewSplitContainer.SuspendLayout();
            this.gbMainLog.SuspendLayout();
            this.gbResults.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBoxMode.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompanyLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheckMark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // nfViewSplitContainer
            // 
            this.nfViewSplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.nfViewSplitContainer, "nfViewSplitContainer");
            this.nfViewSplitContainer.Name = "nfViewSplitContainer";
            // 
            // nfViewSplitContainer.Panel1
            // 
            resources.ApplyResources(this.nfViewSplitContainer.Panel1, "nfViewSplitContainer.Panel1");
            this.nfViewSplitContainer.Panel1.Controls.Add(this.fingerView1);
            // 
            // nfViewSplitContainer.Panel2
            // 
            resources.ApplyResources(this.nfViewSplitContainer.Panel2, "nfViewSplitContainer.Panel2");
            this.nfViewSplitContainer.Panel2.Controls.Add(this.fingerView2);
            this.nfViewSplitContainer.TabStop = false;
            // 
            // fingerView1
            // 
            this.fingerView1.BackColor = System.Drawing.SystemColors.Control;
            this.fingerView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.fingerView1.BoundingRectColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.fingerView1, "fingerView1");
            this.fingerView1.MinutiaColor = System.Drawing.Color.Red;
            this.fingerView1.Name = "fingerView1";
            this.fingerView1.NeighborMinutiaColor = System.Drawing.Color.Orange;
            this.fingerView1.ResultImageColor = System.Drawing.Color.Lime;
            this.fingerView1.SelectedMinutiaColor = System.Drawing.Color.Magenta;
            this.fingerView1.SelectedSingularPointColor = System.Drawing.Color.Magenta;
            this.fingerView1.SingularPointColor = System.Drawing.Color.Red;
            this.fingerView1.TreeColor = System.Drawing.Color.Crimson;
            this.fingerView1.TreeMinutiaNumberDiplayFormat = Neurotec.Biometrics.Gui.MinutiaNumberDiplayFormat.DontDisplay;
            this.fingerView1.TreeMinutiaNumberFont = new System.Drawing.Font("Arial", 10F);
            this.fingerView1.TreeWidth = 2D;
            // 
            // fingerView2
            // 
            this.fingerView2.BackColor = System.Drawing.SystemColors.Control;
            this.fingerView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.fingerView2.BoundingRectColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.fingerView2, "fingerView2");
            this.fingerView2.MinutiaColor = System.Drawing.Color.Red;
            this.fingerView2.Name = "fingerView2";
            this.fingerView2.NeighborMinutiaColor = System.Drawing.Color.Orange;
            this.fingerView2.ResultImageColor = System.Drawing.Color.Lime;
            this.fingerView2.SelectedMinutiaColor = System.Drawing.Color.Magenta;
            this.fingerView2.SelectedSingularPointColor = System.Drawing.Color.Magenta;
            this.fingerView2.ShownImage = Neurotec.Biometrics.Gui.ShownImage.Result;
            this.fingerView2.SingularPointColor = System.Drawing.Color.Red;
            this.fingerView2.TreeColor = System.Drawing.Color.Crimson;
            this.fingerView2.TreeMinutiaNumberDiplayFormat = Neurotec.Biometrics.Gui.MinutiaNumberDiplayFormat.DontDisplay;
            this.fingerView2.TreeMinutiaNumberFont = new System.Drawing.Font("Arial", 10F);
            this.fingerView2.TreeWidth = 2D;
            this.fingerView2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FingerViewMouseClick);
            // 
            // gbMainLog
            // 
            resources.ApplyResources(this.gbMainLog, "gbMainLog");
            this.gbMainLog.Controls.Add(this.lblWaitingForImg);
            this.gbMainLog.Controls.Add(this.rtbMain);
            this.gbMainLog.Name = "gbMainLog";
            this.gbMainLog.TabStop = false;
            // 
            // lblWaitingForImg
            // 
            resources.ApplyResources(this.lblWaitingForImg, "lblWaitingForImg");
            this.lblWaitingForImg.Name = "lblWaitingForImg";
            // 
            // rtbMain
            // 
            resources.ApplyResources(this.rtbMain, "rtbMain");
            this.rtbMain.BackColor = System.Drawing.SystemColors.Window;
            this.rtbMain.Name = "rtbMain";
            this.rtbMain.ReadOnly = true;
            this.rtbMain.TabStop = false;
            // 
            // personId
            // 
            resources.ApplyResources(this.personId, "personId");
            this.personId.Name = "personId";
            // 
            // lblPersonId
            // 
            resources.ApplyResources(this.lblPersonId, "lblPersonId");
            this.lblPersonId.Name = "lblPersonId";
            // 
            // gbResults
            // 
            this.gbResults.Controls.Add(this.radioButtonManAndWoman);
            this.gbResults.Controls.Add(this.radioButtonWoman);
            this.gbResults.Controls.Add(this.radioButtonMan);
            this.gbResults.Controls.Add(this.panel3);
            this.gbResults.Controls.Add(this.panel2);
            this.gbResults.Controls.Add(this.groupBox4);
            this.gbResults.Controls.Add(this.gbMainLog);
            this.gbResults.Controls.Add(this.pictureBoxPhoto);
            this.gbResults.Controls.Add(this.pictureBoxCheckMark);
            this.gbResults.Controls.Add(this.groupBoxMode);
            this.gbResults.Controls.Add(this.lblPersonId);
            this.gbResults.Controls.Add(this.personId);
            resources.ApplyResources(this.gbResults, "gbResults");
            this.gbResults.Name = "gbResults";
            this.gbResults.TabStop = false;
            // 
            // radioButtonManAndWoman
            // 
            resources.ApplyResources(this.radioButtonManAndWoman, "radioButtonManAndWoman");
            this.radioButtonManAndWoman.Name = "radioButtonManAndWoman";
            this.radioButtonManAndWoman.TabStop = true;
            this.radioButtonManAndWoman.Tag = "ll";
            this.radioButtonManAndWoman.UseVisualStyleBackColor = true;
            // 
            // radioButtonWoman
            // 
            resources.ApplyResources(this.radioButtonWoman, "radioButtonWoman");
            this.radioButtonWoman.Name = "radioButtonWoman";
            this.radioButtonWoman.TabStop = true;
            this.radioButtonWoman.Tag = "ll";
            this.radioButtonWoman.UseVisualStyleBackColor = true;
            // 
            // radioButtonMan
            // 
            resources.ApplyResources(this.radioButtonMan, "radioButtonMan");
            this.radioButtonMan.Checked = true;
            this.radioButtonMan.Name = "radioButtonMan";
            this.radioButtonMan.TabStop = true;
            this.radioButtonMan.Tag = "ll";
            this.radioButtonMan.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonScan);
            this.panel3.Controls.Add(this.buttonRequest);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // buttonScan
            // 
            resources.ApplyResources(this.buttonScan, "buttonScan");
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonRequest
            // 
            resources.ApplyResources(this.buttonRequest, "buttonRequest");
            this.buttonRequest.Name = "buttonRequest";
            this.buttonRequest.UseVisualStyleBackColor = true;
            this.buttonRequest.Click += new System.EventHandler(this.buttonRequest_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblLeft);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.checkBox10);
            this.panel2.Controls.Add(this.radioButton4);
            this.panel2.Controls.Add(this.checkBox9);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.checkBox8);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Controls.Add(this.checkBox7);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.checkBox6);
            this.panel2.Controls.Add(this.checkBox5);
            this.panel2.Controls.Add(this.radioButton2);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Controls.Add(this.checkBox3);
            this.panel2.Controls.Add(this.radioButton7);
            this.panel2.Controls.Add(this.checkBox4);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.radioButton5);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.radioButton6);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.lblThumbs);
            this.panel2.Controls.Add(this.radioButton8);
            this.panel2.Controls.Add(this.radioButton10);
            this.panel2.Controls.Add(this.lblRight);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.radioButton9);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lblLeft
            // 
            resources.ApplyResources(this.lblLeft, "lblLeft");
            this.lblLeft.Name = "lblLeft";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // checkBox10
            // 
            resources.ApplyResources(this.checkBox10, "checkBox10");
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Tag = "rt";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.TabStop = true;
            this.radioButton4.Tag = "ll";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            resources.ApplyResources(this.checkBox9, "checkBox9");
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Tag = "lt";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // checkBox8
            // 
            resources.ApplyResources(this.checkBox8, "checkBox8");
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Tag = "rl";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.TabStop = true;
            this.radioButton3.Tag = "lr";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            resources.ApplyResources(this.checkBox7, "checkBox7");
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Tag = "rr";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // checkBox6
            // 
            resources.ApplyResources(this.checkBox6, "checkBox6");
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Tag = "rm";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Tag = "ri";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabStop = true;
            this.radioButton2.Tag = "lm";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Tag = "li";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Tag = "lm";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.Tag = "li";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Tag = "lr";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // radioButton7
            // 
            resources.ApplyResources(this.radioButton7, "radioButton7");
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.TabStop = true;
            this.radioButton7.Tag = "rr";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Tag = "ll";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // radioButton5
            // 
            resources.ApplyResources(this.radioButton5, "radioButton5");
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.TabStop = true;
            this.radioButton5.Tag = "ri";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // radioButton6
            // 
            resources.ApplyResources(this.radioButton6, "radioButton6");
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.TabStop = true;
            this.radioButton6.Tag = "rm";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // lblThumbs
            // 
            resources.ApplyResources(this.lblThumbs, "lblThumbs");
            this.lblThumbs.Name = "lblThumbs";
            // 
            // radioButton8
            // 
            resources.ApplyResources(this.radioButton8, "radioButton8");
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.TabStop = true;
            this.radioButton8.Tag = "rl";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            resources.ApplyResources(this.radioButton10, "radioButton10");
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.TabStop = true;
            this.radioButton10.Tag = "rt";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // lblRight
            // 
            resources.ApplyResources(this.lblRight, "lblRight");
            this.lblRight.Name = "lblRight";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // radioButton9
            // 
            resources.ApplyResources(this.radioButton9, "radioButton9");
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.TabStop = true;
            this.radioButton9.Tag = "lt";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pictureBoxRed);
            this.groupBox4.Controls.Add(this.pictureBoxGreen);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // groupBoxMode
            // 
            this.groupBoxMode.Controls.Add(this.radioButtonIdentify);
            this.groupBoxMode.Controls.Add(this.radioButtonVerify);
            resources.ApplyResources(this.groupBoxMode, "groupBoxMode");
            this.groupBoxMode.Name = "groupBoxMode";
            this.groupBoxMode.TabStop = false;
            // 
            // radioButtonIdentify
            // 
            resources.ApplyResources(this.radioButtonIdentify, "radioButtonIdentify");
            this.radioButtonIdentify.Name = "radioButtonIdentify";
            this.radioButtonIdentify.TabStop = true;
            this.radioButtonIdentify.UseVisualStyleBackColor = true;
            this.radioButtonIdentify.CheckedChanged += new System.EventHandler(this.radioButtonGroup_CheckedChanged);
            // 
            // radioButtonVerify
            // 
            resources.ApplyResources(this.radioButtonVerify, "radioButtonVerify");
            this.radioButtonVerify.Name = "radioButtonVerify";
            this.radioButtonVerify.UseVisualStyleBackColor = true;
            this.radioButtonVerify.CheckedChanged += new System.EventHandler(this.radioButtonGroup_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelError,
            this.toolStripProgressBar});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabelError
            // 
            this.toolStripStatusLabelError.Name = "toolStripStatusLabelError";
            resources.ApplyResources(this.toolStripStatusLabelError, "toolStripStatusLabelError");
            this.toolStripStatusLabelError.Spring = true;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            resources.ApplyResources(this.toolStripProgressBar, "toolStripProgressBar");
            this.toolStripProgressBar.Step = 1;
            this.toolStripProgressBar.Value = 1;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Name = "panel1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbFinger10);
            this.groupBox3.Controls.Add(this.lbFinger9);
            this.groupBox3.Controls.Add(this.fpPictureBox9);
            this.groupBox3.Controls.Add(this.fpPictureBox10);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // lbFinger10
            // 
            resources.ApplyResources(this.lbFinger10, "lbFinger10");
            this.lbFinger10.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger10.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger10.Name = "lbFinger10";
            // 
            // lbFinger9
            // 
            resources.ApplyResources(this.lbFinger9, "lbFinger9");
            this.lbFinger9.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger9.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger9.Name = "lbFinger9";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbFinger8);
            this.groupBox2.Controls.Add(this.lbFinger7);
            this.groupBox2.Controls.Add(this.lbFinger6);
            this.groupBox2.Controls.Add(this.lbFinger5);
            this.groupBox2.Controls.Add(this.fpPictureBox6);
            this.groupBox2.Controls.Add(this.fpPictureBox5);
            this.groupBox2.Controls.Add(this.fpPictureBox7);
            this.groupBox2.Controls.Add(this.fpPictureBox8);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // lbFinger8
            // 
            resources.ApplyResources(this.lbFinger8, "lbFinger8");
            this.lbFinger8.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger8.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger8.Name = "lbFinger8";
            // 
            // lbFinger7
            // 
            resources.ApplyResources(this.lbFinger7, "lbFinger7");
            this.lbFinger7.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger7.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger7.Name = "lbFinger7";
            // 
            // lbFinger6
            // 
            resources.ApplyResources(this.lbFinger6, "lbFinger6");
            this.lbFinger6.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger6.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger6.Name = "lbFinger6";
            // 
            // lbFinger5
            // 
            resources.ApplyResources(this.lbFinger5, "lbFinger5");
            this.lbFinger5.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger5.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger5.Name = "lbFinger5";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbFinger1);
            this.groupBox1.Controls.Add(this.lbFinger2);
            this.groupBox1.Controls.Add(this.lbFinger3);
            this.groupBox1.Controls.Add(this.lbFinger4);
            this.groupBox1.Controls.Add(this.fpPictureBox3);
            this.groupBox1.Controls.Add(this.fpPictureBox1);
            this.groupBox1.Controls.Add(this.fpPictureBox2);
            this.groupBox1.Controls.Add(this.fpPictureBox4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lbFinger1
            // 
            resources.ApplyResources(this.lbFinger1, "lbFinger1");
            this.lbFinger1.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger1.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger1.Name = "lbFinger1";
            // 
            // lbFinger2
            // 
            resources.ApplyResources(this.lbFinger2, "lbFinger2");
            this.lbFinger2.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger2.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger2.Name = "lbFinger2";
            // 
            // lbFinger3
            // 
            resources.ApplyResources(this.lbFinger3, "lbFinger3");
            this.lbFinger3.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger3.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger3.Name = "lbFinger3";
            // 
            // lbFinger4
            // 
            resources.ApplyResources(this.lbFinger4, "lbFinger4");
            this.lbFinger4.BackColor = System.Drawing.Color.Transparent;
            this.lbFinger4.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbFinger4.Name = "lbFinger4";
            // 
            // scannersListBox
            // 
            this.scannersListBox.FormattingEnabled = true;
            resources.ApplyResources(this.scannersListBox, "scannersListBox");
            this.scannersListBox.Name = "scannersListBox";
            this.scannersListBox.SelectedIndexChanged += new System.EventHandler(this.ScannersListBoxSelectedIndexChanged);
            // 
            // manageCacheButton
            // 
            resources.ApplyResources(this.manageCacheButton, "manageCacheButton");
            this.manageCacheButton.Name = "manageCacheButton";
            this.manageCacheButton.UseVisualStyleBackColor = true;
            this.manageCacheButton.Click += new System.EventHandler(this.manageCacheButton_Click);
            // 
            // checkBoxCache4
            // 
            resources.ApplyResources(this.checkBoxCache4, "checkBoxCache4");
            this.checkBoxCache4.Name = "checkBoxCache4";
            this.checkBoxCache4.Tag = "ll";
            this.checkBoxCache4.UseVisualStyleBackColor = true;
            // 
            // labCache4
            // 
            resources.ApplyResources(this.labCache4, "labCache4");
            this.labCache4.BackColor = System.Drawing.Color.Transparent;
            this.labCache4.Name = "labCache4";
            // 
            // labCache3
            // 
            resources.ApplyResources(this.labCache3, "labCache3");
            this.labCache3.BackColor = System.Drawing.Color.Transparent;
            this.labCache3.Name = "labCache3";
            // 
            // checkBoxCache3
            // 
            resources.ApplyResources(this.checkBoxCache3, "checkBoxCache3");
            this.checkBoxCache3.Name = "checkBoxCache3";
            this.checkBoxCache3.Tag = "lr";
            this.checkBoxCache3.UseVisualStyleBackColor = true;
            // 
            // labCache2
            // 
            resources.ApplyResources(this.labCache2, "labCache2");
            this.labCache2.BackColor = System.Drawing.Color.Transparent;
            this.labCache2.Name = "labCache2";
            // 
            // checkBoxCache2
            // 
            resources.ApplyResources(this.checkBoxCache2, "checkBoxCache2");
            this.checkBoxCache2.Name = "checkBoxCache2";
            this.checkBoxCache2.Tag = "lm";
            this.checkBoxCache2.UseVisualStyleBackColor = true;
            // 
            // labCache1
            // 
            resources.ApplyResources(this.labCache1, "labCache1");
            this.labCache1.BackColor = System.Drawing.Color.Transparent;
            this.labCache1.Name = "labCache1";
            // 
            // checkBoxCache1
            // 
            resources.ApplyResources(this.checkBoxCache1, "checkBoxCache1");
            this.checkBoxCache1.Name = "checkBoxCache1";
            this.checkBoxCache1.Tag = "li";
            this.checkBoxCache1.UseVisualStyleBackColor = true;
            // 
            // labCache8
            // 
            resources.ApplyResources(this.labCache8, "labCache8");
            this.labCache8.BackColor = System.Drawing.Color.Transparent;
            this.labCache8.Name = "labCache8";
            // 
            // checkBoxCache8
            // 
            resources.ApplyResources(this.checkBoxCache8, "checkBoxCache8");
            this.checkBoxCache8.Name = "checkBoxCache8";
            this.checkBoxCache8.Tag = "rl";
            this.checkBoxCache8.UseVisualStyleBackColor = true;
            // 
            // labCache7
            // 
            resources.ApplyResources(this.labCache7, "labCache7");
            this.labCache7.BackColor = System.Drawing.Color.Transparent;
            this.labCache7.Name = "labCache7";
            // 
            // checkBoxCache7
            // 
            resources.ApplyResources(this.checkBoxCache7, "checkBoxCache7");
            this.checkBoxCache7.Name = "checkBoxCache7";
            this.checkBoxCache7.Tag = "rr";
            this.checkBoxCache7.UseVisualStyleBackColor = true;
            // 
            // labCache6
            // 
            resources.ApplyResources(this.labCache6, "labCache6");
            this.labCache6.BackColor = System.Drawing.Color.Transparent;
            this.labCache6.Name = "labCache6";
            // 
            // checkBoxCache6
            // 
            resources.ApplyResources(this.checkBoxCache6, "checkBoxCache6");
            this.checkBoxCache6.Name = "checkBoxCache6";
            this.checkBoxCache6.Tag = "rm";
            this.checkBoxCache6.UseVisualStyleBackColor = true;
            // 
            // labCache5
            // 
            resources.ApplyResources(this.labCache5, "labCache5");
            this.labCache5.BackColor = System.Drawing.Color.Transparent;
            this.labCache5.Name = "labCache5";
            // 
            // checkBoxCache5
            // 
            resources.ApplyResources(this.checkBoxCache5, "checkBoxCache5");
            this.checkBoxCache5.Name = "checkBoxCache5";
            this.checkBoxCache5.Tag = "ri";
            this.checkBoxCache5.UseVisualStyleBackColor = true;
            // 
            // labCache10
            // 
            resources.ApplyResources(this.labCache10, "labCache10");
            this.labCache10.BackColor = System.Drawing.Color.Transparent;
            this.labCache10.Name = "labCache10";
            // 
            // checkBoxCache10
            // 
            resources.ApplyResources(this.checkBoxCache10, "checkBoxCache10");
            this.checkBoxCache10.Name = "checkBoxCache10";
            this.checkBoxCache10.Tag = "rt";
            this.checkBoxCache10.UseVisualStyleBackColor = true;
            // 
            // labCache9
            // 
            resources.ApplyResources(this.labCache9, "labCache9");
            this.labCache9.BackColor = System.Drawing.Color.Transparent;
            this.labCache9.Name = "labCache9";
            // 
            // checkBoxCache9
            // 
            resources.ApplyResources(this.checkBoxCache9, "checkBoxCache9");
            this.checkBoxCache9.Name = "checkBoxCache9";
            this.checkBoxCache9.Tag = "lt";
            this.checkBoxCache9.UseVisualStyleBackColor = true;
            // 
            // labelCacheUnavailable
            // 
            resources.ApplyResources(this.labelCacheUnavailable, "labelCacheUnavailable");
            this.labelCacheUnavailable.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelCacheUnavailable.Name = "labelCacheUnavailable";
            // 
            // labelCacheValidationTime
            // 
            resources.ApplyResources(this.labelCacheValidationTime, "labelCacheValidationTime");
            this.labelCacheValidationTime.Name = "labelCacheValidationTime";
            // 
            // buttonRefreshScannerListBox
            // 
            this.buttonRefreshScannerListBox.BackColor = System.Drawing.Color.Transparent;
            this.buttonRefreshScannerListBox.Image = global::PSCBioIdentification.Properties.Resources.refresh32;
            resources.ApplyResources(this.buttonRefreshScannerListBox, "buttonRefreshScannerListBox");
            this.buttonRefreshScannerListBox.Name = "buttonRefreshScannerListBox";
            this.buttonRefreshScannerListBox.UseVisualStyleBackColor = false;
            this.buttonRefreshScannerListBox.Click += new System.EventHandler(this.buttonRefreshScannerListBox_Click);
            // 
            // pictureBoxCompanyLogo
            // 
            resources.ApplyResources(this.pictureBoxCompanyLogo, "pictureBoxCompanyLogo");
            this.pictureBoxCompanyLogo.Name = "pictureBoxCompanyLogo";
            this.pictureBoxCompanyLogo.TabStop = false;
            // 
            // pictureBoxRed
            // 
            this.pictureBoxRed.Active = false;
            this.pictureBoxRed.Image = global::PSCBioIdentification.Properties.Resources.red_light;
            resources.ApplyResources(this.pictureBoxRed, "pictureBoxRed");
            this.pictureBoxRed.Name = "pictureBoxRed";
            this.pictureBoxRed.TabStop = false;
            this.pictureBoxRed.Tag = "leftLittle";
            // 
            // pictureBoxGreen
            // 
            this.pictureBoxGreen.Active = false;
            this.pictureBoxGreen.Image = global::PSCBioIdentification.Properties.Resources.green_light;
            resources.ApplyResources(this.pictureBoxGreen, "pictureBoxGreen");
            this.pictureBoxGreen.Name = "pictureBoxGreen";
            this.pictureBoxGreen.TabStop = false;
            this.pictureBoxGreen.Tag = "leftLittle";
            // 
            // pictureBoxPhoto
            // 
            this.pictureBoxPhoto.BackgroundImage = global::PSCBioIdentification.Properties.Resources.p66;
            this.pictureBoxPhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBoxPhoto, "pictureBoxPhoto");
            this.pictureBoxPhoto.Name = "pictureBoxPhoto";
            this.pictureBoxPhoto.TabStop = false;
            // 
            // pictureBoxCheckMark
            // 
            resources.ApplyResources(this.pictureBoxCheckMark, "pictureBoxCheckMark");
            this.pictureBoxCheckMark.Name = "pictureBoxCheckMark";
            this.pictureBoxCheckMark.TabStop = false;
            // 
            // fpPictureBox9
            // 
            this.fpPictureBox9.Active = false;
            this.fpPictureBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox9, "fpPictureBox9");
            this.fpPictureBox9.Name = "fpPictureBox9";
            this.fpPictureBox9.TabStop = false;
            this.fpPictureBox9.Tag = "leftThumb";
            // 
            // fpPictureBox10
            // 
            this.fpPictureBox10.Active = false;
            this.fpPictureBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox10, "fpPictureBox10");
            this.fpPictureBox10.Name = "fpPictureBox10";
            this.fpPictureBox10.TabStop = false;
            this.fpPictureBox10.Tag = "rightThumb";
            // 
            // fpPictureBox6
            // 
            this.fpPictureBox6.Active = false;
            this.fpPictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox6, "fpPictureBox6");
            this.fpPictureBox6.Name = "fpPictureBox6";
            this.fpPictureBox6.TabStop = false;
            this.fpPictureBox6.Tag = "rightMiddle";
            // 
            // fpPictureBox5
            // 
            this.fpPictureBox5.Active = false;
            this.fpPictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox5, "fpPictureBox5");
            this.fpPictureBox5.Name = "fpPictureBox5";
            this.fpPictureBox5.TabStop = false;
            this.fpPictureBox5.Tag = "rightIndex";
            // 
            // fpPictureBox7
            // 
            this.fpPictureBox7.Active = false;
            this.fpPictureBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox7, "fpPictureBox7");
            this.fpPictureBox7.Name = "fpPictureBox7";
            this.fpPictureBox7.TabStop = false;
            this.fpPictureBox7.Tag = "rightRing";
            // 
            // fpPictureBox8
            // 
            this.fpPictureBox8.Active = false;
            this.fpPictureBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox8, "fpPictureBox8");
            this.fpPictureBox8.Name = "fpPictureBox8";
            this.fpPictureBox8.TabStop = false;
            this.fpPictureBox8.Tag = "rightLittle";
            // 
            // fpPictureBox3
            // 
            this.fpPictureBox3.Active = false;
            this.fpPictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.fpPictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox3, "fpPictureBox3");
            this.fpPictureBox3.Name = "fpPictureBox3";
            this.fpPictureBox3.TabStop = false;
            this.fpPictureBox3.Tag = "leftRing";
            // 
            // fpPictureBox1
            // 
            this.fpPictureBox1.Active = false;
            this.fpPictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox1, "fpPictureBox1");
            this.fpPictureBox1.Name = "fpPictureBox1";
            this.fpPictureBox1.TabStop = false;
            this.fpPictureBox1.Tag = "leftIndex";
            // 
            // fpPictureBox2
            // 
            this.fpPictureBox2.Active = false;
            this.fpPictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox2, "fpPictureBox2");
            this.fpPictureBox2.Name = "fpPictureBox2";
            this.fpPictureBox2.TabStop = false;
            this.fpPictureBox2.Tag = "leftMiddle";
            // 
            // fpPictureBox4
            // 
            this.fpPictureBox4.Active = false;
            this.fpPictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.fpPictureBox4, "fpPictureBox4");
            this.fpPictureBox4.Name = "fpPictureBox4";
            this.fpPictureBox4.TabStop = false;
            this.fpPictureBox4.Tag = "leftLittle";
            // 
            // Form1
            // 
            this.AcceptButton = this.buttonRequest;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkKhaki;
            this.Controls.Add(this.buttonRefreshScannerListBox);
            this.Controls.Add(this.labelCacheValidationTime);
            this.Controls.Add(this.labelCacheUnavailable);
            this.Controls.Add(this.labCache10);
            this.Controls.Add(this.checkBoxCache10);
            this.Controls.Add(this.labCache9);
            this.Controls.Add(this.checkBoxCache9);
            this.Controls.Add(this.labCache8);
            this.Controls.Add(this.checkBoxCache8);
            this.Controls.Add(this.labCache7);
            this.Controls.Add(this.checkBoxCache7);
            this.Controls.Add(this.labCache6);
            this.Controls.Add(this.checkBoxCache6);
            this.Controls.Add(this.labCache5);
            this.Controls.Add(this.checkBoxCache5);
            this.Controls.Add(this.labCache1);
            this.Controls.Add(this.checkBoxCache1);
            this.Controls.Add(this.labCache2);
            this.Controls.Add(this.checkBoxCache2);
            this.Controls.Add(this.labCache3);
            this.Controls.Add(this.checkBoxCache3);
            this.Controls.Add(this.labCache4);
            this.Controls.Add(this.checkBoxCache4);
            this.Controls.Add(this.manageCacheButton);
            this.Controls.Add(this.scannersListBox);
            this.Controls.Add(this.pictureBoxCompanyLogo);
            this.Controls.Add(this.gbResults);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.nfViewSplitContainer);
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.nfViewSplitContainer.Panel1.ResumeLayout(false);
            this.nfViewSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nfViewSplitContainer)).EndInit();
            this.nfViewSplitContainer.ResumeLayout(false);
            this.gbMainLog.ResumeLayout(false);
            this.gbMainLog.PerformLayout();
            this.gbResults.ResumeLayout(false);
            this.gbResults.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCompanyLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPhoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCheckMark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpPictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMainLog;
        private System.Windows.Forms.Label lblWaitingForImg;
        private System.Windows.Forms.RichTextBox rtbMain;
        private System.Windows.Forms.TextBox personId;
        private System.Windows.Forms.Label lblPersonId;
        private System.Windows.Forms.GroupBox gbResults;
        private System.Windows.Forms.GroupBox groupBoxMode;
        private System.Windows.Forms.RadioButton radioButtonIdentify;
        private System.Windows.Forms.RadioButton radioButtonVerify;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label lblThumbs;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Button buttonRequest;
        private System.Windows.Forms.PictureBox pictureBoxCheckMark;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelError;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.Panel panel1;
        private MyPictureBox fpPictureBox10;
        private MyPictureBox fpPictureBox9;
        private MyPictureBox fpPictureBox8;
        private MyPictureBox fpPictureBox7;
        private MyPictureBox fpPictureBox6;
        private MyPictureBox fpPictureBox5;
        private MyPictureBox fpPictureBox1;
        private MyPictureBox fpPictureBox2;
        private MyPictureBox fpPictureBox3;
        private MyPictureBox fpPictureBox4;
        private System.Windows.Forms.PictureBox pictureBoxPhoto;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBoxCompanyLogo;
        private System.Windows.Forms.Label lbFinger5;
        private System.Windows.Forms.Label lbFinger10;
        private System.Windows.Forms.Label lbFinger9;
        private System.Windows.Forms.Label lbFinger8;
        private System.Windows.Forms.Label lbFinger7;
        private System.Windows.Forms.Label lbFinger6;
        private System.Windows.Forms.Label lbFinger1;
        private System.Windows.Forms.Label lbFinger2;
        private System.Windows.Forms.Label lbFinger3;
        private System.Windows.Forms.Label lbFinger4;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox9;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private MyPictureBox pictureBoxRed;
        private MyPictureBox pictureBoxGreen;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox scannersListBox;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.SplitContainer nfViewSplitContainer;
        private Neurotec.Biometrics.Gui.NFingerView fingerView1;
        private Neurotec.Biometrics.Gui.NFingerView fingerView2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button manageCacheButton;
        private System.Windows.Forms.CheckBox checkBoxCache4;
        private System.Windows.Forms.Label labCache4;
        private System.Windows.Forms.Label labCache3;
        private System.Windows.Forms.CheckBox checkBoxCache3;
        private System.Windows.Forms.Label labCache2;
        private System.Windows.Forms.CheckBox checkBoxCache2;
        private System.Windows.Forms.Label labCache1;
        private System.Windows.Forms.CheckBox checkBoxCache1;
        private System.Windows.Forms.Label labCache8;
        private System.Windows.Forms.CheckBox checkBoxCache8;
        private System.Windows.Forms.Label labCache7;
        private System.Windows.Forms.CheckBox checkBoxCache7;
        private System.Windows.Forms.Label labCache6;
        private System.Windows.Forms.CheckBox checkBoxCache6;
        private System.Windows.Forms.Label labCache5;
        private System.Windows.Forms.CheckBox checkBoxCache5;
        private System.Windows.Forms.Label labCache10;
        private System.Windows.Forms.CheckBox checkBoxCache10;
        private System.Windows.Forms.Label labCache9;
        private System.Windows.Forms.CheckBox checkBoxCache9;
        private System.Windows.Forms.Label labelCacheUnavailable;
        private System.Windows.Forms.Label labelCacheValidationTime;
        private System.Windows.Forms.RadioButton radioButtonMan;
        private System.Windows.Forms.RadioButton radioButtonManAndWoman;
        private System.Windows.Forms.RadioButton radioButtonWoman;
        private System.Windows.Forms.Button buttonRefreshScannerListBox;
    }
}

