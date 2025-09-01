using HeroEngine.Framework;
using HeroEngine.Model;
using HeroEngine.Persistance;
using HeroEngine.Util;
using Newtonsoft.Json;

namespace HeroEngine.Request.Hideout
{
    public class StartHideoutRoomProduction : Request
    {
        public int RoomId;
        public int Production;
        public StartHideoutRoomProduction(Account account, int roomId, int production) : base(account, "startHideoutRoomProduction")
        {
            RoomId = roomId;
            Production = production;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["hideoutRoomId"] = RoomId;
            data["productionCount"] = Production;

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

            var updateRoom = JsonConvert.DeserializeObject<HideoutRoom>(JsonConvert.SerializeObject(data.hideout_room));

            foreach (var room in game.HideoutRooms!)
            {
                if (room.Id != updateRoom.Id) continue;

                room.Status = updateRoom.Status;
                room.MaxResources = updateRoom.MaxResources;
                room.TimeResourceChanged = updateRoom.TimeResourceChanged;
                room.TimeActivityFinishes = updateRoom.TimeActivityFinishes;
            }
        }
    }
}
