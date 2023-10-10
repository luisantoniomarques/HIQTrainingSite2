using HIQTraining.Business.Certification;
using HIQTraining.Business.Company;
using HIQTraining.Business.Configuration;
using HIQTraining.DAL.Certification;
using HIQTraining.DAL.Company;
using HIQTraining.DAL.Configuration;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIQTrainingSite.Mappers;
using System.Data.Entity.Infrastructure;
using HIQTraining.ModelDetail;
using HIQTraining.Business.Student;
using HIQTraining.DAL.Student;
using HIQTraining.ActiveDirectory;
using HIQTrainingSite.Common;
using HIQTraining.Business.Log;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.Web.Helpers;
using PagedList;
using System.Data.SqlClient;
using System.Data;
using HIQTraining.Exceptions;

namespace HIQTrainingSite.Controllers.Certification {

    [Authorize]
    public class CertificationController : BaseController {

        private CertificationManager certificationManager;
        CompanyUser companyUser;
        private Mapper mapper;
        CompanyManager companyManager;
        private string result ;

        public CertificationController() {
            certificationManager = new CertificationManager(new CertificationDao());
            companyUser = new CompanyUser();
            companyManager = new CompanyManager(new CompanyDao());
            mapper = new Mapper();
        }

        [Authorize(Roles = "Admin, Staff")]
        //GET: Certification/Index
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(string message, int page = 1) {
            CertificationListViewModel viewModel = new CertificationListViewModel();

            if(!string.IsNullOrEmpty(message)) {
                ModelState.AddModelError(string.Empty, message);
                //viewModel.SetErrorMessage(message);
            }
          
            viewModel.CertificationList = certificationManager.GetPagedCertifications(page, HIQSiteConstants.General.PAGE_SIZE);
            viewModel.Search.Companies.CompanyDetailList = GetCompanyListForSearch();

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        private List<CompanyDetail> GetCompanyListForSearch() {
            var list = companyManager.GetCompanies();
            list.Insert(0, new HIQTraining.ModelDetail.CompanyDetail() { Id = -1, Name = HIQResources.dropdownAllOptions });
            return list;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult SearchCertification(string name, int? companyId, int? status) {
            CertificationListViewModel vm = this.SearchCertifications(name, companyId, status);

            return PartialView("_CertificationIndex", vm);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public CertificationListViewModel SearchCertifications(string name, int? companyId, int? status, int page = 1) {
            CertificationListViewModel vm = new CertificationListViewModel();
            vm.CertificationList = certificationManager.GetCertificationsSearchResults(name, companyId, status, page, HIQSiteConstants.General.PAGE_SIZE);
          
            return vm;
        }

        /// <summary>
        /// partial view that returns all the certifications
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult _CertificationIndex(string nome, int page = 1) {
            CertificationListViewModel viewModel = new CertificationListViewModel();
            viewModel.CertificationList = certificationManager.GetPagedCertifications(page, HIQSiteConstants.General.PAGE_SIZE);

            if(!string.IsNullOrEmpty(nome)) {
                viewModel.CertificationList = certificationManager.GetPagedCertificationsByName(nome, page, HIQSiteConstants.General.PAGE_SIZE);
            }

            return PartialView(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult _CertificationStudent(int? id, string nome, int page = 1) {
            CertificationListViewModel viewModel = new CertificationListViewModel();
            viewModel.CertificationList = certificationManager.GetPagedStudentCertifications(id.Value, page, HIQSiteConstants.General.PAGE_SIZE_SHORT);
            viewModel.DisplayName = nome;

            return PartialView(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CertificationList(int? id, string nome, string message, int page = 1)
        {
            CertificationListViewModel viewModel = new CertificationListViewModel();

            if (id != null) { 
                viewModel = new CertificationListViewModel();
                viewModel.CertificationList = certificationManager.GetPagedStudentCertifications(id.Value, page, HIQSiteConstants.General.PAGE_SIZE_SHORT);
                viewModel.StudentId = (int)id;
                viewModel.DisplayName = nome;

                return View("CertificationList", viewModel);
            }
            else
            {
                return View("Index", viewModel);
            }
        }

        [HttpGet]
        public JsonResult SearchCertificationByName(string certificationName){
            CertificationTypeManager certificationstypesManager = new CertificationTypeManager(new CertificationTypeDao());
            var certificationsTypes = certificationstypesManager.GetCertificationsTypesByName(certificationName).ToList();

            return Json(certificationsTypes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateFullCertification(int? id) {
            CertificationViewModel viewModel = new CertificationViewModel();
            FormativeEntityManager courseTypeManager = new FormativeEntityManager(new FormativeEntityDao());
            var courseTypes = courseTypeManager.GetFormativeEntities();
            viewModel.EntitiesList = courseTypes.ToList();

            if(id.HasValue) {
                StudentManager studentManager = new StudentManager(new StudentDao());
                var student = studentManager.GetStudentById(id.Value);

                viewModel.StudentName = student.Name;
                viewModel.StudentEmail = student.Email;
                viewModel.Id = student.Id;
            }

            return View("CreateFullCertification", viewModel);
        }

        [Authorize(Roles = "Admin, Staff")]
        //GET: Certification/Create
        [HttpGet]
        public ActionResult Create(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }
         

            StudentManager studentManager = new StudentManager(new StudentDao());
            HIQTraining.Model.Student student = studentManager.GetStudentById(id.Value);

            if(student == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CertificationViewModel model = new CertificationViewModel();
            FormativeEntityManager courseTypeManager = new FormativeEntityManager(new FormativeEntityDao());  
            model.CertificationList= certificationManager.GetAllCertifications().ToList();
           

            var courseTypes = courseTypeManager.GetFormativeEntities();
            model.EntitiesList = courseTypes.ToList();
            

      
            model.StudentEmail = student.Email;
            model.StudentId = student.Id;
            model.StudentName = student.Name;
            model.Id = student.Id;

            return View(model);
        }

        [Authorize(Roles = "Admin, Staff")]
        //POST: Certification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CertificationViewModel vm, HttpPostedFileBase file) {
            try {

                FormativeEntityManager courseTypeManager = new FormativeEntityManager(new FormativeEntityDao());
                var courseTypes = courseTypeManager.GetFormativeEntities();
                vm.EntitiesList = courseTypes.ToList();

                if(string.IsNullOrEmpty(vm.Code) || string.IsNullOrEmpty(vm.StudentEmail)) {
                    ModelState.AddModelError(string.Empty, HIQResources.errorMessageRequired);
                }

                if(ModelState.IsValid) {

                    // obter a extenção do ficheiro
                    string fileExtension = Path.GetExtension(file.FileName);
                    //caminho absoluto do ficheiro
                    string fileName =  DateTime.Now.ToString("yyyyMMddHHmmssfff") + Guid.NewGuid() + ".pdf";
                    // validar o ficheiro
                    if (file.ContentLength > 0 && (fileExtension == ".pdf" || fileExtension == ".PDF"))
                    {      
                        var path = Path.Combine(Server.MapPath("~/certificationfile"), fileName);
                        file.SaveAs(path);
                    }

                    vm.pdf = "/certificationfile/" + fileName;
                    HIQTraining.Model.Certification certification = mapper.CertificationVmMapperCertification(vm);
                    certificationManager.Add(certification, base.GetLoggedUser());
                    vm.SetSuccessMessage(HIQResources.messageOperationSuccess);

                    return RedirectToAction("CertificationList", new { id = vm.StudentId, nome = vm.StudentName, message = HIQResources.messageOperationSuccess });
                }
                return View(vm);

            } catch(DbUpdateException ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Certification, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.SetErrorMessage(ex.Message);

                return RedirectToAction("Index");
            } catch(Exception ex) {
                Log.AddLogRecord(LogManager.LogType.Error, LogManager.LogPriority.High, LogManager.LogCategory.Certification, ex.Message, ex.StackTrace, base.GetLoggedUser());
                vm.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);

                return RedirectToAction("Index");
            }
        }


        //GET: Certification/Details
        [HttpGet]
        public ActionResult Details(int? id, string name) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CertificationFullDetail certification = certificationManager.GetCertificationById(id.Value);
            if(certification == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            CertificationViewModel certificationVM = new CertificationViewModel();
            certificationVM = mapper.CertificationFullDetailMapperCertificationVm(certification);

            return View(certificationVM);
        }

        [Authorize(Roles = "Admin, Staff")]
        //GET: Certification/Edit
        [HttpGet]
        public ActionResult Edit(int? id) {
            if(id == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
            }

            CertificationFullDetail certification = certificationManager.GetCertificationById(id.Value);
            if(certification == null) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
            }

            FormativeEntityManager courseTypeManager = new FormativeEntityManager(new FormativeEntityDao());
            var courseTypes = courseTypeManager.GetFormativeEntities();

            CertificationTypeManager certificationTypesManager = new CertificationTypeManager(new CertificationTypeDao());
            var certificationTypes = certificationTypesManager.GetCertificationsTypes();

            CertificationViewModel vm = mapper.CertificationFullDetailMapperCertificationVm(certification);
            vm.EntitiesList = courseTypes.ToList();

            return View(vm);
        }

        [Authorize(Roles = "Admin, Staff")]
        //POST: Certification/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CertificationViewModel vm, HttpPostedFileBase file) {
            FormativeEntityManager courseTypeManager = new FormativeEntityManager(new FormativeEntityDao());
            var courseTypes = courseTypeManager.GetFormativeEntities();
            vm.EntitiesList = courseTypes.ToList();

            if(ModelState.IsValid) {
                HIQTraining.Model.Certification certification = mapper.CertificationVmMapperCertification(vm);
                int updateResult = certificationManager.Save(certification, base.GetLoggedUser());

                // obter a extenção do ficheiro
                string fileExtension = Path.GetExtension(file.FileName);
                //caminho absoluto do ficheiro
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Guid.NewGuid() + ".pdf";
                // validar o ficheiro
                if (file.ContentLength > 0 && (fileExtension == ".pdf" || fileExtension == ".PDF"))
                {
                    var path = Path.Combine(Server.MapPath("~/certificationfile"), fileName);
                    file.SaveAs(path);
                }

                if (updateResult == 1) {
                    vm.SetSuccessMessage(HIQResources.messageOperationSuccess);
                    return RedirectToAction("CertificationList", new { id = vm.StudentId, nome = vm.StudentName });
                } else {
                    vm.SetErrorMessage(HIQResources.errorMessageUnableToExecuteOperation);
                    return View("Index");
                }
            }

            return View(vm);
        }

        [Authorize(Roles = "Admin, Staff")]
        //POST: Certification/Delete
        [HttpPost]
        public ActionResult Delete(int? id) {
            try {
                if(id == null) {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownAction });
                }

                CertificationFullDetail course = certificationManager.GetCertificationById(id.Value);
                if(course == null) {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnknownRecord });
                }

                int deleteResult = certificationManager.Delete(id.Value);
                if(deleteResult == 1) {
                    string url = Request.UrlReferrer.ToString();
                    return RedirectToAction(url);
                } else {
                    return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
                }

            } catch(DbUpdateException ex) {
                Log.AddLogRecord(LogManager.LogType.Warning, LogManager.LogPriority.Low, LogManager.LogCategory.Inscription, ex.Message, ex.StackTrace, base.GetLoggedUser());
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageUnableToDeleteRecord });


            } catch(Exception) {
                return RedirectToAction("Index", new { message = @HIQResources.errorMessageExceptionOccurred });
            }
        }

        [HttpPost]
        public JsonResult SearchUsersByName(string userName, int page = 1) {
            CertificationManager certificationManager = new CertificationManager(new CertificationDao());
            var usersCertifications = certificationManager.GetPagedCertificationsByName(userName, page, HIQSiteConstants.General.PAGE_SIZE);

            return Json(usersCertifications, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentsByName(string userName) {
            StudentManager studentManager = new StudentManager(new StudentDao());
            var students = studentManager.GetPagedStudentsByName(userName);

            return Json(students, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCodeCertificationType(string certificationName)
        {
            CertificationTypeManager certificationstypesManager = new CertificationTypeManager(new CertificationTypeDao());
            var certificationsTypes = certificationstypesManager.GetCertificationsTypesByName(certificationName).ToList();
            
            return Json(certificationsTypes, JsonRequestBehavior.AllowGet);
        }
		// Atenção metodo criado para testes tera de ser feito os DAL's para alimentar este processo...
        [HttpGet]
        public ActionResult Testal(int id)
        {
            
            SqlCommand com;
            string str;
            SqlConnection cn = new SqlConnection(@"Data Source=192.168.0.80\BEEPIDEV;Initial Catalog=HIQTraining;User ID=sa_dev;Password=sa_dev;");
            cn.Open();

            str = "SELECT * FROM Students WHERE Id = '" + id + "'";


            com = new SqlCommand(str, cn);
            SqlDataReader reader = com.ExecuteReader();
            if (reader.Read())
            {
                result = reader["Name"].ToString();
            }

          
            cn.Close();

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml, string nome)
        {
          

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
            

                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 0f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // setting image path
                string imagePath = ("C://Users//Leandro Batista//Documents//Visual Studio 2015//Projects//HIQ-Training//HIQTrainingSite//HIQTrainingSite//Images//header.PNG");
                // string imagePath = Server.MapPath("Images\\demo.PNG") + "";

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);

                image.Alignment = Element.ALIGN_CENTER;

                // set width and height
                image.ScalePercent(75f);

                // adding image to document
                pdfDoc.Add(image);


                Paragraph para = new Paragraph("Relatório", new Font(Font.FontFamily.TIMES_ROMAN, 22));
                Paragraph paraa = new Paragraph(nome, new Font(Font.FontFamily.TIMES_ROMAN, 22));
                para.Alignment = Element.ALIGN_CENTER;
                pdfDoc.Add(paraa);
                pdfDoc.Add(para);
                iTextSharp.text.pdf.draw.LineSeparator line1 = new iTextSharp.text.pdf.draw.LineSeparator(2F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
                pdfDoc.Add(new Chunk(line1));
                pdfDoc.Add(new Paragraph(" "));
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }




    }
}