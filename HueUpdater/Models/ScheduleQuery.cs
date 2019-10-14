using System;

namespace HueUpdater.Models
{

    /// <summary>
    /// Model to query whether a timespan happens within a schedule.
    /// </summary>
    public class ScheduleQuery
    {

        /// <summary>
        /// The name of the schedule to query.
        /// </summary>
        public string ScheduleName { get; set; }


        /// <summary>
        /// The timespan to be checked against the schedule.
        /// </summary>
        public TimeSpan Time { get; set; }

    }

}
