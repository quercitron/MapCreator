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
            this.buttonNextRandom.Location = new System.Drawing.Point(710, 680);
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
            this.textBoxFrequency.Location = new System.Drawing.Point(791, 656);
            this.textBoxFrequency.Name = "textBoxFrequency";
            this.textBoxFrequency.Size = new System.Drawing.Size(100, 20);
            this.textBoxFrequency.TabIndex = 3;
            this.textBoxFrequency.Text = "10";
            this.textBoxFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 712);
            this.Controls.Add(this.textBoxFrequency);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.buttonNextRandom);
            this.Controls.Add(this.textBoxSeed);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSeed;
        private System.Windows.Forms.Button buttonNextRandom;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.TextBox textBoxFrequency;
    }
}

