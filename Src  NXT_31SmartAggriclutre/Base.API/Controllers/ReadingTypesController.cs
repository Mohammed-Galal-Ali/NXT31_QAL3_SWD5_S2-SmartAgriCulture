using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Base.API.Controllers
{
    [Authorize(Roles = nameof(UserTypes.SystemAdmin))]
    [Route("api/readingtypes")]
    [ApiController]
    public class ReadingTypesController : ControllerBase
    {
        private readonly IReadingTypeService _readingTypeService;
        private readonly IHttpContextAccessor _http;

        public ReadingTypesController(IReadingTypeService readingTypeService, IHttpContextAccessor http)
        {
            _readingTypeService = readingTypeService;
            _http = http;
        }

        private string UserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // GET: api/readingtypes/list
        [HttpGet("list")]
        [AllowAnonymous]
        public async Task<ActionResult<ReadingTypeListDto>> GetAll([FromQuery] string? search, int page = 1, int pageSize = 50)
            => Ok(await _readingTypeService.GetAllAsync(search, page, pageSize));

        // GET: api/readingtypes/get-readingtype
        [HttpGet("get-readingtype")]
        public async Task<ActionResult<ReadingTypeDto>> GetById(string id)
            => await _readingTypeService.GetByIdAsync(id) is ReadingTypeDto rt ? Ok(rt) : NotFound();

        // POST: api/readingtypes/create
        [HttpPost("create")]
        public async Task<ActionResult<ReadingTypeDto>> Create(CreateReadingTypeRequest req)
        {
            if (req == null) throw new ArgumentNullException(nameof(req));

            return await _readingTypeService.CreateAsync(req, UserId) is ReadingTypeDto rt ? Ok(rt) : BadRequest();
        }

        // PUT: api/readingtypes/update
        [HttpPut("update")]
        public async Task<ActionResult<ReadingTypeDto>> Update(string id, UpdateReadingTypeRequest req)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));

            return await _readingTypeService.UpdateAsync(id, req, UserId) is ReadingTypeDto rt ? Ok(rt) : NotFound();
        }

        // DELETE: api/readingtypes/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentNullException(nameof(id));
            return await _readingTypeService.DeleteAsync(id) ? Ok() : BadRequest("The reading type cannot be deleted if it has relations.");
        }
    }
}
