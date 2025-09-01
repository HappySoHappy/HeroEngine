using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class HideoutRoom
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("level")]
        public int Level;

#pragma warning disable CS8618
        [JsonProperty("identifier")]
        public string Identifier;
#pragma warning restore CS8618

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("current_resource_amount")]
        public int Resources;

        [JsonProperty("max_resource_amount")]
        public int MaxResources;

        [JsonProperty("ts_last_resource_change")]
        public long TimeResourceChanged;

        [JsonProperty("ts_activity_end")]
        public long TimeActivityFinishes;

        public bool IsAttackerProductionRoom()
        {
            return Identifier == "attacker_production";
        }

        public bool IsAutomaticProductionRoom()
        {
            return Identifier == "main_building" || Identifier == "glue_production" || Identifier == "stone_production" || Identifier == "xp_production";
        }

        public string GetProducedResourceName()
        {
            switch (Identifier)
            {
                case "attacker_production":
                    return "Attack bot";
                //case "blacksmith":
                //    return "Hero Forge";
                case "gem_production":
                    return "Modification";
                case "glue_production":
                    return "Glue";
                case "main_building":
                    return "Gold";
                case "stone_production":
                    return "Zeronite";
                case "xp_production":
                    return "Experience";
                default:
                    return Identifier;
            }
        }

        public string GetRoomName()
        {
            switch (Identifier)
            {
                case "attacker_production":
                    return "Attack Bot Factory";
                case "barracks":
                    return "Attack Bot Improvement Unit";
                case "blacksmith":
                    return "Forge";
                case "brain":
                    return "Room 42.0";
                case "broker":
                    return "Brokerage Center";
                case "defender_production":
                    return "Defense Bot Factory";
                case "exchange_room":
                    return "Exchange Room";
                case "gem_production":
                    return "Workshop";
                case "generator":
                    return "Generator Room";
                case "glue_production":
                    return "Glue Factory";
                case "gym":
                    return "Gym";
                case "kybernetic_improvement":
                    return "Cybernetic Research";
                case "main_building":
                    return "Hideout Base";
                case "personal_arena":
                    return "Fight Club";
                case "robot_storage":
                    return "Warehouse";
                case "scout_center":
                    return "Scout Center";
                case "sports_bar":
                    return "Sports Bar";
                case "stone_production":
                    return "Zeronite Mine";
                case "trophy_room":
                    return "Trophy Room";
                case "wall":
                    return "Defense post";
                case "worker_home":
                    return "Work Robot Improvement Unit";
                case "xp_production":
                    return "Experience Labolatory";
                default:
                    return Identifier;
            }
        }

        public bool IsRoomUpgrading()
        {
            return Status == 3 && UnixTime.Until(TimeActivityFinishes) > 0;
        }

        public bool IsRoomIdle()
        {
            return Status == 6;
        }
    }
}
