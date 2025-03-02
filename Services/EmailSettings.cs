public class EmailSettings // Settings for sending emails 
 {
 public string SmtpServer { get; set; } // SMTP server address - for this application will be for gmail
 public int SmtpPort { get; set; } // The port number to connect to the server 
 public string SmtpUsername { get; set; } // SMTP user name used in email for authentication
 public string SmtpPassword { get; set; } // Secure Password for authentication
 }