namespace MyPacketCapturer
{
    partial class frmCapture
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
            this.scanBtn = new System.Windows.Forms.Button();
            this.roomLbl = new System.Windows.Forms.Label();
            this.roomSelection = new System.Windows.Forms.ComboBox();
            this.machineList = new System.Windows.Forms.ListView();
            this.dbTimer = new System.Windows.Forms.Timer(this.components);
            this.ipCaptureList = new System.Windows.Forms.ListView();
            this.dbProgress = new System.Windows.Forms.ProgressBar();
            this.tbPacketSend = new System.Windows.Forms.TextBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.devLabel1 = new System.Windows.Forms.Label();
            this.devLabel2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.currentIPLbl = new System.Windows.Forms.Label();
            this.bldgLbl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.machineCountLbl = new System.Windows.Forms.Label();
            this.ipCountLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // scanBtn
            // 
            this.scanBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.scanBtn.Font = new System.Drawing.Font("Lucida Fax", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scanBtn.Location = new System.Drawing.Point(236, 12);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(274, 38);
            this.scanBtn.TabIndex = 0;
            this.scanBtn.Text = "[Scan Text]";
            this.scanBtn.UseVisualStyleBackColor = false;
            this.scanBtn.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // roomLbl
            // 
            this.roomLbl.AutoSize = true;
            this.roomLbl.Font = new System.Drawing.Font("Lucida Fax", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomLbl.Location = new System.Drawing.Point(524, 20);
            this.roomLbl.Name = "roomLbl";
            this.roomLbl.Size = new System.Drawing.Size(65, 22);
            this.roomLbl.TabIndex = 5;
            this.roomLbl.Text = "Room";
            // 
            // roomSelection
            // 
            this.roomSelection.Font = new System.Drawing.Font("Lucida Fax", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomSelection.FormattingEnabled = true;
            this.roomSelection.Location = new System.Drawing.Point(595, 20);
            this.roomSelection.Name = "roomSelection";
            this.roomSelection.Size = new System.Drawing.Size(89, 24);
            this.roomSelection.TabIndex = 6;
            this.roomSelection.SelectedIndexChanged += new System.EventHandler(this.roomSelection_SelectedIndexChanged);
            // 
            // machineList
            // 
            this.machineList.BackColor = System.Drawing.SystemColors.Window;
            this.machineList.Font = new System.Drawing.Font("Lucida Fax", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.machineList.Location = new System.Drawing.Point(31, 104);
            this.machineList.Name = "machineList";
            this.machineList.Size = new System.Drawing.Size(466, 508);
            this.machineList.TabIndex = 7;
            this.machineList.UseCompatibleStateImageBehavior = false;
            this.machineList.View = System.Windows.Forms.View.Details;
            // 
            // dbTimer
            // 
            this.dbTimer.Interval = 1000;
            this.dbTimer.Tick += new System.EventHandler(this.dbTimer_Tick);
            // 
            // ipCaptureList
            // 
            this.ipCaptureList.BackColor = System.Drawing.SystemColors.Window;
            this.ipCaptureList.Font = new System.Drawing.Font("Lucida Fax", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipCaptureList.Location = new System.Drawing.Point(503, 104);
            this.ipCaptureList.Name = "ipCaptureList";
            this.ipCaptureList.Size = new System.Drawing.Size(217, 508);
            this.ipCaptureList.TabIndex = 10;
            this.ipCaptureList.UseCompatibleStateImageBehavior = false;
            this.ipCaptureList.View = System.Windows.Forms.View.Details;
            // 
            // dbProgress
            // 
            this.dbProgress.Location = new System.Drawing.Point(31, 651);
            this.dbProgress.Name = "dbProgress";
            this.dbProgress.Size = new System.Drawing.Size(689, 12);
            this.dbProgress.TabIndex = 11;
            // 
            // tbPacketSend
            // 
            this.tbPacketSend.Location = new System.Drawing.Point(719, 13);
            this.tbPacketSend.Multiline = true;
            this.tbPacketSend.Name = "tbPacketSend";
            this.tbPacketSend.Size = new System.Drawing.Size(13, 12);
            this.tbPacketSend.TabIndex = 12;
            this.tbPacketSend.Visible = false;
            // 
            // progressLabel
            // 
            this.progressLabel.Font = new System.Drawing.Font("Lucida Fax", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(31, 624);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(689, 18);
            this.progressLabel.TabIndex = 13;
            this.progressLabel.Text = "[Label]";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // devLabel1
            // 
            this.devLabel1.AutoSize = true;
            this.devLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.devLabel1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.devLabel1.Location = new System.Drawing.Point(92, 666);
            this.devLabel1.Name = "devLabel1";
            this.devLabel1.Size = new System.Drawing.Size(114, 13);
            this.devLabel1.TabIndex = 16;
            this.devLabel1.Text = "Contact the Developer";
            this.devLabel1.Click += new System.EventHandler(this.devLabel1_Click);
            // 
            // devLabel2
            // 
            this.devLabel2.AutoSize = true;
            this.devLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.devLabel2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.devLabel2.Location = new System.Drawing.Point(556, 666);
            this.devLabel2.Name = "devLabel2";
            this.devLabel2.Size = new System.Drawing.Size(99, 13);
            this.devLabel2.TabIndex = 17;
            this.devLabel2.Text = "Product Information";
            this.devLabel2.Click += new System.EventHandler(this.devLabel2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Current IP:";
            // 
            // currentIPLbl
            // 
            this.currentIPLbl.AutoSize = true;
            this.currentIPLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentIPLbl.Location = new System.Drawing.Point(98, 13);
            this.currentIPLbl.Name = "currentIPLbl";
            this.currentIPLbl.Size = new System.Drawing.Size(28, 16);
            this.currentIPLbl.TabIndex = 19;
            this.currentIPLbl.Text = "[IP]";
            // 
            // bldgLbl
            // 
            this.bldgLbl.AutoSize = true;
            this.bldgLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bldgLbl.Location = new System.Drawing.Point(157, 36);
            this.bldgLbl.Name = "bldgLbl";
            this.bldgLbl.Size = new System.Drawing.Size(64, 16);
            this.bldgLbl.TabIndex = 21;
            this.bldgLbl.Text = "[Building]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(36, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Scannable Building:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Fax", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "Machines Online: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Lucida Fax", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(503, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 17);
            this.label4.TabIndex = 23;
            this.label4.Text = "IP Addresses Online:";
            // 
            // machineCountLbl
            // 
            this.machineCountLbl.AutoSize = true;
            this.machineCountLbl.Font = new System.Drawing.Font("Lucida Fax", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.machineCountLbl.Location = new System.Drawing.Point(169, 85);
            this.machineCountLbl.Name = "machineCountLbl";
            this.machineCountLbl.Size = new System.Drawing.Size(136, 17);
            this.machineCountLbl.TabIndex = 24;
            this.machineCountLbl.Text = "[Machine Count]";
            // 
            // ipCountLbl
            // 
            this.ipCountLbl.AutoSize = true;
            this.ipCountLbl.Font = new System.Drawing.Font("Lucida Fax", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipCountLbl.Location = new System.Drawing.Point(666, 86);
            this.ipCountLbl.Name = "ipCountLbl";
            this.ipCountLbl.Size = new System.Drawing.Size(80, 17);
            this.ipCountLbl.TabIndex = 25;
            this.ipCountLbl.Text = "[IPCount]";
            // 
            // frmCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 685);
            this.Controls.Add(this.ipCountLbl);
            this.Controls.Add(this.machineCountLbl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bldgLbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.currentIPLbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.devLabel2);
            this.Controls.Add(this.devLabel1);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.tbPacketSend);
            this.Controls.Add(this.dbProgress);
            this.Controls.Add(this.ipCaptureList);
            this.Controls.Add(this.machineList);
            this.Controls.Add(this.roomSelection);
            this.Controls.Add(this.roomLbl);
            this.Controls.Add(this.scanBtn);
            this.Name = "frmCapture";
            this.Text = "Find Available Machine";
            this.Load += new System.EventHandler(this.frmCapture_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button scanBtn;
        private System.Windows.Forms.Label roomLbl;
        private System.Windows.Forms.Timer dbTimer;
        public System.Windows.Forms.ProgressBar dbProgress;
        public System.Windows.Forms.TextBox tbPacketSend;
        public System.Windows.Forms.Label progressLabel;
        public System.Windows.Forms.ListView machineList;
        public System.Windows.Forms.ListView ipCaptureList;
        public System.Windows.Forms.ComboBox roomSelection;
        private System.Windows.Forms.Label devLabel1;
        private System.Windows.Forms.Label devLabel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label currentIPLbl;
        private System.Windows.Forms.Label bldgLbl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label machineCountLbl;
        private System.Windows.Forms.Label ipCountLbl;
    }
}

