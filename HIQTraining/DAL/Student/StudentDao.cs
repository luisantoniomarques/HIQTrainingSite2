using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HIQTraining.DAL.Student {
    public class StudentDao : IStudentDao
    {

        /// <summary>
        /// Gets a paged list of students ordered by name
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<StudentDetail> GetPagedStudents(int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var students = from s in db.Students
                               join c in db.Companies on s.CompanyId equals c.Id
                               orderby s.Name
                               select new StudentDetail {
                                   Id = s.Id,
                                   Name = s.Name,
                                   CompanyName = c.Name,
                                   Email = s.Email,
                                   Status = s.Status, 
                                   StudentNumberInscriptions = s.Inscriptions.Count
                               };

                return students.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// Searches a student by name company and status
        /// </summary>
        /// <param name="name"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<StudentDetail> GetPagedStudentsByName(string name, int? company, int? status, int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var students = 
                               from s in db.Students
                               join c in db.Companies on s.CompanyId equals c.Id
                               select new StudentDetail {
                                   Id = s.Id,
                                   Name = s.Name,
                                   CompanyId = s.CompanyId,
                                   CompanyName = c.Name,
                                   Email = s.Email,
                                   Status = s.Status,
                                   StudentNumberInscriptions = s.Inscriptions.Count
                               };

                if (!string.IsNullOrWhiteSpace(name))
                    students = students.Where(s => s.Name.StartsWith(name));

                if (company.HasValue && company >= 0)
                    students = students.Where(c =>c.CompanyId == company.Value);

                if (status.HasValue && status >= 0)
                    students = students.Where(s => s.Status == status.Value);

                return students.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// gets a student by name and email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Model.Student> GetStudentByNameAndEmailAsync(string name, string email) {
            using(DBEntities db = new DBEntities()) {
                var student = await (from s in db.Students
                                     where (s.Name == name && s.Email == email)
                                     select s).ToListAsync();

                if(student.Count == 1)
                    return student[0];
            }

            return null;
        }

        /// <summary>
        /// gets a student by name and email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Model.Student GetStudentByNameAndEmail(string name, string email) {
            using(DBEntities db = new DBEntities()) {
                var student = (from s in db.Students
                               where (s.Name == name && s.Email == email)
                               select s).ToList();

                if(student.Count == 1)
                    return student[0];
                if (!student.Any())
            throw new Exception("estudante não encontrado.");
            }

            
            return null;
        }

        /// <summary>
        /// gets a student by name and email
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Model.Student GetStudentByNameAndEmail(string name, string email, Model.DBEntities db) {
            var student = (from s in db.Students
                           where (s.Name == name && s.Email == email)
                           select s).ToList();

            if(student.Count == 1)
                return student[0];
            else
                return null;
        }

        /// <summary>
        /// Adds a new student
        /// </summary>
        /// <param name="student"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Model.Student> AddStudentAsync(Model.Student student, string user) {
            using(DBEntities db = new DBEntities()) {
                student.UserCreated = user;
                student.CreatedDate = DateTime.Now;

                db.Entry(student).State = EntityState.Added;
                await db.SaveChangesAsync();
                return student;
            }
        }

        /// <summary>
        /// adds a new student
        /// </summary>
        /// <param name="student"></param>
        /// <param name="user"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public int AddStudent(Model.Student student, string user, Model.DBEntities db) {
            student.UserCreated = user;
            student.CreatedDate = DateTime.Now;

            db.Entry(student).State = EntityState.Added;
            return db.SaveChanges();
        }

        /// <summary>
        /// Gets a student by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Student GetStudentById(int id) {
            using(DBEntities db = new DBEntities()) {
                var student = db.Students.Find(id);
                return student;
            }
        }

        /// <summary>
        /// gets all the details from a student
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StudentDetail GetStudentDetailById(int id) {
            using(DBEntities db = new DBEntities()) {
                var student = db.Students
                                    .Where(s => s.Id == id)
                                    .Select(x => new StudentDetail {
                                        Id = x.Id,
                                        Name = x.Name,
                                        CompanyId = x.CompanyId,
                                        CompanyName = x.Company.Name,
                                        PhoneNumber = x.PhoneNumber,
                                        Email = x.Email,
                                        Status = x.Status,
                                        Observation = x.Observation,
                                        UserCreated = x.UserCreated,
                                        CreatedDate = x.CreatedDate,
                                        UserUpdated = x.UserUpdated,
                                        UpdateDate = x.UpdateDate
                                    }).FirstOrDefault();
                return student;
            }
        }

        public int AddStudent(Model.Student student, string user) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the student data
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public int UpdateStudent(Model.Student student) {
            using(DBEntities db = new DBEntities()) {
                db.Entry(student).State = EntityState.Modified;
                return db.SaveChanges();
            }
        }

        public int Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var student = db.Students.Find(id);
                if (student != null)
                {
                    db.Entry(student).State = System.Data.Entity.EntityState.Deleted;
                }

                return db.SaveChanges();
            }
        }

        public IEnumerable<StudentDetail> SearchExternalUsersByName(string userName)
        {
            using (DBEntities db = new DBEntities())
            {
                var studentsList = (from students in db.Students
                                    join companies in db.Companies on students.CompanyId equals companies.Id
                                    where students.Name.StartsWith(userName) && companies.External
                                    select new StudentDetail { Id = students.Id,
                                                               Name = students.Name,
                                                               Email = students.Email });
                //falta filtrar por estudante externo
                //return db.Students.Where(c => c.Name.StartsWith(userName) && c.Company.External).Select(x => new StudentDetail { Id = x.Id, Name = x.Name, Email= x.Email}).ToList();
                return studentsList.ToList();

            }
        }
        public IEnumerable<StudentDetail> GetStudents(string studentName, int companyId)
        {
            using (DBEntities db = new DBEntities())
            {
                //saber se a empresa é interna ou externa
                var status = (from c in db.Companies
                             where c.Id == companyId
                             select c.External).Single();
     
               var student = from s in db.Students
                             join c in db.Companies on s.CompanyId equals c.Id
                             where s.Name.ToLower().StartsWith(studentName.ToLower()) && c.External == status
                             select new StudentDetail
                                  {
                                      Id = s.Id,
                                      Name = s.Name,
                                      Email = s.Email
                                  };


                return student.ToList();
            }


               
        }

  
    }

}
