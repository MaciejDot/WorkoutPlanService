using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.GuidService
{
    public class GuidProvider : IGuidProvider
    {
        public Guid GetGuid()
        {
            return Guid.NewGuid();
        }
    }
}
