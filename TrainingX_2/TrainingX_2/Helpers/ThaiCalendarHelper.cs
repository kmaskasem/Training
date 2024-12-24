using System.Globalization;

namespace TrainingX_2.Helpers
{
    public static class ThaiCalendarHelper
    {
        public static DateTime ToThaiYear(this DateTime dateTime)
        {
            // Calculate the Thai Buddhist year (543 years ahead)
            int thaiYear = dateTime.Year + 543;

            // Create a new DateTime object with the Thai Buddhist year
            DateTime thaiDateTime = new DateTime(thaiYear, dateTime.Month, dateTime.Day,
                                                 dateTime.Hour, dateTime.Minute, dateTime.Second,
                                                 dateTime.Millisecond, dateTime.Kind);

            return thaiDateTime;
        }
    }
}
