using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Author
{
    public class AuthorUpdateDto : BaseDto
    {
        // for update dto we will need the same fields as AuthorCreateDto and the same kind of validation but we also need the Id field when updating because Id needs to present because we need to know ehich record is being updated that is why we have put BaseDto to inherit from 
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(250)]
        public string Bio {  get; set; }
    }
}
