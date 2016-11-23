using System;

namespace SCP
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}