using System;
using HueUpdater.Settings;

namespace HueUpdater.Models
{

    /// <summary>
    /// Query to determine whether a time happens within schedule.
    /// </summary>
    public class ScheduleQuery
    {

        /// <summary>
        /// The time to be checked against the schedule.
        /// </summary>
        public TimeSpan Time { get; set; }


        /// <summary>
        /// The schedule to check against.
        /// </summary>
        public ScheduleSettings Schedule { get; set; }

    }

}
