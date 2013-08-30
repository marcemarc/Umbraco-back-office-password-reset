using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Moriyama.Umbraco.Password.Classes.Application;
using Moriyama.Umbraco.Password.Classes.Domain;
using Moriyama.Umbraco.Password.Classes.Helper;
using Umbraco.Core.IO;
using Umbraco.Core.Logging;
using umbraco;
using umbraco.BusinessLogic;


namespace Moriyama.Umbraco.Password.umbraco.Plugins.Moriyama.Password
{
    public partial class Reset : System.Web.UI.Page
    {
        private string _basePath;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_basePath)) return;

            _basePath = IOHelper.MapPath("/App_Data");
            _basePath = Path.Combine(_basePath, @"Moriyama\PasswordReset");
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        { }

        protected void Submit_Click(object sender, EventArgs e)
        {
            Confirm.Visible = true;
            var login = Login.Text.Trim();

            // replace non alpha numerics
            var regex = new Regex("[^a-zA-Z0-9 -]");
            login = regex.Replace(login, "");

            if (string.IsNullOrEmpty(login))
            {
                Login.Text = login;
                Confirm.Text = "<p>Please enter a login</p>";
                return;
            }


            Confirm.Text =
                "<p>Your resest request has been received and you will be emailed instructions on how to reset your password.</p>";

            User u;
            try
            {
                u = new User(login);
            }
            catch (Exception ex)
            {
                // Don't want to notify if the username doesn't exist in case someone is just running through a list of known
                // logins. trying to hack us!
                LogHelper.WarnWithException<Reset>(ex.Message, ex);
                return;
            }

            var resetGuid = Guid.NewGuid();

            var resetRequest = new PasswordResetRequest
                 {
                     Date = DateTime.Now, 
                     Login = u.LoginName,
                     ResetGuid = resetGuid, 
                     UserId = u.Id,
                     Email = u.Email
                 };

            var outputPath = Path.Combine(_basePath,
                                          FilePathService.Instance.RemoveIllegalCharactersFromFileName(u.LoginName));

            FilePathService.Instance.EmptyDirectory(outputPath, true);

            if (!Directory.Exists(outputPath)) 
                Directory.CreateDirectory(outputPath);
            
            var outputFile = Path.Combine(outputPath, resetRequest.ResetGuid + ".xml");

            using (var myWriter = new StreamWriter(outputFile, false))
            {
                var mySerializer = new XmlSerializer(typeof(PasswordResetRequest));
                mySerializer.Serialize(myWriter, resetRequest);
            }
            
            
            var mail = EmailHelper.Instance.GenerateEmail(Request.Url.ToString(), resetRequest);
            var message = new MailMessage {From = new MailAddress(UmbracoSettings.NotificationEmailSender)};

            message.To.Add(u.Email);
            message.Subject = "Your Umbraco password reset request";

            message.Body = mail;

            message.IsBodyHtml = true;

            new SmtpClient().Send(message);
        }
    }
}