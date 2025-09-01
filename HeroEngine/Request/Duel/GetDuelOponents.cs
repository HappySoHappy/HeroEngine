using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Duel
{
    public class GetDuelOponents : Request
    {
        public GetDuelOponents(Account account) : base(account, "getDuelOpponents")
        {
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            return data;
        }

        //{"data":{"character":{"ts_last_duel_enemies_refresh":0,"honor":162,"stat_total_stamina":21,"stat_total_strength":15,"stat_total_critical_rating":10,"stat_total_dodge_rating":12,"active_quest_booster_id":"","active_stats_booster_id":"","active_work_booster_id":"","ts_active_sense_boost_expires":0,"active_league_booster_id":"","ts_active_league_boost_expires":0,"ts_active_multitasking_boost_expires":0,"ts_active_training_sense_boost_expires":0,"quest_energy":80,"max_quest_energy":100,"duel_stamina":100,"max_duel_stamina":100,"ts_last_duel_stamina_change":1732118622,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":1732033526,"training_count":10,"max_training_count":10,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":0,"guild_rank":0,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":0,"guild_donated_game_currency":0,"guild_donated_premium_currency":0,"guild_competition_points_gathered":0,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":0,"event_quest_id":13978,"hidden_object_event_quest_id":0,"draw_event_id":0,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":0,"active_league_fight_id":0,"league_fight_count":0,"league_opponents":"","ts_last_league_opponents_refresh":0,"league_stamina":20,"max_league_stamina":20,"ts_last_league_stamina_change":1732143070,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":0,"new_user_voucher_ids":"[]","id":14854},"opponents":[{"id":15500,"server_id":"pl34","name":"OgKlimek","level":3,"honor":171,"gender":"m","has_missile":false,"stat_total_stamina":10,"stat_total_strength":22,"stat_total_critical_rating":13,"stat_total_dodge_rating":10,"stat_weapon_damage":3,"online_status":2,"total_stats":58},{"id":12798,"server_id":"pl34","name":"Kubitos","level":2,"honor":100,"gender":"m","has_missile":false,"stat_total_stamina":15,"stat_total_strength":21,"stat_total_critical_rating":12,"stat_total_dodge_rating":10,"stat_weapon_damage":0,"online_status":2,"total_stats":58},{"id":12803,"server_id":"pl34","name":"gewafe8209","level":5,"honor":163,"gender":"f","has_missile":false,"stat_total_stamina":12,"stat_total_strength":15,"stat_total_critical_rating":13,"stat_total_dodge_rating":11,"stat_weapon_damage":0,"online_status":2,"total_stats":51},{"id":9068,"server_id":"pl34","name":"fdsaf","level":2,"honor":162,"gender":"m","has_missile":false,"stat_total_stamina":10,"stat_total_strength":12,"stat_total_critical_rating":10,"stat_total_dodge_rating":14,"stat_weapon_damage":0,"online_status":2,"total_stats":46},{"id":9671,"server_id":"pl34","name":"GoStayAwake","level":2,"honor":162,"gender":"m","has_missile":false,"stat_total_stamina":12,"stat_total_strength":12,"stat_total_critical_rating":10,"stat_total_dodge_rating":16,"stat_weapon_damage":3,"online_status":2,"total_stats":53},{"id":13455,"server_id":"pl34","name":"ZasilekSpoleczny","level":3,"honor":162,"gender":"m","has_missile":false,"stat_total_stamina":11,"stat_total_strength":10,"stat_total_critical_rating":11,"stat_total_dodge_rating":13,"stat_weapon_damage":0,"online_status":2,"total_stats":45},{"id":13879,"server_id":"pl34","name":"Brzuchom\u00f3wca","level":2,"honor":345,"gender":"m","has_missile":false,"stat_total_stamina":18,"stat_total_strength":15,"stat_total_critical_rating":10,"stat_total_dodge_rating":12,"stat_weapon_damage":3,"online_status":2,"total_stats":58},{"id":12554,"server_id":"pl34","name":"kandzio","level":3,"honor":391,"gender":"m","has_missile":false,"stat_total_stamina":15,"stat_total_strength":12,"stat_total_critical_rating":12,"stat_total_dodge_rating":16,"stat_weapon_damage":3,"online_status":2,"total_stats":58},{"id":12055,"server_id":"pl34","name":"daxonalka","level":2,"honor":166,"gender":"f","has_missile":false,"stat_total_stamina":15,"stat_total_strength":11,"stat_total_critical_rating":10,"stat_total_dodge_rating":10,"stat_weapon_damage":0,"online_status":2,"total_stats":46},{"id":9461,"server_id":"pl34","name":"kuba8","level":3,"honor":259,"gender":"m","has_missile":false,"stat_total_stamina":14,"stat_total_strength":15,"stat_total_critical_rating":13,"stat_total_dodge_rating":13,"stat_weapon_damage":3,"online_status":2,"total_stats":58}],"missed_duels":4,"time_correction":0.024652957916259766,"server_time":1732143070},"error":""}
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
