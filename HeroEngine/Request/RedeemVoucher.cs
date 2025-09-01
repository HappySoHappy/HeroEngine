using HeroEngine.Framework;
using HeroEngine.Persistance;
using HeroEngine.Util;

namespace HeroEngine.Request
{
    public class RedeemVoucher : Request
    {
        public string VoucherCode;
        public RedeemVoucher(Account account, string voucherCode) : base(account, "redeemVoucher")
        {
            VoucherCode = voucherCode;
        }

        public override RequestData Create()
        {
            RequestData data = new RequestData();

            data["code"] = VoucherCode;

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
