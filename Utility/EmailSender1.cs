using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
namespace Utility;






public class EmailSender1
{
    private readonly string _smtpServer = "mail.etatvasoft.com";
    private readonly string _smtpUser = "test.dotnet@etatvasoft.com";
    private readonly string _smtpPassword = "P}N^{z-]7Ilp";
    private readonly int _smtpPort = 587; // Example for SMTP

    public async Task SendEmailAsync(string email, string subject, string message)

    {



        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(MailboxAddress.Parse(_smtpUser));
        mimeMessage.To.Add(MailboxAddress.Parse(email));

        mimeMessage.Subject = subject;

        // mimeMessage.Body = new TextPart("html")
        // {
        //     Text = message
        // };

        //  var bodyBuilder = new BodyBuilder
        // {
        //     HtmlBody = message
        // };
        // mimeMessage.Body = bodyBuilder.ToMessageBody();

        var bodyBuilder = new BodyBuilder();

        bodyBuilder.HtmlBody = message;




        mimeMessage.Body = bodyBuilder.ToMessageBody();



        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, false);
            await client.AuthenticateAsync(_smtpUser, _smtpPassword);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }

}
