using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface IEquipmentService
    {
        Task<EquipmentListDto> GetAllByZoneAsync(string zoneId, string currentUserId, int page = 1, int pageSize = 20);
        Task<EquipmentDto?> GetByIdAsync(string id, string currentUserId);
        Task<EquipmentDto> CreateAsync(CreateEquipmentRequest request, string creatorId);
        Task<EquipmentDto?> UpdateAsync(string id, UpdateEquipmentRequest request, string updaterId);
        Task<bool> DeleteAsync(string id, string userId);
    }
}
