using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace solvefy.task.Candidate.Dto
{
    public class UpdateCandidateDto : EntityDto<int>
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
    }
}