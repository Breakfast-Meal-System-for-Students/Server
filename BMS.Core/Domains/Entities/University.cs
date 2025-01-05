using BMS.Core.Domains.Entities.BaseEntities;
using System;
using System.Collections.Generic;

namespace BMS.Core.Domains.Entities
{
    public class University : EntityBase<Guid>, ISoftDelete
    {
        // Properties
        public string Name { get; set; } = null!; // University name
        public string Address { get; set; } = null!; // University address
        public string EndMail { get; set; } = null!; // Email domain for university students
        public double? Lng { get; set; } // Longitude of the university's location
        public double? Lat { get; set; } // Latitude of the university's location
        public string IdStudentFormat { get; set; } = null!; // Format of student IDs
        public string Abbreviation { get; set; } = null!; // Short form of the university name

        // Relationships
        public ICollection<ShopUniversity> ShopUniversities { get; set; } = new List<ShopUniversity>();
        public ICollection<StudentApplication>? StudentApplications { get; set; } = new List<StudentApplication>(); // Applications related to this university

        // Soft delete properties
        public bool IsDeleted { get; set; } = false; // Default value
        public DateTime? DeletedDate { get; set; }
    }
}
