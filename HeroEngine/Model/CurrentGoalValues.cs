using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class CurrentGoalValues
    {
        /*[JsonProperty("quests_completed")]
        public ValueCurrentValuePair QuestsCompleted;

        [JsonProperty("duels_completed")]
        public ValueCurrentValuePair DuelsCompleted;

        [JsonProperty("trainings_absolved")]
        public ValueCurrentValuePair TrainingsCompleted;

        [JsonProperty("duels_won")]
        public ValueCurrentValuePair DuelsWon;

        [JsonProperty("duels_lost")]
        public ValueCurrentValuePair DuelsLost;*/


        [JsonProperty("duels_started_a_day")]
        public GoalValue DuelsStarted;

        [JsonProperty("league_fights_started_a_day")]
        public GoalValue LeagueStarted;

        [JsonProperty("dungeon1_unlocked")]
        public GoalValue Dungeon1Unlocked;

        [JsonProperty("dungeon2_unlocked")]
        public GoalValue Dungeon2Unlocked;

        [JsonProperty("dungeon3_unlocked")]
        public GoalValue Dungeon3Unlocked;

        [JsonProperty("dungeon4_unlocked")]
        public GoalValue Dungeon4Unlocked;

        [JsonProperty("dungeon5_unlocked")]
        public GoalValue Dungeon5Unlocked;

        [JsonProperty("dungeon6_unlocked")]
        public GoalValue Dungeon6Unlocked;

        [JsonProperty("dungeon7_unlocked")]
        public GoalValue Dungeon7Unlocked;

        [JsonProperty("dungeon8_unlocked")]
        public GoalValue Dungeon8Unlocked;

        [JsonProperty("dungeon9_unlocked")]
        public GoalValue Dungeon9Unlocked;

        [JsonProperty("dungeon10_unlocked")]
        public GoalValue Dungeon10Unlocked;
    }

    public class GoalValue
    {
        [JsonProperty("value")]
        public int HighestValue;

        [JsonProperty("current_value")]
        public int Value;
    }
}
