using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.Services;

namespace Ges_sco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var grades = await _gradeService.GetAllAsync();
            return Ok(grades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var grade = await _gradeService.GetByIdAsync(id);
            if (grade == null) return NotFound(new { message = "Grade not found" });
            return Ok(grade);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var grades = await _gradeService.GetByStudentIdAsync(studentId);
            return Ok(grades);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetByCourseId(int courseId)
        {
            var grades = await _gradeService.GetByCourseIdAsync(courseId);
            return Ok(grades);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateGradeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var grade = await _gradeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = grade.Id }, grade);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGradeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var grade = await _gradeService.UpdateAsync(id, dto);
            if (grade == null) return NotFound(new { message = "Grade not found" });
            return Ok(grade);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _gradeService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Grade not found" });
            return NoContent();
        }

        [HttpGet("student/{studentId}/average")]
        public async Task<IActionResult> GetStudentAverage(int studentId)
        {
            var average = await _gradeService.CalculateStudentAverageAsync(studentId);
            return Ok(new { studentId, average });
        }

        [HttpGet("course/{courseId}/average")]
        public async Task<IActionResult> GetCourseAverage(int courseId)
        {
            var average = await _gradeService.CalculateCourseAverageAsync(courseId);
            return Ok(new { courseId, average });
        }
    }
}
