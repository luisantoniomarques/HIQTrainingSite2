using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HIQTraining.Model;
using HIQTraining.Business.CourseType;
using HIQTraining.DAL.Configuration;
using HIQTrainingSite.ViewModel;
using HIQTrainingSite.Common;
using System.Data.Entity.Infrastructure;
using HIQTraining.Model;
using HIQTraining.Exceptions;

namespace HIQTrainingSite.Controllers.Configuration
{
    [Authorize]
    public class CourseTypeController : BaseController
    {

        CourseTypeManager courseTypeManager;

        public CourseTypeController() {

            courseTypeManager = new CourseTypeManager(new CourseTypeDao());
        }
        // GET: CourseType
        [HttpGet]
        public ActionResult Index(int page = 1, string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
                ModelState.AddModelError(string.Empty, message);
            CourseTypeListViewModel model = new CourseTypeListViewModel();
            model.CourseTypes = courseTypeManager.GetPagedCourseTypes(page, HIQSiteConstants.General.PAGE_SIZE);
            return View(model);
        }

      

        // GET: CourseType/Create
        [HttpGet]
        public ActionResult Create()
        {
            CourseTypeViewModel viewModel = new CourseTypeViewModel();
            return View(viewModel);
        }
        // POST: CourseType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseTypeViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CourseType courseType = new CourseType();
                    courseType.Id = viewModel.Id;
                    courseType.Name = viewModel.Name;

                    courseTypeManager.Add(courseType, base.GetLoggedUser());
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

            CourseType courseType = courseTypeManager.GetCourseTypeById(id.Value);
            if (courseType == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CourseTypeViewModel viewModel = new CourseTypeViewModel();
            viewModel.Id = courseType.Id;
            viewModel.Name = courseType.Name;
            viewModel.CreateDate = courseType.CreateDate;
            viewModel.UserCreated = courseType.UserCreated;
            viewModel.UpdateDate = courseType.UpdateDate;
            viewModel.UserUpdated = courseType.UserUpdated;

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

            CourseType courseType = courseTypeManager.GetCourseTypeById(id.Value);
            if (courseType == null)
            {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CourseTypeViewModel viewModel = new CourseTypeViewModel();
            viewModel.Id = courseType.Id;
            viewModel.Name = courseType.Name;
            viewModel.CreateDate = courseType.CreateDate;
            viewModel.UserCreated = courseType.UserCreated;
            viewModel.UpdateDate = courseType.UpdateDate;
            viewModel.UserUpdated = courseType.UserUpdated;

            
            return View(viewModel);
        }

        // POST: CourseType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UpdateDate,UserCreated,CreateDate,UserUpdated")] CourseTypeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                CourseType courseType = new CourseType();
                courseType.Id = viewModel.Id;
                courseType.Name = viewModel.Name;
                courseType.UserCreated = viewModel.UserCreated;
                courseType.CreateDate = viewModel.CreateDate;
                courseType.UserUpdated = viewModel.UserUpdated;
                courseType.UpdateDate = viewModel.UpdateDate;

                int result = courseTypeManager.Update(courseType, base.GetLoggedUser());
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

                CourseType courseType = courseTypeManager.GetCourseTypeById(id.Value);
                if (courseType == null)
                {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
                }

                int result = courseTypeManager.Delete(id.Value);
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
