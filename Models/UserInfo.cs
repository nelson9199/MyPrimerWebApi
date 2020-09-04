using System.ComponentModel.DataAnnotations;
namespace MyPrimerWebApi.Models
{
    public class UserInfo
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}