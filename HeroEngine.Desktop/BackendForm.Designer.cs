namespace HeroEngine.Desktop
{
    partial class BackendForm
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
            components = new System.ComponentModel.Container();
            groupBox1 = new GroupBox();
            label1 = new Label();
            borderButton2 = new Design.BorderButton();
            borderButton1 = new Design.BorderButton();
            rjTextBox3 = new Material.BorderTextBox();
            rjTextBox2 = new Material.BorderTextBox();
            rjTextBox1 = new Material.BorderTextBox();
            borderButton3 = new Design.BorderButton();
            groupBox2 = new GroupBox();
            groupBox9 = new GroupBox();
            checkBox21 = new CheckBox();
            checkBox20 = new CheckBox();
            checkBox17 = new CheckBox();
            checkBox14 = new CheckBox();
            groupBox8 = new GroupBox();
            checkBox15 = new CheckBox();
            groupBox7 = new GroupBox();
            checkBox13 = new CheckBox();
            checkBox12 = new CheckBox();
            groupBox6 = new GroupBox();
            checkBox23 = new CheckBox();
            label4 = new Label();
            numericUpDown3 = new NumericUpDown();
            checkBox16 = new CheckBox();
            checkBox11 = new CheckBox();
            label3 = new Label();
            numericUpDown2 = new NumericUpDown();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            checkBox10 = new CheckBox();
            groupBox5 = new GroupBox();
            checkBox19 = new CheckBox();
            checkBox9 = new CheckBox();
            groupBox4 = new GroupBox();
            checkBox18 = new CheckBox();
            checkBox8 = new CheckBox();
            groupBox3 = new GroupBox();
            checkBox22 = new CheckBox();
            checkBox2 = new CheckBox();
            checkBox7 = new CheckBox();
            checkBox3 = new CheckBox();
            checkBox6 = new CheckBox();
            checkBox4 = new CheckBox();
            checkBox5 = new CheckBox();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            checkBox1 = new CheckBox();
            timer1 = new System.Windows.Forms.Timer(components);
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(borderButton2);
            groupBox1.Controls.Add(borderButton1);
            groupBox1.Controls.Add(rjTextBox3);
            groupBox1.Controls.Add(rjTextBox2);
            groupBox1.Controls.Add(rjTextBox1);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(508, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(559, 450);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Zarzadznie kontami";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 172);
            label1.Name = "label1";
            label1.Size = new Size(41, 15);
            label1.TabIndex = 12;
            label1.Text = "Konta:";
            // 
            // borderButton2
            // 
            borderButton2.BackColor = Color.FromArgb(40, 42, 46);
            borderButton2.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            borderButton2.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            borderButton2.BorderColor = Color.FromArgb(64, 64, 64);
            borderButton2.BorderColorMouseDown = Color.FromArgb(64, 64, 64);
            borderButton2.BorderColorMouseOver = Color.FromArgb(64, 64, 64);
            borderButton2.BorderRadius = 7;
            borderButton2.BorderWidth = 1;
            borderButton2.ForeColor = Color.DarkGray;
            borderButton2.Location = new Point(251, 139);
            borderButton2.Name = "borderButton2";
            borderButton2.Size = new Size(150, 30);
            borderButton2.TabIndex = 11;
            borderButton2.Text = "Usun wszystkie konta";
            borderButton2.Click += borderButton2_Click;
            // 
            // borderButton1
            // 
            borderButton1.BackColor = Color.FromArgb(40, 42, 46);
            borderButton1.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            borderButton1.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            borderButton1.BorderColor = Color.FromArgb(64, 64, 64);
            borderButton1.BorderColorMouseDown = Color.FromArgb(64, 64, 64);
            borderButton1.BorderColorMouseOver = Color.FromArgb(64, 64, 64);
            borderButton1.BorderRadius = 7;
            borderButton1.BorderWidth = 1;
            borderButton1.ForeColor = Color.DarkGray;
            borderButton1.Location = new Point(7, 139);
            borderButton1.Name = "borderButton1";
            borderButton1.Size = new Size(150, 30);
            borderButton1.TabIndex = 8;
            borderButton1.Text = "Dodaj konto";
            borderButton1.Click += borderButton1_Click;
            // 
            // rjTextBox3
            // 
            rjTextBox3.BackColor = Color.FromArgb(40, 42, 46);
            rjTextBox3.BorderColor = Color.FromArgb(64, 64, 64);
            rjTextBox3.BorderFocusColor = Color.FromArgb(59, 130, 246);
            rjTextBox3.BorderRadius = 7;
            rjTextBox3.BorderSize = 1;
            rjTextBox3.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rjTextBox3.ForeColor = Color.White;
            rjTextBox3.Location = new Point(7, 101);
            rjTextBox3.Margin = new Padding(4);
            rjTextBox3.Multiline = false;
            rjTextBox3.Name = "rjTextBox3";
            rjTextBox3.Padding = new Padding(10, 7, 10, 7);
            rjTextBox3.PasswordChar = false;
            rjTextBox3.PlaceholderColor = Color.DarkGray;
            rjTextBox3.PlaceholderText = "Serwer";
            rjTextBox3.Size = new Size(394, 31);
            rjTextBox3.TabIndex = 7;
            rjTextBox3.TabStop = false;
            rjTextBox3.Texts = "";
            rjTextBox3.UnderlinedStyle = false;
            // 
            // rjTextBox2
            // 
            rjTextBox2.BackColor = Color.FromArgb(40, 42, 46);
            rjTextBox2.BorderColor = Color.FromArgb(64, 64, 64);
            rjTextBox2.BorderFocusColor = Color.FromArgb(59, 130, 246);
            rjTextBox2.BorderRadius = 7;
            rjTextBox2.BorderSize = 1;
            rjTextBox2.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rjTextBox2.ForeColor = Color.White;
            rjTextBox2.Location = new Point(7, 62);
            rjTextBox2.Margin = new Padding(4);
            rjTextBox2.Multiline = false;
            rjTextBox2.Name = "rjTextBox2";
            rjTextBox2.Padding = new Padding(10, 7, 10, 7);
            rjTextBox2.PasswordChar = true;
            rjTextBox2.PlaceholderColor = Color.DarkGray;
            rjTextBox2.PlaceholderText = "Hasło";
            rjTextBox2.Size = new Size(394, 31);
            rjTextBox2.TabIndex = 6;
            rjTextBox2.TabStop = false;
            rjTextBox2.Texts = "";
            rjTextBox2.UnderlinedStyle = false;
            // 
            // rjTextBox1
            // 
            rjTextBox1.BackColor = Color.FromArgb(40, 42, 46);
            rjTextBox1.BorderColor = Color.FromArgb(64, 64, 64);
            rjTextBox1.BorderFocusColor = Color.FromArgb(59, 130, 246);
            rjTextBox1.BorderRadius = 7;
            rjTextBox1.BorderSize = 1;
            rjTextBox1.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rjTextBox1.ForeColor = Color.White;
            rjTextBox1.Location = new Point(7, 23);
            rjTextBox1.Margin = new Padding(4);
            rjTextBox1.Multiline = false;
            rjTextBox1.Name = "rjTextBox1";
            rjTextBox1.Padding = new Padding(10, 7, 10, 7);
            rjTextBox1.PasswordChar = false;
            rjTextBox1.PlaceholderColor = Color.DarkGray;
            rjTextBox1.PlaceholderText = "Email";
            rjTextBox1.Size = new Size(394, 31);
            rjTextBox1.TabIndex = 5;
            rjTextBox1.TabStop = false;
            rjTextBox1.Texts = "";
            rjTextBox1.UnderlinedStyle = false;
            // 
            // borderButton3
            // 
            borderButton3.BackColor = Color.FromArgb(40, 42, 46);
            borderButton3.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            borderButton3.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            borderButton3.BorderColor = Color.FromArgb(64, 64, 64);
            borderButton3.BorderColorMouseDown = Color.FromArgb(64, 64, 64);
            borderButton3.BorderColorMouseOver = Color.FromArgb(64, 64, 64);
            borderButton3.BorderRadius = 7;
            borderButton3.BorderWidth = 1;
            borderButton3.ForeColor = Color.DarkGray;
            borderButton3.Location = new Point(508, 468);
            borderButton3.Name = "borderButton3";
            borderButton3.Size = new Size(205, 39);
            borderButton3.TabIndex = 9;
            borderButton3.Text = "Start";
            borderButton3.Click += borderButton3_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(groupBox9);
            groupBox2.Controls.Add(checkBox17);
            groupBox2.Controls.Add(checkBox14);
            groupBox2.Controls.Add(groupBox8);
            groupBox2.Controls.Add(groupBox7);
            groupBox2.Controls.Add(groupBox6);
            groupBox2.Controls.Add(groupBox5);
            groupBox2.Controls.Add(groupBox4);
            groupBox2.Controls.Add(groupBox3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(numericUpDown1);
            groupBox2.Controls.Add(checkBox1);
            groupBox2.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox2.Location = new Point(12, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(490, 545);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Ustawienia bota (Wszystkie konta)";
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(checkBox21);
            groupBox9.Controls.Add(checkBox20);
            groupBox9.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox9.Location = new Point(212, 388);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(200, 71);
            groupBox9.TabIndex = 17;
            groupBox9.TabStop = false;
            groupBox9.Text = "Łotr";
            // 
            // checkBox21
            // 
            checkBox21.AutoSize = true;
            checkBox21.Location = new Point(6, 45);
            checkBox21.Name = "checkBox21";
            checkBox21.Size = new Size(167, 19);
            checkBox21.TabIndex = 1;
            checkBox21.Text = "placeholder Uzywaj baterie";
            checkBox21.UseVisualStyleBackColor = true;
            // 
            // checkBox20
            // 
            checkBox20.AutoSize = true;
            checkBox20.Location = new Point(6, 22);
            checkBox20.Name = "checkBox20";
            checkBox20.Size = new Size(87, 19);
            checkBox20.TabIndex = 0;
            checkBox20.Text = "Atakuj lotra";
            checkBox20.UseVisualStyleBackColor = true;
            checkBox20.CheckedChanged += checkBox20_CheckedChanged;
            // 
            // checkBox17
            // 
            checkBox17.AutoSize = true;
            checkBox17.Location = new Point(245, 47);
            checkBox17.Name = "checkBox17";
            checkBox17.Size = new Size(161, 19);
            checkBox17.TabIndex = 16;
            checkBox17.Text = "Akceptuj prosby o baterie";
            checkBox17.UseVisualStyleBackColor = true;
            checkBox17.CheckedChanged += checkBox17_CheckedChanged;
            // 
            // checkBox14
            // 
            checkBox14.AutoSize = true;
            checkBox14.Location = new Point(245, 22);
            checkBox14.Name = "checkBox14";
            checkBox14.Size = new Size(190, 19);
            checkBox14.TabIndex = 15;
            checkBox14.Text = "Odbieraj nagrody za logowanie";
            checkBox14.UseVisualStyleBackColor = true;
            checkBox14.CheckedChanged += checkBox14_CheckedChanged;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(checkBox15);
            groupBox8.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox8.Location = new Point(212, 94);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(200, 52);
            groupBox8.TabIndex = 14;
            groupBox8.TabStop = false;
            groupBox8.Text = "Krety";
            // 
            // checkBox15
            // 
            checkBox15.AutoSize = true;
            checkBox15.Location = new Point(6, 22);
            checkBox15.Name = "checkBox15";
            checkBox15.Size = new Size(107, 19);
            checkBox15.TabIndex = 0;
            checkBox15.Text = "Odbieraj lopaty";
            checkBox15.UseVisualStyleBackColor = true;
            checkBox15.CheckedChanged += checkBox15_CheckedChanged;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(checkBox13);
            groupBox7.Controls.Add(checkBox12);
            groupBox7.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox7.Location = new Point(6, 458);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(200, 71);
            groupBox7.TabIndex = 13;
            groupBox7.TabStop = false;
            groupBox7.Text = "Kryjowka";
            // 
            // checkBox13
            // 
            checkBox13.AutoSize = true;
            checkBox13.Location = new Point(6, 47);
            checkBox13.Name = "checkBox13";
            checkBox13.Size = new Size(151, 19);
            checkBox13.TabIndex = 1;
            checkBox13.Text = "Odbieraj pomieszczenia";
            checkBox13.UseVisualStyleBackColor = true;
            checkBox13.CheckedChanged += checkBox13_CheckedChanged;
            // 
            // checkBox12
            // 
            checkBox12.AutoSize = true;
            checkBox12.ForeColor = Color.FromArgb(64, 64, 64);
            checkBox12.Location = new Point(6, 22);
            checkBox12.Name = "checkBox12";
            checkBox12.Size = new Size(168, 19);
            checkBox12.TabIndex = 0;
            checkBox12.Text = "Rob pojedynki kryjowkowe";
            checkBox12.UseVisualStyleBackColor = true;
            checkBox12.CheckedChanged += checkBox12_CheckedChanged;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(checkBox23);
            groupBox6.Controls.Add(label4);
            groupBox6.Controls.Add(numericUpDown3);
            groupBox6.Controls.Add(checkBox16);
            groupBox6.Controls.Add(checkBox11);
            groupBox6.Controls.Add(label3);
            groupBox6.Controls.Add(numericUpDown2);
            groupBox6.Controls.Add(radioButton2);
            groupBox6.Controls.Add(radioButton1);
            groupBox6.Controls.Add(checkBox10);
            groupBox6.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox6.Location = new Point(212, 152);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(200, 236);
            groupBox6.TabIndex = 12;
            groupBox6.TabStop = false;
            groupBox6.Text = "Misje";
            groupBox6.Enter += groupBox6_Enter;
            // 
            // checkBox23
            // 
            checkBox23.AutoSize = true;
            checkBox23.Location = new Point(6, 211);
            checkBox23.Name = "checkBox23";
            checkBox23.Size = new Size(179, 19);
            checkBox23.TabIndex = 9;
            checkBox23.Text = "placeholder Odbieraj kupony";
            checkBox23.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 139);
            label4.Name = "label4";
            label4.Size = new Size(156, 15);
            label4.TabIndex = 8;
            label4.Text = "Minimalne ratio (exp / gold)";
            // 
            // numericUpDown3
            // 
            numericUpDown3.BackColor = Color.FromArgb(64, 64, 64);
            numericUpDown3.Location = new Point(6, 157);
            numericUpDown3.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(120, 23);
            numericUpDown3.TabIndex = 7;
            numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
            // 
            // checkBox16
            // 
            checkBox16.AutoSize = true;
            checkBox16.Location = new Point(6, 186);
            checkBox16.Name = "checkBox16";
            checkBox16.Size = new Size(186, 19);
            checkBox16.TabIndex = 6;
            checkBox16.Text = "Wybieraj tylko misje eventowe";
            checkBox16.UseVisualStyleBackColor = true;
            checkBox16.CheckedChanged += checkBox16_CheckedChanged;
            // 
            // checkBox11
            // 
            checkBox11.AutoSize = true;
            checkBox11.Location = new Point(6, 116);
            checkBox11.Name = "checkBox11";
            checkBox11.Size = new Size(149, 19);
            checkBox11.TabIndex = 5;
            checkBox11.Text = "wykonuj misje czasowe";
            checkBox11.UseVisualStyleBackColor = true;
            checkBox11.CheckedChanged += checkBox11_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 69);
            label3.Name = "label3";
            label3.Size = new Size(116, 15);
            label3.TabIndex = 4;
            label3.Text = "Maksymalna energia";
            // 
            // numericUpDown2
            // 
            numericUpDown2.BackColor = Color.FromArgb(64, 64, 64);
            numericUpDown2.Location = new Point(6, 87);
            numericUpDown2.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(120, 23);
            numericUpDown2.TabIndex = 3;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(100, 47);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(79, 19);
            radioButton2.TabIndex = 2;
            radioButton2.TabStop = true;
            radioButton2.Text = "Pod golda";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(6, 47);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(74, 19);
            radioButton1.TabIndex = 1;
            radioButton1.TabStop = true;
            radioButton1.Text = "Pod expa";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // checkBox10
            // 
            checkBox10.AutoSize = true;
            checkBox10.Location = new Point(6, 22);
            checkBox10.Name = "checkBox10";
            checkBox10.Size = new Size(78, 19);
            checkBox10.TabIndex = 0;
            checkBox10.Text = "Rob misje";
            checkBox10.UseVisualStyleBackColor = true;
            checkBox10.CheckedChanged += checkBox10_CheckedChanged;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(checkBox19);
            groupBox5.Controls.Add(checkBox9);
            groupBox5.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox5.Location = new Point(6, 379);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(200, 73);
            groupBox5.TabIndex = 11;
            groupBox5.TabStop = false;
            groupBox5.Text = "Liga";
            // 
            // checkBox19
            // 
            checkBox19.AutoSize = true;
            checkBox19.Location = new Point(6, 47);
            checkBox19.Name = "checkBox19";
            checkBox19.Size = new Size(223, 19);
            checkBox19.TabIndex = 2;
            checkBox19.Text = "placeholder Atakuj czlonkow druzyny";
            checkBox19.UseVisualStyleBackColor = true;
            checkBox19.CheckedChanged += checkBox19_CheckedChanged;
            // 
            // checkBox9
            // 
            checkBox9.AutoSize = true;
            checkBox9.Location = new Point(6, 22);
            checkBox9.Name = "checkBox9";
            checkBox9.Size = new Size(140, 19);
            checkBox9.TabIndex = 0;
            checkBox9.Text = "Rob pojedynki ligowe";
            checkBox9.UseVisualStyleBackColor = true;
            checkBox9.CheckedChanged += checkBox9_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(checkBox18);
            groupBox4.Controls.Add(checkBox8);
            groupBox4.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox4.Location = new Point(6, 298);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(200, 75);
            groupBox4.TabIndex = 10;
            groupBox4.TabStop = false;
            groupBox4.Text = "Pojedynki";
            // 
            // checkBox18
            // 
            checkBox18.AutoSize = true;
            checkBox18.Location = new Point(6, 45);
            checkBox18.Name = "checkBox18";
            checkBox18.Size = new Size(223, 19);
            checkBox18.TabIndex = 1;
            checkBox18.Text = "placeholder Atakuj czlonkow druzyny";
            checkBox18.UseVisualStyleBackColor = true;
            checkBox18.CheckedChanged += checkBox18_CheckedChanged;
            // 
            // checkBox8
            // 
            checkBox8.AutoSize = true;
            checkBox8.Location = new Point(6, 22);
            checkBox8.Name = "checkBox8";
            checkBox8.Size = new Size(102, 19);
            checkBox8.TabIndex = 0;
            checkBox8.Text = "Rob pojedynki";
            checkBox8.UseVisualStyleBackColor = true;
            checkBox8.CheckedChanged += checkBox8_CheckedChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(checkBox22);
            groupBox3.Controls.Add(checkBox2);
            groupBox3.Controls.Add(checkBox7);
            groupBox3.Controls.Add(checkBox3);
            groupBox3.Controls.Add(checkBox6);
            groupBox3.Controls.Add(checkBox4);
            groupBox3.Controls.Add(checkBox5);
            groupBox3.ForeColor = Color.FromArgb(64, 64, 64);
            groupBox3.Location = new Point(6, 94);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(200, 198);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "Trening";
            // 
            // checkBox22
            // 
            checkBox22.AutoSize = true;
            checkBox22.Location = new Point(6, 172);
            checkBox22.Name = "checkBox22";
            checkBox22.Size = new Size(114, 19);
            checkBox22.TabIndex = 9;
            checkBox22.Text = "Odbieraj kupony";
            checkBox22.UseVisualStyleBackColor = true;
            checkBox22.CheckedChanged += checkBox22_CheckedChanged;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(6, 22);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(91, 19);
            checkBox2.TabIndex = 3;
            checkBox2.Text = "Rob treningi";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // checkBox7
            // 
            checkBox7.AutoSize = true;
            checkBox7.Location = new Point(6, 147);
            checkBox7.Name = "checkBox7";
            checkBox7.Size = new Size(147, 19);
            checkBox7.TabIndex = 8;
            checkBox7.Text = "Rob najkrotsze treningi";
            checkBox7.UseVisualStyleBackColor = true;
            checkBox7.CheckedChanged += checkBox7_CheckedChanged;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Location = new Point(6, 47);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(101, 19);
            checkBox3.TabIndex = 4;
            checkBox3.Text = "Trening na sile";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // checkBox6
            // 
            checkBox6.AutoSize = true;
            checkBox6.Location = new Point(6, 122);
            checkBox6.Name = "checkBox6";
            checkBox6.Size = new Size(145, 19);
            checkBox6.TabIndex = 7;
            checkBox6.Text = "Trening na inteligencje";
            checkBox6.UseVisualStyleBackColor = true;
            checkBox6.CheckedChanged += checkBox6_CheckedChanged;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(6, 72);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(156, 19);
            checkBox4.TabIndex = 5;
            checkBox4.Text = "Trening na wytrzymalosc";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Location = new Point(6, 97);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(107, 19);
            checkBox5.TabIndex = 6;
            checkBox5.Text = "Trening na unik";
            checkBox5.UseVisualStyleBackColor = true;
            checkBox5.CheckedChanged += checkBox5_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 47);
            label2.Name = "label2";
            label2.Size = new Size(228, 15);
            label2.TabIndex = 2;
            label2.Text = "Czas do ponownego logowania (sekundy)";
            // 
            // numericUpDown1
            // 
            numericUpDown1.BackColor = Color.FromArgb(64, 64, 64);
            numericUpDown1.Location = new Point(6, 65);
            numericUpDown1.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(120, 23);
            numericUpDown1.TabIndex = 1;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(6, 22);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(233, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Wylacz bota przy pracach technicznych";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10000;
            timer1.Tick += timer1_Tick;
            // 
            // BackendForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 17, 18);
            ClientSize = new Size(1079, 569);
            Controls.Add(groupBox2);
            Controls.Add(borderButton3);
            Controls.Add(groupBox1);
            Name = "BackendForm";
            Text = "BackendForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Material.BorderTextBox rjTextBox3;
        private Material.BorderTextBox rjTextBox2;
        private Material.BorderTextBox rjTextBox1;
        private Design.BorderButton borderButton1;
        private Design.BorderButton borderButton2;
        private Design.BorderButton borderButton3;
        private GroupBox groupBox2;
        private CheckBox checkBox1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox6;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private CheckBox checkBox7;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private CheckBox checkBox8;
        private GroupBox groupBox5;
        private CheckBox checkBox9;
        private GroupBox groupBox6;
        private CheckBox checkBox10;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label3;
        private NumericUpDown numericUpDown2;
        private CheckBox checkBox11;
        private GroupBox groupBox7;
        private CheckBox checkBox12;
        private CheckBox checkBox13;
        private GroupBox groupBox8;
        private CheckBox checkBox15;
        private CheckBox checkBox14;
        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private CheckBox checkBox16;
        private CheckBox checkBox17;
        private Label label4;
        private NumericUpDown numericUpDown3;
        private CheckBox checkBox18;
        private CheckBox checkBox19;
        private GroupBox groupBox9;
        private CheckBox checkBox20;
        private CheckBox checkBox21;
        private CheckBox checkBox22;
        private CheckBox checkBox23;
    }
}