using HIQTraining.ModelDetail;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class HomeViewModel
    {
        public HomeViewModel() {
            Notes = new NotesViewModel();
        }

        public NotesViewModel Notes { get; set; }
    }
}