using MailKit.Net.Smtp; // For smtp client
using MailKit.Security; // Security for SMTP connection
using MimeKit; // Email creation 
using Microsoft.Extensions.Options; // Uses appsettings.json to get configuration settings 
public class EmailService // Service that is responsible for managing email sending 
{
 private readonly EmailSettings _emailSettings; // Stores email setting 
 public EmailService(IOptions<EmailSettings> emailSettings) // Injectcs email settings dependency
 {
    _emailSettings = emailSettings.Value;
 }
 public void SendEmail(string toEmail, string subject, string body) //
 {
    var message = new MimeMessage(); // Creating a email message
    message.From.Add(new MailboxAddress("Book Review App Support", _emailSettings.SmtpUsername)); // Use email configuration to set the sender
    message.To.Add(new MailboxAddress("Reciever Name", toEmail)); // Setting who the email will be sent to
    message.Subject = subject; // Setting the subject of the email

    var textPart = new TextPart("plain") // Creating plain text body
    {
        Text = body
    };
    message.Body = textPart; // Assigning the body of the email
    
    using (var client = new SmtpClient()) // Creating SMTP client
    {
        client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, // Connecting to SMTP server 
        SecureSocketOptions.StartTls); 
        client.Authenticate(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword); // Authenticate using username and password
        client.Send(message); // Send the email
        client.Disconnect(true); // Disconnnet from server after email is sent 
    }
 }
}
