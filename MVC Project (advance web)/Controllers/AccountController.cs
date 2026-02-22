using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.Identity.Client;
using MVC_Project__advance_web_.Data;
using MVC_Project__advance_web_.Models;
//using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
//using System.Text.RegularExpressions;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.Runtime.CompilerServices;
//using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
//using Microsoft.Extensions.Configuration.UserSecrets;
//using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
//using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
//using System.Reflection.Metadata;
//using System.Diagnostics;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.AspNetCore.Connections.Features;
//using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
//using AspNetCoreGeneratedDocument;
using MVC_Project__advance_web_.ViewModels;
using Microsoft.AspNetCore.Identity.Data;

namespace MVC_Project__advance_web_.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext db) //connect to Database from appdbcontext file
        {
            _context = db;
        }

        //display register page TO VIEW
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid) {
                return View(model);
            } 

            bool exist = await _context.Users.AnyAsync(u => u.Email == model.Email);//IS EMAIL ALREADY EXISTED?
            if (exist) //IF TRUE
            {
                ModelState.AddModelError("Email", "Email already exists!"); //if exist  add error message to email
                return View(model);
            }

            var allowedDomains = new List<string> //proper validations for valid domains
            {
                "gmail.com",
                "yahoo.com",
                "outlook.com",
                "hotmail.com"
            };

            string domain = model.Email.Split('@')[1].ToLower();

            if (!allowedDomains.Contains(domain))
            {
              ModelState.AddModelError("Email", "Email domain is not allowed. Please use a valid email address."); //show error message if not valid domain 
                return View(model); //returns to view
            }

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(model.Email, emailPattern))
            {
               ModelState.AddModelError("Email", "Invalid email format.");
                return View(model);
            }
            // Prevent double dots
            if (model.Email.Contains(".."))
            {
                ModelState.AddModelError("Email", "Email cannot contain multiple dots.");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password doesn't match");
                return View(model);
            }

            var user = new Users
            {
                Email = model.Email,
                Password = model.Password
            };


            _context.Users.Add(user); //ADD USERS PER REGISTER
            await _context.SaveChangesAsync(); //SAVE TO Database table

            return RedirectToAction("Login", "Account"); //DIRECT TO LOGIN
        }


        [HttpGet]
        public IActionResult ResetPass(int id)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPass(ResetViewModel reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

          
            var users = await _context.Users.FirstOrDefaultAsync(u => u.Id == reset.id);

            if(users == null)
            {
                ModelState.AddModelError("Email", "Email doesn't exist");
            }

            users.Password = reset.newPassword;
          
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(EmailViewModel model)
        {
            if (!ModelState.IsValid)//ERROR INPUT
            {
               return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            

            if (user == null)
            {
               ModelState.AddModelError("Email", "Email doesn't Exist");
                return View(model);
            }

     
            int userId = user.Id;

            return RedirectToAction("ResetPass",new { id = userId });

            //await _context.SaveChangesAsync();
            //return RedirectToAction("Login", "Account");

        }

        [HttpGet] //GET LOGIN PAGE
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
           if(!ModelState.IsValid) //VALIDATION
            {
                ModelState.AddModelError("", "Incorrect Email or Password");
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email); //FIND EMAIL 
            if (user == null) //IF user NOT EXIST
            {
                ModelState.AddModelError("Email", "Email doesn't Exist");
                return View(model);
            }


            if (user.Password != model.Password)
            {
                ModelState.AddModelError("Email", "Incorrect Email or Password");
                return View(model);

            }

            var claim = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),//CREATE/generate TOKEN TO ITS CURRENT UserID assigned
                new Claim(ClaimTypes.Name, user.Email), //name to User Email

            };

            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme); 
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); //sign IN to current User

            return RedirectToAction("Index", "HomePage"); //directed to home 
        }


            [HttpPost]
            public async Task<IActionResult> Logout() 
            {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //reset token logout proceed to login
            return RedirectToAction("Login", "Account"); 
            }
    }
}
