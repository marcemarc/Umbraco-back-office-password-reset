using System;

namespace Moriyama.Umbraco.Password.Classes.Domain
{
    [Serializable]
    public class PasswordResetRequest
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public Guid ResetGuid { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
    }
}