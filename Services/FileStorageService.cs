namespace ProductCrud.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        private string GetWebRootPath()
        {
            if (!string.IsNullOrWhiteSpace(_environment.WebRootPath))
                return _environment.WebRootPath;

            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        private string GetFolderPath(string folderName)
        {
            var folderPath = Path.Combine(GetWebRootPath(), "uploads", folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }

        private static string GenerateFileName(string originalFileName)
        {
            return Guid.NewGuid().ToString("N") + Path.GetExtension(originalFileName);
        }

        public async Task<string?> SingleFileUploadAsync(IFormFile? file, string folderName)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = GenerateFileName(file.FileName);
            var folderPath = GetFolderPath(folderName);
            var filePath = Path.Combine(folderPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }

        public async Task<List<string>> MultipleFileUploadAsync(IEnumerable<IFormFile>? files, string folderName)
        {
            var uploadedFiles = new List<string>();

            if (files == null)
                return uploadedFiles;

            foreach (var file in files)
            {
                var savedFileName = await SingleFileUploadAsync(file, folderName);
                if (!string.IsNullOrWhiteSpace(savedFileName))
                {
                    uploadedFiles.Add(savedFileName);
                }
            }

            return uploadedFiles;
        }

        public string? GetSingleFileUrl(string? fileName, HttpRequest request, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            return $"{request.Scheme}://{request.Host}/uploads/{folderName}/{fileName}";
        }

        public List<string> GetMultipleFileUrls(IEnumerable<string>? fileNames, HttpRequest request, string folderName)
        {
            if (fileNames == null)
                return new List<string>();

            return fileNames
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => $"{request.Scheme}://{request.Host}/uploads/{folderName}/{x}")
                .ToList();
        }

        public Task DeleteSingleFileAsync(string? fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return Task.CompletedTask;

            var folderPath = GetFolderPath(folderName);
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        public async Task DeleteMultipleFilesAsync(IEnumerable<string>? fileNames, string folderName)
        {
            if (fileNames == null)
                return;

            foreach (var fileName in fileNames)
            {
                await DeleteSingleFileAsync(fileName, folderName);
            }
        }
    }
}
