using SmithereenUWP.Core;
using System;
using System.Text;

namespace SmithereenUWP.Extensions
{
    public static class DateTimeExtensions
    {
        // 13 s, 5 min, 7 h, 12 d, 2 m, 3 y...
        public static string ToDiffStringShort(this DateTime target)
        {
            string text = String.Empty;
            DateTime current = DateTime.Now;

            var diff = current - target;
            var mdiff = ((current.Year - target.Year) * 12) + current.Month - target.Month;
            var md = current.AddMonths(-1);

            if (md.Day < target.Day)
            {
                mdiff--;
            }
            else if (md.Day == target.Day && md.Hour < target.Hour)
            {
                mdiff--;
            }

            if (mdiff >= 12)
            {
                int years = mdiff / 12;
                text = Locale.GetFormatted("years_short", years.ToString());
            }
            else if (mdiff > 0)
            {
                text = Locale.GetFormatted("months_short", mdiff.ToString());
            }
            else if (diff.TotalDays >= 1)
            {
                text = Locale.GetFormatted("days_short", diff.Days.ToString());
            }
            else if (diff.TotalHours >= 1)
            {
                text = Locale.GetFormatted("hours_short", diff.Hours.ToString());
            }
            else if (diff.TotalMinutes >= 1)
            {
                text = Locale.GetFormatted("minutes_short", diff.Minutes.ToString());
            }
            //else if (diff.TotalSeconds >= 10) {
            //    text = $"{diff.Seconds} s";
            //}

            return text;
        }

        public static string ToHumanizedDateString(this DateTime target)
        {
            string text = String.Empty;
            DateTime current = DateTime.Now;

            if (current.Date == target.Date)
            {
                text = Locale.Get("today");
            }
            else if (current.Date.AddDays(-1) == target.Date)
            {
                text = Locale.Get("yesterday");
            }
            else if (current.Date.AddDays(1) == target.Date)
            {
                text = Locale.Get("tomorrow");
            }
            else
            {
                if (current.Year == target.Year)
                {
                    text = target.ToString("m");
                }
                else
                {
                    text = $"{target.ToString("m")} {target.Year}";
                }
            }

            return text;
        }

        public static string ToHumanizedString(this DateTime target, bool noAt = false)
        {
            DateTime current = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            if (current.Date != target.Date) sb.Append($"{target.ToHumanizedDateString()} ");
            string at = noAt && sb.Length == 0 ? "" : $"{Locale.Get("time_at")} ";
            sb.Append(at);
            sb.Append(target.ToString("H:mm"));
            return sb.ToString();
        }

        public static string ToHumanizedTimeOrDateString(this DateTime target)
        {
            DateTime current = DateTime.Now;
            if (current.Date != target.Date)
            {
                return target.ToHumanizedDateString();
            }
            else
            {
                return target.ToString("H:mm");
            }
        }

        public static string ToHumanizedTimeAndDateString(this DateTime target)
        {
            DateTime current = DateTime.Now;
            if (current.Date != target.Date)
            {
                return string.Join(" ", target.ToHumanizedDateString(), Locale.Get("time_at"), target.ToString("H:mm"));
            }
            else
            {
                return string.Join(" ", Locale.Get("time_at"), target.ToString("H:mm"));
            }
        }

        public static string ToRelativeOrAbsoluteTime(this DateTime target)
        {
            DateTime current = DateTime.Now;
            if (current.AddHours(-1) < target)
            {
                return target.ToDiffStringShort();
            }
            else
            {
                return target.ToHumanizedTimeAndDateString();
            }
        }

        public static string ToTimeWithHourIfNeeded(this TimeSpan ts)
        {
            return ts.TotalSeconds >= 3600 ? ts.ToString("c") : ts.ToString(@"m\:ss");
        }

        public static string ToTimeWithHourIfNeeded(this int seconds)
        {
            return seconds >= 3600 ? TimeSpan.FromSeconds(seconds).ToString("c") : TimeSpan.FromSeconds(seconds).ToString(@"m\:ss");
        }


    }
}
