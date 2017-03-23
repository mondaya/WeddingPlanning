using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class WeddingPlannerController : Controller
    {

        private WeddingPlannerContext _context;

        public WeddingPlannerController(WeddingPlannerContext context)
        {
            _context = context;
        }

        // GET: /Home/       
        [HttpGet]
        [Route("")]
        public IActionResult Weddings()
        {
            int? userLoginId = HttpContext.Session.GetInt32("userId");
            if (userLoginId != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("userName");

            }

            var targetList = _context.Events
            .GroupJoin(_context.Rsvps,
                    e => e.id,
                    r => r.EventId,
                    (e, r) => new EventView()
                    {
                        id = e.id,
                        Title = $"{e.CreatorName}  and  {e.PartnerName}  weeding!",
                        Count = r.Count(),
                        CreatedAt = e.CreatedAt,
                        Action = r.SingleOrDefault(p => p.UserId == userLoginId) == null ? "RSVP" : "ON-RSVP"
                    })
            .ToList();
            return View(targetList);


        }

        [HttpGet]
        [Route("wedding/{EventId}")]
        public IActionResult ShowEvent(int EventId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("userName");
                ViewBag.EventId = EventId; 
                EventDetails EventDetails = _context.Events
                                        .Include(r=>r.Rsvp)
                                            .ThenInclude(p=>p.User)
                                        .Where(e=>e.id == EventId)
                                        .Select(e => new EventDetails()
                                        {
                                            Title = $"{e.CreatorName}  and  {e.PartnerName}  wedding!".ToString(),
                                            CreatedAt = e.CreatedAt,
                                            GroomName =  e.CreatorType == 2 ? e.CreatorName :  e.PartnerName,
                                            GrooomList = e.Rsvp.Where(r=> r.Side == 1) 
                                                                .Select(u => u.User.FirstName)                                                               
                                                               .ToList(),
                                            BrideName =  e.CreatorType == 1 ? e.CreatorName :  e.PartnerName,
                                            BrideList =  e.Rsvp.Where(r=> r.Side == 2)
                                                                .Select(u => u.User.FirstName) 
                                                               .ToList()
                                        }).Single();
                                      
                ViewBag.EventDetails = EventDetails;
                return View();
            }
            return RedirectToAction("Index", "Home");

        }


        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("userName");
                return View();
            }
            return RedirectToAction("Index", "Home");

        }


        [HttpGet]        
        [Route("rsvp/{EventId}")]
        public IActionResult Rsvp(int EventId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("userName");
                ViewBag.EventId = EventId;
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [Route("rsvp/{EventId}")]
        public IActionResult CreateRsvp(int EventId, RsvpView Attendee)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null && ModelState.IsValid)
            {
                _context.Rsvps.Add(new Rsvp { ShowList = false, Side = Attendee.RsvpSide, EventId = EventId, UserId = (int)userId });
                _context.SaveChanges();
                ViewBag.UserName = HttpContext.Session.GetString("userName");
                return RedirectToAction("Weddings");
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]        
        [Route("create")]
        public IActionResult CreateEvent(Event wedding)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId != null && ModelState.IsValid)
            {
                ViewBag.UserName = HttpContext.Session.GetString("userName");
                wedding.UserId = (int)userId;
                _context.Events.Add(wedding);
                _context.SaveChanges();
                return RedirectToAction("Weddings");
            }
            return RedirectToAction("Index", "Home");

        }




    }

}
