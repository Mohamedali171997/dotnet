using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.Services;

namespace Ges_sco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound(new { message = "Student not found" });
            return Ok(student);
        }

        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClassId(int classId)
        {
            var students = await _studentService.GetByClassIdAsync(classId);
            return Ok(students);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var student = await _studentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var student = await _studentService.UpdateAsync(id, dto);
            if (student == null) return NotFound(new { message = "Student not found" });
            return Ok(student);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Student not found" });
            return NoContent();
        }

        [HttpGet("{id}/grades")]
        public async Task<IActionResult> GetGradesReport(int id)
        {
            var report = await _studentService.GetGradesReportAsync(id);
            if (report == null) return NotFound(new { message = "Student not found" });
            return Ok(report);
        }

        [HttpGet("{id}/attendance")]
        public async Task<IActionResult> GetAttendanceReport(int id)
        {
            var report = await _studentService.GetAttendanceReportAsync(id);
            if (report == null) return NotFound(new { message = "Student not found" });
            return Ok(report);
        }
    }
}
