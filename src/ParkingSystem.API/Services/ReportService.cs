using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Core.DTOs.Reports;
using ParkingSystem.Infrastructure.Data;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;


namespace ParkingSystem.API.Services
{
    public class ReportService : IReportService
    {
        private readonly ParkingDbContext _context;

        public ReportService(ParkingDbContext context)
        {
            _context = context;
        }

        public async Task<DailyReportDto> GenerateDailyReportAsync(DateOnly date)
        {
            var startDate = date.ToDateTime(TimeOnly.MinValue);
            var endDate = startDate.AddDays(1);

            // Busca todos os veículos que saíram nesse dia.
            // A receita é contada na saída.
            var vehiclesExited = await _context.Vehicles
                .Where(v => v.ExitTime >= startDate && v.ExitTime < endDate)
                .ToListAsync();

            var report = new DailyReportDto
            {
                ReportDate = startDate,
                TotalVehiclesEntered = vehiclesExited.Count,
                TotalRevenue = vehiclesExited.Sum(v => v.TotalAmount),
                VehicleEntries = vehiclesExited.Select(v => new VehicleReportEntryDto
                {
                    LicensePlate = v.LicensePlate,
                    EntryTime = v.EntryTime,
                    ExitTime = v.ExitTime,
                    TotalAmount = v.TotalAmount
                }).ToList()
            };

            return report;
        }
        public async Task<byte[]> ExportDailyReportToPdfAsync(DailyReportDto report)
        {
            var document = new DailyReportPdfDocument(report);
            return await Task.FromResult(document.GeneratePdf());
        }

        public async Task<byte[]> ExportDailyReportToExcelAsync(DailyReportDto report)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Relatório Diário");

                // Cabeçalho
                worksheet.Cell("A1").Value = "Relatório Diário de Estacionamento";
                worksheet.Cell("A2").Value = $"Data: {report.ReportDate:dd/MM/yyyy}";
                worksheet.Cell("A1").Style.Font.Bold = true;
                worksheet.Cell("A1").Style.Font.FontSize = 16;
                worksheet.Range("A1:D1").Merge();

                // Sumário
                worksheet.Cell("A4").Value = "Total de Veículos:";
                worksheet.Cell("B4").Value = report.TotalVehiclesEntered;
                worksheet.Cell("A5").Value = "Receita Total:";
                worksheet.Cell("B5").Value = report.TotalRevenue;
                worksheet.Cell("B5").Style.NumberFormat.Format = "\"R$\" #,##0.00";

                // Tabela de Entradas
                worksheet.Cell("A7").Value = "Placa";
                worksheet.Cell("B7").Value = "Entrada";
                worksheet.Cell("C7").Value = "Saída";
                worksheet.Cell("D7").Value = "Valor Pago";
                var headerRange = worksheet.Range("A7:D7");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                var row = 8;
                foreach (var entry in report.VehicleEntries)
                {
                    worksheet.Cell(row, 1).Value = entry.LicensePlate;
                    worksheet.Cell(row, 2).Value = entry.EntryTime;
                    worksheet.Cell(row, 3).Value = entry.ExitTime;
                    worksheet.Cell(row, 4).Value = entry.TotalAmount;
                    worksheet.Cell(row, 4).Style.NumberFormat.Format = "\"R$\" #,##0.00";
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                // Salva o workbook num stream de memória e retorna como array de bytes
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return await Task.FromResult(stream.ToArray());
                }
            }
        }
    }
}
public class DailyReportPdfDocument : IDocument
{
    private readonly DailyReportDto _report;

    public DailyReportPdfDocument(DailyReportDto report)
    {
        _report = report;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                });
            });
    }

    void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Relatório Diário - {_report.ReportDate:dd/MM/yyyy}").SemiBold().FontSize(20);
                column.Item().Text($"Receita Total: {_report.TotalRevenue:C}").Bold();
                column.Item().Text($"Total de Veículos: {_report.TotalVehiclesEntered}");
            });
        });
    }

    void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(20);
            column.Item().Element(ComposeTable);
        });
    }

    void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn(2);
                columns.RelativeColumn(2);
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Text("Placa");
                header.Cell().Text("Entrada");
                header.Cell().Text("Saída");
                header.Cell().AlignRight().Text("Valor");
            });

            foreach (var item in _report.VehicleEntries)
            {
                table.Cell().Text(item.LicensePlate);
                table.Cell().Text($"{item.EntryTime:g}");
                table.Cell().Text($"{item.ExitTime:g}");
                table.Cell().AlignRight().Text($"{item.TotalAmount:C}");
            }
        });
    }
}