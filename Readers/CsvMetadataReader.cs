using CsvHelper;
using CsvHelper.Configuration;
using PdfDownloader.Models;
using PdfDownloader.Interfaces;
using System.Globalization;

namespace PdfDownloader.Readers
{
    // Reader der indlæser metadata fra CSV-fil
    public class CsvMetadataReader : IMetadataReader
    {
        public List<ReportMetadata> Read(string path)
        {
            // Åbner CSV-fil og læser alle rækker som ReportMetadata-objekter
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            return csv.GetRecords<ReportMetadata>().ToList();
        }
    }
}