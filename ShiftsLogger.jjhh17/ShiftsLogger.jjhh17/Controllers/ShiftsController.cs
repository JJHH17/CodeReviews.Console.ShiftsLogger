using System;

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
    }
}
