using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class CourseLocationListViewController
    {
        public CourseLocationListViewController()
        {
            CourseLocationList = new List<CourseLocationDetail>();
        }

        public List<CourseLocationDetail> CourseLocationList { get; set; }
    }
}