using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class CompanyListViewModel : AlertViewModel
    {
        public CompanyListViewModel()
        {
            CompanyList = new PagedList<CompanyDetail>(null, 1, 10);
            Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }
        public IPagedList<CompanyDetail> CompanyList { get; set; }

    }
}