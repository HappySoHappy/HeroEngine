using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace HeroEngine.Request
{
    public class SetTutorialFlags : Request
    {
        public string Flag;
        public string Value;
        public SetTutorialFlags(Account account, string flag, string value = "true") : base(account, "setTutorialFlags")
        {
            Flag = flag;
            Value = value;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["flags"] = CalculateFlags(_account, Flag, Value);
            data["flag"] = Flag;

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

            Console.WriteLine("SetTutorialFlags data doesnt update flags, dont forget to set them manually to current value");
        }

        private static string CalculateFlags(Account account, string newFlag, string value = "true")
        {
            var tutorialFlags = JsonConvert.DeserializeObject<Dictionary<string, string>>(Uri.UnescapeDataString(account.HeroZero!.Data.Character.TutorialFlagsJson))!;

            if (!tutorialFlags.ContainsKey(newFlag))
            {
                tutorialFlags.Add(newFlag, value);
            }

            string encoded = HttpUtility.UrlEncode(JsonConvert.SerializeObject(tutorialFlags));
            encoded = Regex.Replace(encoded, @"%[a-f0-9]{2}", m => m.Value.ToUpperInvariant());
            encoded = encoded
                .Replace("%22true%22", "true")
                .Replace("%22false%22", "false");

            return encoded;
        }

        /*
         * '{"itemImprovementsUpdate":true,
         * "mission_shown":true,
         * "first_visit":true,
         * "first_mission":true,
         * "stats_spent":true,
         * "shop_shown":true,
         * "first_item":true,
         * "duel_shown":true,
         * "first_duel":true,
         * "tutorial_finished":true,
         * "tutorial_resource_quest":true,
         * "second_mission":true,
         * "tutorial_tv_quest":true,
         * "training_new":true,
         * "social_media":true,
         * "appearance_unlocked":true,
         * "league":true,
         * "story_dungeon":true,
         * "hideout_first_building":true,
         * "hideout_first_collect":true,
         * "hideout_first_upgrade":true,
         * "hideout_build_stone":true,
         * "hideout…true,
         * "marketing_optin":true,
         * "dungeons":true,
         * "league_division_change":"13_12",
         * "league_division_change_season":0,
         * "tutorial_herobook_shown":true,
         * "hideout_build_glue":true,
         * "hideout_tutorial_show_attacker":true,
         * "hideout_build_attacker":true,
         * "hideout_collect_attacker_units":true,
         * "hideout_first_attack":true,
         * "hideout_tutorial_completed":true,
         * "hideout_build_attacker_units":true,
         * "sewing_machine_unlocked":true,
         * "quests":true,
         * "tutorial_outfits":true,
         * "goals":true,
         * "tutorial_event_treasure":true,
         * "legendary_dungeon":true}'
         */
    }
}
