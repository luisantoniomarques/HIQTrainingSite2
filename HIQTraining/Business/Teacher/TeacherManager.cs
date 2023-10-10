using HIQTraining.ActiveDirectory;
using HIQTraining.DAL.Teacher;
using HIQTraining.Exceptions;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.Utils;

namespace HIQTraining.Business.Teacher {
    public class TeacherManager {

        ITeacherDao dao;
        CompanyUser companyUser;

        public TeacherManager(ITeacherDao dao) {
            this.dao = dao;
            this.companyUser = new CompanyUser();
        }

        /// <summary>
        /// gets all teachers (paged)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TeacherDetail> GetPagedTeachers(int page, int pageSize) {
            return dao.GetPagedTeachers(page, pageSize);
        }

        /// <summary>
        /// Gets all teachers by name (paged)
        /// </summary>
        /// <param name="teacherName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TeacherDetail> GetPagedTeachersByName(string teacherName, int page, int pageSize) {
            return dao.GetPagedTeachersByName(teacherName, page, pageSize);
        }

        /// <summary>
        /// adds a new teacher
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Teacher AddTeacher(Model.Teacher teacher, string user) {
            using (var db = new Model.DBEntities())
            {
                return this.AddTeacher(teacher.Name, teacher.Email, teacher.CompanyId, teacher.PhoneNumber, teacher.TecnicalSkills, /*teacher.PayRoll,*/ user, db);
            }
        }

        private Model.Teacher AddTeacher(string name, string email, int companyId, string phoneNumber, string tecnicalSkills, string user, DBEntities db)
        {
            throw new NotImplementedException();
        }

        public Model.Teacher AddTeacherExternal(Model.Teacher teacher, string user)
        {
            using (var db = new Model.DBEntities())
            {
                return this.AddTeacherExternal(teacher.Name, teacher.Email, teacher.CompanyId, teacher.PhoneNumber, teacher.TecnicalSkills, /*teacher.PayRoll,*/ user, db);
            }
        }

        private Model.Teacher AddTeacherExternal(string name, string email, int companyId, string phoneNumber, string tecnicalSkills, string user, DBEntities db)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// adds a new teacher (checks if the teacher exists in the AD; if exists and not registed in the system yet, registers him) 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="companyId"></param>
        /// <param name="user"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Model.Teacher AddTeacherExternal(string name, string email, int companyId, string phoneNumber, string skills, string payroll, string user, Model.DBEntities db) {
            if(string.IsNullOrEmpty(user)) {
                throw new ArgumentException("User parameter is required.");
            }
            int status = TeacherStatus.GetActiveStatusId();
          

            Model.Teacher newTeacher = new Model.Teacher();
            newTeacher.Name = name;
            newTeacher.Email = email;
            newTeacher.CompanyId = companyId;
            newTeacher.PhoneNumber = phoneNumber;
            newTeacher.TecnicalSkills = skills;
            newTeacher.Status = status;
            newTeacher.TecnicalSkills = skills;
            //newTeacher.PayRoll = payroll;
            newTeacher.CreatedDate = DateTime.Now;
            newTeacher.UserCreated = user;

            dao.Add(newTeacher);

            return newTeacher;

        }

        public Model.Teacher AddTeacher(string name, string email, int companyId, string phoneNumber, string skills,string payroll, string user, Model.DBEntities db)
        {
            if (string.IsNullOrEmpty(user))
            {
                throw new ArgumentException("User parameter is required.");
            }

            Model.Teacher existsTeacher = dao.GetTeacherByNameAndEmail(name, email, db);
            if (existsTeacher == null)
            {
                List<ADUserDetail> teachers = companyUser.SearchUsersByName(companyId, name);
                if (teachers.Count == 1 && teachers.ElementAt(0).Email.Equals(email))
                {
                    Model.Teacher newTeacher = new Model.Teacher
                    {
                        Name = teachers.ElementAt(0).Name,
                        Email = teachers.ElementAt(0).Email,
                        CompanyId = companyId,
                        PhoneNumber = phoneNumber,
                        TecnicalSkills = skills,
                        //PayRoll = payroll,
                        Status = TeacherStatus.GetActiveStatusId(),
                        UserCreated = user,
                        CreatedDate = DateTime.Now
                    };

                    dao.Add(newTeacher);

                    return newTeacher;
                }
                else
                {
                    throw new HIQTrainingException(LocalResources.HIQResource.errorMessageUserNotFound);
                }
            }
            else
            {
                throw new HIQTrainingException(LocalResources.HIQResource.errorMessageTeacherAlreadyExists);
            }

        }

        /// <summary>
        /// gets all teachers by name (paged)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TeacherDetail> GetTeachersSearchResults(string name, int? companyId, int? state, int page = 1, int pageSize = Common.HIQTrainingConstants.General.MAX_SEARCH_RECORDS) {
            return dao.GetTeachersSearchResults(name, companyId, state, page, pageSize);
        }

        /// <summary>
        /// gets all teachers by name and email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Model.Teacher GetTeacherByNameAndEmail(string name, string email) {
            return dao.GetTeacherByNameAndEmail(name, email, new Model.DBEntities());
        }

        /// <summary>
        /// gets all teachers by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Model.Teacher GetTeacherByName(string name)
        {
            return dao.GetTeacherByName(name, new Model.DBEntities());
        }

        /// <summary>
        /// gets a teacher by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Teacher GetTeacherById(int id) {
            return dao.GetTeacherById(id);
        }

        /// <summary>
        /// gets the details from a teacher, by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherDetail GetTeacherDetailById(int id) {
            return dao.GetTeacherDetailById(id);
        }

        /// <summary>
        /// updates a teacher
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(Model.Teacher teacher, string user) {
            teacher.UserUpdated = user;
            teacher.UpdateDate = DateTime.Now;

            return dao.Update(teacher);
        }

        /// <summary>
        /// deletes a teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id) {
            return dao.Delete(id);
        }

        /// <summary>
        /// gets a teacher by name
        /// </summary>
        /// <param name="teacherName"></param>
        /// <returns></returns>
        public IEnumerable<TeacherDetail> GetTeachers(string teacherName) {
            return dao.GetTeachers(teacherName);
        }

    }
}
