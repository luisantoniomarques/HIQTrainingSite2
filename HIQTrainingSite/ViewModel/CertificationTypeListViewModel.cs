using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel {
    public class CertificationTypeListViewModel {

        public CertificationTypeListViewModel() {
            this.CertificationTypeList = new PagedList<CertificationTypeDetail>(null, 1, 10);
            this.Alert = new AlertViewModel();
        }

        public IPagedList<CertificationTypeDetail> CertificationTypeList { get; set; }

        public AlertViewModel Alert { get; set; }

    }
}