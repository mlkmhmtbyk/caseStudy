using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using caseStudy.Data;
using caseStudy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace caseStudy.Services
{
    public class WorkoutsService
    {
        private readonly DataContext _context;

        public WorkoutsService(DataContext context)
        {
            _context = context;
        }

        public List<Workout> GetWorkoutsWithoutExercies() {
            return _context.Workouts.ToList();
        }
        public List<Workout> GetWorkoutsWithExercises() {  
            var _workouts = _context.Workouts.ToList();
            foreach(var _workout in _workouts) {
                _workout.Exercises = _context.Exercises.Where(x => x.WorkoutId == _workout.WorkoutId).ToList();
            }
            return _workouts;
        }

        public Workout GetWorkout(int workoutId)
        {
            var _workout = _context.Workouts.Find(workoutId);
            if(_workout != null) {
                _workout.Exercises = _context.Exercises.Where(x => x.WorkoutId == workoutId).ToList();
            }else {
                throw new InvalidOperationException($"Workout with id {workoutId} not found");
            }
            return _workout;
        }

        public Workout AddWorkout(string createdBy, Workout workout) {
            var _workout = new Workout()
            {
                WorkoutName = workout.WorkoutName,
                CreatedBy = createdBy,
                LastUpdatedBy = createdBy,
                DifficultyLevel = workout.DifficultyLevel,
                FocusArea = workout.FocusArea,
            };
            _context.Workouts.Add(_workout);
            _context.SaveChanges();
            return _workout;
        }

        public Workout UpdateWorkout(int workoutId, string updatedBy, Workout workout)
        {
            var _workout = _context.Workouts.Find(workoutId);
            if(_workout != null) {
                _workout.WorkoutName = workout.WorkoutName;
                _workout.CreatedBy = workout.CreatedBy;
                _workout.LastUpdatedBy = updatedBy;
                _workout.LastUpdatedAt = DateTime.Now;
                _workout.DifficultyLevel = workout.DifficultyLevel;
                _workout.FocusArea = workout.FocusArea;
                _context.SaveChanges();
                return _workout;
            }else {
                throw new InvalidOperationException($"Workout with id {workout.WorkoutId} not found");
            }
        }

        public void DeleteWorkout(int workoutId)
        {
            var _workout = _context.Workouts.FirstOrDefault(x => x.WorkoutId == workoutId);
            if(_workout != null) {
                _context.Workouts.Remove(_workout);
                _context.SaveChanges();
            }else {
                throw new InvalidOperationException($"Workout with id {workoutId} not found");
            }
        }

        public List<Workout> FilterWorkoutsWithAnd(string difficultyLevel, string focusArea, int totalTime) {
            var filteredWorkouts = _context.Workouts
                .Include(w => w.Exercises)
                .Where(w => 
                    (string.IsNullOrEmpty(focusArea) || w.FocusArea == focusArea) &&
                    (string.IsNullOrEmpty(difficultyLevel) || w.DifficultyLevel == difficultyLevel) &&
                    (w.Exercises.Any() ? w.Exercises.Sum(e => e.Duration) == totalTime : false)
                ).ToList();
            return filteredWorkouts;
        }

        public List<Workout> FilterWorkoutsWithOr(string difficultyLevel, string focusArea, int totalTime) {
            var filteredWorkouts = _context.Workouts
                .Include(w => w.Exercises)
                .Where(w => 
                    (string.IsNullOrEmpty(focusArea) || w.FocusArea == focusArea) ||
                    (string.IsNullOrEmpty(difficultyLevel) || w.DifficultyLevel == difficultyLevel) ||
                    (w.Exercises.Any() ? w.Exercises.Sum(e => e.Duration) == totalTime : false)
                ).ToList();
            return filteredWorkouts;
        }
    }
}