using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Base.API.Controllers
{
    [Authorize]
    [Route("api/zone-crops")]
    [ApiController]
    public class ZoneCropsController : ControllerBase
    {
        private readonly IZoneCropService _zoneCropService;
        private readonly IHttpContextAccessor _http;

        public ZoneCropsController(IZoneCropService zoneCropService, IHttpContextAccessor http)
        {
            _zoneCropService = zoneCropService;
            _http = http;
        }

        private string UserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/zone-crops/list?zoneId=xxx&page=1&pageSize=20
        [HttpGet("list")]
        public async Task<ActionResult<ZoneCropListDto>> GetAllByZone(string zoneId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
            => Ok(await _zoneCropService.GetAllByZoneAsync(zoneId, UserId, page, pageSize));

        // GET: api/zone-crops/get?id=xxx
        [HttpGet("get")]
        public async Task<ActionResult<ZoneCropDto>> GetById(string id)
            => await _zoneCropService.GetByIdAsync(id, UserId) is ZoneCropDto zc ? Ok(zc) : NotFound();

        // POST: api/zone-crops/create
        [HttpPost("create")]
        public async Task<ActionResult<ZoneCropDto>> Create(CreateZoneCropRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            return await _zoneCropService.CreateAsync(req, UserId) is ZoneCropDto zc ? Ok(zc) : BadRequest();
        }

        // PUT: api/zone-crops/update?id=xxx
        [HttpPut("update")]
        public async Task<ActionResult<ZoneCropDto>> Update(string id, UpdateZoneCropRequest req)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            return await _zoneCropService.UpdateAsync(id, req, UserId) is ZoneCropDto zc ? Ok(zc) : NotFound();
        }

        // DELETE: api/zone-crops/delete?id=xxx
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            return await _zoneCropService.DeleteAsync(id, UserId) ? Ok() : Forbid();
        }
    }
}
