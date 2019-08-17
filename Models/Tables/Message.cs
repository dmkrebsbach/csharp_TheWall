using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace projectName.Models{  //change projectName to the name of project
    
    public class Message
    {
        [Key]
        public int MessageId {get;set;}

        [Required]
        [MinLength(10)]
        public string messageContent {get;set;}

        public int UserId{get;set;}
        public User User{get;set;}

        public List<Comment> Comments{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdatedAt{get;set;} = DateTime.Now;
    }
}