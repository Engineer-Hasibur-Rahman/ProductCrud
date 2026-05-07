namespace ProductCrud.Services
{
    public interface IFileStorageService
    {
        Task<string?> SingleFileUploadAsync(IFormFile? file, string folderName);
        Task<List<string>> MultipleFileUploadAsync(IEnumerable<IFormFile>? files, string folderName);

        string? GetSingleFileUrl(string? fileName, HttpRequest request, string folderName);
        List<string> GetMultipleFileUrls(IEnumerable<string>? fileNames, HttpRequest request, string folderName);

        Task DeleteSingleFileAsync(string? fileName, string folderName);
        Task DeleteMultipleFilesAsync(IEnumerable<string>? fileNames, string folderName);
    }
}
