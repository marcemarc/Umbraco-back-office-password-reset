using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Xml.Serialization;
using Moriyama.Umbraco.Password.Classes.Application;
using Moriyama.Umbraco.Password.Classes.Domain;
using Umbraco.Core.IO;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.providers;

namespace Moriyama.Umbraco.Password.umbraco.Plugins.Moriyama.Password
{
    public partial class Confirm : System.Web.UI.Page
    {
        private string _basePath;
        private string _resetFile;
        private int _expiryPeriod = 30; //in minutes

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_basePath)) return;

            _basePath = IOHelper.MapPath("/App_Data");
            _basePath = Path.Combine(_basePath, @"Moriyama\PasswordReset");

            var login = Request["u"].Trim();
            var resetGuid = Request["g"];

            // replace non alpha numerics
            var regex = new Regex("[^a-zA-Z0-9 -]");
            login = regex.Replace(login, "");

            if (string.IsNullOrEmpty(login)) return;

            var outputPath = Path.Combine(_basePath,
                                          FilePathService.Instance.RemoveIllegalCharactersFromFileName(login));

            if (!Directory.Exists(outputPath))
                return;

            var outputFile = Path.Combine(outputPath, resetGuid + ".xml");

            if (!File.Exists(outputFile))
                return;

            //check datetime that reset file was created,
            // if it is more than expiry period eg 30mins,
            // don't let the password be reset
            DateTime dateCreated = File.GetCreationTime(outputFile);
            TimeSpan resetDuration = DateTime.Now - dateCreated;
            // should the reset time be configurable somewhere ?      
            if (resetDuration.TotalMinutes > _expiryPeriod)
            {
                
                ResetExpiredPanel.Visible = true;

            }
            else
            {

                _resetFile = outputFile;
                Panel1.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var password = Password.Text;
            var confirm = ConfirmPassword.Text;

            if (string.IsNullOrEmpty(password))
            {
                ErrorLiteral.Text = "<p>You must specify a password</p>";
                ErrorLiteral.Visible = true;
                return;
            }

            if (password != confirm)
            {
                ErrorLiteral.Text = "<p>Passwords must match</p>";
                ErrorLiteral.Visible = true;
                return;
            }

            if (password.Length < 6)
            {
                ErrorLiteral.Text = "<p>Your password should be 6 characters or more</p>";
                ErrorLiteral.Visible = true;
                return;
            }

            var nonAlphaNumeric = Regex.Replace(password, "[a-zA-Z0-9]", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            if (nonAlphaNumeric.Length < 1)
            {
                ErrorLiteral.Text = "<p>Your password should contain at least one non-alphanumeric character</p>";
                ErrorLiteral.Visible = true;
                return;
            }
            
            PasswordResetRequest p;

            var serializer = new XmlSerializer(typeof(PasswordResetRequest));

            using (var fileStream = new FileStream(_resetFile, FileMode.Open))
            {
                p = (PasswordResetRequest)serializer.Deserialize(fileStream);
            }

            var u = new User(p.UserId);

            var provider = (UsersMembershipProvider)Membership.Providers[UmbracoSettings.DefaultBackofficeProvider];
            u.Password = provider.EncodePassword(password);

            u.Save();

            File.Delete(_resetFile);

            Response.Redirect("/umbraco");
        }
    }
}