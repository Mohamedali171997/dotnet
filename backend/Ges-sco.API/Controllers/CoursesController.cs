using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.Services;

namespace Ges_sco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null) return NotFound(new { message = "Course not found" });
            return Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var course = await _courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var course = await _courseService.UpdateAsync(id, dto);
            if (course == null) return NotFound(new { message = "Course not found" });
            return Ok(course);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Course not found" });
            return NoContent();
        }

        [HttpGet("{id}/grades")]
        public async Task<IActionResult> GetCourseGrades(int id)
        {
            var grades = await _courseService.GetCourseGradesAsync(id);
            return Ok(grades);
        }

        [HttpGet("{id}/attendance")]
        public async Task<IActionResult> GetCourseAttendance(int id)
        {
            var attendance = await _courseService.GetCourseAttendanceAsync(id);
            return Ok(attendance);
        }
    }
}
