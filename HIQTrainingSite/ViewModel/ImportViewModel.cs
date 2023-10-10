using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class ImportViewModel : AlertViewModel
    {
        public ImportViewModel()
        {
            Alert = new AlertViewModel();
        }

        public AlertViewModel Alert { get; set; }
    }
}
