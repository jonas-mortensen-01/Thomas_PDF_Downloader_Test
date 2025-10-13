using CsvHelper.Configuration.Attributes;

namespace PdfDownloader.Models
{
    // Repræsenterer metadata for en PDF-rapport
    public class ReportMetadata
    {
        // Unik rapportnummer
        public string BRnum { get; set; } = "";

        // Primær URL til PDF
        public string Pdf_URL { get; set; } = "";

        // Alternativ URL til PDF, hvis den primære fejler
        [Name("Report Html Address")]
        public string AltPdf_URL { get; set; } = "";

        // Status for download: "Downloadet", "Ikke downloadet" eller "Allerede downloadet"
        public string Status { get; set; } = "Ikke downloadet";
    }
}