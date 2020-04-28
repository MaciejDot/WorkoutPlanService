using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.DTO
{
    public sealed class WorkoutPlanThumbnailDTO
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
