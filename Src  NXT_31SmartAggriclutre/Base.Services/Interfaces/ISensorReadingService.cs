using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface ISensorReadingService
    {
        Task<SensorReadingListDto> GetAllByEquipmentAsync(string equipmentId, string currentUserId, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 50);
        Task<SensorReadingDto?> GetByIdAsync(string id, string currentUserId);
        Task<SensorReadingDto> CreateAsync(CreateSensorReadingRequest request, string creatorId); // هنا يتم التحقق وإنشاء Alert إذا لزم
    }
}
