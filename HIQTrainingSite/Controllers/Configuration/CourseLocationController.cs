using HIQTraining.Business.CourseLocation;
using HIQTraining.Business.Log;
using HIQTraining.DAL.Configuration;
using HIQTraining.Exceptions;
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

namespace HIQTrainingSite.Controllers.Configuration
{
    [Authorize]
    public class CourseLocationController : BaseController
    {
        CourseLocationManager courseLocationManager;

        public CourseLocationController()
        {
            courseLocationManager = new CourseLocationManager(new CourseLocationDao());
        }


        // GET: Location
        public ActionResult Index(string message, int page = 1)
        {

            CourseLocationListViewModel vm = new CourseLocationListViewModel();

            if (TempData["alert"] != null)
            {
                vm.Alert = (AlertViewModel)TempData["alert"];
            }
            else if (!string.IsNullOrEmpty(message))
            {
                vm.Alert.SetErrorMessage(message);
            }

            vm.CourseLocationList = courseLocationManager.GetPagedCourseLocations(page, HIQSiteConstants.General.PAGE_SIZE);

            return View(vm);
        }

        // GET: Location/Create
        [HttpGet]
        public ActionResult Create()
        {
            CourseLocationViewModel viewModel = new CourseLocationViewModel();
            return View(viewModel);
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseLocationViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CourseLocationDetail courseLocation = new CourseLocationDetail();
                    courseLocation.Id = vm.Id;
                    courseLocation.Name = vm.Name;
                    courseLocation.UserCreated = vm.UserCreated;
                    courseLocation.CreatedDate = vm.CreatedDate;
                    courseLocation.UserUpdated = vm.UserUpdated;
                    courseLocation.UpdateDate = vm.UpdateDate;
                    courseLocation.DisplayColor = vm.DisplayColor;
                    courseLocationManager.Add(courseLocation, base.GetLoggedUser());

                    if(courseLocation != null)
                    {
                        vm.Alert.SetSuccessMessage(HIQResources.messageOperationSuccess);
                        TempData["alert"] = vm.Alert;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        vm.Alert.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);
                        TempData["alert"] = vm.Alert;

                        return RedirectToAction("Index");
                    }
   
                }

            }
            catch (HIQTrainingException ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.CourseLocation, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = HIQResources.errorMessageExceptionOccurred });
            }
            catch (Exception ex)
            {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.CourseLocation, ex.Message, ex.StackTrace, base.GetLoggedUser());

                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }

            return View(vm);
        }
        // GET: Location/Edit/3
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CourseLocationViewModel viewModel = new CourseLocationViewModel();

            CourseLocationDetail courseLocation = courseLocationManager.GetCourseLocationById(id.Value);
            if (courseLocation == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            viewModel.Id = courseLocation.Id;
            viewModel.Name = courseLocation.Name;
            viewModel.CreatedDate = courseLocation.CreatedDate.HasValue ? courseLocation.CreatedDate.Value : default(DateTime);
            viewModel.UserCreated = courseLocation.UserCreated;
            viewModel.UpdateDate = courseLocation.UpdateDate;
            viewModel.UserUpdated = courseLocation.UserUpdated;
            viewModel.DisplayColor = courseLocation.DisplayColor;

            return View(viewModel);
        }

        // POST: Location/Edit/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UserCreated,CreatedDate,UserUpdated,UpdateDate,DisplayColor")]CourseLocationViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CourseLocationDetail courseLocation = new CourseLocationDetail();
                    courseLocation.Id = viewModel.Id;
                    courseLocation.Name = viewModel.Name;
                    courseLocation.DisplayColor = viewModel.DisplayColor;

                    int result = courseLocationManager.Update(courseLocation, base.GetLoggedUser());
                    if (result == 1)
                        return RedirectToAction("Index");
                    else
                        return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
                }

                return View(viewModel);
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageDuplicateRecord });
            }
            catch (Exception)
            {
                // TODO: log error
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }
        }


        // GET: CourseLocation/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            var courseLocation = courseLocationManager.GetCourseLocationById(id.Value);
            if (courseLocation == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CourseLocationViewModel viewModel = new CourseLocationViewModel();
            viewModel.Id = courseLocation.Id;
            viewModel.Name = courseLocation.Name;
            viewModel.CreatedDate = courseLocation.CreatedDate.HasValue ? courseLocation.CreatedDate.Value : default(DateTime);
            viewModel.UserCreated = courseLocation.UserCreated;
            viewModel.UpdateDate = courseLocation.UpdateDate;
            viewModel.UserUpdated = courseLocation.UserUpdated;
            viewModel.DisplayColor = courseLocation.DisplayColor;

            return View(viewModel);
        }

        // POST: Location/Delete/3
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

                var courseLocation = courseLocationManager.GetCourseLocationById(id.Value);
                if (courseLocation == null)
                {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
                }

                int result = courseLocationManager.Delete(id.Value);
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