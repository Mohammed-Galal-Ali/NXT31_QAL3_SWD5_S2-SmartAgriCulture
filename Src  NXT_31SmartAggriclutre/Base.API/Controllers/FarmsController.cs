using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Base.API.Controllers
{
    [Authorize]
    [Route("api/farms")]
    [ApiController]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmService _farmService;
        private readonly IHttpContextAccessor _httpContext;

        public FarmsController(IFarmService farmService, IHttpContextAccessor httpContext)
        {
            _farmService = farmService;
            _httpContext = httpContext;
        }

        private string CurrentUserId => _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/farms
        [HttpGet("list")]
        public async Task<ActionResult<FarmListDto>> GetAll(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var result = await _farmService.GetAllAsync(CurrentUserId, search, page, pageSize);
            return Ok(result);
        }

        // GET: api/farms/{id}
        [HttpGet("get-farm")]
        public async Task<ActionResult<FarmDto>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException((nameof(id)));
            var farm = await _farmService.GetByIdAsync(id, CurrentUserId);
            if (farm == null) return NotFound(); // أو NotFound حسب تفضيلك
            return Ok(farm);
        }

        // POST: api/farms
        [HttpPost("create")]
        public async Task<ActionResult<FarmDto>> Create(CreateFarmRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var farm = await _farmService.CreateAsync(request, CurrentUserId);
            if (farm == null) return NotFound();
            return Ok(farm);
        }

        // PUT: api/farms/{id}
        [HttpPut("update")]
        public async Task<ActionResult<FarmDto>> Update(string id, UpdateFarmRequest request)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException((nameof(id)));
            if (request == null) throw new ArgumentNullException(nameof(request));
            var farm = await _farmService.UpdateAsync(id, request, CurrentUserId);
            if (farm == null) return NotFound();
            return Ok(farm);
        }

        // DELETE: api/farms/{id}
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            var success = await _farmService.DeleteAsync(id, CurrentUserId);
            if (!success) return Forbid();
            return Ok();
        }
    }
}
