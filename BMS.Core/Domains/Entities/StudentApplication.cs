using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;

namespace BMS.Core.Domains.Entities
{
    public class StudentApplication : EntityBase<Guid>, ISoftDelete
    {
        // Properties
        public Guid UserId { get; set; }
        public string StudentId { get; set; } = null!;
        public string ImageCardStudent { get; set; } = null!;

        // Relationships
        public Guid UniversityId { get; set; } 
        public University University { get; set; } = null!; 

        public User User { get; set; } = null!;
        public StudentStatus? StatusStudent { get; set; }

        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
