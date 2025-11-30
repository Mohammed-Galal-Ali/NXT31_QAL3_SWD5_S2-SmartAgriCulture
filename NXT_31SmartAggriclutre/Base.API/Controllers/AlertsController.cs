using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Base.Shared.Responses.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Base.API.Controllers
{
    [Authorize]
    [Route("api/alerts")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly IEquipmentService _euipmentService;
        private readonly IZoneService _zoneService;
        private readonly IReadingTypeService _readingTypeService;
        private readonly ICropService _cropService;

        private readonly IHttpContextAccessor _http;

        public AlertsController(IAlertService alertService, IHttpContextAccessor http, IEquipmentService equipmentService, 
            IZoneService zoneService, IReadingTypeService readingTypeService, ICropService cropService)
        {
            _alertService = alertService;
            _euipmentService = equipmentService;
            _http = http;
            _zoneService = zoneService;
            _readingTypeService = readingTypeService;
            _cropService = cropService;
        }

        private string UserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/alerts/list?zoneId=xxx&fromDate=2025-01-01&toDate=2025-01-31&page=1&pageSize=50
        [HttpGet("list")]
        public async Task<ActionResult<AlertListDto>> GetAllByZone(string zoneId, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
            => Ok(await _alertService.GetAllByZoneAsync(zoneId, UserId, fromDate, toDate, page, pageSize));

        // GET: api/alerts/get?id=xxx
        [HttpGet("get")]
        public async Task<ActionResult<AlertDto>> GetById(string id)
        {
            return await _alertService.GetByIdAsync(id, UserId) is AlertDto a ? Ok(a) : NotFound();
        }

        // POST: api/alerts/create (manual، بس عادة تلقائي)
        [HttpPost("create")]
        public async Task<ActionResult<AlertDto>> Create(CreateAlertRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            var EquipmentId =await _euipmentService.GetByIdAsync(req.EquipmentId, UserId);
            if (EquipmentId == null) throw new NotFoundException("Invalid EquipmentId");
            
            var ZoneId = await _zoneService.GetByIdAsync(req.ZoneId, UserId);
            if (ZoneId == null) throw new NotFoundException("Invalid ZoneId");
            
            var ReadingTypeId = await _readingTypeService.GetByIdAsync(req.ReadingTypeId);
            if (ReadingTypeId == null) throw new NotFoundException("Invalid ReadingTypeId");
            
            var CropId = await _cropService.GetByIdAsync(req.CropId);
            if (CropId == null) throw new NotFoundException("Invalid CropId");
            
            var StageId = await _cropService.GetStageByIdAsync(req.CropGrowthStageId);
            if (StageId == null) throw new NotFoundException("Invalid StageId");

            return await _alertService.CreateAsync(req, UserId) is AlertDto a ? Ok(a) : BadRequest();
        }

        // PATCH: api/alerts/resolve?id=xxx
        [HttpPatch("resolve")]
        public async Task<IActionResult> MarkAsResolved(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            return await _alertService.MarkAsResolvedAsync(id, UserId) ? Ok() : NotFound();
        }
    }
}
