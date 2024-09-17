using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace caseStudy.Data.Models
{
    public class Workout
    {
        public int WorkoutId {get; set;}
        public required string WorkoutName  {get; set;} 
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime LastUpdatedAt {get; set;} = DateTime.Now;
        public string? CreatedBy {get; set;}
        public string? LastUpdatedBy {get; set;}
        public string? DifficultyLevel { get; set; }
        public string? FocusArea {get; set;}

        public List<Exercise>? Exercises {get; set;} 
    }
}