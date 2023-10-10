using HIQTraining.Business.CourseLevel;
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

namespace HIQTrainingSite.Controllers.Configuration
{
    [Authorize]
    public class CourseLevelController : BaseController
    {
        CourseLevelManager courseLevelManager;

        public CourseLevelController()
        {
            courseLevelManager = new CourseLevelManager(new CourseLevelDao());
        }


        // GET: CourseLevel
        [HttpGet]
        public ActionResult Index(int page = 1, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                ModelState.AddModelError(string.Empty, message);

            CourseLevelListViewModel model = new CourseLevelListViewModel();
            model.CourseLevels = courseLevelManager.GetPagedCourseLevels(page, HIQSiteConstants.General.PAGE_SIZE);

            return View(model);
        }

        // GET: CourseLevel/Create
        [HttpGet]
        public ActionResult Create()
        {
            CourseLevelViewModel viewModel = new CourseLevelViewModel();
            return View(viewModel);
        }

        // POST: CourseLevel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseLevelViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CourseLevel courseLevel = new CourseLevel();
                    courseLevel.Id = viewModel.Id;
                    courseLevel.Name = viewModel.Name;

                    courseLevelManager.Add(courseLevel, base.GetLoggedUser());
                    return RedirectToAction("Index");
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

        // GET: CourseLevel/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CourseLevel courseLevel = courseLevelManager.GetCourseLevelById(id.Value);
            if (courseLevel == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CourseLevelViewModel viewModel = new CourseLevelViewModel();
            viewModel.Id = courseLevel.Id;
            viewModel.Name = courseLevel.Name;
            viewModel.CreatedDate = courseLevel.CreatedDate;
            viewModel.UserCreated = courseLevel.UserCreated;
            viewModel.UpdateDate = courseLevel.UpdateDate;
            viewModel.UserUpdated = courseLevel.UserUpdated;

            return View(viewModel);
        }

        // GET: CourseLevel/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CourseLevelViewModel viewModel = new CourseLevelViewModel();

            CourseLevel courseLevel = courseLevelManager.GetCourseLevelById(id.Value);
            if (courseLevel == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            viewModel.Id = courseLevel.Id;
            viewModel.Name = courseLevel.Name;
            viewModel.CreatedDate = courseLevel.CreatedDate;
            viewModel.UserCreated = courseLevel.UserCreated;
            viewModel.UpdateDate = courseLevel.UpdateDate;
            viewModel.UserUpdated = courseLevel.UserUpdated;

            return View(viewModel);
        }

        // POST: CourseLevel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UserCreated,CreatedDate,UserUpdated,UpdateDate")]CourseLevelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                CourseLevel courseLevel = new CourseLevel();
                courseLevel.Id = viewModel.Id;
                courseLevel.Name = viewModel.Name;
                courseLevel.UserCreated = viewModel.UserCreated;
                courseLevel.CreatedDate = viewModel.CreatedDate;
                courseLevel.UserUpdated = viewModel.UserUpdated;
                courseLevel.UpdateDate = viewModel.UpdateDate;

                int result = courseLevelManager.Update(courseLevel, base.GetLoggedUser());
                if (result == 1)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToExecuteOperation });
            }

            return View(viewModel);
        }

        // POST: CourseLevel/Delete/5
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

                CourseLevel courseLevel = courseLevelManager.GetCourseLevelById(id.Value);
                if (courseLevel == null)
                {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
                }

                int result = courseLevelManager.Delete(id.Value);
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