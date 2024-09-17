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

        [Authorize]
        [HttpGet("GetExercises")]
        public IActionResult GetExercises()
        {
            try
            {
                var exercises = _exercisesService.GetExercises();
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while getting exercises:" + ex.Message);
            }
        }
        
        [Authorize]
        [HttpGet("GetExercisesByExerciseId/{exerciseId}")]
        public IActionResult GetExercisesByExerciseId(int exerciseId)
        {
            try
            {
                var exercises = _exercisesService.GetExercisesByExerciseId(exerciseId);
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while getting exercises:" + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("AddExercise/{createdBy}")]
        public IActionResult AddExercise(string createdBy, [FromBody] Exercise exercise)
        {
            try
            {
                var addedExercise = _exercisesService.AddExercise(createdBy, exercise);
                return Ok(addedExercise);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while adding exercise:" + ex.Message);
            }
        }

        [Authorize]
        [HttpPut("UpdateExercise/{exerciseId}/{updatedBy}")]
        public IActionResult UpdateExercise(int exerciseId, string updatedBy, [FromBody] Exercise exercise)
        {
            try
            {
                var updatedExercise = _exercisesService.UpdateExercise(exerciseId, updatedBy, exercise);
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

        [Authorize]
        [HttpDelete("DeleteExercise/{exerciseId}")]
        public IActionResult DeleteExercise(int exerciseId)
        {
            try
            {
                _exercisesService.DeleteExercise(exerciseId);
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