using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Hideout
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("hideout_points")]
        public int Points;

        [JsonProperty("idle_worker_count")]
        public int IdleWorkers;

        [JsonProperty("current_resource_glue")]
        public int Glue;

        [JsonProperty("max_resource_glue")]
        public int MaxGlue;

        [JsonProperty("current_resource_stone")]
        public int Zeronite;

        [JsonProperty("max_resource_stone")]
        public int MaxZeronite;

        [JsonProperty("current_attacker_units")]
        public int Attackers;

        /// <summary>
        /// Represents maximum attacker units per attack (this is value doesnt consider robot storage)
        /// </summary>
        [JsonProperty("max_attacker_units")]
        public int MaxAttackers;

        [JsonProperty("ts_last_opponent_refresh")]
        public long TimeOpponentRefreshed;

        [JsonProperty("active_battle_id")]
        public int ActiveBattleId;

        [JsonProperty("ts_last_lost_attack")]
        public long TimeLostAttack;

        [JsonProperty("current_barracks_level")]
        public int BarracksLevel;


        #region Rooms
#pragma warning disable CS0649
        //floor 1
        [JsonProperty("room_slot_0_0")]
        public int RoomSlot00;

        [JsonProperty("room_slot_0_1")]
        public int RoomSlot01;

        [JsonProperty("room_slot_0_2")]
        public int RoomSlot02;

        [JsonProperty("room_slot_0_3")]
        public int RoomSlot03;

        [JsonProperty("room_slot_0_4")]
        public int RoomSlot04;

        //floor 2
        [JsonProperty("room_slot_1_0")]
        public int RoomSlot10;

        [JsonProperty("room_slot_1_1")]
        public int RoomSlot11;

        [JsonProperty("room_slot_1_2")]
        public int RoomSlot12;

        [JsonProperty("room_slot_1_3")]
        public int RoomSlot13;

        [JsonProperty("room_slot_1_4")]
        public int RoomSlot14;

        //floor 3
        [JsonProperty("room_slot_2_0")]
        public int RoomSlot20;

        [JsonProperty("room_slot_2_1")]
        public int RoomSlot21;

        [JsonProperty("room_slot_2_2")]
        public int RoomSlot22;

        [JsonProperty("room_slot_2_3")]
        public int RoomSlot23;

        [JsonProperty("room_slot_2_4")]
        public int RoomSlot24;

        //floor 4
        [JsonProperty("room_slot_3_0")]
        public int RoomSlot30;

        [JsonProperty("room_slot_3_1")]
        public int RoomSlot31;

        [JsonProperty("room_slot_3_2")]
        public int RoomSlot32;

        [JsonProperty("room_slot_3_3")]
        public int RoomSlot33;

        [JsonProperty("room_slot_3_4")]
        public int RoomSlot34;

        //floor 5
        [JsonProperty("room_slot_4_0")]
        public int RoomSlot40;

        [JsonProperty("room_slot_4_1")]
        public int RoomSlot41;

        [JsonProperty("room_slot_4_2")]
        public int RoomSlot42;

        [JsonProperty("room_slot_4_3")]
        public int RoomSlot43;

        [JsonProperty("room_slot_4_4")]
        public int RoomSlot44;

        //floor 6
        [JsonProperty("room_slot_5_0")]
        public int RoomSlot50;

        [JsonProperty("room_slot_5_1")]
        public int RoomSlot51;

        [JsonProperty("room_slot_5_2")]
        public int RoomSlot52;

        [JsonProperty("room_slot_5_3")]
        public int RoomSlot53;

        [JsonProperty("room_slot_5_4")]
        public int RoomSlot54;

        //floor 7
        [JsonProperty("room_slot_6_0")]
        public int RoomSlot60;

        [JsonProperty("room_slot_6_1")]
        public int RoomSlot61;

        [JsonProperty("room_slot_6_2")]
        public int RoomSlot62;

        [JsonProperty("room_slot_6_3")]
        public int RoomSlot63;

        [JsonProperty("room_slot_6_4")]
        public int RoomSlot64;

        //floor 8
        [JsonProperty("room_slot_7_0")]
        public int RoomSlot70;

        [JsonProperty("room_slot_7_1")]
        public int RoomSlot71;

        [JsonProperty("room_slot_7_2")]
        public int RoomSlot72;

        [JsonProperty("room_slot_7_3")]
        public int RoomSlot73;

        [JsonProperty("room_slot_7_4")]
        public int RoomSlot74;

        //floor 9
        [JsonProperty("room_slot_8_0")]
        public int RoomSlot80;

        [JsonProperty("room_slot_8_1")]
        public int RoomSlot81;

        [JsonProperty("room_slot_8_2")]
        public int RoomSlot82;

        [JsonProperty("room_slot_8_3")]
        public int RoomSlot83;

        [JsonProperty("room_slot_8_4")]
        public int RoomSlot84;

        //floor 10
        [JsonProperty("room_slot_9_0")]
        public int RoomSlot90;

        [JsonProperty("room_slot_9_1")]
        public int RoomSlot91;

        [JsonProperty("room_slot_9_2")]
        public int RoomSlot92;

        [JsonProperty("room_slot_9_3")]
        public int RoomSlot93;

        [JsonProperty("room_slot_9_4")]
        public int RoomSlot94;
#pragma warning restore CS0649
        #endregion

        public bool IsRoomPlaced(int id)
        {
            List<int> list = new List<int>() {
                RoomSlot00, RoomSlot01, RoomSlot02, RoomSlot03, RoomSlot04,
                RoomSlot10, RoomSlot11, RoomSlot12, RoomSlot13, RoomSlot14,
                RoomSlot20, RoomSlot21, RoomSlot22, RoomSlot23, RoomSlot24,
                RoomSlot30, RoomSlot31, RoomSlot32, RoomSlot33, RoomSlot34,
                RoomSlot40, RoomSlot41, RoomSlot42, RoomSlot43, RoomSlot44,
                RoomSlot50, RoomSlot51, RoomSlot52, RoomSlot53, RoomSlot54,
                RoomSlot60, RoomSlot61, RoomSlot62, RoomSlot63, RoomSlot64,
                RoomSlot70, RoomSlot71, RoomSlot72, RoomSlot73, RoomSlot74,
                RoomSlot80, RoomSlot81, RoomSlot82, RoomSlot83, RoomSlot84,
                RoomSlot90, RoomSlot91, RoomSlot92, RoomSlot93, RoomSlot94
            };

            return list.Contains(id);
        }
    }
}
