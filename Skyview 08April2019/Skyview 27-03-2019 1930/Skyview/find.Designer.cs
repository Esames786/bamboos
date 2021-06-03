namespace Skyview
{
    partial class find
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
            this.listbox = new System.Windows.Forms.ListBox();
            this.txtdisp = new System.Windows.Forms.TextBox();
            this.txtcat = new System.Windows.Forms.ComboBox();
            this.txtprod = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnadd = new System.Windows.Forms.Button();
            this.btnremo = new System.Windows.Forms.Button();
            this.txt_dt_from = new System.Windows.Forms.DateTimePicker();
            this.txt_dt_to = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listbox
            // 
            this.listbox.FormattingEnabled = true;
            this.listbox.Location = new System.Drawing.Point(110, 195);
            this.listbox.Name = "listbox";
            this.listbox.Size = new System.Drawing.Size(405, 186);
            this.listbox.TabIndex = 1;
            this.listbox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listbox_MouseClick);
            this.listbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listbox_KeyDown);
            // 
            // txtdisp
            // 
            this.txtdisp.Location = new System.Drawing.Point(209, 91);
            this.txtdisp.Multiline = true;
            this.txtdisp.Name = "txtdisp";
            this.txtdisp.Size = new System.Drawing.Size(213, 20);
            this.txtdisp.TabIndex = 2;
            // 
            // txtcat
            // 
            this.txtcat.FormattingEnabled = true;
            this.txtcat.Location = new System.Drawing.Point(210, 12);
            this.txtcat.Name = "txtcat";
            this.txtcat.Size = new System.Drawing.Size(213, 21);
            this.txtcat.TabIndex = 3;
            this.txtcat.SelectedIndexChanged += new System.EventHandler(this.txtcat_SelectedIndexChanged);
            // 
            // txtprod
            // 
            this.txtprod.FormattingEnabled = true;
            this.txtprod.Location = new System.Drawing.Point(209, 48);
            this.txtprod.Name = "txtprod";
            this.txtprod.Size = new System.Drawing.Size(214, 21);
            this.txtprod.TabIndex = 4;
            this.txtprod.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(199, 422);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(177, 53);
            this.button1.TabIndex = 5;
            this.button1.Text = "PRINT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(82, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Category Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(82, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Product Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(82, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Display";
            // 
            // btnadd
            // 
            this.btnadd.Location = new System.Drawing.Point(248, 128);
            this.btnadd.Name = "btnadd";
            this.btnadd.Size = new System.Drawing.Size(114, 53);
            this.btnadd.TabIndex = 9;
            this.btnadd.Text = "Add";
            this.btnadd.UseVisualStyleBackColor = true;
            this.btnadd.Click += new System.EventHandler(this.btnadd_Click);
            // 
            // btnremo
            // 
            this.btnremo.Location = new System.Drawing.Point(154, 128);
            this.btnremo.Name = "btnremo";
            this.btnremo.Size = new System.Drawing.Size(78, 53);
            this.btnremo.TabIndex = 10;
            this.btnremo.Text = "Remove";
            this.btnremo.UseVisualStyleBackColor = true;
            this.btnremo.Click += new System.EventHandler(this.btnremo_Click);
            // 
            // txt_dt_from
            // 
            this.txt_dt_from.CustomFormat = "yyyy-MM-dd";
            this.txt_dt_from.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_dt_from.Location = new System.Drawing.Point(581, 15);
            this.txt_dt_from.Name = "txt_dt_from";
            this.txt_dt_from.Size = new System.Drawing.Size(131, 20);
            this.txt_dt_from.TabIndex = 11;
            // 
            // txt_dt_to
            // 
            this.txt_dt_to.CustomFormat = "yyyy-MM-dd";
            this.txt_dt_to.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_dt_to.Location = new System.Drawing.Point(581, 51);
            this.txt_dt_to.Name = "txt_dt_to";
            this.txt_dt_to.Size = new System.Drawing.Size(131, 20);
            this.txt_dt_to.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(527, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(517, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "From";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(407, 128);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(62, 52);
            this.button2.TabIndex = 15;
            this.button2.Text = "Refresh";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // find
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 521);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_dt_to);
            this.Controls.Add(this.txt_dt_from);
            this.Controls.Add(this.btnremo);
            this.Controls.Add(this.btnadd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtprod);
            this.Controls.Add(this.txtcat);
            this.Controls.Add(this.txtdisp);
            this.Controls.Add(this.listbox);
            this.Name = "find";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Refresh";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listbox;
        private System.Windows.Forms.TextBox txtdisp;
        private System.Windows.Forms.ComboBox txtcat;
        private System.Windows.Forms.ComboBox txtprod;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnadd;
        private System.Windows.Forms.Button btnremo;
        private System.Windows.Forms.DateTimePicker txt_dt_from;
        private System.Windows.Forms.DateTimePicker txt_dt_to;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
    }
}