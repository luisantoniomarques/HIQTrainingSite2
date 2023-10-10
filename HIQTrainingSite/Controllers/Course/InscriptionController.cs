using HIQTraining.ActiveDirectory;
using HIQTraining.Business.Company;
using HIQTraining.Business.Configuration;
using HIQTraining.Business.Course;
using HIQTraining.Business.Inscription;
using HIQTraining.Business.Log;
using HIQTraining.Business.Student;
using HIQTraining.DAL.Company;
using HIQTraining.DAL.Configuration;
using HIQTraining.DAL.Course;
using HIQTraining.DAL.Inscription;
using HIQTraining.DAL.Student;
using HIQTraining.Exceptions;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using HIQTrainingSite.Common;
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
    public class InscriptionController : BaseController
    {
        InscriptionTypeManager inscriptionTypeManager;
        InscriptionManager inscriptionManager;
        CourseManager courseManager;
        StudentManager studentManager;
        CompanyManager companyManager;
        CompanyUser companyUser;
        int currentCourseId;


        public InscriptionController()
        {
            inscriptionManager = new InscriptionManager(new InscriptionDao());
            courseManager = new CourseManager(new CourseDao());
            inscriptionTypeManager = new InscriptionTypeManager(new InscriptionTypeDao());
            companyManager = new CompanyManager(new CompanyDao());
            studentManager = new StudentManager(new StudentDao());
            companyUser = new CompanyUser();
        }


        // GET: Inscription
        [HttpGet]
        public ActionResult Index(int? courseId, string message, string createStudentByInscription)
        {

            if (courseId == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownAction });
            }

            if (!string.IsNullOrWhiteSpace(message))
                ModelState.AddModelError(string.Empty, message);


            InscriptionListViewModel model = new InscriptionListViewModel();

            var course = courseManager.GetCourseById(courseId.Value);
            if (course == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            model.CourseId = course.Id;
            model.CourseName = course.Name;
            currentCourseId = course.Id;
            model.IsCourseFinished = (course.IsCanceled || course.IsFinished) ? true : false;

            model.InscriptionList = inscriptionManager.GetCourseInscriptionList(courseId.Value);

            if (TempData["alert"] != null)
            {
                model.Alert = (AlertViewModel)TempData["alert"];
            }


            return View(model);
        }

        // POST: Inscription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(InscriptionListViewModel viewModel)
        {
            try
            {
                if (TempData["alert"] != null)
                {
                    viewModel.Alert = (AlertViewModel)TempData["alert"];
                }
                if (ModelState.IsValid)
                {
                    int result = inscriptionManager.SaveInscriptionStatus(viewModel.InscriptionList, base.GetLoggedUser());
                    viewModel.SetSuccessMessage(HIQResources.messageOperationSuccess);
                    return View("Index", viewModel);
                }
            }
            catch (DbUpdateException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                viewModel.SetErrorMessage(HIQResources.errorMessageDuplicateRecord);
                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                viewModel.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);
                return View("Index", viewModel);
            }

            return View("Index", viewModel);
        }

  

        // GET: Inscription/Create
        [HttpGet]
        public ActionResult Create(int? courseId, string duplicateName, bool? isExternal, string Name, string Email, int? companyId)
        {
            //criar formando atraves de uma inscricao e se houver um nome duplicado
            if (duplicateName == "true")
            {
                ViewBag.OpenModal = "OpenModal";
                ViewBag.AddModelError = "DuplicateName";
                if (isExternal != null && isExternal == true)
                {
                    ViewBag.statusChekedboxInternal = "";
                    ViewBag.statusChekedboxExternal = "checked";
                    ViewBag.statusChekedboxInternal2 = "";
                    ViewBag.statusChekedboxExternal2 = "checked";
                }
                else
                {
                    ViewBag.statusChekedboxInternal = "checked";
                    ViewBag.statusChekedboxExternal = "";
                    ViewBag.statusChekedboxInternal2 = "checked";
                    ViewBag.statusChekedboxExternal2 = "";
                }

            }
            else if(duplicateName == "false")
            {
                HIQTraining.Model.Student getidStudent = studentManager.GetStudentByNameAndEmail(Name,Email);

                StudentViewModel student = new StudentViewModel();

                student.Name = Name;
                student.Email = Email;

     
                if (isExternal != null && isExternal == false)
                {
                    ViewBag.statusChekedboxInternal = "checked";
                    ViewBag.statusChekedboxExternal = "";
                    ViewBag.statusChekedboxInternal2 = "checked";
                    ViewBag.statusChekedboxExternal2 = "";
                }
                else
                {
                    ViewBag.statusChekedboxInternal = "";
                    ViewBag.statusChekedboxExternal = "checked";
                    ViewBag.statusChekedboxInternal2 = "";
                    ViewBag.statusChekedboxExternal2 = "checked";
                }
            }

            if (courseId == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownAction });
            }

            InscriptionViewModel model = new InscriptionViewModel();

            var course = courseManager.GetCourseById(courseId.Value);
            if (course == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            model.CourseId = courseId.Value;
            model.CourseName = course.Name;

            model.InscriptionTypes.InscriptionTypeDetailList = inscriptionTypeManager.GetInscriptionTypes();
            model.Student.Companies.CompanyDetailList = companyManager.GetCompanies();

            ViewBag.ValueCompany = companyId;
            ViewBag.typeCompany = isExternal;


            return View(model);
        }

        // POST: Inscription/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InscriptionViewModel viewModel)
        {
            try
            {
              
                if (ModelState.IsValid)
                {
                    HIQTraining.Model.Student student = studentManager.GetStudentByNameAndEmail(viewModel.Student.Name, viewModel.Student.Email);

                 if (student == null)
                    {
                        viewModel.Alert.SetErrorMessage(HIQResources.errorMessageStudentNotFound);
                        return View(viewModel);
                    }
                         

                    // student = studentManager.AddStudent(viewModel.Student.Name, viewModel.Student.Email, viewModel.Student.CompanyId, viewModel.Student.PhoneNumber, viewModel.Observation, base.GetLoggedUser());


                    Inscription inscription = new Inscription();
                    inscription.StudentId = student.Id;
                    inscription.CourseId = viewModel.CourseId;
                    inscription.TypeId = viewModel.TypeId;
                    inscription.Status = InscriptionStatus.GetEnrolledStatusId();
                    inscription.Observation = viewModel.Student.Observation;

                   var newInscription =  inscriptionManager.AddInscription(inscription, viewModel.Student.CompanyId, viewModel.Student.Name, viewModel.Student.Email, base.GetLoggedUser());
                    
                   

                    InscriptionListViewModel model = new InscriptionListViewModel();

                    var course = courseManager.GetCourseById(inscription.CourseId);
                    if (course == null)
                    {
                        return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownRecord });
                    }

                    model.CourseId = course.Id;
                    model.CourseName = course.Name;

                    model.InscriptionList = inscriptionManager.GetCourseInscriptionList(inscription.CourseId);

                    if (newInscription != null) {

                   // viewModel.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);

                        model.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = model.Alert;

                  }
                    return View("Index", model);
                }

            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                viewModel.Alert.SetErrorMessage(ex.Message);
                TempData["alert"] = viewModel.Alert;

            }
        
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                Console.WriteLine(ex.Message);
                viewModel.Alert.SetErrorMessage(HIQTrainingSite.HIQResources.errorMessageExceptionOccurred);
            }
           

            viewModel.InscriptionTypes.InscriptionTypeDetailList = inscriptionTypeManager.GetInscriptionTypes();
            viewModel.Student.Companies.CompanyDetailList = companyManager.GetCompanies();


           
            return View(viewModel);
        }

        //public ActionResult SearchUsersByName(int companyId, string userName)
        //{
        //    CompanyUser companyUser = new CompanyUser();
        //    List<ADUserDetail> userList = companyUser.SearchUsersByName(companyId, userName);
        //    return Json(userList, JsonRequestBehavior.AllowGet);
        //}




        [HttpPost]
        public JsonResult StudentIsInscripted(string name, string mail)
        {
            HIQTraining.Model.Student student = studentManager.GetStudentByNameAndEmail(name, mail);

            string course = String.Empty;
            bool StudentInscripted = false;

            if (student != null)
            {
                List<InscriptionDetail> StudentInscriptions = inscriptionManager.GetInscriptionByStudentId(student.Id, currentCourseId);

                if (StudentInscriptions.Count > 0)
                {
                    course = courseManager.GetCourseById(StudentInscriptions[0].CourseId).Name;
                    StudentInscripted = true;

                }
            }


            return Json(new { StudentInscripted = StudentInscripted, course = course });
        }

        // POST: Inscription/Delete/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete( int? id)
        {
            try
            {
                if ( id == null)
                {
                    return RedirectToAction("Index", "Inscription", new { courseId = 4, message = @HIQResources.errorMessageUnknownRecord });
                }


                int result = inscriptionManager.Delete(id.Value);
                if (result == 1) {
                    AlertViewModel alert = new AlertViewModel();
                    alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                    TempData["alert"] = alert;
                    return RedirectToAction("Index");
                }   
                else
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
            }
            catch (DbUpdateException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToDeleteRecord });
            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new {  message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }
        }

        /// <summary>
        /// searches a student by name and company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchUsersByName(int companyId, string userName)
        {
            List<ADUserDetail> userList = companyUser.SearchUsersByName(companyId, userName);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchExternalUsersByName(string userName)
        {
            var userList = studentManager.SearchExternalUsersByName(userName).ToList();
            return Json(userList, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CreateExtern(int? courseId)
        {
            if (courseId == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownAction });
            }

            InscriptionViewModel model = new InscriptionViewModel();

            var course = courseManager.GetCourseById(courseId.Value);
            if (course == null)
            {
                return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            model.CourseId = courseId.Value;
            model.CourseName = course.Name;

            model.InscriptionTypes.InscriptionTypeDetailList = inscriptionTypeManager.GetInscriptionTypes();
            model.Student.Companies.CompanyDetailList = companyManager.GetCompanies();

            return View(model);
        }

        // POST: Inscription/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateExtern(InscriptionViewModel viewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    HIQTraining.Model.Student student = studentManager.GetStudentByNameAndEmail(viewModel.Student.Name, viewModel.Student.Email);

                    if (student == null)
                    {
                        viewModel.Alert.SetErrorMessage(HIQResources.errorMessageStudentNotFound);
                        return View(viewModel);
                    }

                    Inscription inscription = new Inscription();
                    inscription.StudentId = student.Id;
                    inscription.CourseId = viewModel.CourseId;
                    inscription.TypeId = viewModel.TypeId;
                    inscription.Status = InscriptionStatus.GetEnrolledStatusId();
                    inscription.Observation = viewModel.Student.Observation;

                    var newInscription = inscriptionManager.AddInscription(inscription, viewModel.Student.CompanyId, viewModel.Student.Name, viewModel.Student.Email, base.GetLoggedUser());



                    InscriptionListViewModel model = new InscriptionListViewModel();

                    var course = courseManager.GetCourseById(inscription.CourseId);
                    if (course == null)
                    {
                        return RedirectToAction("Index", "Course", new { message = @HIQResources.errorMessageUnknownRecord });
                    }

                    model.CourseId = course.Id;
                    model.CourseName = course.Name;

                    model.InscriptionList = inscriptionManager.GetCourseInscriptionList(inscription.CourseId);

                    if (newInscription != null)
                    {

                        // viewModel.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);

                        model.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = model.Alert;
                    }
                    return View("Index", model);
                }

            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                viewModel.Alert.SetErrorMessage(ex.Message);
                TempData["alert"] = viewModel.Alert;

            }

            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                Console.WriteLine(ex.Message);
                viewModel.Alert.SetErrorMessage(HIQTrainingSite.HIQResources.errorMessageExceptionOccurred);
            }


            viewModel.InscriptionTypes.InscriptionTypeDetailList = inscriptionTypeManager.GetInscriptionTypes();
            viewModel.Student.Companies.CompanyDetailList = companyManager.GetCompanies();



            return View(viewModel);
        }

        [HttpPost]
        public JsonResult SearchUsersByName(string userName, int page = 1)
        {
           
            var users = studentManager.GetPagedStudentsByName(userName, page, HIQSiteConstants.General.PAGE_SIZE);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStudentByName(string studentName, string companyId)
        {
            StudentManager manager = new StudentManager(new StudentDao());
            int convertcompanyId = Int32.Parse(companyId);
            var student = manager.GetStudents(studentName, convertcompanyId);

            return Json(student, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreatePartialViewStudent(string param, string courseId, bool erroState = false)
        {

            StudentViewModel vm = new StudentViewModel();

            vm.CompaniesListExternal = companyManager.GetExternalCompanies();
            vm.Companies.CompanyDetailList = companyManager.GetCompanies();
            vm.isExternal = false;
            vm.createStudentByInscription = true;

          
            if (erroState)
            {
                ModelState.AddModelError("NameStudent", HIQResources.errorMessageDuplicateName);
            }

            return PartialView(vm);
        }

        public void SaveTempData()
        {
            InscriptionViewModel vm = new InscriptionViewModel();

          //  vm.Se = Request["SelectedTypeId"]


        }

    }
}