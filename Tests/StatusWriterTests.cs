using PdfDownloader.Models;
using PdfDownloader.Utils;
using Xunit;

public class StatusWriterTests
{
    private string CreateTempPath()
    {
        string path = Path.Combine(Path.GetTempPath(), $"status_{Guid.NewGuid()}.csv");
        return path;
    }

    // Creates a list of two ReportMetadata objects.
    // Writes to a temporary file.
    // Asserts that the file exists
    // and that the content contains expected headers and values.
    [Fact]
    public void Write_ValidReports_CreatesCsvWithCorrectData()
    {
        var reports = new List<ReportMetadata>
        {
            new() { BRnum = "001", Pdf_URL = "http://example.com/1.pdf", AltPdf_URL = "http://alt1.pdf", Status = "Downloadet" },
            new() { BRnum = "002", Pdf_URL = "http://example.com/2.pdf", AltPdf_URL = "http://alt2.pdf", Status = "Ikke downloadet" }
        };

        string path = CreateTempPath();
        var writer = new StatusWriter();

        writer.Write(reports, path);

        Assert.True(File.Exists(path));

        string content = File.ReadAllText(path);
        Assert.Contains("BRnum", content);
        Assert.Contains("001", content);
        Assert.Contains("Downloadet", content);
        Assert.Contains("002", content);
        Assert.Contains("Ikke downloadet", content);
    }

    // Writes an initial report with status "Old".
    // Writes again with a new list containing "New".
    // Asserts that the "Old" content is gone
    // and "New" content is present.
    // ensures overwriting, not appending.
    [Fact]
    public void Write_ExistingFile_OverwritesContent()
    {
        var initialReports = new List<ReportMetadata>
        {
            new() { BRnum = "001", Pdf_URL = "url1", Status = "Old" }
        };
        var newReports = new List<ReportMetadata>
        {
            new() { BRnum = "002", Pdf_URL = "url2", Status = "New" }
        };

        string path = CreateTempPath();
        var writer = new StatusWriter();

        writer.Write(initialReports, path);
        string oldContent = File.ReadAllText(path);
        Assert.Contains("Old", oldContent);

        writer.Write(newReports, path);
        string newContent = File.ReadAllText(path);

        Assert.DoesNotContain("Old", newContent);
        Assert.Contains("New", newContent);
        Assert.Contains("002", newContent);
    }

    // Writes an empty list.
    // Ensures file still exists
    // and it contains headers (BRnum, etc.).
    [Fact]
    public void Write_EmptyList_CreatesHeaderOnly()
    {
        var emptyReports = new List<ReportMetadata>();
        string path = CreateTempPath();
        var writer = new StatusWriter();

        writer.Write(emptyReports, path);

        Assert.True(File.Exists(path));
        string content = File.ReadAllText(path);

        // Should contain headers but no data rows
        Assert.Contains("BRnum", content);
        Assert.DoesNotContain("Downloadet", content);
        Assert.DoesNotContain("Ikke downloadet", content);
    }
}
