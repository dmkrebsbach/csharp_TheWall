using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace projectName.Models{  //change projectName to the name of project
    
    public class User
    {
        [Key]
        public int UserId {get;set;}


        [Required]
        [MinLength(4, ErrorMessage="Last Name must have at least Four Characters")]
        public string FirstName {get;set;}


        [Required]
        [MinLength(4, ErrorMessage="Last Name must have at least Four Characters")]
        public string LastName {get;set;}

        [Required]
        [EmailAddress(ErrorMessage="Enter a Valid Email Address!")]
        public string Email {get;set;}

        [Required]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        
        // Will not be mapped to your users table!
        [NotMapped]                                             // this validates password matching
        [Compare("Password", ErrorMessage="Passwords Do Not Match; Please Try Again")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}

        
    }

    public class LoginUser{
        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address ex: name@address.com")]
        public string loginEmail {get; set;}
        [Required(ErrorMessage = "Enter a password")]

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string loginPassword { get; set; }
    }
} 


