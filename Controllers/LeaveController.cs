using EmployeePerformance_AttendenceTracking.Models;
using EmployeePerformance_AttendenceTracking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePerformance_AttendenceTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Employee")]
    public class LeaveController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userrepo;
        private readonly IRepository<LeaveRequest> _repo;

        public LeaveController(IRepository<LeaveRequest> repo, UserManager<ApplicationUser> userrepo)
        {
            _repo = repo;
            _userrepo = userrepo;
        }

        [HttpGet("GetAll")]
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
                return NotFound("Not Found");

            return Ok(data);

        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody]LeaveRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not Valid");
           
            var user = new LeaveRequest
            {
                EmployeeId = model.EmployeeId,
                EmployeeName=model.EmployeeName,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                Reason=model.Reason,
                Status = model.Status = "Pending"
            };

            var created = await _repo.AddAsync(user);
            return Ok(created);

        }
        [Authorize(Roles ="Admin,Manager")]
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Data not found");
            data.Status = "Approved";
            await _repo.UpdateAsync(data);

            return Ok("Leave Approved");
        }

        [Authorize("Admin/Manager")]
        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Data not found");

            data.Status = "Reject";
            await _repo.UpdateAsync(data);

            return Ok("Leave Rejected");
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LeaveRequest model)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Data not found");

            data.ToDate = model.ToDate;
            data.FromDate = model.FromDate;
            data.Reason = model.Reason;

            await _repo.UpdateAsync(data);
            return Ok("Leave Updated Successfully");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Not Found");

            await _repo.DeleteAsync(data);
            return Ok("Leave Deleted Successfully");
        }


    }
}
