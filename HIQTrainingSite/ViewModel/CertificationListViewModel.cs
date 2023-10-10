using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class CertificationListViewModel {

        public CertificationListViewModel() {
            CertificationList = new PagedList<CertificationDetail>(null, 1, 10);
            Alert = new AlertViewModel();
            Search = new CertificationSearchViewModel();
        }

        public IPagedList<CertificationDetail> CertificationList { get; set; }

        public string StudentName { get; set; }

        public string StudentEmail { get; set; }

        public int StudentId { get; set; }

        public string DisplayName { get; set; }

        public AlertViewModel Alert { get; set; }

        public CertificationSearchViewModel Search { get; set; }

    }
}