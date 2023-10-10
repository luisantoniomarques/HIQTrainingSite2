using HIQTraining.Business.Certification;
using HIQTraining.Business.Log;
using HIQTraining.DAL.Certification;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using HIQTrainingSite.Common;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.Controllers.Certification {

    [Authorize]
    public class CertificationTypeController : BaseController {

        CertificationTypeManager certificationTypeManager;

        public CertificationTypeController() {
            this.certificationTypeManager = new CertificationTypeManager(new CertificationTypeDao());
        }

        // GET: CertificationType
        [HttpGet]
        public ActionResult Index(int page = 1, string message = null) {
            CertificationTypeListViewModel vm = new CertificationTypeListViewModel();
            vm.CertificationTypeList = certificationTypeManager.GetPagedCertificationTypes(page, HIQSiteConstants.General.PAGE_SIZE);

            if(TempData["alert"] != null) {
                vm.Alert = (AlertViewModel)TempData["alert"];
            } else if(!string.IsNullOrEmpty(message)) {
                vm.Alert.SetErrorMessage(message);
            }

            return View(vm);
        }

        // GET: CertificationType/Details/5
        [HttpGet]
        public ActionResult Details(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownAction });
            }

            CertificationTypeViewModel vm = new CertificationTypeViewModel();
            CertificationType certificationType = certificationTypeManager.GetCertificationTypeById(id.Value);
            vm.Name = certificationType.Name;
            vm.Id = certificationType.Id;
            vm.Code = certificationType.Code;

            return View(vm);
        }

        // GET: CertificationType/Create
        [HttpGet]
        public ActionResult Create() {
            CertificationTypeViewModel vm = new CertificationTypeViewModel();

            return View(vm);
        }

        // POST: CertificationType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code")] CertificationTypeViewModel vm) {
            try {
                if(ModelState.IsValid) {

                    CertificationType certificationType = new CertificationType();
                    certificationType.Name = vm.Name;
                    certificationType.Code = vm.Code;

                    CertificationType newCertificationType = certificationTypeManager.Add(certificationType, base.GetLoggedUser());

                    if(newCertificationType != null) {
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

        // GET: CertificationType/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CertificationType certificationType = certificationTypeManager.GetCertificationTypeById(id.Value);
            if(certificationType == null) {
                return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownRecord });
            }

            CertificationTypeViewModel vm = new CertificationTypeViewModel();
            vm.Id = certificationType.Id;
            vm.Name = certificationType.Name;
            vm.Code = certificationType.Code;

            return View(vm);
        }

        // POST: CertificationType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code")] CertificationTypeViewModel vm) {

            try {
                if(ModelState.IsValid) {
                    CertificationType certificationType = new CertificationType();
                    certificationType.Id = vm.Id;
                    certificationType.Name = vm.Name;
                    certificationType.Code = vm.Code;
                    
                    int result = certificationTypeManager.Update(certificationType, base.GetLoggedUser());
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
                Log.AddLogRecord(LogManager.LogType.Info, LogManager.LogPriority.Low, LogManager.LogCategory.CertificationType, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);

                return RedirectToAction("Index");
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.CertificationType, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.Alert.SetErrorMessage(HIQResources.errorMessageExceptionOccurred);

                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // POST: CertificationType/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id) {
            try {
                if(id == null) {
                    return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownAction });
                }

                CertificationTypeViewModel vm = new CertificationTypeViewModel();
                CertificationType certificationType = certificationTypeManager.GetCertificationTypeById(id.Value);
                if(certificationType == null) {
                    return RedirectToAction("Index", new { message = HIQResources.errorMessageUnknownRecord });
                }

                int result = certificationTypeManager.Delete(certificationType);
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
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.CertificationType, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageUnableToDeleteRecord });
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.CertificationType, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
        }

    }
}