using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSAHandset.Models;
using TSAHandset.ViewModel;

namespace TSAHandset.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {

        private ApplicationDbContext _context;

        public RequestController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Request
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult New()
        {

            var requestViewModel = new RequestFormViewModel()
            {
                Request = new Request(),
                Plans = _context.Plans.ToList(),
                Handsets = _context.Handsets.ToList(),
                RequestTypes = _context.RequestTypes.ToList()
            };


            return View(requestViewModel);
        }

        public ActionResult Submit(Request Request)
        {
            return View();
        }

    }
}