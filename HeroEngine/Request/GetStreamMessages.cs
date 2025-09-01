using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    public class GetStreamMessages : Request
    {
        public string StreamType;
        public GetStreamMessages(Account account, string streamType) : base(account, "getStreamMessages")
        {
            StreamType = streamType;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["stream_type"] = StreamType;
            data["stream_id"] = _account.HeroZero!.Data.Character.Id;
            data["start_message_id"] = "0";

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
        }
    }
}
