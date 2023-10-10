using HIQTraining.Business.Note;
using HIQTraining.DAL.Notes;
using HIQTraining.ModelDetail;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.Threading;
using HIQTrainingSite.ViewModel;

namespace HIQTrainingSite.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private NoteManager noteManager;
        public HomeController()
        {
            noteManager = new NoteManager(new NoteDao());
        }

        [HttpGet]
        public ActionResult Index(DateTime? sDate, DateTime? eDate)
        {
            HomeViewModel homeView = new HomeViewModel();

            if (sDate == null || eDate == null)
            {
                sDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
                eDate = sDate.Value.AddDays(6).AddSeconds(-1);
            }
          
            homeView.Notes.NoteList = noteManager.GetWeekEventsFromUser(sDate, eDate);

            ViewBag.culture = Thread.CurrentThread.CurrentUICulture.TextInfo.CultureName;

            if (homeView.Notes.NoteList.Any())
            {
                ViewBag.IdLastEvent = noteManager.GetLastEventFromUser();
            }
            else
            {
                ViewBag.IdLastEvent = 0;
            }

            ViewBag.barColor = GetUserColor();
            ViewBag.userCreated = GetLoggedUser();

            ViewBag.visibleDateStart = sDate.Value.Date.ToString("yyyy-MM-dd");

            return View(homeView);
        }

        public ActionResult Events(DateTime? startDate, DateTime? endDate)
        {
            IList<NoteDetail> events = noteManager.GetWeekEventsFromUser(startDate, endDate);
            long lastEventId = noteManager.GetLastEventFromUser();

            object result = new
            {
                events,
                lastEventId
            };

            return new JsonResult { Data = result };
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection)
        {
            HomeViewModel homeView = new HomeViewModel();

            JavaScriptSerializer objJavascript = new JavaScriptSerializer();
            string json = collection["noteList"].ToString().Replace(@"/Date(", "\\/Date(").Replace(@")/", ")\\/");
            string jsonEventsRemoved = collection["eventsRemoved"].ToString().Replace(@"/Date(", "\\/Date(").Replace(@")/", ")\\/");

            string userName = base.GetLoggedUser();
            DateTime startDate = Convert.ToDateTime(collection["startDate"]);
            DateTime endDate = Convert.ToDateTime(collection["endDate"]);

            List<NoteDetail> notes = objJavascript.Deserialize<List<NoteDetail>>(json);
            IList<NoteDetail> notesRemoved = objJavascript.Deserialize<List<NoteDetail>>(jsonEventsRemoved);

            if (notesRemoved != null)
            {
                notes.AddRange(notesRemoved);
            }

            notes = notes.Where(n => n.UserCreated == null || n.isUpdated == true || n.isDeleted == true).ToList();

            noteManager.SaveChanges(notes,userName);

            return RedirectToAction("Index", new { sDate = startDate, eDate = endDate });
        }

        public ActionResult Index2()
        {
          return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}