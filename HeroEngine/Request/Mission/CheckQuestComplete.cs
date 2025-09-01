using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Mission
{
    public class CheckQuestComplete : Request
    {
        public CheckQuestComplete(Account account) : base(account, "checkForQuestComplete")
        {
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["quest_id"] = 0;

            return data;
        }

        //{"data":{"user":{"id":454754,"premium_currency":9},"character":{"id":453039,"stat_total_stamina":16,"stat_total_strength":11,"stat_total_critical_rating":23,"stat_total_dodge_rating":16,"active_quest_booster_id":"booster_quest2","active_stats_booster_id":"","active_work_booster_id":"","ts_active_sense_boost_expires":0,"active_league_booster_id":"","ts_active_league_boost_expires":0,"ts_active_multitasking_boost_expires":0,"ts_active_training_sense_boost_expires":0,"quest_energy":99,"max_quest_energy":100,"duel_stamina":100,"max_duel_stamina":100,"ts_last_duel_stamina_change":1731808590,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":0,"training_count":20,"max_training_count":10,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":0,"guild_rank":0,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":0,"guild_donated_game_currency":0,"guild_donated_premium_currency":0,"guild_competition_points_gathered":0,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":0,"event_quest_id":0,"hidden_object_event_quest_id":0,"draw_event_id":27643,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":0,"active_league_fight_id":0,"league_fight_count":0,"league_opponents":"","ts_last_league_opponents_refresh":0,"league_stamina":20,"max_league_stamina":20,"ts_last_league_stamina_change":1731808666,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":0,"new_user_voucher_ids":"[]"},"battle":{"id":114414612,"ts_creation":1731808666,"profile_a_stats":"{\"profile\":\"a\",\"level\":2,\"stamina\":16,\"strength\":11,\"criticalrating\":23,\"dodgerating\":16,\"weapondamage\":3,\"hitpoints\":160,\"damage_normal\":14,\"damage_bonus\":14,\"chance_critical\":0.123,\"chance_dodge\":0.151}","profile_b_stats":"{\"profile\":\"b\",\"level\":2,\"stamina\":14,\"strength\":10,\"criticalrating\":21,\"dodgerating\":15,\"weapondamage\":3,\"hitpoints\":140,\"damage_normal\":13,\"damage_bonus\":13,\"chance_critical\":0.012,\"chance_dodge\":0.075}","winner":"a","rounds":"{\"rounds\":[{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":10},{\"a\":\"a\",\"d\":\"b\",\"r\":3,\"v\":26},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":11},{\"a\":\"a\",\"d\":\"b\",\"r\":2,\"v\":16},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":16},{\"a\":\"a\",\"d\":\"b\",\"r\":2,\"v\":16},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":11},{\"a\":\"a\",\"d\":\"b\",\"r\":2,\"v\":13},{\"a\":\"b\",\"d\":\"a\",\"r\":1},{\"a\":\"a\",\"d\":\"b\",\"r\":3,\"v\":35},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":13},{\"a\":\"a\",\"d\":\"b\",\"r\":2,\"v\":16},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":14},{\"a\":\"a\",\"d\":\"b\",\"r\":2,\"v\":14},{\"a\":\"b\",\"d\":\"a\",\"r\":1},{\"a\":\"a\",\"d\":\"b\",\"r\":2,\"v\":16}],\"profile_a_appearance\":null,\"profile_b_appearance\":null}"},"quest":{"id":377149903,"status":4,"fight_battle_id":114414612},"time_correction":0.01782083511352539,"server_time":1731808666},"error":""}
        public static void Update(Account account, dynamic data)
        {
            if (data == null) return;

            var hz = account.HeroZero;
            if (hz == null) return;

            var game = hz.Data;
            if (game == null) return;

            JsonPropertyUpdater.UpdateFields(game, data);
        }
    }
}
