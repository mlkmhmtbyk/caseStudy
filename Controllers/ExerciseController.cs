using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using caseStudy.Data.Models;
using caseStudy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caseStudy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        public ExercisesService _exercisesService;
        public ExerciseController(ExercisesService exercisesService)
        {
            _exercisesService = exercisesService;
        }

        // [Authorize]
        // [HttpGet("GetExercises")]
        // public IActionResult GetExercises()
        // {
        //     try
        //     {
        //         var exercises = _exercisesService.GetExercises();
        //         return Ok(exercises);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, "Error while getting exercises:" + ex.Message);
        //     }
        // }
        [Authorize]
        [HttpGet("GetExercises")]
        public async Task<IActionResult> GetExercises()
        {
            try
            {
                var exercises = await _exercisesService.GetExercisesAsync();
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while getting exercises:" + ex.Message);
            }
        }

        // [Authorize]
        // [HttpGet("GetExercisesByExerciseId/{exerciseId}")]
        // public IActionResult GetExercisesByExerciseId(int exerciseId)
        // {
        //     try
        //     {
        //         var exercises = _exercisesService.GetExercisesByExerciseId(exerciseId);
        //         return Ok(exercises);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, "Error while getting exercises:" + ex.Message);
        //     }
        // }
        
        [Authorize]
        [HttpGet("GetExercisesByExerciseId/{exerciseId}")]
        public async Task<IActionResult> GetExercisesByExerciseId(int exerciseId)
        {
            try
            {
                var exercises = await _exercisesService.GetExercisesByExerciseIdAsync(exerciseId);
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while getting exercises:" + ex.Message);
            }
        }

        // [Authorize]
        // [HttpPost("AddExercise/{createdBy}")]
        // public IActionResult AddExercise(string createdBy, [FromBody] Exercise exercise)
        // {
        //     try
        //     {
        //         var addedExercise = _exercisesService.AddExercise(createdBy, exercise);
        //         return Ok(addedExercise);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, "Error while adding exercise:" + ex.Message);
        //     }
        // }

        [Authorize]
        [HttpPost("AddExercise/{createdBy}")]
        public async Task<IActionResult> AddExercise(string createdBy, [FromBody] Exercise exercise)
        {
            try
            {
                var addedExercise = await _exercisesService.AddExerciseAsync(createdBy, exercise);
                return Ok(addedExercise);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while adding exercise:" + ex.Message);
            }
        }

        // [Authorize]
        // [HttpPut("UpdateExercise/{exerciseId}/{updatedBy}")]
        // public IActionResult UpdateExercise(int exerciseId, string updatedBy, [FromBody] Exercise exercise)
        // {
        //     try
        //     {
        //         var updatedExercise = _exercisesService.UpdateExercise(exerciseId, updatedBy, exercise);
        //         return Ok(updatedExercise);
        //     }
        //     catch (InvalidOperationException ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, "Error while updating exercise:" + ex.Message);
        //     }
        // }

        [Authorize]
        [HttpPut("UpdateExercise/{exerciseId}/{updatedBy}")]
        public async Task<IActionResult> UpdateExercise(int exerciseId, string updatedBy, [FromBody] Exercise exercise)
        {
            try
            {
                var updatedExercise = await _exercisesService.UpdateExerciseAsync(exerciseId, updatedBy, exercise);
                return Ok(updatedExercise);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while updating exercise:" + ex.Message);
            }
        }

        // [Authorize]
        // [HttpDelete("DeleteExercise/{exerciseId}")]
        // public IActionResult DeleteExercise(int exerciseId)
        // {
        //     try
        //     {
        //         _exercisesService.DeleteExercise(exerciseId);
        //         return Ok("Exercise deleted successfully");
        //     }
        //     catch (InvalidOperationException ex)
        //     {
        //         return NotFound(ex.Message);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, "Error while deleting exercise:" + ex.Message);
        //     }
        // }

        [Authorize]
        [HttpDelete("DeleteExercise/{exerciseId}")]
        public async Task<IActionResult> DeleteExercise(int exerciseId)
        {
            try
            {
                await _exercisesService.DeleteExerciseAsync(exerciseId);
                return Ok("Exercise deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while deleting exercise:" + ex.Message);
            }
        }
    }
}