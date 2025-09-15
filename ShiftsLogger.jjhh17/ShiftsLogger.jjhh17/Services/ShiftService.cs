using ShiftsLogger.jjhh17.Data;
using ShiftsLogger.jjhh17.Model;

namespace ShiftsLogger.jjhh17.Services
{
    public interface IShiftService
    {
        public List<Shift> GetAllShifts();
        public Shift? GetShiftById(int id);
        public Shift CreateShift(Shift shift);
        public Shift UpdateShift(int id, Shift updatedShift);
        public string? DeleteShift(int id);
    }

    public class ShiftService : IShiftService
    {
        private readonly ShiftsDbContext _dbContext;

        public ShiftService(ShiftsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Shift CreateShift(Shift shift)
        {
            var savedShift = _dbContext.Shifts.Add(shift);
            _dbContext.SaveChanges();
            return savedShift.Entity;
        }
    }
}
