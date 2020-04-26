using System;

namespace WorkoutPlanService.Domain.Helpers
{
    public interface IGuidProvider
    {
        Guid GetGuid();
    }
}