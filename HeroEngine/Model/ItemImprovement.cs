using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class ItemImprovement
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("item_id")]
        public int ItemId;

        [JsonProperty("sidekick_id")]
        public int PetId;

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("identifier")]
        public string Identifier = "";

        [JsonProperty("type")]
        public int Type;

        [JsonProperty("quality")]
        public int Quality;

        [JsonProperty("stat_stamina")]
        public int Stamina;

        [JsonProperty("stat_strength")]
        public int Strength;

        [JsonProperty("stat_critical_rating")]
        public int CriticalRating;

        [JsonProperty("stat_dodge_rating")]
        public int DodgeRating;

        public bool IsMaskImprovement()
        {
            return Type == 1;
        }

        public bool IsCapeImprovement()
        {
            return Type == 2;
        }

        public bool IsSuitImprovement()
        {
            return Type == 3;
        }

        public bool IsBeltImprovement()
        {
            return Type == 4;
        }

        public bool IsBootsImprovement()
        {
            return Type == 5;
        }

        public bool IsWeaponImprovement()
        {
            return Type == 6;
        }

        public bool IsGadgetImprovement()
        {
            return Type == 7;
        }

        public bool IsPetGem()
        {
            return Type == 9;
        }
    }


    /*
     * {
"item_improvements": [
    {
        "id": 16884,
        "character_id": 3067,
        "item_id": 3855311,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_belt_rare",
        "type": 4,
        "quality": 2,
        "item_level": 116,
        "stat_stamina": 0,
        "stat_strength": 22,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 70
    },
    {
        "id": 16885,
        "character_id": 3067,
        "item_id": 3854295,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_boots_rare",
        "type": 5,
        "quality": 2,
        "item_level": 120,
        "stat_stamina": 21,
        "stat_strength": 0,
        "stat_critical_rating": 39,
        "stat_dodge_rating": 26
    },
    {
        "id": 17882,
        "character_id": 3067,
        "item_id": 3967734,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_gadget_rare",
        "type": 7,
        "quality": 2,
        "item_level": 110,
        "stat_stamina": 0,
        "stat_strength": 44,
        "stat_critical_rating": 42,
        "stat_dodge_rating": 0
    },
    {
        "id": 18623,
        "character_id": 3067,
        "item_id": 4035076,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_suit_common",
        "type": 3,
        "quality": 1,
        "item_level": 129,
        "stat_stamina": 27,
        "stat_strength": 0,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 50
    },
    {
        "id": 28994,
        "character_id": 3067,
        "item_id": 5538172,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_suit_epic",
        "type": 3,
        "quality": 3,
        "item_level": 168,
        "stat_stamina": 137,
        "stat_strength": 67,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 0
    },
    {
        "id": 33058,
        "character_id": 3067,
        "item_id": 5299782,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_mask_epic",
        "type": 1,
        "quality": 3,
        "item_level": 168,
        "stat_stamina": 23,
        "stat_strength": 94,
        "stat_critical_rating": 87,
        "stat_dodge_rating": 0
    },
    {
        "id": 37661,
        "character_id": 3067,
        "item_id": 5862754,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_gadget_common",
        "type": 7,
        "quality": 1,
        "item_level": 184,
        "stat_stamina": 88,
        "stat_strength": 0,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 62
    },
    {
        "id": 39652,
        "character_id": 3067,
        "item_id": 5538183,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_belt_rare",
        "type": 4,
        "quality": 2,
        "item_level": 190,
        "stat_stamina": 0,
        "stat_strength": 0,
        "stat_critical_rating": 67,
        "stat_dodge_rating": 110
    },
    {
        "id": 40029,
        "character_id": 3067,
        "item_id": 5538141,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_boots_rare",
        "type": 5,
        "quality": 2,
        "item_level": 186,
        "stat_stamina": 104,
        "stat_strength": 0,
        "stat_critical_rating": 28,
        "stat_dodge_rating": 51
    },
    {
        "id": 40030,
        "character_id": 3067,
        "item_id": 6335066,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_weapon_epic",
        "type": 6,
        "quality": 3,
        "item_level": 168,
        "stat_stamina": 55,
        "stat_strength": 23,
        "stat_critical_rating": 62,
        "stat_dodge_rating": 64
    },
    {
        "id": 40031,
        "character_id": 3067,
        "item_id": 4035043,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_cape_epic",
        "type": 2,
        "quality": 3,
        "item_level": 168,
        "stat_stamina": 0,
        "stat_strength": 123,
        "stat_critical_rating": 2,
        "stat_dodge_rating": 79
    },
    {
        "id": 40803,
        "character_id": 3067,
        "item_id": 7352258,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_gadget_epic",
        "type": 7,
        "quality": 3,
        "item_level": 201,
        "stat_stamina": 73,
        "stat_strength": 0,
        "stat_critical_rating": 82,
        "stat_dodge_rating": 72
    },
    {
        "id": 41009,
        "character_id": 3067,
        "item_id": 7381327,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_weapon_epic",
        "type": 6,
        "quality": 3,
        "item_level": 169,
        "stat_stamina": 75,
        "stat_strength": 16,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 101
    },
    {
        "id": 42087,
        "character_id": 3067,
        "item_id": 7483998,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_belt_rare",
        "type": 4,
        "quality": 2,
        "item_level": 219,
        "stat_stamina": 0,
        "stat_strength": 0,
        "stat_critical_rating": 217,
        "stat_dodge_rating": 26
    },
    {
        "id": 42668,
        "character_id": 3067,
        "item_id": 7484302,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_mask_common",
        "type": 1,
        "quality": 1,
        "item_level": 225,
        "stat_stamina": 0,
        "stat_strength": 123,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 101
    },
    {
        "id": 43084,
        "character_id": 3067,
        "item_id": 7483999,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_boots_rare",
        "type": 5,
        "quality": 2,
        "item_level": 229,
        "stat_stamina": 36,
        "stat_strength": 0,
        "stat_critical_rating": 105,
        "stat_dodge_rating": 113
    },
    {
        "id": 43085,
        "character_id": 3067,
        "item_id": 0,
        "sidekick_id": 54334,
        "status": 1,
        "identifier": "improvement_sidekick_epic",
        "type": 9,
        "quality": 3,
        "item_level": 175,
        "stat_stamina": 118,
        "stat_strength": 40,
        "stat_critical_rating": 53,
        "stat_dodge_rating": 0
    },
    {
        "id": 43877,
        "character_id": 3067,
        "item_id": 7484102,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_cape_epic",
        "type": 2,
        "quality": 3,
        "item_level": 241,
        "stat_stamina": 81,
        "stat_strength": 55,
        "stat_critical_rating": 147,
        "stat_dodge_rating": 46
    },
    {
        "id": 44497,
        "character_id": 3067,
        "item_id": 7484483,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_suit_rare",
        "type": 3,
        "quality": 2,
        "item_level": 247,
        "stat_stamina": 0,
        "stat_strength": 239,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 55
    },
    {
        "id": 45261,
        "character_id": 3067,
        "item_id": 8246692,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_gadget_common",
        "type": 7,
        "quality": 1,
        "item_level": 248,
        "stat_stamina": 0,
        "stat_strength": 93,
        "stat_critical_rating": 161,
        "stat_dodge_rating": 0
    },
    {
        "id": 46029,
        "character_id": 3067,
        "item_id": 8149000,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_belt_rare",
        "type": 4,
        "quality": 2,
        "item_level": 250,
        "stat_stamina": 0,
        "stat_strength": 0,
        "stat_critical_rating": 125,
        "stat_dodge_rating": 199
    },
    {
        "id": 46850,
        "character_id": 3067,
        "item_id": 8377568,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_cape_rare",
        "type": 2,
        "quality": 2,
        "item_level": 251,
        "stat_stamina": 0,
        "stat_strength": 68,
        "stat_critical_rating": 173,
        "stat_dodge_rating": 98
    },
    {
        "id": 48999,
        "character_id": 3067,
        "item_id": 0,
        "sidekick_id": 56407,
        "status": 1,
        "identifier": "improvement_sidekick_epic",
        "type": 9,
        "quality": 3,
        "item_level": 254,
        "stat_stamina": 0,
        "stat_strength": 28,
        "stat_critical_rating": 203,
        "stat_dodge_rating": 216
    },
    {
        "id": 51792,
        "character_id": 3067,
        "item_id": 9360164,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_mask_epic",
        "type": 1,
        "quality": 3,
        "item_level": 260,
        "stat_stamina": 47,
        "stat_strength": 252,
        "stat_critical_rating": 173,
        "stat_dodge_rating": 0
    },
    {
        "id": 51793,
        "character_id": 3067,
        "item_id": 9360173,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_cape_epic",
        "type": 2,
        "quality": 3,
        "item_level": 260,
        "stat_stamina": 0,
        "stat_strength": 0,
        "stat_critical_rating": 320,
        "stat_dodge_rating": 152
    },
    {
        "id": 51796,
        "character_id": 3067,
        "item_id": 9360279,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_gadget_epic",
        "type": 7,
        "quality": 3,
        "item_level": 260,
        "stat_stamina": 200,
        "stat_strength": 90,
        "stat_critical_rating": 77,
        "stat_dodge_rating": 105
    },
    {
        "id": 51797,
        "character_id": 3067,
        "item_id": 9360285,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_suit_epic",
        "type": 3,
        "quality": 3,
        "item_level": 260,
        "stat_stamina": 82,
        "stat_strength": 38,
        "stat_critical_rating": 277,
        "stat_dodge_rating": 75
    },
    {
        "id": 51799,
        "character_id": 3067,
        "item_id": 8147559,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_boots_epic",
        "type": 5,
        "quality": 3,
        "item_level": 260,
        "stat_stamina": 72,
        "stat_strength": 87,
        "stat_critical_rating": 131,
        "stat_dodge_rating": 182
    },
    {
        "id": 63131,
        "character_id": 3067,
        "item_id": 9360306,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_belt_epic",
        "type": 4,
        "quality": 3,
        "item_level": 274,
        "stat_stamina": 154,
        "stat_strength": 162,
        "stat_critical_rating": 185,
        "stat_dodge_rating": 0
    },
    {
        "id": 63673,
        "character_id": 3067,
        "item_id": 11090606,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_boots_rare",
        "type": 5,
        "quality": 2,
        "item_level": 269,
        "stat_stamina": 243,
        "stat_strength": 0,
        "stat_critical_rating": 7,
        "stat_dodge_rating": 172
    },
    {
        "id": 79411,
        "character_id": 3067,
        "item_id": 9360310,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_weapon_epic",
        "type": 6,
        "quality": 3,
        "item_level": 298,
        "stat_stamina": 307,
        "stat_strength": 0,
        "stat_critical_rating": 181,
        "stat_dodge_rating": 79
    },
    {
        "id": 79797,
        "character_id": 3067,
        "item_id": 14227491,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_cape_epic",
        "type": 2,
        "quality": 3,
        "item_level": 300,
        "stat_stamina": 259,
        "stat_strength": 153,
        "stat_critical_rating": 67,
        "stat_dodge_rating": 106
    },
    {
        "id": 79798,
        "character_id": 3067,
        "item_id": 14227475,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_boots_epic",
        "type": 5,
        "quality": 3,
        "item_level": 300,
        "stat_stamina": 136,
        "stat_strength": 258,
        "stat_critical_rating": 97,
        "stat_dodge_rating": 94
    },
    {
        "id": 79799,
        "character_id": 3067,
        "item_id": 14227715,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_suit_epic",
        "type": 3,
        "quality": 3,
        "item_level": 300,
        "stat_stamina": 63,
        "stat_strength": 239,
        "stat_critical_rating": 39,
        "stat_dodge_rating": 244
    },
    {
        "id": 79800,
        "character_id": 3067,
        "item_id": 14227752,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_weapon_epic",
        "type": 6,
        "quality": 3,
        "item_level": 300,
        "stat_stamina": 321,
        "stat_strength": 64,
        "stat_critical_rating": 135,
        "stat_dodge_rating": 65
    },
    {
        "id": 79803,
        "character_id": 3067,
        "item_id": 14227710,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_gadget_epic",
        "type": 7,
        "quality": 3,
        "item_level": 300,
        "stat_stamina": 234,
        "stat_strength": 351,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 0
    },
    {
        "id": 79804,
        "character_id": 3067,
        "item_id": 14227729,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_belt_epic",
        "type": 4,
        "quality": 3,
        "item_level": 300,
        "stat_stamina": 0,
        "stat_strength": 100,
        "stat_critical_rating": 225,
        "stat_dodge_rating": 260
    },
    {
        "id": 79805,
        "character_id": 3067,
        "item_id": 0,
        "sidekick_id": 57986,
        "status": 1,
        "identifier": "improvement_sidekick_epic",
        "type": 9,
        "quality": 3,
        "item_level": 300,
        "stat_stamina": 302,
        "stat_strength": 0,
        "stat_critical_rating": 0,
        "stat_dodge_rating": 283
    },
    {
        "id": 92676,
        "character_id": 3067,
        "item_id": 14227476,
        "sidekick_id": 0,
        "status": 1,
        "identifier": "improvement_mask_rare",
        "type": 1,
        "quality": 2,
        "item_level": 301,
        "stat_stamina": 98,
        "stat_strength": 336,
        "stat_critical_rating": 72,
        "stat_dodge_rating": 0
    }
]
}
     */

}
