using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using caseStudy.Data.Models;
using caseStudy.Services;
using Microsoft.AspNetCore.Mvc;

namespace caseStudy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutController : ControllerBase
    {
        public WorkoutsService _workoutsService;
        public WorkoutController(WorkoutsService workoutsService)
        {
            _workoutsService = workoutsService;
        }

        [HttpGet("GetWorkoutsWithoutExercises")]
        public IActionResult GetWorkouts()
        {
            try{
                var workouts = _workoutsService.GetWorkoutsWithoutExercies();
                return Ok(workouts);
            }
            catch (Exception ex){
                return StatusCode(500, "Error while getting workouts:" + ex.Message);
            }
            
        }

        [HttpGet("GetWorkoutsWithExercises")]
        public IActionResult GetWorkoutsWithExercises()
        {
            try{
                var workouts = _workoutsService.GetWorkoutsWithExercises();
                return Ok(workouts);
            }
            catch (Exception ex){
                return StatusCode(500, "Error while getting workouts:" + ex.Message);
            }
        }

        [HttpPost("AddWorkout/{createdBy}")]
        public IActionResult AddWorkout(string createdBy,[FromBody]Workout workout)
        {
            try 
            {
                var addedWorkout = _workoutsService.AddWorkout(createdBy, workout);
                return Ok(addedWorkout);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while adding workout:" + ex.Message);
            }
        }        

        [HttpGet("GetWorkoutWithId/{workoutId}")]
        public IActionResult GetWorkout(int workoutId)
        {
            try{
                var workout = _workoutsService.GetWorkout(workoutId);
                return Ok(workout);
            }
            catch (InvalidOperationException ex){
                return NotFound(ex.Message);
            }
            catch (Exception ex){
                return StatusCode(500, "Error while getting workout:" + ex.Message);
            }
        }

        [HttpPut("UpdateWorkout/{workoutId}/{updatedBy}")]
        public IActionResult UpdateWorkout(int workoutId,string updatedBy,[FromBody] Workout workout)
        {
            try
            {
                var updatedWorkout = _workoutsService.UpdateWorkout(workoutId, updatedBy, workout);
                return Ok(updatedWorkout);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while updating workout:" + ex.Message);
            }
        }

        [HttpDelete("DeleteWorkout/{workoutId}")]
        public IActionResult DeleteWorkout(int workoutId)
        {
            try{
                _workoutsService.DeleteWorkout(workoutId);
                return Ok("Deleted successfully");
            }
            catch (InvalidOperationException ex){
                return NotFound(ex.Message);
            }
            catch (Exception ex){
                return StatusCode(500, "Error while deleting workout:" + ex.Message);
            }
        }

        [HttpGet("FilterWorkoutsWithAnd")]
        public IActionResult FilterWorkoutsWithAnd([FromQuery] string difficultyLevel, [FromQuery] string focusArea, [FromQuery] int totalTime)
        {
            try
            {
                var filteredWorkouts = _workoutsService.FilterWorkoutsWithAnd(difficultyLevel, focusArea, totalTime);
                return Ok(filteredWorkouts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while filtering workouts:" + ex.Message);
            }
        }

        [HttpGet("FilterWorkoutsWithOr")]
        public IActionResult FilterWorkoutsWithOr([FromQuery] string difficultyLevel, [FromQuery] string focusArea, [FromQuery] int totalTime)
        {
            try
            {
                var filteredWorkouts = _workoutsService.FilterWorkoutsWithOr(difficultyLevel, focusArea, totalTime);
                return Ok(filteredWorkouts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error while filtering workouts:" + ex.Message);
            }
        }
    }
}