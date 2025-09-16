using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftsLogger.jjhh17.Model
{
    public class Shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan ClockIn { get; set; }
        public TimeSpan ClockOut { get; set; }
        public string Department { get; set; }

        [NotMapped]
        public string Duration
        {
            get
            {
                var duration = ClockOut - ClockIn;
                if (duration.TotalMinutes < 0)
                {
                    duration += TimeSpan.FromDays(1);
                }

                return duration.ToString(@"hh\:mm");
            }
        }
    }
}
