using HIQTraining.Business.Configuration;
using HIQTraining.Business.Log;
using HIQTraining.DAL.Configuration;
using HIQTraining.Exceptions;
using HIQTraining.Model;
using HIQTrainingSite.Common;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace HIQTrainingSite.Controllers.Configuration
{
    [Authorize]
    public class FormativeEntityController : BaseController
    {
        HIQTraining.Business.Configuration.FormativeEntityManager manager;

        public FormativeEntityController()
        {
            manager = new HIQTraining.Business.Configuration.FormativeEntityManager(new FormativeEntityDao());
        }

        // GET: CourseType
        //[OutputCache(CacheProfile ="Long", VaryByHeader ="X-Request-With;Accept-Language", Location = OutputCacheLocation.Server)]
        public ActionResult Index(string message, int page = 1)
        {
            FormativeEntityListViewModel vm = new FormativeEntityListViewModel();
  
            if (TempData["alert"] != null)
            {
                vm.Alert = (AlertViewModel)TempData["alert"];
            }
            else if (!string.IsNullOrEmpty(message))
            {
                vm.Alert.SetErrorMessage(message);
            }
            vm.FormativeEntitiesList = manager.GetPagedFormativeEntities(page, HIQSiteConstants.General.PAGE_SIZE);

            return View(vm);
        }

        // GET: CourseType/Create
        [HttpGet]
        public ActionResult Create()
        {
            FormativeEntityViewModel viewModel = new FormativeEntityViewModel();
            return View(viewModel);
        }

        // POST: CourseType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormativeEntityViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FormativeEntity formativeEntity = new FormativeEntity();
                    formativeEntity.Id = vm.Id;
                    formativeEntity.Name = vm.Name;

                    manager.Add(formativeEntity, base.GetLoggedUser());

                    if(formativeEntity != null)
                    {
                        vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = vm.Alert;
                    }
                    return RedirectToAction("Index");
                }

            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.FormativeEntity, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.FormativeEntity, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }

            return View(vm);
        }


        // GET: CourseType/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction }); 
            }

            FormativeEntity formativeEntity = manager.GetFormativeEntityById(id.Value);
            if (formativeEntity == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            FormativeEntityViewModel viewModel = new FormativeEntityViewModel();
            viewModel.Id = formativeEntity.Id;
            viewModel.Name = formativeEntity.Name;
            viewModel.CreatedDate = formativeEntity.CreatedDate;
            viewModel.UserCreated = formativeEntity.UserCreated;
            viewModel.UpdateDate = formativeEntity.UpdateDate;
            viewModel.UserUpdated = formativeEntity.UserUpdated;

            return View(viewModel);
        }

        // GET: CourseType/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            FormativeEntityViewModel viewModel = new FormativeEntityViewModel();

            FormativeEntity formativeEntity= manager.GetFormativeEntityById(id.Value);
            if (formativeEntity == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            viewModel.Id = formativeEntity.Id;
            viewModel.Name = formativeEntity.Name;
            viewModel.CreatedDate = formativeEntity.CreatedDate;
            viewModel.UserCreated = formativeEntity.UserCreated;
            viewModel.UpdateDate = formativeEntity.UpdateDate;
            viewModel.UserUpdated = formativeEntity.UserUpdated;

            return View(viewModel);
        }

        // POST:/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UserCreated,CreatedDate,UserUpdated,UpdateDate")]FormativeEntityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                FormativeEntity formativeEntity = new FormativeEntity();
                formativeEntity.Id = viewModel.Id;
                formativeEntity.Name = viewModel.Name;
                formativeEntity.UserCreated = viewModel.UserCreated;
                formativeEntity.CreatedDate = viewModel.CreatedDate;
                formativeEntity.UserUpdated = viewModel.UserUpdated;
                formativeEntity.UpdateDate = viewModel.UpdateDate;

                int result = manager.Save(formativeEntity, base.GetLoggedUser());
                if (result == 1)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
            }

            return View(viewModel);
        }

        // POST: /Delete/5
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

                FormativeEntity formativeEntity = manager.GetFormativeEntityById(id.Value);
                if (formativeEntity == null)
                {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
                }

                int result = manager.Delete(id.Value);
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