using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel {
    public class InscriptionTypeListViewModel {

        public InscriptionTypeListViewModel() {
            InscriptionTypesList = new PagedList<InscriptionTypeDetail>(null, 1, 10);
            Alert = new AlertViewModel();
        }

        public IPagedList<InscriptionTypeDetail> InscriptionTypesList { get; set; }

        public AlertViewModel Alert { get; set; }

    }
}