using PdfDownloader.Models;

namespace PdfDownloader.Services
{
    // Klasse der downloader PDF-filer
    public class PdfDownloader
    {
        // Deles af alle downloads, med timeout på 15 sekunder
        private static readonly HttpClient client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };

        // Downloader PDF fra primær eller alternativ URL
        public async Task<bool> DownloadAsync(ReportMetadata report, string outputDir)
        {
            string outputPath = Path.Combine(outputDir, $"{report.BRnum}.pdf");

            // Sørg for at output-mappen findes
            Directory.CreateDirectory(outputDir);

            // Hvis filen allerede findes, marker som "Allerede downloadet"
            if (File.Exists(outputPath))
            {
                report.Status = "Allerede downloadet";
                return true;
            }

            // Prøv først Pdf_URL, derefter AltPdf_URL
            foreach (var url in new[] { report.Pdf_URL, report.AltPdf_URL })
            {
                if (string.IsNullOrEmpty(url)) continue;

                try
                {
                    var data = await client.GetByteArrayAsync(url);
                    await File.WriteAllBytesAsync(outputPath, data);
                    report.Status = "Downloadet";
                    return true;
                }
                catch
                {
                    // Hvis fejler, prøv næste URL
                }
            }

            report.Status = "Ikke downloadet"; // Hvis ingen URL virker
            return false;
        }
    }
}
