using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Character
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("name")]
        public string Name = "";

        [JsonProperty("game_currency")]
        public int GoldCoins;

        [JsonProperty("xp")]
        public int Experience;

        [JsonProperty("level")]
        public int Level;

        [JsonProperty("stat_total_stamina")]
        public int Stamina;

        [JsonProperty("stat_total_strength")]
        public int Strength;

        [JsonProperty("stat_total_critical_rating")]
        public int CriticalRating;

        [JsonProperty("stat_total_dodge_rating")]
        public int DodgeRating;

        [JsonProperty("stat_weapon_damage")]
        public int WeaponDamage;

        
        [JsonProperty("active_quest_booster_id")]
        public string QuestBooster = "";

        [JsonProperty("ts_active_quest_boost_expires")]
        public long TimeQuestBoosterExpires;


        [JsonProperty("active_stats_booster_id")]
        public string StatsBooster = "";

        [JsonProperty("ts_active_stats_boost_expires")]
        public long TimeStatsBoosterExpires;


        [JsonProperty("active_work_booster_id")]
        public string WorkBooster = "";

        [JsonProperty("ts_active_work_boost_expires")]
        public long TimeWorkBoosterExpires;


        [JsonProperty("active_league_booster_id")]
        public string LeagueBooster = "";

        [JsonProperty("ts_active_league_boost_expires")]
        public long TimeLeagueBoosterExpires;


        /*[JsonProperty("ts_active_multitasking_boost_expires")]
        public long MultitaskingExpires;
        */

        [JsonProperty("quest_energy")]
        public int MissionEnergy;

        [JsonProperty("quest_energy_refill_amount_today")]
        public int MissionEnergyRefilled;

        [JsonProperty("active_quest_id")]
        public int ActiveMissionId;

        [JsonProperty("honor")]
        public int Honor;

        [JsonProperty("ts_last_duel")]
        public long TimeLastDuel;

        [JsonProperty("active_duel_id")]
        public int ActiveDuelId;

        [JsonProperty("duel_stamina")]
        public int DuelEnergy;

        [JsonProperty("duel_stamina_cost")]
        public int DuelEnergyCost;

        [JsonProperty("active_training_id")]
        public int ActiveTrainingId;

        [JsonProperty("ts_last_training_finished")]
        public long TimeTrainingFinished;

        [JsonProperty("ts_last_training_refresh")]
        public long TimeTrainingRefreshed;

        [JsonProperty("training_energy")]
        public int TrainingQuestEnergy;

        //[JsonProperty("max_training_energy")]
        //public int MaxTrainingQuestEnergy;

        [JsonProperty("ts_last_training_energy_change")]
        public long TimeTrainingEnergyChange;


        [JsonProperty("training_count")]
        public int TrainingMotivationEnergy;

        [JsonProperty("active_worldboss_attack_id")]
        public int ActiveBossAttackId;

        [JsonProperty("active_dungeon_quest_id")]
        public int ActiveDungeonQuestId;

        [JsonProperty("active_story_dungeon_attack_id")]
        public int ActiveDungeonAttackId;

        [JsonProperty("ts_last_dungeon_quest_fail")]
        public long TimeDungeonFailed;

        [JsonProperty("tutorial_flags")] //its escaped json string...
        public string TutorialFlagsJson = "";

        [JsonProperty("guild_id")]
        public int GuildId;

        [JsonProperty("finished_guild_battle_attack_id")]
        public int FinishedGuildAttackId;

        [JsonProperty("finished_guild_battle_defense_id")]
        public int FinishedGuildDefenseId;

        [JsonProperty("finished_guild_dungeon_battle_id")]
        public int FinishedGuildDungeonId;

        [JsonProperty("worldboss_event_id")]
        public int WorldbossId;

        [JsonProperty("ts_last_daily_login_bonus")]
        public int TimeDailyBonusClaimed;

        [JsonProperty("daily_login_bonus_day")]
        public int DailyBonusStreak;

        [JsonProperty("treasure_event_id")]
        public int TreasureEventId;

        //those are batteries and something else? maybe vouchers too?
        //unused_resources "{\"1\":15,\"2\":1,\"4\":7}"
        //used_resources

        [JsonProperty("league_points")]
        public int LeaguePoints;

        [JsonProperty("league_group_id")]
        public int LeagueGroupId;

        [JsonProperty("active_league_fight_id")]
        public int ActiveLeagueFightId;

        [JsonProperty("ts_last_league_fight")]
        public long TimeLastLeagueFight;

        [JsonProperty("league_fight_count")]
        public int LeagueFightsComplete;

        [JsonProperty("ts_last_league_opponents_refresh")]
        public long TimeLeagueRefreshed;

        [JsonProperty("league_stamina")]
        public int LeagueEnergy;

        [JsonProperty("ts_last_league_stamina_change")]
        public long TimeLeagueEnergyChange;

        [JsonProperty("league_stamina_cost")]
        public int LeagueEnergyCost;

        [JsonProperty("new_user_voucher_ids")]
        [JsonConverter(typeof(ArrayStringConverter<int>))]
        public int[] UnusedVoucherIds = [];
    }
}
