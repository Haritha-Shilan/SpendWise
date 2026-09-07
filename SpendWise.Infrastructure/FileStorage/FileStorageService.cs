namespace SpendWise.Infrastructure.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;
        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment=environment;
        }

        public Task DeleteAsync(string filePath)
        {
            var fullPath= Path.Combine(_environment.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveAsync(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads", "transactions");

            Directory.CreateDirectory(uploads);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

            var fullPath= Path.Combine(uploads, uniqueFileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine(
                "uploads",
                "transactions",
                uniqueFileName); ;
        }
    }
}
