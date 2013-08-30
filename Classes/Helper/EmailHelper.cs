using System;
using System.Web;
using Moriyama.Umbraco.Password.Classes.Domain;

namespace Moriyama.Umbraco.Password.Classes.Helper
{
    public class EmailHelper
    {
        private static readonly EmailHelper InternalInstance = new EmailHelper();

        private EmailHelper() { }

        public static EmailHelper Instance
         {
            get 
            {
                return InternalInstance; 
            }
        }

        public string GenerateEmail(string resetUrl, PasswordResetRequest p)
        {

            var target = string.Format("Confirm.aspx?u={0}&g={1}", HttpUtility.UrlEncode(p.Login),
                                          HttpUtility.UrlEncode(p.ResetGuid.ToString()));

            resetUrl = resetUrl.Replace("Reset.aspx", target);

            return "To complete the reset of your Umbraco password please click on the link below:<br/><br/>"
                   + Environment.NewLine + Environment.NewLine
                   + "<a href=\"" + resetUrl + "\">" + resetUrl + "</a>"
                   + Environment.NewLine + Environment.NewLine;
        }
    }
}