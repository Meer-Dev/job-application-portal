using System;
using Abp.Application.Services.Dto;

namespace solvefy.task.Candidate.Dto
{
    public class CandidateDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int JobPositionId { get; set; }
        public string ResumePath { get; set; }
        public DateTime CreationTime { get; set; }
        public string JobPositionTitle { get; set; }
    }
}