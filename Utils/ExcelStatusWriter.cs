using ClosedXML.Excel;
using PdfDownloader.Models;

namespace PdfDownloader.Utils
{
    // Skriver status tilbage til Excel
    public class ExcelStatusWriter
    {
        public void WriteStatusToExcel(List<ReportMetadata> reports, string inputPath, string outputPath)
        {
            if (!File.Exists(inputPath)) return;

            using var workbook = new XLWorkbook(inputPath);
            var worksheet = workbook.Worksheets.First();

            var headerRow = worksheet.Row(1);

            // Find BRnum kolonne
            int brnumCol = headerRow.CellsUsed()
                                    .First(c => c.GetString().Equals("BRnum", StringComparison.OrdinalIgnoreCase))
                                    .Address.ColumnNumber;

            // Find Status kolonne, tilføj hvis den ikke findes
            int statusCol = headerRow.CellsUsed()
                                    .FirstOrDefault(c => c.GetString().Equals("Status", StringComparison.OrdinalIgnoreCase))
                                    ?.Address.ColumnNumber ?? headerRow.LastCellUsed().Address.ColumnNumber + 1;

            if (worksheet.Cell(1, statusCol).IsEmpty())
                worksheet.Cell(1, statusCol).Value = "Status";

            // Opdater hver række med status
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                string brnumValue = row.Cell(brnumCol).GetString().Trim();
                var report = reports.FirstOrDefault(r => r.BRnum == brnumValue);
                if (report != null)
                    row.Cell(statusCol).Value = report.Status;
            }

            workbook.SaveAs(outputPath);
        }
    }
}
