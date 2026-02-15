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
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Connections.Features;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using AspNetCoreGeneratedDocument;

namespace MVC_Project__advance_web_.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;


        public AccountController(AppDbContext db) //connect to DB 
        {
            _context = db;
        }

        //display register page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))//PROPER VALIDATION
            {
                ViewBag.Error = "Email or Password must not be empty!";
                ViewBag.Email = email;
                return View();
            }

            bool exist = await _context.Users.AnyAsync(u => u.Email == email);//IS EMAIL ALREADY EXISTED?
            if (exist) //IF TRUE
            {
                ViewBag.Error = "Email already Exist!";
                ViewBag.Email = email;

                return View();
            }


            var allowedDomains = new List<string>
            {
                "gmail.com",
                "yahoo.com",
                "outlook.com",
                "hotmail.com"
            };

            string domain = email.Split('@')[1].ToLower();

            if (!allowedDomains.Contains(domain))
            {
                ViewBag.Error = "Only common email providers are allowed.";
                ViewBag.Email = email;
                return View();
            }


            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}$";

            if (!Regex.IsMatch(email, emailPattern))
            {
                ViewBag.Error = "Invalid email format.";
                ViewBag.Email = email;
                return View();
            }
            // Prevent double dots
            if (email.Contains(".."))
            {
                ViewBag.Error = "Email cannot contain consecutive dots.";
                ViewBag.Email = email;
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.ConfirmError = "Password does not match!"; //IF PASSWORD DOESNT MATCH
                ViewBag.password = password;
                return View();
            }

            var user = new Users
            {
                Email = email,
                Password = password
            };


            _context.Users.Add(user); //ADD USERS PER REGISTER
            await _context.SaveChangesAsync(); //SAVE TO DB

            return RedirectToAction("Login", "Account"); //DIRECT TO LOGIN
        }


        [HttpGet]
        public IActionResult ResetPass(int id)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPass(int id, string newpassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(newpassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "password doesn't exist.";
                ViewBag.Email = newpassword;
                return View();
            }

            if(newpassword != confirmPassword)
            {
                ViewBag.Error = "password doesn't match.";
                ViewBag.Email = newpassword;
                return View();
            }

            var users = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(users == null)
            {
                ViewBag.Error = "User not found!";
                ViewBag.Email = newpassword;
                return View();
            }

            users.Password = newpassword;
          
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))//ERROR INPUT
            {
                ViewBag.Error = "Email and Password is required.";
                ViewBag.Pass = email;
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            

            if (user == null)
            {
                ViewBag.Error = "Email doesn't exist.";
                ViewBag.Email = email;
                return View();
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
        public async Task<IActionResult> Login(string email, string password) 
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))//ERROR INPUT
            { 
                ViewBag.Error = "Email and Password is required."; 
                ViewBag.Pass = password;
                return View();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email); //FIND EMAIL 
            if (user == null) //IF NOT EXIST
            {
                ViewBag.Error = "Email or Password is incorrect";
                ViewBag.Email = email;
                return View();
            }

            if (user.Password != password) //IS PASSWORD VALID?
            {
                ViewBag.Error = "Invalid Password";
                ViewBag.Password = password;
                return View();
            }

            var claim = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),//CREATE/generate TOKEN TO ITS CURRENT UserID assigned
                new Claim(ClaimTypes.Name, user.Email), //name to User Email

            };

            var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme); 
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal); //sign IN to current User

            return RedirectToAction("Index", "Home"); //directed to home 
        }

            [HttpPost]
            public async Task<IActionResult> Logout() 
            {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //reset token logout
            return RedirectToAction("Login", "Account"); 
            }
        public IActionResult Index()
        {
            return View();
        }
    }
}
