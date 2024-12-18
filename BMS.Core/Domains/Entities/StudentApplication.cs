using BMS.Core.Domains.Entities.BaseEntities;
using System;

namespace BMS.Core.Domains.Entities
{
    public class StudentApplication : EntityBase<Guid>, ISoftDelete
    {
        // Properties
        public Guid UserId { get; set; }
        public string UniversityName { get; set; } = null!;
        public string MSSV { get; set; } = null!;
        public Guid ImageId { get; set; }

        // Relationships
        public Guid UniversityId { get; set; } 
        public University University { get; set; } = null!; 

        public User User { get; set; } = null!; 
        public Image Image { get; set; } = null!; 

        // Implement ISoftDelete properties
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
    }
}
