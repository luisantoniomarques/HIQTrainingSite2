using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class CompanyStudentsViewModel
    {
        public int SelectedMonth { get; set; }

        public int SelectedYear { get; set; }

        public string CourseType { get; set; }

        public int CompanyId { get; set; }

    }
}