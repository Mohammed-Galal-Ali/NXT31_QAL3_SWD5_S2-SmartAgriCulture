using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class ReadingTypeDto
    {
        public string Id { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Unit { get; set; } = null!;
    }

    public class ReadingTypeListDto
    {
        public List<ReadingTypeDto> ReadingTypes { get; set; } = new List<ReadingTypeDto>();
        public int TotalCount { get; set; }
    }

    public class CreateReadingTypeRequest
    {
        public string Code { get; set; } = null!;
        public string? Category { get; set; }
        public string DisplayName { get; set; } = null!;
        public string Unit { get; set; } = null!;
    }

    public class UpdateReadingTypeRequest
    {
        public string? Code { get; set; }
        public string? Category { get; set; }
        public string? DisplayName { get; set; }
        public string? Unit { get; set; }
    }
}
