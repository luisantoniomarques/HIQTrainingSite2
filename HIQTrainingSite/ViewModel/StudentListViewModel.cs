using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace HIQTrainingSite.ViewModel
{
    public class StudentListViewModel : AlertViewModel
    {
        public StudentListViewModel()
        {
            Alert = new AlertViewModel();
            Search = new StudentSearchViewModel();
        }

        public AlertViewModel Alert { get; set; }

        public StudentSearchViewModel Search { get; set; }

        public IPagedList<StudentDetail> StudentList { get; set; }
    }
}
