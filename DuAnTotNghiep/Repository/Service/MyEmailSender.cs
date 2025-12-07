using System.Net;
using System.Net.Mail;
using WebApplication1.Repository.Interface;
using WebApplication1.ViewModels.Email;

namespace WebApplication1.Repository.Service
{
    public class MyEmailSender : IMyEmailSender
    {
        private readonly IConfiguration configuration;

        public MyEmailSender(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }
        public async Task<bool> EmailSendAsync(string email, string Subject, string message)
        {
            bool status = false;
            try
            {
                GetEmailSetting getEmailSetting = new GetEmailSetting()
                {
                    SecretKey = configuration.GetValue<string>("AppSettings:SecretKey"),
                    From = configuration.GetValue<string>("AppSettings:EmailSettings:From"),
                    SmtpServer = configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer"),
                    Port = configuration.GetValue<int>("AppSettings:EmailSettings:Port"),
                    EnableSSL = configuration.GetValue<bool>("AppSettings:EmailSettings:EnableSSL"),
                };
                MailMessage mailMessage = new MailMessage()
                {
                    From=new MailAddress(getEmailSetting.From),
                    Subject=Subject,
                    Body=message,
                    BodyEncoding=System.Text.Encoding.UTF8,
                    IsBodyHtml=true // Để gửi email với định dạng HTML
                };
                mailMessage.To.Add(email);
                SmtpClient smtpClient = new SmtpClient(getEmailSetting.SmtpServer)
                {
                    Port = getEmailSetting.Port,
                    Credentials = new NetworkCredential(getEmailSetting.From, getEmailSetting.SecretKey),
                    EnableSsl = getEmailSetting.EnableSSL,
                    UseDefaultCredentials = false // Thêm dòng này để tránh lỗi xác thực

                };
                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch (SmtpException ex) 
            {
                // In ra chi tiết lỗi để dễ dàng gỡ lỗi
                Console.WriteLine($"SMTP Exception: {ex.Message}");
                Console.WriteLine($"StatusCode: {ex.StatusCode}");
                status = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                status = false;
            }
            return status;
        }
    }
}
