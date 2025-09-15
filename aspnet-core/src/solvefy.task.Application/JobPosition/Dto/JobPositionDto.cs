using System;
using Abp.Application.Services.Dto;

namespace solvefy.task.JobPosition.Dto
{
    public class JobPositionDto : EntityDto<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
    }
}