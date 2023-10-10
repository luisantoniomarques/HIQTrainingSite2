using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HIQTraining.Model;
using HIQTraining.Business.Configuration;
using HIQTraining.DAL.Configuration;
using HIQTrainingSite.ViewModel;
using System.Data.Entity.Infrastructure;
using HIQTraining.Business.Log;
using HIQTrainingSite.Common;

namespace HIQTrainingSite.Controllers.Configuration {
    [Authorize]
    public class InscriptionTypeController : BaseController {

        InscriptionTypeManager inscriptionTypeManager;

        public InscriptionTypeController() {
            this.inscriptionTypeManager = new InscriptionTypeManager(new InscriptionTypeDao());
        }

        // GET: InscriptionType
        [HttpGet]
        public ActionResult Index(int page = 1, string message = null) {
            InscriptionTypeListViewModel vm = new InscriptionTypeListViewModel();
            vm.InscriptionTypesList = inscriptionTypeManager.GetPagedInscriptionTypes(page, HIQSiteConstants.General.PAGE_SIZE);

            if(TempData["alert"] != null) {
                vm.Alert = (AlertViewModel)TempData["alert"];
            } else if(!string.IsNullOrEmpty(message)) {
                vm.Alert.SetErrorMessage(message);
            }

            return View(vm);
        }

        // GET: InscriptionType/Details/5
        [HttpGet]
        public ActionResult Details(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownAction });
            }

            InscriptionTypeViewModel vm = new InscriptionTypeViewModel();
            InscriptionType inscriptionType = inscriptionTypeManager.GetInscriptionTypeById(id.Value);
            vm.Description = inscriptionType.Description;
            vm.Id = inscriptionType.Id;

            return View(vm);
        }

        // GET: InscriptionType/Create
        [HttpGet]
        public ActionResult Create() {
            InscriptionTypeViewModel vm = new InscriptionTypeViewModel();

            return View(vm);
        }

        // POST: InscriptionType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] InscriptionTypeViewModel vm) {
            try {
                if(ModelState.IsValid) {

                    InscriptionType inscriptionType = new InscriptionType();
                    inscriptionType.Description = vm.Description;

                    InscriptionType newInscriptionType = inscriptionTypeManager.Add(inscriptionType, base.GetLoggedUser());

                    if(newInscriptionType != null) {
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
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.InscriptionType, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageDuplicateRecord });
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.InscriptionType, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }

            return View(vm);
        }

        // GET: InscriptionType/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            InscriptionType inscriptionType = inscriptionTypeManager.GetInscriptionTypeById(id.Value);
            if(inscriptionType == null) {
                return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownRecord });
            }

            InscriptionTypeViewModel vm = new InscriptionTypeViewModel();
            vm.Id = inscriptionType.Id;
            vm.Description = inscriptionType.Description;

            return View(vm);
        }

        // POST: InscriptionType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] InscriptionTypeViewModel vm) {
            try {
                if(ModelState.IsValid) {
                    InscriptionType inscriptionType = new InscriptionType();
                    inscriptionType.Id = vm.Id;
                    inscriptionType.Description = vm.Description;

                    int result = inscriptionTypeManager.Update(inscriptionType, base.GetLoggedUser());
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
                Log.AddLogRecord(LogManager.LogType.Info, LogManager.LogPriority.Low, LogManager.LogCategory.InscriptionType, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);

                return RedirectToAction("Index");
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.InscriptionType, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);

                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: InscriptionType/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id) {
            try {
                if(id == null) {
                    return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownAction });
                }

                InscriptionTypeViewModel vm = new InscriptionTypeViewModel();
                InscriptionType inscriptionType = inscriptionTypeManager.GetInscriptionTypeById(id.Value);
                if(inscriptionType == null) {
                    return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownRecord });
                }

                int result = inscriptionTypeManager.Delete(inscriptionType);
                if(result == 1) {
                    vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                    TempData["alert"] = vm.Alert;

                    return RedirectToAction("Index");
                } else {
                    vm.Alert.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);
                    TempData["alert"] = vm.Alert;

                    return RedirectToAction("Index");
                }
            } catch(DbUpdateException ex) {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.InscriptionType, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageUnableToDeleteRecord });
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.InscriptionType, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
        }

    }
}
