using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace solvefy.task.Entities
{
    public class JobPosition : CreationAuditedEntity<int>
    {
        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [MaxLength(256)]
        public string Location { get; set; }

        public bool IsActive { get; set; } = true;
    }
}