using System;

namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings that represent a date range.
    /// </summary>
    public class DateRangeSettings
    {
        public DateTime StartDate { get; private set; } = DateTime.MinValue.Date;
        public DateTime FinishDate { get; private set; } = DateTime.MinValue.Date;

        private string _start;
        public string Start
        {
            get
            {
                return _start;
            }
            set
            {
                StartDate = DateTime.Parse(value);
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
                FinishDate = DateTime.Parse(value).Date;
                _finish = value;
            }
        }

    }

}
