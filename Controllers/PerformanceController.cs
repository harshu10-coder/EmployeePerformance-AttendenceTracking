using EmployeePerformance_AttendenceTracking.DTOs;
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
    [Authorize(Roles="Manager,Admin")]
    public class PerformanceController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IRepository<Performance> _repo;
        public PerformanceController(UserManager<ApplicationUser> usermanager, IRepository<Performance> repo)
        {
            _usermanager = usermanager;
            _repo = repo;
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
                return NotFound("Not found");

            return Ok(data); 

        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Performance model)
        {
            if (!ModelState.IsValid)
                return NotFound("Not Found");

            var data = await _usermanager.Users.FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId && x.FullName == model.Name);
            if (data == null)
                return NotFound("Employee Not Found");

            var user = new Performance
            {
                EmployeeId = data.EmployeeId,
                Name = data.FullName,
                Id = model.Id,
                Month = model.Month,
                Year = model.Year,
                Score = model.Score,
                Rating = model.Rating,
                Remarks = model.Remarks
            };

            var created = await _repo.AddAsync(user);
            return Ok(created);
        }

        [HttpPut("Update{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Performance model)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Not found");

            var user = await _usermanager.Users.FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId && x.FullName == model.Name);

            data.EmployeeId = user.EmployeeId;
            data.Name = user.FullName;
            data.Id = model.Id;
            data.Month = model.Month;
            data.Year = model.Year;
            data.Score = model.Score;
            data.Rating = model.Rating;
            data.Remarks = model.Remarks;

            await _repo.UpdateAsync(data);
            return Ok("Performance Updated Successfully");
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return NotFound("Not Found");
            await _repo.DeleteAsync(data);
            return Ok("Delete Performance Successfully");


        }
        
        

    }
}
