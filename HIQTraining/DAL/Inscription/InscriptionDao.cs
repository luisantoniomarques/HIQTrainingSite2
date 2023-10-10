using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.DAL.Inscription
{
    public class InscriptionDao : IInscriptionDao
    {

        public CourseDetail GetCourseDateTime(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                var det =
                    from co in db.Courses
                    where co.Id == courseId
                    select new CourseDetail
                    {
                        StartDate = co.StartDate,
                        StartHour = co.StartHour,
                        EndHour = co.EndHour
                    };
                return det.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets all the students inscriptions for the courseId
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<InscriptionDetail> GetCourseInscriptionList(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                var inscriptions = 
                        from s in db.Inscriptions
                        join c in db.Companies on s.Student.CompanyId equals c.Id
                        where s.CourseId == courseId
                        select new InscriptionDetail
                        {
                            Id = s.Id,
                            StudentId = s.StudentId,
                            StudentName = s.Student.Name,
                            CourseId = s.CourseId,
                            CourseName = s.Course.Name,
                            CompanyName = c.Name,
                            TypeId = s.TypeId,
                            TypeName = s.InscriptionType.Description,
                            Status = s.Status,
                            Observation = s.Observation,
                            UserCreated = s.UserCreated,
                            CreatedDate = s.CreatedDate,
                            UserUpdated = s.UserUpdated,
                            UpdateDate = s.UpdateDate

                        };
                return inscriptions.ToList();
            }
        }

        /// <summary>
        /// Gets a course inscription detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public InscriptionDetail GetInscriptionById(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var inscription =
                        from s in db.Inscriptions
                        join c in db.Companies on s.Student.CompanyId equals c.Id
                        where s.Id == id
                        select new InscriptionDetail
                        {
                            Id = s.Id,
                            StudentId = s.StudentId,
                            StudentName = s.Student.Name,
                            CourseId = s.CourseId,
                            CourseName = s.Course.Name,
                            CompanyName = c.Name,
                            TypeId = s.TypeId,
                            TypeName = s.InscriptionType.Description,
                            Status = s.Status,
                            Observation = s.Observation,
                            UserCreated = s.UserCreated,
                            CreatedDate = s.CreatedDate,
                            UserUpdated = s.UserUpdated,
                            UpdateDate = s.UpdateDate

                        };

               return inscription.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a student inscription
        /// </summary>
        /// <param name="id"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Model.Inscription GetInscriptionById(int id, DBEntities db)
        {
            var inscription =
                        from s in db.Inscriptions
                        join c in db.Companies on s.Student.CompanyId equals c.Id
                        where s.Id == id
                        select s;
            return inscription.FirstOrDefault();
        }

        /// <summary>
        /// Updates a student inscription
        /// </summary>
        /// <param name="inscription"></param>
        /// <param name="db"></param>
        public void UpdateInscription(Model.Inscription inscription, DBEntities db)
        {
            db.Entry(inscription).State = System.Data.Entity.EntityState.Modified;
        }

        /// <summary>
        /// Gets a student by courseId and studentId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public InscriptionDetail GetInscriptionByCourseIDAndStudentId(int courseId, int studentId)
        {
            using (DBEntities db = new DBEntities())
            {
                var inscription =
                        from s in db.Inscriptions
                        where (s.StudentId == studentId && s.CourseId == courseId)
                        select new InscriptionDetail
                        {
                            Id = s.Id,
                            StudentId = s.StudentId,
                            StudentName = s.Student.Name,
                            CourseId = s.CourseId,
                            CourseName = s.Course.Name,
                            TypeId = s.TypeId,
                            TypeName = s.InscriptionType.Description,
                            Status = s.Status,
                            Observation = s.Observation,
                            UserCreated = s.UserCreated,
                            CreatedDate = s.CreatedDate,
                            UserUpdated = s.UserUpdated,
                            UpdateDate = s.UpdateDate

                        };
                return inscription.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a student by courseId and studentId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public List<InscriptionDetail> GetInscriptionByStudentId(int studentId, int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                var inscription =
                        from s in db.Inscriptions
                        where (s.StudentId == studentId) && s.CourseId == courseId
                        select new InscriptionDetail
                        {
                            Id = s.Id,
                            CourseId = s.CourseId,
                            StudentId = s.StudentId,
                            StudentName = s.Student.Name,
                            TypeId = s.TypeId,
                            TypeName = s.InscriptionType.Description,
                            Status = s.Status,
                            Observation = s.Observation,
                            UserCreated = s.UserCreated,
                            CreatedDate = s.CreatedDate,
                            UserUpdated = s.UserUpdated,
                            UpdateDate = s.UpdateDate

                        };

                return inscription.ToList();
            }
        }

        /// <summary>
        /// Adds a new student course inscription
        /// </summary>
        /// <param name="inscription"></param>
        /// <param name="user"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Model.Inscription AddInscription(Model.Inscription inscription, string user, Model.DBEntities db)
        {
            /*
            var dt = GetCourseDateTime(inscription.CourseId);

            var allIns = GetInscriptionByStudentId(inscription.StudentId);
            var allInsList = allIns.ToList();

            for (int i = 0; i < allInsList.Count; i++)
            {
                var c = GetCourseDateTime(allInsList[i].CourseId);

                if (c.StartDate == dt.StartDate && c.StartHour == dt.StartHour)
                {
                    throw new Exception("same date/time"); //todo
                }
            }

          
            */
            inscription.UserCreated = user;
            inscription.CreatedDate = DateTime.Now;

            db.Entry(inscription).State = System.Data.Entity.EntityState.Added;
            db.SaveChanges();
            return inscription;
        }

        /// <summary>
        /// Removes a student course inscription 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id)
        {
            using (DBEntities db = new DBEntities())
            {
                var inscription = db.Inscriptions.Find(id);
                if (inscription == null)
                    //throw new Exception("not Found"); // TODO ApplicationException
                    return 0; // not found

                db.Entry(inscription).State = System.Data.Entity.EntityState.Deleted;
                return db.SaveChanges();
            }
        }

      
    }
            
}
