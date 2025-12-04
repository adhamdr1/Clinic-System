namespace Clinic_System.Data.Helpers
{
    /// <summary>
    /// Helper class to get current time in Egypt timezone (Cairo)
    /// </summary>
    public static class EgyptTimeHelper
    {
        // الحل: Cache الـ TimeZoneInfo لتحسين الأداء
        private static TimeZoneInfo? _egyptTimeZone;
        private static readonly object _lock = new object();

        private static TimeZoneInfo GetEgyptTimeZone()
        {
            if (_egyptTimeZone != null)
                return _egyptTimeZone;

            lock (_lock)
            {
                if (_egyptTimeZone != null)
                    return _egyptTimeZone;

                try
                {
                    // Try Windows timezone first
                    _egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                    return _egyptTimeZone;
                }
                catch
                {
                    try
                    {
                        // Try Linux/Mac timezone
                        _egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");
                        return _egyptTimeZone;
                    }
                    catch
                    {
                        // Fallback: Return null to use UTC+2 calculation
                        return null!;
                    }
                }
            }
        }

        /// <summary>
        /// Get current time in Egypt timezone (Cairo)
        /// </summary>
        public static DateTime GetEgyptTime()
        {
            var timeZone = GetEgyptTimeZone();
            
            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            }
            
            // Fallback: Egypt is UTC+2 (or UTC+3 in summer, but we'll use UTC+2 as default)
            return DateTime.UtcNow.AddHours(2);
        }
    }
}



