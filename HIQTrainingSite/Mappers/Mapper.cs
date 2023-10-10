using HIQTraining.Business.Calendar;
using HIQTraining.Business.Company;
using HIQTraining.Business.Configuration;
using HIQTraining.Business.Teacher;
using HIQTraining.DAL.Calendar;
using HIQTraining.DAL.Company;
using HIQTraining.DAL.Configuration;
using HIQTraining.DAL.Teacher;
using HIQTraining.Model;
using HIQTraining.ModelDetail;
using HIQTrainingSite.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.Mappers {
    public class Mapper {

        public Company CompanyMapperCompanyVm(CompanyViewModel vm, string username)
        {
            CompanyManager companyManager = new CompanyManager(new CompanyDao());
            Company company = new Company();
            company.Id = vm.Id;
            company.Name = vm.Name;
            company.External = vm.External;

            return company;
        } 



        public Course CourseMapperCourseVM(CourseViewModel vm, string username, string calendarDates) {
            CalendarManager calendarManager = new CalendarManager(new CalendarDao());
            Course course = new Course();

            course.Id = vm.Id;
            course.Name = vm.Name;
            course.LevelId = vm.SelectedLevelId;
            course.Code = vm.Code;
            course.LocationId = vm.SelectedLocationId;
            course.FormativeEntityId = vm.EntityFormativeId;
            course.TeacherId = vm.TeacherId.Value;
            course.CourseTypeId = vm.SelectedCourseTypeId;
            course.Observation = vm.Observation;
            course.StartHour = vm.StartHour;
            course.EndHour = vm.EndHour;
            course.Calendars = CalendarMapper(calendarManager.GetCalendarList(calendarDates), username);
            course.StartDate = course.Calendars.Min(x => x.CalendarDate);
            course.Status = course.Status;
            course.CanceledObservation = vm.CanceledObservation;
            // aqui
            course.PayRoll = vm.PayRoll;
            course.Effort = vm.Effort;
            course.CloseDate = course.Calendars.Max(x => x.CalendarDate);
            course.InscriptionEmail = vm.InscriptionEmail;
            course.ConfirmationEmail = vm.ConfirmationEmail;
            course.Reminder = vm.Reminder;
            course.Documentation = vm.Documentation;
            course.Intranet = vm.Intranet;
            course.Material = vm.Material;
            course.Dtp = vm.Dtp;
            course.Certificates = vm.Certificates;
            course.Avaliation = vm.Avaliation;
            course.Confidential = vm.Confidential;
            course.UniqueReport = vm.UniqueReport;

            return course;
        }

        public CourseDetailsViewModel CourseDetails(CourseFullDetail course) {
            CourseDetailsViewModel viewModel = new CourseDetailsViewModel();

            viewModel.Id = course.Id;
            viewModel.Name = course.Name;
            viewModel.Level = course.Level;
            viewModel.Type = course.Type;
            viewModel.Code = course.Code;
            viewModel.Location = course.Location;
            viewModel.Teacher = course.Teacher;
            viewModel.Entity = course.Entity;  
            viewModel.Observation = course.Observation;
            viewModel.StartHour = course.StartHour;
            viewModel.EndHour = course.EndHour;
            viewModel.StartDate = course.StartDate;
            viewModel.CloseDate = course.CloseDate;
            viewModel.StatusId = course.StatusId;
            viewModel.CanceledObservation = course.CanceledObservation;
            // aqui
            viewModel.Effort = course.Effort;
            viewModel.CloseDate = course.StartDate;
            viewModel.InscriptionEmail = course.InscriptionEmail;
            viewModel.ConfirmationEmail = course.ConfirmationEmail;
            viewModel.Reminder = course.Reminder;
            viewModel.Documentation = course.Documentation;
            viewModel.Intranet = course.Intranet;
            viewModel.Material = course.Material;
            viewModel.Dtp = course.Dtp;
            viewModel.Certificates = course.Certificates;
            viewModel.Avaliation = course.Avaliation;
            viewModel.Confidential = course.Confidential;
            viewModel.UniqueReport = course.UniqueReport;

            return viewModel;
        }

        public List<Calendar> CalendarMapper(List<CalendarDetail> details, string user) {
            List<Calendar> list = new List<Calendar>();

            foreach(var item in details) {
                Calendar calendar = new Calendar {
                    CalendarDate = item.Date,
                    CreatedDate = DateTime.Now,
                    UserCreated = user,
                    Status = 1
                };
                list.Add(calendar);
            }

            return list;
        }

        public Certification CertificationVmMapperCertification(CertificationViewModel vm) {
            Certification certification = new Certification();

            certification.Id = vm.Id;
            certification.Name = vm.Name;
            certification.Classification = vm.Result;
            certification.Status = vm.StatusId.Value;
            certification.FormativeEntityId = vm.SelectedEntityId;
            certification.CertificationTypeId = vm.CertificationTypeId;
            certification.FormativeEntityId = vm.SelectedEntityId;
            certification.StudentId = vm.StudentId;
            certification.Date = vm.Date;
            certification.Observation = vm.Observation;
            certification.pdf = vm.pdf;

            return certification;
        }

        public CertificationViewModel CertificationFullDetailMapperCertificationVm(CertificationFullDetail certification) {
            CertificationViewModel vm = new CertificationViewModel();

            vm.Id = certification.Id;
            vm.Name = certification.CertificationName;
            vm.Result = certification.Result;
            vm.SelectedEntityId = certification.EntityId;
            vm.SelectedEntity = certification.Entity;
            vm.CertificationType = certification.CertificationType;
            vm.CertificationTypeId = certification.CertificationTypeId;
            vm.Code = certification.Code;
            vm.StudentName = certification.StudentName;
            vm.StudentEmail = certification.StudentEmail;
            vm.StudentId = certification.StudentId;
            vm.Date = certification.Date;
            vm.Observation = certification.Observation;
            vm.StatusId = certification.StatusId;

            return vm;
        }
    }
}