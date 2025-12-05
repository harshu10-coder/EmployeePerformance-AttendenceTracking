using EmployeePerformance_AttendenceTracking.Models;
using EmployeePerformance_AttendenceTracking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeePerformance_AttendenceTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin,Manager" )]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Department> _repo;
        private readonly IRepository<ApplicationUser> _userrepo;
        public DepartmentController(IRepository<Department> repo, IRepository<ApplicationUser> userrepo)
        {
            _repo = repo;
            _userrepo = userrepo;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            
            return Ok(data);
        }
        [HttpGet("GetBy/{id}")]       
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Data Not Found");
            var user = await _userrepo.GetByIdAsync(data.Id);

            return Ok(data);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Department model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not Found");

            var created = await _repo.AddAsync(model);
            return Ok(created);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, Department model)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound("Data Not Found");
            }
            data.Name = model.Name;

            await _repo.UpdateAsync(data);
            return Ok("Department Updated Successfully");
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Data Not Found");
            await _repo.UpdateAsync(data);
            return Ok("Delete Data Successfully");
        }
    }
}
