using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class CourseEffortViewModel
    {
        public int SelectedMonth { get; set; }

        public int SelectedYear { get; set; }

        public string CourseType { get; set; }


    }

    
}