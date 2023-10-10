using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class CompanyDetailListViewModel
    {
        public CompanyDetailListViewModel()
        {
            CompanyDetailList = new List<CompanyDetail>();
        }


        public List<CompanyDetail> CompanyDetailList { get; set; }

        public int SelectedCompanyId { get; set; }

        public IEnumerable<SelectListItem> CompanyNames
        {
            get { return new SelectList(CompanyDetailList, "Id", "Name", SelectedCompanyId); }
        }
    }
}