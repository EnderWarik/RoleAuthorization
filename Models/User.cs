using System.ComponentModel.DataAnnotations;

namespace AuthFromRole.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ReturnUrl { get; set; }
        [Required]
        public string Role { get; set; }
        //public Role Role { get; set; }
        
       
    }
    public enum Role { User, Admin }
}
