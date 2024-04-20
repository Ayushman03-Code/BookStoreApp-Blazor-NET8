using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Author
{
    public class AuthorReadOnlyDto : BaseDto
    {
        // We are using AuthorDto for the read operations which will also inherit from BaseDto for Id
        // This will not be used for Deletion or any CRUD operation it will only use for GET
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio  { get; set; }
    }
}
