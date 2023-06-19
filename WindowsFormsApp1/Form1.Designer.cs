using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
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
            this.timer1 = new System.Timers.Timer();
            this.dataGridViewHotBar = new DataGridView();
            this.dataGridViewInventory = new DataGridView();
            this.dataGridViewEquipment = new DataGridView();
            this.progressBarForHealth = new ProgressBar();
            this.progressBarForFood = new ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = false;
            this.timer1.Interval = 100D;
            this.timer1.SynchronizingObject = this;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.Timer1_Elapsed);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(
                System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width,
                System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height
                );
            this.Controls.Add(this.dataGridViewInventory);
            this.Controls.Add(this.dataGridViewHotBar);
            this.Controls.Add(this.dataGridViewEquipment);
            this.Controls.Add(this.progressBarForHealth);
            this.Controls.Add(this.progressBarForFood);
            this.Name = "RPG";
            this.Text = "RPG";
            //
            /*this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;*/
            //
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
            this.ResumeLayout(false);
            //
            //progressBarForHealth
            //
            this.progressBarForHealth.Location = new System.Drawing.Point(0, 30);
            this.progressBarForHealth.Name = "progressBarForHealth";
            this.progressBarForHealth.Size = new System.Drawing.Size(300, 40);
            this.progressBarForHealth.TabIndex = 0;
            this.progressBarForHealth.Value = 100;
            //
            //progressBarForFood
            //
            this.progressBarForFood.Location = new System.Drawing.Point(0, 100);
            this.progressBarForFood.Name = "progressBarForFood";
            this.progressBarForFood.Size = new System.Drawing.Size(300, 40);
            this.progressBarForFood.TabIndex = 0;
            this.progressBarForFood.Value = 100;
            //
            Label labelForHealth = new Label();
            labelForHealth.Location = new System.Drawing.Point(0, 10);
            labelForHealth.Size = new System.Drawing.Size(300, 20);
            labelForHealth.Text = "Health";
            labelForHealth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(labelForHealth);
            //
            Label labelForFood = new Label();
            labelForFood.Location = new System.Drawing.Point(0, 80);
            labelForFood.Size = new System.Drawing.Size(300, 20);
            labelForFood.Text = "Food";
            labelForFood.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(labelForFood);
            //
        }

        private System.Windows.Forms.DataGridView dataGridViewHotBar;
        
        private System.Windows.Forms.DataGridView dataGridViewInventory;
        
        private System.Windows.Forms.DataGridView dataGridViewEquipment;
        
        private System.Windows.Forms.ProgressBar progressBarForHealth;
        
        private System.Windows.Forms.ProgressBar progressBarForFood;

        private System.Timers.Timer timer1;

        #endregion
    }
}