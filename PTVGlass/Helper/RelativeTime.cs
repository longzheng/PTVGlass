using System;

namespace PTVGlass
{
	public class TimeHelper
	{
		static public string GetRelativeTime (DateTime absoluteTime)
		{
			TimeSpan timeDiff = absoluteTime.Subtract (DateTime.UtcNow);
			if (timeDiff.TotalMinutes < 1) {
				return "now";
			} else if (timeDiff.TotalHours < 1) {
				var minutesRounded = Math.Floor (timeDiff.TotalMinutes);
				return String.Format ("{0} min{1}", minutesRounded, minutesRounded > 1 ? "s" : "");
			} else {
				var hoursRounded = Math.Floor (timeDiff.TotalHours);
				return String.Format ("{0} hr{1}", hoursRounded, hoursRounded > 1 ? "s" : "");
			}
		}
	}
}

