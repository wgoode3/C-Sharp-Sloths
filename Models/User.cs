using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sloths.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        public string Username {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}