using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using caseStudy.Data;
using caseStudy.Data.Models;

namespace caseStudy.Services
{
    public class ExercisesService
    {
        private readonly DataContext _context;

        public ExercisesService(DataContext context)
        {
            _context = context;
        }

        public List<Exercise> GetExercises() {
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