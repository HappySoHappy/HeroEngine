using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request
{
    public class SyncGame : Request
    {
        public bool Force;
        public SyncGame(Account account, bool force = true) : base(account, "syncGame")
        {
            Force = force;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["force_sync"] = Force.ToString().ToLower();

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

            game.StreamInfo = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, StreamInfo>>>(JsonConvert.SerializeObject(data.streams_info));
        }
    }
}
