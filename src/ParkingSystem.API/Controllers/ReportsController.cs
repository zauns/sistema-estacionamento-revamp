using Microsoft.AspNetCore.Mvc;
using ParkingSystem.API.Services;


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
        [HttpGet("daily/{date}/export/pdf")]
        public async Task<IActionResult> ExportDailyReportToPdf(string date)
        {
            if (!DateOnly.TryParse(date, out var dateOnly))
            {
                return BadRequest("Formato de data inválido. Use yyyy-MM-dd.");
            }

            var report = await _reportService.GenerateDailyReportAsync(dateOnly);
            if (report == null)
            {
                return NotFound("Nenhum dado encontrado para esta data.");
            }
            
            var pdfBytes = await _reportService.ExportDailyReportToPdfAsync(report);
            
            return File(pdfBytes, "application/pdf", $"Relatorio_{date}.pdf");
        }

        [HttpGet("daily/{date}/export/excel")]
        public async Task<IActionResult> ExportDailyReportToExcel(string date)
        {
            if (!DateOnly.TryParse(date, out var dateOnly))
            {
                return BadRequest("Formato de data inválido. Use yyyy-MM-dd.");
            }
            
            var report = await _reportService.GenerateDailyReportAsync(dateOnly);
            if (report == null)
            {
                return NotFound("Nenhum dado encontrado para esta data.");
            }

            var excelBytes = await _reportService.ExportDailyReportToExcelAsync(report);
            
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(excelBytes, contentType, $"Relatorio_{date}.xlsx");
        }
    }
}