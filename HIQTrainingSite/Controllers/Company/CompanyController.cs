using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HIQTraining.Model;
using HIQTrainingSite.ViewModel;
using HIQTraining.Business;
using HIQTraining.Business.Company;
using HIQTrainingSite.Common;
using HIQTraining.DAL.Company;
using HIQTraining.Exceptions;
using HIQTrainingSite.Mappers;
using HIQTraining.Business.Log;
using System.Data.Entity.Infrastructure;

namespace HIQTrainingSite.Controllers.Company
{
    public class CompanyController : BaseController
    {

        private Mapper mapper = new Mapper();
        private CompanyManager companyManager = new CompanyManager(new CompanyDao());
    

        // GET: Company
        public ActionResult Index(string message, int page = 1)
        {
            CompanyListViewModel vm = new CompanyListViewModel();

            if (TempData["alert"] != null)
            {
                vm.Alert = (AlertViewModel)TempData["alert"];
            }
            else if (!string.IsNullOrEmpty(message))
            {
                vm.Alert.SetErrorMessage(message);
            }
            vm.CompanyList = companyManager.GetPagedCompany(page, HIQSiteConstants.General.PAGE_SIZE);

            return View(vm);
        }

        [Authorize(Roles = "Admin, Staff")]
        //GET: Course/Create
        [HttpGet]
        public ActionResult Create()
        {
            CompanyViewModel vm = new CompanyViewModel();
            return View(vm);
        }
       
        [Authorize(Roles = "Admin, Staff")]
        //GET: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyViewModel model)
        {
            try
            {
                CompanyManager companyManager = new CompanyManager(new CompanyDao());

                if (ModelState.IsValid)
                {

                    HIQTraining.Model.Company newCompany = mapper.CompanyMapperCompanyVm(model, base.GetLoggedUser());
                    companyManager.Add(newCompany, base.GetLoggedUser());

                    if (newCompany != null)
                    {
                        model.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = model.Alert;
                    }
                    return RedirectToAction("Index");

                }
            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Company, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Company, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

           HIQTraining.Model.Company company = companyManager.GetCompanyById(id.Value);
            if (company == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CompanyViewModel viewModel = new CompanyViewModel();
            viewModel.Id = company.Id;
            viewModel.Name = company.Name;
            viewModel.External = company.External;

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CompanyViewModel viewModel = new CompanyViewModel();

            HIQTraining.Model.Company company = companyManager.GetCompanyById(id.Value);

            if (company == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            viewModel.Id = company.Id;
            viewModel.Name = company.Name;
            viewModel.External = company.External;
         
            return View(viewModel);
        }

        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                HIQTraining.Model.Company company = new HIQTraining.Model.Company();
                company.Id = viewModel.Id;
                company.Name = viewModel.Name;
                company.External = viewModel.External;
                company.UpdateDate = DateTime.Now;

                int result = companyManager.Update(company, base.GetLoggedUser());
                if (result == 1)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
            }
        
            return View(viewModel);
        }

        // GET: CourseType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
                }

                HIQTraining.Model.Company company = companyManager.GetCompanyById(id.Value);
                if (company == null)
                {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
                }

                int result = companyManager.Delete(id.Value);
                if (result == 1)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToDeleteRecord });
            }
            catch (HIQTrainingException ex)
            {
                return RedirectToAction("Index", new { message = ex.Message });
            }
            catch (Exception)
            {
                // TODO: log error
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }
        }






    }
}
