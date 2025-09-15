using ShiftsLogger.jjhh17.Model;

namespace ShiftsLogger.jjhh17.Services
{
    public interface ShiftService
    {
        public List<Shift> GetAllShifts();
        public Shift? GetShiftById(int id);
        public Shift CreateShift(Shift shift);
        public Shift UpdateShift(int id, Shift updatedShift);
        public string? DeleteShift(int id);
    }
}
