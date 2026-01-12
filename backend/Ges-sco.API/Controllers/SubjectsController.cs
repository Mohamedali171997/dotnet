using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.Services;

namespace Ges_sco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null) return NotFound(new { message = "Subject not found" });
            return Ok(subject);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CreateSubjectDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var subject = await _subjectService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = subject.Id }, subject);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSubjectDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var subject = await _subjectService.UpdateAsync(id, dto);
            if (subject == null) return NotFound(new { message = "Subject not found" });
            return Ok(subject);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subjectService.DeleteAsync(id);
            if (!result) return NotFound(new { message = "Subject not found" });
            return NoContent();
        }
    }
}
