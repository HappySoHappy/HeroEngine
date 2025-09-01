using System.Text;

namespace HeroEngine.Util
{
    public static class UnixTime
    {
        public static long Now()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            return (long)timeSpan.TotalSeconds;
        }

        public static long From(DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - epoch).TotalSeconds;
        }

        public static long Since(long then)
        {
            return Now() - then;
        }

        public static long Until(long then)
        {
            return then - Now();
        }

        public static long SinceMidnight()
        {
            DateTime todayStart = DateTime.Today;
            return Since(((DateTimeOffset)todayStart).ToUnixTimeSeconds());
        }

        public static long UntilMidnight()
        {
            DateTime tomorrowStart = DateTime.Today.AddDays(1);
            return Until(((DateTimeOffset)tomorrowStart).ToUnixTimeSeconds());
        }

        public static string Format(long seconds, bool now = true)
        {
            StringBuilder sb = new StringBuilder();

            int weeks = (int)Math.Floor(seconds / (24f * 3600f * 7f));
            double remaining = seconds % (24f * 3600f * 7f);

            int days = (int)Math.Floor(remaining / (24f * 3600f));
            remaining = remaining % (24f * 3600f);

            int hours = (int)Math.Floor(remaining / 3600f);
            remaining = remaining % 3600f;

            int minutes = (int)Math.Floor(remaining / 60f);
            seconds = (int)Math.Floor(remaining % 60f);

            if (weeks > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(weeks).Append("w");
            }

            if (days > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(days).Append("d");
            }

            if (hours > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(hours).Append("h");
            }

            if (minutes > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(minutes).Append("min");
            }

            if (seconds > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(seconds).Append("s");
            }

            if (sb.Length == 0) sb.Append(now ? "now" : "0s");

            return sb.ToString();
        }

        public static string FormatAsDays(long seconds, bool now = true)
        {
            StringBuilder sb = new StringBuilder();
            double remaining = seconds;

            int days = (int)Math.Floor(remaining / (24f * 3600f));
            remaining = remaining % (24f * 3600f);

            int hours = (int)Math.Floor(remaining / 3600f);
            remaining = remaining % 3600f;

            int minutes = (int)Math.Floor(remaining / 60f);
            seconds = (int)Math.Floor(remaining % 60f);

            if (days > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(days).Append("d");
            }

            if (hours > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(hours).Append("h");
            }

            if (minutes > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(minutes).Append("min");
            }

            if (seconds > 0)
            {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(seconds).Append("s");
            }

            if (sb.Length == 0) sb.Append(now ? "now" : "0s");

            return sb.ToString();
        }
    }
}
