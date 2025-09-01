using HeroEngine.Model;
using Newtonsoft.Json;

namespace HeroEngine.Framework
{
    public class DataHolder
    {
        [JsonProperty("user")]
        public User User = new User();

        [JsonProperty("character")]
        public Character Character = new Character();

        [JsonProperty("inventory")]
        public Inventory Inventory = new Inventory();

        [JsonProperty("guild")]
        public Guild? Guild;

        [JsonProperty("guild_members")]
        public List<GuildMember>? GuildMembers;

        [JsonProperty("guild_competition_data")]
        public GuildCompetition? GuildCompetition;

        [JsonProperty("pending_guild_battle_attack")]
        public GuildAttack? PendingGuildAttack;

        [JsonProperty("pending_guild_battle_defense")]
        public GuildDefense? PendingGuildDefense;

        [JsonProperty("pending_guild_dungeon_battle")]
        public GuildDungeon? PendingGuildDungeon;

        [JsonProperty("finished_guild_battle_attack")]
        public GuildAttack? FinishedGuildAttack;

        [JsonProperty("finished_guild_battle_defense")]
        public GuildDefense? FinishedGuildDefense;

        [JsonProperty("finished_guild_dungeon_battle")]
        public GuildDungeon? FinishedGuildDungeon;

        [JsonProperty("quests")]
        public List<Mission> Missions = new List<Mission>();

        [JsonProperty("items")]
        public List<Item> Items = new List<Item>();

        [JsonProperty("item_improvements")]
        public List<ItemImprovement> ItemImprovements = new List<ItemImprovement>();

        [JsonProperty("dungeons")]
        public List<Dungeon> Dungeons = new List<Dungeon>();

        [JsonProperty("dungeon_quests")]
        public List<DungeonQuest> DungeonQuests = new List<DungeonQuest>();

        [JsonProperty("training")]
        public Training? ActiveTraining;

        [JsonProperty("training_quests")]
        public List<TrainingQuest>? ActiveTrainingQuests;

        [JsonProperty("trainings")]
        public List<Training>? Trainings;

        [JsonProperty("worldboss_event_character_data")]
        public List<WorldBoss> ActiveWorldBosses = new List<WorldBoss>();

        [JsonProperty("treasure_event")]
        public TreasureEvent? ActiveTreasureEvent;

        [JsonProperty("daily_login_bonus_rewards")]
        public LoginBonusRewards? UnclaimedLoginRewards;

        [JsonProperty("current_goal_values")]
        public CurrentGoalValues Goals = new CurrentGoalValues();

        [JsonProperty("current_optical_changes")]
        public CurrentOpticalChanges OpticalChanges = new CurrentOpticalChanges();

        [JsonProperty("missed_duels")]
        public int MissedDuelAttacks;

        [JsonProperty("missed_league_fights")]
        public int MissedLeagueAttacks;

        [JsonProperty("streams_info")]
        public Dictionary<string, Dictionary<string, StreamInfo>> StreamInfo = new Dictionary<string, Dictionary<string, StreamInfo>>();

        [JsonProperty("hideout")]
        public Hideout? Hideout;

        [JsonProperty("hideout_rooms")]
        public List<HideoutRoom>? HideoutRooms;



        [JsonProperty("story_dungeon_lookup")]
        public DungeonLookup? DungeonLookup; // may be null for new accoutns?

        [JsonProperty("story_dungeon_steps")]
        public List<DungeonStep>? DungeonSteps;

        [JsonProperty("daily_bonus_rewards")]
        public List<DailyBonusReward>? UnlockedBonusRewards;


        [JsonProperty("league_locked")]
        public bool LeagueLocked; // true when tutorial

        #region Bot-Defined fields

        [JsonIgnore]
        public List<Voucher> Vouchers = new List<Voucher>();

        [JsonProperty("worldboss_attack")]
        public WorldBossAttack? ActiveWorldBossAttack;

        [JsonProperty("worldboss_event")]
        public WorldBossEvent? ActiveWorldBossEvent;

        #endregion

    }
}
