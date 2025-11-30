using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Base.API.Controllers
{
    [Authorize]
    [Route("api/equipments")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;
        private readonly IHttpContextAccessor _http;

        public EquipmentsController(IEquipmentService equipmentService, IHttpContextAccessor http)
        {
            _equipmentService = equipmentService;
            _http = http;
        }

        private string UserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/equipments/list?zoneId=xxx&page=1&pageSize=20
        [HttpGet("list")]
        public async Task<ActionResult<EquipmentListDto>> GetAllByZone(string zoneId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
            => Ok(await _equipmentService.GetAllByZoneAsync(zoneId, UserId, page, pageSize));

        // GET: api/equipments/get?id=xxx
        [HttpGet("get")]
        public async Task<ActionResult<EquipmentDto>> GetById(string id)
            => await _equipmentService.GetByIdAsync(id, UserId) is EquipmentDto eq ? Ok(eq) : NotFound();

        // POST: api/equipments/create
        [HttpPost("create")]
        public async Task<ActionResult<EquipmentDto>> Create(CreateEquipmentRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            return await _equipmentService.CreateAsync(req, UserId) is EquipmentDto eq ? Ok(eq) : BadRequest();
        }

        // PUT: api/equipments/update?id=xxx
        [HttpPut("update")]
        public async Task<ActionResult<EquipmentDto>> Update(string id, UpdateEquipmentRequest req)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            return await _equipmentService.UpdateAsync(id, req, UserId) is EquipmentDto eq ? Ok(eq) : NotFound();
        }

        // DELETE: api/equipments/delete?id=xxx
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            return await _equipmentService.DeleteAsync(id, UserId) ? Ok() : Forbid();
        }
    }
}
