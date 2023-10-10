using HIQTraining.LocalResources;
using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Utils
{
    public class CourseStatus : StatusBase
    {
        const string STATUS_PREFIX = "statusCourse_";
        static readonly List<StatusDetail> states = LoadStates(STATUS_PREFIX);


        public static List<StatusDetail> GetStates()
        {
            return states;
        }


        public static List<StatusDetail> GetStatesListForSearch()
        {
            List<StatusDetail> statesforSearch = new List<StatusDetail>();
            statesforSearch.Add(new StatusDetail() { Id = -1, Description = HIQResource.dropdownAllOptions });
            statesforSearch.AddRange(states);
            return statesforSearch;
        }


        public static string GetStatusDescription(int id)
        {
            return GetStatusDescription(string.Concat(STATUS_PREFIX, id));
        }
    }

}
