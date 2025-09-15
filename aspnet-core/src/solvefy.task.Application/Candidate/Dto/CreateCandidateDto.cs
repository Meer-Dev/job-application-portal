using System.ComponentModel.DataAnnotations;

namespace solvefy.task.Candidate.Dto
{
    public class CreateCandidateDto
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