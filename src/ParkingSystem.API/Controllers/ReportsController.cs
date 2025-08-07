using Microsoft.AspNetCore.Mvc;
using ParkingSystem.API.Services;
using System;
using System.Threading.Tasks;

namespace ParkingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily/{date}")]
        public async Task<IActionResult> GetDailyReport(string date)
        {
            if (!DateOnly.TryParse(date, out var dateOnly))
            {
                return BadRequest("Formato de data inválido. Use yyyy-MM-dd.");
            }

            var report = await _reportService.GenerateDailyReportAsync(dateOnly);
            return Ok(report);
        }
    }
}