using System;

namespace WorkoutPlanService.DataAccessPoint.GuidService
{
    public interface IGuidProvider
    {
        Guid GetGuid();
    }
}