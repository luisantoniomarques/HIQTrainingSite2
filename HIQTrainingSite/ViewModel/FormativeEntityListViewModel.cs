using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class FormativeEntityListViewModel : AlertViewModel
    {

        public FormativeEntityListViewModel() {
            FormativeEntitiesList = new PagedList<FormativeEntityDetail>(null, 1, 10);
            Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }

        public IPagedList<FormativeEntityDetail> FormativeEntitiesList { get; set; }
    }
}