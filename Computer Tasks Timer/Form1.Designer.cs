namespace Computer_Tasks_Timer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SecondsLBL = new System.Windows.Forms.Label();
            this.MinutesLBL = new System.Windows.Forms.Label();
            this.HoursLBL = new System.Windows.Forms.Label();
            this.SecondsCount = new System.Windows.Forms.NumericUpDown();
            this.MinutesCount = new System.Windows.Forms.NumericUpDown();
            this.HoursCount = new System.Windows.Forms.NumericUpDown();
            this.TaskSelector = new System.Windows.Forms.ComboBox();
            this.StartBTN = new System.Windows.Forms.Button();
            this.TaskTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SecondsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinutesCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HoursCount)).BeginInit();
            this.SuspendLayout();
            // 
            // SecondsLBL
            // 
            this.SecondsLBL.AutoSize = true;
            this.SecondsLBL.Location = new System.Drawing.Point(117, 9);
            this.SecondsLBL.Name = "SecondsLBL";
            this.SecondsLBL.Size = new System.Drawing.Size(49, 13);
            this.SecondsLBL.TabIndex = 17;
            this.SecondsLBL.Text = "Seconds";
            // 
            // MinutesLBL
            // 
            this.MinutesLBL.AutoSize = true;
            this.MinutesLBL.Location = new System.Drawing.Point(65, 9);
            this.MinutesLBL.Name = "MinutesLBL";
            this.MinutesLBL.Size = new System.Drawing.Size(44, 13);
            this.MinutesLBL.TabIndex = 16;
            this.MinutesLBL.Text = "Minutes";
            // 
            // HoursLBL
            // 
            this.HoursLBL.AutoSize = true;
            this.HoursLBL.Location = new System.Drawing.Point(12, 9);
            this.HoursLBL.Name = "HoursLBL";
            this.HoursLBL.Size = new System.Drawing.Size(35, 13);
            this.HoursLBL.TabIndex = 15;
            this.HoursLBL.Text = "Hours";
            // 
            // SecondsCount
            // 
            this.SecondsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecondsCount.Location = new System.Drawing.Point(118, 25);
            this.SecondsCount.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.SecondsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.SecondsCount.Name = "SecondsCount";
            this.SecondsCount.Size = new System.Drawing.Size(50, 38);
            this.SecondsCount.TabIndex = 14;
            this.SecondsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SecondsCount.ValueChanged += new System.EventHandler(this.SecondsCount_ValueChanged);
            // 
            // MinutesCount
            // 
            this.MinutesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinutesCount.Location = new System.Drawing.Point(62, 25);
            this.MinutesCount.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.MinutesCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.MinutesCount.Name = "MinutesCount";
            this.MinutesCount.Size = new System.Drawing.Size(50, 38);
            this.MinutesCount.TabIndex = 13;
            this.MinutesCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MinutesCount.ValueChanged += new System.EventHandler(this.MinutesCount_ValueChanged);
            // 
            // HoursCount
            // 
            this.HoursCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoursCount.Location = new System.Drawing.Point(6, 25);
            this.HoursCount.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.HoursCount.Name = "HoursCount";
            this.HoursCount.Size = new System.Drawing.Size(50, 38);
            this.HoursCount.TabIndex = 12;
            this.HoursCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TaskSelector
            // 
            this.TaskSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TaskSelector.FormattingEnabled = true;
            this.TaskSelector.Items.AddRange(new object[] {
            "Shutdown",
            "Restart",
            "Lock",
            "Sleep",
            "Hibernate",
            "Screen Off",
            "Sign Out"});
            this.TaskSelector.Location = new System.Drawing.Point(174, 6);
            this.TaskSelector.Name = "TaskSelector";
            this.TaskSelector.Size = new System.Drawing.Size(81, 21);
            this.TaskSelector.TabIndex = 14;
            // 
            // StartBTN
            // 
            this.StartBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartBTN.Location = new System.Drawing.Point(174, 33);
            this.StartBTN.Name = "StartBTN";
            this.StartBTN.Size = new System.Drawing.Size(81, 30);
            this.StartBTN.TabIndex = 19;
            this.StartBTN.Text = "Start";
            this.StartBTN.UseVisualStyleBackColor = true;
            this.StartBTN.Click += new System.EventHandler(this.StartBTN_Click);
            // 
            // TaskTimer
            // 
            this.TaskTimer.Interval = 1000;
            this.TaskTimer.Tick += new System.EventHandler(this.TaskTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 71);
            this.Controls.Add(this.StartBTN);
            this.Controls.Add(this.SecondsLBL);
            this.Controls.Add(this.MinutesLBL);
            this.Controls.Add(this.TaskSelector);
            this.Controls.Add(this.HoursLBL);
            this.Controls.Add(this.SecondsCount);
            this.Controls.Add(this.MinutesCount);
            this.Controls.Add(this.HoursCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Tasks Timer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SecondsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinutesCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HoursCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label SecondsLBL;
        private System.Windows.Forms.Label MinutesLBL;
        private System.Windows.Forms.Label HoursLBL;
        private System.Windows.Forms.NumericUpDown SecondsCount;
        private System.Windows.Forms.NumericUpDown MinutesCount;
        private System.Windows.Forms.NumericUpDown HoursCount;
        private System.Windows.Forms.ComboBox TaskSelector;
        private System.Windows.Forms.Button StartBTN;
        private System.Windows.Forms.Timer TaskTimer;
    }
}

