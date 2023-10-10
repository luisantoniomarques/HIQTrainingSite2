using HIQTraining.Business.Calendar;
using HIQTraining.DAL.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.Controllers.Calendar
{
    [Authorize]
    public class CalendarController : BaseController
    {
        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetFullCalendarCourse() {
            CalendarManager manager = new CalendarManager(new CalendarDao());
            var courseTypes = manager.GetCoursesCalendar();

            return Json(courseTypes, JsonRequestBehavior.AllowGet);
        }



    }
}