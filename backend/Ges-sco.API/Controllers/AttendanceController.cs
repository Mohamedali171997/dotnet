using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.Services;

namespace Ges_sco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null) return NotFound(new { message = "Attendance not found" });
            return Ok(attendance);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(int studentId)
        {
            var attendances = await _attendanceService.GetByStudentIdAsync(studentId);
            return Ok(attendances);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetByCourseId(int courseId)
        {
            var attendances = await _attendanceService.GetByCourseIdAsync(courseId);
            return Ok(attendances);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateAttendanceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var attendance = await _attendanceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = attendance.Id }, attendance);
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
        public async Task<IActionResult> CreateBulk([FromBody] BulkAttendanceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var attendances = await _attendanceService.CreateBulkAsync(dto);
            return Ok(attendances);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var attendance = await _attendanceService.UpdateAsync(id, dto);
            if (attendance == null) return NotFound(new { message = "Attendance not found" });
            return Ok(attendance);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attendanceService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Attendance not found" });
            return NoContent();
        }

        [HttpGet("student/{studentId}/rate")]
        public async Task<IActionResult> GetStudentAttendanceRate(int studentId)
        {
            var rate = await _attendanceService.CalculateStudentAttendanceRateAsync(studentId);
            return Ok(new { studentId, attendanceRate = rate });
        }

        [HttpGet("course/{courseId}/rate")]
        public async Task<IActionResult> GetCourseAttendanceRate(int courseId)
        {
            var rate = await _attendanceService.CalculateCourseAttendanceRateAsync(courseId);
            return Ok(new { courseId, attendanceRate = rate });
        }
    }
}
