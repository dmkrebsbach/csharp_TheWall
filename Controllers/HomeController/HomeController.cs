using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http; // FOR USE OF SESSIONS
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using projectName.Models; //change projectName to the name of project

namespace projectName.Controllers  //change projectName to the name of project
{
    public class HomeController : Controller{
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]               // GETS Main Registration and Login Page
        public IActionResult Index(){
            return View("Login");
        }

        [HttpPost("register")]
        public IActionResult CreateUser(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == viewModel.newUser.Email))
                {
                    ModelState.AddModelError("newUser.Email", "Email already in use!");
                    return View("Login");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                viewModel.newUser.Password = Hasher.HashPassword(viewModel.newUser, viewModel.newUser.Password);

                dbContext.Users.Add(viewModel.newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("userInSess", viewModel.newUser.UserId);

                return RedirectToAction("Wall");
            }
            else
            {
                return View("Login");
            }
        }


        [HttpPost("login")]
        public IActionResult LoginUser(LoginViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var dbUser = dbContext.Users.FirstOrDefault(u => u.Email == viewModel.newLogin.loginEmail);
                if(dbUser == null)
                {
                    ModelState.AddModelError("newLogin.loginEmail", "Email does not exist; please create account");
                    return View("Login");
                }

                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(viewModel.newLogin, dbUser.Password, viewModel.newLogin.loginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("newLogin.loginPassword", "Password does not match Account on File");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("userInSess", dbUser.UserId);

                return RedirectToAction("Wall");
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("/success")]
        public IActionResult Success()
        {
            return RedirectToAction("Wall"); // RETURN REDIRECT TO FIRST RENDERED PAGE UPON SUCCESSFUL LOGIN/REG
        }

        // The rest of the Controller Code goes here (routes, Posts, Gets, Linq, etc)

        [HttpGet("wall")]
        public IActionResult Wall(){
            if(HttpContext.Session.GetInt32("userInSess") == null)
            {
                TempData["error"] = "You must be logged in to do that";
                return RedirectToAction("Index");
            }

            WallViewModel viewModel = new WallViewModel();
            viewModel.User = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userInSess"));
            viewModel.Messages = dbContext.Messages
                .Include(m => m.User)
                .Include(m => m.Comments)
                .ThenInclude(c => c.User)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View("Wall", viewModel);
        }

        [HttpPost("writeMessage")]
        public IActionResult CreateMessage(WallViewModel viewModel)
        {
            if(HttpContext.Session.GetInt32("userInSess") == null)
            {
                TempData["error"] = "You must be logged in to do that";
                return RedirectToAction("Index");
            }

            viewModel.Message.UserId = (int)HttpContext.Session.GetInt32("userInSess");

            dbContext.Messages.Add(viewModel.Message);
            dbContext.SaveChanges();

            return RedirectToAction("Wall");
        }

        [HttpGet("message/delete/{MessageId}")]
        public IActionResult DeleteMessage(int MessageId)
        {
            if(HttpContext.Session.GetInt32("userInSess") == null)
            {
                TempData["error"] = "You must be logged in to do that";
                return RedirectToAction("Index");
            }

            Message message = dbContext.Messages
                .Include(m => m.User)
                .FirstOrDefault(m => m.MessageId == MessageId);
                
            dbContext.Messages.Remove(message);
            dbContext.SaveChanges();

            return RedirectToAction("Wall");
        }

        [HttpPost("comment/{MessageId}/create")]
        public IActionResult CreateComment(WallViewModel viewModel, int MessageId)
        {
            if(HttpContext.Session.GetInt32("userInSess") == null)
            {
                TempData["error"] = "You must be logged in to do that";
                return RedirectToAction("Index");
            }
            if(ModelState.IsValid)
            {
                viewModel.Comment.MessageId = MessageId;
                viewModel.Comment.UserId = (int)HttpContext.Session.GetInt32("userInSess");

                System.Console.WriteLine("PLEASE GET HERE ########################################");
                System.Console.WriteLine(viewModel);
                System.Console.WriteLine(viewModel.Comment);
                System.Console.WriteLine(viewModel.Comment.MessageId);
                System.Console.WriteLine(viewModel.Comment.UserId);
                System.Console.WriteLine(viewModel.Comment.commentContent);
                System.Console.WriteLine("PLEASE GET HERE ########################################");

                dbContext.Comments.Add(viewModel.Comment);
                dbContext.SaveChanges();

                return RedirectToAction("Wall");
            }
            return View("Wall");
        }

    }
}