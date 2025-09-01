using HeroEngine.Desktop.Design;
using HeroEngine.Desktop.Properties;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;
using static HeroEngine.Persistance.ExecutionConfiguration;

namespace HeroEngine.Desktop.Interface
{
    public partial class BotForm : Form, IThemeable, ITranslatable
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

        public License License { get; set; }
        private bool _isDragging = false;
        private Point _startPoint = new Point(0, 0);
        private Account? _editedAccount = null;

        private List<Account> _accounts = new List<Account>();
        private ExecutionConfiguration _executionConfiguration;
        private ExecutionCoordinator? _executionCoordinator;
        public BotForm()
        {
            InitializeComponent();
            Size = new Size(1017, 720);
            MinimumSize = Size;
            MaximumSize = Size;

            IThemeable.Subscribe(this);
            ITranslatable.Subscribe(this);
            ((IThemeable)this).OnThemeChanged(Program.Settings!.Theme);
            ((ITranslatable)this).OnLanguageChanged(Program.Settings!.Language);

            panelTop.MouseDown += controlDraggableMouseDown!;
            panelTop.MouseMove += controlDraggableMouseMove!;
            panelTop.MouseUp += controlDraggableMouseUp!;
            panelTop.MouseCaptureChanged += controlDraggableMouseCaptureChanged!;

            string text = Placeholders.Build + " " + Placeholders.BuildCommit;
            Size textSize = TextRenderer.MeasureText(text, gradientLabelBuild.Font);
            int availableWidth = gradientLabelBuild.Parent!.ClientSize.Width - 40;
            while (textSize.Width > availableWidth && gradientLabelBuild.Font.Size > 5)
            {
                gradientLabelBuild.Font = new Font(gradientLabelBuild.Font.FontFamily, gradientLabelBuild.Font.Size - 0.5f);
                textSize = TextRenderer.MeasureText(text, gradientLabelBuild.Font);
            }

            gradientLabelBuild.Visible = true;
            gradientLabelBuild.Left = (gradientLabelBuild.Parent.ClientSize.Width - textSize.Width) / 2;
            gradientLabelBuild.Text = text;

            borderButtonPanel.Click += (sender, args) => ShowPanelMenu();
            borderButtonMission.Click += (sender, args) => ShowMissionMenu();
            borderButtonDuel.Click += (sender, args) => ShowDuelMenu();
            borderButtonLeague.Click += (sender, args) => ShowLeagueMenu();
            borderButtonTraining.Click += (sender, args) => ShowTrainingMenu();
            borderButtonHideout.Click += (sender, args) => ShowHideoutMenu();
            borderButtonOther.Click += (sender, args) => ShowOtherMenu();
            borderButtonAccounts.Click += (sender, args) => ShowAccountsMenu();
            borderButtonSettings.Click += (sender, args) => ShowSettingsMenu();
            borderButtonBooster.Click += (sender, args) => ShowBoostersMenu();
            borderButtonGuild.Click += (sender, args) => ShowGuildMenu();

            LoadConfiguration();
            LoadAccounts();
            if (_executionConfiguration?.ShowConsoleOutput ?? false)
            {
                ConsoleWindow.ShowConsole();
            }

            UpdateSettingsControls();

            ShowPanelMenu();
        }

        #region base
        private void LoadConfiguration()
        {
            try
            {
                string jsonString = File.ReadAllText("config.json");
                if (string.IsNullOrEmpty(jsonString)) throw new Exception(); // remake the file*

                _executionConfiguration = JsonConvert.DeserializeObject<ExecutionConfiguration>(jsonString)!;
            }
            catch
            {
                _executionConfiguration = new ExecutionConfiguration();
            }
        }

        private void SaveConfiguration()
        {
            if (_executionConfiguration == null) return;

            try
            {
                string jsonString = JsonConvert.SerializeObject(_executionConfiguration, Formatting.Indented);
                File.WriteAllText("config.json", jsonString);
            }
            catch
            {

            }
        }

        private void LoadAccounts()
        {
            try
            {
                string jsonString = File.ReadAllText("accounts.json");
                _accounts = JsonConvert.DeserializeObject<List<Account>>(jsonString)!;
            }
            catch
            {
                _accounts = new List<Account>();
            }
        }

        private void SaveAccounts()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(_accounts, Formatting.Indented);
                File.WriteAllText("accounts.json", jsonString);
            }
            catch
            {

            }
        }

        #region panel controls
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
        #endregion

        public void UpdateAccountsPanel()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateAccountsPanel));
                return;
            }

            foreach (Control control in panelAccountHolder.Controls.Cast<Control>().ToList())
            {
                if (control.Tag is string tag && (tag == "account-panel" || tag == "account-add"))
                {
                    panelAccountHolder.Controls.Remove(control);
                    control.Dispose();
                }
            }

            int offset = 0;
            foreach (Account account in _accounts.Take(16))
            {
                var panel = CreateAccountPanel(account, (account) => { account.Enabled = !account.Enabled; UpdateAccountsPanel(); SaveConfiguration(); SaveAccounts(); },
                    (account) =>
                    {
                        _editedAccount = account;

                        borderTextBoxAccountEmail.Texts = account.Email;
                        borderTextBoxAccountPassword.Texts = account.Password;
                        borderTextBoxAccountServer.Texts = account.Server;

                        borderButtonAccountConfirm.Text = "Edytuj konto";
                        borderPanelAddAccount.Visible = true;
                        UpdateAccountsPanel();

                    }, (account) => { if (_editedAccount == account) _editedAccount = null; _accounts.Remove(account); UpdateAccountsPanel(); SaveConfiguration(); SaveAccounts(); });

                panel.Location = new Point(0, offset);
                offset += panel.Height + 6;

                panelAccountHolder.Controls.Add(panel);
            }

            var button = new BorderButton();
            button.Size = new Size(411, 34);
            button.Location = new Point(0, offset);
            button.BackColor = Program.Settings.Theme.SecondaryColor;
            button.BackColorMouseOver = Program.Settings.Theme.TertiaryBorderColor;
            button.BackColorMouseDown = Program.Settings.Theme.TertiaryColor;

            button.BorderColor = Program.Settings.Theme.SecondaryBorderColor;
            button.BorderColorMouseOver = Program.Settings.Theme.SecondaryBorderColor;
            button.BorderColorMouseDown = Program.Settings.Theme.SecondaryBorderColor;
            button.ForeColor = Program.Settings.Theme.TextColor;
            button.Text = "Dodaj konto";
            button.Tag = "account-add";
            button.Click += (sender, e) =>
            {
                _editedAccount = null;
                borderTextBoxAccountEmail.Texts = "";
                borderTextBoxAccountPassword.Texts = "";
                borderTextBoxAccountServer.Texts = "";
                borderButtonAccountConfirm.Text = "Dodaj konto";
                borderPanelAddAccount.Visible = true;
                UpdateAccountsPanel();
            };

            panelAccountHolder.Controls.Add(button);
        }

        public void UpdateSettingsControls()
        {
            checkBoxShowConsole.Checked = ConsoleWindow.IsConsoleVisible();

            if (_executionConfiguration == null) return;

            checkBoxMissions.Checked = _executionConfiguration.Missions;
            checkBoxSkipTimedMissions.Checked = !_executionConfiguration.MissionAllowTimed;
            numberSliderMissionEnergy.Value = _executionConfiguration.MissionMaxEnergy;
            switch (_executionConfiguration.MissionPreferredSelect)
            {
                case MissionSelect.ExpRatio:
                    dropDownMenuMissionPreference.SelectionIndex = 0;
                    break;

                case MissionSelect.GoldRatio:
                    dropDownMenuMissionPreference.SelectionIndex = 1;
                    break;
            }

            //_executionConfiguration.PreferEventMissions;

            dropDownMenuMissionCouponPreference.SelectionIndex = _executionConfiguration.MissionCouponPreferredSelect switch
            {
                MissionCouponSelect.Oldest => 1,
                MissionCouponSelect.LeastEnergy => 2,
                MissionCouponSelect.MostEnergy => 3,
                _ => 0
            };

            checkBoxDuels.Checked = _executionConfiguration.Duels;
            //attack teammates
            checkBoxDuelsUnequipThrowables.Checked = _executionConfiguration.DuelsUnequipThrowables;

            checkBoxLeague.Checked = _executionConfiguration.League;
            checkBoxLeagueUnequipThrowables.Checked = _executionConfiguration.LeagueUnequipThrowables;
            //attack teammates


            checkBoxTrainings.Checked = _executionConfiguration.Training;
            checkBoxTrainingsStrength.Checked = _executionConfiguration.TrainingPreferredSelect.HasFlag(TrainingSelect.Strength);
            checkBoxTrainingsStamina.Checked = _executionConfiguration.TrainingPreferredSelect.HasFlag(TrainingSelect.Stamina);
            checkBoxTrainingsCritical.Checked = _executionConfiguration.TrainingPreferredSelect.HasFlag(TrainingSelect.CriticalRating);
            checkBoxTrainingsDodge.Checked = _executionConfiguration.TrainingPreferredSelect.HasFlag(TrainingSelect.DodgeRating);

            dropDownMenuTrainingsCouponsPreference.SelectionIndex = _executionConfiguration.TrainingCouponPreferredSelect switch
            {
                TrainingCouponSelect.Oldest => 1,
                TrainingCouponSelect.LeastMotivation => 2,
                TrainingCouponSelect.MostMotivation => 3,
                _ => 0
            };

            checkBoxHideoutAttack.Checked = _executionConfiguration.HideoutAttacks;
            checkBoxHideoutCollect.Checked = _executionConfiguration.HideoutCollectResources;
            checkBoxHideoutChest.Checked = _executionConfiguration.HideoutCollectChests;

            dropDownMenuBoosterMission.SelectionIndex = _executionConfiguration.QuestBoosterPreferredSelect switch
            {
                QuestBoosterSelect.Small => 1,
                QuestBoosterSelect.Medium => 2,
                QuestBoosterSelect.Premium => 3,
                _ => 0
            };

            dropDownMenuBoosterStats.SelectionIndex = _executionConfiguration.StatBoosterPreferredSelect switch
            {
                StatBoosterSelect.Small => 1,
                StatBoosterSelect.Medium => 2,
                StatBoosterSelect.Premium => 3,
                _ => 0
            };

            dropDownMenuBoosterWork.SelectionIndex = _executionConfiguration.WorkBoosterPreferredSelect switch
            {
                WorkBoosterSelect.Small => 1,
                WorkBoosterSelect.Medium => 2,
                WorkBoosterSelect.Premium => 3,
                _ => 0
            };

            dropDownMenuBoosterLeague.SelectionIndex = _executionConfiguration.LeagueBoosterPreferredSelect switch
            {
                LeagueBoosterSelect.Medium => 1,
                LeagueBoosterSelect.Premium => 2,
                _ => 0
            };

            checkBoxTeamJoinAttacks.Checked = _executionConfiguration.JoinClaimGuildFights;
            checkBoxTeamJoinDefense.Checked = _executionConfiguration.JoinClaimGuildDefenses;
            checkBoxJoinDungeon.Checked = _executionConfiguration.JoinClaimGuildDungeons;

            checkBoxClaimLoginRewards.Checked = _executionConfiguration.ClaimLoginBonus;
            checkBoxAcceptBatteryRequests.Checked = _executionConfiguration.AcceptBatteryRequests;
            checkBoxClaimShovels.Checked = _executionConfiguration.Treasure;
            checkBoxKillOnMaintenance.Checked = _executionConfiguration.KillOnMaintenance;


            switch (_executionConfiguration.InventorySellPreferredSelect)
            {
                case InventorySellSelect.None:
                    dropDownMenuInventorySellPreference.SelectionIndex = 0;
                    break;

                case InventorySellSelect.Worse:
                    dropDownMenuInventorySellPreference.SelectionIndex = 1;
                    break;

                case InventorySellSelect.All:
                    dropDownMenuInventorySellPreference.SelectionIndex = 2;
                    break;
            }

            checkBoxRunDungeons.Checked = _executionConfiguration.Dungeons;

            checkBoxShowConsole.Checked = _executionConfiguration.ShowConsoleOutput;
        }

        private BorderPanel CreateAccountPanel(Account account, Action<Account>? onClickEnabled, Action<Account>? onClickEdit, Action<Account>? onClickDelete)
        {
            var panel = new BorderPanel();
            panel.Size = new Size(411, 34);
            panel.BackColor = Program.Settings.Theme.SecondaryColor;
            panel.BorderColor = _editedAccount == account ? Program.Settings.Theme.HighlightColor : Program.Settings.Theme.SecondaryBorderColor;

            var label = new GradientLabel();
            label.Size = new Size(318, 15);
            label.Text = !string.IsNullOrEmpty(account.Name) ? $"{account.Server.ToUpper()} - {account.Name}" : $"{account.Server.ToUpper()} {account.Email}";

            var labelColor = account.Enabled ? Program.Settings.Theme.HighlightColor : Program.Settings.Theme.TextColor;
            label.StartColor = labelColor;
            label.EndColor = labelColor;
            label.Location = new Point(3, 9);

            var checkbox = new BorderCheckBox();
            checkbox.BackColor = Program.Settings.Theme.TertiaryBorderColor;
            checkbox.BackColorChecked = Program.Settings.Theme.TertiaryBorderColor;
            checkbox.CheckMarkColor = Program.Settings.Theme.HighlightColor;
            checkbox.Size = new Size(28, 28);
            checkbox.Text = "";
            checkbox.Location = new Point(321, 3);
            checkbox.Checked = account.Enabled;
            checkbox.Click += (sender, e) => onClickEnabled?.Invoke(account);

            var edit = new Design.Button();
            edit.BackColor = Program.Settings.Theme.SecondaryColor;
            edit.BackColorMouseOver = Program.Settings.Theme.TertiaryBorderColor;
            edit.BackColorMouseDown = Program.Settings.Theme.SecondaryBorderColor;
            edit.Size = new Size(28, 28);
            edit.Text = "";
            edit.BackImage = Program.Settings.Theme.Icons == Theme.IconSet.Dark ? Resources.gear_dark : Resources.gear_light;
            edit.BackImageLayout = ImageLayout.Center;
            edit.Location = new Point(350, 3);
            edit.Click += (sender, e) => onClickEdit?.Invoke(account);

            var delete = new Design.Button();
            delete.BackColor = Program.Settings.Theme.SecondaryColor;
            delete.BackColorMouseOver = Program.Settings.Theme.TertiaryBorderColor;
            delete.BackColorMouseDown = Program.Settings.Theme.SecondaryBorderColor;
            delete.Size = new Size(28, 28);
            delete.Text = "";
            delete.BackImage = Program.Settings.Theme.Icons == Theme.IconSet.Dark ? Resources.trash_dark : Resources.trash_light;
            delete.BackImageLayout = ImageLayout.Center;
            delete.Location = new Point(380, 3);
            delete.Click += (sender, e) => onClickDelete?.Invoke(account);

            panel.Controls.Add(label);
            panel.Controls.Add(checkbox);
            panel.Controls.Add(edit);
            panel.Controls.Add(delete);

            panel.Tag = "account-panel";
            return panel;
        }

        public void OnThemeChanged(Theme newTheme)
        {
            buttonMinimize.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.chrome_minimize_dark : Resources.chrome_minimize_light;
            buttonClose.BackImage = newTheme.Icons == Theme.IconSet.Dark ? Resources.chrome_close_dark : Resources.chrome_close_light;

            gradientLabelBuild.StartColor = newTheme.HighlightColor;
            gradientLabelBuild.EndColor = newTheme.HighlightColor;

            gradientLabelExpires.StartColor = newTheme.HighlightColor;
            gradientLabelExpires.EndColor = newTheme.HighlightColor;
            borderPanelLogo.BorderColor = newTheme.HighlightColor;


            borderSliderToggleExecution.BorderColorChecked = newTheme.HighlightColor;
            checkBoxLeague.CheckMarkColor = newTheme.HighlightColor;
            checkBoxLeagueUnequipThrowables.CheckMarkColor = newTheme.HighlightColor;

            checkBoxDuels.CheckMarkColor = newTheme.HighlightColor;
            checkBoxDuelsUnequipThrowables.CheckMarkColor = newTheme.HighlightColor;

            checkBoxTrainings.CheckMarkColor = newTheme.HighlightColor;
            checkBoxTrainingsStrength.CheckMarkColor = newTheme.HighlightColor;
            checkBoxTrainingsStamina.CheckMarkColor = newTheme.HighlightColor;
            checkBoxTrainingsDodge.CheckMarkColor = newTheme.HighlightColor;
            checkBoxTrainingsCritical.CheckMarkColor = newTheme.HighlightColor;

            dropDownMenuTrainingsCouponsPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuTrainingsCouponsPreference.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuTrainingsCouponsPreference.SelectBorderColorMouseOver = newTheme.HighlightColor;

            checkBoxHideoutAttack.CheckMarkColor = newTheme.HighlightColor;
            checkBoxHideoutCollect.CheckMarkColor = newTheme.HighlightColor;
            checkBoxHideoutChest.CheckMarkColor = newTheme.HighlightColor;

            dropDownMenuBoosterMission.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterMission.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuBoosterMission.SelectBorderColorMouseOver = newTheme.HighlightColor;

            dropDownMenuBoosterStats.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterStats.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuBoosterStats.SelectBorderColorMouseOver = newTheme.HighlightColor;

            dropDownMenuBoosterWork.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterWork.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuBoosterWork.SelectBorderColorMouseOver = newTheme.HighlightColor;

            dropDownMenuBoosterLeague.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterLeague.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuBoosterLeague.SelectBorderColorMouseOver = newTheme.HighlightColor;

            checkBoxTeamJoinAttacks.CheckMarkColor = newTheme.HighlightColor;
            checkBoxTeamJoinDefense.CheckMarkColor = newTheme.HighlightColor;
            checkBoxJoinDungeon.CheckMarkColor = newTheme.HighlightColor;


            checkBoxMissions.CheckMarkColor = newTheme.HighlightColor;
            checkBoxSkipTimedMissions.CheckMarkColor = newTheme.HighlightColor;
            numberSliderMissionEnergy.ThumbColorDragging = newTheme.HighlightColor;

            dropDownMenuMissionPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuMissionPreference.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuMissionPreference.SelectBorderColorMouseOver = newTheme.HighlightColor;

            dropDownMenuMissionCouponPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuMissionCouponPreference.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuMissionCouponPreference.SelectBorderColorMouseOver = newTheme.HighlightColor;


            checkBoxClaimLoginRewards.CheckMarkColor = newTheme.HighlightColor;
            checkBoxAcceptBatteryRequests.CheckMarkColor = newTheme.HighlightColor;
            checkBoxClaimShovels.CheckMarkColor = newTheme.HighlightColor;
            checkBoxRunDungeons.CheckMarkColor = newTheme.HighlightColor;
            checkBoxKillOnMaintenance.CheckMarkColor = newTheme.HighlightColor;

            dropDownMenuInventorySellPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuInventorySellPreference.SelectBorderColorMouseDown = newTheme.HighlightColor;
            dropDownMenuInventorySellPreference.SelectBorderColorMouseOver = newTheme.HighlightColor;
            checkBoxShowConsole.CheckMarkColor = newTheme.HighlightColor;



            label2.ForeColor = newTheme.TextColor;
            checkBoxLeagueUnequipThrowables.ForeColor = newTheme.TextColor;
            checkBoxLeague.ForeColor = newTheme.TextColor;

            checkBoxDuelsUnequipThrowables.ForeColor = newTheme.TextColor;
            checkBoxDuels.ForeColor = newTheme.TextColor;

            checkBoxTrainings.ForeColor = newTheme.TextColor;
            checkBoxTrainingsStrength.ForeColor = newTheme.TextColor;
            checkBoxTrainingsStamina.ForeColor = newTheme.TextColor;
            checkBoxTrainingsCritical.ForeColor = newTheme.TextColor;
            checkBoxTrainingsDodge.ForeColor = newTheme.TextColor;

            checkBoxHideoutAttack.ForeColor = newTheme.TextColor;
            checkBoxHideoutChest.ForeColor = newTheme.TextColor;
            checkBoxHideoutCollect.ForeColor = newTheme.TextColor;

            checkBoxMissions.ForeColor = newTheme.TextColor;
            checkBoxSkipTimedMissions.ForeColor = newTheme.TextColor;

            checkBoxClaimLoginRewards.ForeColor = newTheme.TextColor;
            checkBoxAcceptBatteryRequests.ForeColor = newTheme.TextColor;

            checkBoxClaimShovels.ForeColor = newTheme.TextColor;
            checkBoxRunDungeons.ForeColor = newTheme.TextColor;

            checkBoxKillOnMaintenance.ForeColor = newTheme.TextColor;
            checkBoxShowConsole.ForeColor = newTheme.TextColor;
            checkBoxTeamJoinAttacks.ForeColor = newTheme.TextColor;
            checkBoxTeamJoinDefense.ForeColor = newTheme.TextColor;
            checkBoxJoinDungeon.ForeColor = newTheme.TextColor;


            colorPictureBoxLogoVert.PictureTint = newTheme.HighlightColor;



            BackColor = newTheme.PrimaryColor;
            panelTop.BackColor = newTheme.PrimaryColor;

            buttonMinimize.BackColor = newTheme.SecondaryColor;
            buttonMinimize.BackColorMouseDown = newTheme.SecondaryBorderColor;
            buttonMinimize.BackColorMouseOver = newTheme.SecondaryBorderColor;

            buttonClose.BackColor = newTheme.SecondaryColor;

            checkBoxLeagueUnequipThrowables.BackColor = newTheme.SecondaryColor;
            checkBoxLeague.BackColor = newTheme.SecondaryColor;

            checkBoxDuelsUnequipThrowables.BackColor = newTheme.SecondaryColor;
            checkBoxDuels.BackColor = newTheme.SecondaryColor;

            checkBoxTrainings.BackColor = newTheme.SecondaryColor;
            checkBoxTrainingsStrength.BackColor = newTheme.SecondaryColor;
            checkBoxTrainingsStamina.BackColor = newTheme.SecondaryColor;
            checkBoxTrainingsCritical.BackColor = newTheme.SecondaryColor;
            checkBoxTrainingsDodge.BackColor = newTheme.SecondaryColor;

            checkBoxHideoutAttack.BackColor = newTheme.SecondaryColor;
            checkBoxHideoutChest.BackColor = newTheme.SecondaryColor;
            checkBoxHideoutCollect.BackColor = newTheme.SecondaryColor;

            checkBoxMissions.BackColor = newTheme.SecondaryColor;
            checkBoxSkipTimedMissions.BackColor = newTheme.SecondaryColor;

            checkBoxClaimLoginRewards.BackColor = newTheme.SecondaryColor;
            checkBoxAcceptBatteryRequests.BackColor = newTheme.SecondaryColor;

            checkBoxClaimShovels.BackColor = newTheme.SecondaryColor;
            checkBoxRunDungeons.BackColor = newTheme.SecondaryColor;

            checkBoxKillOnMaintenance.BackColor = newTheme.SecondaryColor;
            checkBoxShowConsole.BackColor = newTheme.SecondaryColor;
            checkBoxTeamJoinAttacks.BackColor = newTheme.SecondaryColor;
            checkBoxTeamJoinDefense.BackColor = newTheme.SecondaryColor;
            checkBoxJoinDungeon.BackColor = newTheme.SecondaryColor;



            checkBoxLeagueUnequipThrowables.BackColorChecked = newTheme.SecondaryColor;
            checkBoxLeague.BackColorChecked = newTheme.SecondaryColor;

            checkBoxDuelsUnequipThrowables.BackColorChecked = newTheme.SecondaryColor;
            checkBoxDuels.BackColorChecked = newTheme.SecondaryColor;

            checkBoxTrainings.BackColorChecked = newTheme.SecondaryColor;
            checkBoxTrainingsStrength.BackColorChecked = newTheme.SecondaryColor;
            checkBoxTrainingsStamina.BackColorChecked = newTheme.SecondaryColor;
            checkBoxTrainingsCritical.BackColorChecked = newTheme.SecondaryColor;
            checkBoxTrainingsDodge.BackColorChecked = newTheme.SecondaryColor;

            checkBoxHideoutAttack.BackColorChecked = newTheme.SecondaryColor;
            checkBoxHideoutChest.BackColorChecked = newTheme.SecondaryColor;
            checkBoxHideoutCollect.BackColorChecked = newTheme.SecondaryColor;

            checkBoxMissions.BackColorChecked = newTheme.SecondaryColor;
            checkBoxSkipTimedMissions.BackColorChecked = newTheme.SecondaryColor;

            checkBoxClaimLoginRewards.BackColorChecked = newTheme.SecondaryColor;
            checkBoxAcceptBatteryRequests.BackColorChecked = newTheme.SecondaryColor;

            checkBoxClaimShovels.BackColorChecked = newTheme.SecondaryColor;
            checkBoxRunDungeons.BackColorChecked = newTheme.SecondaryColor;

            checkBoxKillOnMaintenance.BackColorChecked = newTheme.SecondaryColor;
            checkBoxShowConsole.BackColorChecked = newTheme.SecondaryColor;
            checkBoxTeamJoinAttacks.BackColorChecked = newTheme.SecondaryColor;
            checkBoxTeamJoinDefense.BackColorChecked = newTheme.SecondaryColor;
            checkBoxJoinDungeon.BackColorChecked = newTheme.SecondaryColor;

            numberSliderMissionEnergy.ForeColor = newTheme.TextColor;
            numberSliderMissionEnergy.BackColor = newTheme.SecondaryColor;
            numberSliderMissionEnergy.ThumbColor = newTheme.SecondaryBorderColor;
            numberSliderMissionEnergy.ThumbColorDragging = newTheme.HighlightColor;



            dropDownMenuTrainingsCouponsPreference.ForeColor = newTheme.TextColor;
            dropDownMenuTrainingsCouponsPreference.BackColor = newTheme.PrimaryColor;
            dropDownMenuTrainingsCouponsPreference.BorderColor = newTheme.TertiaryColor;
            dropDownMenuTrainingsCouponsPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuTrainingsCouponsPreference.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuTrainingsCouponsPreference.Color = newTheme.SecondaryColor;
            dropDownMenuTrainingsCouponsPreference.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuTrainingsCouponsPreference.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuTrainingsCouponsPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuTrainingsCouponsPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuTrainingsCouponsPreference.ColorMouseOver = newTheme.TertiaryColor;

            dropDownMenuMissionPreference.ForeColor = newTheme.TextColor;
            dropDownMenuMissionPreference.BackColor = newTheme.PrimaryColor;
            dropDownMenuMissionPreference.BorderColor = newTheme.TertiaryColor;
            dropDownMenuMissionPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuMissionPreference.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuMissionPreference.Color = newTheme.SecondaryColor;
            dropDownMenuMissionPreference.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuMissionPreference.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuMissionPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuMissionPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuMissionPreference.ColorMouseOver = newTheme.TertiaryColor;

            dropDownMenuMissionCouponPreference.ForeColor = newTheme.TextColor;
            dropDownMenuMissionCouponPreference.BackColor = newTheme.PrimaryColor;
            dropDownMenuMissionCouponPreference.BorderColor = newTheme.TertiaryColor;
            dropDownMenuMissionCouponPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuMissionCouponPreference.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuMissionCouponPreference.Color = newTheme.SecondaryColor;
            dropDownMenuMissionCouponPreference.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuMissionCouponPreference.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuMissionCouponPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuMissionCouponPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuMissionCouponPreference.ColorMouseOver = newTheme.TertiaryColor;

            dropDownMenuInventorySellPreference.ForeColor = newTheme.TextColor;
            dropDownMenuInventorySellPreference.BackColor = newTheme.PrimaryColor;
            dropDownMenuInventorySellPreference.BorderColor = newTheme.TertiaryColor;
            dropDownMenuInventorySellPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuInventorySellPreference.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuInventorySellPreference.Color = newTheme.SecondaryColor;
            dropDownMenuInventorySellPreference.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuInventorySellPreference.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuInventorySellPreference.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuInventorySellPreference.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuInventorySellPreference.ColorMouseOver = newTheme.TertiaryColor;

            dropDownMenuBoosterMission.ForeColor = newTheme.TextColor;
            dropDownMenuBoosterMission.BackColor = newTheme.PrimaryColor;
            dropDownMenuBoosterMission.BorderColor = newTheme.TertiaryColor;
            dropDownMenuBoosterMission.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterMission.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterMission.Color = newTheme.SecondaryColor;
            dropDownMenuBoosterMission.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterMission.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterMission.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterMission.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterMission.ColorMouseOver = newTheme.TertiaryColor;

            dropDownMenuBoosterStats.ForeColor = newTheme.TextColor;
            dropDownMenuBoosterStats.BackColor = newTheme.PrimaryColor;
            dropDownMenuBoosterStats.BorderColor = newTheme.TertiaryColor;
            dropDownMenuBoosterStats.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterStats.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterStats.Color = newTheme.SecondaryColor;
            dropDownMenuBoosterStats.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterStats.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterStats.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterStats.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterStats.ColorMouseOver = newTheme.TertiaryColor;

            dropDownMenuBoosterWork.ForeColor = newTheme.TextColor;
            dropDownMenuBoosterWork.BackColor = newTheme.PrimaryColor;
            dropDownMenuBoosterWork.BorderColor = newTheme.TertiaryColor;
            dropDownMenuBoosterWork.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterWork.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterWork.Color = newTheme.SecondaryColor;
            dropDownMenuBoosterWork.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterWork.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterWork.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterWork.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterWork.ColorMouseOver = newTheme.TertiaryColor;

            dropDownMenuBoosterLeague.ForeColor = newTheme.TextColor;
            dropDownMenuBoosterLeague.BackColor = newTheme.PrimaryColor;
            dropDownMenuBoosterLeague.BorderColor = newTheme.TertiaryColor;
            dropDownMenuBoosterLeague.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterLeague.BorderColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterLeague.Color = newTheme.SecondaryColor;
            dropDownMenuBoosterLeague.ColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterLeague.ColorMouseOver = newTheme.TertiaryColor;
            dropDownMenuBoosterLeague.SelectedBorderColor = newTheme.HighlightColor;
            dropDownMenuBoosterLeague.BorderColorMouseDown = newTheme.TertiaryBorderColor;
            dropDownMenuBoosterLeague.ColorMouseOver = newTheme.TertiaryColor;


            borderSliderToggleExecution.BackColor = newTheme.SecondaryColor;
            borderSliderToggleExecution.BorderColor = newTheme.SecondaryBorderColor;
            borderSliderToggleExecution.ForeColor = newTheme.IconColor;
            borderSliderToggleExecution.KnobColor = newTheme.IconColor;
            borderSliderToggleExecution.KnobColorChecked = newTheme.IconColor;

            var panels = new BorderPanel[] { borderPanelMain, borderPanelLeague, borderPanelDuel, borderPanelTraining, borderPanelHideout, borderPanelMission, borderPanelOther, borderPanelSettings, borderPanelAccounts, borderPanelBoosters, borderPanelGuild };
            foreach ( var panel in panels )
            {
                panel.BackColor = newTheme.PrimaryColor;
                panel.BorderColor = newTheme.HighlightColor;
            }

            borderPanelAddAccount.BackColor = newTheme.PrimaryColor;
            borderPanelAddAccount.BorderColor = newTheme.SecondaryBorderColor;

            borderTextBoxAccountEmail.BorderFocusColor = newTheme.HighlightColor;
            borderTextBoxAccountEmail.ForeColor = newTheme.TextColor;
            borderTextBoxAccountEmail.PlaceholderColor = newTheme.TextPlaceholderColor;
            borderTextBoxAccountEmail.BackColor = newTheme.SecondaryColor;
            borderTextBoxAccountEmail.BorderColor = newTheme.SecondaryBorderColor;

            borderTextBoxAccountPassword.BorderFocusColor = newTheme.HighlightColor;
            borderTextBoxAccountPassword.ForeColor = newTheme.TextColor;
            borderTextBoxAccountPassword.PlaceholderColor = newTheme.TextPlaceholderColor;
            borderTextBoxAccountPassword.BackColor = newTheme.SecondaryColor;
            borderTextBoxAccountPassword.BorderColor = newTheme.SecondaryBorderColor;

            borderTextBoxAccountServer.BorderFocusColor = newTheme.HighlightColor;
            borderTextBoxAccountServer.ForeColor = newTheme.TextColor;
            borderTextBoxAccountServer.PlaceholderColor = newTheme.TextPlaceholderColor;
            borderTextBoxAccountServer.BackColor = newTheme.SecondaryColor;
            borderTextBoxAccountServer.BorderColor = newTheme.SecondaryBorderColor;

            borderButtonAccountCancel.BackColor = newTheme.SecondaryColor;
            borderButtonAccountCancel.BorderColor = newTheme.SecondaryBorderColor;
            borderButtonAccountCancel.ForeColor = newTheme.TextColor;
            borderButtonAccountCancel.BackColorMouseDown = newTheme.TertiaryColor;
            borderButtonAccountCancel.BackColorMouseOver = newTheme.SecondaryBorderColor;
            borderButtonAccountCancel.BorderColorMouseDown = newTheme.SecondaryBorderColor;
            borderButtonAccountCancel.BorderColorMouseOver = newTheme.SecondaryBorderColor;

            borderButtonAccountConfirm.BackColor = newTheme.SecondaryColor;
            borderButtonAccountConfirm.BorderColor = newTheme.SecondaryBorderColor;
            borderButtonAccountConfirm.ForeColor = newTheme.TextColor;
            borderButtonAccountConfirm.BackColorMouseDown = newTheme.TertiaryColor;
            borderButtonAccountConfirm.BackColorMouseOver = newTheme.SecondaryBorderColor;
            borderButtonAccountConfirm.BorderColorMouseDown = newTheme.SecondaryBorderColor;
            borderButtonAccountConfirm.BorderColorMouseOver = newTheme.SecondaryBorderColor;

            //panelAccountHolder
            ResetMenuButtonHighlights();
            //figure out menu highlight


            UpdateAccountsPanel();
        }

        public void OnLanguageChanged(Language newTable)
        {

        }

        #region Menus
        public void ShowPanelMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowPanelMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonPanel.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonPanel.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonPanel.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelMain.Location = new Point(352, 36);
            borderPanelMain.Visible = true;
            borderPanelMain.BringToFront();
        }

        public void ShowMissionMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowMissionMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonMission.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonMission.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonMission.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelMission.Location = new Point(352, 36);
            borderPanelMission.Visible = true;
            borderPanelMission.BringToFront();
        }

        public void ShowDuelMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowDuelMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonDuel.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonDuel.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonDuel.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelDuel.Location = new Point(352, 36);
            borderPanelDuel.Visible = true;
            borderPanelDuel.BringToFront();
        }

        public void ShowLeagueMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowLeagueMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonLeague.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonLeague.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonLeague.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelLeague.Location = new Point(352, 36);
            borderPanelLeague.Visible = true;
            borderPanelLeague.BringToFront();
        }

        public void ShowTrainingMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowTrainingMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonTraining.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonTraining.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonTraining.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelTraining.Location = new Point(352, 36);
            borderPanelTraining.Visible = true;
            borderPanelTraining.BringToFront();
        }

        public void ShowHideoutMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowHideoutMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonHideout.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonHideout.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonHideout.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelHideout.Location = new Point(352, 36);
            borderPanelHideout.Visible = true;
            borderPanelHideout.BringToFront();
        }

        public void ShowBoostersMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowBoostersMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonBooster.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonBooster.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonBooster.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelBoosters.Location = new Point(352, 36);
            borderPanelBoosters.Visible = true;
            borderPanelBoosters.BringToFront();
        }

        public void ShowGuildMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowGuildMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonGuild.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonGuild.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonGuild.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelGuild.Location = new Point(352, 36);
            borderPanelGuild.Visible = true;
            borderPanelGuild.BringToFront();
        }

        public void ShowOtherMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowOtherMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonOther.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonOther.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonOther.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelOther.Location = new Point(352, 36);
            borderPanelOther.Visible = true;
            borderPanelOther.BringToFront();
        }

        public void ShowAccountsMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowAccountsMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonAccounts.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonAccounts.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonAccounts.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelAccounts.Location = new Point(352, 36);
            borderPanelAccounts.Visible = true;
            borderPanelAccounts.BringToFront();

            UpdateAccountsPanel();
        }

        public void ShowSettingsMenu()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ShowSettingsMenu));
                return;
            }

            ResetMenuButtonHighlights();
            borderButtonSettings.BorderColor = Program.Settings.Theme.HighlightBorderColor;
            borderButtonSettings.BorderColorMouseDown = Program.Settings.Theme.HighlightBorderColor;
            borderButtonSettings.BorderColorMouseOver = Program.Settings.Theme.HighlightBorderColor;

            ResetMenuPanels();
            borderPanelSettings.Location = new Point(352, 36);
            borderPanelSettings.Visible = true;
            borderPanelSettings.BringToFront();
        }

        private void ResetMenuButtonHighlights()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { ResetMenuButtonHighlights(); }));
                return;
            }

            var controls = new List<BorderButton>() {
                borderButtonPanel, borderButtonMission, borderButtonDuel, borderButtonLeague,
                borderButtonTraining, borderButtonHideout, borderButtonBooster, borderButtonGuild,
                borderButtonOther, borderButtonAccounts, borderButtonSettings };
            foreach (BorderButton control in controls)
            {
                control.ForeColor = Program.Settings.Theme.TextColor;

                control.BackColor = Program.Settings.Theme.SecondaryColor;
                control.BackColorMouseDown = Program.Settings.Theme.SecondaryColor;
                control.BackColorMouseOver = Program.Settings.Theme.SecondaryColor;

                control.BorderColor = Program.Settings.Theme.SecondaryColor;
                control.BorderColorMouseDown = Program.Settings.Theme.SecondaryColor;
                control.BorderColorMouseOver = Program.Settings.Theme.SecondaryColor;
            }
        }

        private void ResetMenuPanels()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { ResetMenuPanels(); }));
                return;
            }

            var controls = new List<BorderPanel>() { borderPanelMain, borderPanelMission, borderPanelDuel, borderPanelLeague, borderPanelTraining, borderPanelHideout, borderPanelOther };
            foreach (BorderPanel control in controls)
            {
                control.Visible = false;
            }
        }
        #endregion

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (License != null)
            {
                gradientLabelExpires.Visible = false;
                long seconds = UnixTime.Until(UnixTime.From(License.ExpiryTime));
                if (seconds > 0)
                {
                    string text = Program.Settings.Language.LicenseExpires.Replace("{0}", UnixTime.FormatAsDays(seconds, false));
                    if (TimeSpan.FromSeconds(seconds) > TimeSpan.FromDays(3652))
                    {
                        text = Program.Settings.Language.LicenseNeverExpries;
                    }

                    Size textSize = TextRenderer.MeasureText(text, gradientLabelExpires.Font);

                    int availableWidth = gradientLabelExpires.Parent!.ClientSize.Width - 40;

                    while (textSize.Width > availableWidth && gradientLabelExpires.Font.Size > 5)
                    {
                        gradientLabelExpires.Font = new Font(gradientLabelExpires.Font.FontFamily, gradientLabelExpires.Font.Size - 0.5f);
                        textSize = TextRenderer.MeasureText(text, gradientLabelExpires.Font);
                    }

                    gradientLabelExpires.Visible = true;
                    gradientLabelExpires.Left = (gradientLabelExpires.Parent.ClientSize.Width - textSize.Width) / 2;
                    gradientLabelExpires.Text = text;
                    return;
                }

                Close();
                return;
            }

            Close();
        }

        private void borderButtonAccountCancel_Click(object sender, EventArgs e)
        {
            _editedAccount = null;
            borderPanelAddAccount.Visible = false;
            borderTextBoxAccountEmail.Texts = "";
            borderTextBoxAccountPassword.Texts = "";
            borderTextBoxAccountServer.Texts = "";
            UpdateAccountsPanel();

            SaveConfiguration();
            SaveAccounts();
        }

        private void borderButtonAccountConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(borderTextBoxAccountEmail.Texts)
                || string.IsNullOrEmpty(borderTextBoxAccountPassword.Texts)
                || string.IsNullOrEmpty(borderTextBoxAccountServer.Texts)) return;

            borderPanelAddAccount.Visible = false;

            if (_editedAccount != null)
            {
                _editedAccount.Email = borderTextBoxAccountEmail.Texts;
                _editedAccount.Password = borderTextBoxAccountPassword.Texts;
                _editedAccount.Server = borderTextBoxAccountServer.Texts;

                borderTextBoxAccountEmail.Texts = "";
                borderTextBoxAccountPassword.Texts = "";
                borderTextBoxAccountServer.Texts = "";
                _editedAccount = null;

                UpdateAccountsPanel();
                return;
            }

            var account = new Account()
            {
                Email = borderTextBoxAccountEmail.Texts,
                Password = borderTextBoxAccountPassword.Texts,
                Server = borderTextBoxAccountServer.Texts,
                Enabled = false,
            };

            _accounts.Add(account);
            UpdateAccountsPanel();

            borderTextBoxAccountEmail.Texts = "";
            borderTextBoxAccountPassword.Texts = "";
            borderTextBoxAccountServer.Texts = "";

            SaveConfiguration();
            SaveAccounts();
        }
        #endregion

        private void borderSliderToggleExecution__CheckedChanged(object sender, EventArgs e)
        {
            SaveConfiguration();
            SaveAccounts();

            if (_executionCoordinator?.Starting ?? false)
            {
                return;
            }

            if (borderSliderToggleExecution.Checked)
            {
                if (_executionCoordinator == null)
                {
                    _executionCoordinator = new ExecutionCoordinator(_accounts, _executionConfiguration);
                }

                if (!_executionCoordinator.Running)
                {
                    Task.Run(() =>
                    {
                        _executionCoordinator.Start();
                    });
                }
            }
            else
            {
                if (_executionCoordinator != null)
                {
                    _executionCoordinator.Abort();
                }
            }
        }




        #region League Controls
        private void checkBoxLeague_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.League = checkBoxLeague.Checked;
            SaveConfiguration();
        }

        private void checkBoxLeagueUnequipThrowables_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.LeagueUnequipThrowables = checkBoxLeagueUnequipThrowables.Checked;
            SaveConfiguration();
        }
        #endregion

        #region Duels Controls
        private void checkBoxDuels_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.Duels = checkBoxDuels.Checked;
            SaveConfiguration();
        }

        private void checkBoxDuelsUnequipThrowables_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.DuelsUnequipThrowables = checkBoxDuelsUnequipThrowables.Checked;
            SaveConfiguration();
        }
        #endregion

        #region Training Controls
        private void checkBoxTrainings_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.Training = checkBoxTrainings.Checked;
            SaveConfiguration();
        }

        private void checkBoxTrainingsStrength_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTrainingsStrength.Checked)
            {
                _executionConfiguration.TrainingPreferredSelect |= TrainingSelect.Strength;
            }
            else
            {
                _executionConfiguration.TrainingPreferredSelect &= ~TrainingSelect.Strength;
            }


            SaveConfiguration();
        }

        private void checkBoxTrainingsStamina_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTrainingsStamina.Checked)
            {
                _executionConfiguration.TrainingPreferredSelect |= TrainingSelect.Stamina;
            }
            else
            {
                _executionConfiguration.TrainingPreferredSelect &= ~TrainingSelect.Stamina;
            }


            SaveConfiguration();
        }

        private void checkBoxTrainingsCritical_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTrainingsCritical.Checked)
            {
                _executionConfiguration.TrainingPreferredSelect |= TrainingSelect.CriticalRating;
            }
            else
            {
                _executionConfiguration.TrainingPreferredSelect &= ~TrainingSelect.CriticalRating;
            }

            SaveConfiguration();
        }

        private void checkBoxTrainingsDodge_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTrainingsDodge.Checked)
            {
                _executionConfiguration.TrainingPreferredSelect |= TrainingSelect.DodgeRating;
            }
            else
            {
                _executionConfiguration.TrainingPreferredSelect &= ~TrainingSelect.DodgeRating;
            }


            SaveConfiguration();
        }

        private void dropDownMenuTrainingsCouponsPreference_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.TrainingCouponPreferredSelect = dropDownMenuTrainingsCouponsPreference.SelectionIndex switch
            {
                1 => TrainingCouponSelect.Oldest,
                2 => TrainingCouponSelect.LeastMotivation,
                3 => TrainingCouponSelect.MostMotivation,
                _ => 0
            };


            SaveConfiguration();
        }
        #endregion

        #region Hideout Controls
        private void checkBoxHideoutAttack_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.HideoutAttacks = checkBoxHideoutAttack.Checked;
            SaveConfiguration();
        }

        private void checkBoxHideoutCollect_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.HideoutCollectResources = checkBoxHideoutCollect.Checked;
            SaveConfiguration();
        }

        private void checkBoxHideoutChest_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.HideoutCollectChests = checkBoxHideoutChest.Checked;
            SaveConfiguration();
        }

        private void checkBoxHideoutGemstones_CheckedChanged(object sender, EventArgs e)
        {
            throw new Exception("Not implemented");
        }
        #endregion

        #region Mission Controls
        private void checkBoxMissions_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.Missions = checkBoxMissions.Checked;
            SaveConfiguration();
        }

        private void checkBoxSkipTimedMissions_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.MissionAllowTimed = !checkBoxSkipTimedMissions.Checked;
            SaveConfiguration();
        }

        private void numberSliderMissionEnergy__ValueChanged(object sender, EventArgs e)
        {
            _executionConfiguration.MissionMaxEnergy = (int)numberSliderMissionEnergy.Value;
            SaveConfiguration();
        }

        private void dropDownMenuMissionPreference_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.MissionPreferredSelect = dropDownMenuMissionPreference.SelectionIndex switch
            {
                1 => MissionSelect.GoldRatio,
                _ => MissionSelect.ExpRatio
            };
            SaveConfiguration();
        }

        private void dropDownMenuMissionCouponPreference_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.MissionCouponPreferredSelect = dropDownMenuMissionCouponPreference.SelectionIndex switch
            {
                1 => MissionCouponSelect.Oldest,
                2 => MissionCouponSelect.LeastEnergy,
                3 => MissionCouponSelect.MostEnergy,
                _ => 0
            };
            SaveConfiguration();
        }

        private void dropDownMenuInventorySellPreference_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.InventorySellPreferredSelect = dropDownMenuInventorySellPreference.SelectionIndex switch
            {
                1 => InventorySellSelect.Worse,
                2 => InventorySellSelect.All,
                _ => 0
            };
            SaveConfiguration();
        }
        #endregion

        #region Other Controls
        private void checkBoxClaimLoginRewards_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.ClaimLoginBonus = checkBoxClaimLoginRewards.Checked;
            SaveConfiguration();
        }

        private void checkBoxAcceptBatteryRequests_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.AcceptBatteryRequests = checkBoxAcceptBatteryRequests.Checked;
            SaveConfiguration();
        }

        private void checkBoxClaimShovels_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.Treasure = checkBoxClaimShovels.Checked;
            SaveConfiguration();
        }

        private void checkBoxSolveTreasure_CheckedChanged(object sender, EventArgs e)
        {
            throw new Exception("Not implemented");
        }

        private void checkBoxRunDungeons_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.Dungeons = checkBoxRunDungeons.Checked;
            SaveConfiguration();
        }

        private void checkBoxKillOnMaintenance_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.KillOnMaintenance = checkBoxKillOnMaintenance.Checked;
            SaveConfiguration();
        }
        #endregion

        #region Settings Controls
        private void borderButtonImportSettings_Click(object sender, EventArgs e)
        {
            throw new Exception("Not implemented");
        }

        private void borderButtonExportSettings_Click(object sender, EventArgs e)
        {
            throw new Exception("Not implemented");
        }

        private void checkBoxShowConsole_CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.ShowConsoleOutput = checkBoxShowConsole.Checked;
            if (checkBoxShowConsole.Checked)
            {
                ConsoleWindow.ShowConsole();
            }
            else
            {
                ConsoleWindow.HideConsole();
            }

            Program.SaveConfiguration();
        }
        #endregion

        #region Booster Controls
        private void dropDownMenuBoosterMission_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.QuestBoosterPreferredSelect = dropDownMenuBoosterMission.SelectionIndex switch
            {
                1 => QuestBoosterSelect.Small,
                2 => QuestBoosterSelect.Medium,
                3 => QuestBoosterSelect.Premium,
                _ => 0
            };
            SaveConfiguration();
        }

        private void dropDownMenuBoosterStats_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.StatBoosterPreferredSelect = dropDownMenuBoosterStats.SelectionIndex switch
            {
                1 => StatBoosterSelect.Small,
                2 => StatBoosterSelect.Medium,
                3 => StatBoosterSelect.Premium,
                _ => 0
            };
            SaveConfiguration();
        }

        private void dropDownMenuBoosterWork_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.WorkBoosterPreferredSelect = dropDownMenuBoosterWork.SelectionIndex switch
            {
                1 => WorkBoosterSelect.Small,
                2 => WorkBoosterSelect.Medium,
                3 => WorkBoosterSelect.Premium,
                _ => 0
            };
            SaveConfiguration();
        }

        private void dropDownMenuBoosterLeague_IndexChanged(object sender, EventArgs e)
        {
            _executionConfiguration.LeagueBoosterPreferredSelect = dropDownMenuBoosterLeague.SelectionIndex switch
            {
                1 => LeagueBoosterSelect.Medium,
                2 => LeagueBoosterSelect.Premium,
                _ => 0
            };
            SaveConfiguration();
        }
        #endregion

        private void checkBoxTeamJoinAttacks__CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.JoinClaimGuildFights = checkBoxTeamJoinAttacks.Checked;
            SaveConfiguration();
        }

        private void checkBoxTeamJoinDefense__CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.JoinClaimGuildDefenses = checkBoxTeamJoinDefense.Checked;
            SaveConfiguration();
        }

        private void checkBoxJoinDungeon__CheckedChanged(object sender, EventArgs e)
        {
            _executionConfiguration.JoinClaimGuildDungeons = checkBoxJoinDungeon.Checked;
            SaveConfiguration();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //theme
            if (Program.Settings.ThemeIndex == 0)
            {
                Program.Settings.ThemeIndex = 1;
                Program.Settings.Theme = Theme.Light;
                Console.WriteLine("Theme light");
            }
            else
            {
                Program.Settings.ThemeIndex = 0;
                Program.Settings.Theme = Theme.Dark;
                Console.WriteLine("Theme dark");
            }

            ((IThemeable)this).OnThemeChanged(Program.Settings!.Theme);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //language
            if (Program.Settings.Language == Language.Polish)
            {
                Program.Settings.Language = Language.English;
                Console.WriteLine("Language english");
            }
            else
            {
                Program.Settings.Language = Language.Polish;
                Console.WriteLine("Language polish");
            }

            ((ITranslatable)this).OnLanguageChanged(Program.Settings!.Language);
        }
    }
}
