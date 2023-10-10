using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail
{
    public class EffortsDetail
    {
        public string Effort { get; set; }

        public int ConvertedEffort { get { return Convert.ToInt32(Effort); } }

    }
}
