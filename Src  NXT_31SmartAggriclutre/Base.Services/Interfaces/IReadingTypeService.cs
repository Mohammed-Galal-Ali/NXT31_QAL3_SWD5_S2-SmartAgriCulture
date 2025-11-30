using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Interfaces
{
    public interface IReadingTypeService
    {
        Task<ReadingTypeListDto> GetAllAsync(string? search, int page, int pageSize);
        Task<ReadingTypeDto?> GetByIdAsync(string id);
        Task<ReadingTypeDto> CreateAsync(CreateReadingTypeRequest req, string userId);
        Task<ReadingTypeDto?> UpdateAsync(string id, UpdateReadingTypeRequest req, string userId);
        Task<bool> DeleteAsync(string id);
    }
}
