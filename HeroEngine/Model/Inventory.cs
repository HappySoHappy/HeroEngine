using HeroEngine.Framework;
using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Inventory
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("mask_item_id")]
        public int Mask;

        [JsonProperty("cape_item_id")]
        public int Cape;

        [JsonProperty("suit_item_id")]
        public int Suit;

        [JsonProperty("belt_item_id")]
        public int Belt;

        [JsonProperty("boots_item_id")]
        public int Boots;

        [JsonProperty("weapon_item_id")]
        public int Weapon;

        [JsonProperty("gadget_item_id")]
        public int Gadget;

        [JsonProperty("missiles_item_id")]
        public int Throwable;

        [JsonProperty("sidekick_id")]
        public int Sidekick;




        [JsonProperty("bag_item1_id")]
        public int InventoryItem1;

        [JsonProperty("bag_item2_id")]
        public int InventoryItem2;

        [JsonProperty("bag_item3_id")]
        public int InventoryItem3;

        [JsonProperty("bag_item4_id")]
        public int InventoryItem4;

        [JsonProperty("bag_item5_id")]
        public int InventoryItem5;

        [JsonProperty("bag_item6_id")]
        public int InventoryItem6;

        [JsonProperty("bag_item7_id")]
        public int InventoryItem7;

        [JsonProperty("bag_item8_id")]
        public int InventoryItem8;

        [JsonProperty("bag_item9_id")]
        public int InventoryItem9;

        [JsonProperty("bag_item10_id")]
        public int InventoryItem10;

        [JsonProperty("bag_item11_id")]
        public int InventoryItem11;

        [JsonProperty("bag_item12_id")]
        public int InventoryItem12;

        [JsonProperty("bag_item13_id")]
        public int InventoryItem13;

        [JsonProperty("bag_item14_id")]
        public int InventoryItem14;

        [JsonProperty("bag_item15_id")]
        public int InventoryItem15;

        [JsonProperty("bag_item16_id")]
        public int InventoryItem16;

        [JsonProperty("bag_item17_id")]
        public int InventoryItem17;

        [JsonProperty("bag_item18_id")]
        public int InventoryItem18;

        public List<int> GetInventoryItemIds(int level)
        {
            List<int> slots = new List<int>() { InventoryItem1, InventoryItem2, InventoryItem3, InventoryItem4, InventoryItem5, InventoryItem6 };

            if (level >= HeroZero.Constants.InventoryPage2Level)
                slots.AddRange([InventoryItem7, InventoryItem8, InventoryItem9, InventoryItem10, InventoryItem11, InventoryItem12]);

            if (level >= HeroZero.Constants.InventoryPage3Level)
                slots.AddRange([InventoryItem13, InventoryItem14, InventoryItem15, InventoryItem16, InventoryItem17, InventoryItem18]);

            return slots;
        }

        public bool HasEmptyInventorySlot(int level)
        {
            return GetInventoryItemIds(level).Contains(0);
        }

        public bool Contains(int level, int itemId)
        {
            return GetInventoryItemIds(level).Contains(itemId);
        }

        public int FindEmptyInventorySlotIndex(int level)
        {
            return GetInventoryItemIds(level).IndexOf(0);
        }
    }
}
