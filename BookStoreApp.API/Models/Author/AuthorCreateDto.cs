﻿using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Author
{
    public class AuthorCreateDto
    {

        //public int Id { get; set; } we dont want to show them the ids
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(250)]
        public string Bio { get; set; }
    }
}
