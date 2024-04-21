using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.User
{
    public class UserDto : LoginUserDto
    {
        //public string UserName { get; set; }
       
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Role {  get; set; }
    }
}