using CsvHelper;
using CsvHelper.Configuration;
using PdfDownloader.Models;
using System.Globalization;

namespace PdfDownloader.Utils
{
    // Skriver status til CSV
    public class StatusWriter
    {
        public void Write(List<ReportMetadata> reports, string path)
        {
            using var writer = new StreamWriter(path);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.WriteRecords(reports);
        }
    }
}