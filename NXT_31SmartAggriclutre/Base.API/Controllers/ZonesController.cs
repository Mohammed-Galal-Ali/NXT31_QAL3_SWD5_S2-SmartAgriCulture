using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Base.API.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly IZoneService _zoneService;
        private readonly IHttpContextAccessor _httpContext;

        public ZonesController(IZoneService zoneService, IHttpContextAccessor httpContext)
        {
            _zoneService = zoneService;
            _httpContext = httpContext;
        }

        private string CurrentUserId =>
            _httpContext.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/farms/{farmId}/zones
        [HttpGet("farm-zones")]
        public async Task<ActionResult<ZoneListDto>> GetByFarm(
            string farmId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _zoneService.GetAllByFarmAsync(farmId, CurrentUserId, page, pageSize);
            return Ok(result);
        }

        // GET: api/zones (للـ Admin أو لعرض كل المناطق)
        [HttpGet("list")]
        public async Task<ActionResult<ZoneListDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _zoneService.GetAllAsync(CurrentUserId, search, page, pageSize);
            return Ok(result);
        }

        // GET: api/zones/{id}
        [HttpGet("get-zone")]
        public async Task<ActionResult<ZoneDto>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException((nameof(id)));
            var zone = await _zoneService.GetByIdAsync(id, CurrentUserId);
            if (zone == null) return NotFound();
            return Ok(zone);
        }

        // POST: api/zones
        [HttpPost("create")]
        public async Task<ActionResult<ZoneDto>> Create(CreateZoneRequest request)
        {
            if (request == null) throw new ArgumentNullException((nameof(request)));
            var zone = await _zoneService.CreateAsync(request, CurrentUserId);
            return Ok(zone);
        }

        // PUT: api/zones/{id}
        [HttpPut("update")]
        public async Task<ActionResult<ZoneDto>> Update(string id, UpdateZoneRequest request)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException((nameof(id)));
            if (request == null) throw new ArgumentNullException((nameof(request)));

            var zone = await _zoneService.UpdateAsync(id, request, CurrentUserId);
            if (zone == null) return NotFound();
            return Ok(zone);
        }

        // DELETE: api/zones/{id}
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException((nameof(id)));

            var success = await _zoneService.DeleteAsync(id, CurrentUserId);
            if (!success) return Forbid();
            return Ok();
        }
    }
}
