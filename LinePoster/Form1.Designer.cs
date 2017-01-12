namespace LinePoster
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Post2Line = new System.Windows.Forms.Button();
            this.uxData = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LineID = new System.Windows.Forms.Label();
            this.StatusBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Post2Line
            // 
            this.Post2Line.Location = new System.Drawing.Point(233, 148);
            this.Post2Line.Name = "Post2Line";
            this.Post2Line.Size = new System.Drawing.Size(80, 26);
            this.Post2Line.TabIndex = 0;
            this.Post2Line.Text = "Post to Line";
            this.Post2Line.UseVisualStyleBackColor = true;
            this.Post2Line.Click += new System.EventHandler(this.Post2Line_Click);
            // 
            // uxData
            // 
            this.uxData.Location = new System.Drawing.Point(12, 152);
            this.uxData.Name = "uxData";
            this.uxData.Size = new System.Drawing.Size(208, 22);
            this.uxData.TabIndex = 1;
            this.uxData.Text = "The End";
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(319, 152);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(232, 74);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // LineID
            // 
            this.LineID.AutoSize = true;
            this.LineID.Location = new System.Drawing.Point(326, 9);
            this.LineID.Name = "LineID";
            this.LineID.Size = new System.Drawing.Size(0, 12);
            this.LineID.TabIndex = 3;
            // 
            // StatusBox
            // 
            this.StatusBox.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.StatusBox.Location = new System.Drawing.Point(12, 180);
            this.StatusBox.Name = "StatusBox";
            this.StatusBox.Size = new System.Drawing.Size(289, 27);
            this.StatusBox.TabIndex = 4;
            this.StatusBox.Text = "Initial......";
            this.StatusBox.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(405, 73);
            this.Controls.Add(this.StatusBox);
            this.Controls.Add(this.LineID);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.uxData);
            this.Controls.Add(this.Post2Line);
            this.Name = "Form1";
            this.Text = "Initial...";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.DoubleClick += new System.EventHandler(this.Form1_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Post2Line;
        private System.Windows.Forms.TextBox uxData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label LineID;
        private System.Windows.Forms.TextBox StatusBox;
    }
}

