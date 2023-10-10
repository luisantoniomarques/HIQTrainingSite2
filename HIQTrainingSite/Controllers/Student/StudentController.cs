using HIQTraining.ActiveDirectory;
using HIQTraining.Business.Company;
using HIQTraining.Business.Student;
using HIQTraining.DAL.Company;
using HIQTraining.DAL.Student;
using HIQTraining.Exceptions;
using HIQTraining.ModelDetail;
using HIQTrainingSite.Common;
using HIQTrainingSite.ViewModel;
using HIQTraining.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Infrastructure;
using HIQTraining.Business.Log;
using System.Data.SqlClient;
using System.Data;
//using System.Drawing;





namespace HIQTrainingSite.Controllers.Student
{
    [Authorize]
    public class StudentController : BaseController
    {
        StudentManager studentManager;
        CompanyManager companyManager;
        CompanyUser companyUser;
       
        public StudentController()
        {
            companyManager = new CompanyManager(new CompanyDao());
            studentManager = new StudentManager(new StudentDao());
            companyUser = new CompanyUser();
        }

        /// <summary>
        /// searches a student by name and company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SearchUsersByName(int companyId, string userName, string createPartialStudent, string duplicateName, string isExternal)
        {
            List<ADUserDetail> userList = companyUser.SearchUsersByName(companyId, userName);
            return Json(userList, JsonRequestBehavior.AllowGet);
        }


        // GET: Student
        [HttpGet]
        public ActionResult Index(string message, int page = 1)
        {
            //if (!string.IsNullOrWhiteSpace(message))
            //    ModelState.AddModelError(string.Empty, message);

         


            StudentListViewModel viewModel = new StudentListViewModel();
            //ModelState.Clear();

            if (TempData["alert"] != null)
            {
                viewModel.Alert = (AlertViewModel)TempData["alert"];
            }
            else if (!string.IsNullOrEmpty(message))
            {
                viewModel.Alert.SetErrorMessage(message);
            }

            if (User.IsInRole("Admin") || User.IsInRole("Staff"))
            {
                ViewBag.BlockOrNone = "";
            }
            else
            {
                ViewBag.BlockOrNone = "display:none";
            }

            var students = studentManager.GetPagedStudents(page, HIQSiteConstants.General.PAGE_SIZE);
            viewModel.Search.Companies.CompanyDetailList = GetCompanyListForSearch();
            viewModel.StudentList = students;
            return View(viewModel);
        }

        /// <summary>
        /// Returns all companies
        /// </summary>
        /// <returns></returns>
        private List<CompanyDetail> GetCompanyListForSearch()
        {
            var list = companyManager.GetCompanies();
            list.Insert(0, new HIQTraining.ModelDetail.CompanyDetail() { Id = -1, Name = HIQResources.dropdownAllOptions });
            return list;
        }

        /// <summary>
        /// returns a partial view for the student search
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selectedCompanyId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ActionResult SearchStudent(string name, int? selectedCompanyId, int? status)
        {
            StudentListViewModel viewModel = this.SearchStudents(name, selectedCompanyId, status, 1);
            return PartialView("_StudentList", viewModel);
        }


        // GET: Student/Create
        [Authorize(Roles = "Admin, Staff")]
        [HttpGet]
        public ActionResult Create(string returnUrl, bool isExternal = false)
        {
            StudentViewModel vm = new StudentViewModel();
            vm.isExternal = isExternal;
            return CreateStudent(returnUrl, isExternal);
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentViewModel vm, int? courseId ,string returnUrl, bool isExternal) {
            try {

                CompanyManager companyStudent = new CompanyManager(new CompanyDao());
                vm.Companies.CompanyDetailList = isExternal ? companyStudent.GetExternalCompanies() : companyStudent.GetCompanies();

                if (isExternal == false && studentManager.GetPagedStudentsByName(vm.Name, vm.Companies.SelectedCompanyId, vm.SelectedStatusId, 1, HIQSiteConstants.General.PAGE_SIZE).Count > 0)
                {
                    ModelState.AddModelError("Name", HIQResources.errorMessageDuplicateName);
                    ViewBag.statusChekedboxInternal = isExternal ? "" : "checked";
                    ViewBag.statusChekedboxExternal = isExternal ? "checked" : "";
                    if (vm.createStudentByInscription == true)
                    {
                        
                        return RedirectToAction("create", "Inscription", new { createPartialStudent = "true", courseId = courseId, duplicateName =  "true", isExternal = isExternal });
                    }

                    return View(vm);
                }
               
                if(ModelState.IsValid) {

                    HIQTraining.Model.Student student = new HIQTraining.Model.Student();
                    student.CompanyId = vm.Companies.SelectedCompanyId;
                    student.Email = vm.Email;
                    student.Name = vm.Name;
                    student.PhoneNumber = vm.PhoneNumber;
                    student.Observation = vm.Observation;
 
                    HIQTraining.Model.Student newStudent = isExternal ? studentManager.AddStudentExternal(student, base.GetLoggedUser()) : studentManager.AddStudent(vm.Name, vm.Email, vm.Companies.SelectedCompanyId, vm.PhoneNumber, vm.Observation, base.GetLoggedUser());
                   

                    if(!string.IsNullOrEmpty(returnUrl)) {
                        return RedirectToRoute("CreateCertification", new { id = student.Id });
                    }
                
                    if (newStudent != null)
                    {
                       vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                       TempData["alert"] = vm.Alert;
                        if (vm.createStudentByInscription == true)
                        {

                            return RedirectToAction("create", "Inscription" , new { createPartialStudent = "true", courseId = courseId, duplicateName = "false", isExternal = isExternal, Name = vm.Name, Email = vm.Email, companyId = vm.Companies.SelectedCompanyId });
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                
                    }
                 
                    return RedirectToAction("Index");
                }
            }
            catch (HIQTrainingException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Students, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
            catch (Exception ex)
            {
                // TODO: log exception
                ModelState.AddModelError(string.Empty, HIQTrainingSite.HIQResources.errorMessageExceptionOccurred);

                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Students, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }

            vm.Companies.CompanyDetailList = companyManager.GetCompanies();
            return View(vm);
        }
 
        // GET: Student/Details/3
        [HttpGet]
        public ActionResult Details(int? id) {
            return GetStudent(id);
        }

        [Authorize(Roles = "Admin, Staff")]
        // GET: Student/Edit/3
        [HttpGet]
        public ActionResult Edit(int? id) {
            return GetStudent(id);
        }

        // POST: Student/Details/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentViewModel model) {
            if(ModelState.IsValid) {
                int result = studentManager.UpdateStudent(model.Id, model.PhoneNumber, model.Status, model.Observation , base.GetLoggedUser());
                if(result == 1)
                    return RedirectToAction("index");
                else
                    return RedirectToAction("Index", new { message = HIQResources.errorMessageUnableToExecuteOperation });
            }

            return View(model);
        }
  
        private ActionResult CreateStudent(string returnUrl, bool isExternal)
        {
            StudentViewModel vm = new StudentViewModel();

            CompanyManager companyManager = new CompanyManager(new CompanyDao());

            vm.Companies.CompanyDetailList = companyManager.GetExternalCompanies();
            vm.CompaniesList = companyManager.GetCompanies();

            ViewBag.ReturnUrl = returnUrl;

            return View(vm);
        }



        /// <summary>
        /// Get student details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ActionResult GetStudent(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            StudentViewModel model = new StudentViewModel();

            try {
                StudentDetail student = studentManager.GetStudentDetailById(id.Value);
                if(student == null) {
                    return RedirectToAction("Index", new { Message = HIQResources.errorMessageStudentNotFound });
                }

                model.Id = student.Id;
                model.Name = student.Name;
                model.Email = student.Email;
                model.PhoneNumber = student.PhoneNumber;
                model.CompanyId = student.CompanyId;
                model.CompanyName = student.CompanyName;
                model.Status = student.Status;
                model.SelectedStatusId = student.Status;
                model.StatusDescription = student.StatusDescription;
                model.Observation = student.Observation;
                model.UserCreated = student.UserCreated;
                model.CreatedDate = student.CreatedDate.HasValue ? student.CreatedDate.Value : default(DateTime);
                model.UserUpdated = student.UserUpdated;
                model.UpdateDate = student.UpdateDate;
            } catch(HIQTrainingException ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
            } catch(Exception ex) {
                // TODO: log exception
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, HIQTrainingSite.HIQResources.errorMessageExceptionOccurred);
            }

            return View(model);
        }


        /// <summary>
        /// returns the search results from the student page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="selectedCompanyId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private StudentListViewModel SearchStudents(string name, int? selectedCompanyId, int? status, int page = 1)
        {
            StudentListViewModel viewModel = new StudentListViewModel();
            if (ModelState.IsValid)
            {
                if (selectedCompanyId.HasValue && selectedCompanyId == -1)
                    selectedCompanyId = null;
                if (status.HasValue && status == -1)
                    status = null;
                viewModel.Search = new StudentSearchViewModel(name, selectedCompanyId, status);
                viewModel.StudentList = studentManager.GetStudentsByName(name, selectedCompanyId, status, page, Common.HIQSiteConstants.General.PAGE_SIZE);
            }
            return viewModel;
        }

        [Authorize(Roles = "Admin, Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Student/Delete/3
        public ActionResult Delete(int? id)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=192.168.0.80\BEEPIDEV;Initial Catalog=HIQTraining;User ID=sa_dev;Password=sa_dev;");
            SqlCommand cmd = new SqlCommand("DECLARE @value VARCHAR(50) = (SELECT Id " +
                                            " FROM[dbo].[Inscriptions] " +
                                            " WHERE StudentId =  '"+ id +"'); " +
                                           " DELETE FROM[dbo].[Inscriptions] "+
                                            " WHERE StudentId = '" + id + "' " +
                                           " DELETE FROM[dbo].[Attendances] " +
                                           " where InscriptionId = @value " +
                                          " DELETE FROM[dbo].Certifications " +
                                          "  Where StudentId = '" + id + "' " +
                                           " DELETE FROM[dbo].Students " +
                                           " WHERE Id = '" + id + "' ", cn);
            cn.Open();
           cmd.ExecuteNonQuery();

            return new JsonResult { Data = cmd };


            /*
             string result = null;
            try
            {
                if (id == null)
                {
                    result = HIQResources.errorMessageUnknownAction;
                    return new JsonResult { Data = result };
                }

                StudentViewModel vm = new StudentViewModel();
                StudentDetail studentDetail = studentManager.GetStudentDetailById(id.Value);
                if (studentDetail == null)
                {
                    result = HIQResources.errorMessageUnknownRecord;
                    return new JsonResult { Data = result };
                }


                    int deleteResult = studentManager.Delete(id.Value);

                    if (deleteResult == 1)
                    {
                        vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = vm.Alert;

                        result = HIQResources.messageOperationSuccess;
                        return new JsonResult { Data = result };
                    }

                    vm.Alert.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);
                    TempData["alert"] = vm.Alert;


                    result = HIQResources.errorMessageUnableToExecuteOperation;
                    return new JsonResult { Data = result };
                

            }
            catch (DbUpdateException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.Teacher, ex.Message, ex.StackTrace, base.GetLoggedUser());

                result = HIQResources.errorMessageUnableToDeleteRecord;
                return new JsonResult { Data = result };
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());

                result = HIQResources.errorMessageExceptionOccurred;
                return new JsonResult { Data = result };
            }
             */
        }

       




        public JsonResult SearchExternalCompany(bool external)
        {
            StudentViewModel vm = new StudentViewModel();
            var result = vm.CompaniesList = external ? companyManager.GetExternalCompanies() : companyManager.GetCompanies();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}