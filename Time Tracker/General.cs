using System;

namespace Time_Tracker
{
    class General
    {
        const String MASKED_TEXTBOX_MASK = "{0,3:D3}:{1,2:D2}:{2,2:D2}.{3,3:D3}";

        public static TimeSpan TimeSpanFromString(String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = "0:0.0";
            }

            var explode = text.Split(':');

            int hours = 0,
                minutes = 0,
                seconds = 0,
                centiseconds = 0;

            var sec = explode[explode.Length - 1].Split('.');
            if (sec.Length == 1)
            {
                Int32.TryParse(sec[0], out seconds);
            }
            else
            {
                Int32.TryParse(sec[1], out centiseconds);
                Int32.TryParse(sec[0], out seconds);
                if (explode.Length >= 2) { Int32.TryParse(explode[explode.Length - 2], out minutes); }
                if (explode.Length >= 3) { Int32.TryParse(explode[explode.Length - 3], out hours); }
            }

            return new TimeSpan(0, hours, minutes, seconds, centiseconds);
        }

        public static String StringFromTimeSpan(TimeSpan span)
        {
            return String.Format(General.MASKED_TEXTBOX_MASK, span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
        }
    }
}
