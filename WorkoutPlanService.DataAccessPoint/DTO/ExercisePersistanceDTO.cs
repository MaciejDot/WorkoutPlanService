using System;
using System.Collections.Generic;
using System.Text;

namespace WorkoutPlanService.DataAccessPoint.DTO
{
    public class ExercisePersistanceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return (obj == null && this == null) ||
                (obj.GetType() == this.GetType() &&
                (
                    ((ExercisePersistanceDTO)obj).Id == this.Id &&
                    ((ExercisePersistanceDTO)obj).Name == this.Name
                )
                );
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * Id.GetHashCode() * (Name?.GetHashCode() ?? 1);
        }
    }
}
