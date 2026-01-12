using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.Services;

namespace Ges_sco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classes = await _classService.GetAllAsync();
            return Ok(classes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var classEntity = await _classService.GetByIdAsync(id);
            if (classEntity == null) return NotFound(new { message = "Class not found" });
            return Ok(classEntity);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateClassDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var classEntity = await _classService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = classEntity.Id }, classEntity);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClassDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var classEntity = await _classService.UpdateAsync(id, dto);
            if (classEntity == null) return NotFound(new { message = "Class not found" });
            return Ok(classEntity);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _classService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Class not found" });
            return NoContent();
        }

        [HttpGet("{id}/students")]
        public async Task<IActionResult> GetClassStudents(int id)
        {
            var students = await _classService.GetClassStudentsAsync(id);
            return Ok(students);
        }

        [HttpGet("{id}/courses")]
        public async Task<IActionResult> GetClassCourses(int id)
        {
            var courses = await _classService.GetClassCoursesAsync(id);
            return Ok(courses);
        }
    }
}
