using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace caseStudy.Data.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public required string ExerciseName { get; set; }
        public int Duration { get; set; } = 0;
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime LastUpdatedAt {get; set;} = DateTime.Now;
        public string? CreatedBy {get; set;}
        public string? LastUpdatedBy {get; set;}
        
        public int? WorkoutId { get; set; }
    }
}