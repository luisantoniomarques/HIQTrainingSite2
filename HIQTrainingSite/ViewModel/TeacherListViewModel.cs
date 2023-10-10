using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel {
    public class TeacherListViewModel : AlertViewModel {

        public TeacherListViewModel() {
            TeacherList = new PagedList<TeacherDetail>(null, 1, 10);
            Alert = new AlertViewModel();
            Search = new TeacherSearchViewModel();
        }

        public IPagedList<TeacherDetail> TeacherList { get; set; }

        public AlertViewModel Alert { get; set; }

        public TeacherSearchViewModel Search { get; set; }

    }
}