using System.ComponentModel.DataAnnotations;

namespace SCP.Web.Models
{
    public class AuthenticationViewModel
    {
        public string IdHashed { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }
        
    }

    public class AuthenticationPostModel
    {
        [MinLength(4)]
        [MaxLength(100)]
        public string Username { get; set; }

        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}