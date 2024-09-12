using BMS.Core.Domains.Entities.BaseEntities;
using BMS.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities
{
    public class Transaction : EntityBase<Guid>
    {



        public Guid OrderId { get; set; } 

        public double Price { get; set; }

        public string Method { get; set; } = null!;

        public TransactionStatus Status { get; set; }


        public Order Order { get; set; }
    }

}
