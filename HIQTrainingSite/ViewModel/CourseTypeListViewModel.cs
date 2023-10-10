using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class CourseTypeListViewModel
    {
        public CourseTypeListViewModel()
        {
            CourseTypes = new PagedList<CourseTypeDetail>(null, 1, 10);
        }

        public IPagedList<CourseTypeDetail> CourseTypes { get; set; }
    }
}