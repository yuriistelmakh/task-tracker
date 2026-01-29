namespace TaskTracker.Domain.Helpers;

using TaskTracker.Domain.Enums;

public static class FileTypeHelper
{
    public static FileType GetFileTypeFromExtension(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return FileType.Unknown;

        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            // Images
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".svg" or ".webp" or ".ico" => FileType.Image,

            // Documents
            ".pdf" or ".doc" or ".docx" or ".txt" or ".rtf" or ".odt" or ".pages" => FileType.Document,

            // Spreadsheets
            ".xls" or ".xlsx" or ".csv" or ".ods" or ".numbers" => FileType.Spreadsheet,

            // Video
            ".mp4" or ".avi" or ".mov" or ".wmv" or ".flv" or ".mkv" or ".webm" or ".m4v" => FileType.Video,

            // Audio
            ".mp3" or ".wav" or ".flac" or ".aac" or ".ogg" or ".wma" or ".m4a" => FileType.Audio,

            // Unknown
            _ => FileType.Unknown
        };
    }

    public static string FormatFileSize(double sizeInKB)
    {
        const double KB = 1024;
        const double MB = KB * 1024;

        if (sizeInKB >= MB)
        {
            return $"{sizeInKB / MB:F2} MB";
        }
        else if (sizeInKB >= 1)
        {
            return $"{sizeInKB:F2} KB";
        }
        else
        {
            return $"{sizeInKB * 1024:F0} B";
        }
    }
}
