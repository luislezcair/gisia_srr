using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VTeIC.Requerimientos.Web.Controllers
{
    [Authorize]
    public class VTeICController : Controller
    {
        // GET: VTeIC
        public ActionResult Index()
        {
            return View();
        }
    }
}