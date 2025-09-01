namespace HeroEngine.Desktop.Interface
{
    partial class FrontendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrontendForm));
            borderTextBoxUser = new Material.BorderTextBox();
            borderTextBoxPassword = new Material.BorderTextBox();
            borderPanelMain = new Design.BorderPanel();
            labelNotRegistered = new Label();
            checkBoxRemember = new Design.CheckBox();
            labelResponse = new Label();
            buttonLogin = new Design.Button();
            buttonClose = new Design.Button();
            panelTop = new Design.Panel();
            buttonMinimize = new Design.Button();
            buttonSettings = new Design.Button();
            borderPanelMenu = new Design.BorderPanel();
            checkBoxSaveLogs = new Design.CheckBox();
            buttonThemeSettings = new Design.Button();
            dropDownMenuTheme = new Design.DropDownMenu();
            dropDownMenuLanguage = new Design.DropDownMenu();
            buttonMenuBack = new Design.Button();
            borderPanelThemeEditor = new Design.BorderPanel();
            textBox1 = new TextBox();
            buttonThemeBack = new Design.Button();
            colorPictureBoxLogo = new Design.ColorPictureBox();
            borderPanelRegister = new Design.BorderPanel();
            buttonRegisterBack = new Design.Button();
            checkBoxRegisterRemember = new Design.CheckBox();
            borderTextBoxRegisterUsername = new Material.BorderTextBox();
            labelResponseRegister = new Label();
            buttonRegister = new Design.Button();
            borderTextBoxRegisterPassword = new Material.BorderTextBox();
            borderTextBoxRegisterLicense = new Material.BorderTextBox();
            borderPanelMain.SuspendLayout();
            panelTop.SuspendLayout();
            borderPanelMenu.SuspendLayout();
            borderPanelThemeEditor.SuspendLayout();
            borderPanelRegister.SuspendLayout();
            SuspendLayout();
            // 
            // borderTextBoxUser
            // 
            borderTextBoxUser.BackColor = Color.FromArgb(40, 42, 46);
            borderTextBoxUser.BorderColor = Color.FromArgb(64, 64, 64);
            borderTextBoxUser.BorderFocusColor = Color.FromArgb(235, 155, 0);
            borderTextBoxUser.BorderRadius = 7;
            borderTextBoxUser.BorderSize = 1;
            borderTextBoxUser.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            borderTextBoxUser.ForeColor = Color.White;
            borderTextBoxUser.Location = new Point(52, 28);
            borderTextBoxUser.Margin = new Padding(4);
            borderTextBoxUser.Multiline = false;
            borderTextBoxUser.Name = "borderTextBoxUser";
            borderTextBoxUser.Padding = new Padding(10, 7, 10, 7);
            borderTextBoxUser.PasswordChar = false;
            borderTextBoxUser.PlaceholderColor = Color.DarkGray;
            borderTextBoxUser.PlaceholderText = "Użytkownik";
            borderTextBoxUser.Size = new Size(394, 31);
            borderTextBoxUser.TabIndex = 4;
            borderTextBoxUser.TabStop = false;
            borderTextBoxUser.Texts = "";
            borderTextBoxUser.UnderlinedStyle = false;
            borderTextBoxUser._TextChanged += borderTextBoxUser__TextChanged;
            // 
            // borderTextBoxPassword
            // 
            borderTextBoxPassword.BackColor = Color.FromArgb(40, 42, 46);
            borderTextBoxPassword.BorderColor = Color.FromArgb(64, 64, 64);
            borderTextBoxPassword.BorderFocusColor = Color.FromArgb(235, 155, 0);
            borderTextBoxPassword.BorderRadius = 7;
            borderTextBoxPassword.BorderSize = 1;
            borderTextBoxPassword.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            borderTextBoxPassword.ForeColor = Color.White;
            borderTextBoxPassword.Location = new Point(52, 67);
            borderTextBoxPassword.Margin = new Padding(4);
            borderTextBoxPassword.Multiline = false;
            borderTextBoxPassword.Name = "borderTextBoxPassword";
            borderTextBoxPassword.Padding = new Padding(10, 7, 10, 7);
            borderTextBoxPassword.PasswordChar = true;
            borderTextBoxPassword.PlaceholderColor = Color.DarkGray;
            borderTextBoxPassword.PlaceholderText = "Hasło";
            borderTextBoxPassword.Size = new Size(394, 31);
            borderTextBoxPassword.TabIndex = 5;
            borderTextBoxPassword.TabStop = false;
            borderTextBoxPassword.Texts = "";
            borderTextBoxPassword.UnderlinedStyle = false;
            borderTextBoxPassword._TextChanged += borderTextBoxPassword__TextChanged;
            // 
            // borderPanelMain
            // 
            borderPanelMain.BackColor = Color.FromArgb(27, 28, 30);
            borderPanelMain.BorderColor = Color.FromArgb(47, 48, 50);
            borderPanelMain.BorderRadius = 7;
            borderPanelMain.BorderRadiusSides = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            borderPanelMain.BorderWidth = 1;
            borderPanelMain.Controls.Add(labelNotRegistered);
            borderPanelMain.Controls.Add(checkBoxRemember);
            borderPanelMain.Controls.Add(labelResponse);
            borderPanelMain.Controls.Add(buttonLogin);
            borderPanelMain.Controls.Add(borderTextBoxPassword);
            borderPanelMain.Controls.Add(borderTextBoxUser);
            borderPanelMain.Location = new Point(150, 151);
            borderPanelMain.Name = "borderPanelMain";
            borderPanelMain.Size = new Size(499, 184);
            borderPanelMain.TabIndex = 8;
            // 
            // labelNotRegistered
            // 
            labelNotRegistered.AutoSize = true;
            labelNotRegistered.Font = new Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point, 0);
            labelNotRegistered.ForeColor = Color.White;
            labelNotRegistered.Location = new Point(357, 102);
            labelNotRegistered.Name = "labelNotRegistered";
            labelNotRegistered.Size = new Size(89, 15);
            labelNotRegistered.TabIndex = 65;
            labelNotRegistered.Text = "Nie mam konta";
            labelNotRegistered.Click += labelNotRegistered_Click;
            // 
            // checkBoxRemember
            // 
            checkBoxRemember.BackColor = Color.FromArgb(40, 42, 46);
            checkBoxRemember.BackColorChecked = Color.FromArgb(40, 42, 46);
            checkBoxRemember.BorderRadius = 4;
            checkBoxRemember.Checked = false;
            checkBoxRemember.CheckMarkColor = Color.FromArgb(255, 175, 0);
            checkBoxRemember.CheckMarkSize = 2;
            checkBoxRemember.ForeColor = Color.White;
            checkBoxRemember.Location = new Point(52, 105);
            checkBoxRemember.Name = "checkBoxRemember";
            checkBoxRemember.Size = new Size(150, 20);
            checkBoxRemember.TabIndex = 64;
            checkBoxRemember.Text = "Zapamietaj mnie";
            checkBoxRemember._CheckedChanged += checkBoxRemember__CheckedChanged;
            // 
            // labelResponse
            // 
            labelResponse.AutoSize = true;
            labelResponse.ForeColor = Color.White;
            labelResponse.Location = new Point(52, 161);
            labelResponse.Name = "labelResponse";
            labelResponse.Size = new Size(93, 15);
            labelResponse.TabIndex = 17;
            labelResponse.Text = "HeroEngine Text";
            labelResponse.Visible = false;
            // 
            // buttonLogin
            // 
            buttonLogin.BackColor = Color.FromArgb(235, 155, 0);
            buttonLogin.BackColorMouseDown = Color.FromArgb(225, 145, 0);
            buttonLogin.BackColorMouseOver = Color.FromArgb(225, 145, 0);
            buttonLogin.BackImageLayout = ImageLayout.Stretch;
            buttonLogin.BorderRadius = 7;
            buttonLogin.Font = new Font("Verdana", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonLogin.ForeColor = Color.White;
            buttonLogin.Location = new Point(296, 124);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(150, 30);
            buttonLogin.TabIndex = 14;
            buttonLogin.Text = "Zaloguj";
            buttonLogin.Click += buttonLogin_Click;
            // 
            // buttonClose
            // 
            buttonClose.BackColor = Color.FromArgb(27, 28, 30);
            buttonClose.BackColorMouseDown = Color.FromArgb(200, 0, 0);
            buttonClose.BackColorMouseOver = Color.FromArgb(250, 0, 0);
            buttonClose.BackImage = Properties.Resources.chrome_close_dark;
            buttonClose.BackImageLayout = ImageLayout.Center;
            buttonClose.BorderRadius = 0;
            buttonClose.Dock = DockStyle.Right;
            buttonClose.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonClose.ForeColor = Color.White;
            buttonClose.Location = new Point(756, 0);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(44, 36);
            buttonClose.TabIndex = 12;
            buttonClose.Click += buttonClose_Click;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.FromArgb(16, 17, 18);
            panelTop.BorderColor = Color.FromArgb(16, 17, 18);
            panelTop.BorderRadius = 0;
            panelTop.Controls.Add(buttonMinimize);
            panelTop.Controls.Add(buttonClose);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(800, 36);
            panelTop.TabIndex = 13;
            // 
            // buttonMinimize
            // 
            buttonMinimize.BackColor = Color.FromArgb(27, 28, 30);
            buttonMinimize.BackColorMouseDown = Color.FromArgb(47, 48, 50);
            buttonMinimize.BackColorMouseOver = Color.FromArgb(37, 38, 40);
            buttonMinimize.BackImage = Properties.Resources.chrome_minimize_dark;
            buttonMinimize.BackImageLayout = ImageLayout.Center;
            buttonMinimize.BorderRadius = 0;
            buttonMinimize.Dock = DockStyle.Right;
            buttonMinimize.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonMinimize.ForeColor = Color.White;
            buttonMinimize.Location = new Point(712, 0);
            buttonMinimize.Name = "buttonMinimize";
            buttonMinimize.Size = new Size(44, 36);
            buttonMinimize.TabIndex = 13;
            buttonMinimize.Click += buttonMinimize_Click;
            // 
            // buttonSettings
            // 
            buttonSettings.BackColor = Color.FromArgb(27, 28, 30);
            buttonSettings.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            buttonSettings.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            buttonSettings.BackImage = Properties.Resources.gear_dark;
            buttonSettings.BackImageLayout = ImageLayout.Center;
            buttonSettings.BorderRadius = 7;
            buttonSettings.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonSettings.ForeColor = Color.White;
            buttonSettings.Location = new Point(619, 115);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(30, 30);
            buttonSettings.TabIndex = 15;
            buttonSettings.Click += buttonSettings_Click;
            // 
            // borderPanelMenu
            // 
            borderPanelMenu.BackColor = Color.FromArgb(27, 28, 30);
            borderPanelMenu.BorderColor = Color.FromArgb(47, 48, 50);
            borderPanelMenu.BorderRadius = 7;
            borderPanelMenu.BorderRadiusSides = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            borderPanelMenu.BorderWidth = 1;
            borderPanelMenu.Controls.Add(checkBoxSaveLogs);
            borderPanelMenu.Controls.Add(buttonThemeSettings);
            borderPanelMenu.Controls.Add(dropDownMenuTheme);
            borderPanelMenu.Controls.Add(dropDownMenuLanguage);
            borderPanelMenu.Controls.Add(buttonMenuBack);
            borderPanelMenu.Location = new Point(36, 358);
            borderPanelMenu.Name = "borderPanelMenu";
            borderPanelMenu.Size = new Size(485, 233);
            borderPanelMenu.TabIndex = 17;
            borderPanelMenu.Visible = false;
            // 
            // checkBoxSaveLogs
            // 
            checkBoxSaveLogs.BackColor = Color.FromArgb(40, 42, 46);
            checkBoxSaveLogs.BackColorChecked = Color.FromArgb(40, 42, 46);
            checkBoxSaveLogs.BorderRadius = 4;
            checkBoxSaveLogs.Checked = false;
            checkBoxSaveLogs.CheckMarkColor = Color.FromArgb(255, 175, 0);
            checkBoxSaveLogs.CheckMarkSize = 3;
            checkBoxSaveLogs.ForeColor = Color.White;
            checkBoxSaveLogs.Location = new Point(6, 112);
            checkBoxSaveLogs.Name = "checkBoxSaveLogs";
            checkBoxSaveLogs.Size = new Size(171, 25);
            checkBoxSaveLogs.TabIndex = 63;
            checkBoxSaveLogs.Text = "Zapisuj logi do pliku";
            checkBoxSaveLogs._CheckedChanged += checkBoxSaveLogs__CheckedChanged;
            // 
            // buttonThemeSettings
            // 
            buttonThemeSettings.BackColor = Color.FromArgb(27, 28, 30);
            buttonThemeSettings.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            buttonThemeSettings.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            buttonThemeSettings.BackImage = Properties.Resources.gear_dark;
            buttonThemeSettings.BackImageLayout = ImageLayout.Center;
            buttonThemeSettings.BorderRadius = 7;
            buttonThemeSettings.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonThemeSettings.ForeColor = Color.White;
            buttonThemeSettings.Location = new Point(183, 76);
            buttonThemeSettings.Name = "buttonThemeSettings";
            buttonThemeSettings.Size = new Size(30, 30);
            buttonThemeSettings.TabIndex = 60;
            buttonThemeSettings.Click += buttonThemeSettings_Click;
            // 
            // dropDownMenuTheme
            // 
            dropDownMenuTheme.BackColor = Color.FromArgb(27, 28, 30);
            dropDownMenuTheme.BackgroundImageLayout = ImageLayout.None;
            dropDownMenuTheme.BorderColor = Color.FromArgb(27, 28, 30);
            dropDownMenuTheme.BorderColorMouseDown = Color.FromArgb(27, 28, 30);
            dropDownMenuTheme.BorderColorMouseOver = Color.FromArgb(27, 28, 30);
            dropDownMenuTheme.BorderRadius = 7;
            dropDownMenuTheme.BorderWidth = 1;
            dropDownMenuTheme.Color = Color.FromArgb(40, 42, 46);
            dropDownMenuTheme.ColorMouseDown = Color.FromArgb(64, 64, 64);
            dropDownMenuTheme.ColorMouseOver = Color.FromArgb(40, 42, 46);
            dropDownMenuTheme.Elements.Add("Ciemny Motyw");
            dropDownMenuTheme.Elements.Add("Jasny Motyw");
            dropDownMenuTheme.Expanded = false;
            dropDownMenuTheme.ForeColor = Color.White;
            dropDownMenuTheme.HeaderHeight = 28;
            dropDownMenuTheme.Location = new Point(8, 78);
            dropDownMenuTheme.MarkSize = 2;
            dropDownMenuTheme.MinimumSize = new Size(0, 28);
            dropDownMenuTheme.Name = "dropDownMenuTheme";
            dropDownMenuTheme.SelectBorderColorMouseDown = Color.FromArgb(235, 155, 0);
            dropDownMenuTheme.SelectBorderColorMouseOver = Color.FromArgb(235, 155, 0);
            dropDownMenuTheme.SelectedBorderColor = Color.FromArgb(235, 155, 0);
            dropDownMenuTheme.SelectionIndex = 0;
            dropDownMenuTheme.Size = new Size(169, 28);
            dropDownMenuTheme.TabIndex = 59;
            dropDownMenuTheme.Text = "dropDownMenu1";
            dropDownMenuTheme.IndexChanged += dropDownMenuTheme_IndexChanged;
            // 
            // dropDownMenuLanguage
            // 
            dropDownMenuLanguage.BackColor = Color.FromArgb(27, 28, 30);
            dropDownMenuLanguage.BackgroundImageLayout = ImageLayout.None;
            dropDownMenuLanguage.BorderColor = Color.FromArgb(27, 28, 30);
            dropDownMenuLanguage.BorderColorMouseDown = Color.FromArgb(27, 28, 30);
            dropDownMenuLanguage.BorderColorMouseOver = Color.FromArgb(27, 28, 30);
            dropDownMenuLanguage.BorderRadius = 7;
            dropDownMenuLanguage.BorderWidth = 1;
            dropDownMenuLanguage.Color = Color.FromArgb(40, 42, 46);
            dropDownMenuLanguage.ColorMouseDown = Color.FromArgb(64, 64, 64);
            dropDownMenuLanguage.ColorMouseOver = Color.FromArgb(40, 42, 46);
            dropDownMenuLanguage.Elements.Add("Polski");
            dropDownMenuLanguage.Elements.Add("English");
            dropDownMenuLanguage.Expanded = false;
            dropDownMenuLanguage.ForeColor = Color.White;
            dropDownMenuLanguage.HeaderHeight = 28;
            dropDownMenuLanguage.Location = new Point(8, 44);
            dropDownMenuLanguage.MarkSize = 2;
            dropDownMenuLanguage.MinimumSize = new Size(0, 28);
            dropDownMenuLanguage.Name = "dropDownMenuLanguage";
            dropDownMenuLanguage.SelectBorderColorMouseDown = Color.FromArgb(235, 155, 0);
            dropDownMenuLanguage.SelectBorderColorMouseOver = Color.FromArgb(235, 155, 0);
            dropDownMenuLanguage.SelectedBorderColor = Color.FromArgb(235, 155, 0);
            dropDownMenuLanguage.SelectionIndex = 0;
            dropDownMenuLanguage.Size = new Size(169, 28);
            dropDownMenuLanguage.TabIndex = 56;
            dropDownMenuLanguage.Text = "dropDownMenu1";
            dropDownMenuLanguage.IndexChanged += dropDownMenuLanguage_IndexChanged;
            // 
            // buttonMenuBack
            // 
            buttonMenuBack.BackColor = Color.FromArgb(27, 28, 30);
            buttonMenuBack.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            buttonMenuBack.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            buttonMenuBack.BackImage = Properties.Resources.arrow_left_dark;
            buttonMenuBack.BackImageLayout = ImageLayout.Center;
            buttonMenuBack.BorderRadius = 7;
            buttonMenuBack.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonMenuBack.ForeColor = Color.White;
            buttonMenuBack.Location = new Point(8, 8);
            buttonMenuBack.Name = "buttonMenuBack";
            buttonMenuBack.Size = new Size(30, 30);
            buttonMenuBack.TabIndex = 16;
            buttonMenuBack.Click += buttonMenuBack_Click;
            // 
            // borderPanelThemeEditor
            // 
            borderPanelThemeEditor.BackColor = Color.FromArgb(27, 28, 30);
            borderPanelThemeEditor.BorderColor = Color.FromArgb(47, 48, 50);
            borderPanelThemeEditor.BorderRadius = 7;
            borderPanelThemeEditor.BorderRadiusSides = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            borderPanelThemeEditor.BorderWidth = 1;
            borderPanelThemeEditor.Controls.Add(textBox1);
            borderPanelThemeEditor.Controls.Add(buttonThemeBack);
            borderPanelThemeEditor.Location = new Point(36, 609);
            borderPanelThemeEditor.Name = "borderPanelThemeEditor";
            borderPanelThemeEditor.Size = new Size(485, 233);
            borderPanelThemeEditor.TabIndex = 18;
            borderPanelThemeEditor.Visible = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(45, 78);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(71, 23);
            textBox1.TabIndex = 17;
            textBox1.Text = "#EB9B00";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // buttonThemeBack
            // 
            buttonThemeBack.BackColor = Color.FromArgb(27, 28, 30);
            buttonThemeBack.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            buttonThemeBack.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            buttonThemeBack.BackImage = Properties.Resources.arrow_left_dark;
            buttonThemeBack.BackImageLayout = ImageLayout.Center;
            buttonThemeBack.BorderRadius = 7;
            buttonThemeBack.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonThemeBack.ForeColor = Color.White;
            buttonThemeBack.Location = new Point(8, 8);
            buttonThemeBack.Name = "buttonThemeBack";
            buttonThemeBack.Size = new Size(30, 30);
            buttonThemeBack.TabIndex = 16;
            buttonThemeBack.Click += buttonThemeBack_Click;
            // 
            // colorPictureBoxLogo
            // 
            colorPictureBoxLogo.BackgroundImageCustom = Properties.Resources.heroengine;
            colorPictureBoxLogo.BackgroundImageLayout = ImageLayout.Center;
            colorPictureBoxLogo.ImageAlignment = ContentAlignment.BottomCenter;
            colorPictureBoxLogo.Location = new Point(360, 53);
            colorPictureBoxLogo.Name = "colorPictureBoxLogo";
            colorPictureBoxLogo.PictureTint = Color.FromArgb(235, 155, 0);
            colorPictureBoxLogo.Size = new Size(80, 80);
            colorPictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            colorPictureBoxLogo.TabIndex = 19;
            // 
            // borderPanelRegister
            // 
            borderPanelRegister.BackColor = Color.FromArgb(27, 28, 30);
            borderPanelRegister.BorderColor = Color.FromArgb(47, 48, 50);
            borderPanelRegister.BorderRadius = 7;
            borderPanelRegister.BorderRadiusSides = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            borderPanelRegister.BorderWidth = 1;
            borderPanelRegister.Controls.Add(buttonRegisterBack);
            borderPanelRegister.Controls.Add(checkBoxRegisterRemember);
            borderPanelRegister.Controls.Add(borderTextBoxRegisterUsername);
            borderPanelRegister.Controls.Add(labelResponseRegister);
            borderPanelRegister.Controls.Add(buttonRegister);
            borderPanelRegister.Controls.Add(borderTextBoxRegisterPassword);
            borderPanelRegister.Controls.Add(borderTextBoxRegisterLicense);
            borderPanelRegister.Location = new Point(150, 848);
            borderPanelRegister.Name = "borderPanelRegister";
            borderPanelRegister.Size = new Size(499, 222);
            borderPanelRegister.TabIndex = 20;
            borderPanelRegister.Visible = false;
            // 
            // buttonRegisterBack
            // 
            buttonRegisterBack.BackColor = Color.FromArgb(27, 28, 30);
            buttonRegisterBack.BackColorMouseDown = Color.FromArgb(40, 42, 46);
            buttonRegisterBack.BackColorMouseOver = Color.FromArgb(40, 42, 46);
            buttonRegisterBack.BackImage = Properties.Resources.arrow_left_dark;
            buttonRegisterBack.BackImageLayout = ImageLayout.Center;
            buttonRegisterBack.BorderRadius = 7;
            buttonRegisterBack.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonRegisterBack.ForeColor = Color.White;
            buttonRegisterBack.Location = new Point(8, 8);
            buttonRegisterBack.Name = "buttonRegisterBack";
            buttonRegisterBack.Size = new Size(30, 30);
            buttonRegisterBack.TabIndex = 66;
            buttonRegisterBack.Click += buttonRegisterBack_Click;
            // 
            // checkBoxRegisterRemember
            // 
            checkBoxRegisterRemember.BackColor = Color.FromArgb(40, 42, 46);
            checkBoxRegisterRemember.BackColorChecked = Color.FromArgb(40, 42, 46);
            checkBoxRegisterRemember.BorderRadius = 4;
            checkBoxRegisterRemember.Checked = false;
            checkBoxRegisterRemember.CheckMarkColor = Color.FromArgb(255, 175, 0);
            checkBoxRegisterRemember.CheckMarkSize = 2;
            checkBoxRegisterRemember.ForeColor = Color.White;
            checkBoxRegisterRemember.Location = new Point(52, 144);
            checkBoxRegisterRemember.Name = "checkBoxRegisterRemember";
            checkBoxRegisterRemember.Size = new Size(150, 20);
            checkBoxRegisterRemember.TabIndex = 65;
            checkBoxRegisterRemember.Text = "Zapamietaj mnie";
            checkBoxRegisterRemember._CheckedChanged += checkBoxRegisterRemember__CheckedChanged;
            // 
            // borderTextBoxRegisterUsername
            // 
            borderTextBoxRegisterUsername.BackColor = Color.FromArgb(40, 42, 46);
            borderTextBoxRegisterUsername.BorderColor = Color.FromArgb(64, 64, 64);
            borderTextBoxRegisterUsername.BorderFocusColor = Color.FromArgb(235, 155, 0);
            borderTextBoxRegisterUsername.BorderRadius = 7;
            borderTextBoxRegisterUsername.BorderSize = 1;
            borderTextBoxRegisterUsername.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            borderTextBoxRegisterUsername.ForeColor = Color.White;
            borderTextBoxRegisterUsername.Location = new Point(52, 67);
            borderTextBoxRegisterUsername.Margin = new Padding(4);
            borderTextBoxRegisterUsername.Multiline = false;
            borderTextBoxRegisterUsername.Name = "borderTextBoxRegisterUsername";
            borderTextBoxRegisterUsername.Padding = new Padding(10, 7, 10, 7);
            borderTextBoxRegisterUsername.PasswordChar = false;
            borderTextBoxRegisterUsername.PlaceholderColor = Color.DarkGray;
            borderTextBoxRegisterUsername.PlaceholderText = "Użytkownik";
            borderTextBoxRegisterUsername.Size = new Size(394, 31);
            borderTextBoxRegisterUsername.TabIndex = 18;
            borderTextBoxRegisterUsername.TabStop = false;
            borderTextBoxRegisterUsername.Texts = "";
            borderTextBoxRegisterUsername.UnderlinedStyle = false;
            // 
            // labelResponseRegister
            // 
            labelResponseRegister.AutoSize = true;
            labelResponseRegister.ForeColor = Color.White;
            labelResponseRegister.Location = new Point(52, 198);
            labelResponseRegister.Name = "labelResponseRegister";
            labelResponseRegister.Size = new Size(93, 15);
            labelResponseRegister.TabIndex = 17;
            labelResponseRegister.Text = "HeroEngine Text";
            labelResponseRegister.Visible = false;
            // 
            // buttonRegister
            // 
            buttonRegister.BackColor = Color.FromArgb(235, 155, 0);
            buttonRegister.BackColorMouseDown = Color.FromArgb(225, 145, 0);
            buttonRegister.BackColorMouseOver = Color.FromArgb(225, 145, 0);
            buttonRegister.BackImageLayout = ImageLayout.Stretch;
            buttonRegister.BorderRadius = 7;
            buttonRegister.Font = new Font("Verdana", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonRegister.ForeColor = Color.White;
            buttonRegister.Location = new Point(296, 162);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(150, 30);
            buttonRegister.TabIndex = 14;
            buttonRegister.Text = "Stwórz";
            buttonRegister.Click += buttonRegister_Click;
            // 
            // borderTextBoxRegisterPassword
            // 
            borderTextBoxRegisterPassword.BackColor = Color.FromArgb(40, 42, 46);
            borderTextBoxRegisterPassword.BorderColor = Color.FromArgb(64, 64, 64);
            borderTextBoxRegisterPassword.BorderFocusColor = Color.FromArgb(235, 155, 0);
            borderTextBoxRegisterPassword.BorderRadius = 7;
            borderTextBoxRegisterPassword.BorderSize = 1;
            borderTextBoxRegisterPassword.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            borderTextBoxRegisterPassword.ForeColor = Color.White;
            borderTextBoxRegisterPassword.Location = new Point(52, 106);
            borderTextBoxRegisterPassword.Margin = new Padding(4);
            borderTextBoxRegisterPassword.Multiline = false;
            borderTextBoxRegisterPassword.Name = "borderTextBoxRegisterPassword";
            borderTextBoxRegisterPassword.Padding = new Padding(10, 7, 10, 7);
            borderTextBoxRegisterPassword.PasswordChar = true;
            borderTextBoxRegisterPassword.PlaceholderColor = Color.DarkGray;
            borderTextBoxRegisterPassword.PlaceholderText = "Hasło";
            borderTextBoxRegisterPassword.Size = new Size(394, 31);
            borderTextBoxRegisterPassword.TabIndex = 5;
            borderTextBoxRegisterPassword.TabStop = false;
            borderTextBoxRegisterPassword.Texts = "";
            borderTextBoxRegisterPassword.UnderlinedStyle = false;
            // 
            // borderTextBoxRegisterLicense
            // 
            borderTextBoxRegisterLicense.BackColor = Color.FromArgb(40, 42, 46);
            borderTextBoxRegisterLicense.BorderColor = Color.FromArgb(64, 64, 64);
            borderTextBoxRegisterLicense.BorderFocusColor = Color.FromArgb(235, 155, 0);
            borderTextBoxRegisterLicense.BorderRadius = 7;
            borderTextBoxRegisterLicense.BorderSize = 1;
            borderTextBoxRegisterLicense.Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            borderTextBoxRegisterLicense.ForeColor = Color.White;
            borderTextBoxRegisterLicense.Location = new Point(52, 28);
            borderTextBoxRegisterLicense.Margin = new Padding(4);
            borderTextBoxRegisterLicense.Multiline = false;
            borderTextBoxRegisterLicense.Name = "borderTextBoxRegisterLicense";
            borderTextBoxRegisterLicense.Padding = new Padding(10, 7, 10, 7);
            borderTextBoxRegisterLicense.PasswordChar = false;
            borderTextBoxRegisterLicense.PlaceholderColor = Color.DarkGray;
            borderTextBoxRegisterLicense.PlaceholderText = "Klucz licencji";
            borderTextBoxRegisterLicense.Size = new Size(394, 31);
            borderTextBoxRegisterLicense.TabIndex = 4;
            borderTextBoxRegisterLicense.TabStop = false;
            borderTextBoxRegisterLicense.Texts = "";
            borderTextBoxRegisterLicense.UnderlinedStyle = false;
            // 
            // FrontendForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 17, 18);
            ClientSize = new Size(800, 1104);
            Controls.Add(borderPanelRegister);
            Controls.Add(colorPictureBoxLogo);
            Controls.Add(borderPanelThemeEditor);
            Controls.Add(borderPanelMenu);
            Controls.Add(buttonSettings);
            Controls.Add(panelTop);
            Controls.Add(borderPanelMain);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new Size(800, 450);
            Name = "FrontendForm";
            Text = "HeroEngine";
            borderPanelMain.ResumeLayout(false);
            borderPanelMain.PerformLayout();
            panelTop.ResumeLayout(false);
            borderPanelMenu.ResumeLayout(false);
            borderPanelThemeEditor.ResumeLayout(false);
            borderPanelThemeEditor.PerformLayout();
            borderPanelRegister.ResumeLayout(false);
            borderPanelRegister.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Material.BorderTextBox borderTextBoxUser;
        private Material.BorderTextBox borderTextBoxPassword;
        private Design.BorderPanel borderPanelMain;
        private Design.Button buttonClose;
        private Design.Panel panelTop;
        private Design.Button buttonLogin;
        private Design.Button buttonMinimize;
        private Design.Button buttonSettings;
        private Label labelResponse;
        private Design.BorderPanel borderPanelMenu;
        private Design.Button buttonMenuBack;
        private Design.DropDownMenu dropDownMenuLanguage;
        private Design.DropDownMenu dropDownMenuTheme;
        private Design.Button buttonThemeSettings;
        private Design.BorderPanel borderPanelThemeEditor;
        private Design.Button buttonThemeBack;
        private TextBox textBox1;
        private Design.CheckBox checkBoxSaveLogs;
        private Design.CheckBox checkBoxRemember;
        private Design.ColorPictureBox colorPictureBoxLogo;
        private Label labelNotRegistered;
        private Design.BorderPanel borderPanelRegister;
        private Material.BorderTextBox borderTextBoxRegisterUsername;
        private Label labelResponseRegister;
        private Design.Button buttonRegister;
        private Material.BorderTextBox borderTextBoxRegisterPassword;
        private Material.BorderTextBox borderTextBoxRegisterLicense;
        private Design.CheckBox checkBoxRegisterRemember;
        private Design.Button buttonRegisterBack;
    }
}