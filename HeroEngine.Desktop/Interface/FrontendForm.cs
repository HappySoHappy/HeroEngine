using HeroEngine.Desktop.Design;
using HeroEngine.Desktop.Properties;
using HeroEngine.Persistance;
using HeroEngine.Util;
using System.Drawing;
using static HeroEngine.Persistance.ExecutionConfiguration;

namespace HeroEngine.Desktop.Interface
{
    public partial class FrontendForm : Form, IThemeable, ITranslatable
    {
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private bool _isDragging = false;
        private Point _startPoint = new Point(0, 0);
        public bool Success = false;
        public License? License = null;
        public bool _sendingLicense = false;

        public FrontendForm()
        {
            InitializeComponent();
            Size = new Size(800, 450);

#if DEBUG
            ConsoleWindow.ShowConsole();
#else
            ConsoleWindow.KillConsole();
#endif

            IThemeable.Subscribe(this);
            ITranslatable.Subscribe(this);
            ((IThemeable)this).OnThemeChanged(Program.Settings!.Theme);
            ((ITranslatable)this).OnLanguageChanged(Program.Settings!.Language);

            panelTop.MouseDown += controlDraggableMouseDown!;
            panelTop.MouseMove += controlDraggableMouseMove!;
            panelTop.MouseUp += controlDraggableMouseUp!;
            panelTop.MouseCaptureChanged += controlDraggableMouseCaptureChanged!;

            UpdateControls();

            textBox1.Text = ColorTranslator.ToHtml(Program.Settings.Highlight);
        }

        public void ShowLicenseStatusLabel(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { ShowLicenseStatusLabel(message); }));
                return;
            }

            Size textSize = TextRenderer.MeasureText(message, labelResponse.Font);

            int availableWidth = labelResponse.Parent!.ClientSize.Width - 40;

            while (textSize.Width > availableWidth && labelResponse.Font.Size > 5)
            {
                labelResponse.Font = new Font(labelResponse.Font.FontFamily, labelResponse.Font.Size - 0.5f);
                textSize = TextRenderer.MeasureText(message, labelResponse.Font);
            }

            labelResponse.Left = (labelResponse.Parent.ClientSize.Width - textSize.Width) / 2;
            labelResponse.Text = message;
            labelResponse.Visible = true;
        }

        public void HideLicenseStatusLabel()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { HideLicenseStatusLabel(); }));
                return;
            }

            labelResponse.Visible = false;
        }

        #region controls
        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                Application.Exit();
            }
        }

        private void buttonMinimize_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void controlDraggableMouseDown(object sender, MouseEventArgs e)
        {
            _isDragging = true;
            _startPoint = new Point(e.X, e.Y);
        }

        private void controlDraggableMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            Point newPoint = PointToScreen(new Point(e.X, e.Y));
            Location = new Point(newPoint.X - _startPoint.X, newPoint.Y - _startPoint.Y);
        }

        private void controlDraggableMouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        private void controlDraggableMouseCaptureChanged(object? sender, EventArgs e)
        {
            _isDragging = false;
        }


        private void buttonSettings_Click(object sender, EventArgs e)
        {
            buttonSettings.Visible = false;
            borderPanelMain.Visible = false;
            borderPanelThemeEditor.Visible = false;
            borderPanelRegister.Visible = false;

            colorPictureBoxLogo.Location = new Point(360, 41);
            borderPanelMenu.Location = new Point(158, 127);
            borderPanelMenu.Visible = true;
        }

        private void borderTextBoxUser__TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(borderTextBoxUser.Texts) && string.IsNullOrEmpty(borderTextBoxPassword.Texts))
            {
                labelNotRegistered.Visible = true;
            }
            else
            {
                //labelNotRegistered.Visible = false;
            }
        }

        private void borderTextBoxPassword__TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(borderTextBoxUser.Texts) && string.IsNullOrEmpty(borderTextBoxPassword.Texts))
            {
                labelNotRegistered.Visible = true;
            }
            else
            {
                //labelNotRegistered.Visible = false;
            }
        }

        private void checkBoxRemember__CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.Remember = checkBoxRemember.Checked;
            checkBoxRegisterRemember.Checked = checkBoxRemember.Checked;

            Program.SaveConfiguration();
        }

        private void checkBoxRegisterRemember__CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.Remember = checkBoxRegisterRemember.Checked;
            checkBoxRemember.Checked = checkBoxRegisterRemember.Checked;

            Program.SaveConfiguration();
        }

        private void buttonMenuBack_Click(object sender, EventArgs e)
        {
            borderPanelMenu.Visible = false;
            borderPanelThemeEditor.Visible = false;
            borderPanelRegister.Visible = false;

            colorPictureBoxLogo.Location = new Point(360, 53);
            buttonSettings.Visible = true;

            borderPanelMain.Location = new Point(150, 151);
            borderPanelMain.Visible = true;
        }

        private void buttonThemeSettings_Click(object sender, EventArgs e)
        {
            borderPanelMenu.Visible = false;
            buttonSettings.Visible = false;
            borderPanelMain.Visible = false;
            borderPanelRegister.Visible = false;

            colorPictureBoxLogo.Location = new Point(360, 41);
            borderPanelThemeEditor.Location = new Point(158, 127);
            borderPanelThemeEditor.Visible = true;
        }

        private void buttonThemeBack_Click(object sender, EventArgs e)
        {
            buttonSettings.Visible = false;
            borderPanelMain.Visible = false;
            borderPanelThemeEditor.Visible = false;
            borderPanelRegister.Visible = false;

            colorPictureBoxLogo.Location = new Point(360, 41);
            borderPanelMenu.Location = new Point(158, 127);
            borderPanelMenu.Visible = true;
        }

        private void labelNotRegistered_Click(object sender, EventArgs e)
        {
            buttonSettings.Visible = false;
            borderPanelMain.Visible = false;
            borderPanelThemeEditor.Visible = false;
            borderPanelMenu.Visible = false;

            colorPictureBoxLogo.Location = new Point(360, 44);
            borderPanelRegister.Location = new Point(150, 132);
            borderPanelRegister.Visible = true;
        }

        private void buttonRegisterBack_Click(object sender, EventArgs e)
        {
            borderPanelMenu.Visible = false;
            borderPanelThemeEditor.Visible = false;
            borderPanelRegister.Visible = false;

            colorPictureBoxLogo.Location = new Point(360, 53);
            buttonSettings.Visible = true;

            borderPanelMain.Location = new Point(150, 151);
            borderPanelMain.Visible = true;
        }

        //its so easy to patch this out, move the license checking to ExecutionCoordinator of its constructor
        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            if (_sendingLicense) return;

            Success = true;
            License = new License() { ExpiryTime = DateTime.Now + TimeSpan.FromDays(31), LicenseKey = "test" };
            Close();
            return;


            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                Success = false;
                License = null;
                buttonLogin.UseWaitCursor = true;
                _sendingLicense = true;

                /*if (!Internet.HasInternetConnection(out string address))
                {
                    ShowLicenseStatusLabel(Program.Settings!.Language.NoInternetConnection);
                    return;
                }*/

                if (string.IsNullOrEmpty(borderTextBoxUser.Texts) || string.IsNullOrEmpty(borderTextBoxPassword.Texts))
                {
                    //give license here
                    buttonLogin.UseWaitCursor = false;
                    _sendingLicense = false;
                    return;
                }

                Program.Settings.Remember = false;
                if (checkBoxRemember.Checked)
                {
                    Program.Settings.User = borderTextBoxUser.Texts;
                    Program.Settings.Password = borderTextBoxPassword.Texts;
                    Program.Settings.Remember = true;
                }

                Program.SaveConfiguration();

                var result = await License.LoginAsync(borderTextBoxUser.Texts, borderTextBoxPassword.Texts);

                buttonLogin.UseWaitCursor = false;
                _sendingLicense = false;
                if (result.Success)
                {
                    Success = true;
                    License = result.License;
                    Close();
                    return;
                }
                else
                {
                    switch (result.Error)
                    {
                        case "expired":
                            ShowLicenseStatusLabel("wygasla");
                            return;

                        case "suspended":
                            ShowLicenseStatusLabel("Licencja zawieszona");
                            return;

                        case "rejected":
                        case "not_found":
                            ShowLicenseStatusLabel(Program.Settings!.Language.LicenseIncorrectCreds);
                            return;

                        default:
                        case "connection":
                            ShowLicenseStatusLabel("Blad polaczenia");
                            return;
                    }
                }
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            borderPanelMenu.Visible = false;
            borderPanelThemeEditor.Visible = false;
            borderPanelRegister.Visible = false;

            colorPictureBoxLogo.Location = new Point(360, 53);
            buttonSettings.Visible = true;

            borderPanelMain.Location = new Point(150, 151);
            borderPanelMain.Visible = true;

            borderTextBoxUser.Texts = borderTextBoxRegisterUsername.Texts;
            borderTextBoxPassword.Texts = borderTextBoxRegisterPassword.Texts;
        }
        #endregion

        public void OnThemeChanged(Theme newTheme)
        {
            buttonMinimize.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.chrome_minimize_dark : Resources.chrome_minimize_light;
            buttonClose.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.chrome_close_dark : Resources.chrome_close_light;

            buttonSettings.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.gear_dark : Resources.gear_light;
            buttonMenuBack.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.arrow_left_dark : Resources.arrow_left_light;
            buttonThemeBack.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.arrow_left_dark : Resources.arrow_left_light;
            buttonThemeSettings.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.gear_dark : Resources.gear_light;
            buttonRegisterBack.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.arrow_left_dark : Resources.arrow_left_light;

            borderTextBoxUser.BorderFocusColor = newTheme.HighlightColor;
            borderTextBoxPassword.BorderFocusColor = newTheme.HighlightColor;
            borderTextBoxRegisterLicense.BorderFocusColor = newTheme.HighlightColor;
            borderTextBoxRegisterUsername.BorderFocusColor = newTheme.HighlightColor;
            borderTextBoxRegisterPassword.BorderFocusColor = newTheme.HighlightColor;

            checkBoxRemember.CheckMarkColor = newTheme.HighlightColor;
            checkBoxRegisterRemember.CheckMarkColor = newTheme.HighlightColor;
            buttonLogin.BackColor = newTheme.HighlightColor;
            buttonLogin.BackColorMouseDown = newTheme.HighlightBorderColor;
            buttonLogin.BackColorMouseOver = newTheme.HighlightBorderColor;

            buttonRegister.BackColor = newTheme.HighlightColor;
            buttonRegister.BackColorMouseDown = newTheme.HighlightBorderColor;
            buttonRegister.BackColorMouseOver = newTheme.HighlightBorderColor;

            dropDownMenuLanguage.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuLanguage.SelectBorderColorMouseOver = newTheme.HighlightColor;
            dropDownMenuLanguage.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuTheme.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuTheme.SelectBorderColorMouseOver = newTheme.HighlightColor;
            dropDownMenuTheme.SelectedBorderColor = newTheme.HighlightColor;
            checkBoxSaveLogs.CheckMarkColor = newTheme.HighlightColor;
            colorPictureBoxLogo.PictureTint = newTheme.HighlightColor;



            borderTextBoxUser.ForeColor = newTheme.TextColor;
            borderTextBoxPassword.ForeColor = newTheme.TextColor;
            borderTextBoxRegisterLicense.ForeColor = newTheme.TextColor;
            borderTextBoxRegisterUsername.ForeColor = newTheme.TextColor;
            borderTextBoxRegisterPassword.ForeColor = newTheme.TextColor;
            checkBoxRemember.ForeColor = newTheme.TextColor;
            checkBoxRegisterRemember.ForeColor = newTheme.TextColor;
            dropDownMenuLanguage.ForeColor = newTheme.TextColor;
            dropDownMenuTheme.ForeColor = newTheme.TextColor;
            checkBoxSaveLogs.ForeColor = newTheme.TextColor;
            labelNotRegistered.ForeColor = newTheme.TextColor;

            borderTextBoxUser.PlaceholderColor = newTheme.TextPlaceholderColor;
            borderTextBoxPassword.PlaceholderColor = newTheme.TextPlaceholderColor;
            borderTextBoxRegisterLicense.PlaceholderColor = newTheme.TextPlaceholderColor;
            borderTextBoxRegisterUsername.PlaceholderColor = newTheme.TextPlaceholderColor;
            borderTextBoxRegisterPassword.PlaceholderColor = newTheme.TextPlaceholderColor;



            BackColor = newTheme.PrimaryColor;
            panelTop.BackColor = newTheme.PrimaryColor;



            buttonMinimize.BackColor = newTheme.SecondaryColor;
            buttonMinimize.BackColorMouseDown = newTheme.SecondaryBorderColor;
            buttonMinimize.BackColorMouseOver = newTheme.SecondaryBorderColor;

            buttonClose.BackColor = newTheme.SecondaryColor;
            buttonSettings.BackColor = newTheme.SecondaryColor;
            buttonThemeSettings.BackColor = newTheme.SecondaryColor;

            dropDownMenuLanguage.BackColor = newTheme.SecondaryColor;
            dropDownMenuLanguage.ColorMouseDown = newTheme.SecondaryBorderColor;
            dropDownMenuLanguage.ColorMouseOver = newTheme.SecondaryBorderColor;

            dropDownMenuTheme.BackColor = newTheme.SecondaryColor;
            dropDownMenuTheme.ColorMouseDown = newTheme.SecondaryBorderColor;
            dropDownMenuTheme.ColorMouseOver = newTheme.SecondaryBorderColor;

            borderPanelMain.BackColor = newTheme.SecondaryColor;
            borderPanelMenu.BackColor = newTheme.SecondaryColor;
            borderPanelThemeEditor.BackColor = newTheme.SecondaryColor;
            borderPanelRegister.BackColor = newTheme.SecondaryColor;

            borderPanelMain.BorderColor = newTheme.SecondaryBorderColor;
            borderPanelMenu.BorderColor = newTheme.SecondaryBorderColor;
            borderPanelThemeEditor.BorderColor = newTheme.SecondaryBorderColor;
            borderPanelRegister.BorderColor = newTheme.SecondaryBorderColor;




            buttonMenuBack.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonMenuBack.BackColorMouseOver = newTheme.TertiaryBorderColor;

            buttonSettings.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonSettings.BackColorMouseOver = newTheme.TertiaryBorderColor;

            buttonThemeSettings.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonThemeSettings.BackColorMouseOver = newTheme.TertiaryBorderColor;

            buttonThemeBack.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonThemeBack.BackColorMouseOver = newTheme.TertiaryBorderColor;

            buttonRegisterBack.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonRegisterBack.BackColorMouseOver = newTheme.TertiaryBorderColor;

            buttonMenuBack.BackColor = newTheme.TertiaryColor;
            buttonMenuBack.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonMenuBack.BackColorMouseOver = newTheme.TertiaryBorderColor;

            buttonThemeBack.BackColor = newTheme.TertiaryColor;
            buttonThemeBack.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonThemeBack.BackColorMouseOver = newTheme.TertiaryBorderColor;

            buttonRegisterBack.BackColor = newTheme.TertiaryColor;
            buttonRegisterBack.BackColorMouseDown = newTheme.TertiaryBorderColor;
            buttonRegisterBack.BackColorMouseOver = newTheme.TertiaryBorderColor;

            dropDownMenuLanguage.Color = newTheme.TertiaryColor;
            dropDownMenuLanguage.BorderColor = newTheme.TertiaryColor;
            dropDownMenuLanguage.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuLanguage.BorderColorMouseOver = newTheme.TertiaryBorderColor;

            dropDownMenuTheme.Color = newTheme.TertiaryColor;
            dropDownMenuTheme.BorderColor = newTheme.TertiaryColor;
            dropDownMenuTheme.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuTheme.BorderColorMouseOver = newTheme.TertiaryBorderColor;

            checkBoxSaveLogs.BackColor = newTheme.TertiaryColor;
            checkBoxSaveLogs.BackColorChecked = newTheme.TertiaryColor;

            borderTextBoxUser.BackColor = newTheme.TertiaryColor;
            borderTextBoxPassword.BackColor = newTheme.TertiaryColor;

            borderTextBoxRegisterLicense.BackColor = newTheme.TertiaryColor;
            borderTextBoxRegisterUsername.BackColor = newTheme.TertiaryColor;
            borderTextBoxRegisterPassword.BackColor = newTheme.TertiaryColor;

            borderTextBoxUser.BorderColor = newTheme.TertiaryBorderColor;
            borderTextBoxPassword.BorderColor = newTheme.TertiaryBorderColor;

            borderTextBoxRegisterLicense.BorderColor = newTheme.TertiaryBorderColor;
            borderTextBoxRegisterUsername.BorderColor = newTheme.TertiaryBorderColor;
            borderTextBoxRegisterPassword.BorderColor = newTheme.TertiaryBorderColor;

            checkBoxRemember.BackColor = newTheme.TertiaryColor;
            checkBoxRemember.BackColorChecked = newTheme.TertiaryColor;

            checkBoxRegisterRemember.BackColor = newTheme.TertiaryColor;
            checkBoxRegisterRemember.BackColorChecked = newTheme.TertiaryColor;
        }

        public void OnLanguageChanged(Language newTable)
        {
            borderTextBoxRegisterLicense.PlaceholderText = newTable.LicenseKey;

            borderTextBoxUser.PlaceholderText = newTable.Username;
            borderTextBoxRegisterUsername.PlaceholderText = newTable.Username;

            borderTextBoxPassword.PlaceholderText = newTable.Password;
            borderTextBoxRegisterPassword.PlaceholderText = newTable.Password;

            checkBoxRemember.Text = newTable.RememberMe;
            checkBoxRegisterRemember.Text = newTable.RememberMe;

            labelNotRegistered.Text = newTable.DontHaveAccount;
            if (newTable == Language.Polish)
            {
                labelNotRegistered.Location = new Point(357, labelNotRegistered.Location.Y);
            }
            else if (newTable == Language.English)
            {
                labelNotRegistered.Location = new Point(320, labelNotRegistered.Location.Y);
            }

            buttonLogin.Text = newTable.Login;

            buttonRegister.Text = newTable.Register;

            dropDownMenuTheme.Elements = new List<string> { newTable.DarkMode, newTable.LightMode };

            checkBoxSaveLogs.Text = newTable.SaveLogs;


        }

        public void UpdateControls()
        {
            var settings = Program.Settings;
            if (settings == null) return;

            borderTextBoxUser.Texts = settings.User;
            borderTextBoxPassword.Texts = settings.Password;
            checkBoxRemember.Checked = settings.Remember;
            checkBoxRegisterRemember.Checked = settings.Remember;

            dropDownMenuLanguage.SelectionIndex = settings.Language switch
            {
                var lang when lang == Language.Polish => 0,
                var lang when lang == Language.English => 1,
                _ => 0
            };

            dropDownMenuTheme.SelectionIndex = settings.Theme switch
            {
                var theme when theme == Theme.Dark => 0,
                var theme when theme == Theme.Light => 1,
                _ => 0
            };

            checkBoxSaveLogs.Checked = settings.FileLogs;
        }

        private void dropDownMenuLanguage_IndexChanged(object sender, EventArgs e)
        {
            switch (dropDownMenuLanguage.SelectionIndex)
            {
                case 0:
                    Program.Settings.Language = Language.Polish;
                    ITranslatable.NotifyChange(Language.Polish);
                    break;

                case 1:
                    Program.Settings.Language = Language.English;
                    ITranslatable.NotifyChange(Language.English);
                    break;
            }

            Program.SaveConfiguration();
        }

        private void dropDownMenuTheme_IndexChanged(object sender, EventArgs e)
        {
            switch (dropDownMenuTheme.SelectionIndex)
            {
                case 0:
                    Program.Settings.ThemeIndex = 0;
                    Program.Settings.Theme = Theme.Dark;
                    IThemeable.NotifyChange(Theme.Dark);
                    break;

                case 1:
                    Program.Settings.ThemeIndex = 1;
                    Program.Settings.Theme = Theme.Light;
                    IThemeable.NotifyChange(Theme.Light);
                    break;
            }

            Program.SaveConfiguration();
        }

        private void checkBoxSaveLogs__CheckedChanged(object sender, EventArgs e)
        {
            Program.Settings.FileLogs = checkBoxSaveLogs.Checked;
            FileLogger.WriteFile = checkBoxSaveLogs.Checked;

            Program.SaveConfiguration();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var color = ColorTranslator.FromHtml(textBox1.Text);

                Theme.Dark.HighlightColor = color;
                Theme.Light.HighlightColor = color;

                int r = Math.Max(color.R - 30, 0);
                int g = Math.Max(color.G - 30, 0);
                int b = Math.Max(color.B - 30, 0);
                color = Color.FromArgb(r, g, b);

                Theme.Dark.HighlightBorderColor = color;
                Theme.Light.HighlightBorderColor = color;

                ((IThemeable)this).OnThemeChanged(Program.Settings!.Theme);
                Program.SaveConfiguration();
            }
            catch
            {

            }
        }
    }
}
