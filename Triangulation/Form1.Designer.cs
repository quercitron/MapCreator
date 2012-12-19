﻿namespace Triangulation
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
            this.buttonAddRandomPoint = new System.Windows.Forms.Button();
            this.buttonAddRange = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxShowRivers = new System.Windows.Forms.CheckBox();
            this.checkBoxElevation = new System.Windows.Forms.CheckBox();
            this.checkBoxCoast = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxLinealBorders = new System.Windows.Forms.CheckBox();
            this.checkBoxNoiseBorders = new System.Windows.Forms.CheckBox();
            this.checkBoxApplyNoise = new System.Windows.Forms.CheckBox();
            this.checkBoxPolygons = new System.Windows.Forms.CheckBox();
            this.checkBoxTriangles = new System.Windows.Forms.CheckBox();
            this.checkBoxPoints = new System.Windows.Forms.CheckBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.checkBoxShowTimes = new System.Windows.Forms.CheckBox();
            this.buttonRedraw = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonSeed = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAddRandomPoint
            // 
            this.buttonAddRandomPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddRandomPoint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddRandomPoint.Location = new System.Drawing.Point(1272, 12);
            this.buttonAddRandomPoint.Name = "buttonAddRandomPoint";
            this.buttonAddRandomPoint.Size = new System.Drawing.Size(180, 45);
            this.buttonAddRandomPoint.TabIndex = 0;
            this.buttonAddRandomPoint.Text = "Add Random Point";
            this.buttonAddRandomPoint.UseVisualStyleBackColor = true;
            this.buttonAddRandomPoint.Click += new System.EventHandler(this.ButtonAddRandomPointClick);
            // 
            // buttonAddRange
            // 
            this.buttonAddRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddRange.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddRange.Location = new System.Drawing.Point(1272, 63);
            this.buttonAddRange.Name = "buttonAddRange";
            this.buttonAddRange.Size = new System.Drawing.Size(180, 45);
            this.buttonAddRange.TabIndex = 1;
            this.buttonAddRange.Text = "Add Range";
            this.buttonAddRange.UseVisualStyleBackColor = true;
            this.buttonAddRange.Click += new System.EventHandler(this.ButtonAddRangeClick);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1254, 1054);
            this.panel1.TabIndex = 2;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1Paint);
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Panel1MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxShowRivers);
            this.groupBox1.Controls.Add(this.checkBoxElevation);
            this.groupBox1.Controls.Add(this.checkBoxCoast);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkBoxApplyNoise);
            this.groupBox1.Controls.Add(this.checkBoxPolygons);
            this.groupBox1.Controls.Add(this.checkBoxTriangles);
            this.groupBox1.Controls.Add(this.checkBoxPoints);
            this.groupBox1.Location = new System.Drawing.Point(1272, 267);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 253);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show Options";
            // 
            // checkBoxShowRivers
            // 
            this.checkBoxShowRivers.AutoSize = true;
            this.checkBoxShowRivers.Checked = true;
            this.checkBoxShowRivers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowRivers.Location = new System.Drawing.Point(7, 230);
            this.checkBoxShowRivers.Name = "checkBoxShowRivers";
            this.checkBoxShowRivers.Size = new System.Drawing.Size(86, 17);
            this.checkBoxShowRivers.TabIndex = 9;
            this.checkBoxShowRivers.Text = "Show Rivers";
            this.checkBoxShowRivers.UseVisualStyleBackColor = true;
            // 
            // checkBoxElevation
            // 
            this.checkBoxElevation.AutoSize = true;
            this.checkBoxElevation.Location = new System.Drawing.Point(7, 208);
            this.checkBoxElevation.Name = "checkBoxElevation";
            this.checkBoxElevation.Size = new System.Drawing.Size(100, 17);
            this.checkBoxElevation.TabIndex = 8;
            this.checkBoxElevation.Text = "Show Elevation";
            this.checkBoxElevation.UseVisualStyleBackColor = true;
            // 
            // checkBoxCoast
            // 
            this.checkBoxCoast.AutoSize = true;
            this.checkBoxCoast.Checked = true;
            this.checkBoxCoast.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCoast.Location = new System.Drawing.Point(7, 184);
            this.checkBoxCoast.Name = "checkBoxCoast";
            this.checkBoxCoast.Size = new System.Drawing.Size(53, 17);
            this.checkBoxCoast.TabIndex = 7;
            this.checkBoxCoast.Text = "Coast";
            this.checkBoxCoast.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxLinealBorders);
            this.groupBox2.Controls.Add(this.checkBoxNoiseBorders);
            this.groupBox2.Location = new System.Drawing.Point(8, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(128, 65);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Borders";
            // 
            // checkBoxLinealBorders
            // 
            this.checkBoxLinealBorders.AutoSize = true;
            this.checkBoxLinealBorders.Location = new System.Drawing.Point(6, 19);
            this.checkBoxLinealBorders.Name = "checkBoxLinealBorders";
            this.checkBoxLinealBorders.Size = new System.Drawing.Size(93, 17);
            this.checkBoxLinealBorders.TabIndex = 2;
            this.checkBoxLinealBorders.Text = "Lineal Borders";
            this.checkBoxLinealBorders.UseVisualStyleBackColor = true;
            // 
            // checkBoxNoiseBorders
            // 
            this.checkBoxNoiseBorders.AutoSize = true;
            this.checkBoxNoiseBorders.Location = new System.Drawing.Point(6, 42);
            this.checkBoxNoiseBorders.Name = "checkBoxNoiseBorders";
            this.checkBoxNoiseBorders.Size = new System.Drawing.Size(92, 17);
            this.checkBoxNoiseBorders.TabIndex = 3;
            this.checkBoxNoiseBorders.Text = "Noise Borders";
            this.checkBoxNoiseBorders.UseVisualStyleBackColor = true;
            // 
            // checkBoxApplyNoise
            // 
            this.checkBoxApplyNoise.AutoSize = true;
            this.checkBoxApplyNoise.Location = new System.Drawing.Point(7, 161);
            this.checkBoxApplyNoise.Name = "checkBoxApplyNoise";
            this.checkBoxApplyNoise.Size = new System.Drawing.Size(82, 17);
            this.checkBoxApplyNoise.TabIndex = 4;
            this.checkBoxApplyNoise.Text = "Apply Noise";
            this.checkBoxApplyNoise.UseVisualStyleBackColor = true;
            // 
            // checkBoxPolygons
            // 
            this.checkBoxPolygons.AutoSize = true;
            this.checkBoxPolygons.Checked = true;
            this.checkBoxPolygons.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPolygons.Location = new System.Drawing.Point(7, 138);
            this.checkBoxPolygons.Name = "checkBoxPolygons";
            this.checkBoxPolygons.Size = new System.Drawing.Size(69, 17);
            this.checkBoxPolygons.TabIndex = 3;
            this.checkBoxPolygons.Text = "Polygons";
            this.checkBoxPolygons.UseVisualStyleBackColor = true;
            // 
            // checkBoxTriangles
            // 
            this.checkBoxTriangles.AutoSize = true;
            this.checkBoxTriangles.Location = new System.Drawing.Point(7, 44);
            this.checkBoxTriangles.Name = "checkBoxTriangles";
            this.checkBoxTriangles.Size = new System.Drawing.Size(69, 17);
            this.checkBoxTriangles.TabIndex = 1;
            this.checkBoxTriangles.Text = "Triangles";
            this.checkBoxTriangles.UseVisualStyleBackColor = true;
            // 
            // checkBoxPoints
            // 
            this.checkBoxPoints.AutoSize = true;
            this.checkBoxPoints.Checked = true;
            this.checkBoxPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPoints.Location = new System.Drawing.Point(7, 20);
            this.checkBoxPoints.Name = "checkBoxPoints";
            this.checkBoxPoints.Size = new System.Drawing.Size(55, 17);
            this.checkBoxPoints.TabIndex = 0;
            this.checkBoxPoints.Text = "Points";
            this.checkBoxPoints.UseVisualStyleBackColor = true;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonReset.Location = new System.Drawing.Point(1272, 216);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(180, 45);
            this.buttonReset.TabIndex = 4;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonResetClick);
            // 
            // checkBoxShowTimes
            // 
            this.checkBoxShowTimes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxShowTimes.AutoSize = true;
            this.checkBoxShowTimes.Location = new System.Drawing.Point(1272, 526);
            this.checkBoxShowTimes.Name = "checkBoxShowTimes";
            this.checkBoxShowTimes.Size = new System.Drawing.Size(84, 17);
            this.checkBoxShowTimes.TabIndex = 5;
            this.checkBoxShowTimes.Text = "Show Times";
            this.checkBoxShowTimes.UseVisualStyleBackColor = true;
            // 
            // buttonRedraw
            // 
            this.buttonRedraw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRedraw.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRedraw.Location = new System.Drawing.Point(1272, 165);
            this.buttonRedraw.Name = "buttonRedraw";
            this.buttonRedraw.Size = new System.Drawing.Size(180, 45);
            this.buttonRedraw.TabIndex = 6;
            this.buttonRedraw.Text = "Redraw";
            this.buttonRedraw.UseVisualStyleBackColor = true;
            this.buttonRedraw.Click += new System.EventHandler(this.ButtonRedrawClick);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.Location = new System.Drawing.Point(1272, 129);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(84, 20);
            this.numericUpDown1.TabIndex = 7;
            // 
            // buttonSeed
            // 
            this.buttonSeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSeed.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSeed.Location = new System.Drawing.Point(1362, 114);
            this.buttonSeed.Name = "buttonSeed";
            this.buttonSeed.Size = new System.Drawing.Size(90, 45);
            this.buttonSeed.TabIndex = 8;
            this.buttonSeed.Text = "Seed";
            this.buttonSeed.UseVisualStyleBackColor = true;
            this.buttonSeed.Click += new System.EventHandler(this.ButtonSeedClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1464, 1054);
            this.Controls.Add(this.buttonSeed);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.buttonRedraw);
            this.Controls.Add(this.checkBoxShowTimes);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonAddRange);
            this.Controls.Add(this.buttonAddRandomPoint);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddRandomPoint;
        private System.Windows.Forms.Button buttonAddRange;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxTriangles;
        private System.Windows.Forms.CheckBox checkBoxPoints;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.CheckBox checkBoxPolygons;
        private System.Windows.Forms.CheckBox checkBoxShowTimes;
        private System.Windows.Forms.CheckBox checkBoxApplyNoise;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxLinealBorders;
        private System.Windows.Forms.CheckBox checkBoxNoiseBorders;
        private System.Windows.Forms.CheckBox checkBoxCoast;
        private System.Windows.Forms.CheckBox checkBoxElevation;
        private System.Windows.Forms.Button buttonRedraw;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button buttonSeed;
        private System.Windows.Forms.CheckBox checkBoxShowRivers;
    }
}
