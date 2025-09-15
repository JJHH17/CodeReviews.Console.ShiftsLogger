namespace ShiftsLogger.jjhh17.Model
{
    public class Shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ClockIn { get; set; }
        public DateTime ClockOut { get; set; }
        public string Department { get; set; }
        public int Duration { get; set; }
    }
}
