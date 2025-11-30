using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Shared.DTOs
{
    public class CreateEquipmentRequest
    {
        public string ZoneId { get; set; } = null!;
        public string ReadingTypeId { get; set; } = null!; // نوع القراءة (درجة حرارة، رطوبة، إلخ)
        public string SerialNumber { get; set; } = null!; // كود الجهاز الفريد
        public string EquipmentModel { get; set; } = null!; // كود الجهاز الفريد
        public DateTime InstallationDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        // اضف حقول تانية لو لزم (مثل Location داخل الـ Zone)
    }

    public class UpdateEquipmentRequest
    {
        public string? ReadingTypeId { get; set; }
        public bool IsActive { get; set; }
        // اضف لو لزم (مثل MaintenanceDate)
    }

    public class EquipmentDto
    {
        public string Id { get; set; } = null!;
        public string ZoneId { get; set; } = null!;
        public string ReadingTypeId { get; set; } = null!;
        public string ReadingTypeName { get; set; } = null!;
        public string EquipmentModel { get; set; } = null!;
        public string SerialNumber { get; set; } = null!; // كود الجهاز الفريد

        public DateTime InstallationDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class EquipmentListDto
    {
        public List<EquipmentDto> Equipments { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
