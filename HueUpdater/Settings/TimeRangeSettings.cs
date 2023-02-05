using System;

namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings that represent a time range.
    /// </summary>
    public class TimeRangeSettings
    {
        public TimeSpan StartTime { get; private set; } = TimeSpan.MinValue;
        public TimeSpan FinishTime { get; private set; } = TimeSpan.MinValue;

        private string _start;
        public string Start
        {
            get
            {
                return _start;
            }
            set
            {
                StartTime = TimeSpan.Parse(value);
                _start = value;
            }
        }

        private string _finish;
        public string Finish
        {
            get
            {
                return _finish;
            }
            set
            {
                FinishTime = TimeSpan.Parse(value);
                _finish = value;
            }
        }

    }

}
