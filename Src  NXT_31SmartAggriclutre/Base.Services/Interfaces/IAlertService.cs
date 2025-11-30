using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface IAlertService
    {
        Task<AlertListDto> GetAllByZoneAsync(string zoneId, string currentUserId, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 50);
        Task<AlertDto?> GetByIdAsync(string id, string currentUserId);
        Task<AlertDto> CreateAsync(CreateAlertRequest request, string creatorId); // يُستخدم تلقائيًا أو manual
        Task<bool> MarkAsResolvedAsync(string id, string userId);
    }
}
