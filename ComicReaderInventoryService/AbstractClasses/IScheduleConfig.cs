using System;
using System.Collections.Generic;

namespace ComicReaderInventoryService.AbstractClasses
{
    public interface IScheduleConfig<T>
    {
        string Name { get; set; }
        string Database { get; set; }
        string Folder { get; set; }
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string Name { get; set; }
        public string Database { get; set; }
        public string Folder { get; set; }
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
