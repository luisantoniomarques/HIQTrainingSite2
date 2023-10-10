using HIQTraining.ActiveDirectory;
using HIQTraining.DAL.Student;
using HIQTraining.Exceptions;
using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HIQTraining.Business.Student
{
    public class StudentManager
    {
        IStudentDao dao;
        CompanyUser companyUser;

        public StudentManager(IStudentDao dao)
        {
            this.dao = dao;
            companyUser = new CompanyUser();
        }

        /// <summary>
        /// Gets a paged list of students
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<StudentDetail> GetPagedStudents(int page = 1, int pageSize = 10)
        {
            return dao.GetPagedStudents(page, pageSize);
        }

        /// <summary>
        /// gets all students by name (paged)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<StudentDetail> GetPagedStudentsByName(string name, int page = 1, int pageSize = 10)
        {
            return this.GetPagedStudentsByName(name, 0, -1, page, pageSize);
        }

        /// <summary>
        /// gets all the students by name, company and status (paged)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<StudentDetail> GetPagedStudentsByName(string name, int? companyId, int? status, int page = 1, int pageSize = 10) {
            return dao.GetPagedStudentsByName(name, companyId, status, page, pageSize);
        }
        
        /// <summary>
        /// gets all students by name and email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Model.Student GetStudentByNameAndEmailAsync(string name, string email)
        {
            var task = dao.GetStudentByNameAndEmailAsync(name, email);
            task.Wait();

            return task.Result;
        }

        /// <summary>
        /// gets all students by name and email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Model.Student GetStudentByNameAndEmail(string name, string email)
        {
            return dao.GetStudentByNameAndEmail(name, email);
        }


        ///
        public Model.Student GetStudentByNameAndEmail(string name, string email, Model.DBEntities dbEntities)
        {
            return dao.GetStudentByNameAndEmail(name, email, dbEntities);
        }

        public Model.Student AddStudentAsync(Model.Student student, string user)
        {
            

            // TODO: validate model...
            var task = dao.AddStudentAsync(student, user);
            task.Wait();
            return task.Result;
        }


        /// <summary>
        /// Add a new student
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="companyId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Student AddStudent(string name, string email, int companyId, string phoneNumber, string observation, string user)
        {
            using (var db = new Model.DBEntities())
            {
                return this.AddStudent(name, email, companyId, phoneNumber, observation, user, db);
            }
        }

        public Model.Student AddStudentExternal(Model.Student student, string user)
        {
            using (var db = new Model.DBEntities())
            {
                return this.AddStudentExternal(student ,user, db);
            }
        }

        public Model.Student AddStudentExternal(Model.Student student, string user, Model.DBEntities db)
        {
            if (string.IsNullOrEmpty(user))
            {
                throw new ArgumentException("User parameter is required.");
            }
            int status = StudentStatus.GetActiveStatusId();

            Model.Student newStudent = new Model.Student();
            newStudent.Name = student.Name;
            newStudent.Email = student.Email;
            newStudent.CompanyId = student.CompanyId;
            newStudent.PhoneNumber = student.PhoneNumber;       
            newStudent.Status = status;
            newStudent.CreatedDate = DateTime.Now;
            newStudent.UserCreated = user;
            newStudent.Observation = student.Observation;

            dao.AddStudent(newStudent, user, db);
        
            return newStudent;
        }






        /// <summary>
        /// Adds a new student
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="companyId"></param>
        /// <param name="user"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Model.Student AddStudent(string name, string email, int companyId, string PhoneNumber , string observation, string user, Model.DBEntities db)
        {
            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("user parameter is required");


            // validate if user with email is known...
            Model.Student existsStudent = dao.GetStudentByNameAndEmail(name, email, db);
            if (existsStudent == null)
            {
                // validates if the person exists on the active directory
                List<ADUserDetail> students = companyUser.SearchUsersByName(companyId, name);
                if (students.Count == 1 && students[0].Email.Equals(email))
                {
                    Model.Student newStudent = new Model.Student();
                    newStudent.Name = students[0].Name;
                    newStudent.Email = students[0].Email;
                    newStudent.CompanyId = companyId;
                    newStudent.Status = StudentStatus.GetActiveStatusId();
                    newStudent.PhoneNumber = PhoneNumber;
                    newStudent.Observation = observation;

                    // add student...
                    dao.AddStudent(newStudent, user, db);
                    return newStudent;
                }
                else
                {
                    throw new HIQTrainingException(LocalResources.HIQResource.errorMessageUserNotFound);
                }
            }
            else
            {
                // student already exists...
                throw new HIQTrainingException(LocalResources.HIQResource.errorMessageStudentAlreadyExists);
            }
        }

        /// <summary>
        /// Updates the student data (phone number and status only)
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int UpdateStudent(int studentId, string phoneNumber, int? status, string observation, string user)
        {
            var student = dao.GetStudentById(studentId);
            if (student == null)
                throw new HIQTrainingException(LocalResources.HIQResource.errorMessageStudentNotFound);

            // these are the only fields that can be updated
            student.PhoneNumber = phoneNumber;
            student.Status = status;
            student.Observation = observation;
            student.UserUpdated = user;
            student.UpdateDate = DateTime.Now;

            return dao.UpdateStudent(student);
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Student GetStudentById(int id) {
            return dao.GetStudentById(id);
        }

        /// <summary>
        /// Gets the student detail by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StudentDetail GetStudentDetailById(int id)
        {
            return dao.GetStudentDetailById(id);
        }

        /// <summary>
        /// deletes a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            return dao.Delete(id);
        }


        /// <summary>
        /// gets a student by name, company, status
        /// </summary>
        /// <param name="name"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<StudentDetail> GetStudentsByName(string name, int? company, int? status, int pageNumber, int pageSize)
        {
            IPagedList<StudentDetail> list = dao.GetPagedStudentsByName(name, company, status, pageNumber, pageSize);
            return list;
        }

        public IEnumerable<StudentDetail> SearchExternalUsersByName(string userName)
        {
            return dao.SearchExternalUsersByName(userName);
        }
        public IEnumerable<StudentDetail> GetStudents(string studentName, int companyId)
        {
            return dao.GetStudents(studentName, companyId);
        }
    }
}
