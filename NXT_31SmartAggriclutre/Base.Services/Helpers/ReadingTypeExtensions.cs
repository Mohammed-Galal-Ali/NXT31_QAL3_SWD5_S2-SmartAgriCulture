using Base.DAL.Models.SystemModels;
using Base.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Services.Helpers
{
    public static class ReadingTypeExtensions
    {
        public static ReadingTypeDto ToReadingTypeDto(this ReadingType rt)
        {
            return new ReadingTypeDto
            {
                Id = rt.Id,
                Code = rt.Code,
                Category = rt.Category,
                DisplayName = rt.DisplayName,
                Unit = rt.Unit
            };
        }

        public static IEnumerable<ReadingTypeDto> ToReadingTypeDtoSet(this IEnumerable<ReadingType> readingTypes)
        {
            return readingTypes.Select(rt => rt.ToReadingTypeDto());
        }

        public static ReadingType ToReadingType(this CreateReadingTypeRequest request)
        {
            return new ReadingType
            {
                Code = request.Code,
                Category = request.Category ?? string.Empty,
                DisplayName = request.DisplayName,
                Unit = request.Unit
            };
        }

        public static void ToReadingType(this UpdateReadingTypeRequest request, ReadingType rt)
        {
            if (!string.IsNullOrEmpty(request.Code)) rt.Code = request.Code;
            if (request.Category != null) rt.Category = request.Category;
            if (!string.IsNullOrEmpty(request.DisplayName)) rt.DisplayName = request.DisplayName;
            if (!string.IsNullOrEmpty(request.Unit)) rt.Unit = request.Unit;
        }
    }
}
