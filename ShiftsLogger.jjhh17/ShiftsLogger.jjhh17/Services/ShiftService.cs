using ShiftsLogger.jjhh17.Data;
using ShiftsLogger.jjhh17.Model;

namespace ShiftsLogger.jjhh17
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
        private readonly ShiftsDbContext context;

        public ShiftService(ShiftsDbContext dbContext)
        {
            context = context;
        }

        public Shift CreateShift(Shift shift)
        {
            var savedShift = _dbContext.Shifts.Add(shift);
            _dbContext.SaveChanges();
            return savedShift.Entity;
        }

        public string? DeleteShift(int id)
        {
            Shift savedShift = _dbContext.Shifts.Find(id);

            if (savedShift == null)
            {
                return null;
            }

            _dbContext.Shifts.Remove(savedShift);
            _dbContext.SaveChanges();

            return $"Shift ID: {id} deleted";
        }

        public List<Shift> GetAllShifts()
        {
            return _dbContext.Shifts.ToList();
        }

        public Shift? GetShiftById(int id)
        {
            Shift savedShift = _dbContext.Shifts.Find(id);
            return savedShift;
        }

        public Shift UpdateShift(int id, Shift shift)
        {
            Shift savedShift = _dbContext.Shifts.Find(id);
            if (savedShift == null)
            {
                return null;
            }

            _dbContext.Entry(savedShift).CurrentValues.SetValues(shift);
            _dbContext.SaveChanges();

            return savedShift;
        }
    }
}
