using PdfDownloader.Models;
using PdfDownloader.Readers;
using PdfDownloader.Services;
using PdfDownloader.Interfaces;
using PdfDownloader.Utils;

class Program
{
    static async Task Main(string[] args)
    {
        string inputFile = args.Length > 0 ? args[0] : "GRI_2017_2020.xlsx";
        string outputDir = args.Length > 1 ? args[1] : "Downloads";
        int maxConcurrency = args.Length > 2 ? int.Parse(args[2]) : 10;

        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Filen '{inputFile}' blev ikke fundet.");
            return;
        }

        // Vælg reader afhængigt af filtype
        IMetadataReader reader = inputFile.EndsWith(".xlsx")
            ? new ExcelMetadataReader()
            : new CsvMetadataReader();

        var reports = reader.Read(inputFile);

        // Start download med max 10 samtidige downloads
        var manager = new DownloadManager(outputDir, maxConcurrency);
        await manager.StartDownloadsAsync(reports);

        // Gem CSV
        var statusWriter = new StatusWriter();
        string csvPath = Path.Combine(outputDir, "status.csv");
        statusWriter.Write(reports, csvPath);
        Console.WriteLine($"Status CSV gemmes i: {csvPath}");

        // Gem Excel
        var excelWriter = new ExcelStatusWriter();
        string excelOutputPath = Path.Combine(outputDir, "GRI_2017_2020_Status.xlsx");
        excelWriter.WriteStatusToExcel(reports, inputFile, excelOutputPath);
        Console.WriteLine($"Excel status gemmes i: {excelOutputPath}");

        Console.WriteLine("Downloadforsøg færdig");
    }
}