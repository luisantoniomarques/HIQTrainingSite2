using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class CourseLocationListViewModel : AlertViewModel
    {
        public CourseLocationListViewModel()
        {
            CourseLocationList = new PagedList<CourseLocationDetail>(null, 1, 10);
            Alert = new AlertViewModel();
        }
        public AlertViewModel Alert { get; set; }
        public IPagedList<CourseLocationDetail> CourseLocationList { get; set; }
    }
}