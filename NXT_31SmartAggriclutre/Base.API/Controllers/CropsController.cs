using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Base.API.Controllers
{
    // Controllers/CropsController.cs
    [Route("api/crops")]
    [ApiController]
    public class CropsController : ControllerBase
    {
        private readonly ICropService _cropService;
        private readonly IHttpContextAccessor _http;

        public CropsController(ICropService cropService, IHttpContextAccessor http)
        {
            _cropService = cropService;
            _http = http;
        }

        private string UserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/crops
        [HttpGet("list")]
        [Authorize]
        public async Task<ActionResult<CropListDto>> GetAll([FromQuery] string? search, int page = 1, int pageSize = 50)
            => Ok(await _cropService.GetAllAsync(search, page, pageSize));

        // GET: api/crops/{id}
        [HttpGet("get-crop")]
        [Authorize]
        public async Task<ActionResult<CropDetailsDto>> GetById(string id)
            => await _cropService.GetByIdAsync(id) is CropDetailsDto crop ? Ok(crop) : NotFound();

        // POST: api/crops
        [HttpPost("create")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<ActionResult<CropDto>> Create(CreateCropRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            return await _cropService.CreateAsync(req, UserId) is CropDto crop ? Ok(crop) : BadRequest();

        }

        // PUT: api/crops/{id}
        [HttpPut("update")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<ActionResult<CropDto>> Update(string id, UpdateCropRequest req)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            return await _cropService.UpdateAsync(id, req, UserId) is CropDto crop ? Ok(crop) : NotFound();

        }

        // DELETE: api/crops/{id}
        [HttpDelete("delete")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            return await _cropService.DeleteAsync(id) ? Ok() : BadRequest("The crop cannot be deleted if it has stages.");
        }


        // === Stages ===
        [HttpPost("stage-create")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<ActionResult<CropStageDto>> AddStage(CreateStageRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            return await _cropService.AddStageAsync(req, UserId) is CropStageDto c ? Ok(c) : BadRequest();
        }

        [HttpPut("stage-update")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<ActionResult<CropStageDto>> UpdateStage(string stageId, UpdateStageRequest req)
        {
            if (string.IsNullOrWhiteSpace(stageId)) throw new ArgumentNullException(nameof(stageId));

            return await _cropService.UpdateStageAsync(stageId, req, UserId) is CropStageDto s ? Ok(s) : NotFound();
        }

        [HttpDelete("stage-delete")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<IActionResult> DeleteStage(string stageId)
        {
            if (string.IsNullOrWhiteSpace(stageId)) throw new ArgumentNullException(nameof(stageId));
            return await _cropService.DeleteStageAsync(stageId) ? Ok() : NotFound();
        }


        // === Requirements ===
        [HttpPost("requirement-create")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<ActionResult<StageRequirementDto>> AddRequirement(CreateRequirementRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            return await _cropService.AddRequirementAsync(req, UserId) is StageRequirementDto s ? Ok(s) : BadRequest();
        }
        [HttpDelete("requirement-delete")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<IActionResult> DeleteRequirement(string reqId)
        {
            if (string.IsNullOrWhiteSpace(reqId)) throw new ArgumentNullException(nameof(reqId));
            return await _cropService.DeleteRequirementAsync(reqId) ? Ok() : NotFound();
        }

        // === Seasons ===
        [HttpPost("season-create")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<ActionResult<CropSeasonDto>> AddSeason(CreateSeasonRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));
            return await _cropService.AddSeasonAsync(req, UserId) is CropSeasonDto s ? Ok(s) : BadRequest();
        }

        [HttpPut("season-update")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<ActionResult<CropSeasonDto>> UpdateSeason(string seasonId, UpdateSeasonRequest req)
        {
            if (string.IsNullOrWhiteSpace(seasonId)) throw new ArgumentNullException(nameof(seasonId));
            return await _cropService.UpdateSeasonAsync(seasonId, req, UserId) is CropSeasonDto s ? Ok(s) : NotFound();
        }

        [HttpDelete("season-delete")]
        [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
        public async Task<IActionResult> DeleteSeason(string seasonId)
        {
            if (string.IsNullOrWhiteSpace(seasonId)) throw new ArgumentNullException(nameof(seasonId));
            return await _cropService.DeleteSeasonAsync(seasonId) ? Ok() : NotFound();
        }
    }
}
