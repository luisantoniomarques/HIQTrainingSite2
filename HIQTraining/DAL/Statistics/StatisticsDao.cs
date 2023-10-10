using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIQTraining.ModelDetail;
using HIQTraining.Model;
using System.Collections;
using System.Globalization;

namespace HIQTraining.DAL.Statistics {
    public class StatisticsDao : IStatisticsDao
    {

        /// <summary>
        /// number of days wich a course takes place
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        private int GetNumberCourseDays(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                return db.Calendars.Where(x => x.CourseId.Equals(courseId)).Count();

            }
        }

        /// <summary>
        /// gets the success rate of a student
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        private IEnumerable<StudentSuccessRateDetail> GetAttendaces(int courseId, int studentId)
        {
            using (DBEntities db = new DBEntities())
            {
                var studentsRate = from s in db.Students
                                   join i in db.Inscriptions on s.Id equals i.StudentId
                                   join c in db.Courses on i.CourseId equals c.Id
                                   join a in db.Attendances on i.Id equals a.InscriptionId
                                   where i.CourseId.Equals(courseId)
                                   where s.Id.Equals(studentId)
                                   group a by new { a.Status, s.Name, c.Id } into temp
                                   select new StudentSuccessRateDetail
                                   {
                                       CourseId = temp.Key.Id,
                                       StudentName = temp.Key.Name,
                                       Status = temp.Key.Status,
                                       Count = temp.Count()
                                   };

                return studentsRate.ToList();
            }
        }

        /// <summary>
        /// returns a formation's canceled rate
        /// </summary>
        /// <returns></returns>
        public CourseCanceledRateDetail GetCourseCanceledRate(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                CourseCanceledRateDetail details = new CourseCanceledRateDetail();

                var getCanceled = (from i in db.Inscriptions
                                   join c in db.Courses on i.CourseId equals c.Id
                                   where i.CourseId.Equals(courseId) && i.Status.Equals(1)
                                   select new { i.Status }).Count();

                details.CanceledRate = Convert.ToInt32(getCanceled);

                var getOthers = (from i in db.Inscriptions
                                 join c in db.Courses on i.CourseId equals c.Id
                                 where i.CourseId.Equals(courseId) && (i.Status.Equals(2) || i.Status.Equals(0))
                                 select new { i.Status }).Count();

                details.OtherRate = Convert.ToInt32(getOthers);

                return details;
            }
        }

        /// <summary>
        /// returns a formation's success rate
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public CourseSuccessRateDetail GetCourseSuccessRate(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                CourseSuccessRateDetail details = new CourseSuccessRateDetail();

                var totalCourseDays = GetNumberCourseDays(courseId);
                var studentIds = db.Inscriptions.Where(x => x.CourseId.Equals(courseId)).Select(x => x.Student.Id);

                foreach (var item in studentIds)
                {
                    var attencances = GetAttendaces(courseId, item);
                    var statusF = attencances.Except(attencances.Where(s => s.Status.Equals(1))); //Deixa apenas as faltas

                    if (attencances.Count() == 0) //Caso não existam coloca como failure
                    {
                        details.FailureRate++;
                    }
                    else if (statusF.Count() == 0) //nao tem nenhuma falta contabilizada
                    {
                        details.SuccessRate++;
                    }
                    else //se existir valores com o status = 0
                    {
                        if (statusF.Count() > (totalCourseDays * 0.3))
                        {
                            details.FailureRate++;
                        }
                        else
                        {
                            details.SuccessRate++;
                        }
                    }
                }

                return details;
            }
        }

        /// <summary>
        /// gets the attendance rate for each student in each formation's day
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IEnumerable<CourseAttendanceRateDetail> GetCourseAttendanceRate(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                var rates = (from calendar in db.Calendars
                             join a in db.Attendances on calendar.Id equals a.CalendarId
                             join i in db.Inscriptions on new { s1 = calendar.CourseId, s2 = a.InscriptionId } equals new { s1 = i.CourseId, s2 = i.Id }
                             where calendar.CourseId.Equals(courseId)
                             group new { calendar.CalendarDate, a.Status } by new { calendar.CalendarDate } into temp
                             select new CourseAttendanceRateDetail
                             {
                                 Date = temp.Key.CalendarDate,
                                 NotAttended = temp.Count(x => x.Status.Equals(0)),
                                 Attended = temp.Count(x => x.Status.Equals(1)),
                                 Justified = temp.Count(x => x.Status.Equals(2))
                             });

                return rates.ToList();
            }
        }

        public CourseFullAttendanceDetail GetCourseFullAttendance(int courseId)
        {
            using (DBEntities db = new DBEntities())
            {
                CourseFullAttendanceDetail detail = new CourseFullAttendanceDetail();

                var totalCourseDays = GetNumberCourseDays(courseId);
                var studentIds = db.Inscriptions.Where(x => x.CourseId.Equals(courseId)).Select(x => x.Student.Id);

                //TEST Queries
                //var internalStudentsPerMonth = (from students in db.Students
                //                                join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                                join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                //                                where attendances.Calendar.CalendarDate.Month.Equals(1) && !students.Company.External
                //                                select new { students.Id }).Count();

                //var externalStudentsPerMonth = (from students in db.Students
                //                                join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                                join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                //                                where attendances.Calendar.CalendarDate.Month.Equals(1) && students.Company.External
                //                                select new { students.Id }).Count();

                //var internalStudentsPerYear = (from students in db.Students
                //                               join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                               join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                //                               where attendances.Calendar.CalendarDate.Year.Equals(2017) && !students.Company.External
                //                               select new { students.Id }).Count();

                //var externalStudentsPerYear = (from students in db.Students
                //                               join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                               join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                //                               where attendances.Calendar.CalendarDate.Year.Equals(2017) && students.Company.External
                //                               select new { students.Id }).Count();

                //var internalStudentsPerCourseType = (from students in db.Students
                //                                     join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                                     join courses in db.Courses on inscriptions.CourseId equals courses.Id
                //                                     join courseTypes in db.CourseTypes on courses.CourseTypeId equals courseTypes.Id
                //                                     where courseTypes.Name.Equals("") && !students.Company.External
                //                                     select new { students.Id }).Count();

                //var externalStudentsPerCourseType = (from students in db.Students
                //                                     join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                                     join courses in db.Courses on inscriptions.CourseId equals courses.Id
                //                                     join courseTypes in db.CourseTypes on courses.CourseTypeId equals courseTypes.Id
                //                                     where courseTypes.Name.Equals("") && students.Company.External
                //                                     select new { students.Id }).Count();

                //var companyStudentsPerMonth = (from students in db.Students
                //                               join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                               join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                //                               where attendances.Calendar.CalendarDate.Month.Equals(1) && students.Company.Name.Equals("")
                //                               select new { students.Id }).Count();

                //var companyStudentsPerYear = (from students in db.Students
                //                              join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                              join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                //                              where attendances.Calendar.CalendarDate.Year.Equals(2017) && students.Company.Name.Equals("")
                //                              select new { students.Id }).Count();

                //var companyStudentsPerType = (from students in db.Students
                //                              join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                //                              join courses in db.Courses on inscriptions.CourseId equals courses.Id
                //                              join courseTypes in db.CourseTypes on courses.CourseTypeId equals courseTypes.Id
                //                              where courseTypes.Name.Equals("") && students.Company.Name.Equals("")
                //                              select new { students.Id }).Count();
                //END of Test Queries



                foreach (var item in studentIds)
                {
                    var attendancesAndAbsences = GetAttendaces(courseId, item);
                    var attendances = attendancesAndAbsences.Except(attendancesAndAbsences.Where(s => s.Status.Equals(0) || s.Status.Equals(2))); //Tira as faltas

                    if (attendancesAndAbsences.Count() == 0) //Caso não existam coloca como failure
                    {
                        detail.NeverAttended++;
                    }
                    else if (attendances.Count() == 0) //Não tem presenças, apenas faltas
                    {
                        detail.LimitAndBelow++;
                    }
                    else
                    {

                        if ( attendances.First().Count > (totalCourseDays * 0.7))
                        {
                            detail.MoreThanLimit++;
                        }
                        else
                        {
                            detail.LimitAndBelow++;
                        }
                    }
                }

                return detail;
            }

        }

        public List<string> GetCourseTypes()
        {
            using (DBEntities db = new DBEntities())
            {
                var types = db.CourseTypes.Select(x => x.Name);

                return types.ToList();
            }
        }

        public EffortsCollectionDetail GetCoursesEffortPerUserSelection(int month, int year, string courseType, int selection)
        {
            using (DBEntities db = new DBEntities())
            {
                EffortsCollectionDetail effortsCollectionDetail = new EffortsCollectionDetail();

                SortedList<int, int> efforts = new SortedList<int, int>();

                var allCourseEfforts = (from calendar in db.Calendars
                                        join c in db.Courses on calendar.CourseId equals c.Id
                                        select new { c.Effort, calendar.CalendarDate , c.CourseType.Name});

                var courseEffortsPerSelection = allCourseEfforts;

                switch (selection)
                {
                    case 1:
                        courseEffortsPerSelection = allCourseEfforts.Where(x => x.CalendarDate.Month.Equals(month) && x.CalendarDate.Year.Equals(year));
                        break;

                    case 2:
                        courseEffortsPerSelection = allCourseEfforts.Where(x => x.CalendarDate.Year.Equals(year));
                        break;

                    case 3:
                        courseEffortsPerSelection = allCourseEfforts.Where(x => x.Name.Equals(courseType));
                        break;

                    case 4:
                        courseEffortsPerSelection = allCourseEfforts.Where(x => x.CalendarDate.Month.Equals(month));
                        break;
                }


                var effortsDetailSet = courseEffortsPerSelection.Select(x => new EffortsDetail() { Effort = x.Effort });

                


                foreach (EffortsDetail effort in effortsDetailSet)
                {
                    int convertedEffort = effort.ConvertedEffort;

                    if (efforts.ContainsKey(convertedEffort))
                    {
                        int value;
                        efforts.TryGetValue(convertedEffort, out value);
                        efforts.Remove(convertedEffort);
                        efforts.Add(convertedEffort, ++value);
                    }
                    else{
                        efforts.Add(convertedEffort, 1);
                    }
                }

                effortsCollectionDetail.Efforts = efforts.Keys.ToArray();
                effortsCollectionDetail.EffortsCount = efforts.Count;
                effortsCollectionDetail.EffortsRepetition = efforts.Values.ToArray();

                return effortsCollectionDetail;
            }
        }

        public int[] GetCompanyStudentsPerMonthAndYear(int companyId, int month, int year)
        {
            using (DBEntities db = new DBEntities())
            {
                var studentsPerMonth = (from students in db.Students
                                        join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                                        join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                                        where attendances.Calendar.CalendarDate.Month.Equals(month) && attendances.Calendar.CalendarDate.Year.Equals(year)
                                        && students.Company.Id == companyId
                                        select new { students.Id, attendances.Calendar.CalendarDate }).Distinct();

                int[] studentsPerWeek = {0, 0, 0, 0, 0};

                foreach(var student in studentsPerMonth)
                {
                    int weekNumber = GetWeekOfMonth(student.CalendarDate);
                    studentsPerWeek[weekNumber]++;
                }

                return studentsPerWeek;

            }
        }

        public int[] GetCompanyStudentsPerMonth(int companyId, int month)
        {
            using (DBEntities db = new DBEntities())
            {
                var studentsPerMonth = (from students in db.Students
                                        join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                                        join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                                        where attendances.Calendar.CalendarDate.Month.Equals(month) && students.Company.Id == companyId
                                        select new { students.Id, attendances.Calendar.CalendarDate });

                int[] studentsPerWeek = { 0, 0, 0, 0, 0 };

                foreach (var student in studentsPerMonth)
                {
                    int weekNumber = GetWeekOfMonth(student.CalendarDate);
                    studentsPerWeek[weekNumber]++;
                }

                return studentsPerWeek;

            }
        }

        private int GetWeekOfMonth(DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return GetWeekNumber(time, CultureInfo.CurrentCulture) - GetWeekNumber(first, CultureInfo.CurrentCulture);
        }

        private int GetWeekNumber(DateTime date, CultureInfo culture)
        {
            return culture.Calendar.GetWeekOfYear(date,
                CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public int[] GetCompanyStudentsPerYear(int companyId, int year)
        {
            using (DBEntities db = new DBEntities())
            {
                var companyStudentsPerYears = (from students in db.Students
                                               join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                                               join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                                               where attendances.Calendar.CalendarDate.Year.Equals(year) && students.Company.Id == companyId
                                               select new { students.Id, attendances.Calendar.CalendarDate.Month }).Distinct();

                int[] numberOfStudentsArray = new int[12];

                for(int i = 1; i < 13; i++)
                {
                    int numberOfStudentsPerMonth = companyStudentsPerYears.Where(x => x.Month.Equals(i)).Count();

                    numberOfStudentsArray[i-1] = numberOfStudentsPerMonth;
                }

                return numberOfStudentsArray;
            }
        }

        public int GetCompanyStudentsPerType(int companyId, string type)
        {
            using (DBEntities db = new DBEntities())
            {
                var companyStudentsPerType = (from students in db.Students
                                             join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                                             join courses in db.Courses on inscriptions.CourseId equals courses.Id
                                             join courseTypes in db.CourseTypes on courses.CourseTypeId equals courseTypes.Id
                                             where courseTypes.Name.Equals(type) && students.Company.Id == companyId
                                             select new { students.Id }).Distinct().Count();

                return companyStudentsPerType;
            }
        }

        public int[] GetStudentsPerMonthAndYear(int month, int year)
        {
            using (DBEntities db = new DBEntities())
            {
                var studentsPerMonth = (from students in db.Students
                                        join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                                        join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                                        where attendances.Calendar.CalendarDate.Month.Equals(month) && attendances.Calendar.CalendarDate.Year.Equals(year)
                                        select new { students.Id, attendances.Calendar.CalendarDate, students.Company.External }).Distinct();

                int[] studentsPerWeek = { 0, 0, 0, 0, 0 };

                foreach (var student in studentsPerMonth)
                {
                    int weekNumber = GetWeekOfMonth(student.CalendarDate);
                    studentsPerWeek[weekNumber]++;
                }

                return studentsPerWeek;

            }
        }

        public StudentsPerYearDetail GetStudentsPerYear(int year)
        {
            using (DBEntities db = new DBEntities())
            {
                var studentsPerYears = (from students in db.Students
                                               join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                                               join attendances in db.Attendances on inscriptions.Id equals attendances.InscriptionId
                                               where attendances.Calendar.CalendarDate.Year.Equals(year)
                                               select new { students.Id, attendances.Calendar.CalendarDate.Month, students.Company.External }).Distinct();

                StudentsPerYearDetail studentsDetail = new StudentsPerYearDetail();

                int[] internalStudentsPerMonth = new int[12];
                int[] externalStudentsPerMonth = new int[12];
                int studentsPerMonth;

                for (int i = 1; i < 13; i++)
                {
                    studentsPerMonth = studentsPerYears.Where(x => x.Month.Equals(i) && !x.External).Count();

                    internalStudentsPerMonth[i - 1] = studentsPerMonth;
                }

                for (int i = 1; i < 13; i++)
                {
                    studentsPerMonth = studentsPerYears.Where(x => x.Month.Equals(i) && x.External).Count();

                    externalStudentsPerMonth[i - 1] = studentsPerMonth;
                }

                studentsDetail.InternalStudentsPerMonth = internalStudentsPerMonth;
                studentsDetail.ExternalStudentsPerMonth = externalStudentsPerMonth;

                return studentsDetail;
            }
        }

        public int[] GetStudentsPerType(string type)
        {
            using (DBEntities db = new DBEntities())
            {
                var  studentsPerType = (from students in db.Students
                                              join inscriptions in db.Inscriptions on students.Id equals inscriptions.Student.Id
                                              join courses in db.Courses on inscriptions.CourseId equals courses.Id
                                              join courseTypes in db.CourseTypes on courses.CourseTypeId equals courseTypes.Id
                                              where courseTypes.Name.Equals(type)
                                              select new { students.Id, students.Company.External}).Distinct();

                int externalStudentsCount = studentsPerType.Where(x => x.External).Count();

                int internalStudentCount = studentsPerType.Where(x => !x.External).Count();

                int[] totalStudentsCount = { internalStudentCount, externalStudentsCount };

                return totalStudentsCount;
            }
        }


    }
}
