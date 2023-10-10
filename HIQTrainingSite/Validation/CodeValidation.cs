using HIQTraining.Business.Certification;
using HIQTraining.Business.Course;
using HIQTraining.DAL.Certification;
using HIQTraining.DAL.Course;
using HIQTrainingSite.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.Validation {
    public class CodeValidation : ValidationAttribute {

        private string certificationCode;

        public CodeValidation(string certificationCode) {
            this.certificationCode = certificationCode;
        }

        /// <summary>
        /// Validation for the certification's code
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            string message = HIQResources.validationCode;

            if(validationContext.ObjectInstance is CertificationTypeViewModel){
                CertificationTypeViewModel vm = (CertificationTypeViewModel)validationContext.ObjectInstance;
                HIQTraining.Model.CertificationType certificationType = new HIQTraining.Model.CertificationType();
                CertificationTypeManager manager = new CertificationTypeManager(new CertificationTypeDao());

                certificationType.Id = vm.Id;
                certificationType.Code = vm.Code;
                certificationType.Name = vm.Name;

                if(manager.IsValidCode(certificationType)) {
                    return new ValidationResult(string.Format(message, HIQResources.validationCertification));
                }
            } else { //course 
                CourseViewModel vm = (CourseViewModel)validationContext.ObjectInstance;
                CourseManager manager = new CourseManager(new CourseDao());

                HIQTraining.Model.Course course = new HIQTraining.Model.Course();
                course.Id = vm.Id;
                course.Code = vm.Code;
                course.Name = vm.Name;

                if(manager.GetCode(course)) {
                    return new ValidationResult(string.Format(message, HIQResources.validationCourse));
                }
            }

            return ValidationResult.Success;
        }
    }
}