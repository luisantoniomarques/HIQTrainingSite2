using HIQTraining.Business.Calendar;
using HIQTraining.Business.Configuration;
using HIQTraining.Business.Course;
using HIQTraining.Business.CourseLevel;
using HIQTraining.Business.CourseLocation;
using HIQTraining.Business.Teacher;
using HIQTraining.DAL.Configuration;
using HIQTraining.DAL.Course;
using HIQTraining.DAL.Teacher;
using HIQTraining.ModelDetail;
using HIQTrainingSite.Mappers;
using HIQTrainingSite.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using HIQTraining.DAL.Calendar;
using System.Data.Entity.Infrastructure;
using HIQTrainingSite.Common;
using HIQTraining.Exceptions;
using HIQTraining.Business.Log;
using System.Collections.Generic;
using HIQTrainingSite.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using HIQTraining.Business.CourseType;
using HIQTraining.Business.Inscription;
using HIQTraining.DAL.Inscription;
using System.Threading;
using System.Web.Script.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Helpers;


namespace HIQTrainingSite.Controllers.Course {

    [Authorize]
    public class CourseController : BaseController {

        private Mapper mapper;
        private CourseManager courseManager;
        //TODO Add calendar manager to private variables to replace local ones, ajust names

        private static List<DateTime> datesToRemove = new List<DateTime>();
        public CourseController() {
            mapper = new Mapper();
            courseManager = new CourseManager(new CourseDao());
        }

        //GET: Course/Index
        public ActionResult Index(string message, int page = 1) {

          //  ModelState.AddModelError(string.Empty, message);

            CourseListViewModel vm = new CourseListViewModel();
            //ModelState.Clear();

            if (User.IsInRole("Admin") || User.IsInRole("Staff"))
                ViewBag.BlockOrNone = "";
            else
                ViewBag.BlockOrNone = "display:none";

            if (TempData["alert"] != null)
            {
                vm.Alert = (AlertViewModel)TempData["alert"];
            }
            else if(!string.IsNullOrEmpty(message))
            {
                vm.Alert.SetErrorMessage(message);
            }


            vm.CourseList = courseManager.GetPagedCourse(page, HIQSiteConstants.General.PAGE_SIZE);
           

            return View(vm);
        }

        public ActionResult SearchCourse(string name, string level, string teacher, DateTime? date, int? estado, string entity) {
            CourseListViewModel vm = this.SearchCourses(name, level, teacher, date, estado, entity);

            return PartialView("_CourseList", vm);
        }

        public CourseListViewModel SearchCourses(string name, string level, string teacher, DateTime? date, int? status, string entity, int page = 1) {
            CourseListViewModel vm = new CourseListViewModel();
            if (status.HasValue && status == -1)
                status = null;
            vm.CourseList = courseManager.GetCoursesSearchResults(name, level, teacher, date, status, entity, page, HIQSiteConstants.General.PAGE_SIZE);

            return vm;
        }

        [Authorize(Roles = "Admin, Staff")]
        //GET: Course/Create
        [HttpGet]
        public ActionResult Create(int? teacherId)
        {
            //guardar os dados do formolário depois de criar 
            CourseViewModel tempdata = (CourseViewModel)TempData["TempDataCourse"];
         


            //caso a formador for criado atraves do curso
            TeacherViewModel vm = (TeacherViewModel)TempData["teacherView"];
            //se exister um erro ap criar um formador atraves do curso
            if (TempData["TeacherErrorDuplicateName"] != null)
            {
                ViewBag.OpenModal = "OpenModal";
                ViewBag.statusChekedboxInternalPartial = "checked";

                ViewBag.ErrorState= true;
            }
          


            CourseViewModel model = new CourseViewModel();
            if (vm != null)
            {
                ViewBag.TeacherName = vm.Name;
                ViewBag.Teacherid = courseManager.GetTeacherIdByEmail(vm.Email);
                ViewBag.email = vm.Email;
            }

            if (tempdata != null)
            {
                model.Name = tempdata.Name;
                model.SelectedLevelId = tempdata.SelectedLevelId;
                model.SelectedCourseTypeId = tempdata.SelectedCourseTypeId;
                model.Code = tempdata.Code;
                model.SelectedLocationId = tempdata.SelectedLocationId;
                model.SelectedLocationId = tempdata.SelectedLocationId;
                model.EntityFormativeId = tempdata.EntityFormativeId;
                model.StartHour = tempdata.StartHour;
                model.PayRoll = tempdata.PayRoll;

            }



            CourseLocationManager managerLocation = new CourseLocationManager(new CourseLocationDao());
            var locations = managerLocation.GetCourseLocations();
            model.LocationsList = locations.ToList();

            // criar aqui a lista para aceder a lista coursetype
            CourseTypeManager courseType = new CourseTypeManager(new CourseTypeDao());
            var types = courseType.GetCourseTypes();
            model.CourseTypeList = types.ToList();

            CourseLevelManager managerLevel = new CourseLevelManager(new CourseLevelDao());
            var levels = managerLevel.GetCourseLevels();
            model.LevelsList = levels.ToList();

            
            FormativeEntityManager formativeEntity = new FormativeEntityManager(new FormativeEntityDao());
            var entities = formativeEntity.GetFormativeEntities();
            model.FormativeEntitiesList = entities.ToList();

            if(teacherId.HasValue) {
                TeacherManager teacherManager = new TeacherManager(new TeacherDao());
                TeacherDetail teacher = teacherManager.GetTeacherDetailById(teacherId.Value);
                model.Teacher = teacher.Name;
                model.TeacherId = teacher.Id;
                model.TeacherEmail = teacher.Email;
            }


            ViewBag.backColor = GetUserColor();
            ViewBag.culture = Thread.CurrentThread.CurrentUICulture.TextInfo.CultureName;

            return View(model);
        }

        //POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel, string calendarEvents)
        {

            List<CalendarDetail> calendarDetails = new List<CalendarDetail>();

            try
            {
                string calendarDates = string.Empty;

                TeacherManager teacherManager = new TeacherManager(new TeacherDao());
                HIQTraining.Model.Teacher teacher = teacherManager.GetTeacherByName(viewModel.Teacher);

                bool ocupiedTeacher = false;
                string ocupiedDate = String.Empty;

                //validar se ja existe uma formaçao para aquela data com aquele professo :D
                HIQTraining.Model.Course course = new HIQTraining.Model.Course();
                course.Id = viewModel.Id;
                course.Code = viewModel.Code;
                course.Name = viewModel.Name;
                course.EndHour = viewModel.EndHour;
                course.StartHour = viewModel.StartHour;
                course.Effort = viewModel.Effort;
                course.Teacher = teacher;
                course.PayRoll = viewModel.PayRoll;
               

                if (!String.IsNullOrEmpty(calendarEvents))
                {
                
                    CalendarManager calendar = new CalendarManager(new CalendarDao());

                    calendarEvents = calendarEvents.Replace(@"/Date(", "\\/Date(").Replace(@")/", ")\\/");
                    dynamic calendarResults = JsonConvert.DeserializeObject<dynamic>(calendarEvents);
                    calendarDates = GetDatesFromCalendar(calendarResults);
                    calendarDetails = calendar.GetCalendarList(calendarDates);

                    if (teacher == null)
                    {
                        ModelState.AddModelError(string.Empty, "O formador não existe.");
                    }
                    else
                    {
                        foreach (CalendarDetail item in calendarDetails)
                        {

                            if (courseManager.ValidateDate(course, item.Date))
                            {
                                ocupiedTeacher = true;
                                ocupiedDate = item.Date.ToShortDateString().ToString();
                                break;
                            }
                        }
                    }
                }

                //validar a inserção de dados.
                CourseLocationManager managerLocation = new CourseLocationManager(new CourseLocationDao());
                var locations = managerLocation.GetCourseLocations();
                viewModel.LocationsList = locations.ToList();

                CourseLevelManager managerLevel = new CourseLevelManager(new CourseLevelDao());
                var levels = managerLevel.GetCourseLevels();
                viewModel.LevelsList = levels.ToList();

                FormativeEntityManager formativeEntity = new FormativeEntityManager(new FormativeEntityDao());
                var entities = formativeEntity.GetFormativeEntities();
                viewModel.FormativeEntitiesList = entities.ToList();

                CourseTypeManager courseType = new CourseTypeManager(new CourseTypeDao());
                var types = courseType.GetCourseTypes();
                viewModel.CourseTypeList = types.ToList();

                if (calendarDates != string.Empty)
                {
                    if (calendarDetails.Count > 0)
                    {
                        if (calendarDetails.Any(a => a.Date < DateTime.Today))
                            ModelState.AddModelError(string.Empty, "Não são permitidas datas no passado.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "As datas da formação são de preenchimento obrigatório.");
                }
                if (viewModel.StartHour.CompareTo(viewModel.EndHour) >= 0)
                {
                    ModelState.AddModelError("EndHour", "Hora de inicio tem de ser menor que a hora de fim.");
                    //return View(viewModel);
                }
                if (ocupiedTeacher)
                {
                    //mudar tipo de erro
                    ModelState.AddModelError("Teacher", "Formador Ocupado no dia : " + ocupiedDate + " no horario " + course.StartHour + " - " + course.EndHour);
                    //return View(viewModel);

                }
                if (ModelState.IsValid)
                {

                    if (teacher == null)
                    {
                        ModelState.AddModelError("Teacher", HIQResources.errorMessageTeacherNotFound);
                        //return View(viewModel);
                    }

                    HIQTraining.Model.Course newCourse = mapper.CourseMapperCourseVM(viewModel, base.GetLoggedUser(), calendarDates);
                    courseManager.Add(newCourse, base.GetLoggedUser());

                    if (newCourse != null)
                    {
                        viewModel.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = viewModel.Alert;

                        return RedirectToAction("Index");
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }

            ViewBag.backColor = GetUserColor();
            ViewBag.culture = Thread.CurrentThread.CurrentUICulture.TextInfo.CultureName;

            return View(viewModel);
        }

        private string GetDatesFromCalendar(dynamic calendarResults)
        {
            string datesToCalendar = string.Empty;
            foreach (var calendar in calendarResults)
            {
                if (calendar.isUpdated == true)
                {
                    DateTime startDate = calendar.start;
                    DateTime endDate = calendar.end;

                    for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                    {
                        datesToCalendar += dt.ToString("yyyy-MM-dd") + ",";
                    }
                }
            }
            return datesToCalendar != string.Empty ? datesToCalendar.Remove(datesToCalendar.Length - 1) : string.Empty;
        }

        [HttpGet]
        public ActionResult GetTeacherByName(string teacherName) {
            TeacherManager manager = new TeacherManager(new TeacherDao());
            var teachers = manager.GetTeachers(teacherName);

            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCourseLevelByName(string courseLevel) {
            CourseLevelManager managerLevel = new CourseLevelManager(new CourseLevelDao());
            var levels = managerLevel.GetCourseLevels();

            return Json(levels, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCourseByName(string courseName) {
            var courses = courseManager.GetCoursesByName(courseName);

            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCourseTypeByName(string courseType) {
            FormativeEntityManager manager = new FormativeEntityManager(new FormativeEntityDao());
            var courseTypes = manager.GetFormativeEntitiesByName(courseType);

            return Json(courseTypes, JsonRequestBehavior.AllowGet);
        }

        //GET: Course/Details
        [HttpGet]
        public ActionResult Details(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CourseFullDetail course = courseManager.GetCourseById(id.Value);
            if(course == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CourseDetailsViewModel courseVm = new CourseDetailsViewModel();
            courseVm = mapper.CourseDetails(course);

            return View(courseVm);
        }

        //GET: Course/Edit
        [Authorize(Roles = "Admin, Staff")]
        [HttpGet]
        public ActionResult Edit(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CourseFullDetail course = courseManager.GetCourseById(id.Value);

            if(course == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CourseLocationManager managerLocation = new CourseLocationManager(new CourseLocationDao());
            var locations = managerLocation.GetCourseLocations();
            CourseLevelManager managerLevel = new CourseLevelManager(new CourseLevelDao());
            var levels = managerLevel.GetCourseLevels();
            FormativeEntityManager formativeEntity = new FormativeEntityManager(new FormativeEntityDao());
            var entities = formativeEntity.GetFormativeEntities();

            CourseTypeManager courseType = new CourseTypeManager(new CourseTypeDao());
            var types = courseType.GetCourseTypes();
         
            CalendarManager calendarManager = new CalendarManager(new CalendarDao());
            List<NoteDetail> calendarDates = GetCalendarDates(calendarManager.GetCourseCalendar(course.Id));

            CourseViewModel courseVm = new CourseViewModel();
            courseVm.Id = course.Id;
            courseVm.Name = course.Name;
            courseVm.LevelsList = levels.ToList();
            courseVm.SelectedLevelId = course.LevelId;
            courseVm.CourseTypeList = types.ToList();
            courseVm.SelectedCourseTypeId = course.TypeId;
            courseVm.Code = course.Code;
            courseVm.LocationsList = locations.ToList();
            courseVm.SelectedLocationId = course.LocationId;
            courseVm.Teacher = course.Teacher;
            courseVm.TeacherId = course.TeacherId;
            courseVm.FormativeEntitiesList = entities.ToList();
            courseVm.EntityFormativeId = course.EntityId;
            courseVm.Observation = course.Observation;
            courseVm.StartDate = course.StartDate;
            courseVm.CloseDate = course.CloseDate;
            courseVm.StartHour = course.StartHour;
            courseVm.EndHour = course.EndHour;
            courseVm.Effort = course.Effort;
            courseVm.StatusId = course.StatusId;
            courseVm.IsFinished = course.IsFinished;
            courseVm.IsCanceled = course.IsCanceled;
            courseVm.isDtp = course.IsDtp;
            courseVm.CanceledObservation = course.CanceledObservation;
            courseVm.PayRoll = course.PayRoll;

            ViewBag.backColor = GetUserColor();
            ViewBag.culture = Thread.CurrentThread.CurrentUICulture.TextInfo.CultureName;
            ViewBag.calendar = calendarDates;
            ViewBag.visibleDateStart = calendarDates.Any() != false ? calendarDates.First().EventStart.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.visibleDateClose = calendarDates.Any() != false ? calendarDates.Last().EventEnd.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");

            return View(courseVm);
        }

        private List<NoteDetail> GetCalendarDates(IEnumerable<CalendarDetail> calendarDetail)
        {
            List<NoteDetail> calendarEvents = new List<NoteDetail>();
            if (calendarDetail.Any())
            {
                for (int i = 0; i < calendarDetail.Count(); i++)
                {
                    calendarEvents.Add(new NoteDetail
                    {
                        id = i + 1,
                        EventStart = calendarDetail.ElementAt(i).Date,
                        EventEnd = calendarDetail.ElementAt(i).Date,
                        backColor = GetUserColor(),
                        text = string.Empty
                    });
                }
            }

            return calendarEvents;
        }


        public JsonResult GetDateInformation(int courseId, DateTime date) {
            CalendarManager calendarManager = new CalendarManager(new CalendarDao());
            var result = calendarManager.GetDateInformation(courseId, date);
         
            if (datesToRemove.IndexOf(date) > -1)
                datesToRemove.Remove(date);

            datesToRemove.Add(date);


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisabledCourseDay(int courseId, DateTime date, string observation) {
            int i = courseManager.DisableCourseDate(courseId, date, observation);

            return Json(i, JsonRequestBehavior.AllowGet);
        }

        //POST: Course/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseViewModel vm, string calendarEvents, string eventsRemoved)
        {
            CourseLocationManager managerLocation = new CourseLocationManager(new CourseLocationDao());
            var locations = managerLocation.GetCourseLocations();
            vm.LocationsList = locations.ToList();

            CourseLevelManager managerLevel = new CourseLevelManager(new CourseLevelDao());
            var levels = managerLevel.GetCourseLevels();
            vm.LevelsList = levels.ToList();

            CourseTypeManager courseType = new CourseTypeManager(new CourseTypeDao());
            var types = courseType.GetCourseTypes();
            vm.CourseTypeList = types.ToList();

            FormativeEntityManager formativeEntity = new FormativeEntityManager(new FormativeEntityDao());
            var entities = formativeEntity.GetFormativeEntities();
            vm.FormativeEntitiesList = entities.ToList();

            TeacherManager teacherManager = new TeacherManager(new TeacherDao());
            HIQTraining.Model.Teacher teacher = teacherManager.GetTeacherById(vm.TeacherId.Value);

            if (vm.StartHour.CompareTo(vm.EndHour) >= 0)
            {
                ModelState.AddModelError("InvalidHour", "Hora de inicio tem de ser menor que a hora de fim.");
            }

            /*    if (vm.CourseCalendar == null || vm.CourseCalendar == string.Empty)
                {
                     ModelState.AddModelError(string.Empty, HIQResources.errorMessageRequired);          
                    return View(vm);
                }
             */
            ModelState.Remove("CourseCalendar");

            if (ModelState.IsValid) {
                
                CalendarManager calendarManager = new CalendarManager(new CalendarDao());
                HIQTraining.Model.Course course = new HIQTraining.Model.Course();

                course.Id = vm.Id;
                course.Name = vm.Name;
                course.LevelId = vm.SelectedLevelId;
                course.Code = vm.Code;
                course.LocationId = vm.SelectedLocationId;
                course.TeacherId = vm.TeacherId.Value;
                course.FormativeEntityId = vm.EntityFormativeId;
                course.Observation = vm.Observation;
                course.StartDate = vm.StartDate;
                course.StartHour = vm.StartHour;
                course.EndHour = vm.EndHour;
                course.CourseTypeId = vm.SelectedCourseTypeId;
                course.Effort = vm.Effort;
                course.Teacher = teacher;
                course.PayRoll = vm.PayRoll;
               // course.Calendars = vm.CourseCalendar;
                course.CanceledObservation = vm.CanceledObservation;

                if(vm.IsFinished)
                {
                    course.Status = 2;
                }

                if (vm.IsCanceled)
                {
                    course.Status = 3;
                }
                if (vm.isDtp)
                {
                    course.Status = 2;
                    course.Dtp = 1;


                }

                string datesRemoved = eventsRemoved.ToString().Replace(@"/Date(", "\\/Date(").Replace(@")/", ")\\/");
                dynamic calendarResults = JsonConvert.DeserializeObject<dynamic>(datesRemoved);

                if (eventsRemoved != string.Empty)
                {
                    datesToRemove = GetDatesListFromCalendar(calendarResults);
                    if (datesToRemove.Count() > 0)
                    {
                        foreach (var item in datesToRemove)
                        {
                            courseManager.DisableCourseDate(course.Id, item, null);
                        }

                        datesToRemove.Clear();
                    }
                }

                if (!string.IsNullOrEmpty(calendarEvents))
                {
                    calendarEvents = calendarEvents.Replace(@"/Date(", "\\/Date(").Replace(@")/", ")\\/");
                    calendarResults = JsonConvert.DeserializeObject<dynamic>(calendarEvents);
                    string calendarDates = GetDatesFromCalendar(calendarResults);

                    if (calendarDates != string.Empty)
                    {
                        List<CalendarDetail> cal = calendarManager.GetCalendarList(calendarDates);
                        JavaScriptSerializer objJavascript = new JavaScriptSerializer();
                        string ocupiedDate = String.Empty;
                        bool ocupiedTeacher = false;

                        foreach (CalendarDetail item in cal)
                        {
                            if (courseManager.ValidateDate(course, item.Date))
                            {
                                ocupiedTeacher = true;
                                ocupiedDate = item.Date.ToShortDateString().ToString();
                                break;
                            }
                        }

                        if (ocupiedTeacher)
                        {
                            ModelState.AddModelError("DateOcupied", "Formador Ocupado no dia : " + ocupiedDate + " no horario " + course.StartHour + " - " + course.EndHour);

                            ViewBag.backColor = GetUserColor();
                            ViewBag.culture = Thread.CurrentThread.CurrentUICulture.TextInfo.CultureName;
                            ViewBag.calendar = objJavascript.Deserialize<List<NoteDetail>>(calendarEvents);
                            ViewBag.visibleDateStart = cal.First().Date.ToString("yyyy-MM-dd");
                            return View(vm);
                        }

                        course.Calendars = (mapper.CalendarMapper(calendarManager.GetCalendarList(calendarDates), base.GetLoggedUser()));
                    }
                }

                int updateResult = courseManager.Save(course, base.GetLoggedUser());
                if(updateResult > 0) {

                    vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                    TempData["alert"] = vm.Alert;

                    return RedirectToAction("Index", new { message = @HIQResources.SuccessUpdated });
                } else {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
                }
            }
            
            return View(vm);
        }

        private List<DateTime> GetDatesListFromCalendar(dynamic calendarResults)
        {
            List<DateTime> datesToCalendar = new List<DateTime>();
            foreach (var calendar in calendarResults)
            {
                DateTime startDate = calendar.start;
                DateTime endDate = calendar.end;

                if (startDate == endDate)
                {
                    datesToCalendar.Add(startDate);
                }
                else
                {
                    for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1))
                    {
                        datesToCalendar.Add(dt);
                    }
                }
            }
            return datesToCalendar;
        }

        [HttpGet]
        public JsonResult GetCourseDates(int id) {
            CalendarManager manager = new CalendarManager(new CalendarDao());
            var dates = manager.GetCourseCalendar(id);

            return Json(dates, JsonRequestBehavior.AllowGet);
        }

        //POST: Course/Delete
        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            string result = null;
            try
            {
                if (id == null)
                {
                    result = HIQResources.errorMessageUnknownAction;
                    return new JsonResult { Data = result };
                }

                CourseViewModel vm = new CourseViewModel();



                CourseFullDetail courseDetail = courseManager.GetCourseById(id.Value);
                if(courseDetail == null)
                {

                    result = HIQResources.errorMessageUnknownRecord;
                    return new JsonResult { Data = result };
                }

                InscriptionManager inscriptionManager = new InscriptionManager(new InscriptionDao());
                List<InscriptionDetail> courseInscriptions = inscriptionManager.GetCourseInscriptionList(id.Value);

                CalendarManager calendarManager = new CalendarManager(new CalendarDao());
                IEnumerable<CalendarDetail> courseDates = calendarManager.GetCourseCalendar(id.Value);


                if (HasNoActiveInscriptions(courseInscriptions) && HasNoAttendances(calendarManager, courseDates, id.Value))
                {
                    foreach (InscriptionDetail inscription in courseInscriptions)
                    {
                        inscriptionManager.Delete(inscription.Id);
                    }

                    int datesDeleteResult = calendarManager.DeleteCoursesCalendar(id.Value);
                    if (calendarManager.CourseHasCalendarEntries(id.Value) || courseInscriptions.Count > 0)
                    {
                        vm.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);
                        TempData["alert"] = vm.Alert;

                        result = HIQResources.errorMessageUnableToExecuteOperation;
                        return new JsonResult { Data = result };
                        //   return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
                    }
                    else
                    {
                        int deleteResult = courseManager.Delete(id.Value);
                        if (deleteResult == 1)
                        {
                            vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                            TempData["alert"] = vm.Alert;

                            result = HIQResources.messageOperationSuccess;
                            return new JsonResult { Data = result };

                            //     return RedirectToAction("Index", new { message = HIQResources.SuccessDeleted });
                        }
                        else
                        {
                            vm.Alert.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);
                            TempData["alert"] = vm.Alert;

                            result = HIQResources.errorMessageUnableToExecuteOperation;
                            return new JsonResult { Data = result };
                            // return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
                        }
                    }
                }
                else
                {
                    vm.Alert.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);
                    TempData["alert"] = vm.Alert;

                    result = HIQResources.errorMessageUnableToExecuteOperation;
                    return new JsonResult { Data = result };
                }
            }
            catch (DbUpdateException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.Course, ex.Message, ex.StackTrace, base.GetLoggedUser());

                result = HIQResources.errorMessageUnableToDeleteRecord;
                return new JsonResult { Data = result };
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Course, ex.Message, ex.StackTrace, base.GetLoggedUser());

                result = HIQResources.errorMessageExceptionOccurred;
                return new JsonResult { Data = result };
            }
        }

        //Checks for course dependencies in which case the course cannot be removed
        private bool HasNoActiveInscriptions(List<InscriptionDetail> courseInscriptions)
        {
            if (courseInscriptions.Count > 0)
            {
                foreach (InscriptionDetail inscription in courseInscriptions)
                {
                    if (inscription.Status == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool HasNoAttendances(CalendarManager calendarManager, IEnumerable<CalendarDetail> dates, int courseId)
        {
            foreach (CalendarDetail date in dates)
            {
                if (calendarManager.GetCalendarAttendance(courseId, date.Date) > 0) return false;
            }

            return true;
        }

        [HttpGet]
        public ActionResult GetCoursesByStudent(int? id, int page = 1) {
            if(!id.HasValue)
                return Json(null, JsonRequestBehavior.AllowGet);

            var result = courseManager.GetCoursesByStudentId(id.Value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCoursesByTeacher(int? id, int page = 1)
        {
            if (!id.HasValue)
                return Json(null, JsonRequestBehavior.AllowGet);
        
            var result = courseManager.GetCoursesByTeacherId(id.Value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetCourseById(int? id)
        {
            if (!id.HasValue)
                return Json(null, JsonRequestBehavior.AllowGet);

            var result = courseManager.GetCourseById(id.Value);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }

}
