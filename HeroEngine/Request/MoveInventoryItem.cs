using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    //type 0 = move between bag


    //type 3 = equip
    //slot 8 = throwable

    //item_id=22622904&target_slot=8&action_type=3&action=moveInventoryItem&user_id=3093&user_session_id=gKZzibMoSDl0jLyOYQZDgjMP9GSarT&client_version=html5_237&build_number=130&auth=f64570eae0383d78c765a29555d6424c&rct=2&keep_active=true&device_type=web
    //{"data":{"user":{"id":3093,"premium_currency":40,"blocked_premium_currency":0},"character":{"id":3067,"stat_total_stamina":17459,"stat_total_strength":5144,"stat_total_critical_rating":17236,"stat_total_dodge_rating":17152,"active_quest_booster_id":"booster_quest3","active_stats_booster_id":"booster_stats3","active_work_booster_id":"booster_work2","ts_active_sense_boost_expires":0,"active_league_booster_id":"booster_league1","ts_active_league_boost_expires":1739614960,"ts_active_multitasking_boost_expires":-1,"ts_active_training_sense_boost_expires":0,"quest_energy":0,"max_quest_energy":100,"duel_stamina":9,"max_duel_stamina":100,"ts_last_duel_stamina_change":1738965553,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":1738916205,"training_count":0,"max_training_count":42,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":1,"guild_rank":2,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":0,"guild_donated_game_currency":0,"guild_donated_premium_currency":30,"guild_competition_points_gathered":102,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":1738911522,"event_quest_id":34690,"hidden_object_event_quest_id":0,"draw_event_id":0,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":1300001,"active_league_fight_id":0,"league_fight_count":20,"league_opponents":"[233,434,24047]","ts_last_league_opponents_refresh":0,"league_stamina":60,"max_league_stamina":60,"ts_last_league_stamina_change":1738965564,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":8,"new_user_voucher_ids":"[]"},"inventory":{"id":2249,"missiles_item_id":22622904,"bag_item3_id":0},"time_correction":0.02698993682861328,"server_time":1738965564},"error":""}

    //type 4 = unequip
    //item_id=22622904&target_slot=11&action_type=4&action=moveInventoryItem&user_id=3093&user_session_id=gKZzibMoSDl0jLyOYQZDgjMP9GSarT&client_version=html5_237&build_number=130&auth=f64570eae0383d78c765a29555d6424c&rct=2&keep_active=true&device_type=web
    //{"data":{"user":{"id":3093,"premium_currency":40,"blocked_premium_currency":0},"character":{"id":3067,"stat_total_stamina":17459,"stat_total_strength":5144,"stat_total_critical_rating":17236,"stat_total_dodge_rating":17152,"active_quest_booster_id":"booster_quest3","active_stats_booster_id":"booster_stats3","active_work_booster_id":"booster_work2","ts_active_sense_boost_expires":0,"active_league_booster_id":"booster_league1","ts_active_league_boost_expires":1739614960,"ts_active_multitasking_boost_expires":-1,"ts_active_training_sense_boost_expires":0,"quest_energy":0,"max_quest_energy":100,"duel_stamina":9,"max_duel_stamina":100,"ts_last_duel_stamina_change":1738965553,"duel_stamina_cost":20,"training_energy":30,"max_training_energy":30,"ts_last_training_energy_change":1738916205,"training_count":0,"max_training_count":42,"active_worldboss_attack_id":0,"active_dungeon_quest_id":0,"guild_id":1,"guild_rank":2,"finished_guild_battle_attack_id":0,"finished_guild_battle_defense_id":0,"finished_guild_dungeon_battle_id":0,"guild_donated_game_currency":0,"guild_donated_premium_currency":30,"guild_competition_points_gathered":102,"pending_guild_competition_tournament_rewards":0,"pending_solo_guild_competition_tournament_rewards":0,"worldboss_event_id":0,"worldboss_event_attack_count":0,"pending_tournament_rewards":0,"pending_global_tournament_rewards":0,"ts_last_shop_refresh":1738911522,"event_quest_id":34690,"hidden_object_event_quest_id":0,"draw_event_id":0,"treasure_event_id":0,"legendary_dungeon_id":0,"league_group_id":1300001,"active_league_fight_id":0,"league_fight_count":20,"league_opponents":"[233,434,24047]","ts_last_league_opponents_refresh":0,"league_stamina":60,"max_league_stamina":60,"ts_last_league_stamina_change":1738965565,"league_stamina_cost":20,"pending_league_tournament_rewards":0,"slotmachine_spin_count":8,"new_user_voucher_ids":"[]"},"inventory":{"id":2249,"missiles_item_id":0,"bag_item3_id":22622904},"time_correction":0.023426055908203125,"server_time":1738965565},"error":""}

    public class MoveInventoryItem : Request
    {
        public int ItemId;
        public int TargetSlot;
        public InventoryActionType InventoryAction;
        public MoveInventoryItem(Account account, int itemId, int targetSlot, InventoryActionType inventoryAction) : base(account, "moveInventoryItem")
        {
            ItemId = itemId;
            TargetSlot = targetSlot;
            InventoryAction = inventoryAction;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["item_id"] = ItemId;
            data["target_slot"] = TargetSlot;
            data["action_type"] = (int)InventoryAction;

            return data;
        }

        public static void Update(Account account, dynamic data)
        {
            if (data == null) return;

            var hz = account.HeroZero;
            if (hz == null) return;

            var game = hz.Data;
            if (game == null) return;

            JsonPropertyUpdater.UpdateFields(game, data);
        }

        public enum InventoryActionType
        {
            Move = 0,
            Equip = 3,
            Unequip = 4
        }
    }
}
