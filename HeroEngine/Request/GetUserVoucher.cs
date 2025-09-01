using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    public class GetUserVoucher : Request
    {
        public int VoucherId;
        public GetUserVoucher(Account account, int voucherId) : base(account, "getUserVoucher")
        {
            VoucherId = voucherId;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["id"] = VoucherId;

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
//{"data":{"voucher":{"id":11511096,"code":"UX_2025402795_training12","rewards":"{\"training_sessions\":4}","ts_end":1739559614},"server_time":1739214014},"error":""}

