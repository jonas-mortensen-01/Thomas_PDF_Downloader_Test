using PdfDownloader.Readers;
using Xunit;

public class CsvMetadataReaderTests
{
    private string CreateTempCsv(string content)
    {
        string path = Path.GetTempFileName();
        File.WriteAllText(path, content);
        return path;
    }


    // Creates a simple, valid CSV string with 2 rows.
    // Writes it to a temporary file.
    // Calls CsvMetadataReader.Read().
    // Asserts that it returns 2 reports.
    // The first report’s properties match what’s in the file.
    [Fact]
    public void Read_ValidCsv_ReturnsExpectedReports()
    {
        string csvContent = 
            "BRnum,Pdf_URL,Report Html Address,Status\n" +
            "123,http://example.com/pdf1.pdf,http://alt1.pdf,Ikke downloadet\n" +
            "456,http://example.com/pdf2.pdf,http://alt2.pdf,Ikke downloadet";

        string path = CreateTempCsv(csvContent);
        var reader = new CsvMetadataReader();

        var reports = reader.Read(path);

        Assert.Equal(2, reports.Count);
        Assert.Equal("123", reports[0].BRnum);
        Assert.Equal("http://example.com/pdf1.pdf", reports[0].Pdf_URL);
        Assert.Equal("http://alt1.pdf", reports[0].AltPdf_URL);
        Assert.Equal("Ikke downloadet", reports[0].Status);
    }

    // Tests that reading an empty CSV file produces an empty list instead of throwing.
    // CsvHelper returns an empty enumeration by default, so this should pass.
    [Fact]
    public void Read_EmptyFile_ReturnsEmptyList()
    {
        string path = CreateTempCsv("");
        var reader = new CsvMetadataReader();

        var reports = reader.Read(path);

        Assert.Empty(reports);
    }

    // Writes malformed CSV data (missing headers, wrong delimiters).
    // Confirms that CsvMetadataReader throws some kind of exception 
    // (HeaderValidationException or similar).
    [Fact]
    public void Read_InvalidCsv_ThrowsException()
    {
        string invalidCsv = "This;is;not,a,valid,csv";
        string path = CreateTempCsv(invalidCsv);
        var reader = new CsvMetadataReader();

        Assert.ThrowsAny<Exception>(() => reader.Read(path));
    }
}
