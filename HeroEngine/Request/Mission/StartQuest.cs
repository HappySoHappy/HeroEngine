using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request.Mission
{
    public class StartQuest : Request
    {
        public int MissionId;
        public StartQuest(Account account, int missionId) : base(account, "startQuest")
        {
            MissionId = missionId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["quest_id"] = MissionId;

            return data;
        }

        //{"data":{"user":{"id":454754,"premium_currency":9},"character":{"id":453039,"user_id":454754,"name":"kegedo4592","locale":"de_DE","gender":"f","title":"","game_currency":33,"xp":547,"level":2,"description":"","note":"","ts_last_action":1731808590,"online_status":1,"score_honor":3570002,"score_level":200000357,"stat_points_available":0,"stat_base_stamina":10,"stat_base_strength":10,"stat_base_critical_rating":11,"stat_base_dodge_rating":11,"stat_total_stamina":16,"stat_total_strength":11,"stat_total_critical_rating":23,"stat_total_dodge_rating":16,"stat_weapon_damage":3,"stat_total":69,"stat_bought_stamina":0,"stat_bought_strength":0,"stat_bought_critical_rating":0,"stat_bought_dodge_rating":0,"active_quest_booster_id":"booster_quest2","ts_active_quest_boost_expires":1732130167,"active_stats_booster_id":"","ts_active_stats_boost_expires":0,"active_work_booster_id":"","ts_active_work_boost_expires":0,"ts_active_sense_boost_expires":0,"active_league_booster_id":"","ts_active_league_boost_expires":0,"ts_active_multitasking_boost_expires":0,"ts_active_training_sense_boost_expires":0,"max_quest_stage":1,"current_quest_stage":1,"quest_energy":99,"max_quest_energy":100,"ts_last_quest_energy_refill":1731808590,"quest_energy_refill_amount_today":0,"quest_reward_training_sessions_rewarded_today":0,"active_quest_id":377149903,"quest_pool":"{\"1\":[377149902,377149903,377149904]}","honor":357,"ts_last_duel":1731785402,"active_duel_id":0,"duel_stamina":100,"max_duel_stamina":100,"ts_last_duel_stamina_change":1731808590,"duel_stamina_cost":20,"ts_last_duel_enemies_refresh":0,"current_work_offer_id":"work3","active_work_id":0,"active_training_id":0,"training_pool":"","ts_last_training_finished":0,"ts_last_training_refresh":0,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":0,"stat_trained_stamina":0,"stat_trained_strength":0,"stat_trained_critical_rating":0,"stat_trained_dodge_rating":0,"training_count":20,"max_training_count":10,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"ts_last_dungeon_quest_fail":0,"max_dungeon_index":0,"appearance_skin_color":1,"appearance_hair_color":1,"appearance_hair_type":14,"appearance_head_type":4,"appearance_eyes_type":14,"appearance_eyebrows_type":8,"appearance_nose_type":2,"appearance_mouth_type":1,"appearance_facial_hair_type":0,"appearance_decoration_type":0,"show_mask":true,"show_cape":true,"tutorial_flags":"{\"itemImprovementsUpdate\":true,\"mission_shown\":true,\"first_visit\":true,\"first_mission\":true,\"stats_spent\":true,\"shop_shown\":true,\"first_item\":true,\"duel_shown\":true,\"first_duel\":true,\"tutorial_finished\":true,\"dungeons\":true}","guild_id":0,"guild_rank":0,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":0,"guild_donated_game_currency":0,"guild_donated_premium_currency":0,"guild_competition_points_gathered":0,"pending_guild_competition_tournament_rewards":0,"new_guild_competition_tournament":false,"pending_solo_guild_competition_tournament_rewards":0,"unread_mass_system_messages":0,"friend_messages_only":false,"worldboss_event_id":0,"worldboss_event_attack_count":0,"ts_last_wash_item":0,"ts_last_daily_login_bonus":1731808592,"daily_login_bonus_day":1,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":0,"max_free_shop_refreshes":1,"shop_refreshes":0,"event_quest_id":0,"hidden_object_event_quest_id":0,"draw_event_id":27643,"treasure_event_id":0,"legendary_dungeon_id":0,"friend_data":"","unused_resources":"{\"1\":4,\"2\":1}","used_resources":"","league_points":0,"league_group_id":0,"active_league_fight_id":0,"ts_last_league_fight":0,"league_fight_count":0,"league_opponents":"","ts_last_league_opponents_refresh":0,"league_stamina":20,"max_league_stamina":20,"ts_last_league_stamina_change":1731808602,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"herobook_objectives_renewed_today":0,"current_slotmachine_spin":0,"slotmachine_spin_count":0,"ts_last_slotmachine_refill":1731808590,"new_user_voucher_ids":"[]","current_energy_storage":0,"current_training_storage":0,"active_story_dungeon_attack_id":0,"storygoal":"","active_season_progress_id":29293},"quest":{"id":377149903,"status":2,"ts_complete":1731808662},"time_correction":0.014820098876953125,"server_time":1731808602},"error":""}
        public static void Update(Account account, dynamic data)
        {
            if (data == null) return;

            var hz = account.HeroZero;
            if (hz == null) return;

            var game = hz.Data;
            if (game == null) return;

            JsonPropertyUpdater.UpdateFields(game, data);

            var missionUpdate = JsonConvert.DeserializeObject<Model.Mission>(JsonConvert.SerializeObject(data.quest));
            var missionFound = game.Missions.Where(mission => mission.Id == missionUpdate.Id).FirstOrDefault();
            if (missionFound != null)
            {
                missionFound.Id = missionUpdate.Id;
                missionFound.TimeComplete = missionUpdate.TimeComplete;
            }
        }
    }
}
