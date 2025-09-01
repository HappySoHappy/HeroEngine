using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Item
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("identifier")]
        public string Identifier = "";

        [JsonProperty("type")]
        public int Type;

        [JsonProperty("premium_item")]
        public bool Premium;

        [JsonProperty("sell_price")]
        public int SellValue;

        [JsonProperty("stat_stamina")]
        public int Stamina;

        [JsonProperty("stat_strength")]
        public int Strength;

        [JsonProperty("stat_critical_rating")]
        public int CriticalRating;

        [JsonProperty("stat_dodge_rating")]
        public int DodgeRating;

        [JsonProperty("stat_weapon_damage")]
        public int WeaponDamage;

        public string GetTypeName()
        {
            switch (Type)
            {
                case 1:
                    return "Mask";
                case 2:
                    return "Cape";
                case 3:
                    return "Suit";
                case 4:
                    return "Belt";
                case 5:
                    return "Boots";
                case 6:
                    return "Weapon";
                case 7:
                    return "Gadget";
                case 8:
                    return "Throwable";

                case 9:
                    return "Pet";

                case 10:
                    return "Surprise Box";

                case 11:
                    return "Hammer Of Amnesia";

                case 12:
                    return "Improvement Gem"; // improvement_weapon_epic / improvement_sidekick_epic

                case 13:
                    return "Gem Slot Stapler"; // special_add_improvement_slot

                case 14:
                    return "Pet Gem Collar"; // special_add_sidekick_improvement_slot

                default:
                    return $"Unknown slot {Type}";
            }
        }

        public bool IsMaskItem() => Type == 1;
        public bool IsCapeItem() => Type == 2;
        public bool IsSuitItem() => Type == 3;
        public bool IsBeltItem() => Type == 4;
        public bool IsBootsItem() => Type == 5;
        public bool IsWeaponItem() => Type == 6;
        public bool IsGadgetItem() => Type == 7;
        public bool IsThrowableItem() => Type == 8;
    }
}
