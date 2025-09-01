using Newtonsoft.Json;

namespace HeroEngine.Model
{
    public class LeagueOpponent
    {
        [JsonProperty("opponent")]
        public Opponent Opponent;
    }
}
