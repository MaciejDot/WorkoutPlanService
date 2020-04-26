using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.Domain.Helpers
{
    public class GuidProvider : IGuidProvider
    {
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}
