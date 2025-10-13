using PdfDownloader.Models;

namespace PdfDownloader.Services
{
    // Styrer download af flere PDF'er med begrænset samtidighed
    public class DownloadManager
    {
        private readonly string _outputDir;
        private readonly int _maxConcurrency;
        private readonly PdfDownloader _downloader = new PdfDownloader();

        public DownloadManager(string outputDir, int maxConcurrency)
        {
            _outputDir = outputDir;
            _maxConcurrency = maxConcurrency;
        }

        public async Task StartDownloadsAsync(List<ReportMetadata> reports)
        {
            // Semaphore for at begrænse antal samtidige downloads
            using var semaphore = new SemaphoreSlim(_maxConcurrency);

            var tasks = reports.Select(async report =>
            {
                await semaphore.WaitAsync();
                try
                {
                    bool success = await _downloader.DownloadAsync(report, _outputDir);
                    Console.WriteLine($"{report.Status}: {report.BRnum}");
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }
    }
}