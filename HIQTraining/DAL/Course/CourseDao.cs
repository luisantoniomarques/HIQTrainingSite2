using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PagedList;

namespace HIQTraining.DAL.Course {
    public class CourseDao : ICourseDao {

        /// <summary>
        /// Gets all courses 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseDetail> GetCourses() {
           
            using(DBEntities db = new DBEntities()) {
                var courses = (from course in db.Courses
                               join f in db.FormativeEntities on course.FormativeEntityId equals f.Id
                               join l in db.CourseLevels on course.LevelId equals l.Id
                               join th in db.Teachers on course.TeacherId equals th.Id
                               orderby course.StartDate ascending
                               select new CourseDetail {
                                   Id = course.Id,
                                   Name = course.Name,
                                   Teacher = th.Name,
                                   Code = course.Code,
                                   Level = l.Name,
                                   FormativeEntity = f.Name,
                                   Status = course.Status,
                                   StartDate = course.StartDate,
                                   NumberOfStudents = course.Inscriptions.Count
                               }).ToList();

                return courses.ToList();
            }
        }

        /// <summary>
        /// returns all courses
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private IQueryable<CourseDetail> GetPagedCourses(DBEntities db) {

            var courses = from course in db.Courses
                          join f in db.FormativeEntities on course.FormativeEntityId equals f.Id
                          join l in db.CourseLevels on course.LevelId equals l.Id
                          join t in db.CourseTypes on course.CourseTypeId equals t.Id
                          join th in db.Teachers on course.TeacherId equals th.Id
                          orderby course.StartDate ascending
                          select new CourseDetail {
                              Id = course.Id,
                              Name = course.Name,
                              Teacher = th.Name,
                              Code = course.Code,
                              Level = l.Name,
                              Type = t.Name,
                              FormativeEntity = f.Name,
                              Status = course.Status,
                              StartDate = course.StartDate,
                              EndDate = course.CloseDate,
                              NumberOfStudents = course.Inscriptions.Count
                          };

            return courses;

        }

        /// <summary>
        /// gets all courses that start with a string
        /// </summary>
        /// <param name="courseName"></param>
        /// <returns></returns>
        public IEnumerable<CourseDetail> GetCoursesByName(string courseName) {
            using(DBEntities db = new DBEntities()) {
                var courses = (from course in db.Courses
                               where course.Name.StartsWith(courseName)
                               select new CourseDetail {
                                   Id = course.Id,
                                   Name = course.Name,
                               }).ToList();

                return courses;
            }
        }

        /// <summary>
        /// returns all courses by name
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseDetail> GetPagedCoursesByName(string courseName, int page, int pageSize) {
            using(DBEntities db = new DBEntities()) {
                var courses = (from course in db.Courses
                               where course.Name.StartsWith(courseName)
                               select new CourseDetail {
                                   Id = course.Id,
                                   Name = course.Name,
                               }).ToList();

                return courses.ToPagedList(page, pageSize);
            }
        }

        /// <summary>
        /// returns the courses where a student has an inscription
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseDetail> GetCoursesByStudentId(int studentId, int page, int pageSize)
        {
            using(var db = new DBEntities())
            {
                var courses = from c in db.Courses
                              join i in db.Inscriptions on c.Id equals i.CourseId
                              where i.StudentId == studentId
                              orderby c.StartDate descending
                              select new CourseDetail
                              {
                                  Id = c.Id,
                                  Name = c.Name,
                                  Code = c.Code,
                                  Teacher = c.Teacher.Name,
                                  Level = c.CourseLevel.Name,
                                  FormativeEntity = c.FormativeEntity.Name,
                                  Status = c.Status,
                                  StartDate = c.StartDate
                              };

                

                var studentAttendances = from s in db.Students
                                         join i in db.Inscriptions on s.Id equals i.StudentId
                                         join c in db.Courses on i.CourseId equals c.Id
                                         join a in db.Attendances on i.Id equals a.InscriptionId
                                         where s.Id.Equals(studentId)
                                         group a by new { a.Status, s.Name, c.Id } into temp
                                         select new StudentSuccessRateDetail
                                         {
                                             CourseId = temp.Key.Id,
                                             StudentName = temp.Key.Name,
                                             Status = temp.Key.Status,
                                             Count = temp.Count()
                                         };

                IPagedList<CourseDetail> coursesReturn = courses.ToPagedList(page, pageSize);

                foreach (CourseDetail course in coursesReturn)
                {
                    var courseAttendances = studentAttendances.Where(x => x.CourseId == course.Id);

                    int totalAttendances = courseAttendances.Count();

                    if(totalAttendances > 0)
                    {
                        int positiveAttendances = courseAttendances.Where(x => x.Status == 1).Count();

                        course.CourseAttendancePercentage = Convert.ToInt32(Decimal.Divide(positiveAttendances, totalAttendances) * 100);
                    }
                }

                return coursesReturn;
            }
        }

        /// <summary>
        /// returns the courses where a teacher has taught
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IEnumerable<CourseDetail> GetCoursesByTeacherId(int teacherId)
        {
            using (var db = new DBEntities())
            {
                var courses = from c in db.Courses
                              where c.TeacherId == teacherId
                              orderby c.StartDate descending
                              select new CourseDetail
                              {
                                  Id = c.Id,
                                  Name = c.Name,
                                  Code = c.Code,
                                  Teacher = c.Teacher.Name,
                                  Level = c.CourseLevel.Name,
                                  FormativeEntity = c.FormativeEntity.Name,
                                  Status = c.Status,
                                  StartDate = c.StartDate,
                                  Effort = c.Effort                                                                
                              };

                return courses.ToList();
            }
        }

        public int GetTeacherNumberOfCourses(int? teacherId)
        {
            using (var db = new DBEntities())
            {
                int coursesNumber = db.Courses.Where(x => x.TeacherId == teacherId).Count();

                return coursesNumber;
            }
        }

        public int GetTeacherIdByEmail(string TeacherEmail)
        {
            using (var db = new DBEntities())
            {
                //int coursesNumber = db.Courses.Where(x => x.TeacherId == teacherId).Count();
                int teacherId = db.Teachers.Where(x => x.Email == TeacherEmail).Select(i => i.Id).FirstOrDefault();
                return teacherId;
            }
        }


        /// <summary>
        /// Add a new course
        /// </summary>
        /// <param name="course"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Model.Course Add(Model.Course course, string user) {
            using(DBEntities db = new DBEntities()) {
                // TODO: excel
                course.UserCreated = user;
                course.CreatedDate = DateTime.Now;
                course.CourseTypeId = course.CourseTypeId;
                course.Status = 0;
                course.InscriptionEmail = 0;
                course.ConfirmationEmail = 0;
                course.Reminder = 0;
                course.Documentation = 0;
                course.Intranet = 0;
                course.Material = 0;
                course.Dtp = 0;
                course.Certificates = 0;
                course.Avaliation = 0;
                course.Confidential = 0;
                course.UniqueReport = 0;

                db.Entry(course).State = System.Data.Entity.EntityState.Added;
                db.Courses.Add(course);

                db.SaveChanges();

                return course;
            }
        }

        /// <summary>
        /// Return whether a course code exists or not 
        /// </summary>
        /// <param name="course"></param>
        /// <returns>true if exists (cant add)</returns>
        public bool GetCode(Model.Course course) {
            using(DBEntities db = new DBEntities()) {
                return db.Courses.Any(c => !c.Name.Equals(course.Name) && c.Code.Equals(course.Code) && !c.Id.Equals(course.Id));
            }
        }
        /// <summary>
        /// validate if exists a course in the date for that teacher 
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool ValidateDate(Model.Course course, DateTime date)
        {
            using (DBEntities db = new DBEntities())
            {
                bool foundDate = false;
                //query n esta bem feita
                var courses = from c in db.Courses
                              join i in db.Calendars on c.Id equals i.CourseId
                              where c.Teacher.Name.Equals(course.Teacher.Name) &&
                                    c.EndHour == course.EndHour &&
                                    c.StartHour == course.StartHour &&
                                    i.CalendarDate == date
                              select c;

                List<Model.Course> list = courses.ToList();

                if (list != null && list.Count() > 0)
                    foundDate = true;
                             


                return foundDate;
                
            }
        }
        /// <summary>
        /// deletes a course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(int id) {
            using(DBEntities db = new DBEntities()) {
                var course = db.Courses.Find(id);
                if(course != null) {
                    db.Entry(course).State = System.Data.Entity.EntityState.Deleted;
                }
                
                return db.SaveChanges();
            }
        }

        

        /// <summary>
        /// Gets an course from the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseFullDetail GetCourseById(int id) {
            using(DBEntities db = new DBEntities()) {
                CourseFullDetail course = (from c in db.Courses
                                           join level in db.CourseLevels on c.LevelId equals level.Id
                                           join type in db.CourseTypes on c.CourseTypeId equals type.Id
                                           join location in db.CourseLocations on c.LocationId equals location.Id
                                           join f in db.FormativeEntities on c.FormativeEntityId equals f.Id
                                           join teacher in db.Teachers on c.TeacherId equals teacher.Id
                                           where c.Id.Equals(id)
                                           select new CourseFullDetail {
                                               Id = id,
                                               Name = c.Name,
                                               Level = level.Name,
                                               LevelId = level.Id,
                                               Type = type.Name,
                                               TypeId = type.Id,
                                               Code = c.Code,
                                               PayRoll = c.PayRoll,
                                               Location = location.Name,
                                               LocationId = location.Id,
                                               Teacher = teacher.Name,
                                               TeacherId = teacher.Id,
                                               Entity = f.Name,
                                               EntityId = f.Id,
                                               Observation = c.Observation,
                                               StartHour = c.StartHour,
                                               EndHour = c.EndHour,
                                               Effort = c.Effort,
                                               StartDate = c.StartDate,
                                               CloseDate = c.CloseDate,
                                               CanceledObservation = c.CanceledObservation,
                                              StatusId = c.Status
                                           }).SingleOrDefault();

                return course;
            }
        }
      
        /// <summary>
        /// updates a course
        /// </summary>
        /// <param name="course"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int Update(Model.Course course, string username) {
            using(DBEntities db = new DBEntities()) {
                var updateCourse = db.Courses.Find(course.Id);

                if(updateCourse == null) {
                    return 0;
                }

                updateCourse.UserUpdated = username;
                updateCourse.UpdateDate = DateTime.Now;
                updateCourse.Name = course.Name;
                updateCourse.LevelId = course.LevelId;
                updateCourse.CourseTypeId = course.CourseTypeId;
                updateCourse.Code = course.Code;
                updateCourse.LocationId = course.LocationId;
                updateCourse.TeacherId = course.TeacherId;
                updateCourse.FormativeEntityId = course.FormativeEntityId;
                updateCourse.Observation = course.Observation;
                updateCourse.StartHour = course.StartHour;
                updateCourse.EndHour = course.EndHour;
                updateCourse.Effort = course.Effort;
                updateCourse.Status = course.Status;
                updateCourse.PayRoll = course.PayRoll;
                updateCourse.CanceledObservation = course.CanceledObservation;
                if(course.Calendars.Count() > 0) {
                    AddCourseDay(course, username);
                }

              

                db.Entry(updateCourse).State = System.Data.Entity.EntityState.Modified;

                return db.SaveChanges();
            }
        }
        /// <summary>
        /// function to update the status based on the start date and end date of the course
        /// </summary>
        /// <returns></returns>
        public int UpdateStatus()
        {
            using (DBEntities db = new DBEntities())
            {
                var courses = db.Courses.ToList();
                foreach (var item in courses)
                {
                    if (item.Status != 0)
                        continue;

                    if (item.StartDate <= DateTime.Now && item.CloseDate >= DateTime.Now)
                    {
                        item.Status = 1;
                    } else if (item.CloseDate <= DateTime.Now) {
                        item.Status = 2;
                    }
                    else if (item.StartDate >= DateTime.Now && item.CloseDate <= DateTime.Now)
                    {
                        item.Status = 3;
                    }
                }
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// adds a new calendar day to a course
        /// </summary>
        /// <param name="course"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private int AddCourseDay(Model.Course course, string username) {
            using(DBEntities db = new DBEntities()) {

                foreach(var item in course.Calendars) {
                    Model.Calendar calendar = new Model.Calendar {
                        CourseId = course.Id,
                        CalendarDate = item.CalendarDate,
                        UserCreated = username,
                        CreatedDate = DateTime.Now,
                        Status = 1
                    };

                    db.Entry(calendar).State = System.Data.Entity.EntityState.Added;
                    db.Calendars.Add(calendar);
                }

                return db.SaveChanges();
            }
        }


        /// <summary>
        /// disables a date from a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="date"></param>
        /// <param name="observation"></param>
        /// <returns></returns>
        public int DisabledCourseDate(int courseId, DateTime date, string observation) {
            using (DBEntities db = new DBEntities())
            {

                var calendar = (from c in db.Calendars
                                where c.CourseId.Equals(courseId)
                                where c.CalendarDate.Equals(date)
                                select c).SingleOrDefault();

                calendar.Observation = observation;
                calendar.Status = 0;

                RemoveAttendance(calendar.Id);
                db.Calendars.Remove(calendar);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// remove the attendance from a specific date 
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        private int RemoveAttendance(int calendarId) {
            using(DBEntities db = new DBEntities()) {
                var attendances = from a in db.Attendances
                                  where a.CalendarId.Equals(calendarId)
                                  select a;

                db.Attendances.RemoveRange(attendances);

                return db.SaveChanges();
            }
        }
      

        /// <summary>
        /// returns the search results from the course page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseDetail> GetCoursesSearchResults(int page, int pageSize) {
            return this.GetCoursesSearchResults(null, null, null, null, null, null, page, pageSize);
        }
        
        /// <summary>
        /// returns the search results from the course page
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="courseLevel"></param>
        /// <param name="teacherName"></param>
        /// <param name="date"></param>
        /// <param name="estado"></param>
        /// <param name="entidade"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<CourseDetail> GetCoursesSearchResults(string courseName, string courseLevel, string teacherName, DateTime? date, int? estado, string entidade, int page, int pageSize)
        {
            UpdateStatus();

            using (DBEntities db = new DBEntities()) {
                var courses = GetPagedCourses(db);

                if(!string.IsNullOrEmpty(courseName)) {
                    courses = courses.Where(n => n.Name.StartsWith(courseName));
                }

                if(!string.IsNullOrEmpty(courseLevel)) {
                    courses = courses.Where(l => l.Level.StartsWith(courseLevel));
                }

                if(!string.IsNullOrEmpty(teacherName)) {
                    courses = courses.Where(t => t.Teacher.StartsWith(teacherName));
                }

                if(date.HasValue) {
                    courses = courses.Where(d => d.StartDate == date.Value);
                }

                if(estado.HasValue) {
                    courses = courses.Where(e => e.Status.Equals(estado.Value));
                }

                if(!string.IsNullOrEmpty(entidade)) {
                    courses = courses.Where(f => f.FormativeEntity.StartsWith(entidade));
                }

                return courses.OrderBy(x => x.Name).ToPagedList(page, pageSize);
            }
        }

        public IQueryable<CourseDetail> GetPagedCourses(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

    }
}
