using EmployeePerformance_AttendenceTracking.DTOs;
using EmployeePerformance_AttendenceTracking.Models;
using EmployeePerformance_AttendenceTracking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Extensions;
using Microsoft.VisualBasic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EmployeePerformance_AttendenceTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin,Manager")]
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Department> _dept;
        public EmployeeController(UserManager<ApplicationUser> userManager, IRepository<Department>dept)
        {
            _userManager = userManager;
            _dept = dept;
           
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _userManager.Users.Include(d=>d.Department).ToListAsync();


            var dtolist = data.Select(u => new EmployeeDto
            {
                EmployeeId=u.EmployeeId,
                FullName=u.FullName,
                DepartmentName=u.Department.Name,
                Email=u.Email,
                Salary = u.Salary,
                DepartmentId=u.DepartmentId,
                JoinDate = u.JoinDate
            }).ToList();
            return Ok(dtolist); 

        }

        [HttpGet("GetBy/{id}")]
        public async Task<IActionResult> GetById(int EmployeeId)
        {
            var user = await _userManager.Users.Include(d=>d.Department).FirstOrDefaultAsync(x =>x.EmployeeId==EmployeeId);
            if (user == null)
                return NotFound("Employee not found");
            var dto = new EmployeeDto
            { 
                FullName = user.FullName,
                Email = user.Email,
                DepartmentName = user.Department.Name,
                DepartmentId = user.DepartmentId,
                JoinDate = user.JoinDate,
                Salary = user.Salary
            };

            return Ok(dto);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] EmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var lastId = await _userManager.Users
    .OrderByDescending(u => u.EmployeeId)
    .Select(u => u.EmployeeId)
    .FirstOrDefaultAsync();
            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                UserName = dto.Email,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId,         
                Salary = dto.Salary,
                JoinDate = dto.JoinDate,
                EmployeeId = lastId + 1
            };
            var created = await _userManager.CreateAsync(user);
            
            return Ok(created);
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id,EmployeeDto model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (user == null)
                return NotFound("Employee Not Found");

            user.FullName = model.FullName;
            user.Salary = model.Salary;
            user.Email = model.Email;
            user.JoinDate = model.JoinDate;
            user.DepartmentId = model.DepartmentId;

            var update = await _userManager.UpdateAsync(user);

            if (!update.Succeeded)
                return BadRequest(update.Errors);

            return Ok("Employee Updated Successfully");
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int EmployeeId)
        {
            var data = await _userManager.Users.FirstOrDefaultAsync(x=>x.EmployeeId==EmployeeId);
            if (data == null)
                return NotFound("Data Not Found");

            var result = await _userManager.DeleteAsync(data);
            
            return Ok("Delete Successfully");
        }
    }
}
