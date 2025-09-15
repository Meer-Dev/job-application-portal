using System.ComponentModel.DataAnnotations;

namespace solvefy.task.JobPosition.Dto
{
    public class CreateJobPositionDto
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
