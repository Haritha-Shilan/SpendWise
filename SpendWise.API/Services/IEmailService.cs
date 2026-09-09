namespace SpendWise.API.Services
{
    public interface IEmailService
    {
       public Task SendWelcomeEmailAsync(string email, string fullName);
    }
}