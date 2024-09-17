using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using caseStudy.Data;
using caseStudy.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;


namespace caseStudy.Services
{
    public class ExercisesService
    {
        private readonly DataContext _context;
        private readonly Logger _logger;
        private readonly IConfiguration _configuration;

        public ExercisesService(DataContext context, Logger logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        
        public async Task<List<Exercise>> GetExercisesAsync()
        {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            string spName = "GetExercises";

            List<Exercise> exercises = new List<Exercise>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Exercise exercise = new Exercise
                                {
                                    ExerciseId = reader.GetInt32("ExerciseId"),
                                    ExerciseName = reader.GetString("ExerciseName"),
                                    Duration = reader.GetInt32("Duration"),
                                    CreatedAt = reader.GetDateTime("CreatedAt"),
                                    LastUpdatedAt = reader.GetDateTime("LastUpdatedAt"),
                                    CreatedBy = reader.GetString("CreatedBy"),
                                    LastUpdatedBy = reader.GetString("LastUpdatedBy"),
                                    WorkoutId = reader.GetInt32("WorkoutId")
                                };
                                exercises.Add(exercise);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.Log($"An error occurred while getting exercises: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while getting exercises: {ex.Message}");
                throw;
            }
            return exercises;
        }


        public async Task<Exercise?> GetExercisesByExerciseIdAsync(int exerciseId) {
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");
            string spName = "GetExercisesByExerciseId";

            try{
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExerciseId", exerciseId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Exercise exercise = new Exercise{
                                    ExerciseName = reader.GetString("ExerciseName"),
                                    Duration = reader.GetInt32("Duration"),
                                    CreatedAt = reader.GetDateTime("CreatedAt"),
                                    LastUpdatedAt = reader.GetDateTime("LastUpdatedAt"),
                                    CreatedBy = reader.GetString("CreatedBy"),
                                    LastUpdatedBy = reader.GetString("LastUpdatedBy"),
                                    ExerciseId = reader.GetInt32("ExerciseId"),
                                    WorkoutId = reader.GetInt32("WorkoutId")
                                };

                                return exercise;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.Log($"An error occurred while getting exercise with id {exerciseId}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while getting exercise with id {exerciseId}: {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<Exercise> AddExerciseAsync(string createdBy, Exercise exercise)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string spName = "AddExercise";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ExerciseName", exercise.ExerciseName);
                        command.Parameters.AddWithValue("@CreatedBy", createdBy);
                        command.Parameters.AddWithValue("@Duration", exercise.Duration);
                        command.Parameters.AddWithValue("@WorkoutId", exercise.WorkoutId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.Log($"An error occurred while adding exercise: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while adding exercise: {ex.Message}");
                throw;
            }
            return exercise; 
        }

        public async Task<Exercise> UpdateExerciseAsync(int exerciseId, string updatedBy, Exercise exercise)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_UpdateExercise", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@exerciseId", exerciseId);
                        command.Parameters.AddWithValue("@updatedBy", updatedBy);
                        command.Parameters.AddWithValue("@exerciseName", exercise.ExerciseName);
                        command.Parameters.AddWithValue("@duration", exercise.Duration);

                        await command.ExecuteNonQueryAsync();

                        return exercise;
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.Log($"An error occurred while updating exercise with id {exerciseId}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An error occurred while updating exercise with id {exerciseId}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteExerciseAsync(int exerciseId)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_DeleteExercise", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@exerciseId", exerciseId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.Log($"An error occurred while deleting exercise with id {exerciseId}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.Log($"An unexpected error occurred while deleting exercise with id {exerciseId}: {ex.Message}");
                throw;
            }
        }

            // public List<Exercise> GetExercises() {
        //     try
        //     {
        //         _logger.Log("Getting exercises");
        //         return _context.Exercises.ToList();
        //     }
        //     catch (DbException ex)
        //     {
        //         _logger.Log($"An error occurred while getting exercises: {ex.Message}");
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.Log($"An unexpected error occurred while getting exercises: {ex.Message}");
        //         throw;
        //     }
        // }
        
        // public Exercise GetExercisesByExerciseId(int exerciseId) {
        //     try
        //     {
        //         var _exercise = _context.Exercises.Find(exerciseId);
        //         if(_exercise != null) {
        //             return _exercise;
        //         }else {
        //             _logger.Log($"Exercise with id {exerciseId} not found");
        //             throw new InvalidOperationException($"Exercise with id {exerciseId} not found");
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.Log($"An error occurred while getting exercise with id {exerciseId}: {ex.Message}");
        //         throw;
        //     }
        // }

        // public Exercise AddExercise(string createdBy, Exercise exercise) {
        //     try
        //     {
        //         var _exercise = new Exercise()
        //         {
        //             ExerciseName = exercise.ExerciseName,
        //             CreatedBy = createdBy,
        //             CreatedAt = DateTime.Now,
        //             LastUpdatedBy = createdBy,
        //             Duration = exercise.Duration,
        //             LastUpdatedAt = DateTime.Now,
        //             WorkoutId = exercise.WorkoutId
        //         };
        //         _context.Exercises.Add(_exercise);
        //         _context.SaveChanges();
        //         return _exercise;
        //     }
        //     catch (DbUpdateException ex)
        //     {
        //         // Veritabanı güncelleme hatası
        //         _logger.LogError(ex, "Updating database error");
        //         throw;
        //     }
        //     catch (ValidationException ex)
        //     {
        //         // Veri doğrulama hatası
        //         _logger.LogError(ex, "Validation error");
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         // Diğer hatalar
        //         _logger.LogError(ex, "Error occurred");
        //         throw;
        //     }
        // }

        // public Exercise UpdateExercise(int exerciseId, string updatedBy, Exercise exercise)
        // {
        //     try
        //     {
        //         var _exercise = _context.Exercises.Find(exerciseId);
        //         if(_exercise != null) {
        //             _exercise.ExerciseName = exercise.ExerciseName;
        //             _exercise.CreatedBy = _exercise.CreatedBy;
        //             _exercise.LastUpdatedBy = updatedBy;
        //             _exercise.Duration = exercise.Duration;
        //             _exercise.LastUpdatedAt = DateTime.Now;
        //             _context.SaveChanges();
        //         }else {
        //             _logger.Log($"Exercise with id {exerciseId} not found");
        //             throw new InvalidOperationException($"Exercise with id {exerciseId} not found");
        //         }
        //         return _exercise;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.Log($"An error occurred while updating exercise with id {exerciseId}: {ex.Message}");
        //         throw;
        //     }
        // }

        // public void DeleteExercise(int exerciseId)
        // {
        //     try
        //     {
        //         var _exercise = _context.Exercises.Find(exerciseId);
        //         if(_exercise != null) {
        //             _context.Exercises.Remove(_exercise);
        //             _context.SaveChanges();
        //         }else {
        //              _logger.Log($"Exercise with id {exerciseId} not found");
        //             throw new InvalidOperationException($"Exercise with id {exerciseId} not found");
        //         }
        //     }
        //     catch (DbUpdateException ex)
        //     {
        //         _logger.Log($"An error occurred while deleting exercise with id {exerciseId}: {ex.Message}");
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.Log($"An unexpected error occurred while deleting exercise with id {exerciseId}: {ex.Message}");
        //         throw;
        //     }
        // }
    }
}