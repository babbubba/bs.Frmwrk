using bs.Frmwrk.Core.Models.Configuration;

namespace bs.Frmwrk.Application.Models.Configuration
{
    public class FileSystemSettingsModel : IFileSystemSettings
    {
        public int? MemoryBufferThresholdMb { get; set; }
        public int? MultipartBodyLengthLimitMb { get; set; }
        public string RootPath { get; set; }
        public int? ValueLengthLimitMb { get; set; }
    }
}