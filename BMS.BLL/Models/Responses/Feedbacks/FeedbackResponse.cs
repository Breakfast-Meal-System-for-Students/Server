﻿using BMS.Core.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.BLL.Models.Responses.Feedbacks
{
    public class FeedbackResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public int Rate { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;


    }
}
