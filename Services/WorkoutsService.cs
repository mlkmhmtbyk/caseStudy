using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
        private readonly Logger _logger;

        public WorkoutsService(DataContext context, Logger logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Workout> GetWorkoutsWithExercises() {  
            try
            {
                var _workouts = _context.Workouts.ToList();
                foreach(var _workout in _workouts) {
                    _workout.Exercises = _context.Exercises.Where(x => x.WorkoutId == _workout.WorkoutId).ToList();
                }
                return _workouts;
            }
            catch (DbException ex)
            {
                _logger.Log($"An error occurred while getting exercises: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while getting exercises: {ex.Message}");
                throw;
            }
        }

        public Workout GetWorkout(int workoutId)
        {
            try{
                var _workout = _context.Workouts.Find(workoutId);
                if(_workout != null) {
                    _workout.Exercises = _context.Exercises.Where(x => x.WorkoutId == workoutId).ToList();
                }else {
                    throw new InvalidOperationException($"Workout with id {workoutId} not found");
                }
                return _workout;
            }
            catch (InvalidOperationException ex){
                _logger.Log(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while getting exercise with id {workoutId}: {ex.Message}");
                throw;
            }  
        }

        public Workout AddWorkout(string createdBy, Workout workout) {
            try
            {
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
            catch (DbUpdateException ex)
            {
                _logger.Log($"An error occurred while adding workout: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while adding workout: {ex.Message}");
                throw;
            }
        }

        public Workout UpdateWorkout(int workoutId, string updatedBy, Workout workout)
        {
            try
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
            catch (DbUpdateException ex)
            {
                _logger.Log($"An error occurred while updating workout: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while updating workout: {ex.Message}");
                throw;
            }
        }

        public void DeleteWorkout(int workoutId)
        {
            try
            {
                var _workout = _context.Workouts.FirstOrDefault(x => x.WorkoutId == workoutId);
                if(_workout != null) {
                    _context.Workouts.Remove(_workout);
                    _context.SaveChanges();
                }else {
                    throw new InvalidOperationException($"Workout with id {workoutId} not found");
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.Log($"An error occurred while deleting workout: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while deleting workout: {ex.Message}");
                throw;
            }
        }

        public List<Workout> FilterWorkoutsWithAnd(string difficultyLevel, string focusArea, int totalTime) {
            try{
                var filteredWorkouts = _context.Workouts
                    .Include(w => w.Exercises)
                    .Where(w => 
                        (string.IsNullOrEmpty(focusArea) || w.FocusArea == focusArea) &&
                        (string.IsNullOrEmpty(difficultyLevel) || w.DifficultyLevel == difficultyLevel) &&
                        (w.Exercises.Any() ? w.Exercises.Sum(e => e.Duration) == totalTime : false)
                    ).ToList();
                return filteredWorkouts;
            }
            catch (DbUpdateException ex)
            {
                _logger.Log($"An error occurred while filtering workouts: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while filtering workouts: {ex.Message}");
                throw;
            }
        }

        public List<Workout> FilterWorkoutsWithOr(string difficultyLevel, string focusArea, int totalTime) {
            try
            {
                var filteredWorkouts = _context.Workouts
                    .Include(w => w.Exercises)
                    .Where(w => 
                        (string.IsNullOrEmpty(focusArea) || w.FocusArea == focusArea) ||
                        (string.IsNullOrEmpty(difficultyLevel) || w.DifficultyLevel == difficultyLevel) ||
                        (w.Exercises.Any() ? w.Exercises.Sum(e => e.Duration) == totalTime : false)
                    ).ToList();
                return filteredWorkouts;
            }
            catch (DbUpdateException ex)
            {
                _logger.Log($"An error occurred while filtering workouts: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while filtering workouts: {ex.Message}");
                throw;
            }
        }
    }
}