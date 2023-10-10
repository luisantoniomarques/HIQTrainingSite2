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
    public class StudentStatus : StatusBase
    {
        const string STATUS_PREFIX = "statusStudent_";
        static readonly List<StatusDetail> states = LoadStates(STATUS_PREFIX);


        public static List<StatusDetail> GetStatusList()
        {
            return states;
        }
        public static List<StatusDetail> GetStatusListForSearch()
        {
            List<StatusDetail> statesforSearch = new List<StatusDetail>();
            statesforSearch.Add(new StatusDetail() { Id = -1, Description = HIQResource.dropdownAllOptions });
            statesforSearch.AddRange(states);
            return statesforSearch;
        }

        public static string GetStatusDescription(int? id)
        {
            return GetStatusDescription(string.Concat(STATUS_PREFIX, id));
        }

        public static int GetActiveStatusId()
        {
            return 1; // Active TODO: create a method to get the id
        }
    }

}
