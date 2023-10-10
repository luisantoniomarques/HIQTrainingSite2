using HIQTraining.ModelDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.Utils
{
    public class InscriptionStatus : StatusBase
    {
        const string STATUS_PREFIX = "statusInscription_";
        static readonly List<StatusDetail> states = LoadStates(STATUS_PREFIX);


        public static List<StatusDetail> GetStates()
        {
            return states;
        }

        public static string GetStatusDescription(int id)
        {
            return GetStatusDescription(string.Concat(STATUS_PREFIX, id));
        }

        public static int GetEnrolledStatusId()
        {
            return 1; // Active TODO: create a method to get the id
        }
    }
}
