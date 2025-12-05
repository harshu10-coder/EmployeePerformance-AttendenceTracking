using EmployeePerformance_AttendenceTracking.DTOs;
using EmployeePerformance_AttendenceTracking.Models;
using EmployeePerformance_AttendenceTracking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeePerformance_AttendenceTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Employee")]
    public class AttendanceController : ControllerBase
    {
        private readonly IRepository<Attendance> _repo;
        private readonly UserManager<ApplicationUser> _userRepo;
        public AttendanceController(IRepository<Attendance> repo, UserManager<ApplicationUser> userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync()
        {
            var data=await _repo.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Id Not Found");
            var user = await _userRepo.Users.FirstOrDefaultAsync(u => u.EmployeeId == id);
            var dto = new AttendenceDto
            {
                AttendanceId=data.AttendanceId,
                EmployeeId=data.EmployeeId,
                CheckIn=data.CheckIn,
                CheckOut=data.CheckOut,
                Status=data.Status,
                FullName=user.FullName,
                TotalHours=data.TotalHours,
                IsLate=data.IsLate
            };
            return Ok(dto);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AttendenceDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not Valid");
       
        
            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                Date = dto.Date,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,              
                TotalHours = dto.TotalHours,
                IsLate = dto.IsLate,
                Status = dto.Status
            };
            var create = await _repo.AddAsync(attendance);
            return Ok(attendance);
            
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update([FromBody] AttendenceDto dto,int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return BadRequest("Data not found");

            data.Date = dto.Date;
            data.CheckIn = dto.CheckIn;
            data.CheckOut = dto.CheckOut;
            data.TotalHours = dto.TotalHours;
            data.IsLate = dto.IsLate;
            data.Status = dto.Status;

            await _repo.UpdateAsync(data);
            return Ok(data);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Data Not Found");
            await _repo.DeleteAsync(data);
            return Ok(data);
        }
    }
}
