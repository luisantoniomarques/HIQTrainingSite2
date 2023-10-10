using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;
using PagedList;

namespace HIQTraining.DAL.Teacher {
    public class TeacherDao : ITeacherDao {

        /// <summary>
        /// Adds a new teacher
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public Model.Teacher Add(Model.Teacher teacher) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(teacher).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return teacher;
            }
        }

        /// <summary>
        /// deletes a teacher
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id) {
            using(DBEntities db = new DBEntities()) {
                var teacher = db.Teachers.Find(id);
                if(teacher == null) {
                    throw new Exception("Formado não encontrado."); // TODO: 
                }
                db.Entry(teacher).State = System.Data.Entity.EntityState.Deleted;

                return db.SaveChanges();
            }
        }

        /// <summary>
        /// returns all teachers
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TeacherDetail> GetPagedTeachers(int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var teachers = from t in db.Teachers
                               join c in db.Companies on t.CompanyId equals c.Id
                               select new TeacherDetail {
                                   Id = t.Id,
                                   Name = t.Name,
                                   Company = c.Name,
                                   CompanyId = c.Id, 
                                   StatusId = t.Status,
                                   TeacherNumberCourses = t.Courses.Count
                               };

                return teachers.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// returns all teachers
        /// </summary>
        /// <param name="teacherName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TeacherDetail> GetPagedTeachersByName(string teacherName, int page, int pageSize) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// return reacher by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Teacher GetTeacherById(int id) {
            using(DBEntities db = new DBEntities()) {
                return db.Teachers.Find(id);
            }
        }

        /// <summary>
        /// returns teacher by name and email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Model.Teacher GetTeacherByNameAndEmail(string name, string email, DBEntities db) {
            return db.Teachers.Where(t => t.Name.Equals(name) && t.Email.Equals(email)).SingleOrDefault();
        }

        public Model.Teacher GetTeacherByName(string name, DBEntities db)
        {
            return db.Teachers.Where(t => t.Name.Equals(name)).SingleOrDefault();
        }

        /// <summary>
        /// returns the teachers details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherDetail GetTeacherDetailById(int id) {
            using(DBEntities db = new DBEntities()) {
                var teacher = (from t in db.Teachers
                               join c in db.Companies on t.CompanyId equals c.Id
                               where t.Id.Equals(id)
                               select new TeacherDetail {
                                   Company = c.Name,
                                   CompanyId = c.Id,
                                   Name = t.Name,
                                   Id = t.Id,
                                   Email = t.Email,
                                   Phone = t.PhoneNumber,
                                   StatusId = t.Status,
                                   TecnicalSkills = t.TecnicalSkills,
                                   //PayRoll = t.PayRoll
                               }).SingleOrDefault();

                return teacher;
            }
        }

        /// <summary>
        /// gets all the teachers
        /// </summary>
        /// <returns></returns>
        public IQueryable<TeacherDetail> GetTeachers(DBEntities db) {
                var teachers = from t in db.Teachers
                               join c in db.Companies
                               on t.CompanyId equals c.Id
                               select new TeacherDetail {
                                   Id = t.Id,
                                   Name = t.Name,
                                   Company = c.Name, 
                                   CompanyId = c.Id, 
                                   StatusId = t.Status
                               };

                return teachers;
        }

        /// <summary>
        /// gets a teacher by name
        /// </summary>
        /// <param name="teacherName"></param>
        /// <returns></returns>
        public IEnumerable<TeacherDetail> GetTeachers(string teacherName) {
            using(DBEntities db = new DBEntities()) {

                var teachers = from t in db.Teachers
                               join c in db.Companies
                               on t.CompanyId equals c.Id
                               where t.Name.ToLower().StartsWith(teacherName.ToLower())
                               select new TeacherDetail {
                                   Id = t.Id,
                                   Name = t.Name,
                                   Company = c.Name, 
                                   Email = t.Email, 
                                   StatusId = t.Status
                               };

                return teachers.ToList();
            }
        }

        /// <summary>
        /// returns the search results from the teacher page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="state"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TeacherDetail> GetTeachersSearchResults(string name, int? companyId, int? state, int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
          //      var teachers = GetTeachers(db);

                var teachers = from t in db.Teachers
                               join c in db.Companies on t.CompanyId equals c.Id
                               select new TeacherDetail
                               {
                                   Id = t.Id,
                                   Name = t.Name,
                                   Company = c.Name,
                                   CompanyId = c.Id,
                                   StatusId = t.Status,
                                   TeacherNumberCourses = t.Courses.Count
                               };

                if (!string.IsNullOrEmpty(name)) {
                    teachers = teachers.Where(n => n.Name.StartsWith(name));
                }

                if(companyId.HasValue && companyId >= 0) {
                    teachers = teachers.Where(c => c.CompanyId.Equals(companyId.Value));
                }

                if(state.HasValue && state.Value >= 0) {
                    teachers = teachers.Where(s => s.StatusId.Equals(state.Value));
                }
                //alterar
          

                return teachers.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// updats a teacher
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public int Update(Model.Teacher teacher) {
            using(DBEntities db = new DBEntities()) {
                Model.Teacher updatedTeacher = db.Teachers.Find(teacher.Id);

                if(updatedTeacher == null) {
                    throw new Exception("Formador não encontrado.");
                    // TODO: Error
                }

                updatedTeacher.CompanyId = teacher.CompanyId;
                updatedTeacher.Email = teacher.Email;
                updatedTeacher.Name = teacher.Name;
                updatedTeacher.PhoneNumber = teacher.PhoneNumber;
                updatedTeacher.Status = teacher.Status;
                updatedTeacher.TecnicalSkills = teacher.TecnicalSkills;
                //updatedTeacher.PayRoll = teacher.PayRoll;

                db.Entry(updatedTeacher).State = System.Data.Entity.EntityState.Modified;

                return db.SaveChanges();
            }
        }
    }
}
