using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;

namespace solvefy.task.Entities
{
    public class Candidate : CreationAuditedEntity<int>
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        public int JobPositionId { get; set; }

        [MaxLength(500)]
        public string ResumePath { get; set; }

        public virtual JobPosition JobPosition { get; set; }
    }
}