namespace Clinic_System.Data.Helpers
{
    /// <summary>
    /// Helper class to get current time in Egypt timezone (Cairo)
    /// </summary>
    public static class EgyptTimeHelper
    {
        /// <summary>
        /// Get current time in Egypt timezone (Cairo)
        /// </summary>
        public static DateTime GetEgyptTime()
        {
            try
            {
                // Try Windows timezone first
                var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            }
            catch
            {
                try
                {
                    // Try Linux/Mac timezone
                    var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
                }
                catch
                {
                    // Fallback: Egypt is UTC+2 (or UTC+3 in summer, but we'll use UTC+2 as default)
                    return DateTime.UtcNow.AddHours(2);
                }
            }
        }
    }
}

