using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace HIQTrainingSite.ViewModel
{
    public class CourseLevelListViewModel
    {
        public CourseLevelListViewModel()
        {
            CourseLevels = new PagedList<CourseLevelDetail>(null, 1, 10);
        }

        public IPagedList<CourseLevelDetail> CourseLevels { get; set; }
    }
}