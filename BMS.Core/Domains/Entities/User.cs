﻿using BMS.Core.Domains.Entities.BaseEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;

        public ICollection<UserRole>? UserRoles { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Shop> Shops { get; set; } = new List<Shop>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
    }
}