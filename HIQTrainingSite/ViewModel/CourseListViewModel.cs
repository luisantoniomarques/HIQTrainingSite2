using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class CourseListViewModel : AlertViewModel
    {
        public CourseListViewModel() {
            CourseList = new PagedList<CourseDetail>(null, 1, 10);
            Search = new SearchCourseViewModel();
            Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }

        public IPagedList<CourseDetail> CourseList { get; set; }

        public SearchCourseViewModel Search { get; set; }

    }
}