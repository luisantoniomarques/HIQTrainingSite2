using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIQTrainingSite.Helper;
using HIQTraining.Business.Company;
using HIQTraining.DAL.Company;
using HIQTraining.Exceptions;
using Microsoft.AspNet.Identity;
using HIQTraining.ModelDetail;
using HIQTraining.ActiveDirectory;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using HIQTrainingSite.Excel;
using OfficeOpenXml;

namespace HIQTrainingSite.Controllers.Administration
{
    [Authorize]
    public class UserController : BaseController
    {
        IdentityHelper identityManager;
        CompanyManager companyManager;
        CompanyUser companyUser;
        int excelLimitFile;

        public UserController()
        {
            identityManager = new IdentityHelper();
            companyManager = new CompanyManager(new CompanyDao());
            companyUser = new CompanyUser();
            excelLimitFile = 90000;
        }



        // GET: User
        [HttpGet]
        public ActionResult Index(string message, int page = 1 )
        {
            if (!string.IsNullOrWhiteSpace(message))
                ModelState.AddModelError(string.Empty, message);


            UserListViewModel model = new UserListViewModel();
            model.UsersList = identityManager.GetApplicationUsers(page);

            if (TempData["alert"] != null)
            {
                model.Alert = (AlertViewModel)TempData["alert"];
            }



            return View(model);
        }

        [HttpGet]
        public ActionResult Import(string message)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        //POST: User/ImportRes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportRes(FormCollection formCollection)
        {
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName)))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] filebytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(filebytes, 0, Convert.ToInt32(file.ContentLength));

                    var suppleirList = new List<FormaIte>();

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfColumns = workSheet.Dimension.End.Column;
                        var noOfRows = workSheet.Dimension.End.Row;// Here is where my issue is

                        for (int rowIterator = 5; rowIterator < noOfRows; rowIterator++)
                        {
                            var claim = new FormaIte();
                            bool[] cc = new bool[13];
                            int ee = 0;

                            claim.ColaboCode = workSheet.Cells[rowIterator, 1].ToString();
                            claim.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            claim.Corse = workSheet.Cells[rowIterator, 3].Value.ToString();
                            cc[4] = Int32.TryParse(workSheet.Cells[rowIterator, 4].Value.ToString(), out ee);
                            claim.CursCod = ee;
                            claim.Init = Convert.ToDateTime(workSheet.Cells[rowIterator, 5].Value.ToString());
                            claim.End = Convert.ToDateTime(workSheet.Cells[rowIterator, 6].Value.ToString());
                            cc[7] = Int32.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out ee);
                            claim.Duracion = ee;
                            claim.Teacher = workSheet.Cells[rowIterator, 6].Value.ToString();
                            cc[8] = Int32.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out ee);
                            claim.FormCod = ee;
                            cc[9] = Int32.TryParse(workSheet.Cells[rowIterator, 6].Value.ToString(), out ee);
                            claim.FormArea = ee;
                            claim.Local = workSheet.Cells[rowIterator, 6].Value.ToString();
                            claim.Schedule = workSheet.Cells[rowIterator, 6].Value.ToString();
                            claim.Certification = workSheet.Cells[rowIterator, 6].Value.ToString();

                            suppleirList.Add(claim);
                        }
                    }
                }
            }
            return RedirectToAction("Index", new { message = @HIQResources.messageOperationSuccess });
        }

        [Authorize(Roles = "Admin")]
        //POST: User/ImportRes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportRes(HttpPostedFileBase file)
        {
            System.IO.File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\.xls");
            System.IO.File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\.xlsx");
            System.IO.File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\e.xls");
            System.IO.File.Delete(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\e.xls");

            var fileExtension = Path.GetExtension(file.FileName);

            if(file.ContentLength > excelLimitFile) return RedirectToAction("Index", new { message = @HIQResources.errorFileToLong });

            //if (file.ContentLength <= 0 || fileExtension != ".xls" || fileExtension != ".xlsx" ||
            //                                fileExtension != ".XLS" || fileExtension != ".XLSX")
            //{
            //    return RedirectToAction("Index", new { message = @HIQResources.errorNotExcel });
            //}

            string www = "";
            var path = Path.Combine(Server.MapPath("~/Images/"), fileExtension);
            file.SaveAs(path);
            if (fileExtension == ".xls") { System.IO.File.Move(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\.xls", @"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\e.xls"); www = "e.xls"; }
            else if (fileExtension == ".xlsx") { System.IO.File.Move(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\.xlsx", @"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Images\e.xlsx"); www = "e.xlsx"; }
            else return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });

            /*string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            string ee = @"C:\HIQTraining\ExcellImport\" + file.FileName;

            string wr = "Erro_" + file.FileName;*/
            ExcelToBD etd = new ExcelToBD();
            int ret = etd.Extract(www);


            //if(ret == 1) return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
            //if (ret == 1) return View();
            //return View();
            //if(ret == 1) return File(@"C:\Adentis++++++++++++++++\HIQTraining\HIQ-Training\HIQTrainingSite\HIQTrainingSite\Excel\Expo.xlsx", "application/xlsx", /*"Expo.xlsx"*/wr);
            return RedirectToAction("Index", new { message = @HIQResources.messageOperationSuccess });
        }

        // GET: User/Details/3
        [HttpGet]
        public ActionResult Details(string id)
        {
            return GetUserInformation(id);
        }

        // GET: User/Edit/3
        [HttpGet]
        public ActionResult Edit(string id)
        {
            return GetUserInformation(id);
        }

        // POST: User/Edit/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Password,ConfirmPassword")] UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                 //   bool result = identityManager.ChangeUserRoles(model.Id, model.ApplicationRoles);
                   IdentityResult result = identityManager.UpdateApplicationUser(model);
                    if (result.Succeeded)
                        return RedirectToAction("index");
                    else
                    {
                        //TODO: log erros list: result.Errors
                        return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
                    }
                }

            }
            catch (HIQTrainingException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                // TODO: log exception
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, HIQTrainingSite.HIQResources.errorMessageExceptionOccurred);
            }

            return View(model);
        }

        // GET: User/Create
        [HttpGet]
        public ActionResult Create()
        {
            UserViewModel model = new UserViewModel();
            model.ApplicationRoles = identityManager.GetApplicationRoles();
            model.Companies.CompanyDetailList = companyManager.GetCompanies();

            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = identityManager.AddApplicationUser(model.Companies.SelectedCompanyId, model.Name, model.Email, model.ApplicationRoles, model.DisplayColor);
                    if (result.Succeeded)
                    {
                        model.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = model.Alert;

                        return RedirectToAction("index");

                    }
                    else
                    {
                        //TODO: log erros list: result.Errors
                        GetListOfErrors(result);
                    }
                }
            }
            catch (HIQTrainingException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                // TODO: log exception
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, HIQTrainingSite.HIQResources.errorMessageExceptionOccurred);
            }

            model.Companies.CompanyDetailList = companyManager.GetCompanies();

            return View(model);
        }

        // POST: User/Delete/3
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownUser });
                }

                var result = identityManager.RemoveApplicationUser(id);
                if (result.Succeeded)
                    return RedirectToAction("index");
                else
                {
                    //TODO: log erros list: result.Errors
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
                }
            }
            catch (HIQTrainingException ex)
            {
                return RedirectToAction("Index", new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // TODO: log exception
                Console.WriteLine(ex.Message);
                return RedirectToAction("index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }
        }

        private ActionResult GetUserInformation(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });


            UserViewModel model = identityManager.GetUserById(id);
            model.ApplicationRoles = identityManager.GetApplicationRoles();

            return View(model);
        }

        private void GetListOfErrors(IdentityResult result)
        {
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);
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
    }
}