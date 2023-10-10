using HIQTraining.ActiveDirectory;
using HIQTraining.Business.Calendar;
using HIQTraining.Business.Company;
using HIQTraining.Business.Student;
using HIQTraining.DAL.Calendar;
using HIQTraining.DAL.Company;
using HIQTraining.DAL.Inscription;
using HIQTraining.DAL.Student;
using HIQTraining.Exceptions;
using HIQTraining.LocalResources;
using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.Model;

namespace HIQTraining.Business.Inscription
{
    public class InscriptionManager
    {
        IInscriptionDao dao;
        CompanyUser companyUser;
        CompanyManager companyManager;
        CalendarManager calendarManager;

        public InscriptionManager(IInscriptionDao dao)
        {
            this.dao = dao;
            companyUser = new CompanyUser();
            companyManager = new CompanyManager(new CompanyDao());
            calendarManager = new CalendarManager(new CalendarDao());
        }


        /// <summary>
        /// Gets all the students inscriptions for the courseId
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<InscriptionDetail> GetCourseInscriptionList(int courseId)
        {
            return dao.GetCourseInscriptionList(courseId);
        }

        /// <summary>
        /// gets an inscription by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InscriptionDetail GetInscriptionById(int id)
        {
            return dao.GetInscriptionById(id);
        }

        /// <summary>
        /// Updates the student inscription status and observations
        /// </summary>
        /// <param name="inscriptionList"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int SaveInscriptionStatus(List<InscriptionDetail> inscriptionList, string user)
        {
            int result = 0;
            using (var db = new Model.DBEntities())
            {
                foreach (var inscription in inscriptionList)
                {
                    Model.Inscription studentInscription = dao.GetInscriptionById(inscription.Id, db);
                    if (studentInscription != null)
                    {
                        studentInscription.Status = inscription.Status;
                        studentInscription.Observation = inscription.Observation;
                        studentInscription.UserUpdated = user;
                        studentInscription.UpdateDate = DateTime.Now;

                        dao.UpdateInscription(studentInscription, db);
                    } // else throw Exception ???
                }

                result = db.SaveChanges();
            }
            return result;
        }

        /// <summary>
        /// adds a new inscription
        /// </summary>
        /// <param name="inscription"></param>
        /// <param name="companyId"></param>
        /// <param name="studentName"></param>
        /// <param name="email"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Inscription AddInscription(Model.Inscription inscription, int companyId, string studentName, string email, string user)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("user parameter is required");


            using (var db = new Model.DBEntities())
            {

                // Validates if the student is already inscript
                var inscriptionExists = dao.GetInscriptionByCourseIDAndStudentId(inscription.CourseId, inscription.StudentId);
               if (inscriptionExists != null)
                   throw new HIQTrainingException(LocalResources.HIQResource.errorMessageStudentInscriptionAlreadyMade);





                //// validate if user and email is known...
                //Model.Student student = studentManager.GetStudentByNameAndEmail(studentName, email, db);
                //if (student == null)
                //{
                //    List<ADUserDetail> students = companyUser.SearchUsersByName(companyId, studentName);
                //    if (students.Count == 1 && students[0].Email.Equals(email))
                //    {
                //        Model.Student newStudent = new Model.Student();
                //        newStudent.Name = studentName;
                //        newStudent.Email = email;
                //        newStudent.CompanyId = companyId;
                //        newStudent.Status = StudentStatus.GetActiveState();

                //        // first add new student...
                //        int i = studentManager.AddStudent(newStudent, user, db);

                //        // register student inscription
                //        inscription.StudentId = newStudent.Id;
                //    }
                //    else
                //    {
                //        throw new HIQTrainingException(LocalResources.HIQResource.errorMessageUserNotFound);
                //    }
                //}
                //else
                //{
                //    // Validates if the student is already inscript
                //    var inscriptionExists = dao.GetInscriptionByCourseIDAndStudentId(inscription.CourseId, student.Id);
                //    if (inscriptionExists != null)
                //        throw new HIQTrainingException(LocalResources.HIQResource.errorMessageStudentInscriptionAlreadyMade);

                //    inscription.StudentId = student.Id;
                //}


                inscription = dao.AddInscription(inscription, user, db);
            }

            return inscription;
        }

        public List<InscriptionDetail> GetInscriptionByStudentId(int studentId, int courseId)
        {
            return dao.GetInscriptionByStudentId(studentId, courseId);
        }

        /// <summary>
        /// Cancels the student inscription
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            var inscription = dao.GetInscriptionById(id);
            // inscription canot be canceled after the course starts
            var calendarList = calendarManager.GetCourseCalendar(inscription.CourseId);
            if (calendarList != null && calendarList.Count() > 0)
            {
                var minCalendarDate = calendarList.Min(c => c.Date);
                if (DateTime.Now.Date > minCalendarDate)
                    throw new HIQTrainingException(HIQResource.errorMessageCanNotCancelInscription);
            }

            return dao.Delete(id);
        }
    }
}
