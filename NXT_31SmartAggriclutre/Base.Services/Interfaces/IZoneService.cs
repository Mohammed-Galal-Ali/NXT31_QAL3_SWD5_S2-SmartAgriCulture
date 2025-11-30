using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface IZoneService
    {
        Task<ZoneListDto> GetAllByFarmAsync(string farmId, string currentUserId, int page = 1, int pageSize = 20);
        Task<ZoneListDto> GetAllAsync(string currentUserId, string? search, int page = 1, int pageSize = 20);
        Task<ZoneDto?> GetByIdAsync(string id, string currentUserId);
        Task<ZoneDto> CreateAsync(CreateZoneRequest request, string creatorId);
        Task<ZoneDto?> UpdateAsync(string id, UpdateZoneRequest request, string updaterId);
        Task<bool> DeleteAsync(string id, string userId);
    }
}
