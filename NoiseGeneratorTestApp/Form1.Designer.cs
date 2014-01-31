namespace NoiseGeneratorTestApp
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
            this.textBoxSeed = new System.Windows.Forms.TextBox();
            this.buttonNextRandom = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.textBoxFrequency = new System.Windows.Forms.TextBox();
            this.buttonTestSort = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbPolarize = new System.Windows.Forms.CheckBox();
            this.cbNormalize = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSeed
            // 
            this.textBoxSeed.Location = new System.Drawing.Point(791, 682);
            this.textBoxSeed.Name = "textBoxSeed";
            this.textBoxSeed.Size = new System.Drawing.Size(100, 20);
            this.textBoxSeed.TabIndex = 0;
            this.textBoxSeed.Text = "1";
            this.textBoxSeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonNextRandom
            // 
            this.buttonNextRandom.Location = new System.Drawing.Point(897, 651);
            this.buttonNextRandom.Name = "buttonNextRandom";
            this.buttonNextRandom.Size = new System.Drawing.Size(75, 23);
            this.buttonNextRandom.TabIndex = 1;
            this.buttonNextRandom.Text = "Random";
            this.buttonNextRandom.UseVisualStyleBackColor = true;
            this.buttonNextRandom.Click += new System.EventHandler(this.ButtonNextRandomClick);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(897, 680);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerate.TabIndex = 2;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.ButtonGenerateClick);
            // 
            // textBoxFrequency
            // 
            this.textBoxFrequency.Location = new System.Drawing.Point(791, 654);
            this.textBoxFrequency.Name = "textBoxFrequency";
            this.textBoxFrequency.Size = new System.Drawing.Size(100, 20);
            this.textBoxFrequency.TabIndex = 3;
            this.textBoxFrequency.Text = "10";
            this.textBoxFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonTestSort
            // 
            this.buttonTestSort.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonTestSort.Location = new System.Drawing.Point(897, 12);
            this.buttonTestSort.Name = "buttonTestSort";
            this.buttonTestSort.Size = new System.Drawing.Size(75, 23);
            this.buttonTestSort.TabIndex = 4;
            this.buttonTestSort.Text = "Test Sort";
            this.buttonTestSort.UseVisualStyleBackColor = true;
            this.buttonTestSort.Click += new System.EventHandler(this.buttonTestSort_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbPolarize);
            this.groupBox1.Controls.Add(this.cbNormalize);
            this.groupBox1.Location = new System.Drawing.Point(504, 641);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 62);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Decorators";
            // 
            // cbPolarize
            // 
            this.cbPolarize.AutoSize = true;
            this.cbPolarize.Location = new System.Drawing.Point(7, 39);
            this.cbPolarize.Name = "cbPolarize";
            this.cbPolarize.Size = new System.Drawing.Size(63, 17);
            this.cbPolarize.TabIndex = 1;
            this.cbPolarize.Text = "Polarize";
            this.cbPolarize.UseVisualStyleBackColor = true;
            // 
            // cbNormalize
            // 
            this.cbNormalize.AutoSize = true;
            this.cbNormalize.Location = new System.Drawing.Point(7, 18);
            this.cbNormalize.Name = "cbNormalize";
            this.cbNormalize.Size = new System.Drawing.Size(72, 17);
            this.cbNormalize.TabIndex = 0;
            this.cbNormalize.Text = "Normalize";
            this.cbNormalize.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(710, 680);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 712);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonTestSort);
            this.Controls.Add(this.textBoxFrequency);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.buttonNextRandom);
            this.Controls.Add(this.textBoxSeed);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSeed;
        private System.Windows.Forms.Button buttonNextRandom;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.TextBox textBoxFrequency;
        private System.Windows.Forms.Button buttonTestSort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbPolarize;
        private System.Windows.Forms.CheckBox cbNormalize;
        private System.Windows.Forms.Button buttonSave;
    }
}

