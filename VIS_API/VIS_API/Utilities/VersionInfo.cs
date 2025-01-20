using System.Diagnostics;

namespace VIS_API.Utilities
{
    public static class VersionInfo
    {
        private static readonly FileVersionInfo _vi = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string? Title => _vi.FileDescription;
        public static string? Product => _vi.ProductName;
        public static string? Copyright => _vi.LegalCopyright;
#if DEBUG
        public static string? Version => $"{_vi.FileVersion}.{DateTime.Now:yyMMddHHmmss}";
#else
    public static string? Version => _vi.FileVersion;
#endif
    }
}

