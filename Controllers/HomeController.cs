using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {

        private WeddingPlannerContext _context;

        public HomeController(WeddingPlannerContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("loginReg")]
        public IActionResult Index()
        {
            ViewBag.LoginView = new LoginViewModel();
            ViewBag.Login = null;
            return View("Index");
        }
               
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel userForm)
        {

            if (ModelState.IsValid)
            {

                User userDb = _context.Users.SingleOrDefault(newUser => newUser.Email == userForm.Email);
                if(userDb == null){

                    User user = new User
                    {
                        FirstName = userForm.FirstName,
                        LastName = userForm.LastName,
                        Email = userForm.Email,
                        Password = userForm.Password
                    };
                    //TODO more chaecks need before saving user                   
                    _context.Add(user);
                    _context.SaveChanges();
                    userDb = _context.Users.SingleOrDefault(newUser => newUser.Email == userForm.Email);
                    HttpContext.Session.SetInt32("userId", userDb.id);
                    HttpContext.Session.SetString("userName", userDb.FirstName);
                    return RedirectToAction("Weddings", "WeddingPlanner");
                    //return RedirectToAction("Transactions", new { userId = userDb.id });
                }
                else
                {
                    ViewBag.UserExistsMsg = "user Already exists";
                }
            }
            ViewBag.Login = null;
            return View("Index");
        }


        // GET: /Home/
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel userForm)
        {

            if (ModelState.IsValid)
            {
                User userDb = _context.Users.SingleOrDefault(user => user.Email == userForm.Email);
                if (userDb != null && userDb.Password ==  userForm.Password)
                {
                    HttpContext.Session.SetInt32("userId", userDb.id);  
                    HttpContext.Session.SetString("userName", userDb.FirstName);                                    
                    return RedirectToAction("Weddings", "WeddingPlanner");
                }
                else 
                {
                    ViewBag.InvalidUserMsg = "Unable to match user name or password";
                }
            }

            ViewBag.Errors = ModelState.Values;
            ViewBag.Login = new LoginViewModel();
            return View("Index");


        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();           
            return View("Index");


        }
        [HttpGet]
        [Route("account/{userId}")]
        public IActionResult Transactions(int userId)
        {

            if (HttpContext.Session.GetInt32("userId") == userId)
            {
                User userDb = _context.Users.SingleOrDefault(user => user.id == userId);
                ViewBag.UserName = userDb.FirstName;
                ViewBag.Balance = _context.getBalance(userId);   
                List<Transaction> transactions = _context.Transactions.Where( record => record.UserId == userId).ToList();

                return View(transactions);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("account/{userId}")]
        public JsonResult Transaction(int userId, Transaction transaction)
        {

            if (HttpContext.Session.GetInt32("userId") == userId)
            {
                int currentBalance = _context.getBalance(userId);
                int newBalance = 0;

                if (transaction.Amount < 0 && Math.Abs(transaction.Amount) >= currentBalance)
                {
                    return Json(new { error = true, message = $"Insuffient Balance ${currentBalance}" });
                }
                newBalance = currentBalance + transaction.Amount;          


                Transaction newTransaction = new Transaction()
                {
                    Description = transaction.Amount < 0 ? "withdraw" : "deposit",
                    Balance = newBalance,
                    Amount = transaction.Amount,
                    UserId = userId
                };

                _context.Add(newTransaction);
                _context.SaveChanges();

                return Json(newTransaction);
            }
            return Json(new { error = true, message = "Invalid user,. Please login or Register" });
        }



    }
}
