using HeroEngine.Persistance;
using Newtonsoft.Json;
using static HeroEngine.Persistance.ExecutionConfiguration;

namespace HeroEngine.Desktop
{
    public partial class BackendForm : Form
    {
        private ExecutionConfiguration? _config;
        private ExecutionCoordinator? _executionCoordinator;
        private List<Account> _accounts = new List<Account>();
        public BackendForm()
        {
            InitializeComponent();

            LoadConfiguration();
            UpdateUI();
        }

        private void LoadConfiguration()
        {
            try
            {
                string jsonString = File.ReadAllText("config.json");
                _config = JsonConvert.DeserializeObject<ExecutionConfiguration>(jsonString);
            }
            catch
            {
                _config = new ExecutionConfiguration()
                {
                    KillOnMaintenance = true,
                    ReloginDelay = 120,
                    ClaimLoginBonus = true,
                    AcceptBatteryRequests = true,
                    Training = true,
                    Duels = true,
                    League = true,
                    WorldbossVillain = true,
                    Missions = true,
                    HideoutAttacks = true,
                    HideoutCollectResources = true,
                    Treasure = true
                };
            }

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

        private void SaveConfiguration()
        {
            if (_config == null) return;

            try
            {
                string jsonString = JsonConvert.SerializeObject(_config, Formatting.Indented);
                File.WriteAllText("config.json", jsonString);
            }
            catch
            {

            }

            try
            {
                string accountsString = JsonConvert.SerializeObject(_accounts, Formatting.Indented);
                File.WriteAllText("accounts.json", accountsString);
            }
            catch
            {

            }
        }

        private void UpdateUI()
        {
            if (_config == null)
            {
                return;
            }

            checkBox1.Checked = _config.KillOnMaintenance;
            numericUpDown1.Value = _config.ReloginDelay;
            checkBox14.Checked = _config.ClaimLoginBonus;
            checkBox2.Checked = _config.Training;

            TrainingSelect selected = _config.TrainingPreferredSelect;

            checkBox3.Checked = (selected & TrainingSelect.Strength) == TrainingSelect.Strength;
            checkBox4.Checked = (selected & TrainingSelect.Stamina) == TrainingSelect.Stamina;
            checkBox5.Checked = (selected & TrainingSelect.DodgeRating) == TrainingSelect.DodgeRating;
            checkBox6.Checked = (selected & TrainingSelect.CriticalRating) == TrainingSelect.CriticalRating;
            checkBox7.Checked = _config.TrainingShortest;

            checkBox10.Checked = _config.Missions;
            radioButton1.Checked = !_config.MissionAllowTimed;
            radioButton2.Checked = _config.MissionAllowTimed;
            numericUpDown2.Value = _config.MissionMaxEnergy;
            checkBox11.Checked = _config.MissionAllowTimed;

            checkBox8.Checked = _config.Duels;
            checkBox9.Checked = _config.League;

            checkBox12.Checked = _config.HideoutAttacks;
            checkBox13.Checked = _config.HideoutCollectResources;

            checkBox15.Checked = _config.Treasure;

            checkBox16.Checked = _config.PreferEventMissions;

            checkBox17.Checked = _config.AcceptBatteryRequests;

            numericUpDown3.Value = _config.MissionMinimumRatio;

            checkBox18.Checked = _config.DuelsAttackTeamMembers;

            checkBox19.Checked = _config.LeagueAttackTeamMembers;

            checkBox20.Checked = _config.WorldbossVillain;

            label1.Text = "Konta:";
            foreach (var acc in _accounts)
            {
                label1.Text += $"\n{acc.Server} {acc.Email}";
            }
        }

        private void borderButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rjTextBox1.Texts) || string.IsNullOrEmpty(rjTextBox2.Texts) || string.IsNullOrEmpty(rjTextBox3.Texts)) return;
            if (_accounts.Where(acc => acc.Email == rjTextBox1.Texts && acc.Password == rjTextBox2.Texts && acc.Server == rjTextBox3.Texts).Count() > 0)
            {
                return;
            }

            _accounts.Add(new Account() { Email = rjTextBox1.Texts, Password = rjTextBox2.Texts, Server = rjTextBox3.Texts });

            rjTextBox1.Texts = "";
            rjTextBox2.Texts = "";
            rjTextBox3.Texts = "";

            UpdateUI();
        }

        private void borderButton2_Click(object sender, EventArgs e)
        {
            _accounts.Clear();
            if (_executionCoordinator != null)
            {
                _executionCoordinator.Abort();
                _executionCoordinator = null;

                borderButton3.Text = "Start";
            }

            UpdateUI();
        }

        private void borderButton3_Click(object sender, EventArgs e)
        {
            if (_executionCoordinator == null)
            {
                _executionCoordinator = new ExecutionCoordinator(_accounts, _config);
                _executionCoordinator.Start();

                borderButton3.Text = "Stop";
            }
            else
            {
                _executionCoordinator.Abort();
                _executionCoordinator = null;

                borderButton3.Text = "Start";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _config.KillOnMaintenance = checkBox1.Checked;
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            _config.Treasure = checkBox14.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            _config.ReloginDelay = (int)numericUpDown1.Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _config.Training = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                _config.TrainingPreferredSelect |= TrainingSelect.Strength;
            else
                _config.TrainingPreferredSelect &= ~TrainingSelect.Strength;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                _config.TrainingPreferredSelect |= TrainingSelect.Stamina;
            else
                _config.TrainingPreferredSelect &= ~TrainingSelect.Stamina;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                _config.TrainingPreferredSelect |= TrainingSelect.DodgeRating;
            else
                _config.TrainingPreferredSelect &= ~TrainingSelect.DodgeRating;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                _config.TrainingPreferredSelect |= TrainingSelect.CriticalRating;
            else
                _config.TrainingPreferredSelect &= ~TrainingSelect.CriticalRating;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            _config.TrainingShortest = checkBox7.Checked;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            _config.Missions = checkBox10.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                _config.MissionPreferredSelect = MissionSelect.ExpRatio;
            else
                _config.MissionPreferredSelect = MissionSelect.GoldRatio;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                _config.MissionPreferredSelect = MissionSelect.GoldRatio;
            else
                _config.MissionPreferredSelect = MissionSelect.ExpRatio;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            _config.MissionMaxEnergy = (int)numericUpDown2.Value;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            _config.MissionAllowTimed = checkBox11.Checked;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            _config.Duels = checkBox8.Checked;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            _config.League = checkBox9.Checked;
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            _config.HideoutAttacks = checkBox12.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            _config.HideoutCollectResources = checkBox13.Checked;
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            _config.Treasure = checkBox15.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            _config.PreferEventMissions = checkBox16.Checked;
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            _config.AcceptBatteryRequests = checkBox17.Checked;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            _config.MissionMinimumRatio = (int)numericUpDown3.Value;
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            _config.DuelsAttackTeamMembers = checkBox18.Checked;
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            _config.LeagueAttackTeamMembers = checkBox19.Checked;
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            _config.WorldbossVillain = checkBox20.Checked;
        }
    }
}
