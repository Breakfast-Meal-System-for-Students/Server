using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Domains.Entities.BaseEntities
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; set; }
    }

    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow.AddHours(7);
    }

    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedDate { get; set; }
    }
}
