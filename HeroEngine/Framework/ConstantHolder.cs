using HeroEngine.Model;
using Newtonsoft.Json;

namespace HeroEngine.Framework
{
    public class ConstantHolder
    {
        [JsonProperty("coins_per_time_base")]
        public double CoinBase;

        [JsonProperty("coins_per_time_scale")]
        public double CoinScale;

        [JsonProperty("coins_per_time_level_scale")]
        public double CoinLevelScale;

        [JsonProperty("coins_per_time_level_exp")]
        public double CoinLevelExperience;

        [JsonProperty("duel_stamina_refresh_amount_per_minute_first_duel")]
        public double FirstDuelEnergyRefreshPerMinute;

        [JsonProperty("duel_stamina_refresh_amount_per_minute")]
        public double DuelEnergyRefreshPerMinute;

        [JsonProperty("quest_energy_refill_premium_amount")]
        public int MissionEnergyRefillDonutsCost;

        [JsonProperty("quest_max_refill_amount_per_day")]
        public int MissionEnergyRefillMax;

        [JsonProperty("quest_energy_refill_amount")]
        public int MissionEnergyRefillAmount;



        [JsonProperty("quest_energy_refill1_cost_factor")]
        public int MissionEnergyRefillFactor1;

        [JsonProperty("quest_energy_refill2_cost_factor")]
        public int MissionEnergyRefillFactor2;

        [JsonProperty("quest_energy_refill3_cost_factor")]
        public int MissionEnergyRefillFactor3;

        [JsonProperty("quest_energy_refill4_cost_factor")]
        public int MissionEnergyRefillFactor4;

        [JsonProperty("quest_energy_refill5_cost_factor")]
        public int MissionEnergyRefillFactor5;

        [JsonProperty("quest_energy_refill6_cost_factor")]
        public int MissionEnergyRefillFactor6;

        [JsonProperty("quest_energy_refill7_cost_factor")]
        public int MissionEnergyRefillFactor7;

        [JsonProperty("quest_energy_refill8_cost_factor")]
        public int MissionEnergyRefillFactor8;



        [JsonProperty("booster_small_costs_time")]
        public int BoosterSmallCost;

        [JsonProperty("booster_medium_costs_time")]
        public int BoosterMediumCost;

        [JsonProperty("booster_large_costs_premium_currency")]
        public int BoosterLargePremiumCost;

        [JsonProperty("booster_costs_coins_step")]
        public int BoosterCoinCostStep;

        [JsonProperty("booster_league2_costs_premium_currency_amount")]
        public int BoosterLeaguePremiumCost;



        [JsonProperty("inventory_bag2_unlock_level")]
        public int InventoryPage2Level;

        [JsonProperty("inventory_bag3_unlock_level")]
        public int InventoryPage3Level;


        [JsonProperty("dungeon_quest_cooldown")]
        public int DungeonDefeatCooldown;

        [JsonProperty("dungeon_repeat_cooldown")]
        public int DungeonRepeatCooldown;

        [JsonProperty("dungeon_repeat_cooldown_short")]
        public int DungeonRepeatShortCooldown;

        [JsonProperty("story_dungeon_attack_cooldown")]
        public int StoryDungeonAttackCooldown;

        //story_dungeon_attack_cooldown



        [JsonProperty("league_max_daily_league_fights")]
        public int LeagueFightsPerDay;

        [JsonProperty("league_min_honor_points")]
        public int LeagueMinimumHonor;

        [JsonProperty("league_stamina_refresh_amount_per_minute_first_fight_nonbooster")]
        public double FirstLeagueEnergyRefreshPerMinuteNoBooster;

        [JsonProperty("league_stamina_refresh_amount_per_minute_first_fight_booster1")]
        public double FirstLeagueEnergyRefreshPerMinuteBooster1;

        [JsonProperty("league_stamina_refresh_amount_per_minute_first_fight_booster2")]
        public double FirstLeagueEnergyRefreshPerMinuteBooster2;

        [JsonProperty("league_stamina_refresh_amount_per_minute")]
        public double LeagueEnergyRefreshPerMinute;

        [JsonProperty("hideout_battle_cooldown")]
        public int HideoutLostFightCooldownMinutes;

        [JsonProperty("training_cooldown")]
        public int TrainingCooldown;

        [JsonProperty("event_treasure_free_reveal_item_cooldown")]
        public int TreasureEventCollectCooldown;

        [JsonProperty("guild_artifacts")]
        public Dictionary<string, GuildArtifact> GuildArtifacts = new Dictionary<string, GuildArtifact>();
    }
}
