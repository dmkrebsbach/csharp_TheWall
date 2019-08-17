using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projectName.Models{  //change projectName to the name of project

    public class WallViewModel
    {
        public User User{get;set;}
        public Message Message{get;set;}
        public Comment Comment{get;set;}
        public List<Message> Messages{get;set;}
        public List<Comment> Comments{get;set;}
    }
}