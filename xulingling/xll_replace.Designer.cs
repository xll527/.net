namespace xulingling
{
    partial class xll_replace
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
            this.查找label = new System.Windows.Forms.Label();
            this.替换label = new System.Windows.Forms.Label();
            this.查找button = new System.Windows.Forms.Button();
            this.tb_findtext = new System.Windows.Forms.TextBox();
            this.tb_replace = new System.Windows.Forms.TextBox();
            this.替换button = new System.Windows.Forms.Button();
            this.全部替换button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 查找label
            // 
            this.查找label.AutoSize = true;
            this.查找label.Location = new System.Drawing.Point(64, 43);
            this.查找label.Name = "查找label";
            this.查找label.Size = new System.Drawing.Size(98, 18);
            this.查找label.TabIndex = 0;
            this.查找label.Text = "查找内容：";
            // 
            // 替换label
            // 
            this.替换label.AutoSize = true;
            this.替换label.Location = new System.Drawing.Point(67, 116);
            this.替换label.Name = "替换label";
            this.替换label.Size = new System.Drawing.Size(80, 18);
            this.替换label.TabIndex = 1;
            this.替换label.Text = "替换为：";
            // 
            // 查找button
            // 
            this.查找button.Location = new System.Drawing.Point(53, 197);
            this.查找button.Name = "查找button";
            this.查找button.Size = new System.Drawing.Size(109, 39);
            this.查找button.TabIndex = 2;
            this.查找button.Text = "查找";
            this.查找button.UseVisualStyleBackColor = true;
            this.查找button.Click += new System.EventHandler(this.查找button_Click);
            // 
            // tb_findtext
            // 
            this.tb_findtext.Location = new System.Drawing.Point(182, 40);
            this.tb_findtext.Name = "tb_findtext";
            this.tb_findtext.Size = new System.Drawing.Size(510, 28);
            this.tb_findtext.TabIndex = 5;
            // 
            // tb_replace
            // 
            this.tb_replace.Location = new System.Drawing.Point(182, 113);
            this.tb_replace.Name = "tb_replace";
            this.tb_replace.Size = new System.Drawing.Size(510, 28);
            this.tb_replace.TabIndex = 6;
            // 
            // 替换button
            // 
            this.替换button.Location = new System.Drawing.Point(285, 197);
            this.替换button.Name = "替换button";
            this.替换button.Size = new System.Drawing.Size(109, 39);
            this.替换button.TabIndex = 7;
            this.替换button.Text = "替换";
            this.替换button.UseVisualStyleBackColor = true;
            this.替换button.Click += new System.EventHandler(this.替换button_Click);
            // 
            // 全部替换button
            // 
            this.全部替换button.Location = new System.Drawing.Point(502, 197);
            this.全部替换button.Name = "全部替换button";
            this.全部替换button.Size = new System.Drawing.Size(109, 39);
            this.全部替换button.TabIndex = 8;
            this.全部替换button.Text = "全部替换";
            this.全部替换button.UseVisualStyleBackColor = true;
            this.全部替换button.Click += new System.EventHandler(this.全部替换button_Click);
            // 
            // xll_replace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 265);
            this.Controls.Add(this.全部替换button);
            this.Controls.Add(this.替换button);
            this.Controls.Add(this.tb_replace);
            this.Controls.Add(this.tb_findtext);
            this.Controls.Add(this.查找button);
            this.Controls.Add(this.替换label);
            this.Controls.Add(this.查找label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "xll_replace";
            this.Text = "查找替换";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.xll_replace_FormClosed);
            this.Load += new System.EventHandler(this.xll_replace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label 查找label;
        private System.Windows.Forms.Label 替换label;
        private System.Windows.Forms.Button 查找button;
        private System.Windows.Forms.TextBox tb_findtext;
        private System.Windows.Forms.TextBox tb_replace;
        private System.Windows.Forms.Button 替换button;
        private System.Windows.Forms.Button 全部替换button;
    }
}