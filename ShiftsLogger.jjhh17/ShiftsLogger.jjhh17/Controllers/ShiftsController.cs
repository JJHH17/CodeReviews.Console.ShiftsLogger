using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShiftsLogger.jjhh17.Services;

namespace ShiftsLogger.jjhh17
{
    [ApiController]
    [Route("api/shift")]
    //example: http://localhost:5609/api/shift/

    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;
        public ShiftsController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
         public ActionResult<List<Shift>> GetAllShifts(int id)
         {
            return Ok(_shiftService.GetAllShifts(id));
         }

        [HttpGet("{id}")]
        public ActionResult<Shift> GetShiftById(int id)
        {
            return Ok(_shiftService.GetShiftById(id));
        }

        [HttpPost]
        public ActionResult<Shift> CreateShift(Shift shift)
        {
            return Ok(_shiftService.CreateShift(shift));
        }

        [HttpPut("{id}")]
        public ActionResult<Shift> UpdateShift(int id, Shift updatedShift)
        {
            return Ok(_shiftService.UpdateShift(id, updatedShift));
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteShift(int id)
        {
            return ok(_shiftService.DeleteShift(id));
        }
    }
}
