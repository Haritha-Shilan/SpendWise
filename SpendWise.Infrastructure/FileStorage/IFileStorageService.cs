namespace SpendWise.Infrastructure.FileStorage
{
    public interface IFileStorageService
    {
        public Task<string> SaveAsync(IFormFile file);

        public Task DeleteAsync(string filePath);
    }
}
