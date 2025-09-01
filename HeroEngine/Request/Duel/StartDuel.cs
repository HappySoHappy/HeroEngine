using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request.Duel
{
    public class StartDuel : Request
    {
        public int OpponentId;
        public StartDuel(Account account, int opponentId) : base(account, "startDuel")
        {
            OpponentId = opponentId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["character_id"] = OpponentId;
            data["use_premium"] = "false";
            data["refresh_opponents"] = "true";
            data["server_id"] = _account.Server;

            return data;
        }

        // {"data":{"user":{"id":14870,"premium_currency":7},"character":{"id":14809,"stat_total_stamina":12,"stat_total_strength":13,"stat_total_critical_rating":11,"stat_total_dodge_rating":15,"active_quest_booster_id":"","active_stats_booster_id":"","active_work_booster_id":"","ts_active_sense_boost_expires":0,"active_league_booster_id":"","ts_active_league_boost_expires":0,"ts_active_multitasking_boost_expires":0,"ts_active_training_sense_boost_expires":0,"quest_energy":100,"max_quest_energy":100,"ts_last_duel":1732298805,"active_duel_id":2719907,"duel_stamina":77,"max_duel_stamina":100,"ts_last_duel_stamina_change":1732298771,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":1731626077,"training_count":10,"max_training_count":10,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":0,"guild_rank":0,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":0,"guild_donated_game_currency":0,"guild_donated_premium_currency":0,"guild_competition_points_gathered":0,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":0,"event_quest_id":14268,"hidden_object_event_quest_id":0,"draw_event_id":0,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":0,"active_league_fight_id":0,"league_fight_count":0,"league_opponents":"","ts_last_league_opponents_refresh":0,"league_stamina":20,"max_league_stamina":20,"ts_last_league_stamina_change":1732298805,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":0,"new_user_voucher_ids":"[]"},"duel":{"id":2719907,"ts_creation":1732298805,"battle_id":10648231,"character_a_id":14809,"character_b_id":16442,"character_a_status":1,"character_b_status":1,"character_a_rewards":"{\"coins\":6,\"honor\":119}","character_b_rewards":"{\"coins\":1,\"honor\":-15}"},"battle":{"id":10648231,"ts_creation":1732298805,"profile_a_stats":"{\"profile\":\"a\",\"level\":11,\"stamina\":12,\"strength\":13,\"criticalrating\":11,\"dodgerating\":15,\"weapondamage\":0,\"season_effect\":{\"1054\":0.06},\"hitpoints\":120,\"damage_normal\":13,\"damage_bonus\":56,\"chance_critical\":0.128,\"chance_dodge\":0.1,\"used_damagebonus1_charges\":5}","profile_b_stats":"{\"profile\":\"b\",\"level\":3,\"stamina\":13,\"strength\":13,\"criticalrating\":10,\"dodgerating\":15,\"weapondamage\":0,\"season_effect\":{\"1054\":0.05},\"oc\":[],\"hitpoints\":130,\"damage_normal\":13,\"damage_bonus\":13,\"chance_critical\":0.011,\"chance_dodge\":0.1}","winner":"a","rounds":"{\"rounds\":[{\"a\":\"b\",\"d\":\"a\",\"r\":1},{\"a\":\"a\",\"d\":\"b\",\"m\":1,\"r\":1},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":15},{\"a\":\"a\",\"d\":\"b\",\"m\":1,\"r\":2,\"v\":55},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":17},{\"a\":\"a\",\"d\":\"b\",\"m\":1,\"r\":2,\"v\":44},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":13},{\"a\":\"a\",\"d\":\"b\",\"m\":1,\"r\":1},{\"a\":\"b\",\"d\":\"a\",\"r\":2,\"v\":14},{\"a\":\"a\",\"d\":\"b\",\"m\":1,\"r\":2,\"v\":40}],\"profile_a_appearance\":{\"name\":\"cehib96608\",\"gender\":\"f\",\"appearance_skin_color\":1,\"appearance_hair_color\":1,\"appearance_hair_type\":39,\"appearance_head_type\":5,\"appearance_eyes_type\":8,\"appearance_eyebrows_type\":5,\"appearance_nose_type\":6,\"appearance_mouth_type\":2,\"appearance_facial_hair_type\":0,\"appearance_decoration_type\":0,\"show_mask\":true,\"show_cape\":true,\"boots\":\"boots_flipflops1\",\"missiles\":\"missiles_curlers\"},\"profile_b_appearance\":{\"name\":\"MaciejT\\u0142ok187\",\"gender\":\"m\",\"appearance_skin_color\":5,\"appearance_hair_color\":1,\"appearance_hair_type\":27,\"appearance_head_type\":6,\"appearance_eyes_type\":15,\"appearance_eyebrows_type\":14,\"appearance_nose_type\":3,\"appearance_mouth_type\":11,\"appearance_facial_hair_type\":3,\"appearance_decoration_type\":0,\"show_mask\":true,\"show_cape\":true,\"boots\":\"boots_flipflops1\",\"suit\":\"suit_bathrobe1\"}}"},"opponent":{"id":16442,"name":"MaciejT\u0142ok187","gender":"m","level":3,"stat_base_stamina":10,"stat_base_strength":13,"stat_base_critical_rating":10,"stat_base_dodge_rating":10,"stat_total_stamina":13,"stat_total_strength":13,"stat_total_critical_rating":10,"stat_total_dodge_rating":15,"stat_weapon_damage":0,"honor":298,"league_points":0,"league_group_id":0,"online_status":2,"appearance_skin_color":5,"appearance_hair_color":1,"appearance_hair_type":27,"appearance_head_type":6,"appearance_eyes_type":15,"appearance_eyebrows_type":14,"appearance_nose_type":3,"appearance_mouth_type":11,"appearance_facial_hair_type":3,"appearance_decoration_type":0,"show_mask":true,"show_cape":true},"opponent_inventory":{"id":15785,"mask_item_id":0,"cape_item_id":0,"suit_item_id":9996846,"belt_item_id":0,"boots_item_id":9996845,"missiles_item_id":0,"sidekick_id":0},"opponent_inventory_items":[{"id":9996845,"identifier":"boots_flipflops1"},{"id":9996846,"identifier":"suit_bathrobe1"}],"items":[{"charges":95,"id":9707352}],"inventory":{"id":14147,"character_id":14809,"mask_item_id":0,"cape_item_id":0,"suit_item_id":0,"belt_item_id":0,"boots_item_id":8528727,"weapon_item_id":0,"gadget_item_id":0,"missiles_item_id":9707352,"missiles1_item_id":-1,"missiles2_item_id":-1,"missiles3_item_id":-1,"missiles4_item_id":-1,"sidekick_id":0,"bag_item1_id":0,"bag_item2_id":0,"bag_item3_id":0,"bag_item4_id":0,"bag_item5_id":0,"bag_item6_id":0,"bag_item7_id":0,"bag_item8_id":0,"bag_item9_id":0,"bag_item10_id":0,"bag_item11_id":0,"bag_item12_id":0,"bag_item13_id":0,"bag_item14_id":0,"bag_item15_id":0,"bag_item16_id":0,"bag_item17_id":0,"bag_item18_id":0,"shop_item1_id":0,"shop_item2_id":8528728,"shop_item3_id":8528729,"shop_item4_id":8528730,"shop_item5_id":8528731,"shop_item6_id":8528732,"shop_item7_id":8528733,"shop_item8_id":8528734,"shop_item9_id":8528735,"shop2_item1_id":0,"shop2_item2_id":0,"shop2_item3_id":0,"shop2_item4_id":0,"shop2_item5_id":0,"shop2_item6_id":0,"shop2_item7_id":0,"shop2_item8_id":0,"shop2_item9_id":0,"item_set_data":"","sidekick_data":""},"current_goal_values":{"missles_fired":{"value":5,"current_value":5}},"time_correction":0.03724813461303711,"server_time":1732298805},"error":""}
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
