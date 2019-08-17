using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projectName.Models{  //change projectName to the name of project

    public class LoginViewModel
    {
        public User newUser {get;set;}  // public "ClassName" should match the class name in the file, not the file name itself
        public LoginUser newLogin {get;set;} // include all classes and a new instance of the class within this file
    }
}