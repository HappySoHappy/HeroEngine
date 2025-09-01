using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request.Hideout
{
    public class CollectHideoutRoomActivityResult : Request
    {
        public int RoomId;
        public CollectHideoutRoomActivityResult(Account account, int roomId) : base(account, "collectHideoutRoomActivityResult")
        {
            RoomId = roomId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["hideout_room_id"] = RoomId;
            data["collect"] = "true";

            return data;
        }

        // hideout_room_id=12998&collect=true&action=collectHideoutRoomActivityResult&user_id=3093&user_session_id=zDJWtaI3a28Yhqth7ppSf8yGohTo22&client_version=html5_231&auth=fc0f9d613add9c4dfa60d9332807a13e&rct=2&keep_active=true&device_type=web
        public static void Update(Account account, dynamic data)
        {
            if (data == null) return;

            var hz = account.HeroZero;
            if (hz == null) return;

            var game = hz.Data;
            if (game == null) return;

            JsonPropertyUpdater.UpdateFields(game, data);

            var updateRoom = JsonConvert.DeserializeObject<HideoutRoom>(JsonConvert.SerializeObject(data.hideout_room));

            foreach (var room in game.HideoutRooms!)
            {
                if (room.Id != updateRoom.Id) continue;

                if (room.IsAttackerProductionRoom())
                {
                    room.MaxResources = 0;
                    room.TimeResourceChanged = updateRoom.TimeResourceChanged;
                    continue;
                }

                room.Resources = updateRoom.Resources;
                room.TimeResourceChanged = updateRoom.TimeResourceChanged;
            }

        }
    }
}
