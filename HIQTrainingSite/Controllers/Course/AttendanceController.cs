using HIQTraining.Business.Attendance;
using HIQTraining.Business.Calendar;
using HIQTraining.Business.Course;
using HIQTraining.Business.Log;
using HIQTraining.DAL.Attendance;
using HIQTraining.DAL.Calendar;
using HIQTraining.DAL.Course;
using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.Controllers.Course
{
    [Authorize]
    public class AttendanceController : BaseController
    {
        CalendarManager calendarManager;
        CourseManager courseManager;
        AttendanceManager attendanceManager;

        public AttendanceController()
        {
            calendarManager = new CalendarManager(new CalendarDao());
            courseManager = new CourseManager(new CourseDao());
            attendanceManager = new AttendanceManager(new AttendanceDao());
        }

        [Authorize(Roles = "Admin, Staff")]
        // GET: Attendance
        public ActionResult Index(int? id, int? selectedDateId, string message)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownAction });
            }

            AttendanceListViewModel model = new AttendanceListViewModel();
            if (TempData["selectedDateId"] != null)
            {
                model.SelectedDateId = ((int)TempData["selectedDateId"]);
            }
         

            if (TempData["alert"] != null)
                model.Alert = ((AlertViewModel)TempData["alert"]);
            else if (!string.IsNullOrWhiteSpace(message))
                model.Alert.SetErrorMessage(message);


            var course = courseManager.GetCourseById(id.Value);
            if (course == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            model.CourseId = course.Id;
            model.CourseName = course.Name;
            model.IsCourseFinished = (course.IsCanceled || course.IsFinished) ? true : false;

            model.CourseDates = calendarManager.GetCourseCalendar(id.Value).ToList();
            if (model.CourseDates.Count > 0)
            {
                if (selectedDateId == null)
                    selectedDateId = model.CourseDates.FirstOrDefault().Id;
                model.AttendanceList = attendanceManager.GetCourseAttendanceForDate(id.Value, selectedDateId.Value);
            }
            else
            {
                //model.AttendanceList = new List<AttendanceDetail>();
                //caso nao existir datas vai desparar um popUp.
                TempData["Error"] = "true";
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnableToExecuteOperation });

            }
               

            return View(model);
        }

        // POST: Attendance/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(AttendanceListViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    viewModel.CourseDates = calendarManager.GetCourseCalendar(viewModel.CourseId).ToList();
                    int result = attendanceManager.SaveAttendanceList(viewModel.CourseId, viewModel.SelectedDateId, viewModel.AttendanceList, base.GetLoggedUser());
                    viewModel.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);

                    TempData["alert"] = viewModel.Alert;
                    TempData["selectedDateId"] = viewModel.SelectedDateId;
                    return RedirectToAction("Index", new { Id = viewModel.CourseId });
                }

                return View("Index", viewModel);
            }
            catch (DbUpdateException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.Attendance, ex.Message, ex.StackTrace, base.GetLoggedUser());
                viewModel.Alert.SetErrorMessage(HIQResources.errorMessageDuplicateRecord);
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Attendance, ex.Message, ex.StackTrace, base.GetLoggedUser());
                viewModel.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);
                return View("Index", viewModel);
            }
        }
       
    }
}