using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface IZoneCropService
    {
        Task<ZoneCropListDto> GetAllByZoneAsync(string zoneId, string currentUserId, int page = 1, int pageSize = 20);
        Task<ZoneCropDto?> GetByIdAsync(string id, string currentUserId);
        Task<ZoneCropDto> CreateAsync(CreateZoneCropRequest request, string creatorId);
        Task<ZoneCropDto?> UpdateAsync(string id, UpdateZoneCropRequest request, string updaterId);
        Task<bool> DeleteAsync(string id, string userId);
    }
}
