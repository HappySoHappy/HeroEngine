namespace HeroEngine.Persistance
{
    public class ExecutionConfiguration
    {
        public string __WARNING__ { get { return "Manually modifying this file will cause it to reset, if invalid JSON schema or values are used"; } set { } }
        public bool ShowConsoleOutput { get; set; } = false;
        //public bool SaveConsoleOutput { get; set; } = false;
        public bool KillOnMaintenance { get; set; } = false;
        public long ReloginDelay { get; set; } = 120;
        public bool ClaimLoginBonus { get; set; } = false;
        public bool AcceptBatteryRequests { get; set; } = false;

        public bool Training { get; set; } = false;
        public TrainingSelect TrainingPreferredSelect { get; set; } = 0;
        public bool TrainingShortest { get; set; } = false;
        public TrainingCouponSelect TrainingCouponPreferredSelect { get; set; } = TrainingCouponSelect.None;

        public  bool Duels { get; set; } = false;
        public bool DuelsAttackTeamMembers { get; set; } = false;
        public bool DuelsUnequipThrowables { get; set; } = false;
        //use leaderboard
        //dont attack lost defenders
        //attack teammates
        //counter duels that were won

        public bool League { get; set; } = false;
        public bool LeagueAttackTeamMembers { get; set; } = false;
        //attack teammates
        public bool LeagueUnequipThrowables { get; set; } = false;

        public bool WorldbossVillain = false;

        public bool Missions { get; set; } = false;
        public bool PreferEventMissions { get; set; } = true;
        public MissionSelect MissionPreferredSelect { get; set; } = MissionSelect.ExpRatio;
        public int MissionMaxEnergy { get; set; } = 0;
        public bool MissionAllowTimed = false;
        public int MissionMinimumRatio = 500;
        public MissionCouponSelect MissionCouponPreferredSelect { get; set; } = MissionCouponSelect.None;


        public bool HideoutAttacks { get; set; } = false;
        public bool HideoutCollectResources { get; set; } = false;
        public bool HideoutCollectChests { get; set; } = false;

        public bool Treasure { get; set; } = false;
        //add autosolver

        public bool Dungeons { get; set; } = false;

        public bool RedeemVouchersLater { get; set; } = false;
        public bool ClaimDailyBonusRewards { get; set; } = false;

        public QuestBoosterSelect QuestBoosterPreferredSelect { get; set; } = QuestBoosterSelect.None;
        public StatBoosterSelect StatBoosterPreferredSelect { get; set; } = StatBoosterSelect.None;
        public WorkBoosterSelect WorkBoosterPreferredSelect { get; set; } = WorkBoosterSelect.None;
        public LeagueBoosterSelect LeagueBoosterPreferredSelect { get; set; } = LeagueBoosterSelect.None;

        public InventorySellSelect InventorySellPreferredSelect { get; set; } = InventorySellSelect.None;

        public DuelOpponentSelect DuelOpponentPreferredSelect { get; set; } = DuelOpponentSelect.Default;

        public bool JoinClaimGuildFights = false;
        public bool JoinClaimGuildDefenses = false;
        public bool JoinClaimGuildDungeons = false;

        [Flags]
        public enum TrainingSelect
        {
            Strength = 1 << 0,       // 1
            Stamina = 1 << 1,        // 2
            DodgeRating = 1 << 2,    // 4
            CriticalRating = 1 << 3  // 8
        }

        public enum TrainingCouponSelect
        {
            None,
            Oldest,
            LeastMotivation,
            MostMotivation
        }

        public enum MissionCouponSelect
        {
            None,
            Oldest,
            LeastEnergy,
            MostEnergy
        }

        public enum MissionSelect
        {
            ExpRatio,
            GoldRatio,
        }

        public enum QuestBoosterSelect
        {
            None,
            Small,
            Medium,
            Premium
        }

        public enum StatBoosterSelect
        {
            None,
            Small,
            Medium,
            Premium
        }

        public enum WorkBoosterSelect
        {
            None,
            Small,
            Medium,
            Premium
        }

        public enum LeagueBoosterSelect
        {
            None,
            Medium,
            Premium,
        }

        public enum InventorySellSelect
        {
            None,
            Worse,
            All
        }

        public enum DuelOpponentSelect
        {
            Default,
            Honor,
            Level
        }
    }
}
