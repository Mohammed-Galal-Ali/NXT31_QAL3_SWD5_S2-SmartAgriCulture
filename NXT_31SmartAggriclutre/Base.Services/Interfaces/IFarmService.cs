using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface IFarmService
    {
        Task<FarmListDto> GetAllAsync(string? currentUserId, string? search, int page = 1, int pageSize = 20);
        Task<FarmDto?> GetByIdAsync(string id, string currentUserId);
        Task<FarmDto> CreateAsync(CreateFarmRequest request, string creatorId);
        Task<FarmDto?> UpdateAsync(string id, UpdateFarmRequest request, string updaterId);
        Task<bool> DeleteAsync(string id, string userId);
    }
}
