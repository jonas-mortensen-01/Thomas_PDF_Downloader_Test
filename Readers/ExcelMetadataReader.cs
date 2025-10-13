using ClosedXML.Excel;
using PdfDownloader.Models;
using PdfDownloader.Interfaces;

namespace PdfDownloader.Readers
{
    // Reader der indlæser metadata fra Excel-fil
    public class ExcelMetadataReader : IMetadataReader
    {
        public List<ReportMetadata> Read(string filePath)
        {
            var reports = new List<ReportMetadata>();
            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheets.First();
            var headerRow = worksheet.Row(1);

            // Find kolonner
            int colBRnum = FindColumn(headerRow, "BRnum");
            int colPdfUrl = FindColumn(headerRow, "Pdf_URL");
            int colAltPdfUrl = FindColumn(headerRow, "Report Html Address");

            // Læs alle rækker og gem som ReportMetadata
            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var report = new ReportMetadata
                {
                    BRnum = row.Cell(colBRnum).GetString().Trim(),
                    Pdf_URL = row.Cell(colPdfUrl).GetString().Trim(),
                    AltPdf_URL = colAltPdfUrl != -1 ? row.Cell(colAltPdfUrl).GetString().Trim() : "",
                    Status = "Ikke downloadet"
                };

                if (!string.IsNullOrEmpty(report.BRnum))
                    reports.Add(report);
            }

            return reports;
        }

        // Hjælpefunktion til at finde kolonnenummer på baggrund af header navn
        private int FindColumn(IXLRow headerRow, string headerName)
        {
            foreach (var cell in headerRow.CellsUsed())
            {
                if (cell.GetString().Trim().Equals(headerName, StringComparison.OrdinalIgnoreCase))
                    return cell.Address.ColumnNumber;
            }
            return -1;
        }
    }
}