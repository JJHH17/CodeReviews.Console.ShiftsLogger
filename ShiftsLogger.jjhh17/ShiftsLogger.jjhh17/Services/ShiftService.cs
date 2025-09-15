using ShiftsLogger.jjhh17.Data;
using ShiftsLogger.jjhh17.Model;
using Microsoft.EntityFrameworkCore;

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
        private readonly ShiftsDbContext Context;

        public ShiftService(ShiftsDbContext context)
        {
            Context = context;
        }

        public Shift CreateShift(Shift shift)
        {
            var savedShift = Context.Shifts.Add(shift);
            Context.SaveChanges();
            return savedShift.Entity;
        }

        public string? DeleteShift(int id)
        {
            Shift savedShift = Context.Shifts.Find(id);

            if (savedShift == null)
            {
                return null;
            }

            Context.Shifts.Remove(savedShift);

            return $"Shift ID: {id} deleted";
        }

        public List<Shift> GetAllShifts()
        {
            return Context.Shifts.ToList();
        }

        public Shift? GetShiftById(int id)
        {
            Shift savedShift = Context.Shifts.Find(id);
            return savedShift;
        }

        public Shift UpdateShift(int id, Shift shift)
        {
            Shift savedShift = Context.Shifts.Find(id);
            if (savedShift == null)
            {
                return null;
            }

            Context.Entry(savedShift).CurrentValues.SetValues(shift);
            Context.SaveChanges();

            return savedShift;
        }
    }
}
