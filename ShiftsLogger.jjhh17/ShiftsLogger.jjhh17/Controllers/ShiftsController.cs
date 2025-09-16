using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.jjhh17.Model;
using ShiftsLogger.jjhh17.Services;
using ShiftsLogger.jjhh17.Data;

namespace ShiftsLogger.jjhh17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //example: http://localhost:5609/api/shift/

    public class ShiftsController(IShiftService shiftService) : Controller
    {
        private readonly IShiftService _shiftService = shiftService; 

        [HttpGet]
         public ActionResult<List<Shift>> GetAllShifts()
         {
            return Ok(_shiftService.GetAllShifts());
         }

        [HttpGet("{id}")]
        public ActionResult<Shift> GetShiftById(int id)
        {
            var result = _shiftService.GetShiftById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Shift> CreateShift(Shift shift)
        {
            return Ok(_shiftService.CreateShift(shift));
        }

        [HttpPut("{id}")]
        public ActionResult<Shift> UpdateShift(int id, Shift updatedShift)
        {
            var result = _shiftService.UpdateFlight(id, updatedShift);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteShift(int id)
        {
            var result = _shiftService.DeleteShift(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
