using Microsoft.EntityFrameworkCore;

namespace projectName.Models //change projectName to the name of project
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;} // needs one line for each Model.cs file created, 
        public DbSet<Message> Messages {get;set;}
        public DbSet<Comment> Comments {get;set;}                                    //User is Model Name, Users is the Db Property & Table Name
    }
}