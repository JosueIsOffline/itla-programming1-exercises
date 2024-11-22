namespace AppSimple
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btn_suma = new Button();
            btn_restar = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            label3 = new Label();
            button1 = new Button();
            listView1 = new ListView();
            SuspendLayout();
            // 
            // btn_suma
            // 
            btn_suma.BackColor = Color.Lime;
            btn_suma.FlatAppearance.BorderSize = 0;
            btn_suma.FlatStyle = FlatStyle.Popup;
            btn_suma.Font = new Font("Segoe UI", 16F);
            btn_suma.Location = new Point(73, 198);
            btn_suma.Name = "btn_suma";
            btn_suma.Size = new Size(129, 70);
            btn_suma.TabIndex = 0;
            btn_suma.Text = "Sumar";
            btn_suma.UseVisualStyleBackColor = false;
            btn_suma.Click += btn_suma_Click;
            // 
            // btn_restar
            // 
            btn_restar.BackColor = Color.Red;
            btn_restar.FlatStyle = FlatStyle.Popup;
            btn_restar.Font = new Font("Segoe UI", 16F);
            btn_restar.Location = new Point(73, 274);
            btn_restar.Name = "btn_restar";
            btn_restar.Size = new Size(129, 70);
            btn_restar.TabIndex = 1;
            btn_restar.Text = "Restar";
            btn_restar.UseVisualStyleBackColor = false;
            btn_restar.Click += btn_restar_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 10F);
            textBox1.Location = new Point(73, 41);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(135, 31);
            textBox1.TabIndex = 2;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(73, 23);
            label1.Name = "label1";
            label1.Size = new Size(63, 15);
            label1.TabIndex = 3;
            label1.Text = "Número 1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(73, 96);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 5;
            label2.Text = "Número 2 ";
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Segoe UI", 10F);
            textBox2.Location = new Point(73, 114);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(135, 31);
            textBox2.TabIndex = 4;
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label3.Location = new Point(415, 114);
            label3.Name = "label3";
            label3.Size = new Size(169, 37);
            label3.TabIndex = 6;
            label3.Text = "Calculadora";
            // 
            // button1
            // 
            button1.BackColor = Color.DodgerBlue;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Segoe UI", 16F);
            button1.Location = new Point(73, 350);
            button1.Name = "button1";
            button1.Size = new Size(129, 70);
            button1.TabIndex = 9;
            button1.Text = "Limpiar";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // listView1
            // 
            listView1.Location = new Point(246, 200);
            listView1.Name = "listView1";
            listView1.Size = new Size(518, 222);
            listView1.TabIndex = 10;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(listView1);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(btn_restar);
            Controls.Add(btn_suma);
            Cursor = Cursors.Hand;
            Name = "Form1";
            Text = "App Simple";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_suma;
        private Button btn_restar;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox2;
        private Label label3;
        private Button button1;
        private ListView listView1;
    }
}
