namespace WebApplication1.Repository.Interface
{
    public interface IMyEmailSender
    {
        Task<bool> EmailSendAsync(string email, string Subject, string message);
    }
}
