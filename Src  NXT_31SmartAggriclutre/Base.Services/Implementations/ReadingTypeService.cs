using Base.DAL.Models.SystemModels;
using Base.Repo.Interfaces;
using Base.Services.Interfaces;
using Base.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using RepositoryProject.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Implementations
{
    public class ReadingTypeService : IReadingTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReadingTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReadingTypeListDto> GetAllAsync(string? search, int page, int pageSize)
        {
            var repo = _unitOfWork.Repository<ReadingType>();
            var query = new BaseSpecification<ReadingType>();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(rt => rt.Code.Contains(search) || rt.DisplayName.Contains(search));

            var total = await repo.CountAsync(query);

            query.AddOrderBy(rt => rt.Code);
            query.ApplyPaging((page - 1) * pageSize, pageSize);

            var list = await repo.ListAsync(query, true);
            var dtos = list.Select(rt => new ReadingTypeDto
            {
                Id = rt.Id,
                Code = rt.Code,
                Category = rt.Category,
                DisplayName = rt.DisplayName,
                Unit = rt.Unit
            }).ToList();

            return new ReadingTypeListDto
            {
                ReadingTypes = dtos,
                TotalCount = total
            };
        }

        public async Task<ReadingTypeDto?> GetByIdAsync(string id)
        {
            var repo = _unitOfWork.Repository<ReadingType>();
            var query = new BaseSpecification<ReadingType>(rt => rt.Id == id);
            var rt = await repo.GetEntityWithSpecAsync(query, true);
            return rt == null ? null : new ReadingTypeDto
            {
                Id = rt.Id,
                Code = rt.Code,
                Category = rt.Category,
                DisplayName = rt.DisplayName,
                Unit = rt.Unit
            };
        }

        public async Task<ReadingTypeDto> CreateAsync(CreateReadingTypeRequest request, string userId)
        {
            var repo = _unitOfWork.Repository<ReadingType>();
            var rt = new ReadingType
            {
                Id = Guid.NewGuid().ToString(),
                Code = request.Code,
                Category = request.Category ?? "",
                DisplayName = request.DisplayName,
                Unit = request.Unit
            };
            await repo.AddAsync(rt);
            await _unitOfWork.CompleteAsync();
            return new ReadingTypeDto
            {
                Id = rt.Id,
                Code = rt.Code,
                Category = rt.Category,
                DisplayName = rt.DisplayName,
                Unit = rt.Unit
            };
        }

        public async Task<ReadingTypeDto?> UpdateAsync(string id, UpdateReadingTypeRequest request, string userId)
        {
            var repo = _unitOfWork.Repository<ReadingType>();
            var rt = await repo.GetByIdAsync(id);
            if (rt == null) return null;

            if (!string.IsNullOrEmpty(request.Code)) rt.Code = request.Code;
            if (!string.IsNullOrEmpty(request.Category)) rt.Category = request.Category;
            if (!string.IsNullOrEmpty(request.DisplayName)) rt.DisplayName = request.DisplayName;
            if (!string.IsNullOrEmpty(request.Unit)) rt.Unit = request.Unit;

            await repo.UpdateAsync(rt);
            await _unitOfWork.CompleteAsync();
            return new ReadingTypeDto
            {
                Id = rt.Id,
                Code = rt.Code,
                Category = rt.Category,
                DisplayName = rt.DisplayName,
                Unit = rt.Unit
            };
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var repo = _unitOfWork.Repository<ReadingType>();
            var query = new BaseSpecification<ReadingType>(rt => rt.Id == id);
            query.AllIncludes.Add(q => q.Include(rt => rt.cropStageRequirments)
                                        .Include(rt => rt.Alerts)
                                        .Include(rt => rt.SensorReadings)
                                        .Include(rt => rt.Equipments));
            var rt = await repo.GetEntityWithSpecAsync(query);
            if (rt == null) return false;

            if (rt.cropStageRequirments.Any() || rt.Alerts.Any() || rt.SensorReadings.Any() || rt.Equipments.Any())
                return false; // لا يمكن الحذف إذا كان هناك علاقات

            await repo.DeleteAsync(rt);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
