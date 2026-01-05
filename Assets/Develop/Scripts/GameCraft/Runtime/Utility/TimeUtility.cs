using System;

namespace OfflineFantasy.GameCraft.Utility
{
    public static class TimeUtility
    {
        public const string DateTimeFormat_yyyyMMddHHmmss = "yyyy-MM-dd  HH:mm:ss";
        public const string DateTimeFormat_HHmmss = "HH:mm:ss";
        public const string TimeSpanFormat_mmss = @"mm\:ss";
        public const string TimeSpanFormat_mm = @"mm";

        public enum TimestampType
        {
            /// <summary>
            /// JavaScript时间戳, 精确到毫秒
            /// </summary>
            JavaScript,
            /// <summary>
            /// Unix时间戳, 精确到秒
            /// </summary>
            Unix
        }

        public static readonly DateTime m_StartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetCurrentTimestamp(TimestampType _timestampType = TimestampType.JavaScript)
        {
            return _timestampType == TimestampType.JavaScript ?
                   (long)(DateTime.UtcNow - m_StartTime).TotalMilliseconds :
                   (long)(DateTime.UtcNow - m_StartTime).TotalSeconds;
        }

        /// <summary>
        /// 将DateTime转换为时间戳
        /// </summary>
        /// <param name="_dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToTimestamp(DateTime _dateTime, TimestampType _timestampType = TimestampType.JavaScript)
        {
            return _timestampType == TimestampType.JavaScript ?
                   (long)(_dateTime - m_StartTime).TotalMilliseconds :
                   (long)(_dateTime - m_StartTime).TotalSeconds;
        }

        /// <summary>
        /// 将时间戳转换为DateTime
        /// </summary>
        /// <param name="_timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(long _timestamp, TimestampType _timestampType = TimestampType.JavaScript)
        {
            return _timestampType == TimestampType.JavaScript ?
                   m_StartTime.AddMilliseconds(_timestamp) :
                   m_StartTime.AddSeconds(_timestamp);
        }
    }
}