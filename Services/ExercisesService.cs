using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using caseStudy.Data;
using caseStudy.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace caseStudy.Services
{
    public class ExercisesService
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public ExercisesService(DataContext context, ILogger<ExercisesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Exercise> GetExercises() {
            _logger.LogInformation("Getting exercises");
            return _context.Exercises.ToList();
        }

        public Exercise GetExercisesByExerciseId(int exerciseId) {

            var _exercise = _context.Exercises.Find(exerciseId);
            if(_exercise != null) {
                return _exercise;
            }else {
                throw new InvalidOperationException($"Exercise with id {exerciseId} not found");
            }
        }

        public Exercise AddExercise(string createdBy, Exercise exercise) {
            try
            {
                
                var _exercise = new Exercise()
                {
                    ExerciseName = exercise.ExerciseName,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.Now,
                    LastUpdatedBy = createdBy,
                    Duration = exercise.Duration,
                    LastUpdatedAt = DateTime.Now,
                    WorkoutId = exercise.WorkoutId
                };
                _context.Exercises.Add(_exercise);
                _context.SaveChanges();
                return _exercise;
            }
            catch (DbUpdateException ex)
            {
                // Veritabanı güncelleme hatası
                _logger.LogError(ex, "Updating database error");
                throw;
            }
            catch (ValidationException ex)
            {
                // Veri doğrulama hatası
                _logger.LogError(ex, "Validation error");
                throw;
            }
            catch (Exception ex)
            {
                // Diğer hatalar
                _logger.LogError(ex, "Error occurred");
                throw;
            }
        }

        public Exercise UpdateExercise(int exerciseId, string updatedBy, Exercise exercise)
        {
            var _exercise = _context.Exercises.Find(exerciseId);
            if(_exercise != null) {
                _exercise.ExerciseName = exercise.ExerciseName;
                _exercise.CreatedBy = _exercise.CreatedBy;
                _exercise.LastUpdatedBy = updatedBy;
                _exercise.Duration = exercise.Duration;
                _exercise.LastUpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }else {
                throw new InvalidOperationException($"Exercise with id {exerciseId} not found");
            }
            return _exercise;
        }

        public void DeleteExercise(int exerciseId)
        {
            var _exercise = _context.Exercises.Find(exerciseId);
            if(_exercise != null) {
                _context.Exercises.Remove(_exercise);
                _context.SaveChanges();
            }else {
                throw new InvalidOperationException($"Exercise with id {exerciseId} not found");
            }
        }
    }
}