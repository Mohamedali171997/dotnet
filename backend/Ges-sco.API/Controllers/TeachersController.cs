using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.Services;

namespace Ges_sco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllAsync();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetByIdAsync(id);
            if (teacher == null) return NotFound(new { message = "Teacher not found" });
            return Ok(teacher);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var teacher = await _teacherService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, teacher);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTeacherDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var teacher = await _teacherService.UpdateAsync(id, dto);
            if (teacher == null) return NotFound(new { message = "Teacher not found" });
            return Ok(teacher);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _teacherService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Teacher not found" });
            return NoContent();
        }

        [HttpGet("{id}/courses")]
        public async Task<IActionResult> GetTeacherCourses(int id)
        {
            var courses = await _teacherService.GetTeacherCoursesAsync(id);
            return Ok(courses);
        }
    }
}
