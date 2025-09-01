using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class Dungeon
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("identifier")]
        public string Identifier = "";

        [JsonProperty("status")]
        public int Status;

        [JsonProperty("current_dungeon_quest_id")]
        public int CurrentQuestId;

        [JsonProperty("progress_index")]
        public int ProgressIndex;

        [JsonProperty("mode")]
        public int Mode;

        [JsonProperty("ts_last_complete")]
        public long TimeCompleted;

        public bool IsRunning()
        {
            return Status == 3;
        }

        public bool IsCompleted()
        {
            return Status == 4;
        }

        public string GetDungeonName()
        {
            switch (Identifier)
            {
                case "dungeon1":
                    return "Where is Lucy?";

                case "dungeon2":
                    return "What is being covered up?";

                case "dungeon3":
                    return "What is The Plan?";

                case "dungeon4":
                    return "Everything Under Water?";

                case "dungeon5":
                    return "What is Sparkling Over There?";

                case "dungeon6":
                    return "Totally Twisted?";

                case "dungeon7":
                    return "Not of this Earth?";

                case "dungeon8":
                    return "What is Being Sanded?";

                case "dungeon9":
                    return "What is Being Programmed Here?";

                default:
                    return $"Unknown dungeon {Identifier}";
            }
        }

        public int GetRequiredLevel()
        {
            switch(Identifier) {
                case "dungeon1":
                    return 15;

                case "dungeon2":
                    return 30;

                case "dungeon3":
                    return 45;

                case "dungeon4":
                    return 60;

                case "dungeon5":
                    return 75;

                case "dungeon6":
                    return 90;

                case "dungeon7":
                    return 105;

                case "dungeon8":
                    return 120;

                case "dungeon9":
                    return 135;

                default:
                    return 0;
            }
        }
    }
}
