using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel
{
    public class InscriptionTypeDetailListViewModel
    {
        public InscriptionTypeDetailListViewModel()
        {
            InscriptionTypeDetailList = new List<InscriptionTypeDetail>();
        }


        public List<InscriptionTypeDetail> InscriptionTypeDetailList { get; set; }

        public int SelectedInscriptionTypeId { get; set; }

        public IEnumerable<SelectListItem> InscriptionTypeItems
        {
            get { return new SelectList(InscriptionTypeDetailList, "Id", "Description"); }
        }

    }
}