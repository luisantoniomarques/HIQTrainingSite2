using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIQTrainingSite.ViewModel
{
    public class FormaIte
    {
        public string ColaboCode { get; set; }
        public string Name { get; set; }
        public string Corse { get; set; }
        public int CursCod { get; set; }

        public DateTime Init { get; set; }
        public DateTime End { get; set; }
        public int Duracion { get; set; }
        public string Teacher { get; set; }
        public int FormCod { get; set; }
        public int FormArea { get; set; }
        public string Local { get; set; }
        public string Schedule { get; set; }
        public string Certification { get; set; }
    }
}
