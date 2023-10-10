using HIQTraining.Business.Course;
using HIQTraining.DAL.Course;
using HIQTraining.Model;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.Validation {
    public class CourseCodeUpdateValidation : ValidationAttribute {

        private string courseCode;

        public CourseCodeUpdateValidation(string courseCode) {
            this.courseCode = courseCode;
        }

        /// <summary>
        /// Validation for the course's code
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            CourseUpdateViewModel vm = (CourseUpdateViewModel)validationContext.ObjectInstance;
            CourseManager manager = new CourseManager(new CourseDao());

            Course course = new Course();
            course.Id = vm.Id;
            course.Code = vm.Code;
            course.Name = vm.Name;

            if(manager.GetCode(course)) {
                return new ValidationResult("O código inserido já se encontra associado a outra formação.");
            }

            return ValidationResult.Success;
        }

    }
}