using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Base.API.Controllers
{
    [Authorize]
    [Route("api/sensor-readings")]
    [ApiController]
    public class SensorReadingsController : ControllerBase
    {
        private readonly ISensorReadingService _sensorReadingService;
        private readonly IHttpContextAccessor _http;

        public SensorReadingsController(ISensorReadingService sensorReadingService, IHttpContextAccessor http)
        {
            _sensorReadingService = sensorReadingService;
            _http = http;
        }

        private string UserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/sensor-readings/list?equipmentId=xxx&fromDate=2025-01-01&toDate=2025-01-31&page=1&pageSize=50
        [HttpGet("list")]
        public async Task<ActionResult<SensorReadingListDto>> GetAllByEquipment(string equipmentId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
            => Ok(await _sensorReadingService.GetAllByEquipmentAsync(equipmentId, UserId, fromDate, toDate, page, pageSize));

        // GET: api/sensor-readings/get?id=xxx
        [HttpGet("get")]
        public async Task<ActionResult<SensorReadingDto>> GetById(string id)
            => await _sensorReadingService.GetByIdAsync(id, UserId) is SensorReadingDto sr ? Ok(sr) : NotFound();

        // POST: api/sensor-readings/create (من IoT device أو manual)
        [HttpPost("create")]
        public async Task<ActionResult<SensorReadingDto>> Create(CreateSensorReadingRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            return await _sensorReadingService.CreateAsync(req, UserId) is SensorReadingDto sr ? Ok(sr) : BadRequest();
        }
    }

}
