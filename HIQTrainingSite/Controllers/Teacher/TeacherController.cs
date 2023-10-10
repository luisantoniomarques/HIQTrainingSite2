using HIQTraining.ActiveDirectory;
using HIQTraining.Business.Company;
using HIQTraining.Business.Course;
using HIQTraining.Business.Log;
using HIQTraining.Business.Teacher;
using HIQTraining.DAL.Company;
using HIQTraining.DAL.Course;
using HIQTraining.DAL.Teacher;
using HIQTraining.Exceptions;
using HIQTraining.ModelDetail;
using HIQTrainingSite.Common;
using HIQTrainingSite.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.Controllers.Teacher {

    [Authorize]
    public class TeacherController : BaseController {

        private TeacherManager teacherManager;
        CompanyManager companyManager;
        private CourseManager courseManager;

        public TeacherController() {
            teacherManager = new TeacherManager(new TeacherDao());
            companyManager = new CompanyManager(new CompanyDao());
            courseManager = new CourseManager(new CourseDao());
        }

        // GET: Teacher
        [HttpGet]
        public ActionResult Index(AlertViewModel alert, string message, int page = 1)
        {
            TeacherListViewModel vm = new TeacherListViewModel();

            if(TempData["alert"] != null) {
                vm.Alert = (AlertViewModel)TempData["alert"];
            } else if(!string.IsNullOrEmpty(message)) {
                vm.Alert.SetErrorMessage(message);
            }
            if (User.IsInRole("Admin") || User.IsInRole("Staff"))
            {
                ViewBag.BlockOrNone = "";
            }
            else
            {
                ViewBag.BlockOrNone = "display:none";
            }

            vm.TeacherList = teacherManager.GetPagedTeachers(page, HIQSiteConstants.General.PAGE_SIZE);
            vm.Search.Companies.CompanyDetailList = GetCompanyListForSearch();

            return View(vm);
        }

        /// <summary>
        /// Gets all companies
        /// </summary>
        /// <returns></returns>
        private List<CompanyDetail> GetCompanyListForSearch() {
            var list = companyManager.GetCompanies();
            list.Insert(0, new CompanyDetail() { Id = -1, Name = HIQResources.dropdownAllOptions });
            return list;
        }

        /// <summary>
        /// returns the partial view for the teacher search
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult SearchTeacher(string name, int? companyId, int? status) {
            TeacherListViewModel vm = this.SearchTeachers(name, companyId, status);
            
            return PartialView("_TeacherList", vm);
        }

        /// <summary>
        /// returns a list with all teachers 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public TeacherListViewModel SearchTeachers(string name, int? companyId, int? state, int page = 1) {
            TeacherListViewModel vm = new TeacherListViewModel();
            vm.TeacherList = teacherManager.GetTeachersSearchResults(name, companyId, state, page, HIQSiteConstants.General.PAGE_SIZE);
            
            return vm;
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpGet]
        // GET: Teacher/Create
        public ActionResult Create(string returnUrl, bool isExternal = false, bool createTeacherByCourses = false) {

            TeacherViewModel vm = new TeacherViewModel();
            vm.isExternal = isExternal;
            vm.createTeacherBycourses = createTeacherByCourses;

            return CreateTeacher(returnUrl, isExternal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Teacher/Create
        public ActionResult Create(TeacherViewModel vm,string returnUrl, bool isExternal)
        {
            return CreateTeacher(vm, returnUrl, isExternal);
        }

        private ActionResult CreateTeacher(TeacherViewModel vm, string returnUrl,bool isExternal)
        {
            try
            {
                CompanyManager companyTeacher = new CompanyManager(new CompanyDao());

                vm.CompaniesList = isExternal ? companyTeacher.GetExternalCompanies() : companyTeacher.GetCompanies();

                if (isExternal == false && teacherManager.GetTeachersSearchResults(vm.Name, vm.SelectedCompanyId, vm.StatusId, 1, HIQSiteConstants.General.PAGE_SIZE).Count > 0)
                {  
                    ViewBag.statusChekedboxInternal = isExternal ? "" : "checked";
                    ViewBag.statusChekedboxExternal = isExternal ? "checked" : "";

                    TempData["statusChekedboxInternal"] = isExternal ? "" : "checked";
                    TempData["statusChekedboxExternal"] = isExternal ? "checked" : "";

                    if (vm.createTeacherBycourses == true)
                    {
                      TempData["TeacherErrorDuplicateName"] = "Error";

                         return RedirectToAction("Create", "Course");
                      // return View(vm);
                    }
                  
                     
                   
                    
                }
           
                if (ModelState.IsValid)
                {
                    HIQTraining.Model.Teacher teacher = new HIQTraining.Model.Teacher();
                    teacher.Name = vm.Name;
                    teacher.CompanyId = vm.SelectedCompanyId;
                    teacher.Email = vm.Email;
                    teacher.PhoneNumber = vm.PhoneNumber;
                    teacher.TecnicalSkills = vm.TecnicalSkill;
                  
              
  
                    HIQTraining.Model.Teacher newTeacher = isExternal ? teacherManager.AddTeacherExternal(teacher, base.GetLoggedUser()) : teacherManager.AddTeacher(teacher, base.GetLoggedUser());

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        //validar o URL
                        //  return RedirectToRoute("CreateCourse", new { teacherId = newTeacher.Id });
                        return RedirectToAction("Create", "Course", new { teacherId = newTeacher.Id });
                    }

                    if (teacher != null && vm.createTeacherBycourses  == false)
                    {
                        vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = vm.Alert;

                        return RedirectToAction("Index", vm.Alert);
                    }else if (teacher != null && vm.createTeacherBycourses == true)
                    {
                        TempData["teacherView"] = vm;
                        return RedirectToAction("Create", "Course");                     
                    }

                    
                }
            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }

            return View(vm);
        }

        private ActionResult CreateTeacher(string returnUrl, bool isExternal)
        {
            TeacherViewModel vm = new TeacherViewModel();

            CompanyManager companyManager = new CompanyManager(new CompanyDao());

            //vm.CompaniesList = isExternal ? companyManager.GetExternalCompanies() : companyManager.GetCompanies();

            vm.CompaniesListExternal = companyManager.GetExternalCompanies();
            vm.CompaniesList = companyManager.GetCompanies();
            vm.isExternal = isExternal;
          

            return View(vm);
        }

        /// <summary>
        /// search a teacher by name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchUsersByName(int companyId, string userName) {
           CompanyUser companyUser = new CompanyUser();
            List<ADUserDetail> userList = companyUser.SearchUsersByName(companyId, userName);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        // GET: Teacher/Details/3
        [HttpGet]
        public ActionResult Details(int? id) {
            return GetTeacher(id);
        }

        [Authorize(Roles = "Admin, Staff")]
        // GET: Teacher/Edit/3
        [HttpGet]
        public ActionResult Edit(int? id) {
            return GetTeacher(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //POST: Teacher/Edit/3
        public ActionResult Edit(TeacherViewModel vm) {
            try {
                CompanyManager companyManager = new CompanyManager(new CompanyDao());
                vm.CompaniesList = companyManager.GetCompanies();
              
                if(ModelState.IsValid) {

                    HIQTraining.Model.Teacher teacher = new HIQTraining.Model.Teacher();
                    teacher.Id = vm.Id;
                    teacher.Name = vm.Name;
                    teacher.CompanyId = vm.SelectedCompanyId;
                    teacher.Email = vm.Email;
                    teacher.PhoneNumber = vm.PhoneNumber;
                    teacher.Status = vm.StatusId.Value;
                    teacher.TecnicalSkills = vm.TecnicalSkill;
         

                    int result = teacherManager.Update(teacher, base.GetLoggedUser());
                    if(result == 1) {
                        vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = vm.Alert;

                        return RedirectToAction("Index");
                    } else {
                        vm.Alert.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);
                        TempData["alert"] = vm.Alert;

                        return RedirectToAction("Index");
                    }
                }
            } catch(DbUpdateException ex) {
                Log.AddLogRecord(LogManager.LogType.Info, LogManager.LogPriority.Low, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);

                return RedirectToAction("Index");
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        private ActionResult GetTeacher(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            TeacherViewModel vm = new TeacherViewModel();

            CompanyManager companyManager = new CompanyManager(new CompanyDao());
           
            
            TeacherDetail teacherDetail = teacherManager.GetTeacherDetailById(id.Value);

            //TODO: if teacherDetail == null
            vm.SelectedCompanyId = teacherDetail.CompanyId;
            vm.SelectedCompany = teacherDetail.Company;
            vm.Id = teacherDetail.Id;
            vm.Name = teacherDetail.Name;
            vm.Email = teacherDetail.Email;
            vm.PhoneNumber = teacherDetail.Phone;
            vm.StatusId = teacherDetail.StatusId;
            vm.TecnicalSkill = teacherDetail.TecnicalSkills;
            vm.PayRoll = teacherDetail.PayRoll;

            HIQTraining.Model.Company comp = companyManager.GetCompanyById(vm.SelectedCompanyId);
  
            if (comp.External == true)
            {
                vm.CompaniesList = companyManager.GetExternalCompanies();
            }
            else {
                vm.CompaniesList = companyManager.GetCompanies();
            }
               
            return View(vm);
        } 





        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Teacher/Delete/3
        public ActionResult Delete(int? id) {
            string result = null;
            try {
                if(id == null) {
                    result = HIQResources.errorMessageUnknownAction;
                    return new JsonResult { Data = result };
                    //return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownAction });
                }

                TeacherViewModel vm = new TeacherViewModel();
                TeacherDetail teacherDetail = teacherManager.GetTeacherDetailById(id.Value);
                if(teacherDetail == null) {
                    result = HIQResources.errorMessageUnknownRecord;
                    return new JsonResult { Data = result };
                    //return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownRecord });
                }
                
                if(courseManager.GetTeacherNumberOfCourses(id.Value) == 0)
                {
                    int deleteResult = teacherManager.Delete(id.Value);

                    if (deleteResult == 1)
                    {
                        vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = vm.Alert;

                        result = HIQResources.messageOperationSuccess;
                        return new JsonResult { Data = result };
                        // return RedirectToAction("Index");
                    }
                }

                //Both above if's would have this block of code as a else, put it outside to avoid replication
                vm.Alert.SetErrorMessage(HIQResources.errorMessageUnableToDeleteRecord);
                TempData["alert"] = vm.Alert;

                result = HIQResources.errorMessageUnableToDeleteRecord;
                return new JsonResult { Data = result };




            } catch(DbUpdateException ex) {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());

                result = HIQResources.errorMessageUnableToDeleteRecord;
                return new JsonResult { Data = result };

                //return RedirectToAction("Index", new { message = HIQResources.errorMessageUnableToDeleteRecord });
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());


                result = HIQResources.errorMessageExceptionOccurred;
                return new JsonResult { Data = result };

                //return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
        }
        [HttpGet]
        public JsonResult SearchExternalCompany(bool external)
        {
            TeacherViewModel vm = new TeacherViewModel();
            var result = vm.CompaniesList = external ? companyManager.GetExternalCompanies() : companyManager.GetCompanies();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult CreatePartialViewTeacher(string param, bool erroState = false)
        {
            TeacherViewModel vm = new TeacherViewModel();

            vm.CompaniesListExternal = companyManager.GetExternalCompanies();
            vm.CompaniesList = companyManager.GetCompanies();
            vm.isExternal = false;
            vm.createTeacherBycourses = true;

            if (erroState)
            {
                ModelState.AddModelError("NameTeacher", HIQResources.errorMessageDuplicateName);
            }

            return PartialView(vm);
        }

        public void SaveTempData()
        {
            CourseViewModel vm = new CourseViewModel();
         
            vm.Name = Request["Name"];
            vm.SelectedLevelId = Int32.Parse(Request["SelectedLevelId"]);
            vm.SelectedCourseTypeId = Int32.Parse(Request["SelectedCourseTypeId"]);
            vm.Code = Request["Code"];
            vm.SelectedLocationId = Int32.Parse(Request["SelectedLocationId"]);
            vm.EntityFormativeId = Int32.Parse(Request["EntityFormativeId"]);
            vm.StartHour = TimeSpan.Parse(Request["StartHour"]);
            vm.EndHour = TimeSpan.Parse(Request["EndHour"]);
            vm.Effort = Request["Effort"];
            vm.Observation = Request["Observation"];

            TempData["TempDataCourse"] = vm;
            
        }

    }
}