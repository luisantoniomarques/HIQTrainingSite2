using HIQTraining.Model;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class InscriptionListViewModel : AlertViewModel
    {
        public InscriptionListViewModel()
        {
            InscriptionList = new List<InscriptionDetail>();
            this.Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }

        public int CourseId { get; set; }
        [Display(Name = "labelCourseName", ResourceType = typeof(HIQResources))]
        public string CourseName { get; set; }

        public bool IsCourseFinished { get; set; }

        public List<InscriptionType> InscriptionTypeList { get; set; }
        public List<InscriptionDetail> InscriptionList { get; set; }

    }
}