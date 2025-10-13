using PdfDownloader.Models;
using System.Collections.Generic;

namespace PdfDownloader.Interfaces
{
    public interface IMetadataReader
    {
        List<ReportMetadata> Read(string path);
    }
}
