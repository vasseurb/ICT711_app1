namespace ICT711_app1
{
    partial class gol
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
            this.btn_load = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_calc = new System.Windows.Forms.Button();
            this.txt_gen_calc = new System.Windows.Forms.TextBox();
            this.txt_gen_current = new System.Windows.Forms.TextBox();
            this.lbl_gen_calc = new System.Windows.Forms.Label();
            this.lbl_gen_cur = new System.Windows.Forms.Label();
            this.txt_statusbar = new System.Windows.Forms.TextBox();
            this.btn_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(672, 20);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(100, 25);
            this.btn_load.TabIndex = 10;
            this.btn_load.Text = "Load Grid";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(672, 49);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(100, 25);
            this.btn_save.TabIndex = 20;
            this.btn_save.Text = "Save Grid";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(672, 80);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(100, 25);
            this.btn_reset.TabIndex = 30;
            this.btn_reset.Text = "Reset All Cells";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_calc
            // 
            this.btn_calc.Location = new System.Drawing.Point(672, 193);
            this.btn_calc.Name = "btn_calc";
            this.btn_calc.Size = new System.Drawing.Size(100, 25);
            this.btn_calc.TabIndex = 50;
            this.btn_calc.Text = "Calculate";
            this.btn_calc.UseVisualStyleBackColor = true;
            this.btn_calc.Click += new System.EventHandler(this.btn_calc_Click);
            // 
            // txt_gen_calc
            // 
            this.txt_gen_calc.Location = new System.Drawing.Point(672, 160);
            this.txt_gen_calc.Name = "txt_gen_calc";
            this.txt_gen_calc.Size = new System.Drawing.Size(50, 20);
            this.txt_gen_calc.TabIndex = 40;
            this.txt_gen_calc.Validating += new System.ComponentModel.CancelEventHandler(this.txt_gen_calc_Validating);
            // 
            // txt_gen_current
            // 
            this.txt_gen_current.Location = new System.Drawing.Point(672, 268);
            this.txt_gen_current.Name = "txt_gen_current";
            this.txt_gen_current.ReadOnly = true;
            this.txt_gen_current.Size = new System.Drawing.Size(50, 20);
            this.txt_gen_current.TabIndex = 6;
            this.txt_gen_current.TabStop = false;
            // 
            // lbl_gen_calc
            // 
            this.lbl_gen_calc.Location = new System.Drawing.Point(672, 129);
            this.lbl_gen_calc.Name = "lbl_gen_calc";
            this.lbl_gen_calc.Size = new System.Drawing.Size(100, 26);
            this.lbl_gen_calc.TabIndex = 7;
            this.lbl_gen_calc.Text = "Generations to Calculate";
            // 
            // lbl_gen_cur
            // 
            this.lbl_gen_cur.AutoSize = true;
            this.lbl_gen_cur.Location = new System.Drawing.Point(672, 248);
            this.lbl_gen_cur.Name = "lbl_gen_cur";
            this.lbl_gen_cur.Size = new System.Drawing.Size(96, 13);
            this.lbl_gen_cur.TabIndex = 8;
            this.lbl_gen_cur.Text = "Current Generation";
            // 
            // txt_statusbar
            // 
            this.txt_statusbar.Location = new System.Drawing.Point(12, 591);
            this.txt_statusbar.Name = "txt_statusbar";
            this.txt_statusbar.ReadOnly = true;
            this.txt_statusbar.Size = new System.Drawing.Size(700, 20);
            this.txt_statusbar.TabIndex = 9;
            this.txt_statusbar.TabStop = false;
            // 
            // btn_exit
            // 
            this.btn_exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_exit.Location = new System.Drawing.Point(672, 311);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 23);
            this.btn_exit.TabIndex = 60;
            this.btn_exit.Text = "Exit";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // gol
            // 
            this.AcceptButton = this.btn_calc;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_exit;
            this.ClientSize = new System.Drawing.Size(792, 625);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.txt_statusbar);
            this.Controls.Add(this.lbl_gen_cur);
            this.Controls.Add(this.lbl_gen_calc);
            this.Controls.Add(this.txt_gen_current);
            this.Controls.Add(this.txt_gen_calc);
            this.Controls.Add(this.btn_calc);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_load);
            this.Name = "gol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game of Life";
            this.Load += new System.EventHandler(this.gol_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Button btn_calc;
        private System.Windows.Forms.TextBox txt_gen_calc;
        private System.Windows.Forms.TextBox txt_gen_current;
        private System.Windows.Forms.Label lbl_gen_calc;
        private System.Windows.Forms.Label lbl_gen_cur;
        private System.Windows.Forms.TextBox txt_statusbar;
        private System.Windows.Forms.Button btn_exit;
    }
}

