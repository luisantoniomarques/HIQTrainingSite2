using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ModelDetail
{
    public abstract class ModelBaseDetail
    {
        public string UserCreated { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; } // has to be nullable because of the query outer joins
        public string UserUpdated { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
    }
}
